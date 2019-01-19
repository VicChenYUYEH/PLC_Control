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
    public partial class DlgSwitch : Form
    {
        public string _PlcDevice { get { return textBox1.Text; } set { textBox1.Text = value; } }

        public DlgSwitch(bool m_Status)
        {
            InitializeComponent();

            if (m_Status)
            {
                btnOn.BackColor = Color.Lime;
                btnOff.BackColor = SystemColors.Control;
            }
            else
            {
                btnOn.BackColor = SystemColors.Control;
                btnOff.BackColor = Color.Lime;
            }
        }
    }
}
