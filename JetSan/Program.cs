using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace HyTemplate
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //獲取程式集Guid作為唯一標識
            Attribute flag = Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(GuidAttribute));
            string guid = ((GuidAttribute)flag).Value;
            Mutex _mutex = new Mutex(true, guid, out bool newApp);
            if (!newApp)//發現重複進程
            {
                MessageBox.Show("應用系統已開啟!");
                return;
            }
            _mutex.ReleaseMutex();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
