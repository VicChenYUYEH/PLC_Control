using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;

namespace HyTemplate
{
    class DBControl : Db
    {
        public DBControl():base("JetSan")
        {

        }
        public string InsertHistoryLog(string User, string Description, string Address = "N/A", string Recipe = "N/A")
        {
            string strSQL = "INSERT INTO HistoryLog(Occur_Time, PLC_Adress, Description, Recipe, [User])VALUES(" + "'" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "', '" + Address
                                      + "', '" + Description + "', '" + Recipe + "', '" + User + "')";
            string err = funSQL(strSQL);
            return err;
        }
    }
}
