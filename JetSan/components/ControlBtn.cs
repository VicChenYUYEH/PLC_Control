using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HyTemplate.components
{
    public partial class ControlBtn : Button
    {
        #region Properties
        public string _PlcDevice { get; set; }
        public string _PlcDisplayOnDevice { get; set; }
        public string _PlcDisplayOffDevice { get; set; }
        public bool _Reverse { get; set; }
        public bool _ReadOnly { get; set; }
        public string _Text { get; set; }
        public bool _ShowMsg { get; set; }
        public bool _AutoOff { get; set; }
        public EqBase _EqBase { get; set; }
        public bool _CurrentStatus { get; set; }
        #endregion

        public ControlBtn()
        {
            InitializeComponent();

            _PlcDevice = "";
            _Text = "";
            _ShowMsg = false;
            _AutoOff = false;
             _PlcDisplayOnDevice = "";
            _PlcDisplayOffDevice = "";
            _ReadOnly = false;
            _Reverse = false;
            _CurrentStatus = false;

            this.HandleCreated += controlBtn_HandleCreated;
            this.Click += btn_ClickAsync;

        }
        private void controlBtn_HandleCreated(object sender, EventArgs e)
        {
            this.Text = _Text;
            this.BackColor = (_ReadOnly)? Color.WhiteSmoke : Color.Transparent;
            this.Enabled = (_ReadOnly) ? false : true;
        }        

        private async void btn_ClickAsync(object sender, EventArgs e)
        {
            try
            {
                if (_PlcDevice.Trim() == "" || _EqBase == null) return;

                if (_ShowMsg)
                {
                    DialogResult result = MessageBox.Show("    是    否    繼    續   ?", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result != DialogResult.Yes) return;
                }

                _EqBase.flOperator.WriteLog(_PlcDevice, "ControlButton_Click");
                if (_AutoOff)
                {
                    _EqBase.pPlcKernel[_PlcDevice] = 1;
                    await putTaskDelay();
                    _EqBase.pPlcKernel[_PlcDevice] = 0;
                    return;
                }
                _EqBase.pPlcKernel[_PlcDevice] = (_CurrentStatus) ? 0 : 1;
            }
            catch (Exception ex)
            {
                _EqBase.flDebug.WriteLog(_PlcDevice, ex.ToString());
            }
        }

        public void RefreshStatus()
        {
            if (_PlcDisplayOnDevice.Trim() == "" || _EqBase == null) return;
            
            bool status = _EqBase.pPlcKernel[_PlcDisplayOnDevice] == 1 ? true : false;

            if (_PlcDisplayOffDevice != null && _PlcDisplayOffDevice.Trim() != "")
            {
                if ((!status && _EqBase.pPlcKernel[_PlcDisplayOffDevice] == 0)
                    || (status && _EqBase.pPlcKernel[_PlcDisplayOffDevice] == 1))
                {
                    this.BackColor = (this.BackColor == Color.Green) ? Color.WhiteSmoke : Color.Green;

                    return;
                }
            }
            
            _CurrentStatus = status;

            if ((_CurrentStatus && !_Reverse) || (!_CurrentStatus && _Reverse))
            {
                this.BackColor = Color.Green;
            }
            else
            {
                this.BackColor = (_ReadOnly) ? Color.WhiteSmoke : Color.Transparent;
            }
        }
        async Task putTaskDelay()
        {
            await Task.Delay(1000);
        }
    }
            
}
