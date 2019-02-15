using HyTemplate.components;
using System;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class FrmControl : Form
    {
        private EqBase ebKernel;

        public FrmControl(EqBase m_EqBase)
        {
            InitializeComponent();

            ebKernel = m_EqBase;

            if (ebKernel != null)
            {
                #region Initial Component
                initialComponents(this);
                #endregion
            }

            //System.Threading.Thread.Sleep(1000);
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            if (ebKernel == null)
            {
                return;
            }
            else if (!this.Visible)
            {
                timerStatus.Enabled = false;
            }
            refreshStatus(this);
        }

        private void refreshStatus(Control m_Object)
        {
            foreach (Control obj in m_Object.Controls)
            {
                if (obj.GetType().Equals(typeof(PlcObject)))
                {
                    ((PlcObject)obj).RefreshStatus();
                }
                else if (obj.GetType().Equals(typeof(DisplayTextBox)))
                {
                    ((DisplayTextBox)obj).RefreshData();
                }
                else if (obj.GetType().Equals(typeof(StatusPictureBox)))
                {
                    ((StatusPictureBox)obj).RefreshStatus();
                }
                else if (obj.GetType().Equals(typeof(InputTextBox)))
                {
                    ((InputTextBox)obj).RefreshData();
                }
                else if (obj.GetType().Equals(typeof(ControlBtn)))
                {
                    ((ControlBtn)obj).RefreshStatus();
                }
                else if (obj.GetType().Equals(typeof(TabControl)) || obj.GetType().Equals(typeof(TabPage)) || obj.GetType().Equals(typeof(GroupBox)))
                {
                    refreshStatus(obj);
                }
            }

            if (ebKernel.pPlcKernel["HMI_Service_OnOff"] == 1)
            {
                controlBtn2.Enabled = false;
                controlBtn8.Enabled = false;
                controlBtn3.Enabled = false;
                controlBtn9.Enabled = false;
                controlBtn4.Enabled = false;
                controlBtn10.Enabled = false;
                controlBtn5.Enabled = false;
            }
            else
            {
                controlBtn3.Enabled = true;
                controlBtn9.Enabled = true;
                controlBtn4.Enabled = true;
                controlBtn10.Enabled = true;
                controlBtn5.Enabled = true;
                controlBtn2.Enabled = (ebKernel.pPlcKernel["HMI_Process_Start"] == 1)? false: true;
                controlBtn8.Enabled = (ebKernel.pPlcKernel["HMI_Process_Stop"] == 1) ? false : true;
            }
        }

        private void initialComponents(Control m_Object)
        {
            foreach (Control obj in m_Object.Controls)
            {
                if (obj.GetType().Equals(typeof(PlcObject)))
                {
                    ((PlcObject)obj)._EqBase = ebKernel;
                }
                else if (obj.GetType().Equals(typeof(DisplayTextBox)))
                {
                    ((DisplayTextBox)obj)._EqBase = ebKernel;
                }
                else if (obj.GetType().Equals(typeof(InputTextBox)))
                {
                    ((InputTextBox)obj)._EqBase = ebKernel;
                }
                else if (obj.GetType().Equals(typeof(StatusPictureBox)))
                {
                    ((StatusPictureBox)obj)._EqBase = ebKernel;
                }
                else if (obj.GetType().Equals(typeof(ControlBtn)))
                {
                    ((ControlBtn)obj)._EqBase = ebKernel;
                }
                else if (obj.GetType().Equals(typeof(TabControl)) || obj.GetType().Equals(typeof(TabPage)) || obj.GetType().Equals(typeof(GroupBox)))
                {
                    initialComponents(obj);
                }
            }
        }

        private void frmControl_Shown(object sender, EventArgs e)
        {
            timerStatus.Enabled = true;
        }
        public new void Show()
        {
            timerStatus.Enabled = true;
        }
    }
}
