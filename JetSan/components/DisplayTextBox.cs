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
    public partial class DisplayTextBox : TextBox
    {
        #region Properties
        public string _PlcDevice { get; set; }
        public double _Division { get; set; }
        public double _Multiplication { get; set; }
        public double _MaxLimit { get; set; }
        public double _MinLimit { get; set; }
        public bool _DoubleWord { get; set; }
        public bool _ITR { get; set; }
        public bool _TTR { get; set; }
        public bool _Float { get; set; }
        public int _LimitSignal { get; set; }
        public string _Limit { get; set; }
        public EqBase _EqBase { get; set; }

        #endregion

        public DisplayTextBox()
        {
            InitializeComponent();

            _PlcDevice = "";
            _Division = 1;
            _Multiplication = 1;
            _MaxLimit = 999;
            _MinLimit = 0;
            _LimitSignal = 0;
            _Limit = "";
            _Float = false;
            _DoubleWord = false;
            _ITR = false;
            _TTR = false;

            this.TextAlign = HorizontalAlignment.Center;
            this.BackColor = SystemColors.InfoText;
            this.ReadOnly = true;
            
        }      
        

        public void RefreshData()
        {
            if (_PlcDevice.Trim() == "" || /*_CurrentStatus == m_Status || */_EqBase == null) return;

            //_CurrentStatus = m_Status;
            //int value = _EqBase.PlcKernel[_PlcDevice];

            //if (value == 0 || _Multiplication == 0 || _Division == 0)
            //    this.Text = "0";
            //else
            //    this.Text = (value * _Multiplication / _Division).ToString();

            int value = (_DoubleWord ? _EqBase.pPlcKernel.GetPlcDbValue(_PlcDevice) : _EqBase.pPlcKernel[_PlcDevice]);

            if (_Limit != "")
            {
                this.Text = (_EqBase.pPlcKernel[_Limit] != _LimitSignal) ? "N/A" : "";
                this.ForeColor = Color.Lime;
            }
            if(this.Text != "N/A")
            {
                if (_ITR)
                {
                    this.Text = ConvertFormat.GetITR(value);
                }
                else if (_TTR)
                {
                    this.Text = ConvertFormat.GetTTR(value);
                }
                else
                {
                    if (value == 0 || _Multiplication == 0 || _Division == 0)
                        this.Text = "0";
                    else
                    {
                        if (_Float)
                            this.Text = Convert.ToDouble((value * (float)_Multiplication) / (float)_Division).ToString("0.0");
                        else this.Text = Convert.ToInt32((value * (float)_Multiplication) / (float)_Division).ToString();
                    }
                }
                this.ForeColor = (double.Parse(this.Text) > _MaxLimit || double.Parse(this.Text) < _MinLimit) ? Color.Red : Color.Lime;
            }
            this.Update();
        }
    }
}
