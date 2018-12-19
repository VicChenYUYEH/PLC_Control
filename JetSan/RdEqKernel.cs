using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyTemplate
{
    class RdEqKernel :EqBase
    {
        private RdEqThread eqThread;
        public RdEqKernel()
        {
            eqThread = new RdEqThread(this);
        }
    }
}
