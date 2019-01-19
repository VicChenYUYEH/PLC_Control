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
    public partial class DlgConfirm : Form
    {
        public string sConfirmId { get { return textBox1.Text; } set { textBox1.Text = value; } }
        public int sConfirm_InUse { get { return domainUpDown1.SelectedIndex; } set { domainUpDown1.SelectedIndex = value; } }
        public int sConfirmType { get; set; }

        public DlgConfirm()
        {
            InitializeComponent();
            domainUpDown1.SelectedIndex = 1;
        }
       
    }
}
