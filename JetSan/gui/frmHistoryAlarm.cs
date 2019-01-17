using System;
using System.Data;
using System.Windows.Forms;

namespace HyTemplate.gui
{
    public partial class frmHistoryAlarm : Form
    {
        DBControl db;
        private EventClient ecClient;

        public frmHistoryAlarm()
        {
            InitializeComponent();
            db = new DBControl();
            ecClient = new EventClient(this);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Query();
        }

        private void Query()
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
            DataTable DT;
            string strSQL = "SELECT * FROM HistoryAlarm WHERE Start_Time BETWEEN '" + dtStart.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'" + "AND '" + dtEnd.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            string err = db.funSQL(strSQL, out DT); //從DB 取得該區段時間的 Data Table
            dataGridView1.DataSource = DT;
            DT = null;

            if (err != "")
            {
                TEvent data = new TEvent();
                data.MessageName = ProxyMessage.MSG_WRITE_LOG;
                data.EventData["DB_Fail : HistoryAlarm_Query "] = err;

                ecClient.SendMessage(data);
            }
        }
    }
}
