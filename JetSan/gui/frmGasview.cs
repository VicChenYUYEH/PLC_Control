﻿using HyTemplate.components;
using System;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class frmGasview : Form
    {
        private EqBase ebKernel;

        public frmGasview(EqBase m_EqBase)
        {
            InitializeComponent();

            ebKernel = m_EqBase;

            if (ebKernel != null)
            {
                #region Initial Component
                initialComponents(TabControl_Main);                
                #endregion
            }

            System.Threading.Thread.Sleep(1000);
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
            refreshStatus(TabControl_Main.SelectedTab);
        }

        private void refreshStatus(Control m_Object)
        {
            foreach (Control obj in m_Object.Controls)
            {
                if (obj.GetType().Equals(typeof(PlcObject)))
                {
                    ((PlcObject)obj).refreshStatus();
                }
                else if (obj.GetType().Equals(typeof(DisplayTextBox)))
                {
                    ((DisplayTextBox)obj).refreshData();
                }
                else if (obj.GetType().Equals(typeof(StatusPictureBox)))
                {
                    ((StatusPictureBox)obj).refreshStatus();
                }
                else if (obj.GetType().Equals(typeof(InputTextBox)))
                {
                    ((InputTextBox)obj).refreshData();
                }
                else if (obj.GetType().Equals(typeof(ControlBtn)))
                {
                    ((ControlBtn)obj).refreshStatus();
                }
                else if (obj.GetType().Equals(typeof(GroupBox)))
                {
                    refreshStatus(obj);
                }
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
                else if (obj.GetType().Equals(typeof(TabPage)) || obj.GetType().Equals(typeof(GroupBox)))
                {
                    initialComponents(obj);                    
                }
            }
        }

        private void frmGasview_Shown(object sender, EventArgs e)
        {
            timerStatus.Enabled = true;
        }
        public new void Show()
        {
            timerStatus.Enabled = true;
        }
    }
}
