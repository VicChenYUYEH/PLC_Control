using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;

namespace HyTemplate
{
    public class EqBase : IDisposable
    {

        #region 宣告變數
        private EventClient ecClient;
        private MonitorAlarmThread matAlarm;
        public FileLog flDebug;
        public FileLog flOperator;

        public PlcHandler pPlcKernel { get; }
        public Recipe rRecipe { get; private set; }
        public Db dDb;
        #endregion

        public EqBase()
        {
            flDebug = new FileLog("DebugLog");
            flOperator = new FileLog("OperatorLog");
            dDb = new Db("JetSan");
            
            pPlcKernel = new PlcHandler(flDebug);
            matAlarm = new MonitorAlarmThread(pPlcKernel,flDebug);
            rRecipe = new Recipe();

            ecClient = new EventClient(this);
            ecClient.OnEventHandler += OnReceiveMessage;
        }

        ~EqBase()
        {
            pPlcKernel.Dispose();
            dDb = null;
            rRecipe = null;
            ecClient = null;
        }

        private void OnReceiveMessage(string m_MessageName, TEvent m_Event)
        {
            try
            {
                flDebug.WriteLog("EqBase Receive Message [" + m_MessageName + "]");
                if (m_MessageName == ProxyMessage.MSG_WRITE_LOG)
                {
                    foreach (KeyValuePair<String, String> data in m_Event.EventData)
                    {
                        flOperator.WriteLog(data.Key + ", " + data.Value);
                    }
                }
                else if (m_MessageName == ProxyMessage.MSG_RECIPE_SAVE)
                {
                    rRecipe.SaveFile();
                }
                else if (m_MessageName == ProxyMessage.MSG_RECIPE_SET)
                {
                    string rcp_id = m_Event.EventData["RecipeId"];
                    rRecipe.LoadFile(rcp_id);
                    foreach (KeyValuePair<string, RecipeInfo> info in rRecipe.DicRecipeDetail)
                    {
                        pPlcKernel[info.Value.DeviceName] = (short)rRecipe.DicRecipeDetail[info.Value.DeviceName].SetPoint;
                    }
                }
                else if (m_MessageName == ProxyMessage.MSG_PARAMETER_SET)
                {
                    rRecipe.LoadFile("System");
                    foreach (KeyValuePair<string, RecipeInfo> info in rRecipe.DicSystemDetail)
                    {
                        pPlcKernel[info.Value.DeviceName] = (short)rRecipe.DicSystemDetail[info.Value.DeviceName].SetPoint;
                    }
                }
            }
            catch (Exception ex)
            {
                flDebug.WriteLog("EqBase Receive Message failed :" + ex);
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
        // ~EqBase() {
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
