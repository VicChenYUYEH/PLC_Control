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

namespace HyTemplate.gui
{
    public partial class frmUserRegister : Form
    { 
        private string sUserRegisterFileName = Directory.GetCurrentDirectory() + "\\config\\UserRegister.xml";
        private EventClient ecClient;
        public frmUserRegister()
        {
            InitializeComponent();

            ecClient = new EventClient(this);

            loadFile();
        }

        private bool loadFile()
        {
            if (!File.Exists(sUserRegisterFileName)) return false;

            dataGridView1.Rows.Clear();
            DataGridViewRowCollection rows = dataGridView1.Rows;

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(sUserRegisterFileName);

            XmlNodeList nodes = XmlDoc.SelectNodes("UserList/User");
            foreach (XmlNode chile_node in nodes)
            {
                String user_id = chile_node.Attributes["ID"].Value;
                String user_name = chile_node.Attributes["Name"].Value;
                String user_authority = chile_node.Attributes["Authority"].Value;

                rows.Add(new Object[] { user_id, user_name});
            }
            
            return true;
        }

        private bool saveFile()
        {
            if (!System.IO.File.Exists(sUserRegisterFileName)) return false;

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(sUserRegisterFileName);

            XmlNodeList nodes = XmlDoc.SelectNodes("UserList/User");

            DataGridViewRowCollection rows = dataGridView1.Rows;
            bool find = false;
            foreach (XmlNode chile_node in nodes)
            {
                String user_id = chile_node.Attributes["ID"].Value;
                if (textBox1.Text != user_id) continue;

                find = true;
                chile_node.Attributes["Name"].Value = textBox3.Text;
                chile_node.Attributes["Password"].Value = String.Format("{0:X8}", textBox2.Text.GetHashCode());
                chile_node.Attributes["Authority"].Value = String.Format("{0:X8}", comboBox1.Text.GetHashCode());

            }

            if (!find)
            {
                XmlNode node = XmlDoc.SelectSingleNode("UserList");//選擇節點

                //建立子節點
                XmlElement group = XmlDoc.CreateElement("User");
                group.SetAttribute("ID", textBox1.Text);//設定屬性
                group.SetAttribute("Name", textBox3.Text);//設定屬性
                group.SetAttribute("Authority", String.Format("{0:X8}", comboBox1.Text.GetHashCode()));//設定屬性
                group.SetAttribute("Password", String.Format("{0:X8}", textBox2.Text.GetHashCode()));


                node.AppendChild(group);
            }

            XmlDoc.Save(sUserRegisterFileName);
            return true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            textBox1.Text = row.Cells[0].Value.ToString();
            textBox3.Text = row.Cells[1].Value.ToString();

        }

        private void btnSaveChange_Click(object sender, EventArgs e)
        {
            if (this.saveFile())
            {
                this.loadFile();

                TEvent data = new TEvent();
                data.MessageName = ProxyMessage.MSG_USER_REGISTER_CHANGED;

                ecClient.SendMessage(data);
            }
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
