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

        public bool bReadyToStart { get; set; }
        #endregion

        ToolTip trackTip;
        private int iLastX;
        private int iLastY;
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

            bReadyToStart = true;

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

            this.HandleCreated += turboPump_HandleCreated;
            this.MouseDoubleClick += turboPump_MouseDoubleClick;
            this.MouseMove += turboPump_MouseMove;
        }
        

        private void turboPump_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X != this.iLastX || e.Y != this.iLastY)
            {
                trackTip.SetToolTip(this, "Device: " + _EqBase.pPlcKernel.GetPlcMap(_PlcStartDevice));
                this.iLastX = e.X;
                this.iLastY = e.Y;
            }
        }

        private void turboPump_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (_PlcStartDevice.Trim() == "" || _EqBase == null || !bReadyToStart) return;

                DlgSwitch dlg = new DlgSwitch((_EqBase.pPlcKernel[_PlcStartDevice] == 1 ? true : false));
                dlg._PlcDevice = _EqBase.pPlcKernel.GetPlcMap(_PlcStartDevice);
                dlg._PlcDevice = (_EqBase.pPlcKernel[_PlcStartDevice] == 1) ? dlg._PlcDevice + " Opening" : dlg._PlcDevice;
                DialogResult result = dlg.ShowDialog();
                _EqBase.pPlcKernel[_PlcStartDevice] = (result == DialogResult.Yes) ? 1 : 0;
                _EqBase.flOperator.WriteLog(_PlcStartDevice, "DoubleClick " + result);
                dlg.Dispose();
            }
            catch (Exception ex)
            {
                _EqBase.flDebug.WriteLog(_PlcStartDevice, ex.ToString());
            }
        }

        private void turboPump_HandleCreated(object sender, EventArgs e)
        {
            this.Image = OBJECT_IMAGE[_ImageSize][TurboStatus.tsStop];
        }

        public void RefreshStatus()
        {
            if (_PlcReadyDevice.Trim() == "" || _EqBase == null) return;
           
            TurboStatus status = TurboStatus.tsStop;

            if (_PlcAlarmDevice.Trim() != "" && _EqBase.pPlcKernel[_PlcAlarmDevice] == 1)
            {
                status = TurboStatus.tsAlarm;
            }
            else if (_PlcDecDevice.Trim() != "" && _EqBase.pPlcKernel[_PlcDecDevice] == 1)
            {
                status = TurboStatus.tsDec;
            }
            else if (_PlcReadyDevice.Trim() != "" && _EqBase.pPlcKernel[_PlcReadyDevice] == 1)
            {
                status = TurboStatus.tsReady;
            }
            else if (_PlcAccDevice.Trim() != "" && _EqBase.pPlcKernel[_PlcAccDevice] == 1)
            {
                status = TurboStatus.tsAcc;
            }
                     
            if (status == tsCurrentStatus) return;

            tsCurrentStatus = status;

            this.Image = OBJECT_IMAGE[_ImageSize][tsCurrentStatus];
        }
    }
}
