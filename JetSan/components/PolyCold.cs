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

    public partial class PolyCold : PictureBox
    {
        public enum ImageSize
        {
            isSmall,
            isLarge
        }

        public enum PolycoldStatus
        {
            tsRun,
            tsOff,
            tsAlarm,
            tsCooling,
            tsDefrosting
        }

        #region Properties
        public string _PlcRunDevice { get; set; }
        public string _PlcAlarmDevice { get; set; }
        public string _PlcCoolingDevice { get; set; }
        public string _PlcDefrostingDevice { get; set; }
        
        public bool _Reverse { get; set; }
        public ImageSize _ImageSize { get; set; }
        public EqBase _EqBase { get; set; }

        public bool ReadyToStart { get; set; }
        #endregion
        ToolTip trackTip;
        private int lastX;
        private int lastY;
        PolycoldStatus tsCurrentStatus;

        Dictionary<ImageSize, Dictionary<PolycoldStatus, Bitmap>> OBJECT_IMAGE = new Dictionary<ImageSize, Dictionary<PolycoldStatus, Bitmap>>();

        public PolyCold()
        {
            InitializeComponent();

            _PlcRunDevice = "";
            _PlcAlarmDevice = "";
            _PlcCoolingDevice = "";
            _PlcDefrostingDevice = "";
             _Reverse = false;
            tsCurrentStatus = PolycoldStatus.tsOff;
            _ImageSize = ImageSize.isSmall;

            ReadyToStart = true;

            #region Small
            Dictionary<PolycoldStatus, Bitmap> on_small_image = new Dictionary<PolycoldStatus, Bitmap>();
            on_small_image.Add(PolycoldStatus.tsRun, Properties.Resources.Polycold_Run);
            on_small_image.Add(PolycoldStatus.tsOff, Properties.Resources.Polycold_Off);
            on_small_image.Add(PolycoldStatus.tsAlarm, Properties.Resources.Polycold_Alarm);
            on_small_image.Add(PolycoldStatus.tsCooling, Properties.Resources.Polycold_Cooling);
            on_small_image.Add(PolycoldStatus.tsDefrosting, Properties.Resources.Polycold_Defrosting);

            //Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>> on_small = new Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>>();
            OBJECT_IMAGE.Add(ImageSize.isSmall, on_small_image);
            #endregion

            #region Large
            Dictionary<PolycoldStatus, Bitmap> on_large_image = new Dictionary<PolycoldStatus, Bitmap>();
            on_large_image.Add(PolycoldStatus.tsRun, Properties.Resources.Polycold_Run);
            on_large_image.Add(PolycoldStatus.tsOff, Properties.Resources.Polycold_Off);
            on_large_image.Add(PolycoldStatus.tsAlarm, Properties.Resources.Polycold_Alarm);
            on_large_image.Add(PolycoldStatus.tsCooling, Properties.Resources.Polycold_Cooling);
            on_large_image.Add(PolycoldStatus.tsDefrosting, Properties.Resources.Polycold_Defrosting);

            //Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>> on_large = new Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>>();
            OBJECT_IMAGE.Add(ImageSize.isLarge, on_large_image);
            #endregion

            this.SizeMode = PictureBoxSizeMode.StretchImage;

            trackTip = new ToolTip();

            this.HandleCreated += PolyCold_HandleCreated;
            this.MouseDoubleClick += PolyCold_MouseDoubleClick;
            this.MouseMove += PolyCold_MouseMove;
        }
        

        private void PolyCold_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X != this.lastX || e.Y != this.lastY)
            {
                trackTip.SetToolTip(this, "Device: " + _EqBase.PlcKernel.getPlcMap(_PlcRunDevice));
                this.lastX = e.X;
                this.lastY = e.Y;
            }
        }

        private void PolyCold_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_PlcRunDevice.Trim() == "" || _EqBase == null || !ReadyToStart) return;

            dlgSwitch dlg = new dlgSwitch((_EqBase.PlcKernel[_PlcRunDevice] == 1 ? true : false));
            dlg.PlcDevice = _EqBase.PlcKernel.getPlcMap(_PlcRunDevice);
            dlg.PlcDevice = (_EqBase.PlcKernel[_PlcRunDevice] == 1) ? dlg.PlcDevice + " Opening" : dlg.PlcDevice;
            DialogResult result = dlg.ShowDialog();
            _EqBase.PlcKernel[_PlcRunDevice] = (result == DialogResult.Yes) ? 1 : 0;
            dlg.Dispose();
        }

        private void PolyCold_HandleCreated(object sender, EventArgs e)
        {
            this.Image = OBJECT_IMAGE[_ImageSize][PolycoldStatus.tsOff];
        }

        public void refreshStatus()
        {
            if (_EqBase == null) return;
           
            PolycoldStatus status = PolycoldStatus.tsOff;

            if (_PlcAlarmDevice.Trim() != "" && _EqBase.PlcKernel[_PlcAlarmDevice] == 1)
            {
                status = PolycoldStatus.tsAlarm;
            }
            else if (_PlcCoolingDevice.Trim() != "" && _EqBase.PlcKernel[_PlcCoolingDevice] == 1)
            {
                status = PolycoldStatus.tsCooling;
            }
            else if (_PlcDefrostingDevice.Trim() != "" && _EqBase.PlcKernel[_PlcDefrostingDevice] == 1)
            {
                status = PolycoldStatus.tsDefrosting;
            }
            else if (_PlcRunDevice.Trim() != "" && _EqBase.PlcKernel[_PlcRunDevice] == 1)
            {
                status = PolycoldStatus.tsRun;
            }


            if (status == tsCurrentStatus) return;

            tsCurrentStatus = status;

            this.Image = OBJECT_IMAGE[_ImageSize][tsCurrentStatus];
        }
    }
}
