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
        public string ConfirmName { get { return textBox2.Text; } set { textBox2.Text = value; } }
        public int ConfirmType { get; set; }

        public dlgConfirm(int m_Type)
        {
            InitializeComponent();

            if (m_Type == 1)
            {
                textBox1.ReadOnly = false;
            }
            else
            {
                textBox1.ReadOnly = true;
            }
        }
        
    }
}
