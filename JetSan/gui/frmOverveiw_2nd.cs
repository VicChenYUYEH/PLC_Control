using System;
using System.Drawing;
using System.Windows.Forms;
using HyTemplate.components;

namespace HyTemplate.gui
{
    public partial class frmOverview_2nd : Form
    {
        private EqBase ebKernel;
        bool bIsInitial = false;

        public frmOverview_2nd(EqBase m_EqBase)
        {
            InitializeComponent();

            ebKernel = m_EqBase;

            if (ebKernel != null)
            {
                #region Initial Component
                //initialComponents(this);
                #endregion
            }

            System.Threading.Thread.Sleep(1000);
        }

    }
}
