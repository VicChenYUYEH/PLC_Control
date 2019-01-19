using System;
using System.Data;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class FrmHistoryAlarm : Form
    {
        private EventClient ecClient;
        private RdEqKernel rdKernel;

        public FrmHistoryAlarm(RdEqKernel m_Kernel)
        {
            InitializeComponent();
            ecClient = new EventClient(this);
            rdKernel = m_Kernel;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            query();
        }

        private void query()
        {
            string t_sDate, t_sTime, t_eDate, t_eTime;
            t_sDate = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            t_sTime = dateTimePicker3.Value.ToString("HH:mm");

            string s_datetime = t_sDate + " " + t_sTime;// 設定的起始時間

            t_eDate = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            t_eTime = dateTimePicker4.Value.ToString("HH:mm");
            string e_datetime = t_eDate + " " + t_eTime;// 設定的截止時間

            DateTime dtStart = DateTime.Parse(s_datetime);
            DateTime dtEnd = DateTime.Parse(e_datetime);
            DataTable dt;
            string strSQL = "SELECT * FROM HistoryAlarm WHERE Start_Time BETWEEN '" + dtStart.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "AND '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            string err = rdKernel.dDb.funSQL(strSQL, out dt); //從DB 取得該區段時間的 Data Table
            dataGridView1.DataSource = dt;
            dt = null;

            if (err != "")
            {
                rdKernel.WriteDebugLog("DB_Fail : HistoryAlarm_Query => " + err);
            }
        }
    }
}
