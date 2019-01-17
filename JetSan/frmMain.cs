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
    public partial class frmMain : Form
    {
        private string sUserRegisterFileName = Directory.GetCurrentDirectory() + "\\config\\UserRegister.xml";

        EventClient ecClient;
        Recipe rRecipe;
        Dictionary<String, KeyValuePair<String, String>> dicUsers = new Dictionary<string, KeyValuePair<string, string>>();

        frmHistoryLog log;
        frmHistoryAlarm alarm;
        frmLogin login;
        frmSystemParameter sysPara;
        frmRecipe recipe;
        frmOverview overview;
        frmControl control;
        frmGasview gasView;
        frmProcess process;
        frmDeviceConstant deviceConstant;
        frmMaintenance maintenance;

        DBControl db; 
        RdEqKernel rdKernel;
        Form currentForm = null;

        public frmMain()
        {
            InitializeComponent();

            ecClient = new EventClient(this);
            ecClient.OnEventHandler += OnReceiveMessage;

            rdKernel = new RdEqKernel();
            rRecipe = new Recipe();

            log = new frmHistoryLog();
            alarm = new frmHistoryAlarm();
            recipe = new frmRecipe(rRecipe);
            sysPara = new frmSystemParameter(rRecipe);
            login = new frmLogin();
            overview = new frmOverview(rdKernel);
            control = new frmControl(rdKernel);
            gasView = new frmGasview(rdKernel);
            process = new frmProcess(rdKernel);
            deviceConstant = new frmDeviceConstant(rdKernel);
            maintenance = new frmMaintenance(rdKernel);
            db = new DBControl();

            this.LoadUserRegister();

            System.Threading.Thread.Sleep(1000);

            ReloadGui(overview);

            checkInitialStatus();

            Login_out(false);
        }

        private void checkInitialStatus()
        {
            if (rdKernel.PlcKernel["X00024"] == 1 || rdKernel.PlcKernel["X00024"] == 1 || rdKernel.PlcKernel["X00025"] == 1)
            {

                TEvent data = new TEvent();
                data.MessageName = ProxyMessage.MSG_PROCESS_VACUUM;

                ecClient.SendMessage(data);
            }
        }

        private void OnReceiveMessage(string m_MessageName, TEvent m_Event)
        {
            if (m_MessageName == ProxyMessage.MSG_USER_REGISTER_CHANGED)
            {
                this.LoadUserRegister();
            }
            else if (m_MessageName == ProxyMessage.MSG_ALARM_OCCURE || m_MessageName == ProxyMessage.MSG_ALARM_CLEAR)
            {
                DataTable dt = CreatAlarmTable();
                int alarm_count = Convert.ToInt16(m_Event.EventData["Count"]);
                for (int index = 1; index <=alarm_count; index++)
                {
                    DateTime alarm_time = Convert.ToDateTime(m_Event.EventData["OccurTime" + index.ToString()]);
                    string alarm_level = m_Event.EventData["Level" + index.ToString()];
                    string alarm_text = m_Event.EventData["Description" + index.ToString()];
                    string alarm_solution = m_Event.EventData["Solution" + index.ToString()];

                    string alarm_msg = alarm_level + " - " + alarm_time + " - " + alarm_text + "";
                    dt.Rows.Add(new Object[] { alarm_time, alarm_level, alarm_text, alarm_solution });
                }
                dataGrdAlarm.Invoke(new Action(()=>
                {
                    SetAlarmGridFormat(dt);
                }));

            }
            else if (m_MessageName == ProxyMessage.MSG_ALARM_RESET)
            {
                ;
            }
            else if (m_MessageName == ProxyMessage.MSG_PLC_CONNECT || m_MessageName == ProxyMessage.MSG_PLC_DISCONNECT)
            {
                statusPictureBox1.Invoke(new Action(() =>
                {
                    statusPictureBox1.refreshStatus(rdKernel);
                }));
            }
            else if (m_MessageName == ProxyMessage.MSG_RECIPE_SET)
            {
                string Current_rcp = m_Event.EventData["CurrentRCP"];
                TxtRecipeName.Text = Current_rcp;
            }
        }

        private DataTable CreatAlarmTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("發生時間");
            dt.Columns.Add("異常等級");
            dt.Columns.Add("異常描述");
            dt.Columns.Add("解決辦法");
            return dt;
        }

        private void SetAlarmGridFormat(DataTable DT)
        {
            dataGrdAlarm.DataSource = DT;
            dataGrdAlarm.Columns[0].Width = 200;
            dataGrdAlarm.Columns[1].Width = 80;
            dataGrdAlarm.Columns[2].Width = 200;
            dataGrdAlarm.Columns[3].Width = 710;
            dataGrdAlarm.Refresh();
        }

        private void ReloadGui(Form m_Form)
        {
            if (currentForm == m_Form) return;
            if (currentForm != null)
            {
                currentForm.Hide();
            }

            if (m_Form != null && currentForm != m_Form)
            {
                m_Form.FormBorderStyle = FormBorderStyle.None;
                m_Form.TopLevel = false;
                panel1.Controls.Add(m_Form);
                m_Form.Show();

                currentForm = m_Form;
            }
        }

        private void Login_out(bool success, string id = "", int authority = 1)
        {
            TEvent data = new TEvent();
            data.MessageName = (success) ? ProxyMessage.MSG_USER_LOGIN : ProxyMessage.MSG_USER_LOGOUT;
            data.EventData["Authority"] = authority.ToString();           
            lblID.Text = (success)? id : "N/A";
            data.EventData["UserName"] = lblID.Text;
            ecClient.SendMessage(data);
        }

        private void LoadUserRegister()
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
            rdKernel.PlcKernel.Dispose();
            rdKernel.Dispose();
            System.Threading.Thread.Sleep(500);
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void btnVacuum_Click(object sender, EventArgs e)
        {
            if (   rdKernel.PlcKernel[ConstPlcDefine.PLC_DI_WATER_FLOW_1] == 0
                || rdKernel.PlcKernel[ConstPlcDefine.PLC_DI_WATER_FLOW_2] == 0)
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
            
            rdKernel.PlcKernel["HMI_Alarm_Reset"] = 1;
            System.Threading.Thread.Sleep(1000);

            TEvent data = new TEvent();
            data.MessageName = ProxyMessage.MSG_ALARM_RESET;

            ecClient.SendMessage(data);
            rdKernel.PlcKernel["HMI_Alarm_Reset"] = 0;
        }

        #region Form Buttom Click Event
        private void btnLogin_Click(object sender, EventArgs e)
        {
            login.LoginId = "";
            login.LoginPwd = "";
            DialogResult result = login.ShowDialog();

            if (result == DialogResult.Abort)
            {
                Login_out(false);
                db.InsertHistoryLog("N/A", "User Logout"); 
            }
            else if (result == DialogResult.OK)
            {
                string id = login.LoginId;
                string pwd = login.LoginPwd;
                if (!dicUsers.ContainsKey(id)) //有無該User
                {
                    MessageBox.Show("User no found", "Warning", MessageBoxButtons.OK);
                    return;
                }
                int authority = 1;
                Dictionary<String, KeyValuePair<String, String>> dicLogin = new Dictionary<string, KeyValuePair<string, string>>();
                dicLogin.Add(id, new KeyValuePair<string, string>(pwd, dicUsers[id].Value));

                if (!dicUsers.ContainsValue(dicLogin[id])) //查詢User對應密碼是否正確，錯誤則登出
                {
                    MessageBox.Show("User & Password mismatch", "Warning", MessageBoxButtons.OK);
                    Login_out(false, id, authority);
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
                Login_out(true, id, authority);
                db.InsertHistoryLog(id, "User Login");
            }
        }

        private void btnHistoryLog_Click(object sender, EventArgs e)
        {
            ReloadGui(log);
        }

        private void BtnAlarm_Click(object sender, EventArgs e)
        {
            ReloadGui(alarm);
        }

        private void btnRecipe_Click(object sender, EventArgs e)
        {
            ReloadGui(recipe);
        }

        private void btnSysPara_Click(object sender, EventArgs e)
        {
            ReloadGui(sysPara);
        }

        private void btnOverview_Click(object sender, EventArgs e)
        {
            ReloadGui(overview);
            overview.Show();
        }

        private void Control_Click(object sender, EventArgs e)
        {
            ReloadGui(control);
            control.Show();
        }

        private void btnGasView_Click(object sender, EventArgs e)
        {
            ReloadGui(gasView);
            gasView.Show();
        }

        private void btnProcView_Click(object sender, EventArgs e)
        {
            ReloadGui(process);
            process.Show();
        }

        private void btnDeviceConstant_Click(object sender, EventArgs e)
        {
            ReloadGui(deviceConstant);
            deviceConstant.Show();
        }

        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            ReloadGui(maintenance);
            maintenance.Show();
        }
        #endregion
    }
}
