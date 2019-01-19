using HyTemplate.components;
using System;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class FrmOverview : Form
    {
        private EqBase ebKernel;

        public FrmOverview(EqBase m_EqBase)
        {
            InitializeComponent();

            ebKernel = m_EqBase;

            if (ebKernel != null)
            {
                #region Initial Component
                initialComponents(this);                
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
            refreshStatus(this);
        }
             
        private void plcObject19_DoubleClick(object sender, EventArgs e)
        {
            //bool status = ebKernel.PlcKernel[statusPictureBox2._PlcDevice] == 1 ? true : false;
            //dlgSwitch dlg = new dlgSwitch(status);
            //dlg.PlcDevice = statusPictureBox2._PlcDevice;

            //DialogResult result = dlg.ShowDialog();
            //if (result == DialogResult.Yes)
            //{
            //    //Power02_INTERLOCK <== ON
            //    ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_INTERLOCK] = 1;
            //    //Power02_PULSE ON <== ON
            //    ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_PULSE] = 1;
            //    //Power02_OFF <== ON
            //    ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_OFF] = 1;

            //    System.Threading.Thread.Sleep(1000);
            //    //Power01_OUTPUT ON <== ON
            //    ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_OUTPUT] = 1;
            //}
            //else if (result == DialogResult.No)
            //{
            //    //Power01_OUTPUT ON <== OFF
            //    ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_OUTPUT] = 0;

            //    System.Threading.Thread.Sleep(1000);

            //    //Power02_INTERLOCK <== OFF
            //    ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_INTERLOCK] = 0;
            //    //Power02_PULSE ON <== OFF
            //    ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_PULSE] = 0;
            //    //Power02_OFF <== OFF
            //    ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_OFF] = 0;
            //}
        }

        private void turboPump2_DoubleClick(object sender, EventArgs e)
        {
            ////Check Valve
            ////if (   ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_OPEN] == 0
            ////    || ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_CLOSE] == 1)
            ////{
            ////    turboPump2.ReadyToStart = false;
            ////    return;
            ////}
            //dlgSwitch dlg = new dlgSwitch((ebKernel.PlcKernel[turboPump2._PlcStartDevice] == 1 ? true : false));
            //dlg.PlcDevice = turboPump2._PlcStartDevice;
            //DialogResult result = dlg.ShowDialog();
            //if (result == DialogResult.Yes)
            //{
            //    if (ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_RP_3_ON] == 0)
            //    {
            //        MessageBox.Show("RP is not open !!");
            //        return;
            //    }
            //    else if (ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_LVG_3_ON] == 0)
            //    {//Check LVG
            //        MessageBox.Show("LVG not ready !!");
            //        return;
            //    }
            //    //else if (ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_WATER_FLOW_1] == 0
            //    //         || ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_WATER_FLOW_1] == 0)
            //    //{//Check Water
            //    //    MessageBox.Show("Please Check Water !!");
            //    //    return;
            //    //}

            //    //if (checkTpStartCondition())
            //    {
            //        ebKernel.PlcKernel[turboPump2._PlcStopDevice] = 0;
            //        ebKernel.PlcKernel[turboPump2._PlcStartDevice] = 1;

            //        //ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_OPEN] = 1;
            //    }
                    
            //}
            //else if (result == DialogResult.No)
            //{
            //    //Check Valve
            //    //if (   ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_OPEN] == 1
            //    //    || ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_CLOSE] == 0)
            //    //{
            //    //    return;
            //    //}

            //    if (turboPump1._PlcStopDevice.Trim() == "")
            //    {
            //        ebKernel.PlcKernel[turboPump2._PlcStartDevice] = 0;
            //    }
            //    else
            //    {
            //        ebKernel.PlcKernel[turboPump2._PlcStartDevice] = 0;
            //        ebKernel.PlcKernel[turboPump2._PlcStopDevice] = 1;
            //    }

            //}
            //dlg.Dispose();
            
        }

        private void turboPump1_DoubleClick(object sender, EventArgs e)
        {
            //dlgSwitch dlg = new dlgSwitch((ebKernel.PlcKernel[turboPump1._PlcStartDevice] == 1 ? true : false));
            //dlg.PlcDevice = turboPump1._PlcStartDevice;
            //DialogResult result = dlg.ShowDialog();
            //if (result == DialogResult.Yes)
            //{
            //    if (ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_RP_2_ON] == 0)
            //    {
            //        MessageBox.Show("RP is not open !!");
            //        return;
            //    }
            //    else if (ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_LVG_2_ON] == 0)
            //    {//Check LVG
            //        MessageBox.Show("LVG not ready !!");
            //        return;
            //    }

            //    //if (checkTpStartCondition())
            //    {
            //        ebKernel.PlcKernel[turboPump1._PlcStopDevice] = 0;
            //        ebKernel.PlcKernel[turboPump1._PlcStartDevice] = 1;

            //        //ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_3_OPEN] = 1;
            //    }
                
            //}
            //else if (result == DialogResult.No)
            //{
            //    //Check Valve
            //    //if (   ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_3_OPEN] == 1
            //    //    || ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_3_CLOSE] == 0)
            //    //{
            //    //    return;
            //    //}

            //    if (turboPump1._PlcStopDevice.Trim() == "")
            //    {
            //        ebKernel.PlcKernel[turboPump1._PlcStartDevice] = 0;
            //    }
            //    else
            //    {
            //        ebKernel.PlcKernel[turboPump1._PlcStartDevice] = 0;
            //        ebKernel.PlcKernel[turboPump1._PlcStopDevice] = 1;
            //    }

            //}
            //dlg.Dispose();            
        }

        private bool checkTpStartCondition()
        {
            if (   ebKernel.pPlcKernel[ConstPlcDefine.PLC_DI_RP_1_ON] == 0
                //|| ebKernel.PlcKernel[ConstPlcDefine.PLC_DI_RP_2_ON] == 0
                || ebKernel.pPlcKernel[ConstPlcDefine.PLC_DI_RP_3_ON] == 0)
            {
                MessageBox.Show("RP is not open !!");
                return false;
            }
            else if (   ebKernel.pPlcKernel[ConstPlcDefine.PLC_DI_LVG_2_ON] == 0
                     || ebKernel.pPlcKernel[ConstPlcDefine.PLC_DI_LVG_3_ON] == 0
                     || ebKernel.pPlcKernel[ConstPlcDefine.PLC_DI_HVG_L] == 0)
            {//Check LVG
                MessageBox.Show("LVG not ready !!");
                return false;
            }
            else if (   ebKernel.pPlcKernel[ConstPlcDefine.PLC_DI_WATER_FLOW_1] == 0
                     || ebKernel.pPlcKernel[ConstPlcDefine.PLC_DI_WATER_FLOW_1] == 0)
            {//Check Water
                MessageBox.Show("Please Check Water !!");
                return false;
            }

            return true;
        }

        private void plcObject22_DoubleClick(object sender, EventArgs e)
        {
            //bool status = ebKernel.PlcKernel[statusPictureBox3._PlcDevice] == 1 ? true : false;
            //dlgSwitch dlg = new dlgSwitch(status);
            //dlg.PlcDevice = statusPictureBox3._PlcDevice;

            //DialogResult result = dlg.ShowDialog();
            //if (result == DialogResult.Yes)
            //{
            //    ////Power02_INTERLOCK <== ON
            //    //ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_INTERLOCK] = 1;
            //    ////Power02_PULSE ON <== ON
            //    //ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_PULSE] = 1;
            //    ////Power02_OFF <== ON
            //    //ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_OFF] = 1;

            //    //System.Threading.Thread.Sleep(1000);
            //    //Power01_OUTPUT ON <== ON
            //    ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_3_REMOTE] = 1;
            //}
            //else if (result == DialogResult.No)
            //{
            //    //Power01_OUTPUT ON <== OFF
            //    ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_3_REMOTE] = 0;

            //    //System.Threading.Thread.Sleep(1000);

            //    ////Power02_INTERLOCK <== OFF
            //    //ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_INTERLOCK] = 0;
            //    ////Power02_PULSE ON <== OFF
            //    //ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_PULSE] = 0;
            //    ////Power02_OFF <== OFF
            //    //ebKernel.PlcKernel[ConstPlcDefine.PLC_DO_POWER_2_OFF] = 0;
            //}
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
                else if (obj.GetType().Equals(typeof(TurboPump)))
                {
                    ((TurboPump)obj).RefreshStatus();
                }
                else if (obj.GetType().Equals(typeof(Motor)))
                {
                    ((Motor)obj).RefreshStatus();
                }
                else if (obj.GetType().Equals(typeof(PolyCold)))
                {
                    ((PolyCold)obj).RefreshStatus();
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
                else if (obj.GetType().Equals(typeof(TurboPump)))
                {
                    ((TurboPump)obj)._EqBase = ebKernel;
                }
                else if (obj.GetType().Equals(typeof(StatusPictureBox)))
                {
                    ((StatusPictureBox)obj)._EqBase = ebKernel;
                }
                else if (obj.GetType().Equals(typeof(Motor)))
                {
                    ((Motor)obj)._EqBase = ebKernel;
                }
                else if (obj.GetType().Equals(typeof(PolyCold)))
                {
                    ((PolyCold)obj)._EqBase = ebKernel;
                }
                else if (obj.GetType().Equals(typeof(GroupBox)))
                {
                    initialComponents(obj);                    
                }
            }
        }

        private void frmOverview_Shown(object sender, EventArgs e)
        {
            timerStatus.Enabled = true;
        }
        public new void Show()
        {
            timerStatus.Enabled = true;
        }
    }
}
