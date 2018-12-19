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

        frmHistoryLog frmLog;
        frmHistoryAlarm frmAlarm;
        frmRecipe frmRecipe;
        frmSystemParameter frmSysPara;
        frmIoView frmIoView;
        frmLogin frmLogin;
        frmOverview frmOverview;

        RdEqKernel rdKernel;

        Form currentForm = null;

        public frmMain()
        {
            InitializeComponent();

            ecClient = new EventClient(this);
            ecClient.OnEventHandler += OnReceiveMessage;

            rdKernel = new RdEqKernel();

            rRecipe = new Recipe();

            frmLog = new frmHistoryLog();
            frmAlarm = new frmHistoryAlarm();
            frmRecipe = new frmRecipe(rRecipe);
            frmSysPara = new frmSystemParameter(rRecipe);
            frmIoView = new frmIoView(rdKernel);
            frmLogin = new frmLogin();
            frmOverview = new frmOverview(rdKernel);

            //timerStatus.Enabled = true;

            this.LoadUserRegister();

            System.Threading.Thread.Sleep(2000);

            ReloadGui(frmOverview);

            checkInitialStatus();

            Login_out(false);
        }

        private void checkInitialStatus()
        {
            if (rdKernel.PlcKernel["X00024"] == 1 || rdKernel.PlcKernel["X00024"] == 1 || rdKernel.PlcKernel["X00025"] == 1)
            {
                btnVacuum.BackColor = Color.Yellow;

                TEvent data = new TEvent();
                data.MessageName = ProxyMessage.MSG_PROCESS_VACUUM;

                ecClient.SendMessage(data);
            }
        }

        private void OnReceiveMessage(string m_MessageName, TEvent m_Event)
        {
            //            System.Threading.Thread.Sleep(100);
            if (m_MessageName == ProxyMessage.MSG_USER_REGISTER_CHANGED)
            {
                this.LoadUserRegister();
            }
            else if (m_MessageName == ProxyMessage.MSG_PROCESS_VACUUM_COMPLETE)
            {
                btnVacuum.BackColor = Color.Lime;
            }
            else if (m_MessageName == ProxyMessage.MSG_ALARM_OCCURE)
            {
                int alarm_count = Convert.ToInt16(m_Event.EventData["Count"]);
                for (int index = 1; index <= alarm_count; index++)
                {
                    string alarm_id = m_Event.EventData["Code" + index.ToString()];
                    bool is_heavy_alarm = m_Event.EventData["HeavyAlarm" + index.ToString()] == "1" ? true : false;
                    string alarm_text = m_Event.EventData["Description" + index.ToString()];

                    string alarm_msg = (is_heavy_alarm ? "Alarm" : "Warning") + " - " + alarm_id + " - " + alarm_text;
                    displayTextBox_Alarm.Text = alarm_msg;
                    System.Threading.Thread.Sleep(100);
                }
            }
            else if (m_MessageName == ProxyMessage.MSG_PLC_CONNECT)
            {
                statusPictureBox1.refreshStatus(rdKernel);
            }
        }

        private void btnHistoryLog_Click(object sender, EventArgs e)
        {
            ReloadGui(frmLog);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ReloadGui(frmAlarm);
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

        private void btnRecipe_Click(object sender, EventArgs e)
        {
            ReloadGui(frmRecipe);
        }

        private void btnOverview_Click(object sender, EventArgs e)
        {
            ReloadGui(frmOverview);
            frmOverview.Show();
        }

        private void btnSysPara_Click(object sender, EventArgs e)
        {
            ReloadGui(frmSysPara);
        }

        private void btnIoView_Click(object sender, EventArgs e)
        {//frmIoView
            ReloadGui(frmIoView);
            frmIoView.Show();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            frmLogin.LoginId = "";
            frmLogin.LoginPwd = "";
            DialogResult result = frmLogin.ShowDialog();

            if (result == DialogResult.Abort)
            {
                if (MessageBox.Show("Really want to quit ?!", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Close();
                }                
            }
            else if (result == DialogResult.OK)
            {
                string id = frmLogin.LoginId;
                string pwd = frmLogin.LoginPwd;
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
            }
        }

        private void Login_out(bool success, string id = "", int authority = 1)
        {
            TEvent data = new TEvent();
            data.MessageName = ProxyMessage.MSG_USER_LOGIN;
            data.EventData["Authority"] = authority.ToString();           
            lblID.Text = (success)? id : "None";
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

            btnVacuum.BackColor = Color.Yellow;

            TEvent data = new TEvent();
            data.MessageName = ProxyMessage.MSG_PROCESS_VACUUM;

            ecClient.SendMessage(data);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            rdKernel.PlcKernel[ConstPlcDefine.PLC_BUF_ALM_RST] = 1;
            System.Threading.Thread.Sleep(100);

            TEvent data = new TEvent();
            data.MessageName = ProxyMessage.MSG_ALARM_RESET;

            ecClient.SendMessage(data);
            rdKernel.PlcKernel[ConstPlcDefine.PLC_BUF_ALM_RST] = 0;
        }
    }
}
