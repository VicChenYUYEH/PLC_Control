using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HyTemplate.gui;
using System.IO;
using System.Xml;

namespace HyTemplate
{
    public partial class FrmMain : Form
    {
        private string sUserRegisterFileName = Directory.GetCurrentDirectory() + "\\config\\UserRegister.xml";

        EventClient ecClient;
        Dictionary<String, KeyValuePair<String, String>> dicUsers = new Dictionary<string, KeyValuePair<string, string>>();

        FrmHistoryLog frmLog;
        FrmHistoryAlarm frmAlarm;
        FrmLogin frmLogin;
        FrmSystemParameter frmSysPara;
        FrmRecipe frmRecipe;
        FrmOverview frmOverview;
        FrmControl frmControl;
        FrmGasview frmGasView;
        FrmProcess frmProcess;
        FrmDeviceConstant frmDeviceConstant;
        FrmMaintenance frmMaintenance;
        
        RdEqKernel rdKernel;
        Form frmCurrent = null;

        public FrmMain()
        {
            InitializeComponent();

            ecClient = new EventClient(this);
            ecClient.OnEventHandler += onReceiveMessage;

            rdKernel = new RdEqKernel();

            frmLog = new FrmHistoryLog(rdKernel);
            frmAlarm = new FrmHistoryAlarm(rdKernel);
            frmRecipe = new FrmRecipe(rdKernel);
            frmSysPara = new FrmSystemParameter(rdKernel);
            frmLogin = new FrmLogin();
            frmOverview = new FrmOverview(rdKernel);
            frmControl = new FrmControl(rdKernel);
            frmGasView = new FrmGasview(rdKernel);
            frmProcess = new FrmProcess(rdKernel);
            frmDeviceConstant = new FrmDeviceConstant(rdKernel);
            frmMaintenance = new FrmMaintenance(rdKernel);

            this.loadUserRegister();

            System.Threading.Thread.Sleep(1000);

            reloadGui(frmOverview);

            checkInitialStatus();

            rdKernel.WriteOperatorLog("StartMark", "Program Start ......");
            login_out(false);
        }

        private void checkInitialStatus()
        {
            if (rdKernel.pPlcKernel["X00024"] == 1 || rdKernel.pPlcKernel["X00024"] == 1 || rdKernel.pPlcKernel["X00025"] == 1)
            {

                TEvent data = new TEvent();
                data.MessageName = ProxyMessage.MSG_PROCESS_VACUUM;

                ecClient.SendMessage(data);
            }
        }

        private void onReceiveMessage(string m_MessageName, TEvent m_Event)
        {
            switch (m_MessageName)
            {
                case ProxyMessage.MSG_USER_REGISTER_CHANGED:
                    loadUserRegister();
                    break;
                case ProxyMessage.MSG_ALARM_OCCURE:
                case ProxyMessage.MSG_ALARM_CLEAR:
                    DataTable dt = creatAlarmTable();
                    int alarm_count = Convert.ToInt16(m_Event.EventData["Count"]);
                    for (int index = 1; index <= alarm_count; index++)
                    {
                        DateTime alarm_time = Convert.ToDateTime(m_Event.EventData["OccurTime" + index.ToString()]);
                        string alarm_level = m_Event.EventData["Level" + index.ToString()];
                        string alarm_text = m_Event.EventData["Description" + index.ToString()];
                        string alarm_solution = m_Event.EventData["Solution" + index.ToString()];

                        string alarm_msg = alarm_level + " - " + alarm_time + " - " + alarm_text + "";
                        dt.Rows.Add(new Object[] { alarm_time, alarm_level, alarm_text, alarm_solution });
                    }
                    dataGrdAlarm.Invoke(new Action(() =>
                    {
                        setAlarmGridFormat(dt);
                    }));
                    break;
                case ProxyMessage.MSG_PLC_CONNECT:
                case ProxyMessage.MSG_PLC_DISCONNECT:
                    statusPictureBox1.Invoke(new Action(() =>
                    {
                        statusPictureBox1.RefreshStatus(rdKernel);
                    }));
                    break;
                case ProxyMessage.MSG_RECIPE_SET:
                    string Current_rcp = m_Event.EventData["CurrentRCP"];
                    TxtRecipeName.Text = Current_rcp;
                    break;
                default:
                    break;
            }
        }

        private DataTable creatAlarmTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("發生時間");
            dt.Columns.Add("異常等級");
            dt.Columns.Add("異常描述");
            dt.Columns.Add("解決辦法");
            return dt;
        }

        private void setAlarmGridFormat(DataTable m_Dt)
        {
            dataGrdAlarm.DataSource = m_Dt;
            dataGrdAlarm.Columns[0].Width = 200;
            dataGrdAlarm.Columns[1].Width = 80;
            dataGrdAlarm.Columns[2].Width = 200;
            dataGrdAlarm.Columns[3].Width = 710;
            dataGrdAlarm.Refresh();
        }

        private void reloadGui(Form m_Form)
        {
            if (frmCurrent == m_Form) return;
            if (frmCurrent != null)
            {
                frmCurrent.Hide();
            }

            if (m_Form != null && frmCurrent != m_Form)
            {
                m_Form.FormBorderStyle = FormBorderStyle.None;
                m_Form.TopLevel = false;
                panel1.Controls.Add(m_Form);
                m_Form.Show();

                frmCurrent = m_Form;
            }
        }

