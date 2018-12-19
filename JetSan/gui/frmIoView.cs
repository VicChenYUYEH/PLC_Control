using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class frmIoView : Form
    {
        private EqBase ebKernel;
        Dictionary<string, KeyValuePair<string, int>> dicPlcMap;

        public frmIoView(EqBase m_EqBase)
        {
            InitializeComponent();

            InitialInputGrid(btnInput1.Text);
            InitialOutputGrid(btnOutput1.Text);

            ebKernel = m_EqBase;
            if (ebKernel != null)
            {
                dicPlcMap = ebKernel.PlcKernel.getPlcMap();
            }
        }

        private void InitialInputGrid(string m_Device)
        {
            dataGridViewInput.Rows.Clear();

            DataGridViewRowCollection rows = dataGridViewInput.Rows;

            for ( int index = 0; index < 16; index++ )
            {
                string device = m_Device.Substring(0,1) + (Int32.Parse(m_Device.Substring(1), NumberStyles.HexNumber) + index).ToString("X").PadLeft(5, '0');
                rows.Add(new Object[] { "X", device });

                dataGridViewInput.Rows[index].Cells[0].Style.BackColor = Color.Red;
            }
        }

        private void InitialOutputGrid(string m_Device)
        {
            dataGridViewOutput.Rows.Clear();

            DataGridViewRowCollection rows = dataGridViewOutput.Rows;

            for (int index = 0; index < 16; index++)
            {
                string device = m_Device.Substring(0, 1) + (Int32.Parse(m_Device.Substring(1), NumberStyles.HexNumber) + index).ToString("X").PadLeft(5, '0');
                rows.Add(new Object[] { "X", device });

                dataGridViewOutput.Rows[index].Cells[0].Style.BackColor = Color.Red;
            }
        }

        private void btnInput1_Click(object sender, EventArgs e)
        {
            timerStatus.Enabled = false;
            InitialInputGrid(((Button)sender).Text);
            timerStatus.Enabled = true;
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                timerStatus.Enabled = false;
                return;
            }

            if (ebKernel == null) return;

            try
            {
                for (int index = 0; index < 16; index++)
                {
                    //Output
                    string device = dataGridViewOutput.Rows[index].Cells[1].Value.ToString();
                    KeyValuePair<string, KeyValuePair<string, int>> tmp = dicPlcMap.FirstOrDefault(t => t.Value.Key == device);
                    if (tmp.Key != null)
                    {
                        string device_name = tmp.Key;
                        if (ebKernel.PlcKernel[device_name] == 1)
                        {
                            dataGridViewOutput.Rows[index].Cells[0].Value = "O";
                            dataGridViewOutput.Rows[index].Cells[0].Style.BackColor = Color.Lime;
                        }
                        else
                        {
                            dataGridViewOutput.Rows[index].Cells[0].Value = "X";
                            dataGridViewOutput.Rows[index].Cells[0].Style.BackColor = Color.Red;
                        }
                    }

                    //Input
                    device = dataGridViewInput.Rows[index].Cells[1].Value.ToString();
                    tmp = dicPlcMap.FirstOrDefault(t => t.Value.Key == device);
                    if (tmp.Key != null)
                    {
                        string device_name = tmp.Key;
                        if (ebKernel.PlcKernel[device_name] == 1)
                        {
                            dataGridViewInput.Rows[index].Cells[0].Value = "O";
                            dataGridViewInput.Rows[index].Cells[0].Style.BackColor = Color.Lime;
                        }
                        else
                        {
                            dataGridViewInput.Rows[index].Cells[0].Value = "X";
                            dataGridViewInput.Rows[index].Cells[0].Style.BackColor = Color.Red;
                        }
                    }
                        
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void frmIoView_VisibleChanged(object sender, EventArgs e)
        {
            timerStatus.Enabled = this.Visible;
        }

        private void btnOutput1_Click(object sender, EventArgs e)
        {
            timerStatus.Enabled = false;
            InitialOutputGrid(((Button)sender).Text);
            timerStatus.Enabled = true;
        }

        private void frmIoView_Shown(object sender, EventArgs e)
        {
            timerStatus.Enabled = true;
        }

        public new void Show()
        {
            timerStatus.Enabled = true;
        }
    }
}
