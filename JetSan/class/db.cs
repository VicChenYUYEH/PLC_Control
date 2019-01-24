using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB
{
    public class Db
    {
        SqlConnection db_connect;
        private bool isconnect = false;
        public Db(string DB_Name)
        {
            this.Open(DB_Name);
        }
        ~Db()
        {
            this.Close();
        }

        private struct DBPara
        {
            public string sServer;              //DataBase Server Name
            public string sUser;                //DataBase User
            public string sPassword;            //DataBase User's PWD
            public string sName;                //DataBase NAME
            public string sConnTimeOut;         //DataBase Connect Timeout
            public string sPort;
            public DBPara(string DBName = "Db_Test", string DBServer = "localhost",
                          string DBUser = "sa", string DBPassword = "P@ssw0rd",
                          string DBConnTimeOut = "5", string DBPort = "1521")
            {
                sName = DBName;
                sServer = DBServer;
                sUser = DBUser;
                sPassword = DBPassword;
                sConnTimeOut = DBConnTimeOut;
                sPort = DBPort;
            }
        }

        /// 取得資料庫連線狀態
        /// </summary>
        public bool IsConnect
        {
            get
            {
                if (!object.ReferenceEquals(db_connect, null))
                {
                    isconnect = (db_connect.State == ConnectionState.Open) ? true : false;
                }
                return isconnect;
            }
        }

        /// <summary>
        /// 開啟MS SQL資料庫連線
        /// </summary>
        /// <param name="dbName">資料庫名稱</param>
        /// <returns>錯誤訊息</returns>
        private string Open(string dbName)
        {
            string err = "";
            try
            {
                if (IsConnect) return err = "Database already connected";

                DBPara para = new DBPara(dbName);
                string sConnect = String.Concat("Initial Catalog=", para.sName,
                                        ";Password=", para.sPassword,
                                        ";User ID=", para.sUser,
                                        ";Data Source=", para.sServer,
                                        ";Connect Timeout=", para.sConnTimeOut);

                db_connect = new SqlConnection(sConnect);
                db_connect.Open();
                return err;
            }
            catch (Exception ex)
            {
                return err = ex.Message;
            }
        }

        /// <summary>
        /// 關閉MS SQL資料庫連線
        /// </summary>
        /// <returns>錯誤訊息</returns>
        private string Close()
        {
            string err = "";
            try
            {
                if (!object.ReferenceEquals(db_connect, null))
                {
                    db_connect.Close();
                }
                return err;
            }
            catch (Exception ex)
            {
                return err = ex.Message;
            }
        }
        
        /// <summary>
        /// SQL執行方法
        /// </summary>
        /// <param name="cmd">SQL字串</param>
        /// <returns>錯誤訊息</returns>
        public string funSQL(string cmd)
        {
            string err = "";
            try
            {
                if (!IsConnect)
                {
                    return err = "Database is disconnected";
                }

                SqlCommand command;
                command = new SqlCommand(cmd, db_connect);
                string CmdType = cmd.Substring(0, 6).ToUpper();
                switch (CmdType)
                {
                    case "SELECT":
                        SqlDataReader Reader_SQL = command.ExecuteReader();
                        break;

                    case "INSERT":
                        SqlDataAdapter adapter_Insert = new SqlDataAdapter();
                        adapter_Insert.InsertCommand = new SqlCommand(cmd, db_connect);
                        if (adapter_Insert.InsertCommand.ExecuteNonQuery() <= 0) err = "No Data Update";
                        break;

                    case "UPDATE":
                        SqlDataAdapter adapter_Update = new SqlDataAdapter();
                        adapter_Update.UpdateCommand = new SqlCommand(cmd, db_connect);
                        if (adapter_Update.UpdateCommand.ExecuteNonQuery() <= 0) err = "No Data Update";
                        break;

                    case "DELETE":
                        SqlDataAdapter adapter_Delete = new SqlDataAdapter();
                        adapter_Delete.DeleteCommand = new SqlCommand(cmd, db_connect);
                        if (adapter_Delete.DeleteCommand.ExecuteNonQuery() <= 0) err = "No Data Update";
                        break;

                    default:
                        err = "Command Format Mismatch";
                        break;
                }
                command.Dispose();
                return err;
            }
            catch (Exception ex)
            {
                return err = ex.Message;
            }
        }

        /// <summary>
        /// 提供SQL Select回傳Data Table方法
        /// </summary>
        /// <param name="cmd">SQL字串</param>
        /// <param name="DT">Data Table</param>
        /// <returns>錯誤訊息</returns>
        public string funSQL(string cmd, out DataTable DT)
        {
            string err = "";
            DataTable objDT = new DataTable();
            try
            {
                if (!IsConnect)
                {
                    DT = objDT;
                    return err = "Database is disconnected";
                }

                SqlCommand command;
                command = new SqlCommand(cmd, db_connect);
                string CmdType = cmd.Substring(0, 6).ToUpper();
                switch (CmdType)
                {
                    case "SELECT":
                        SqlDataAdapter adapter_SQL = new SqlDataAdapter();
                        adapter_SQL.SelectCommand = new SqlCommand(cmd, db_connect);
                        adapter_SQL.Fill(objDT);
                        if (objDT.Rows.Count <= 0) err = "No Data Selected";
                        break;

                    default:
                        err = "Command Format Mismatch";
                        break;
                }
                command.Dispose();
                DT = objDT;
                return err;
            }
            catch (Exception ex)
            {
                DT = objDT;
                return err = ex.Message;
            }
        }
    }
}
