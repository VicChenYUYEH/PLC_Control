#define _SIMULATOR1

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Xml;
using HyTemplate.gui;
using System.Collections;
using System.Threading.Tasks;

namespace HyTemplate
{
    enum PlcDeviceType
    {
        pdtNone = 0,
        pdtX = 1,
        pdtY = 2,
        pdtB = 3,
        pdtW = 4,
        pdtL = 5,
        pdtM = 6,
        pdtD = 7,
        pdtR = 8,
        pdtZR = 9
    }

    enum PlcValueType
    {
        pvtBool = 0,
        pvtInteger = 1,
        pvtDoubleWord = 2,
        pvtAscii = 3
    }

    struct PlcDataInfo
    {
        public string FirstDevice;
        public string LastDevice;
        public int Length;
        public int Id;
        public NumberStyles DeviceType;
    }

    public class PlcHandler : IDisposable
    {
        private MelsecAccessor melPlcAccessor;
        private int iLogicalStationNumber = 0;
        private Thread thExecute;
        private EventClient ecClient;
        private FileLog flLog;
        private bool bChange = true;
        private bool bNeedRetry = true;
        private FrmLoading lLoading = new FrmLoading();

        public bool bConnect { get; private set; } = false;
        bool bDisponse = false;

        Dictionary<PlcDeviceType, PlcDataInfo> dicPlcInfo = new Dictionary<PlcDeviceType, PlcDataInfo>();
        Dictionary<string, KeyValuePair<string, int>> dicPlcBuffer = new Dictionary<string, KeyValuePair<string, int>>();
        Dictionary<string, PlcValueType> dicPlcValueType = new Dictionary<string, PlcValueType>();
        Dictionary<PlcDeviceType, PlcDataInfo> dicAlarmInfo = new Dictionary<PlcDeviceType, PlcDataInfo>();
        public Dictionary<string, bool> DicAlarmStatus { get; } = new Dictionary<string, bool>();

        public PlcHandler(FileLog m_Log=null)
        {
            flLog = m_Log;

            string xml_file = System.IO.Directory.GetCurrentDirectory() + "\\config\\Plc.xml";
            string Alarm_xml = System.IO.Directory.GetCurrentDirectory() + "\\config\\Alarm.xml";
            if (parserPlcXml(xml_file) && parserAlarmXml(Alarm_xml))
            {
#if _SIMULATOR

#else
                melPlcAccessor = new MelsecAccessor(iLogicalStationNumber);
                if (melPlcAccessor.Open() == 0)
                {
                    bNeedRetry = false;
                }
                thExecute = new Thread(doExecute);
                thExecute.Start();
                Thread.Sleep(100);
#endif
            }

            ecClient = new EventClient(this);
            ecClient.OnEventHandler += onReceiveMessage;            
        }
        
        ~PlcHandler()
        {
            //melPlcAccessor.Close();
            dicPlcBuffer = null;

            if (thExecute != null && thExecute.IsAlive)
                thExecute.Abort();
        }

        private void onReceiveMessage(string m_MessageName, TEvent args)
        {
            //System.Threading.Thread.Sleep(100);
        }

        private void doExecute()
        {
            while (true && !bDisponse)
            {
                try
                {
                    checkPLCConnect();
                    readPlcData(dicPlcInfo);
                    readPlcData(dicAlarmInfo, true);
                }
                catch (Exception ex)
                {
                    writeLog(ex.ToString());
                }
                finally
                {
                    Thread.Sleep(10);
                }
            }
        }

