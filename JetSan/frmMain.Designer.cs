namespace HyTemplate
{
    partial class frmMain
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnHistoryLog = new System.Windows.Forms.Button();
            this.btnRecipe = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.btnSysPara = new System.Windows.Forms.Button();
            this.btnOverview1 = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblaccount = new System.Windows.Forms.Label();
            this.lblID = new System.Windows.Forms.Label();
            this.lblPLC_Connect = new System.Windows.Forms.Label();
            this.statusPictureBox1 = new HyTemplate.components.StatusPictureBox();
            this.displayTextBox_Alarm = new HyTemplate.components.DisplayTextBox();
            this.currentDateTime1 = new HyTemplate.components.CurrentDateTime();
            this.btnControl = new System.Windows.Forms.Button();
            this.btnGasView = new System.Windows.Forms.Button();
            this.Power_RunStop = new System.Windows.Forms.Button();
            this.Power_RunStart = new System.Windows.Forms.Button();
            this.btnProcView = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusPictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHistoryLog
            // 
            this.btnHistoryLog.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnHistoryLog.Location = new System.Drawing.Point(828, 8);
            this.btnHistoryLog.Name = "btnHistoryLog";
            this.btnHistoryLog.Size = new System.Drawing.Size(130, 60);
            this.btnHistoryLog.TabIndex = 1;
            this.btnHistoryLog.Text = "歷史資料";
            this.btnHistoryLog.UseVisualStyleBackColor = true;
            this.btnHistoryLog.Click += new System.EventHandler(this.btnHistoryLog_Click);
            // 
            // btnRecipe
            // 
            this.btnRecipe.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRecipe.Location = new System.Drawing.Point(1100, 8);
            this.btnRecipe.Name = "btnRecipe";
            this.btnRecipe.Size = new System.Drawing.Size(130, 60);
            this.btnRecipe.TabIndex = 2;
            this.btnRecipe.Text = "Recipe";
            this.btnRecipe.UseVisualStyleBackColor = true;
            this.btnRecipe.Click += new System.EventHandler(this.btnRecipe_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button3.Location = new System.Drawing.Point(964, 8);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(130, 60);
            this.button3.TabIndex = 3;
            this.button3.Text = "歷史異常";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnSysPara
            // 
            this.btnSysPara.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSysPara.Location = new System.Drawing.Point(1236, 8);
            this.btnSysPara.Name = "btnSysPara";
            this.btnSysPara.Size = new System.Drawing.Size(130, 60);
            this.btnSysPara.TabIndex = 5;
            this.btnSysPara.Text = "系統參數";
            this.btnSysPara.UseVisualStyleBackColor = true;
            this.btnSysPara.Click += new System.EventHandler(this.btnSysPara_Click);
            // 
            // btnOverview1
            // 
            this.btnOverview1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOverview1.Location = new System.Drawing.Point(12, 8);
            this.btnOverview1.Name = "btnOverview1";
            this.btnOverview1.Size = new System.Drawing.Size(130, 60);
            this.btnOverview1.TabIndex = 6;
            this.btnOverview1.Text = "製程系統圖";
            this.btnOverview1.UseVisualStyleBackColor = true;
            this.btnOverview1.Click += new System.EventHandler(this.btnOverview_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnLogin.Location = new System.Drawing.Point(1380, 8);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(130, 60);
            this.btnLogin.TabIndex = 7;
            this.btnLogin.Text = "登入";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button8.Location = new System.Drawing.Point(1520, 1018);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(169, 27);
            this.button8.TabIndex = 12;
            this.button8.Text = "Alarm Reset";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 74);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1502, 940);
            this.panel1.TabIndex = 15;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::HyTemplate.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(1520, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(173, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // lblaccount
            // 
            this.lblaccount.AutoSize = true;
            this.lblaccount.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblaccount.Location = new System.Drawing.Point(1527, 149);
            this.lblaccount.Name = "lblaccount";
            this.lblaccount.Size = new System.Drawing.Size(90, 21);
            this.lblaccount.TabIndex = 16;
            this.lblaccount.Text = "Account  : ";
            this.lblaccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblID.Location = new System.Drawing.Point(1628, 149);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(52, 21);
            this.lblID.TabIndex = 17;
            this.lblID.Text = "None";
            this.lblID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPLC_Connect
            // 
            this.lblPLC_Connect.AutoSize = true;
            this.lblPLC_Connect.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblPLC_Connect.Location = new System.Drawing.Point(1527, 185);
            this.lblPLC_Connect.Name = "lblPLC_Connect";
            this.lblPLC_Connect.Size = new System.Drawing.Size(107, 21);
            this.lblPLC_Connect.TabIndex = 25;
            this.lblPLC_Connect.Text = "PLC Connect";
            this.lblPLC_Connect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // statusPictureBox1
            // 
            this.statusPictureBox1._CurrentStatus = false;
            this.statusPictureBox1._EqBase = null;
            this.statusPictureBox1._PlcDevice = "IsConnect";
            this.statusPictureBox1._Reverse = false;
            this.statusPictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.statusPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("statusPictureBox1.Image")));
            this.statusPictureBox1.Location = new System.Drawing.Point(1648, 185);
            this.statusPictureBox1.Name = "statusPictureBox1";
            this.statusPictureBox1.Size = new System.Drawing.Size(32, 32);
            this.statusPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.statusPictureBox1.TabIndex = 24;
            this.statusPictureBox1.TabStop = false;
            // 
            // displayTextBox_Alarm
            // 
            this.displayTextBox_Alarm._Division = ((short)(1));
            this.displayTextBox_Alarm._DoubleWord = false;
            this.displayTextBox_Alarm._EqBase = null;
            this.displayTextBox_Alarm._MaxLimit = 999D;
            this.displayTextBox_Alarm._MinLimit = 0D;
            this.displayTextBox_Alarm._Multiplication = ((short)(1));
            this.displayTextBox_Alarm._PlcDevice = "";
            this.displayTextBox_Alarm.BackColor = System.Drawing.SystemColors.InfoText;
            this.displayTextBox_Alarm.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.displayTextBox_Alarm.Location = new System.Drawing.Point(12, 1020);
            this.displayTextBox_Alarm.Name = "displayTextBox_Alarm";
            this.displayTextBox_Alarm.ReadOnly = true;
            this.displayTextBox_Alarm.Size = new System.Drawing.Size(1502, 27);
            this.displayTextBox_Alarm.TabIndex = 11;
            this.displayTextBox_Alarm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // currentDateTime1
            // 
            this.currentDateTime1.AutoSize = true;
            this.currentDateTime1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentDateTime1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.currentDateTime1.Location = new System.Drawing.Point(1520, 75);
            this.currentDateTime1.Margin = new System.Windows.Forms.Padding(4);
            this.currentDateTime1.Name = "currentDateTime1";
            this.currentDateTime1.Size = new System.Drawing.Size(173, 63);
            this.currentDateTime1.TabIndex = 0;
            // 
            // btnControl
            // 
            this.btnControl.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnControl.Location = new System.Drawing.Point(148, 8);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(130, 60);
            this.btnControl.TabIndex = 26;
            this.btnControl.Text = "製程系統控制";
            this.btnControl.UseVisualStyleBackColor = true;
            this.btnControl.Click += new System.EventHandler(this.Control_Click);
            // 
            // btnGasView
            // 
            this.btnGasView.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnGasView.Location = new System.Drawing.Point(420, 8);
            this.btnGasView.Name = "btnGasView";
            this.btnGasView.Size = new System.Drawing.Size(130, 60);
            this.btnGasView.TabIndex = 27;
            this.btnGasView.Text = "氣流系統圖";
            this.btnGasView.UseVisualStyleBackColor = true;
            this.btnGasView.Click += new System.EventHandler(this.btnGasView_Click);
            // 
            // Power_RunStop
            // 
            this.Power_RunStop.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Power_RunStop.Location = new System.Drawing.Point(1608, 232);
            this.Power_RunStop.Name = "Power_RunStop";
            this.Power_RunStop.Size = new System.Drawing.Size(88, 59);
            this.Power_RunStop.TabIndex = 2705;
            this.Power_RunStop.Text = "Run Stop";
            this.Power_RunStop.UseVisualStyleBackColor = true;
            // 
            // Power_RunStart
            // 
            this.Power_RunStart.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Power_RunStart.Location = new System.Drawing.Point(1520, 232);
            this.Power_RunStart.Name = "Power_RunStart";
            this.Power_RunStart.Size = new System.Drawing.Size(85, 59);
            this.Power_RunStart.TabIndex = 2704;
            this.Power_RunStart.Text = "Run Start";
            this.Power_RunStart.UseVisualStyleBackColor = true;
            // 
            // btnProcView
            // 
            this.btnProcView.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnProcView.Location = new System.Drawing.Point(284, 8);
            this.btnProcView.Name = "btnProcView";
            this.btnProcView.Size = new System.Drawing.Size(130, 60);
            this.btnProcView.TabIndex = 28;
            this.btnProcView.Text = "製程流程參數";
            this.btnProcView.UseVisualStyleBackColor = true;
            this.btnProcView.Click += new System.EventHandler(this.btnProcView_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(556, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 60);
            this.button1.TabIndex = 2706;
            this.button1.Text = "機台參數";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button2.Location = new System.Drawing.Point(692, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 60);
            this.button2.TabIndex = 2707;
            this.button2.Text = "維護參數";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1696, 1059);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGasView);
            this.Controls.Add(this.btnProcView);
            this.Controls.Add(this.Power_RunStop);
            this.Controls.Add(this.Power_RunStart);
            this.Controls.Add(this.btnControl);
            this.Controls.Add(this.lblPLC_Connect);
            this.Controls.Add(this.statusPictureBox1);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.lblaccount);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.displayTextBox_Alarm);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnOverview1);
            this.Controls.Add(this.btnSysPara);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnRecipe);
            this.Controls.Add(this.btnHistoryLog);
            this.Controls.Add(this.currentDateTime1);
            this.Font = new System.Drawing.Font("標楷體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hongyu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusPictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private components.CurrentDateTime currentDateTime1;
        private System.Windows.Forms.Button btnHistoryLog;
        private System.Windows.Forms.Button btnRecipe;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnSysPara;
        private System.Windows.Forms.Button btnOverview1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.PictureBox pictureBox1;
        private components.DisplayTextBox displayTextBox_Alarm;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblaccount;
        private System.Windows.Forms.Label lblID;
        private components.StatusPictureBox statusPictureBox1;
        private System.Windows.Forms.Label lblPLC_Connect;
        private System.Windows.Forms.Button btnControl;
        private System.Windows.Forms.Button btnGasView;
        internal System.Windows.Forms.Button Power_RunStop;
        internal System.Windows.Forms.Button Power_RunStart;
        private System.Windows.Forms.Button btnProcView;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

