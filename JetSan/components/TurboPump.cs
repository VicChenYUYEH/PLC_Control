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

    public partial class TurboPump : PictureBox
    {
        public enum ImageSize
        {
            isSmall,
            isLarge
        }

        public enum TurboStatus
        {
            tsStop,
            tsAcc,
            tsDec,
            tsReady,
            tsAlarm
        }

        #region Properties
        public string _PlcStartDevice { get; set; }
        public string _PlcStopDevice { get; set; }
        public string _PlcReadyDevice { get; set; }
        public string _PlcAccDevice { get; set; }
        public string _PlcDecDevice { get; set; }
        public string _PlcAlarmDevice { get; set; }
        public bool _Reverse { get; set; }
        public ImageSize _ImageSize { get; set; }
        public EqBase _EqBase { get; set; }

        public bool ReadyToStart { get; set; }
        #endregion

        ToolTip trackTip;
        bool bTipDisplay = false;
        TurboStatus tsCurrentStatus;

        Dictionary<ImageSize, Dictionary<TurboStatus, Bitmap>> OBJECT_IMAGE = new Dictionary<ImageSize, Dictionary<TurboStatus, Bitmap>>();

        public TurboPump()
        {
            InitializeComponent();

            _PlcStartDevice = "";
            _PlcStopDevice = "";
            _PlcReadyDevice = "";
            _PlcAccDevice = "";
            _PlcDecDevice = "";
            _PlcAlarmDevice = "";
            _Reverse = false;
            tsCurrentStatus = TurboStatus.tsStop;
            _ImageSize = ImageSize.isSmall;

            ReadyToStart = true;

            #region Small
            Dictionary<TurboStatus, Bitmap> on_small_image = new Dictionary<TurboStatus, Bitmap>();
            on_small_image.Add(TurboStatus.tsStop, Properties.Resources.TP_Stop);
            on_small_image.Add(TurboStatus.tsAcc, Properties.Resources.TP_SpeedUp);
            on_small_image.Add(TurboStatus.tsDec, Properties.Resources.TP_SlowDown);
            on_small_image.Add(TurboStatus.tsReady, Properties.Resources.TP_Ready);
            on_small_image.Add(TurboStatus.tsAlarm, Properties.Resources.TP_Alarm);

            //Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>> on_small = new Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>>();
            OBJECT_IMAGE.Add(ImageSize.isSmall, on_small_image);
            #endregion

            #region Large
            Dictionary<TurboStatus, Bitmap> on_large_image = new Dictionary<TurboStatus, Bitmap>();
            on_large_image.Add(TurboStatus.tsStop, Properties.Resources.TP_Stop_L);
            on_large_image.Add(TurboStatus.tsAcc, Properties.Resources.TP_SpeedUp_L);
            on_large_image.Add(TurboStatus.tsDec, Properties.Resources.TP_SlowDown_L);
            on_large_image.Add(TurboStatus.tsReady, Properties.Resources.TP_Ready_L);
            on_large_image.Add(TurboStatus.tsAlarm, Properties.Resources.TP_Alarm_L);

            //Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>> on_large = new Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>>();
            OBJECT_IMAGE.Add(ImageSize.isLarge, on_large_image);
            #endregion

            this.SizeMode = PictureBoxSizeMode.StretchImage;

            trackTip = new ToolTip();

            this.HandleCreated += TurboPump_HandleCreated;
            this.MouseDoubleClick += TurboPump_MouseDoubleClick;
            this.MouseMove += TurboPump_MouseMove;
            this.MouseLeave += TurboPump_MouseLeave;
        }

        private void TurboPump_MouseLeave(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            trackTip.Hide(this);
            bTipDisplay = false;
        }

        private void TurboPump_MouseMove(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            if (bTipDisplay) return;
            trackTip.Show("Device: " + _PlcStartDevice, this, e.Location);
            bTipDisplay = true;
        }

        private void TurboPump_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
            if (_PlcStartDevice.Trim() == "" || _EqBase == null || !ReadyToStart) return;

            //dlgSwitch dlg = new dlgSwitch((_EqBase.PlcKernel[_PlcStartDevice] ==1?true:false));
            //dlg.PlcDevice = _PlcStartDevice;
            //DialogResult result = dlg.ShowDialog();
            //if (result == DialogResult.Yes)
            //{
            //    _EqBase.PlcKernel[_PlcStopDevice] = 0;
            //    _EqBase.PlcKernel[_PlcStartDevice] = 1;
            //}
            //else if (result == DialogResult.No)
            //{
            //    if (_PlcStopDevice.Trim() == "")
            //    {
            //        _EqBase.PlcKernel[_PlcStartDevice] = 0;
            //    }
            //    else
            //    {
            //        _EqBase.PlcKernel[_PlcStartDevice] = 0;
            //        _EqBase.PlcKernel[_PlcStopDevice] = 1;
            //    }
                
            //}
            //dlg.Dispose();
        }

        private void TurboPump_HandleCreated(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            this.Image = OBJECT_IMAGE[_ImageSize][TurboStatus.tsStop];
        }

        public void refreshStatus()
        {
            if (_PlcReadyDevice.Trim() == "" || _EqBase == null) return;

            
            TurboStatus status = TurboStatus.tsStop;// _EqBase.PlcKernel[_PlcReadyDevice] == 1 ? true : false;

            if (_PlcAlarmDevice.Trim() != "" && _EqBase.PlcKernel[_PlcAlarmDevice] == 1)
            {
                status = TurboStatus.tsAlarm;
            }
            else if (_PlcDecDevice.Trim() != "" && _EqBase.PlcKernel[_PlcDecDevice] == 1)
            {
                status = TurboStatus.tsDec;
            }
            else if (_PlcAccDevice.Trim() != "" && _EqBase.PlcKernel[_PlcReadyDevice] == 1)
            {
                status = TurboStatus.tsReady;
            }
            else if (_PlcAccDevice.Trim() != "" && _EqBase.PlcKernel[_PlcAccDevice] == 1)
            {
                status = TurboStatus.tsAcc;
            }
            
            

            if (status == tsCurrentStatus) return;

            tsCurrentStatus = status;

            this.Image = OBJECT_IMAGE[_ImageSize][tsCurrentStatus];
        }
    }
}
