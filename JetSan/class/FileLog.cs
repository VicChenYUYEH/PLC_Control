using System;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO;

namespace HyTemplate
{   
    enum LogFolderType
    {
        lftNone = 0,
        lftDay = 1,
        lftMonth = 2
    } 

    public class FileLog
    {
        private string sFileName = "";
        private string sPath = "";
        private long lFileSize = 0;
        private uint iLevel = 1;
        private LogFolderType fType = LogFolderType.lftNone;

        public FileLog(string m_Log)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            string file = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            while (file != Path.GetFileNameWithoutExtension(file))
            {
                file = Path.GetFileNameWithoutExtension(file);
            }
            //string file = Path.GetFileNameWithoutExtension(file_name);
            //file = Path.GetFileNameWithoutExtension(file);
            IniFile ini = new IniFile(path + "\\Config\\" + file + ".ini");
            sFileName = m_Log;
            sPath = ini.GetValue(m_Log, "FilePath");
            fType = (LogFolderType) Convert.ToInt16(ini.GetValue(m_Log, "FolderType"));
            lFileSize = Convert.ToInt64(ini.GetValue(m_Log, "FileSize"));
            iLevel = Convert.ToUInt16(ini.GetValue(m_Log, "LogLevel"));
        }

        public void WriteLog(string m_body, string m_Text, uint m_LogLevel = 1)
        {
            if (m_LogLevel > iLevel) return;

            string file_name = GetLogFileName();
            StackTrace ss = new StackTrace(true);
            MethodBase mb = ss.GetFrame(1).GetMethod();
            try
            {
                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(file_name, true))
                {
                    string log_txt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + mb.DeclaringType.Name + " | " + m_body + " : " + m_Text;
                    file.WriteLine(log_txt);
                }

                //FileStream fs = new FileStream(file_name, FileMode.Create);
                ////獲得位元組陣列
                //string log_txt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " " + m_Text;
                //byte[] data = System.Text.Encoding.Default.GetBytes(log_txt);
                ////開始寫入
                //fs.Write(data, 0, data.Length);
                // //清空緩衝區、關閉流
                //fs.Flush();
                //fs.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }            
        }

        private string GetLogFileName()
        {
            string file_name = "";
            string path = sPath + "\\";
            switch (fType)
            {
                case LogFolderType.lftMonth:
                    path = path + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString();
                    break;
                case LogFolderType.lftDay:
                    path = path + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\" + DateTime.Now.Day.ToString();
                    break;
                case LogFolderType.lftNone:

                    break;
            }

            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                file_name = path + "\\" + sFileName + "_" + DateTime.Now.ToString("yyyyMMdd");// + ".txt";

                if (lFileSize > 0)
                {
                    int index = 1;
                    bool find_file = false;
                    string new_file = file_name + "_" + index.ToString("00");
                    while (File.Exists(new_file + ".txt") && !find_file)
                    {
                        FileInfo file = new FileInfo(new_file + ".txt");
                        if (file.Length < lFileSize)
                        {
                            break;
                        }
                        index++;
                        new_file = file_name + "_" + index.ToString("00");
                    }
                    file_name = new_file;
                }
                file_name += ".txt";

                return file_name;
            }
            catch (Exception ex)
            {

            }
            
            return file_name;
        }
    }
}
