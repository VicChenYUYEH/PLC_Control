using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using HongYuDLL;

namespace HyTemplate.gui
{
    public partial class FrmLogin : Form
    {
        private string sUserRegisterFileName = Directory.GetCurrentDirectory() + "\\config\\UserRegister.xml";
        EventClient ecClient;

        public string sLoginId { get { return textBox1.Text; } set { textBox1.Text = value; } }
        public string sLoginPwd { get { return textBox2.Text; } set { textBox2.Text = value; } }

        FrmUserRegister frmRegister;
        public FrmLogin()
        {
            InitializeComponent();
            ecClient = new EventClient(this);
            ecClient.OnEventHandler += onReceiveMessage;

            frmRegister = new FrmUserRegister();
        }

        private bool loadFile()
        {
            if (!File.Exists(sUserRegisterFileName)) return false;

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(sUserRegisterFileName);

            XmlNodeList nodes = XmlDoc.SelectNodes("UserList/User");
            foreach (XmlNode chile_node in nodes)
            {
                String user_id = chile_node.Attributes["ID"].Value;
                String user_name = chile_node.Attributes["Name"].Value;

            }

            return true;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            frmRegister.Show();      
        }

        private void onReceiveMessage(string m_MessageName, TEvent m_Event)
        {
            // System.Threading.Thread.Sleep(100);
            if (m_MessageName == ProxyMessage.MSG_USER_LOGIN)
            {
                int authority = int.Parse(m_Event.EventData["Authority"]); switch (authority)
                {
                    case 1: //OP
                        btn_Control(false);
                        break;
                    case 2: //Engineer
                    case 3: //Supervisor
                    case 4: //Administrator
                        btn_Control(true);
                        break;
                }
            }
            else if (m_MessageName == ProxyMessage.MSG_USER_LOGOUT)
            {
                btn_Control(false);
            }
        }
        private void btn_Control(bool enable)
        {
            btnRegister.Enabled = enable;
        }
    }
}
