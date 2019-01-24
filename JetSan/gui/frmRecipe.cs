using System;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class FrmRecipe : Form
    {
        private RdEqKernel rdKernel;
        private EventClient ecClient;
        string sCurrentUser ="N/A";
        int iAuthority = 1;
        public FrmRecipe(RdEqKernel m_Kernel)
        {
            InitializeComponent();

            ecClient = new EventClient(this);
            ecClient.OnEventHandler += onReceiveMessage;

            rdKernel = m_Kernel;
            rdKernel.rRecipe.LoadFile();

            initialRecipeTable();
        }

        private void initialRecipeTable()
        {
            listView1.Items.Clear();

            foreach (XmlItem rcp_item in rdKernel.rRecipe.GetNodes())
            {
                if (rcp_item.Key == "System") continue;

                ListViewItem item = new ListViewItem(rcp_item.Key); //ID   
                string in_use = (rcp_item.Value == "true") ? "使用中" : "關閉";
                item.SubItems.Add(in_use);                          //Default
                listView1.Items.Add(item);
            }
            dataGridView1.Rows.Clear();
            if(listView1.Items.Count <= 1)//少於1個不可刪除
            {
                btnDelete.Enabled = false;
            }
            else
            {   //有權限才解鎖
                if(iAuthority == 4) btnDelete.Enabled = true;
            }
            listView1.Focus();
            listView1.Items[0].Selected = true;
        }

        private void initialRecipeBody(string m_RcpId)
        {
            dataGridView1.Rows.Clear();
            DataGridViewRowCollection rows = dataGridView1.Rows;

            foreach (XmlItem item in rdKernel.rRecipe[m_RcpId].Nodes)
            {
                rows.Add(new Object[] { item.Key, item.Value, rdKernel.rRecipe.DicRecipeDetail[item.Key].Unit, rdKernel.rRecipe.DicRecipeDetail[item.Key].Address, rdKernel.rRecipe.DicRecipeDetail[item.Key].Description });
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

        private void btnCreate_Click(object sender, EventArgs e)
        {
            DlgConfirm dlg = new DlgConfirm();
            dlg.sConfirmId = "";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.sConfirmId.Trim() == "") return;

                rdKernel.rRecipe.AppendRecipe(dlg.sConfirmId, "false");
                rdKernel.rRecipe.SaveFile();
                rdKernel.rRecipe.LoadFile();
                initialRecipeTable();
                string err = rdKernel.InsertHistoryLog(sCurrentUser, "Creat New Recipe", m_Recipe: dlg.sConfirmId);
                rdKernel.WriteOperatorLog("Creat New Recipe", dlg.sConfirmId);
            }
            dlg.Dispose();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selected = listView1.SelectedItems;

            if (selected.Count > 0)
            {
                if (MessageBox.Show("Really want to delete Recipe[" + selected[0].Text + "]", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    rdKernel.rRecipe.EraseRecipe(selected[0].Text);
                    rdKernel.rRecipe.LoadFile();
                    initialRecipeTable();
                    string err = rdKernel.InsertHistoryLog(sCurrentUser, "Delete Recipe", m_Recipe: selected[0].Text);

                    rdKernel.WriteOperatorLog("Delete Recipe", selected[0].Text);
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selected = listView1.SelectedItems;

            if (selected.Count > 0)
            {
                initialRecipeBody(selected[0].Text);
            }            
        }

        private void btnSaveChange_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(" 是 否 儲 存 該 Recipe 參 數 ?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;
            ListView.SelectedListViewItemCollection selected = listView1.SelectedItems;

            if (selected.Count > 0)
            {
                DataGridViewRowCollection rows = dataGridView1.Rows;
                foreach (DataGridViewRow row in rows)
                {
                    string key = row.Cells[0].Value.ToString();
                    string value = row.Cells[1].Value.ToString();
                    rdKernel.rRecipe[selected[0].Text][key].Value = value;
                }
                rdKernel.rRecipe.SaveFile();
                rdKernel.rRecipe.LoadFile();
                listView1.Refresh();
                string err = rdKernel.InsertHistoryLog(sCurrentUser, "Recipe Data Change", m_Recipe: selected[0].Text);
                rdKernel.WriteOperatorLog("Recipe Data Change", selected[0].Text);
                if (err != "")
                {
                    rdKernel.WriteDebugLog("DB_Fail", err);
                }
            }
        }

        private void onReceiveMessage(string m_MessageName, TEvent m_Event)
        {
            if (m_MessageName == ProxyMessage.MSG_USER_LOGIN)
            {
                iAuthority = int.Parse(m_Event.EventData["Authority"]);
                sCurrentUser = m_Event.EventData["UserName"];
                switch (iAuthority)
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

        private void btn_Control(bool m_enable)
        {
            btnCreate.Enabled = m_enable;
            if (listView1.Items.Count <= 1)//少於1個不可刪除
            {
                btnDelete.Enabled = false;
            }
            else btnDelete.Enabled = m_enable;
            btnSaveChange.Enabled = m_enable;
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(" 是 否 寫 入 Recipe 參 數 ?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;
            ListView.SelectedListViewItemCollection selected = listView1.SelectedItems;

            if (selected.Count > 0)
            {
                string current_rcp;
                TEvent data = new TEvent();
                data.MessageName = ProxyMessage.MSG_RECIPE_SET;
                data.EventData["RecipeId"] = selected[0].Text;
                //寫入使用中參數
                foreach (XmlItem rcp_item in rdKernel.rRecipe.GetNodes())
                {
                    if (rcp_item.Key == "System") continue;

                    rdKernel.rRecipe[rcp_item.Key].Value = "false"; //皆改為關閉
                }
                rdKernel.rRecipe[selected[0].Text].Value = "true";
                rdKernel.rRecipe.SaveFile();
                rdKernel.rRecipe.LoadFile();
                initialRecipeTable();
                GetCurrentRecipeName(out current_rcp);
                data.EventData["CurrentRCP"] = current_rcp;
                ecClient.SendMessage(data);
                string err = rdKernel.InsertHistoryLog(sCurrentUser, "Recipe Set", m_Recipe: current_rcp);
                rdKernel.WriteOperatorLog("Recipe Set", current_rcp);
                if (err != "")
                {
                    rdKernel.WriteDebugLog("DB_Fail", err);
                }
            }
        }

        private void frmRecipe_Load(object sender, EventArgs e)
        {
            listView1.Focus();
            listView1.Items[0].Selected = true;
        }

        public void GetCurrentRecipeName(out string m_rcp)
        {
            m_rcp = "";
            foreach (XmlItem rcp_item in rdKernel.rRecipe.GetNodes())
            {
                if (rcp_item.Key == "System") continue;

                if(rdKernel.rRecipe[rcp_item.Key].Value == "true")
                {
                    m_rcp = rdKernel.rRecipe[rcp_item.Key].Key;
                }
            }
        }
    }
}
