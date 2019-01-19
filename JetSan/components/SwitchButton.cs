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
    public partial class SwitchButton : Button
    {
        #region Properties
        public string _PlcDevice { get; set; }
        public string _PlcDisplayDevice { get; set; }
        public bool _Reverse { get; set; }
        public bool _CurrentStatus { get; set; }
        public EqBase _EqBase { get; set; }
        #endregion

        public SwitchButton()
        {
            InitializeComponent();

            _PlcDevice = "";
            _PlcDisplayDevice = "";
            _Reverse = false;
            _CurrentStatus = false;

            this.HandleCreated += switchButton_HandleCreated;
            this.MouseClick += switchButton_MouseClick;
        }

        private void switchButton_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (_PlcDevice.Trim() == "" || _EqBase == null) return;

                _EqBase.pPlcKernel[_PlcDevice] = (short)(_EqBase.pPlcKernel[_PlcDevice] == 1 ? 0 : 1);

                _EqBase.flOperator.WriteLog(_PlcDevice, "Click");
            }
            catch (Exception ex)
            {
                _EqBase.flDebug.WriteLog(_PlcDevice, ex.ToString());
            }
        }

        private void switchButton_HandleCreated(object sender, EventArgs e)
        {
            if (_PlcDisplayDevice.Trim() == "")
                _PlcDisplayDevice = _PlcDevice;

            if (_PlcDisplayDevice.Trim() == "")
                this.BackColor = Color.Red;
        }

        public void RefreshStatus()
        {
            if (_PlcDisplayDevice.Trim() == "" || _EqBase == null) return;

            //_CurrentStatus = m_Status;
            bool status = _EqBase.pPlcKernel[_PlcDisplayDevice] == 1 ? true : false;

            _CurrentStatus = status;
            if ((status && !_Reverse) || (!status && _Reverse))
            {
                this.BackColor = Color.Lime;
            }
            else
            {
                this.BackColor = Color.GhostWhite;
            }
        }
    }
}
