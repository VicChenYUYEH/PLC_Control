using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Windows.Forms;

using HyTemplate.gui;

namespace HyTemplate.components
{
    public partial class PlcObject : PictureBox
    {
        public enum ImageSize
        {
            isSmall,
            isLarge
        }

        public enum ObjectType
        {
            otBP,
            otRP,
            otTP,
            otValve,
            otCathode,
            otMFC,
            otPower,
            otATM,
            otLVG,
            otHVG,
            otHeater
        }

        #region Properties
        public string _PlcDevice { get; set; }
        public string _PlcDisplayOnDevice { get; set; }
        public string _PlcDisplayOffDevice { get; set; }
        public bool _Reverse { get; set; }
        public bool _CurrentStatus { get; set; }
        public ImageSize _ImageSize { get; set; }
        public ObjectType _ObjectType { get; set; }
        public EqBase _EqBase { get; set; }
        #endregion

        Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>> OBJECT_ON_IMAGE = new Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>>();
        Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>> OBJECT_OFF_IMAGE = new Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>>();

        ToolTip trackTip;
        bool bTipDisplay = false;

        public PlcObject()
        {
            InitializeComponent();

            _PlcDevice = "";
            _PlcDisplayOnDevice = "";
            _PlcDisplayOffDevice = "";
            _Reverse = false;
            _CurrentStatus = false;
            _ImageSize = ImageSize.isSmall;
            _ObjectType = ObjectType.otBP;

            #region ON / Small
            Dictionary<ObjectType, Bitmap> on_small_image = new Dictionary<ObjectType, Bitmap>();
            on_small_image.Add(ObjectType.otBP, Properties.Resources.BP_On);
            on_small_image.Add(ObjectType.otRP, Properties.Resources.RP_On);
            on_small_image.Add(ObjectType.otTP, Properties.Resources.TP_Ready);
            on_small_image.Add(ObjectType.otValve, Properties.Resources.VALVE_On);
            on_small_image.Add(ObjectType.otCathode, Properties.Resources.CATH_On);
            on_small_image.Add(ObjectType.otMFC, Properties.Resources.MfcOn_U);
            on_small_image.Add(ObjectType.otPower, Properties.Resources.Power);
            on_small_image.Add(ObjectType.otATM, Properties.Resources.ATM_On);
            on_small_image.Add(ObjectType.otLVG, Properties.Resources.LVG_On);
            on_small_image.Add(ObjectType.otHVG, Properties.Resources.HVG_On);
            on_small_image.Add(ObjectType.otHeater, Properties.Resources.Heater_On);

            //Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>> on_small = new Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>>();
            OBJECT_ON_IMAGE.Add(ImageSize.isSmall, on_small_image);
            #endregion
            
            #region ON /Large
            Dictionary<ObjectType, Bitmap> on_large_image = new Dictionary<ObjectType, Bitmap>();
            on_large_image.Add(ObjectType.otBP, Properties.Resources.BP_On_L);
            on_large_image.Add(ObjectType.otRP, Properties.Resources.RP_On_L);
            on_large_image.Add(ObjectType.otTP, Properties.Resources.TP_Ready_L);
            on_large_image.Add(ObjectType.otValve, Properties.Resources.VALVE_On);
            on_large_image.Add(ObjectType.otCathode, Properties.Resources.Cath_Ready);
            on_large_image.Add(ObjectType.otMFC, Properties.Resources.MfcOn_U);
            on_large_image.Add(ObjectType.otPower, Properties.Resources.Power);
            on_large_image.Add(ObjectType.otATM, Properties.Resources.ATM_On);
            on_large_image.Add(ObjectType.otLVG, Properties.Resources.LVG_On);
            on_large_image.Add(ObjectType.otHVG, Properties.Resources.HVG_On);

            //Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>> on_large = new Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>>();
            OBJECT_ON_IMAGE.Add(ImageSize.isLarge, on_large_image);
            #endregion
            
            #region Off / Small
            Dictionary<ObjectType, Bitmap> off_small_image = new Dictionary<ObjectType, Bitmap>();
            off_small_image.Add(ObjectType.otBP, Properties.Resources.BP_Off);
            off_small_image.Add(ObjectType.otRP, Properties.Resources.RP_Off);
            off_small_image.Add(ObjectType.otTP, Properties.Resources.TP_Stop);
            off_small_image.Add(ObjectType.otValve, Properties.Resources.VALVE_Off);
            off_small_image.Add(ObjectType.otCathode, Properties.Resources.CATH_Off);
            off_small_image.Add(ObjectType.otMFC, Properties.Resources.MfcOff_U);
            off_small_image.Add(ObjectType.otPower, Properties.Resources.Power);
            off_small_image.Add(ObjectType.otATM, Properties.Resources.ATM_Off);
            off_small_image.Add(ObjectType.otLVG, Properties.Resources.LVG_Off);
            off_small_image.Add(ObjectType.otHVG, Properties.Resources.HVG_Off);
            off_small_image.Add(ObjectType.otHeater, Properties.Resources.Heater_Off);

            // Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>> off_small = new Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>>();
            OBJECT_OFF_IMAGE.Add(ImageSize.isSmall, off_small_image);
            #endregion

            #region ON /Large
            Dictionary<ObjectType, Bitmap> off_large_image = new Dictionary<ObjectType, Bitmap>();
            off_large_image.Add(ObjectType.otBP, Properties.Resources.BP_Off_L);
            off_large_image.Add(ObjectType.otRP, Properties.Resources.RP_Off_L);
            off_large_image.Add(ObjectType.otTP, Properties.Resources.TP_Stop_L);
            off_large_image.Add(ObjectType.otValve, Properties.Resources.VALVE_Off);
            off_large_image.Add(ObjectType.otCathode, Properties.Resources.Cath_Stop);
            off_large_image.Add(ObjectType.otMFC, Properties.Resources.MfcOff_U);
            off_large_image.Add(ObjectType.otPower, Properties.Resources.Power);
            off_large_image.Add(ObjectType.otATM, Properties.Resources.ATM_Off);
            off_large_image.Add(ObjectType.otLVG, Properties.Resources.LVG_Off);
            off_large_image.Add(ObjectType.otHVG, Properties.Resources.HVG_Off);

            //Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>> off_large = new Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>>();
            OBJECT_OFF_IMAGE.Add(ImageSize.isLarge, off_large_image);
            #endregion
            

            this.SizeMode = PictureBoxSizeMode.StretchImage;
            //this.Image = STATUS_IMAGE[false];
            //this.InitialImage = ComponentResourceManager.

            trackTip = new ToolTip();

            this.HandleCreated += PlcObject_HandleCreated;
            this.MouseDoubleClick += PlcObject_MouseDoubleClick;
            this.MouseMove += PlcObject_MouseMove;
            this.MouseLeave += PlcObject_MouseLeave;
        }

