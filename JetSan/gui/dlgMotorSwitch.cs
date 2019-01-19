using System.Drawing;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class DlgMotorSwitch : Form
    {
        public string _JogForwardDevice { get; set; }
        public string _JogBackDevice { get; set; }
        public string _SpeedDevice { get; set; }
        public int _MaxMotorRpm { get; set; }
        public int _MaxMotorPlcValue { get; set; }
        public int _Division { get; set; }
        public int _Multiplication { get; set; }

        private EqBase ebKernel;

        public DlgMotorSwitch(int m_Status, EqBase m_EqBase)
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
            if (ebKernel == null || _JogForwardDevice.Trim() == "") return;

            ebKernel.pPlcKernel[_JogForwardDevice] = 1;
        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            if (ebKernel == null || _JogForwardDevice.Trim() == "") return;

            ebKernel.pPlcKernel[_JogForwardDevice] = 0;
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            if (ebKernel == null || _JogBackDevice.Trim() == "") return;

            ebKernel.pPlcKernel[_JogBackDevice] = 1;
        }

        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            if (ebKernel == null || _JogBackDevice.Trim() == "") return;

            ebKernel.pPlcKernel[_JogBackDevice] = 0;
        }

        private void dlgMotorSwitch_Shown(object sender, System.EventArgs e)
        {
            if (ebKernel != null && _SpeedDevice != "")
            {
                inputTextBox_MotoSpeed._PlcDevice = _SpeedDevice;
                inputTextBox_MotoSpeed._EqBase = ebKernel;
                inputTextBox_MotoSpeed._MaxLimit = _MaxMotorRpm;
                //inputTextBox_MotoSpeed._Multiplication = maxMotorPlcValue;
                
                inputTextBox_MotoSpeed._Division = _Division;
                inputTextBox_MotoSpeed._Multiplication = _Multiplication;
                
                inputTextBox_MotoSpeed.RefreshData();
            }
        }

        private void inputTextBox_MotoSpeed_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (ebKernel.pPlcKernel[_JogForwardDevice] == 1)
                {
                    ebKernel.pPlcKernel[_JogForwardDevice] = 0;
                    System.Threading.Thread.Sleep(50);
                    ebKernel.pPlcKernel[_JogForwardDevice] = 1;
                }
                else if (ebKernel.pPlcKernel[_JogBackDevice] == 1)
                {
                    ebKernel.pPlcKernel[_JogBackDevice] = 0;
                    System.Threading.Thread.Sleep(50);
                    ebKernel.pPlcKernel[_JogBackDevice] = 1;
                }
            }
        }
    }
}
