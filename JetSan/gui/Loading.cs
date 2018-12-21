using System;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class frmLoading : Form
    {
        public frmLoading()
        {
            InitializeComponent();
            pictureBox1.Image = Properties.Resources.PLC;
            pictureBox1.Refresh();
        }
    }
}
