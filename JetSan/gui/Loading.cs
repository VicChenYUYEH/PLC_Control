using System;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class FrmLoading : Form
    {
        public FrmLoading()
        {
            InitializeComponent();
            pictureBox1.Image = Properties.Resources.PLC;
            pictureBox1.Refresh();
        }
    }
}
