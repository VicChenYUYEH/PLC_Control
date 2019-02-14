using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using HongYuDLL;

namespace HyTemplate
{
    class RdEqThread :IDisposable
    {
        enum ProcessVacuumSequence
        {
            pvsNone,
            pvsStart,
            pvsWaitDelayOpenBP,
            pvsWaitLowVacuumReady,
            pvsWaitChamberLowVacuumReady,
            pvsCheckValveStatus,
            pvsWaitChamberHighVacuumReady,
            pvsEnd
        }

        enum ProcessVentSequence
        {
            pvsNone,
            pvsStart,
            pvsWaitCloseMfc,
            pvsWaitTpStop,
            pvsWaitCloseValve,
            pvsProcessVent,
            pvsEnd
        }

        private RdEqKernel eqKernel;
        private Thread thExecute;
        private EventClient ecClient;
        private DateTime dtStart;
        int iInterval;
        int iDeleteDay;

        ProcessVacuumSequence pvsVacuumSequence;
        ProcessVentSequence pvsVentSequence;
        private Timer tmPLCRecord = null;

        public RdEqThread(RdEqKernel m_Kernel)
        {
            eqKernel = m_Kernel;

            ecClient = new EventClient(this);
            ecClient.OnEventHandler += OnReceiveMessage;

            pvsVacuumSequence = ProcessVacuumSequence.pvsNone;
            pvsVentSequence = ProcessVentSequence.pvsNone;

            thExecute = new Thread(doExecute);
            thExecute.Start();

            string path = Directory.GetCurrentDirectory();
            string file = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            while (file != Path.GetFileNameWithoutExtension(file))
            {
                file = Path.GetFileNameWithoutExtension(file);
            }

            IniFile ini = new IniFile(path + "\\Config\\" + file + ".ini");
            bool record = Convert.ToBoolean(ini.GetValue("PLCRawData", "Record"));
            iInterval = Convert.ToInt32(ini.GetValue("PLCRawData", "Interval"));
            iDeleteDay = Convert.ToInt32(ini.GetValue("PLCRawData", "DB_Delete_Interval"));

            if(record) //根據INI決定是否紀錄
            {
                tmPLCRecord = new Timer(new TimerCallback(doDataRecord), null, 500, 150);
            }
            if(iDeleteDay > 0) //根據INI決定幾天刪除Database資料
            {
                System.Timers.Timer tmDelete = new System.Timers.Timer
                {
                    Enabled = true,
                    Interval = 60000
                };
                tmDelete.Start();
                tmDelete.Elapsed += new System.Timers.ElapsedEventHandler(tmDelete_Elapsed);
            }
            Thread.Sleep(100);

        }

        private void OnReceiveMessage(string m_MessageName, TEvent m_EventData)
        {
            //throw new NotImplementedException
            if (m_MessageName == ProxyMessage.MSG_PROCESS_VACUUM)
            {
                //if (eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_HVG_H] == 1) //高真空計_高
                //{
                //    pvsVacuumSequence = ProcessVacuumSequence.pvsWaitChamberLowVacuumReady;
                //}
                //else
                //{
                //    pvsVacuumSequence = ProcessVacuumSequence.pvsStart;
                //}
                pvsVacuumSequence = ProcessVacuumSequence.pvsStart;
            }
            else if (m_MessageName == ProxyMessage.MSG_PROCESS_VENT)
            {
                pvsVentSequence = ProcessVentSequence.pvsStart;
            }
        }

        private void doExecute()
        {
            while (true && !disposedValue)
            {
                try
                {
                    if (!doCheckStatus()) continue;

                    //Do something
                    doCheckVacuumSequence();
                    doProcessVentSequence();
                }
                catch (Exception ex)
                {
                    eqKernel.flDebug.WriteLog("doExecute", ex.ToString());
                }
                finally
                {
                    Thread.Sleep(10);
                }
            }
        }

        ~RdEqThread()
        {

        }

        private bool doCheckStatus()
        {
            if (eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_EMO] == 1)
            {
                return false;
            }