        private void login_out(bool m_Success, string m_Id = "", int m_Authority = 1)
        {
            TEvent data = new TEvent();
            data.MessageName = (m_Success) ? ProxyMessage.MSG_USER_LOGIN : ProxyMessage.MSG_USER_LOGOUT;
            data.EventData["Authority"] = m_Authority.ToString();           
            lblID.Text = (m_Success)? m_Id : "N/A";
            data.EventData["UserName"] = lblID.Text;
            ecClient.SendMessage(data);
        }

        private void loadUserRegister()
        {
            dicUsers.Clear();

            if (!File.Exists(sUserRegisterFileName)) return;

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(sUserRegisterFileName);

            XmlNodeList nodes = XmlDoc.SelectNodes("UserList/User");
            foreach (XmlNode chile_node in nodes)
            {
                String user_id = chile_node.Attributes["ID"].Value;
                String user_pwd = chile_node.Attributes["Password"].Value;
                String user_authority = chile_node.Attributes["Authority"].Value;

                dicUsers.Add( user_id, new KeyValuePair<string, string>(user_pwd, user_authority) );
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            rdKernel.WriteOperatorLog("StartMark", "Program Close ......");
            rdKernel.pPlcKernel.Dispose();
            rdKernel.Dispose();
            System.Threading.Thread.Sleep(500);
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void btnVacuum_Click(object sender, EventArgs e)
        {
            if (   rdKernel.pPlcKernel[ConstPlcDefine.PLC_DI_WATER_FLOW_1] == 0
                || rdKernel.pPlcKernel[ConstPlcDefine.PLC_DI_WATER_FLOW_2] == 0)
            {
                MessageBox.Show("Please Check Water Flow !!");
                return;
            }
            

            TEvent data = new TEvent();
            data.MessageName = ProxyMessage.MSG_PROCESS_VACUUM;

            ecClient.SendMessage(data);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            rdKernel.pPlcKernel["HMI_Alarm_Reset"] = 1;
            System.Threading.Thread.Sleep(1000);

            TEvent data = new TEvent();
            data.MessageName = ProxyMessage.MSG_ALARM_RESET;

            ecClient.SendMessage(data);
            rdKernel.pPlcKernel["HMI_Alarm_Reset"] = 0;
            rdKernel.flOperator.WriteLog("Alarm_Reset", "Click");
        }

        #region Form Buttom Click Event
        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmLogin.sLoginId = "";
            frmLogin.sLoginPwd = "";
            DialogResult result = frmLogin.ShowDialog();

            if (result == DialogResult.Abort)
            {
                login_out(false);
                rdKernel.InsertHistoryLog("N/A", "User Logout");
                rdKernel.flOperator.WriteLog("User Logout", "N/A");
            }
            else if (result == DialogResult.OK)
            {
                string id = frmLogin.sLoginId;
                string pwd = frmLogin.sLoginPwd;
                if (!dicUsers.ContainsKey(id)) //有無該User
                {
                    MessageBox.Show("User no found", "Warning", MessageBoxButtons.OK);
                    rdKernel.flOperator.WriteLog("User LogIn", "Fail...... ");
                    return;
                }
                int authority = 1;
                Dictionary<String, KeyValuePair<String, String>> dicLogin = new Dictionary<string, KeyValuePair<string, string>>();
                dicLogin.Add(id, new KeyValuePair<string, string>(pwd, dicUsers[id].Value));

                if (!dicUsers.ContainsValue(dicLogin[id])) //查詢User對應密碼是否正確，錯誤則登出
                {
                    MessageBox.Show("User & Password mismatch", "Warning", MessageBoxButtons.OK);
                    login_out(false, id, authority);
                    rdKernel.flOperator.WriteLog("User LogIn", "Fail...... ");
                    return;
                }

                //判斷使用者權限
                if (dicUsers[id].Value == string.Format("{0:X8}", ((string)("Administrator")).GetHashCode()))
                {
                    authority = 4;
                }
                else if (dicUsers[id].Value == string.Format("{0:X8}", ((string)("Supervisor")).GetHashCode()))
                {
                    authority = 3;
                }
                else if (dicUsers[id].Value == string.Format("{0:X8}", ((string)("Engineer")).GetHashCode()))
                {
                    authority = 2;
                }

                //傳送登入訊息給各頁面
                login_out(true, id, authority);
                rdKernel.InsertHistoryLog(id, "User Login");
                rdKernel.flOperator.WriteLog("User Login", id);
            }
        }

        private void btnHistoryLog_Click(object sender, EventArgs e)
        {
            reloadGui(frmLog);
        }

        private void BtnAlarm_Click(object sender, EventArgs e)
        {
            reloadGui(frmAlarm);
        }

        private void btnRecipe_Click(object sender, EventArgs e)
        {
            reloadGui(frmRecipe);
        }

        private void btnSysPara_Click(object sender, EventArgs e)
        {
            reloadGui(frmSysPara);
        }

        private void btnOverview_Click(object sender, EventArgs e)
        {
            reloadGui(frmOverview);
            frmOverview.Show();
        }

        private void Control_Click(object sender, EventArgs e)
        {
            reloadGui(frmControl);
            frmControl.Show();
        }

        private void btnGasView_Click(object sender, EventArgs e)
        {
            reloadGui(frmGasView);
            frmGasView.Show();
        }

        private void btnProcView_Click(object sender, EventArgs e)
        {
            reloadGui(frmProcess);
            frmProcess.Show();
        }

        private void btnDeviceConstant_Click(object sender, EventArgs e)
        {
            reloadGui(frmDeviceConstant);
            frmDeviceConstant.Show();
        }

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            reloadGui(frmMaintenance);
            frmMaintenance.Show();
        }
        #endregion
    }
}
