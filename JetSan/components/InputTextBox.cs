﻿using System;
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
    public partial class InputTextBox : TextBox
    {
        #region Properties
        public string _PlcDevice { get; set; }
        public int _Division { get; set; }
        public int _Multiplication { get; set; }
        public bool _NumberOnly { get; set; }
        public bool _FloatNumber { get; set; }
        public double _MaxLimit { get; set; }
        public double _MinLimit { get; set; }
        public EqBase _EqBase { get; set; }
        #endregion
        private bool bSetting = false;

        public InputTextBox()
        {
            InitializeComponent();

            _PlcDevice = "";
            _Division = 1;
            _Multiplication = 1;
            _NumberOnly = true;
            _FloatNumber = false;
            _MaxLimit = 999;
            _MinLimit = 0;

            this.HandleCreated += inputTextBox_HandleCreated;
            this.Enter += inputTextBox_Enter;
            this.Leave += inputTextBox_Leave;
            this.KeyUp += inputTextBox_KeyUp;

            this.TextAlign = HorizontalAlignment.Center;

        }

        private void inputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)48 || e.KeyChar == (Char)49 ||
               e.KeyChar == (Char)50 || e.KeyChar == (Char)51 ||
               e.KeyChar == (Char)52 || e.KeyChar == (Char)53 ||
               e.KeyChar == (Char)54 || e.KeyChar == (Char)55 ||
               e.KeyChar == (Char)56 || e.KeyChar == (Char)57 ||
               /*e.KeyChar == (Char)13 || */e.KeyChar == (Char)8)
            {
                //if (Convert.ToDouble(this.Text + e.KeyChar) > _MaxLimit || Convert.ToDouble(this.Text + e.KeyChar) < _MinLimit)
                //{
                //    e.Handled = true;
                //    return;
                //}
                //e.Handled = false;
            }
            else
            {
                if (_FloatNumber && e.KeyChar == (Char)46 && this.Text.IndexOf('.')==-1)
                {
                    e.Handled = false;
                    return;
                }
                e.Handled = true;
            }
        }

        private void inputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    double result = 0;
                    bool check_text = double.TryParse(this.Text, out result);

                    if (_EqBase == null || !check_text) return;

                    if (result > _MaxLimit) result = _MaxLimit;
                    if (result < _MinLimit) result = _MinLimit;

                    if (_Multiplication <= 0) _Multiplication = 1;
                    if (_Division <= 0) _Division = 1;

                    result = result / _Multiplication * _Division;
                    _EqBase.pPlcKernel[_PlcDevice] = (int)result;

                    _EqBase.flOperator.WriteLog(_PlcDevice, "InputTextBox_KeyEnter : " + result);

                    this.Parent.Focus();
                    inputTextBox_Leave(sender, e);
                }
            }
            catch (Exception ex)
            {
                _EqBase.flDebug.WriteLog(_PlcDevice, ex.ToString());
            }
        }

        private void inputTextBox_HandleCreated(object sender, EventArgs e)
        {
            if (this.ReadOnly)
                this.BackColor = Color.CornflowerBlue;
            else
                this.BackColor = Color.White;

            if (_NumberOnly)
                this.KeyPress += inputTextBox_KeyPress;
        }

        private void inputTextBox_Leave(object sender, EventArgs e)
        {
            if (this.ReadOnly) return;

            this.BackColor = Color.White;
            bSetting = false;
        }

        private void inputTextBox_Enter(object sender, EventArgs e)
        {
            if (this.ReadOnly) return;

            bSetting = true;
            this.BackColor = Color.FromArgb(255, 255, 192);
        }

        public void RefreshData()
        {
            if (_EqBase == null || _PlcDevice == null || _PlcDevice == "" || bSetting) return;

            float value = _EqBase.pPlcKernel[_PlcDevice];
            if (value == 0 || _Multiplication == 0 || _Division == 0)
                this.Text = "0";
            else
            { 
                if (_FloatNumber)
                    this.Text = ((value * (float)_Multiplication) / (float)_Division).ToString("0.0");
                else
                    this.Text = Convert.ToInt32((value * (float)_Multiplication) / (float)_Division).ToString();
            }
        }

        public string GetSetValue()
        {
            RefreshData();
            return this.Text;
        }

    }
}
