namespace HyTemplate.gui
{
    partial class FrmOxyPlot
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.plotViewPLCRaw = new OxyPlot.WindowsForms.PlotView();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dateTimePicker4 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.comBoxVauleType = new System.Windows.Forms.ComboBox();
            this.chkList = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // plotViewPLCRaw
            // 
            this.plotViewPLCRaw.Location = new System.Drawing.Point(185, 126);
            this.plotViewPLCRaw.Margin = new System.Windows.Forms.Padding(2);
            this.plotViewPLCRaw.Name = "plotViewPLCRaw";
            this.plotViewPLCRaw.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotViewPLCRaw.Size = new System.Drawing.Size(1123, 534);
            this.plotViewPLCRaw.TabIndex = 0;
            this.plotViewPLCRaw.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotViewPLCRaw.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotViewPLCRaw.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(725, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 17);
            this.label2.TabIndex = 22;
            this.label2.Text = "End Date Time :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(205, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 17);
            this.label1.TabIndex = 21;
            this.label1.Text = "Start Date Time :";
            // 
            // btnSearch
            // 
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSearch.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSearch.Location = new System.Drawing.Point(1334, 85);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(120, 27);
            this.btnSearch.TabIndex = 20;
            this.btnSearch.Text = "Query";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dateTimePicker4
            // 
            this.dateTimePicker4.CustomFormat = "HH : mm : ss";
            this.dateTimePicker4.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dateTimePicker4.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker4.Location = new System.Drawing.Point(998, 88);
            this.dateTimePicker4.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePicker4.Name = "dateTimePicker4";
            this.dateTimePicker4.ShowUpDown = true;
            this.dateTimePicker4.Size = new System.Drawing.Size(94, 25);
            this.dateTimePicker4.TabIndex = 19;
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.CustomFormat = "HH : mm : ss";
            this.dateTimePicker3.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker3.Location = new System.Drawing.Point(530, 90);
            this.dateTimePicker3.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.ShowUpDown = true;
            this.dateTimePicker3.Size = new System.Drawing.Size(94, 25);
            this.dateTimePicker3.TabIndex = 18;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(857, 88);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(121, 25);
            this.dateTimePicker2.TabIndex = 17;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(353, 90);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(121, 25);
            this.dateTimePicker1.TabIndex = 16;
            // 
            // comBoxVauleType
            // 
            this.comBoxVauleType.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.comBoxVauleType.FormattingEnabled = true;
            this.comBoxVauleType.Items.AddRange(new object[] {
            "HeaterData",
            "MfcFlowData",
            "PowerData",
            "PressureData"});
            this.comBoxVauleType.Location = new System.Drawing.Point(1126, 88);
            this.comBoxVauleType.Name = "comBoxVauleType";
            this.comBoxVauleType.Size = new System.Drawing.Size(118, 25);
            this.comBoxVauleType.TabIndex = 23;
            // 
            // chkList
            // 
            this.chkList.CheckOnClick = true;
            this.chkList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkList.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.chkList.FormattingEnabled = true;
            this.chkList.Location = new System.Drawing.Point(1334, 140);
            this.chkList.Name = "chkList";
            this.chkList.Size = new System.Drawing.Size(120, 344);
            this.chkList.TabIndex = 24;
            this.chkList.Visible = false;
            this.chkList.SelectedValueChanged += new System.EventHandler(this.chkList_SelectedValueChanged);
            // 
            // FrmOxyPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1466, 710);
            this.Controls.Add(this.chkList);
            this.Controls.Add(this.comBoxVauleType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dateTimePicker4);
            this.Controls.Add(this.dateTimePicker3);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.plotViewPLCRaw);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmOxyPlot";
            this.Text = "FrmOxyPlot";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OxyPlot.WindowsForms.PlotView plotViewPLCRaw;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker dateTimePicker4;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ComboBox comBoxVauleType;
        private System.Windows.Forms.CheckedListBox chkList;
    }
}