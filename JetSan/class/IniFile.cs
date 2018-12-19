using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.InteropServices;

namespace HyTemplate
{
    class IniFile
    {
        private string sIniFile = "";
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNames", SetLastError = true)]
        private static extern uint GetPrivateProfileSectionNames(IntPtr retVal, uint size, string filePath);

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSection", SetLastError = true)]
        private static extern uint GetPrivateProfileSection(string section, IntPtr retVal, uint size, string filePath);

        public IniFile(string m_IniFile)
        {
            sIniFile = m_IniFile;
        }

        public void setIniFile(string m_IniFile)
        {
            sIniFile = m_IniFile;
        }

        public bool setValue(string m_Section, string m_Key, string m_Value)
        {
            if (sIniFile.Trim() == "") return false;

            if ( WritePrivateProfileString(m_Section, m_Key, m_Value, sIniFile) == 0 )
            {
                return false;
            }
            return true;
        }

        public string getValue(string m_Section, string m_Key)
        {
            if (sIniFile.Trim() == "") return "";

            StringBuilder temp = new StringBuilder(255);
            if ( GetPrivateProfileString(m_Section, m_Key, "", temp, 255, sIniFile) == 0 )
            {
                return "";
            }
            return temp.ToString();
        }

        #region 取得檔案中所有Section名稱
        public List<string> getSectionNames()
        {
            if (sIniFile.Trim() == "") return null;

            uint MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER);

            uint bytesReturned = GetPrivateProfileSectionNames(pReturnedString, MAX_BUFFER, sIniFile);
            string[] sections = IntPtrToStringArray(pReturnedString, bytesReturned);

            List<string> result = new List<string>();
            for ( int index = 0; index < sections.Length; index++)
            {
                result.Add(sections[index]);
            }
            return result;
        }
        #endregion

        #region 取得Section中的所有資料
        public string[] getSectionValues(string m_Section)
        {
            if (sIniFile.Trim() == "") return null;

            uint MAX_BUFFER = 32767;
            IntPtr pReturnedString = Marshal.AllocCoTaskMem((int)MAX_BUFFER);
            uint bytesReturned = GetPrivateProfileSection(m_Section, pReturnedString, MAX_BUFFER, sIniFile);
            return IntPtrToStringArray(pReturnedString, bytesReturned);
        }
        #endregion

        //指標資料轉字串陣列
        private string[] IntPtrToStringArray(IntPtr pReturnedString, uint bytesReturned)
        {
            //use of Substring below removes terminating null for split
            if (bytesReturned == 0)
            {
                Marshal.FreeCoTaskMem(pReturnedString);
                return null;
            }
            string local = Marshal.PtrToStringAnsi(pReturnedString, (int)bytesReturned).ToString();
            Marshal.FreeCoTaskMem(pReturnedString);
            return local.Substring(0, local.Length - 1).Split('\0');
        }
    }
    
}

#region Sample
//    CIniFile ini = new CIniFile("C:\\ORBKWsmcu\\config\\ow-smcu.ini");
//    List<string> sections;
//    sections = ini.getSectionNames();


//    string[] keys = ini.getSectionValues(sections[6]);
#endregion
