using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class dlgConfirm : Form
    {
        public string ConfirmId { get { return textBox1.Text; } set { textBox1.Text = value; } }
        public int Confirm_InUse { get { return domainUpDown1.SelectedIndex; } set { domainUpDown1.SelectedIndex = value; } }
        public int ConfirmType { get; set; }

        public dlgConfirm()
        {
            InitializeComponent();
            domainUpDown1.SelectedIndex = 1;
        }
       
    }
}
