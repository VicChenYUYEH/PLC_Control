using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace HongYuDLL
{
    public class Db
    {
        SqlConnection db_connect;
        private bool isconnect = false;
        public Db()
        {
            string path = Directory.GetCurrentDirectory();
            string file = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            while (file != Path.GetFileNameWithoutExtension(file))
            {
                file = Path.GetFileNameWithoutExtension(file);
            }

            IniFile ini = new IniFile(path + "\\Config\\" + file + ".ini");
            string sName = ini.GetValue("DataBase", "DBName");
            string sServer = ini.GetValue("DataBase", "DBServer");
            string sUser = ini.GetValue("DataBase", "User");
            string sPassword = ini.GetValue("DataBase", "Password");
            this.open(sName, sServer, sUser, sPassword);
        }
        ~Db()
        {
            this.close();
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
        /// <param name="m_dbName">資料庫名稱</param>
        /// <returns>錯誤訊息</returns>
        private string open(string m_dbName, string m_Server, string m_User, string m_Password)
        {
            string err = "";
            try
            {
                if (IsConnect) return err = "Database already connected";

                DBPara para = new DBPara(m_dbName, m_Server, m_User, m_Password);
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
        private string close()
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
        /// <param name="m_Cmd">SQL字串</param>
        /// <returns>錯誤訊息</returns>
        public string FunSQL(string m_Cmd)
        {
            string err = "";
            try
            {
                if (!IsConnect)
                {
                    return err = "Database is disconnected";
                }

                SqlCommand command;
                command = new SqlCommand(m_Cmd, db_connect);
                string CmdType = m_Cmd.Substring(0, 6).ToUpper();
                switch (CmdType)
                {
                    case "SELECT":
                        SqlDataReader Reader_SQL = command.ExecuteReader();
                        break;

                    case "INSERT":
                        SqlDataAdapter adapter_Insert = new SqlDataAdapter();
                        adapter_Insert.InsertCommand = new SqlCommand(m_Cmd, db_connect);
                        if (adapter_Insert.InsertCommand.ExecuteNonQuery() <= 0) err = "No Data Update";
                        break;

                    case "UPDATE":
                        SqlDataAdapter adapter_Update = new SqlDataAdapter();
                        adapter_Update.UpdateCommand = new SqlCommand(m_Cmd, db_connect);
                        if (adapter_Update.UpdateCommand.ExecuteNonQuery() <= 0) err = "No Data Update";
                        break;

                    case "DELETE":
                        SqlDataAdapter adapter_Delete = new SqlDataAdapter();
                        adapter_Delete.DeleteCommand = new SqlCommand(m_Cmd, db_connect);
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
        /// <param name="m_Cmd">SQL字串</param>
        /// <param name="m_DT">Data Table</param>
        /// <returns>錯誤訊息</returns>
        public string FunSQL(string m_Cmd, out DataTable m_DT)
        {
            string err = "";
            DataTable objDT = new DataTable();
            try
            {
                if (!IsConnect)
                {
                    m_DT = objDT;
                    return err = "Database is disconnected";
                }

                SqlCommand command;
                command = new SqlCommand(m_Cmd, db_connect);
                string CmdType = m_Cmd.Substring(0, 6).ToUpper();
                switch (CmdType)
                {
                    case "SELECT":
                        SqlDataAdapter adapter_SQL = new SqlDataAdapter();
                        adapter_SQL.SelectCommand = new SqlCommand(m_Cmd, db_connect);
                        adapter_SQL.Fill(objDT);
                        if (objDT.Rows.Count <= 0) err = "No Data Selected";
                        break;

                    default:
                        err = "Command Format Mismatch";
                        break;
                }
                command.Dispose();
                m_DT = objDT;
                return err;
            }
            catch (Exception ex)
            {
                m_DT = objDT;
                return err = ex.Message;
            }
        }
    }
}
