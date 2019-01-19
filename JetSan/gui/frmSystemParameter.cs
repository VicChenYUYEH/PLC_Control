using System;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class FrmSystemParameter : Form
    {
        private RdEqKernel rdKernel;
        private EventClient ecClient;
        string sCurrentUser = "";

        public FrmSystemParameter(RdEqKernel m_Kernel)
        {
            InitializeComponent();
            ecClient = new EventClient(this);
            ecClient.OnEventHandler += onReceiveMessage;

            rdKernel = m_Kernel;
            rdKernel.rRecipe.LoadFile();

            initialRecipeBody("System");
        }
        
        private void initialRecipeBody(string m_RcpId)
        {
            dataGridView1.Rows.Clear();
            DataGridViewRowCollection rows = dataGridView1.Rows;

            foreach (XmlItem item in rdKernel.rRecipe[m_RcpId].Nodes)
            {
                rows.Add(new Object[] { item.Key, item.Value, rdKernel.rRecipe.DicSystemDetail[item.Key].Unit, rdKernel.rRecipe.DicSystemDetail[item.Key].Address, rdKernel.rRecipe.DicSystemDetail[item.Key].Description });
            }
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

        private void onReceiveMessage(string m_MessageName, TEvent m_Event)
        {
            if (m_MessageName == ProxyMessage.MSG_USER_LOGIN)
            {
                int authority = int.Parse(m_Event.EventData["Authority"]);
                string currentUser = m_Event.EventData["UserName"];
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
            else if(m_MessageName == ProxyMessage.MSG_USER_LOGOUT)
            {
                btn_Control(false);
            }
        }
        private void btn_Control(bool m_enable)
        {
            button6.Enabled = m_enable;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(" 是 否 寫 入 系 統 參 數 ?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;
            DataGridViewRowCollection rows = dataGridView1.Rows;
            foreach (DataGridViewRow row in rows)
            {
                string key = row.Cells[0].Value.ToString();
                string value = row.Cells[1].Value.ToString();
                rdKernel.rRecipe["System"][key].Value = value;
            }
            rdKernel.rRecipe.SaveFile();
            TEvent data = new TEvent();
            data.MessageName = ProxyMessage.MSG_PARAMETER_SET;
            ecClient.SendMessage(data);
            rdKernel.InsertHistoryLog(sCurrentUser, "System Parameter Set");
            rdKernel.WriteOperatorLog("System Parameter Set");
        }
    }
}
