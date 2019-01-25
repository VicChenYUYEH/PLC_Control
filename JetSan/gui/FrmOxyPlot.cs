using OxyPlot;
using System;
using System.Windows.Forms;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HyTemplate.gui
{
    public partial class FrmOxyPlot : Form
    {
        private PlotModel plotModel;
        private RdEqKernel rdKernel;
        public FrmOxyPlot(RdEqKernel m_Kernel)
        {
            InitializeComponent();
            plotModel = new PlotModel();
            rdKernel = m_Kernel;
            setUpModel();
            comBoxVauleType.SelectedIndex = 0;
        }
        public class Data
        {
            public static List<PLCRawData> GetData(DataTable m_DT)
            {
                var rawdatas = new List<PLCRawData>();
                for (int i = 0; i < m_DT.Rows.Count; i++)
                {
                    object dateTime = m_DT.Rows[i]["Insert_Time"];
                    List<int> value = new List<int> { };
                    for (int j = 0; j < m_DT.Columns.Count - 1; j++)
                    {
                        int val = Convert.ToInt32(m_DT.Rows[i][j]);
                        value.Add(val);
                        rawdatas.Add(new PLCRawData() { DetectorId = j, DateTime = dateTime, Value = value[j] });
                    }
                }
                return rawdatas;
            }
        }

        public class PLCRawData
        {
            public int DetectorId { get; set; }
            public object DateTime { get; set; }
            public int Value { get; set; }
        }

        #region Method
        private void setUpModel()
        {
            plotModel.LegendTitle = "PLC Raw Data Trend";
            plotModel.LegendOrientation = LegendOrientation.Horizontal;
            plotModel.LegendPlacement = LegendPlacement.Outside;
            plotModel.LegendPosition = LegendPosition.TopRight;
            plotModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            plotModel.LegendBorder = OxyColors.Black;

        }

        private void updateoxyplot()
        {
            chkList.Items.Clear();
            plotViewPLCRaw.Model = null;
            plotModel.Axes.Clear();
            plotModel.Series.Clear();
            var dateAxis = new DateTimeAxis()
            {
                StringFormat = "hh:mm:ss",
                IntervalType = DateTimeIntervalType.Seconds,
                MajorGridlineStyle = LineStyle.DashDotDot,
                MinorGridlineStyle = LineStyle.Dot
            };
            var valueAxis = new LinearAxis()
            {
                MajorGridlineStyle = LineStyle.DashDotDot,
                MinorGridlineStyle = LineStyle.Dot,
                IntervalLength = 20
            };

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
            getPLCDataFromDB(comBoxVauleType.Text, dtStart, dtEnd, out dt);
            if (dt.Rows.Count > 0)
            {
                chkList.Visible = true;
                dateAxis.Minimum = DateTimeAxis.ToDouble(dt.Rows[0]["Insert_Time"]);
                dateAxis.Maximum = DateTimeAxis.ToDouble(Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1]["Insert_Time"]).AddMinutes(1));
                plotModel.Axes.Add(dateAxis);
                plotModel.Axes.Add(valueAxis);
                List<PLCRawData> rawDatas = Data.GetData(dt);

                var dataPerDetector = rawDatas.GroupBy(m => m.DetectorId).OrderBy(m => m.Key).ToList();
                foreach (var data in dataPerDetector)
                {
                    var lineSerie = new LineSeries
                    {
                        StrokeThickness = 2,
                        MarkerSize = 3,
                        MarkerType = MarkerType.Circle,
                        Title = string.Format("{0}", dt.Columns[data.Key]),
                        CanTrackerInterpolatePoints = false,
                        Smooth = false,
                    };

                    data.ToList().ForEach(d => lineSerie.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.DateTime), d.Value)));
                    plotModel.Series.Add(lineSerie);
                    chkList.Items.Add(lineSerie.Title);
                }
                for(int i = 0; i < chkList.Items.Count; i++)
                {
                    chkList.SetItemChecked(i, true);
                }
                chkList.Refresh();
                plotViewPLCRaw.Model = plotModel;
                plotViewPLCRaw.Refresh();
            }
            else { MessageBox.Show("No Data Record!", "Warning", MessageBoxButtons.OK); }
        }
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            updateoxyplot();
        }

        private void getPLCDataFromDB(string m_TableName, DateTime m_Start, DateTime m_End, out DataTable m_DT)
        {
            string start ="'" + m_Start.ToString("yyyy/MM/dd HH:mm:ss.fff") + "'";
            string end = "'" + m_End.ToString("yyyy/MM/dd HH:mm:ss.fff") + "'";
            string strSQL = "SELECT * FROM " + m_TableName + " WHERE Insert_Time BETWEEN " + start + " AND " + end + " ORDER BY Insert_Time ASC";
            string err = rdKernel.dDb.FunSQL(strSQL, out m_DT);
            if (err != "")
            {
                rdKernel.flDebug.WriteLog("DB_fail", err);
            }
        }
        
        private void chkList_SelectedValueChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < plotModel.Series.Count; i++)
            {
                plotModel.Series[i].IsVisible = chkList.GetItemChecked(i);
            }
            plotViewPLCRaw.Refresh();
        }
        #endregion
    }
}
