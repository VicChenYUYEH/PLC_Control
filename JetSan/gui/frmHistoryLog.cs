using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class frmHistoryLog : Form
    {
        Dictionary<String, String> dicLog = new Dictionary<string, string>();
        public frmHistoryLog()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dicLog.Clear();
            FilterLog();
            RefreshGrid();
        }

        private void FilterLog()
        {
            const string INI_SECTION_DEBUG_LOG = "DebugLog";
            //cv_LogData.clear();

            string path = System.IO.Directory.GetCurrentDirectory();
            string file = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            while (file != Path.GetFileNameWithoutExtension(file))
            {
                file = Path.GetFileNameWithoutExtension(file);
            }
            
            IniFile ini = new IniFile(path + "\\Config\\" + file + ".ini");
            string sFileName = INI_SECTION_DEBUG_LOG;
            string sPath = ini.getValue(INI_SECTION_DEBUG_LOG, "FilePath");
            int fType = 0;
            string tmp = ini.getValue(INI_SECTION_DEBUG_LOG, "FolderType");
            if (tmp.Trim() != "")
            {
                fType = Convert.ToInt16(tmp);
            }

            string t_sDate, t_sTime, t_eDate, t_eTime;
            t_sDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            t_sTime = dateTimePicker3.Value.ToString("HH:mm");

            string s_datetime = t_sDate + " " + t_sTime;

            t_eDate = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            t_eTime = dateTimePicker4.Value.ToString("HH:mm");
            string e_datetime = t_eDate + " " + t_eTime;

            DateTime dtStart = DateTime.Parse(s_datetime);
            DateTime dtEnd = DateTime.Parse(e_datetime);

            while (dtStart.Date <= dtEnd.Date)
            {
                string read_file;
                int index = 1;

                if (fType == 1) //Day
                {                    
                    read_file = sPath + "\\" + dtStart.Year.ToString() + "\\" + dtStart.Month.ToString() + "\\" + dtStart.Day.ToString() + "\\" +
                                sFileName + "_" + dtStart.ToString("yyyyMMdd") + "_" + index.ToString("00") + ".txt";

                    while (File.Exists(read_file))
                    {
                        DoParserLog(read_file, dtStart, dtEnd);
                        index++;
                        read_file = sPath + "\\" + dtStart.Year.ToString() + "\\" + dtStart.Month.ToString() + "\\" + dtStart.Day.ToString() + "\\" +
                                sFileName + "_" + dtStart.ToString("yyyyMMdd") + "_" + index.ToString("00") + ".txt";
                    }
                }
                else if (fType == 2) //Month
                {
                    read_file = sPath + "\\" + dtStart.Year.ToString() + "\\" + dtStart.Month.ToString() + "\\" +
                                sFileName + "_" + dtStart.ToString("yyyyMMdd") + "_" + index.ToString("00") + ".txt";

                    while (File.Exists(read_file))
                    {
                        //DoParserLog();
                        index++;
                        read_file = sPath + "\\" + dtStart.Year.ToString() + "\\" + dtStart.Month.ToString() + "\\" + 
                                sFileName + "_" + dtStart.ToString("yyyyMMdd") + "_" + index.ToString("00") + ".txt";
                    }
                }
                else //None
                {
                    read_file = sPath + "\\" + 
                                sFileName + "_" + dtStart.ToString("yyyyMMdd") + "_" + index.ToString("00") + ".txt";

                    while (File.Exists(read_file))
                    {
                        //DoParserLog();
                        index++;
                        read_file = sPath + "\\" + 
                                sFileName + "_" + dtStart.ToString("yyyyMMdd") + "_" + index.ToString("00") + ".txt";
                    }
                }

                dtStart = dtStart.Date.AddDays(1);
            }            
        }

        private void DoParserLog(string m_File, DateTime m_Start, DateTime m_End)
        {
            System.IO.StreamReader file = new System.IO.StreamReader(m_File);
            string line = "";
            while ((line = file.ReadLine()) != null)
            {
                string[] arr = line.Split(',');
                DateTime dt = DateTime.Parse(arr[0]);
                if (dt < m_Start || dt > m_End)
                    continue;

                //System.Console.WriteLine(line);
                dicLog.Add(line.Substring(0, 19), line.Substring(21));
            }

            file.Close();
        }

        private void RefreshGrid()
        {
            dataGridView1.Rows.Clear();
            DataGridViewRowCollection rows = dataGridView1.Rows;            

            foreach (KeyValuePair<String, String> log in dicLog)
            {
                rows.Add(new Object[] { log.Key, log.Value });
            }
        }
    }
}
