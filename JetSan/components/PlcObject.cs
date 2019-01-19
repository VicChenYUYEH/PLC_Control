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
        public string _Limit { get; set; }
        public int _LimitSignal { get; set; }
        public bool _Reverse { get; set; }
        public bool _CurrentStatus { get; set; }
        public ImageSize _ImageSize { get; set; }
        public ObjectType _ObjectType { get; set; }
        public EqBase _EqBase { get; set; }
        #endregion

        Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>> OBJECT_ON_IMAGE = new Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>>();
        Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>> OBJECT_OFF_IMAGE = new Dictionary<ImageSize, Dictionary<ObjectType, Bitmap>>();

        ToolTip trackTip;
        private int iLastX;
        private int iLastY;

        public PlcObject()
        {
            InitializeComponent();

            _PlcDevice = "";
            _PlcDisplayOnDevice = "";
            _PlcDisplayOffDevice = "";
            _Limit = "";
            _LimitSignal = 0;
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

            this.HandleCreated += plcObject_HandleCreated;
            this.MouseDoubleClick += plcObject_MouseDoubleClick;
            this.MouseMove += plcObject_MouseMove;
            //this.MouseLeave += PlcObject_MouseLeave;
        }

        private void plcObject_MouseLeave(object sender, EventArgs e)
        {
            //trackTip.Hide(this);
            //bTipDisplay = false;
        }

        private void plcObject_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X != this.iLastX || e.Y != this.iLastY)
            {
                //if (bTipDisplay) return;
                trackTip.SetToolTip(this, "Device: " + _EqBase.pPlcKernel.GetPlcMap(_PlcDevice));
                //bTipDisplay = true;
                this.iLastX = e.X;
                this.iLastY = e.Y;
            }
        }

        private void plcObject_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_PlcDevice.Trim() == "" || _EqBase == null) return;

            if(_Limit != "")
            {
                if(_EqBase.pPlcKernel[_Limit] != _LimitSignal)
                {
                    string LimitMap =_EqBase.pPlcKernel.GetPlcMap(_Limit);
                    MessageBox.Show(LimitMap + " 狀態錯誤，無法切換 ", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            DlgSwitch dlg = new DlgSwitch(_CurrentStatus);
            dlg._PlcDevice = _EqBase.pPlcKernel.GetPlcMap(_PlcDevice);
            dlg._PlcDevice = (_EqBase.pPlcKernel[_PlcDevice] == 1) ? dlg._PlcDevice + " Opening" : dlg._PlcDevice;
            DialogResult result = dlg.ShowDialog();
            _EqBase.pPlcKernel[_PlcDevice] = (result == DialogResult.Yes) ? 1 : 0;
            _EqBase.flOperator.WriteLog(_PlcDevice + " DoubleClick : " + result);
            dlg.Dispose();
        }

        private void plcObject_HandleCreated(object sender, EventArgs e)
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

        public void RefreshStatus()
        {
            if (_PlcDisplayOnDevice.Trim() == "" || /*_CurrentStatus == m_Status || */_EqBase == null) return;
            
            bool status = _EqBase.pPlcKernel[_PlcDisplayOnDevice] == 1 ? true : false;

            if (_PlcDisplayOffDevice != null && _PlcDisplayOffDevice.Trim() != "")
            {
                if ((!status && _EqBase.pPlcKernel[_PlcDisplayOffDevice] == 0)
                    || (status && _EqBase.pPlcKernel[_PlcDisplayOffDevice] == 1))
                {
                    this.Image = this.Image == OBJECT_ON_IMAGE[_ImageSize][_ObjectType] ? this.Image = OBJECT_OFF_IMAGE[_ImageSize][_ObjectType] : OBJECT_ON_IMAGE[_ImageSize][_ObjectType];
                    
                    return;
                }
            }           
            _CurrentStatus = status;

            if ((_CurrentStatus && !_Reverse) || (!_CurrentStatus && _Reverse))
            {
                this.Image = OBJECT_ON_IMAGE[_ImageSize][_ObjectType];
            }
            else
            {
                this.Image = OBJECT_OFF_IMAGE[_ImageSize][_ObjectType];
            }
        }
        
    }
}
