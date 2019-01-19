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
    public partial class CurrentDateTime : UserControl
    {
        public CurrentDateTime()
        {
            InitializeComponent();

            timer1.Start();

            //this.HandleCreated += CurrentDateTime_HandleCreated;
            this.HandleDestroyed += currentDateTime_HandleDestroyed;
        }

        private void currentDateTime_HandleDestroyed(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void currentDateTime_HandleCreated(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string date = DateTime.Now.ToString("yyyy - MM - dd") ;
                string time = DateTime.Now.ToString("HH : mm : ss");

                txtDate.Text = date;
                txtTime.Text = time;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