        private void PlcObject_MouseLeave(object sender, EventArgs e)
        {
            trackTip.Hide(this);
            bTipDisplay = false;
        }

        private void PlcObject_MouseMove(object sender, MouseEventArgs e)
        {
            //String tipText = String.Format("({0}, {1})", e.X, e.Y);
            if (bTipDisplay) return;
            trackTip.Show("Device: " + _PlcDevice, this, e.Location);
            bTipDisplay = true;
        }

        private void PlcObject_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_PlcDevice.Trim() == "" || _EqBase == null) return;

            dlgSwitch dlg = new dlgSwitch(_CurrentStatus);
            dlg.PlcDevice = _PlcDevice;
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.Yes)
            {
                _EqBase.PlcKernel[_PlcDevice] = 1;
            }
            else if (result == DialogResult.No)
            {
                _EqBase.PlcKernel[_PlcDevice] = 0;
            }
            dlg.Dispose();
        }

        private void PlcObject_HandleCreated(object sender, EventArgs e)
        {
            if (_CurrentStatus)
            {
                this.Image = OBJECT_ON_IMAGE[_ImageSize][_ObjectType];
            }
            else
            {
                this.Image = OBJECT_OFF_IMAGE[_ImageSize][_ObjectType];
            }

            if (_PlcDisplayOnDevice.Trim() == "")
                _PlcDisplayOnDevice = _PlcDevice;
        }

        public void refreshStatus()
        {
            if (_PlcDisplayOnDevice.Trim() == "" || /*_CurrentStatus == m_Status || */_EqBase == null) return;

            //_CurrentStatus = m_Status;
            bool status = _EqBase.PlcKernel[_PlcDisplayOnDevice] == 1 ? true : false;

            if (_PlcDisplayOffDevice != null && _PlcDisplayOffDevice.Trim() != "")
            {
                if ((!status && _EqBase.PlcKernel[_PlcDisplayOffDevice] == 0)
                    || (status && _EqBase.PlcKernel[_PlcDisplayOffDevice] == 1))
                {
                    this.Image = this.Image == OBJECT_ON_IMAGE[_ImageSize][_ObjectType] ? this.Image = OBJECT_OFF_IMAGE[_ImageSize][_ObjectType] : OBJECT_ON_IMAGE[_ImageSize][_ObjectType];
                    
                    return;
                }
            }

            //if (status == _CurrentStatus) return;

            _CurrentStatus = status;

            if ((_CurrentStatus && !_Reverse) || (!_CurrentStatus && _Reverse))
            {
                this.Image = OBJECT_ON_IMAGE[_ImageSize][_ObjectType];
            }
            else
            {
                this.Image = OBJECT_OFF_IMAGE[_ImageSize][_ObjectType];
            }

            //if (this.BackColor == Color.Red)
            //{
            //    this.BackColor = SystemColors.Control;
            //}
        }
        
    }
}