        private void checkPLCConnect()
        {
            short[] values;
            bool bPlcStatusReg = bConnect;
            bConnect = (melPlcAccessor.ReadDeviceBlock("W0100", 24, out values) == 0) ? true : false;
            bChange = (bPlcStatusReg != bConnect) ? true : false;

            if (bChange) //Update Signal
            {
                if (!bConnect)
                {
                    bNeedRetry = true;
                }
                bChange = false;
                TEvent data = new TEvent();
                data.MessageName = (bConnect)? ProxyMessage.MSG_PLC_CONNECT : ProxyMessage.MSG_PLC_DISCONNECT;
                Thread.Sleep(1000);
                ecClient.SendMessage(data);
            }
            if (!bConnect && bNeedRetry)
            {
                lLoading.ShowDialog();
                lLoading.Refresh();
                if(lLoading.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    if (melPlcAccessor.Open() == 0)
                    {
                        bNeedRetry = false;
                    }                
                }
                else
                {
                    Thread.Sleep(300000); //5 min
                }
            }
        }

        private bool parserPlcXml(string m_Xml)
        {
            if (!System.IO.File.Exists(m_Xml)) return false;

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(m_Xml);

            XmlNode rootNode = XmlDoc.SelectSingleNode("PLC");
            iLogicalStationNumber = Convert.ToInt16(rootNode.Attributes["ActLogicalStationNumber"].Value);

            XmlNodeList nodes = XmlDoc.SelectNodes("PLC/Group");
            foreach (XmlNode chile_node in nodes)
            {
                foreach (XmlNode node in chile_node)
                {
                    string device_name = node.Attributes["Name"].Value;

                    if (dicPlcBuffer.ContainsKey(device_name))
                    {
                        //Device Exist
                        continue;
                    }

                    string device = node.Attributes["Device"].Value; //node.InnerText;

                    PlcDeviceType device_type = getDeviceType(device);

                    device = (device_type == PlcDeviceType.pdtZR)? device.Substring(0, 2) + device.Substring(2).PadLeft(5, '0') : device = device.Substring(0, 1) + device.Substring(1).PadLeft(5, '0');

                    int value_type = Convert.ToInt16(node.Attributes["ValueType"].Value);
                    dicPlcValueType.Add(device_name, (PlcValueType)value_type);

                    dicPlcBuffer.Add(device_name, new KeyValuePair<string, int>(device, 0));

                    storePlcInfo(device_type, device, dicPlcInfo);
                }
            }

            #region 找出所有類型的起始點及長度
            for (int count = 0; count < dicPlcInfo.Count; count++)
            {
                KeyValuePair<PlcDeviceType, PlcDataInfo> info = dicPlcInfo.ElementAt(count);
                PlcDataInfo data = info.Value;

                int length = (convertDecimal((info.Value.DeviceType == NumberStyles.HexNumber ? "0x" : "") + info.Value.LastDevice.Substring(info.Key == PlcDeviceType.pdtZR ? 2 : 1)) - data.Id);

                if (   info.Key == PlcDeviceType.pdtX 
                    || info.Key == PlcDeviceType.pdtY
                    || info.Key == PlcDeviceType.pdtB
                    || info.Key == PlcDeviceType.pdtL
                    || info.Key == PlcDeviceType.pdtM)
                {
                    string first_device = info.Value.FirstDevice;
                    int index = info.Value.Id % 16;
                    if (index > 0)
                    {
                        data.Id = info.Value.Id - index;
                        data.FirstDevice = data.FirstDevice.Substring(0, 1) + data.Id.ToString((info.Value.DeviceType == NumberStyles.HexNumber?"X":"")).PadLeft(5, '0');
                    }

                    length = convertDecimal((info.Value.DeviceType == NumberStyles.HexNumber?"0x":"") + info.Value.LastDevice.Substring(1)) - data.Id;
                    if (info.Value.DeviceType == NumberStyles.HexNumber)
                    {
                        length = length / 16 + 1;
                    }
                }

                data.Length = length;
                dicPlcInfo[info.Key] = data;
            }
            #endregion

            return true;
        }
        
