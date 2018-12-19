using System.Drawing;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class dlgMotorSwitch : Form
    {
        public string jogForwardDevice { get; set; }
        public string jogBackDevice { get; set; }
        public string speedDevice { get; set; }
        public int maxMotorRpm { get; set; }
        public int maxMotorPlcValue { get; set; }
        public int _Division { get; set; }
        public int _Multiplication { get; set; }

        private EqBase ebKernel;

        public dlgMotorSwitch(int m_Status, EqBase m_EqBase)
        {
            InitializeComponent();

            ebKernel = m_EqBase;

            button1.BackColor = SystemColors.Control;
            button2.BackColor = SystemColors.Control;
            button3.BackColor = SystemColors.Control;
            if (m_Status == 1) //forward
            {
                button3.BackColor = Color.Lime;
            }
            else if (m_Status == 2) //back
            {
                button1.BackColor = Color.Lime;
            }
            else //Stop
            {
                button2.BackColor = Color.Lime;
            }
        }
        

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            if (ebKernel == null || jogForwardDevice.Trim() == "") return;

            ebKernel.PlcKernel[jogForwardDevice] = 1;
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            if (ebKernel == null || jogForwardDevice.Trim() == "") return;

            ebKernel.PlcKernel[jogForwardDevice] = 0;
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            if (ebKernel == null || jogBackDevice.Trim() == "") return;

            ebKernel.PlcKernel[jogBackDevice] = 1;
        }

        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            if (ebKernel == null || jogBackDevice.Trim() == "") return;

            ebKernel.PlcKernel[jogBackDevice] = 0;
        }

        private void dlgMotorSwitch_Shown(object sender, System.EventArgs e)
        {
            if (ebKernel != null && speedDevice != "")
            {
                inputTextBox_MotoSpeed._PlcDevice = speedDevice;
                inputTextBox_MotoSpeed._EqBase = ebKernel;
                inputTextBox_MotoSpeed._MaxLimit = maxMotorRpm;
                //inputTextBox_MotoSpeed._Multiplication = maxMotorPlcValue;

                inputTextBox_MotoSpeed._DoubleWord = true;
                inputTextBox_MotoSpeed._Division = _Division;
                inputTextBox_MotoSpeed._Multiplication = _Multiplication;
                
                inputTextBox_MotoSpeed.refreshData();
            }
        }

        private void inputTextBox_MotoSpeed_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ebKernel.PlcKernel[jogForwardDevice] == 1)
                {
                    ebKernel.PlcKernel[jogForwardDevice] = 0;
                    System.Threading.Thread.Sleep(50);
                    ebKernel.PlcKernel[jogForwardDevice] = 1;
                }
                else if (ebKernel.PlcKernel[jogBackDevice] == 1)
                {
                    ebKernel.PlcKernel[jogBackDevice] = 0;
                    System.Threading.Thread.Sleep(50);
                    ebKernel.PlcKernel[jogBackDevice] = 1;
                }
            }
        }
    }
}