            return true;
        }

        private void doCheckVacuumSequence()
        {
            if (pvsVacuumSequence == ProcessVacuumSequence.pvsStart)
            {
                
                //再次確認所有Valve OFF
                if (   eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_1] == 0
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_2] == 0
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] == 0
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_4] == 0
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_5] == 0)
                {
                    //開啟RP1 -> BP1, RP2 RP3
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_RP_1_ON] = 1; //RP1
                    //System.Threading.Thread.Sleep(2000);
                    dtStart = DateTime.Now;

                    pvsVacuumSequence = ProcessVacuumSequence.pvsWaitDelayOpenBP;
                }
                else
                {
                    //Set BP1, RP1, RP2, RP3, V1, V2, V4 ON ; V3, V5 OFF
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_1] = 0; //V1
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_2] = 0; //V2
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] = 0; //V3
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_4] = 0; //V4
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_5] = 0; //V5
                }
            }
            else if (pvsVacuumSequence == ProcessVacuumSequence.pvsWaitDelayOpenBP)
            {
                TimeSpan tsInterval = DateTime.Now - dtStart;

                if (tsInterval.TotalMilliseconds >= 2000)
                {
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_BP_1_ON] = 1; //BP1

                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_RP_2_ON] = 1; //RP2
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_RP_3_ON] = 1; //RP3

                    pvsVacuumSequence = ProcessVacuumSequence.pvsWaitLowVacuumReady;
                }
                
            }
            else if (pvsVacuumSequence == ProcessVacuumSequence.pvsWaitLowVacuumReady)
            {
                //檢查低真空計是否到達, 成立即開啟Valve抽腔體
                if (eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_LVG_1_ON] == 1 && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_1] == 0)
                {
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_1] = 1;
                }

                if (eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_LVG_2_ON] == 1 && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_2] == 0)
                {
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_2] = 1;
                }

                if (eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_LVG_3_ON] == 1 && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] == 0)
                {
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] = 1;
                }
                

                if (   eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_LVG_1_ON] == 1 
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_LVG_2_ON] == 1 
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_LVG_3_ON] == 1)
                {
                    pvsVacuumSequence = ProcessVacuumSequence.pvsWaitChamberLowVacuumReady;
                }                
            }
            else if (pvsVacuumSequence == ProcessVacuumSequence.pvsWaitChamberLowVacuumReady)
            {
                if (eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_HVG_L] == 1) //高真空計_低
                {
                    //Set V1 OFF, V3, V5 ON
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_1] = 0;

                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] = 1; //V3
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_5] = 1; //V5

                    pvsVacuumSequence = ProcessVacuumSequence.pvsCheckValveStatus;
                }                
            }
            else if (pvsVacuumSequence == ProcessVacuumSequence.pvsCheckValveStatus)
            {
                if (eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_VALVE_1_OPEN] == 1)
                {
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_1] = 0;
                }
                else if (   eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_VALVE_3_OPEN] == 0
                         || eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_VALVE_3_CLOSE] == 1)
                {
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] = 1; //V3
                }
                else if (   eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_OPEN] == 0
                         || eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_CLOSE] == 1)
                {
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_5] = 1; //V5
                }
                else
                {
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_TP_2_START] = 1; //TP2
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_TP_3_START] = 1; //TP3
                    pvsVacuumSequence = ProcessVacuumSequence.pvsWaitChamberHighVacuumReady;
                }                
            }
            else if (pvsVacuumSequence == ProcessVacuumSequence.pvsWaitChamberHighVacuumReady)
            {
                if (eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_HVG_H] == 1) //高真空計_高
                {
                    TEvent data = new TEvent();
                    data.MessageName = ProxyMessage.MSG_PROCESS_VACUUM_COMPLETE;

                    ecClient.SendMessage(data);

                    pvsVacuumSequence = ProcessVacuumSequence.pvsEnd;
                }                
            }
        }

        private void doProcessVentSequence()
        {
            if (pvsVentSequence == ProcessVentSequence.pvsNone) return;

            if (pvsVentSequence == ProcessVentSequence.pvsStart)
            {
                eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_MFC_1] = 0;
                eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_MFC_2] = 0;
                eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_MFC_3] = 0;
                eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_MFC_4] = 0;

                pvsVentSequence = ProcessVentSequence.pvsWaitCloseMfc;
            }
            else if (pvsVentSequence == ProcessVentSequence.pvsWaitCloseMfc)
            {
                if (   eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_MFC_1] == 0
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_MFC_2] == 0
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_MFC_3] == 0
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_MFC_4] == 0)
                {
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_TP_2_START] = 0;
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_TP_2_STOP] = 1;

                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_TP_3_START] = 0;
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_TP_3_STOP] = 1;

                    pvsVentSequence = ProcessVentSequence.pvsWaitTpStop;
                }
            }
            else if (pvsVentSequence == ProcessVentSequence.pvsWaitTpStop)
            {
                if (   eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_TP_2_ROTATION] == 0
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_TP_3_ROTATION] == 0)
                {
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] = 0;
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VALVE_5] = 0;

                    pvsVentSequence = ProcessVentSequence.pvsWaitCloseValve;
                }
            }
            else if (pvsVentSequence == ProcessVentSequence.pvsWaitCloseValve)
            {
                if (   eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_VALVE_3_OPEN] == 0
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_VALVE_3_CLOSE] == 1
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_OPEN] == 0
                    && eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_CLOSE] == 1)
                {
                    pvsVentSequence = ProcessVentSequence.pvsProcessVent;
                }
            }
            else if (pvsVentSequence == ProcessVentSequence.pvsProcessVent)
            {
                eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VENT] = 1;
            }
            else if (pvsVentSequence == ProcessVentSequence.pvsEnd)
            {
                if (eqKernel.pPlcKernel[ConstPlcDefine.PLC_DI_HVG_ATM] == 1)
                {
                    eqKernel.pPlcKernel[ConstPlcDefine.PLC_DO_VENT] = 0;
                    pvsVentSequence = ProcessVentSequence.pvsNone;
                }
            }
        }

        #region Record PLC RawData To Database
        private void doDataRecord(object m_ObjKey)
        {
            try
            {
                tmPLCRecord.Change(-1, -1);
                if(eqKernel.pPlcKernel.bConnect)
                { 
                    string[] listheater = { "Heater1_PV", "Heater2_PV", "Heater3_PV", "Heater4_PV" };
                    insertPLCDataDB("HeaterData", listheater);

                    string[] listpressure = { "HVG1_M1", "HVG2_M2", "HVG3_M3_1", "HVG4_M4", "LVG1_M1", "LVG2_M2", "LVG3_M3", "LVG4_M4"};
                    insertPLCDataDB("PressureData", listpressure);

                    string[] listMfc = { "MFC_0303_Ar" ,"MFC_0304_O2", "MFC_0305_Ar", "MFC_0306_O2", "MFC_0307_Ar", "MFC_0308_O2","MFC_0309_Ar", "MFC_0310_O2"};
                    double[] listRatio = {4, 20, 4, 200, 4, 20, 4, 200};
                    insertPLCDataDB("MfcFlowData", listMfc, listRatio);

                    string[] listpower = { "MF1_Power", "MF2_Power", "DC1_Power", "DC2_Power", "DC3_Power" , "DC4_Power" };
                    insertPLCDataDB("PowerData", listpower);
                }
            }
            catch (Exception ex)
            {
                eqKernel.flDebug.WriteLog("doDataRecord", ex.ToString());
            }
            finally
            {
                tmPLCRecord.Change(iInterval, 1000); //參數第一個為DB寫入間隔 EX: 2000 = 2sec
            }
        }
        
        private void tmDelete_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //取得hh:mm
            int hour = e.SignalTime.Hour;
            int minute = e.SignalTime.Minute;
            
            //中午12時0分
            if (hour == 12 && minute == 0)
            {
                deletePLCDataDB(iDeleteDay);
            }
        }

        /// <summary>
        /// 將PLC Raw Data紀錄於Data Base
        /// </summary>
        /// <param name="m_TableName">DB 目標Table</param>
        /// <param name="m_Value">DB 欄位名稱(同PLC Device)</param>
        /// <param name="m_Ratio">單位比率</param>
        private void insertPLCDataDB(string m_TableName, string[] m_Value, double[] m_Ratio = null)
        {
            string rowsName = "(";
            for(int i = 0; i< m_Value.Length; i++)
            {
                if(i == m_Value.Length - 1)
                {
                    rowsName = rowsName + m_Value[i];
                }
                else
                {
                    rowsName = rowsName + m_Value[i] + ", ";
                }
            }
            rowsName = rowsName + ", " + "Insert_Time" + ")";

            string values = "('";
            for (int i = 0; i < m_Value.Length; i++)
            {
                if (i == m_Value.Length - 1)
                {
                    switch(m_TableName)
                    {
                        case "PressureData":
                            values = values + ConvertFormat.GetTTR(eqKernel.pPlcKernel[m_Value[i]]);
                            break;
                        case "PowerData":
                            values = values + Convert.ToDouble(eqKernel.pPlcKernel[m_Value[i]]) / 100;
                            break;
                        case "MfcFlowData":
                            values = values + Convert.ToDouble(eqKernel.pPlcKernel[m_Value[i]]) / m_Ratio[i];
                            break;
                        default:
                            values = values + eqKernel.pPlcKernel[m_Value[i]];
                            break;
                    }
                }
                else
                {
                    switch (m_TableName)
                    {
                        case "PressureData":
                            if (i < 4)
                                values = values + ConvertFormat.GetITR(eqKernel.pPlcKernel[m_Value[i]]) + "', '";
                            else
                                values = values + ConvertFormat.GetTTR(eqKernel.pPlcKernel[m_Value[i]]) + "', '";
                            break;
                        case "PowerData":
                            values = values + Convert.ToDouble(eqKernel.pPlcKernel[m_Value[i]]) / 100 + "', '";
                            break;
                        case "MfcFlowData":
                            values = values + Convert.ToDouble(eqKernel.pPlcKernel[m_Value[i]]) / m_Ratio[i] + "', '";
                            break;
                        default:
                            values = values + eqKernel.pPlcKernel[m_Value[i]] + "', '";
                            break;
                    }
                }
            }
            values = values +"', '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff") + "')";
            string strSQL = "INSERT INTO " + m_TableName + rowsName + "VALUES" + values;
            string err = eqKernel.dDb.FunSQL(strSQL);
            if(err !="")
            {
                eqKernel.flDebug.WriteLog("DB_fail", err);
            }
        }

        /// <summary>
        /// 刪除DB內PLC Raw資料依Config
        /// </summary>
        /// <param name="m_DB_Delete">刪除天數(N天前)</param>
        private void deletePLCDataDB(int m_DB_Delete)
        {
            DateTime datetime = DateTime.Now.AddDays(-m_DB_Delete);
            string strSQL = "DELETE FROM HeaterData WHERE Insert_Time < '" + datetime.ToString("yyyy/MM/dd HH:mm:ss.fff") +" '";
            string err = eqKernel.dDb.FunSQL(strSQL);
            strSQL = "DELETE FROM PressureData WHERE Insert_Time < '" + datetime.ToString("yyyy/MM/dd HH:mm:ss.fff") + " '";
            err = eqKernel.dDb.FunSQL(strSQL);
            strSQL = "DELETE FROM MfcFlowData WHERE Insert_Time < '" + datetime.ToString("yyyy/MM/dd HH:mm:ss.fff") + " '";
            err = eqKernel.dDb.FunSQL(strSQL);
            strSQL = "DELETE FROM PowerData WHERE Insert_Time < '" + datetime.ToString("yyyy/MM/dd HH:mm:ss.fff") + " '";
            err = eqKernel.dDb.FunSQL(strSQL);
        }
        #endregion

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
        // ~RdEqThread() {
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
