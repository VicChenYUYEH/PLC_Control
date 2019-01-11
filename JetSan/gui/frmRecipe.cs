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

namespace HyTemplate.gui
{
    public partial class frmRecipe : Form
    {
        Recipe rRecipe;
        EventClient ecClient;
        int authority = 1;
        public frmRecipe(Recipe m_Recipe)
        {
            InitializeComponent();

            ecClient = new EventClient(this);
            ecClient.OnEventHandler += OnReceiveMessage;

            rRecipe = m_Recipe;
            rRecipe.loadFile();

            InitialRecipeTable();
        }

        private void InitialRecipeTable()
        {
            listView1.Items.Clear();

            foreach (XmlItem rcp_item in rRecipe.getNodes())
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
                if(authority == 4) btnDelete.Enabled = true;
            }
            listView1.Focus();
            listView1.Items[0].Selected = true;
        }

        private void InitialRecipeBody(string m_RcpId)
        {
            dataGridView1.Rows.Clear();
            DataGridViewRowCollection rows = dataGridView1.Rows;

            foreach (XmlItem item in rRecipe[m_RcpId].Nodes)
            {
                rows.Add(new Object[] { item.Key, item.Value, rRecipe.RecipeDetail[item.Key].Unit, rRecipe.RecipeDetail[item.Key].Address, rRecipe.RecipeDetail[item.Key].Description });
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
            dlgConfirm dlg = new dlgConfirm();
            dlg.ConfirmId = "";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.ConfirmId.Trim() == "") return;
                
                rRecipe.appendRecipe(dlg.ConfirmId, "false");
                rRecipe.saveFile();
                rRecipe.loadFile();
                InitialRecipeTable();
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
                    rRecipe.eraseRecipe(selected[0].Text);
                    rRecipe.loadFile();
                    InitialRecipeTable();
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selected = listView1.SelectedItems;

            if (selected.Count > 0)
            {
                InitialRecipeBody(selected[0].Text);
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
                    rRecipe[selected[0].Text][key].Value = value;
                }
                rRecipe.saveFile();
                rRecipe.loadFile();
                listView1.Refresh();
            }
        }

        private void OnReceiveMessage(string m_MessageName, TEvent m_Event)
        {
            if (m_MessageName == ProxyMessage.MSG_USER_LOGIN)
            {
                authority = int.Parse(m_Event.EventData["Authority"]);
                switch(authority)
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
            btnCreate.Enabled = enable;
            if (listView1.Items.Count <= 1)//少於1個不可刪除
            {
                btnDelete.Enabled = false;
            }
            else btnDelete.Enabled = enable;
            btnSaveChange.Enabled = enable;
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
                foreach (XmlItem rcp_item in rRecipe.getNodes())
                {
                    if (rcp_item.Key == "System") continue;

                    rRecipe[rcp_item.Key].Value = "false"; //皆改為關閉
                }
                rRecipe[selected[0].Text].Value = "true";
                rRecipe.saveFile();
                rRecipe.loadFile();
                InitialRecipeTable();
                getCurrentRecipeName(out current_rcp);
                data.EventData["CurrentRCP"] = current_rcp;
                ecClient.SendMessage(data);
            }
        }

        private void frmRecipe_Load(object sender, EventArgs e)
        {
            listView1.Focus();
            listView1.Items[0].Selected = true;
        }

        public void getCurrentRecipeName(out string rcp)
        {
            rcp = "";
            foreach (XmlItem rcp_item in rRecipe.getNodes())
            {
                if (rcp_item.Key == "System") continue;

                if(rRecipe[rcp_item.Key].Value == "true")
                {
                    rcp = rRecipe[rcp_item.Key].Key;
                }
            }
        }
    }
}
