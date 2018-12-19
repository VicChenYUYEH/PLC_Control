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
    public partial class StatusPictureBox : PictureBox
    {
        #region Properties
        public string _PlcDevice { get; set; }
        public bool _Reverse { get; set; }
        public bool _CurrentStatus { get; set; }
        public EqBase _EqBase { get; set; }
        #endregion

        Dictionary<bool, Bitmap> STATUS_IMAGE = new Dictionary<bool, Bitmap>();

        public StatusPictureBox()
        {
            InitializeComponent();

            _PlcDevice = "";
            _Reverse = false;
            _CurrentStatus = false;

            STATUS_IMAGE.Add(true, Properties.Resources.StatusOn);
            STATUS_IMAGE.Add(false, Properties.Resources.StatusOff);

            this.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Image = STATUS_IMAGE[false];
            //this.InitialImage = ComponentResourceManager.

            this.HandleCreated += StatusPictureBox_HandleCreated;
        }

        private void StatusPictureBox_HandleCreated(object sender, EventArgs e)
        {
            if (_PlcDevice.Trim() == "")
                this.BackColor = Color.Red;
            else
                this.BackColor = SystemColors.Control;
        }

        public void refreshStatus(bool m_Status)
        {
            UpdateStatus(m_Status);
        }

        public void refreshStatus(short m_Status)
        {
            bool status = m_Status > 0 ? true : false;

            UpdateStatus(status);
        }

        public void refreshStatus()
        {
            if (_PlcDevice.Trim() == "" || _EqBase == null ) return;

            bool status = _EqBase.PlcKernel[_PlcDevice] == 1 ? true : false;

            _CurrentStatus = status;

            if ((_CurrentStatus && !_Reverse) || (!_CurrentStatus && _Reverse))
            {
                this.Image = STATUS_IMAGE[true];
            }
            else
            {
                this.Image = STATUS_IMAGE[false];
            }
        }
        public void refreshStatus(EqBase _EqBase)
        {
            bool status = _EqBase.PlcKernel.IsConnect;

            this.Image = null;
            if (status)
            {
                this.Image = STATUS_IMAGE[true];
            }
            else
            {
                this.Image = STATUS_IMAGE[false];
            }
        }

        private void UpdateStatus(bool m_Status)
        {
            if (_CurrentStatus == m_Status || _PlcDevice.Trim() == "") return;
            if (!_EqBase.PlcKernel.IsConnect) m_Status = false;

            _CurrentStatus = m_Status;

            if ((_CurrentStatus && !_Reverse) || (!_CurrentStatus && _Reverse))
            {
                this.Image = STATUS_IMAGE[true];
            }
            else
            {
                this.Image = STATUS_IMAGE[false];
            }
        }
    }
}
