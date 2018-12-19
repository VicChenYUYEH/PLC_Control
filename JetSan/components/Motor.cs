using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HyTemplate.gui;

namespace HyTemplate.components
{
    public partial class Motor : UserControl
    {
        #region Properties
        public string _PlcForwardDevice { get; set; }
        public string _PlcBackDevice { get; set; }
        public string _PlcJogForwardDevice { get; set; }
        public string _PlcJogBackDevice { get; set; }
        public string _PlcSpeedDevice { get; set; }
        public int _MotorMaxRpm { get; set; }
        public int _MotorMaxPlcValue { get; set; }
        public int _CurrentStatus { get; set; }
        public int _Division { get; set; }
        public int _Multiplication { get; set; }
        public EqBase _EqBase { get; set; }
        #endregion

        Dictionary<bool, Bitmap> STATUS_IMAGE = new Dictionary<bool, Bitmap>();
        Dictionary<bool, Bitmap> STATUS_FORWARD_IMAGE = new Dictionary<bool, Bitmap>();
        Dictionary<bool, Bitmap> STATUS_BACK_IMAGE = new Dictionary<bool, Bitmap>();

        private int iDisplayIndex=0;

        public Motor()
        {
            InitializeComponent();

            _PlcForwardDevice = "";
            _PlcBackDevice = "";
            _CurrentStatus = 0;
            _Division = 1;
            _Multiplication = 1;


            STATUS_IMAGE.Add(true, Properties.Resources.Moto_Run);
            STATUS_IMAGE.Add(false, Properties.Resources.Moto_Stop);

            STATUS_FORWARD_IMAGE.Add(true, Properties.Resources.Moto_Act_Right);
            STATUS_FORWARD_IMAGE.Add(false, Properties.Resources.Moto_Stop_Right);

            STATUS_BACK_IMAGE.Add(true, Properties.Resources.Moto_Act_Left);
            STATUS_BACK_IMAGE.Add(false, Properties.Resources.Moto_Stop_Left);

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            this.HandleCreated += Motor_HandleCreated;
        }

        
        private void Motor_HandleCreated(object sender, EventArgs e)
        {
            //pictureBox1.Image = STATUS_IMAGE[false];
            //pictureBox2.Image = STATUS_BACK_IMAGE[false];
            //pictureBox3.Image = STATUS_FORWARD_IMAGE[false];
        }

        public void refreshStatus()
        {
            if (_PlcForwardDevice.Trim() == "" || _PlcBackDevice.Trim() == "" || _EqBase == null) return;

            //_CurrentStatus = m_Status;
            int status = 0;

            if (_EqBase.PlcKernel[_PlcForwardDevice] == 1)
            {
                status = 1;
            }
            else if (_EqBase.PlcKernel[_PlcBackDevice] == 1)
            {
                status = 2;
            }

            if (_CurrentStatus == 1)
            {
                if (iDisplayIndex == 0)
                {
                    pictureBox4.Image = STATUS_FORWARD_IMAGE[false];
                    pictureBox5.Image = STATUS_FORWARD_IMAGE[true];
                }
                else
                {
                    pictureBox4.Image = STATUS_FORWARD_IMAGE[true];
                    pictureBox5.Image = STATUS_FORWARD_IMAGE[false];
                }
                pictureBox2.Image = STATUS_BACK_IMAGE[false];
                pictureBox3.Image = STATUS_BACK_IMAGE[false];
            }
            else if (_CurrentStatus == 2)
            {
                if (iDisplayIndex == 0)
                {
                    pictureBox2.Image = STATUS_BACK_IMAGE[false];
                    pictureBox3.Image = STATUS_BACK_IMAGE[true];
                }
                else
                {
                    pictureBox2.Image = STATUS_BACK_IMAGE[true];
                    pictureBox3.Image = STATUS_BACK_IMAGE[false];
                }
                pictureBox4.Image = STATUS_FORWARD_IMAGE[false];
                pictureBox5.Image = STATUS_FORWARD_IMAGE[false];
            }

            iDisplayIndex = (iDisplayIndex == 1 ? 0 : 1);

            if (status == _CurrentStatus) return;

            _CurrentStatus = status;

            if (_CurrentStatus > 0)
            {
                pictureBox1.Image = STATUS_IMAGE[true];                
            }
            else
            {
                pictureBox1.Image = STATUS_IMAGE[false];

                pictureBox2.Image = STATUS_BACK_IMAGE[false];
                pictureBox3.Image = STATUS_BACK_IMAGE[false];

                pictureBox4.Image = STATUS_FORWARD_IMAGE[false];
                pictureBox5.Image = STATUS_FORWARD_IMAGE[false];
            }
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_PlcForwardDevice.Trim() == "" || _PlcBackDevice.Trim() == "" || _EqBase == null) return;

            int status = 0;
            if (_EqBase.PlcKernel[_PlcForwardDevice] == 1)
            {
                status = 1;
            }
            else if (_EqBase.PlcKernel[_PlcBackDevice] == 1)
            {
                status = 2;
            }

            dlgMotorSwitch dlg = new dlgMotorSwitch(status, _EqBase);
            dlg.jogForwardDevice = _PlcForwardDevice;
            dlg.jogBackDevice = _PlcBackDevice;
            dlg.speedDevice = _PlcSpeedDevice;
            dlg.maxMotorRpm = _MotorMaxRpm;
            dlg.maxMotorPlcValue = _MotorMaxPlcValue;
            dlg._Division = _Division;
            dlg._Multiplication = _Multiplication;

            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.Yes)
            {
                _EqBase.PlcKernel[_PlcBackDevice] = 0;
                _EqBase.PlcKernel[_PlcForwardDevice] = 1;
            }
            else if (result == DialogResult.No)
            {
                _EqBase.PlcKernel[_PlcForwardDevice] = 0;
                _EqBase.PlcKernel[_PlcBackDevice] = 1;
            }
            else if (result == DialogResult.Abort)
            {
                _EqBase.PlcKernel[_PlcForwardDevice] = 0;
                _EqBase.PlcKernel[_PlcBackDevice] = 0;
            }
        }
    }
}
