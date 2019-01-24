using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyTemplate
{
    public class RdEqKernel :EqBase
    {
        private RdEqThread eqThread;
        public RdEqKernel()
        {
            eqThread = new RdEqThread(this);
        }
        public void WriteDebugLog(string m_body, string m_Log)
        {
            flDebug.WriteLog(m_body, m_Log);
        }
        public void WriteOperatorLog(string m_body, string m_Log)
        {
            flOperator.WriteLog(m_body, m_Log);
        }

        public string InsertHistoryLog(string m_User, string m_Description, string m_Address = "N/A", string m_Recipe = "N/A")
        {
            string strSQL = "INSERT INTO HistoryLog(Occur_Time, PLC_Adress, Description, Recipe, [User])VALUES(" + "'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "', '" + m_Address
                                      + "', '" + m_Description + "', '" + m_Recipe + "', '" + m_User + "')";
            string err = dDb.funSQL(strSQL);
            return err;
        }
    }

}
