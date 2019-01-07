using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class frmSystemParameter : Form
    {
        Recipe rRecipe;
        EventClient ecClient;

        public frmSystemParameter(Recipe m_Recipe)
        {
            InitializeComponent();
            ecClient = new EventClient(this);
            ecClient.OnEventHandler += OnReceiveMessage;

            rRecipe = m_Recipe;
            rRecipe.loadFile();

            InitialRecipeBody("System");
        }
        
        private void InitialRecipeBody(string m_RcpId)
        {
            dataGridView1.Rows.Clear();
            DataGridViewRowCollection rows = dataGridView1.Rows;

            foreach (XmlItem item in rRecipe[m_RcpId].Nodes)
            {
                rows.Add(new Object[] { item.Key, item.Value, rRecipe.RecipeDetail[item.Key].Unit, rRecipe.RecipeDetail[item.Key].Description });

            }
        }

        private void btnSaveChange_Click(object sender, EventArgs e)
        {
            DataGridViewRowCollection rows = dataGridView1.Rows;
            foreach (DataGridViewRow row in rows)
            {
                string key = row.Cells[0].Value.ToString();
                string value = row.Cells[1].Value.ToString();
                rRecipe["System"][key].Value = value;
            }
            rRecipe.saveFile();
            TEvent data = new TEvent();
            data.MessageName = ProxyMessage.MSG_PARAMETER_SET;
            ecClient.SendMessage(data);
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                //儲存編輯前的文字，可以用來復原編輯前的狀態
                //若Value為null，則會設為空字串
                //textBeforeEdit = (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "").ToString();
                return;
            }
            else
            {
                e.Cancel = true; //讓使用者無法繼續進行修改
            }
        }

        private void OnReceiveMessage(string m_MessageName, TEvent m_Event)
        {
            // System.Threading.Thread.Sleep(100);
            if (m_MessageName == ProxyMessage.MSG_USER_LOGIN)
            {
                int authority = int.Parse(m_Event.EventData["Authority"]);
                switch (authority)
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
        }
        private void btn_Control(bool enable)
        {
            btnSaveChange.Enabled = enable;
        }
    }
}