        private bool parserAlarmXml(string m_Xml)
        {
            if (!System.IO.File.Exists(m_Xml)) return false;

            XmlDocument XmlDoc = new XmlDocument();
            XmlDoc.Load(m_Xml);
            
            XmlNodeList nodes = XmlDoc.SelectNodes("Alarms/Link");
            foreach (XmlNode chile_node in nodes)
            {
                string address = chile_node.Attributes["Address"].Value;
                bool entity = (chile_node.Attributes["Entity"].Value == "true")? true: false;

                PlcDeviceType device_type = getDeviceType(address);
                if (DicAlarmStatus.ContainsKey(address))
                {
                    //Device Exist
                    continue;
                }
                DicAlarmStatus.Add(address, false);
                if (!entity)//有實體PLC點位 因PLC XML已有該資料就不重複建立
                {
                    storePlcInfo(device_type, address, dicAlarmInfo);
                }
            }

            #region 找出所有類型的起始點及長度
            for (int count = 0; count < dicAlarmInfo.Count; count++)
            {
                KeyValuePair<PlcDeviceType, PlcDataInfo> info = dicAlarmInfo.ElementAt(count);
                PlcDataInfo data = info.Value;

                int length = (convertDecimal((info.Value.DeviceType == NumberStyles.HexNumber ? "0x" : "") + info.Value.LastDevice.Substring(info.Key == PlcDeviceType.pdtZR ? 2 : 1)) - data.Id);

                if (info.Key == PlcDeviceType.pdtX
                    || info.Key == PlcDeviceType.pdtY
                    || info.Key == PlcDeviceType.pdtB
                    || info.Key == PlcDeviceType.pdtL
                    || info.Key == PlcDeviceType.pdtM)
                {
                    string first_device = info.Value.FirstDevice;
                    int index = info.Value.Id % 16;
                    if (index > 0)
                    {
                        data.Id = info.Value.Id - index;
                        data.FirstDevice = data.FirstDevice.Substring(0, 1) + data.Id.ToString((info.Value.DeviceType == NumberStyles.HexNumber ? "X" : "")).PadLeft(5, '0');
                    }

                    length = convertDecimal((info.Value.DeviceType == NumberStyles.HexNumber ? "0x" : "") + info.Value.LastDevice.Substring(1)) - data.Id;
                    if (info.Value.DeviceType == NumberStyles.HexNumber)
                    {
                        length = length / 16 + 1;
                    }
                }

                data.Length = length;
                dicAlarmInfo[info.Key] = data;
            }
            #endregion

            return true;
        }
        
