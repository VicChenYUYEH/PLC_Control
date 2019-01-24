using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Xml;
using HongYuDLL;

namespace HyTemplate
{
    struct AlarmInfo
    {
        public string Code;
        public string Address;
        public string Description;
        public string Level;
        public string Solution;
    }

    class MonitorAlarmThread : IDisposable
    {
        private Thread thExecute;
        private PlcHandler phPlcKernel;
        private EventClient ecClient;
        private FileLog flLog;
        private Db dDb;

        bool bDisponse = false;

        Dictionary<string, AlarmInfo> dicAlarmInfo = new Dictionary<string, AlarmInfo>();
        Dictionary<string, bool> dicAlarmStatus = new Dictionary<string, bool>();
        Dictionary<AlarmInfo, DateTime> currentAlarm = new Dictionary<AlarmInfo, DateTime>();

        public MonitorAlarmThread(PlcHandler m_PlcHandler, FileLog m_FileLog)
        {
            dDb = new Db();
            phPlcKernel = m_PlcHandler;
            flLog = m_FileLog;

            string xml_file = System.IO.Directory.GetCurrentDirectory() + "\\config\\Alarm.xml";
            if (/*m_PlcHandler != null &&*/ parserPlcXml(xml_file))
            {
                thExecute = new Thread(doExecute);
                thExecute.Start();
                Thread.Sleep(100);
            }

            ecClient = new EventClient(this);
            ecClient.OnEventHandler += OnReceiveMessage;
        }

        ~MonitorAlarmThread()
        {           
            if (thExecute != null && thExecute.IsAlive)
                thExecute.Abort();
        }

        private void OnReceiveMessage(string m_MessageName, TEvent args)
        {
            if (m_MessageName == ProxyMessage.MSG_ALARM_RESET)
            {
                //bAlarmReset = true;
            }
        }

        private void doExecute()
        {
            while (true && !bDisponse)
            {
                try
                {
                    doMonitorAlarm();
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }
        }

        private bool parserPlcXml(string m_Xml)
        {
            if (!System.IO.File.Exists(m_Xml)) return false;

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(m_Xml);
            
            XmlNodeList nodes = XmlDoc.SelectNodes("Alarms/Link");
            foreach (XmlNode node in nodes)
            {
                String code = node.Attributes["Address"].Value;

                if (dicAlarmInfo.ContainsKey(code))
                {
                    //Device Exist
                    continue;
                }

                AlarmInfo alarm;
                alarm.Code = code;
                alarm.Address = node.Attributes["Address"].Value; //node.InnerText;
                alarm.Solution = node.Attributes["HowTo"].Value; //node.InnerText;
                alarm.Description = node.Attributes["Message"].Value; //node.InnerText;
                alarm.Level = node.Attributes["Type"].Value; //node.InnerText;

                dicAlarmInfo.Add(code, alarm);
                dicAlarmStatus.Add(code, false);
            }

            return true;
        }

        private void doMonitorAlarm()
        {
            try
            {
                bool new_err = false;
                bool err_clear = false;
                DataTable dt;
                string strSQL = "";
                string err = "";
                foreach (KeyValuePair<string, AlarmInfo> alarm in dicAlarmInfo)
                {
                    if (phPlcKernel.DicAlarmStatus[alarm.Key])
                    {//Alarm Occur
                        if (!dicAlarmStatus[alarm.Key])
                        {
                            dicAlarmStatus[alarm.Key] = true;
                            currentAlarm.Add(alarm.Value, DateTime.Now);
                            strSQL = "SELECT * FROM HistoryAlarm WHERE PLC_Adress = " + "'" + alarm.Value.Address +"' AND End_Time is NULL"; 
                            err = dDb.FunSQL(strSQL, out dt);
                            if(dt.Rows.Count == 0)// 找尋DB是否有當下正發生的Alarm，若有則不新增
                            {
                                strSQL = "INSERT INTO HistoryAlarm (Start_Time, PLC_Adress, [Level], Description, Solution)VALUES(" + "'" + currentAlarm[alarm.Value].ToString("yyyy/MM/dd HH:mm:ss") + "', '" + alarm.Value.Address
                                       + "', '" + alarm.Value.Level + "', '" + alarm.Value.Description + "', '" + alarm.Value.Solution + "')";
                                err = dDb.FunSQL(strSQL);
                            }
                            new_err = true;
                            dt = null;
                            flLog.WriteLog(alarm.Value.Address, "Alarm Occur");
                        }
                    }
                    else
                    {   //Alarm clear  When error off
                        if (currentAlarm.ContainsKey(alarm.Value))
                        {
                            dicAlarmStatus[alarm.Key] = false;
                            currentAlarm.Remove(alarm.Value);
                            err_clear = true;
                            //Alarm清除時，更新DB內資料(End_Time為NULL即當前異常)
                            strSQL = "UPDATE HistoryAlarm SET End_Time = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'" + "WHERE PLC_Adress =" + "'" + alarm.Value.Address +"' AND End_Time is NULL";

                            err = dDb.FunSQL(strSQL);
                            flLog.WriteLog(alarm.Value.Address, "Alarm Clear");
                        }
                    }
                }

                if (new_err || err_clear) //refresh Current alarm from DB
                {
                    strSQL = "SELECT * FROM HistoryAlarm WHERE End_Time is NULL";
                    err = dDb.FunSQL(strSQL, out dt);
                    TEvent data = new TEvent();
                    if (new_err)
                    {
                        data.MessageName = ProxyMessage.MSG_ALARM_OCCURE;
                    }
                    else if (err_clear)
                    {
                        data.MessageName = ProxyMessage.MSG_ALARM_CLEAR;
                    }
                    data.EventData["Count"] = dt.Rows.Count.ToString();
                    int alarm_index = 1;
                    for (int index = 0; index < dt.Rows.Count; index++)
                    {
                        data.EventData["OccurTime" + alarm_index.ToString()] = dt.Rows[index]["Start_Time"].ToString();
                        data.EventData["Level" + alarm_index.ToString()] = dt.Rows[index]["Level"].ToString();
                        data.EventData["Description" + alarm_index.ToString()] = dt.Rows[index]["Description"].ToString();
                        data.EventData["Solution" + alarm_index.ToString()] = dt.Rows[index]["Solution"].ToString();
                        alarm_index++;
                    }
                    ecClient.SendMessage(data);
                    dt = null;
                }
                if (err != "")
                {
                    flLog.WriteLog("DB_Fail", err);
                }
            }
            catch(Exception ex)
            {
                flLog.WriteLog("MonitorAlarmThread", ex.ToString());
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 偵測多餘的呼叫

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 處置 Managed 狀態 (Managed 物件)。
                }

                // TODO: 釋放 Unmanaged 資源 (Unmanaged 物件) 並覆寫下方的完成項。
                // TODO: 將大型欄位設為 null。

                disposedValue = true;
            }
        }

        // TODO: 僅當上方的 Dispose(bool disposing) 具有會釋放 Unmanaged 資源的程式碼時，才覆寫完成項。
        // ~MonitorAlarmThread() {
        //   // 請勿變更這個程式碼。請將清除程式碼放入上方的 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 加入這個程式碼的目的在正確實作可處置的模式。
        public void Dispose()
        {
            // 請勿變更這個程式碼。請將清除程式碼放入上方的 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果上方的完成項已被覆寫，即取消下行的註解狀態。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
