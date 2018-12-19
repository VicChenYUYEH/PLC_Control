using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;

namespace HyTemplate
{
    struct AlarmInfo
    {
        public string Code;
        public string Device;
        public string Description;
        public bool isHeavyAlarm;        
    }

    class MonitorAlarmThread : IDisposable
    {
        private Thread thExecute;
        private PlcHandler phPlcKernel;
        private EventClient ecClient;
        private FileLog flLog;

        bool bDisponse = false;
        bool bAlarmReset = false;

        Dictionary<string, AlarmInfo> dicAlarmInfo = new Dictionary<string, AlarmInfo>();
        Dictionary<string, bool> dicAlarmStatus = new Dictionary<string, bool>();

        public MonitorAlarmThread(PlcHandler m_PlcHandler, FileLog m_Log = null)
        {
            flLog = m_Log;
            phPlcKernel = m_PlcHandler;

            string xml_file = System.IO.Directory.GetCurrentDirectory() + "\\config\\Alarm.xml";
            if (/*m_PlcHandler != null &&*/ parserPlcXml(xml_file))
            {
                thExecute = new Thread(DoExecute);
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
            //System.Threading.Thread.Sleep(100);
            if (m_MessageName == ProxyMessage.MSG_ALARM_RESET)
            {
                bAlarmReset = true;
            }
        }

        private void DoExecute()
        {
            while (true && !bDisponse)
            {
                try
                {
                    //ReadPlcData();
                }
                catch (Exception ex)
                {
                    TEvent data = new TEvent();
                    data.MessageName = "WriteLog";
                    data.EventData["PlcHandler"] = ex.ToString();

                    ecClient.SendMessage(data);
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

            XmlNode rootNode = XmlDoc.SelectSingleNode("Alarms");

            XmlNodeList nodes = rootNode.ChildNodes;
            foreach (XmlNode node in nodes)
            {
                String code = node.Attributes["Id"].Value;

                if (dicAlarmInfo.ContainsKey(code))
                {
                    //Device Exist
                    continue;
                }

                AlarmInfo alarm;
                alarm.Code = code;
                alarm.Device = node.Attributes["Device"].Value; //node.InnerText;
                alarm.isHeavyAlarm = node.Attributes["HeavyAlarm"].Value.ToString() == "1" ? true : false; //node.InnerText;
                alarm.Description = node.Attributes["Description"].Value; //node.InnerText;

                dicAlarmInfo.Add(code, alarm);
                dicAlarmStatus.Add(code, false);
            }

            return true;
        }

        private void doMonitorAlarm()
        {
            if (bAlarmReset)
            {
                bAlarmReset = false;
                foreach (KeyValuePair<string, AlarmInfo> alarm in dicAlarmInfo)
                {
                    dicAlarmStatus[alarm.Key] = false;
                }
                Thread.Sleep(100);
            }

            //Dictionary<string, AlarmInfo> dicAlarmInfo
            List<AlarmInfo> currentAlarm = new List<AlarmInfo>();
            try
            {
                foreach (KeyValuePair<string, AlarmInfo> alarm in dicAlarmInfo)
                {
                    if (phPlcKernel[alarm.Key] == 1)
                    {//Alarm Occure
                        if (!dicAlarmStatus[alarm.Key])
                        {
                            dicAlarmStatus[alarm.Key] = true;
                            currentAlarm.Add(alarm.Value);
                        }
                    }
                    else
                    {//Alarm reset
                        if (!dicAlarmStatus[alarm.Key])
                        {
                            dicAlarmStatus[alarm.Key] = false;
                        }
                    }
                }

                if (currentAlarm.Count() > 0)
                {
                    TEvent data = new TEvent();
                    data.MessageName = ProxyMessage.MSG_ALARM_OCCURE;
                    data.EventData["Count"] = currentAlarm.Count.ToString();
                    int alarm_index = 1;
                    foreach (AlarmInfo alarm in currentAlarm)
                    {
                        data.EventData["Code" + alarm_index.ToString()] = alarm.Code;
                        data.EventData["HeavyAlarm" + alarm_index.ToString()] = alarm.isHeavyAlarm ? "1" : "0";
                        data.EventData["Description" + alarm_index.ToString()] = alarm.Description;
                        alarm_index++;
                    }
                    ecClient.SendMessage(data);
                }
                
            }
            catch(Exception ex)
            {

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