        private void readPlcData(Dictionary<PlcDeviceType, PlcDataInfo> m_DicSearchPlc, bool m_bAlarm =false)
        {
            const uint BATCH_READ_LENGTH = 320;
            try
            {
                foreach (KeyValuePair<PlcDeviceType, PlcDataInfo> info in m_DicSearchPlc)
                {
                    int type_length = (info.Key == PlcDeviceType.pdtM) ? (info.Value.Length / 16) + 1 : info.Value.Length;

                    int loop_count = (int)((type_length / BATCH_READ_LENGTH) + 1);

                    int device_type_count = info.Key == PlcDeviceType.pdtZR ? 2 : 1;
                    for (int loop = 0; loop < loop_count; loop++)
                    {
                        string start_adr = "";
                        string device_type = (info.Value.DeviceType == NumberStyles.HexNumber ? "X" : ""); //判斷是否為HEX

                        start_adr = info.Value.FirstDevice.Substring(0, device_type_count) + (info.Value.Id + (loop * BATCH_READ_LENGTH)).ToString(device_type);

                        int length = (int)(type_length - ((loop + 1) * BATCH_READ_LENGTH) > 0 ? (int)BATCH_READ_LENGTH : type_length);
                        short[] values;

                        if (melPlcAccessor == null) return;
                        if (melPlcAccessor.ReadDeviceBlock(start_adr, length, out values) == 0)
                        {
                            if (   info.Key == PlcDeviceType.pdtX
                                || info.Key == PlcDeviceType.pdtY
                                || info.Key == PlcDeviceType.pdtB
                                || info.Key == PlcDeviceType.pdtM)
                            {                      
                                for (int index = 0; index < length; )
                                {
                                    string binary = Convert.ToString(values[index], 2).PadLeft(16, '0');
                                    char[] arr_binary = binary.ToCharArray();
                                    Array.Reverse(arr_binary);
                                    for (int binary_index = 0; binary_index < 16; binary_index++)
                                    {                                        
                                        string buf_adr = start_adr.Substring(0, device_type_count) + (info.Value.Id + (loop * BATCH_READ_LENGTH) + (16 * index) + binary_index).ToString(device_type).PadLeft(5, '0');

                                        /////////dicPlcInfo || dicAlarmInfo 皆會更新 (PLC實體 與 PLC軟體的異常) /////////
                                        if (DicAlarmStatus.ContainsKey(buf_adr)) 
                                        { DicAlarmStatus[buf_adr] = (arr_binary[binary_index] == '1') ? true : false; }
                                        /////////////////////////////////////////////////////////////////////////////////
                                        if (!m_bAlarm) //更新PLC XML對應的Infomation
                                        {
                                            KeyValuePair<string, KeyValuePair<string, int>> data = dicPlcBuffer.FirstOrDefault(address => address.Value.Key == buf_adr);
                                            if (data.Value.Key == null)
                                            {
                                                continue;
                                            }
                                            KeyValuePair<string, int> new_data = new KeyValuePair<string, int>(data.Value.Key, int.Parse(arr_binary[binary_index].ToString()));
                                            dicPlcBuffer[data.Key] = new_data;
                                        }
                                    }
                                    if (!(info.Value.DeviceType == NumberStyles.HexNumber || info.Key == PlcDeviceType.pdtM)) 
                                        index += 16;
                                    else//M type回傳資料型態為一個陣列內有16Bit的Bool值  ex:[0] = 2 M0與M1皆為為ON，其他M2~M15為OFF
                                        index++;
                                }
                            }
                            else if (info.Key == PlcDeviceType.pdtL)
                            {
                                length /= 16;
                                for (int index = 0; index <= length;)
                                {
                                    string binary = Convert.ToString(values[index], 2).PadLeft(16, '0');
                                    char[] arr_binary = binary.ToCharArray();
                                    Array.Reverse(arr_binary);
                                    for (int binary_index = 0; binary_index < 16; binary_index++)
                                    {
                                        string buf_adr = start_adr.Substring(0, device_type_count) + (info.Value.Id + (loop * BATCH_READ_LENGTH) + (16 * index) + binary_index).ToString(device_type).PadLeft(5, '0');

                                        KeyValuePair<string, KeyValuePair<string, int>> data = dicPlcBuffer.FirstOrDefault(address => address.Value.Key == buf_adr);
                                        if (data.Value.Key == null)
                                        {
                                            continue;
                                        }
                                        KeyValuePair<string, int> new_data = new KeyValuePair<string, int>(data.Value.Key, int.Parse(arr_binary[binary_index].ToString()));
                                        dicPlcBuffer[data.Key] = new_data;
                                    }
                                    index++;
                                }
                            }
                            else
                            {
                                for (int index = 0; index < length; index++)
                                {
                                    string buf_adr = start_adr.Substring(0, device_type_count) + ((info.Value.Id + (loop * BATCH_READ_LENGTH)) + index).ToString(device_type).PadLeft(5, '0');
                                    KeyValuePair<string, KeyValuePair<string, int>> data = dicPlcBuffer.FirstOrDefault(address => address.Value.Key == buf_adr);
                                    if (data.Value.Key == null)
                                    {
                                        continue;
                                    }
                                    KeyValuePair<string, int> new_data = new KeyValuePair<string, int>(data.Value.Key, values[index]);
                                    dicPlcBuffer[data.Key] = new_data;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                writeLog(ex.ToString());
            }
        }

        private Int32 convertDecimal(string m_Value)
        {
            int value;
            if (m_Value.StartsWith("0x"))
            {
                value = Int32.Parse(m_Value.Substring(2), NumberStyles.HexNumber);
            }
            else
            {
                value = Int32.Parse(m_Value);
            }
            return value;
        }

        private int convertStrToAscii(string m_Str)
        {
            int value = Convert.ToInt32(m_Str);
            // Convert the decimal value to a hexadecimal value in string form.
            //string hexOutput = String.Format("{0:X}", value);

            return value;
        }

        /* 
         * PLC Device Type Define
        enum PlcDeviceType {           
            pdtX = 1,
            pdtY = 2,
            pdtB = 3,
            pdtL = 4,
            pdtM = 5,
            pdtD = 6,
            pdtR = 7,
            pdtZR = 8
        }
        */
        private PlcDeviceType getDeviceType(string m_Device)
        {
            PlcDeviceType type = PlcDeviceType.pdtNone;

            switch (m_Device.Substring(0, 1))
            {
                case "X":
                    type = PlcDeviceType.pdtX;
                    break;
                case "Y":
                    type = PlcDeviceType.pdtY;
                    break;
                case "B":
                    type = PlcDeviceType.pdtB;
                    break;
                case "W":
                    type = PlcDeviceType.pdtW;
                    break;
                case "L":
                    type = PlcDeviceType.pdtL;
                    break;
                case "M":
                    type = PlcDeviceType.pdtM;
                    break;
                case "D":
                    type = PlcDeviceType.pdtD;
                    break;
                case "R":
                    type = PlcDeviceType.pdtR;
                    break;
                case "Z":
                    type = PlcDeviceType.pdtZR;
                    break;
            }

            return type;
        }
        
        private void storePlcInfo(PlcDeviceType m_DeviceType, string m_Device, Dictionary<PlcDeviceType, PlcDataInfo> m_Container)
        {
            int index = 1;
            int device_id = 0;
            if (m_DeviceType == PlcDeviceType.pdtZR)
            {
                index = 2;
            }

            if (m_Container.ContainsKey(m_DeviceType))
            {
                PlcDataInfo info = m_Container[m_DeviceType];
                device_id = convertDecimal((info.DeviceType == NumberStyles.HexNumber ? "0x" : "") + m_Device.Substring(index));

                if (info.Id > device_id)
                {
                    info.FirstDevice = m_Device;
                    info.Id = device_id;
                }
                else if (info.Id < device_id)
                {
                    info.LastDevice = m_Device;
                }
                m_Container[m_DeviceType] = info;
            }
            else
            {
                PlcDataInfo info = new PlcDataInfo();
                info.FirstDevice = m_Device;
                info.LastDevice = m_Device;

                if (m_DeviceType == PlcDeviceType.pdtX || m_DeviceType == PlcDeviceType.pdtY || m_DeviceType == PlcDeviceType.pdtB 
                    || m_DeviceType == PlcDeviceType.pdtW)
                {
                    info.DeviceType = NumberStyles.HexNumber;
                }
                else
                {
                    info.DeviceType = NumberStyles.Integer;
                }
                info.Id = convertDecimal((info.DeviceType == NumberStyles.HexNumber ? "0x" : "") + m_Device.Substring(index));

                m_Container.Add(m_DeviceType, info);
            }
        }

        public short GetPlcValue(string m_DeviceName)
        {
            if (!dicPlcBuffer.ContainsKey(m_DeviceName)) return 0;

            return (short)dicPlcBuffer[m_DeviceName].Value;
        }

        public void SetPlcValue(string m_DeviceName, short m_Value)
        {
            if (!dicPlcBuffer.ContainsKey(m_DeviceName)) return;
            
            melPlcAccessor.WriteDeviceRandom2(dicPlcBuffer[m_DeviceName].Key, m_Value);
        }

        public int GetPlcDbValue(string m_DeviceName)
        {
            if (!dicPlcBuffer.ContainsKey(m_DeviceName)) return 0;

            string plc_device = dicPlcBuffer[m_DeviceName].Key;
            string high_device = plc_device.Substring(0, 1) + ((Convert.ToInt16(plc_device.Substring(1)) + 1).ToString()).PadLeft(5, '0'); //找LowDevice下一個Word並補滿5個數字
            string low_device = m_DeviceName;
            short[] values;
            melPlcAccessor.ReadDeviceBlock(high_device, 1, out values); //直接從PLC讀取，不從Dictionary獲得
            
            string high_value = values[0].ToString("X").PadLeft(4, '0');
            string low_value = GetPlcValue(low_device).ToString("X").PadLeft(4, '0');

            string value = "0x" + high_value + low_value;
            
            return Convert.ToInt32(value, 16);
        }
        
        public void SetPlcDbValue(string m_DeviceName, int m_Value)
        {
            if (!dicPlcBuffer.ContainsKey(m_DeviceName)) return;

            string plc_device = dicPlcBuffer[m_DeviceName].Key;
            string high_device = plc_device.Substring(0, 1) + ((Convert.ToInt16(plc_device.Substring(1)) + 1).ToString()).PadLeft(5,'0');//找LowDevice下一個Word並補滿5個數字

            //用寫入位址從Dictionary 找到 High Deveice的Device Name(XML需設定High Deveice資料)
            KeyValuePair<String, KeyValuePair<string, int>> tmp = dicPlcBuffer.FirstOrDefault(t => t.Value.Key == high_device);
            if (tmp.Key == null) return;

            high_device = tmp.Key;
            string low_device = m_DeviceName;

            long high_value = m_Value & 0xFFFF0000;
            high_value = high_value >> (4 * 4);

            long low_value = m_Value & 0xFFFF;
            
            melPlcAccessor.WriteDeviceRandom2(dicPlcBuffer[high_device].Key, (short)high_value);
            melPlcAccessor.WriteDeviceRandom2(dicPlcBuffer[low_device].Key, (short)low_value);
        }

        public int this[string m_DeviceName]
        {
            get
            {
                if (!dicPlcValueType.ContainsKey(m_DeviceName)) return 0;
                if (dicPlcValueType[m_DeviceName] == PlcValueType.pvtDoubleWord)
                {
                    return GetPlcDbValue(m_DeviceName);
                }
                else
                {
                    return GetPlcValue(m_DeviceName);
                }
            }
            set
            {
                if (!dicPlcValueType.ContainsKey(m_DeviceName)) return;
                if (dicPlcValueType[m_DeviceName] == PlcValueType.pvtDoubleWord)
                {
                    SetPlcDbValue(m_DeviceName, value);
                }
                else
                {
                    SetPlcValue(m_DeviceName, (short)value);
                }
            }
        }

        public Dictionary<string, KeyValuePair<string, int>> GetPlcMap()
        {
            return dicPlcBuffer;
        }

        public string GetPlcMap(string m_DeviceName)
        {
            if (!dicPlcBuffer.ContainsKey(m_DeviceName)) return "InVaild Address";
            return dicPlcBuffer[m_DeviceName].Key;
        }

        private void writeLog(string m_Log)
        {
            if (flLog != null)
            {
                 flLog.WriteLog("PlcHandler", m_Log);
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

                //melPlcAccessor.Close();
                //dicPlcBuffer = null;

                //if (thExecute != null && thExecute.IsAlive)
                //    thExecute.Abort();

                bDisponse = true;

                melPlcAccessor.Close();
                melPlcAccessor = null;
                
                disposedValue = true;
            }
        }

        // TODO: 僅當上方的 Dispose(bool disposing) 具有會釋放 Unmanaged 資源的程式碼時，才覆寫完成項。
        // ~PlcHandler() {
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

#region Sample
//    //plc_handler["PLC_IN_L1010"] = 1;
//    //plc_handler["PLC_IN_L1011"] = 0;
#endregion
