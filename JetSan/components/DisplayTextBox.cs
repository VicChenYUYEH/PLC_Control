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
        public short _Division { get; set; }
        public short _Multiplication { get; set; }
        public double _MaxLimit { get; set; }
        public double _MinLimit { get; set; }
        public bool _DoubleWord { get; set; }
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
            _DoubleWord = false;

            this.TextAlign = HorizontalAlignment.Center;
            this.BackColor = SystemColors.InfoText;
            this.ReadOnly = true;
            
        }      
        

        public void refreshData()
        {
            if (_PlcDevice.Trim() == "" || /*_CurrentStatus == m_Status || */_EqBase == null) return;

            //_CurrentStatus = m_Status;
            //int value = _EqBase.PlcKernel[_PlcDevice];

            //if (value == 0 || _Multiplication == 0 || _Division == 0)
            //    this.Text = "0";
            //else
            //    this.Text = (value * _Multiplication / _Division).ToString();

            int value = (_DoubleWord ? _EqBase.PlcKernel.getPlcDbValue(_PlcDevice) : _EqBase.PlcKernel[_PlcDevice]);
            if (value == 0 || _Multiplication == 0 || _Division == 0)
                this.Text = "0";
            else
                this.Text = Convert.ToInt32((value / (float)_Multiplication) * (float)_Division).ToString();

            if (double.Parse(this.Text) > _MaxLimit || double.Parse(this.Text) < _MinLimit)
            {
                this.ForeColor = Color.Red;
            }
            else
            {
                this.ForeColor = Color.Lime;
            }
            this.Update();
        }

        

    }
}
