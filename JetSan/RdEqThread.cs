using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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

        ProcessVacuumSequence pvsVacuumSequence;
        ProcessVentSequence pvsVentSequence;

        public RdEqThread(RdEqKernel m_Kernel)
        {
            eqKernel = m_Kernel;

            ecClient = new EventClient(this);
            ecClient.OnEventHandler += OnReceiveMessage;

            pvsVacuumSequence = ProcessVacuumSequence.pvsNone;
            pvsVentSequence = ProcessVentSequence.pvsNone;

            thExecute = new Thread(DoExecute);
            thExecute.Start();
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

        private void DoExecute()
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
            if (eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_EMO] == 1)
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
                if (   eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_1] == 0
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_2] == 0
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] == 0
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_4] == 0
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_5] == 0)
                {
                    //開啟RP1 -> BP1, RP2 RP3
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_RP_1_ON] = 1; //RP1
                    //System.Threading.Thread.Sleep(2000);
                    dtStart = DateTime.Now;

                    pvsVacuumSequence = ProcessVacuumSequence.pvsWaitDelayOpenBP;
                }
                else
                {
                    //Set BP1, RP1, RP2, RP3, V1, V2, V4 ON ; V3, V5 OFF
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_1] = 0; //V1
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_2] = 0; //V2
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] = 0; //V3
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_4] = 0; //V4
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_5] = 0; //V5
                }
            }
            else if (pvsVacuumSequence == ProcessVacuumSequence.pvsWaitDelayOpenBP)
            {
                TimeSpan tsInterval = DateTime.Now - dtStart;

                if (tsInterval.TotalMilliseconds >= 2000)
                {
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_BP_1_ON] = 1; //BP1

                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_RP_2_ON] = 1; //RP2
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_RP_3_ON] = 1; //RP3

                    pvsVacuumSequence = ProcessVacuumSequence.pvsWaitLowVacuumReady;
                }
                
            }
            else if (pvsVacuumSequence == ProcessVacuumSequence.pvsWaitLowVacuumReady)
            {
                //檢查低真空計是否到達, 成立即開啟Valve抽腔體
                if (eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_LVG_1_ON] == 1 && eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_1] == 0)
                {
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_1] = 1;
                }

                if (eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_LVG_2_ON] == 1 && eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_2] == 0)
                {
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_2] = 1;
                }

                if (eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_LVG_3_ON] == 1 && eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] == 0)
                {
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] = 1;
                }
                

                if (   eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_LVG_1_ON] == 1 
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_LVG_2_ON] == 1 
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_LVG_3_ON] == 1)
                {
                    pvsVacuumSequence = ProcessVacuumSequence.pvsWaitChamberLowVacuumReady;
                }                
            }
            else if (pvsVacuumSequence == ProcessVacuumSequence.pvsWaitChamberLowVacuumReady)
            {
                if (eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_HVG_L] == 1) //高真空計_低
                {
                    //Set V1 OFF, V3, V5 ON
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_1] = 0;

                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] = 1; //V3
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_5] = 1; //V5

                    pvsVacuumSequence = ProcessVacuumSequence.pvsCheckValveStatus;
                }                
            }
            else if (pvsVacuumSequence == ProcessVacuumSequence.pvsCheckValveStatus)
            {
                if (eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_1_OPEN] == 1)
                {
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_1] = 0;
                }
                else if (   eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_3_OPEN] == 0
                         || eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_3_CLOSE] == 1)
                {
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] = 1; //V3
                }
                else if (   eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_OPEN] == 0
                         || eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_CLOSE] == 1)
                {
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_5] = 1; //V5
                }
                else
                {
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_TP_2_START] = 1; //TP2
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_TP_3_START] = 1; //TP3
                    pvsVacuumSequence = ProcessVacuumSequence.pvsWaitChamberHighVacuumReady;
                }                
            }
            else if (pvsVacuumSequence == ProcessVacuumSequence.pvsWaitChamberHighVacuumReady)
            {
                if (eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_HVG_H] == 1) //高真空計_高
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
                eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_MFC_1] = 0;
                eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_MFC_2] = 0;
                eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_MFC_3] = 0;
                eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_MFC_4] = 0;

                pvsVentSequence = ProcessVentSequence.pvsWaitCloseMfc;
            }
            else if (pvsVentSequence == ProcessVentSequence.pvsWaitCloseMfc)
            {
                if (   eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_MFC_1] == 0
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_MFC_2] == 0
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_MFC_3] == 0
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_MFC_4] == 0)
                {
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_TP_2_START] = 0;
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_TP_2_STOP] = 1;

                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_TP_3_START] = 0;
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_TP_3_STOP] = 1;

                    pvsVentSequence = ProcessVentSequence.pvsWaitTpStop;
                }
            }
            else if (pvsVentSequence == ProcessVentSequence.pvsWaitTpStop)
            {
                if (   eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_TP_2_ROTATION] == 0
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_TP_3_ROTATION] == 0)
                {
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_3] = 0;
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VALVE_5] = 0;

                    pvsVentSequence = ProcessVentSequence.pvsWaitCloseValve;
                }
            }
            else if (pvsVentSequence == ProcessVentSequence.pvsWaitCloseValve)
            {
                if (   eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_3_OPEN] == 0
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_3_CLOSE] == 1
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_OPEN] == 0
                    && eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_VALVE_5_CLOSE] == 1)
                {
                    pvsVentSequence = ProcessVentSequence.pvsProcessVent;
                }
            }
            else if (pvsVentSequence == ProcessVentSequence.pvsProcessVent)
            {
                eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VENT] = 1;
            }
            else if (pvsVentSequence == ProcessVentSequence.pvsEnd)
            {
                if (eqKernel.PlcKernel[ConstPlcDefine.PLC_DI_HVG_ATM] == 1)
                {
                    eqKernel.PlcKernel[ConstPlcDefine.PLC_DO_VENT] = 0;
                    pvsVentSequence = ProcessVentSequence.pvsNone;
                }
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
