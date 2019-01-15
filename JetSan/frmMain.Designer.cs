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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.btnControl = new System.Windows.Forms.Button();
            this.btnGasView = new System.Windows.Forms.Button();
            this.Power_RunStop = new System.Windows.Forms.Button();
            this.Power_RunStart = new System.Windows.Forms.Button();
            this.btnProcView = new System.Windows.Forms.Button();
            this.btnDeviceConstant = new System.Windows.Forms.Button();
            this.btnMaintenance = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label90 = new System.Windows.Forms.Label();
            this.TxtRecipeName = new System.Windows.Forms.TextBox();
            this.statusPictureBox1 = new HyTemplate.components.StatusPictureBox();
            this.currentDateTime1 = new HyTemplate.components.CurrentDateTime();
            this.dataGrdAlarm = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusPictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrdAlarm)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHistoryLog
            // 
            this.btnHistoryLog.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHistoryLog.BackgroundImage")));
            this.btnHistoryLog.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnHistoryLog.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnHistoryLog.Location = new System.Drawing.Point(1609, 688);
            this.btnHistoryLog.Name = "btnHistoryLog";
            this.btnHistoryLog.Size = new System.Drawing.Size(80, 60);
            this.btnHistoryLog.TabIndex = 1;
            this.btnHistoryLog.UseVisualStyleBackColor = true;
            this.btnHistoryLog.Click += new System.EventHandler(this.btnHistoryLog_Click);
            // 
            // btnRecipe
            // 
            this.btnRecipe.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRecipe.BackgroundImage")));
            this.btnRecipe.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRecipe.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnRecipe.Location = new System.Drawing.Point(1609, 820);
            this.btnRecipe.Name = "btnRecipe";
            this.btnRecipe.Size = new System.Drawing.Size(80, 60);
            this.btnRecipe.TabIndex = 2;
            this.btnRecipe.UseVisualStyleBackColor = true;
            this.btnRecipe.Click += new System.EventHandler(this.btnRecipe_Click);
            // 
            // button3
            // 
            this.button3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button3.BackgroundImage")));
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button3.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button3.Location = new System.Drawing.Point(1609, 754);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 60);
            this.button3.TabIndex = 3;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.BtnAlarm_Click);
            // 
            // btnSysPara
            // 
            this.btnSysPara.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSysPara.BackgroundImage")));
            this.btnSysPara.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSysPara.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSysPara.Location = new System.Drawing.Point(1609, 886);
            this.btnSysPara.Name = "btnSysPara";
            this.btnSysPara.Size = new System.Drawing.Size(80, 60);
            this.btnSysPara.TabIndex = 5;
            this.btnSysPara.UseVisualStyleBackColor = true;
            this.btnSysPara.Click += new System.EventHandler(this.btnSysPara_Click);
            // 
            // btnOverview1
            // 
            this.btnOverview1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOverview1.BackgroundImage")));
            this.btnOverview1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnOverview1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnOverview1.Location = new System.Drawing.Point(1609, 292);
            this.btnOverview1.Name = "btnOverview1";
            this.btnOverview1.Size = new System.Drawing.Size(80, 60);
            this.btnOverview1.TabIndex = 6;
            this.btnOverview1.UseVisualStyleBackColor = true;
            this.btnOverview1.Click += new System.EventHandler(this.btnOverview_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLogin.BackgroundImage")));
            this.btnLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnLogin.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnLogin.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnLogin.Location = new System.Drawing.Point(1609, 952);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(80, 60);
            this.btnLogin.TabIndex = 7;
            this.btnLogin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button8.Location = new System.Drawing.Point(1256, 934);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(114, 73);
            this.button8.TabIndex = 12;
            this.button8.Text = "Alarm Reset";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1501, 906);
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
            this.lblaccount.Location = new System.Drawing.Point(1520, 151);
            this.lblaccount.Name = "lblaccount";
            this.lblaccount.Size = new System.Drawing.Size(133, 31);
            this.lblaccount.TabIndex = 16;
            this.lblaccount.Text = "Account  : ";
            this.lblaccount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblID
            // 
            this.lblID.AutoSize = true;
            this.lblID.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblID.Location = new System.Drawing.Point(1628, 151);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(78, 31);
            this.lblID.TabIndex = 17;
            this.lblID.Text = "None";
            this.lblID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPLC_Connect
            // 
            this.lblPLC_Connect.AutoSize = true;
            this.lblPLC_Connect.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblPLC_Connect.Location = new System.Drawing.Point(1520, 185);
            this.lblPLC_Connect.Name = "lblPLC_Connect";
            this.lblPLC_Connect.Size = new System.Drawing.Size(160, 31);
            this.lblPLC_Connect.TabIndex = 25;
            this.lblPLC_Connect.Text = "PLC Connect";
            this.lblPLC_Connect.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnControl
            // 
            this.btnControl.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnControl.BackgroundImage")));
            this.btnControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnControl.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnControl.Location = new System.Drawing.Point(1609, 358);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(80, 60);
            this.btnControl.TabIndex = 26;
            this.btnControl.UseVisualStyleBackColor = true;
            this.btnControl.Click += new System.EventHandler(this.Control_Click);
            // 
            // btnGasView
            // 
            this.btnGasView.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGasView.BackgroundImage")));
            this.btnGasView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnGasView.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnGasView.Location = new System.Drawing.Point(1609, 490);
            this.btnGasView.Name = "btnGasView";
            this.btnGasView.Size = new System.Drawing.Size(80, 60);
            this.btnGasView.TabIndex = 27;
            this.btnGasView.UseVisualStyleBackColor = true;
            this.btnGasView.Click += new System.EventHandler(this.btnGasView_Click);
            // 
            // Power_RunStop
            // 
            this.Power_RunStop.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Power_RunStop.Location = new System.Drawing.Point(1611, 221);
            this.Power_RunStop.Name = "Power_RunStop";
            this.Power_RunStop.Size = new System.Drawing.Size(78, 48);
            this.Power_RunStop.TabIndex = 2705;
            this.Power_RunStop.Text = "Run Stop";
            this.Power_RunStop.UseVisualStyleBackColor = true;
            // 
            // Power_RunStart
            // 
            this.Power_RunStart.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Power_RunStart.Location = new System.Drawing.Point(1520, 221);
            this.Power_RunStart.Name = "Power_RunStart";
            this.Power_RunStart.Size = new System.Drawing.Size(83, 48);
            this.Power_RunStart.TabIndex = 2704;
            this.Power_RunStart.Text = "Run Start";
            this.Power_RunStart.UseVisualStyleBackColor = true;
            // 
            // btnProcView
            // 
            this.btnProcView.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnProcView.BackgroundImage")));
            this.btnProcView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnProcView.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnProcView.Location = new System.Drawing.Point(1609, 424);
            this.btnProcView.Name = "btnProcView";
            this.btnProcView.Size = new System.Drawing.Size(80, 60);
            this.btnProcView.TabIndex = 28;
            this.btnProcView.UseVisualStyleBackColor = true;
            this.btnProcView.Click += new System.EventHandler(this.btnProcView_Click);
            // 
            // btnDeviceConstant
            // 
            this.btnDeviceConstant.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDeviceConstant.BackgroundImage")));
            this.btnDeviceConstant.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDeviceConstant.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnDeviceConstant.Location = new System.Drawing.Point(1609, 556);
            this.btnDeviceConstant.Name = "btnDeviceConstant";
            this.btnDeviceConstant.Size = new System.Drawing.Size(80, 60);
            this.btnDeviceConstant.TabIndex = 2706;
            this.btnDeviceConstant.UseVisualStyleBackColor = true;
            this.btnDeviceConstant.Click += new System.EventHandler(this.btnDeviceConstant_Click);
            // 
            // btnMaintenance
            // 
            this.btnMaintenance.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnMaintenance.BackgroundImage")));
            this.btnMaintenance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMaintenance.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnMaintenance.Location = new System.Drawing.Point(1609, 622);
            this.btnMaintenance.Name = "btnMaintenance";
            this.btnMaintenance.Size = new System.Drawing.Size(80, 60);
            this.btnMaintenance.TabIndex = 2707;
            this.btnMaintenance.UseVisualStyleBackColor = true;
            this.btnMaintenance.Click += new System.EventHandler(this.btnMaintenance_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(1520, 967);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 29);
            this.label1.TabIndex = 2708;
            this.label1.Text = "使用者";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(1520, 893);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 45);
            this.label2.TabIndex = 2709;
            this.label2.Text = "系統參數";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(1516, 825);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 48);
            this.label3.TabIndex = 2710;
            this.label3.Text = "製程配方";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label4.Location = new System.Drawing.Point(1520, 766);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 35);
            this.label4.TabIndex = 2711;
            this.label4.Text = "歷史異常";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(1520, 702);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 31);
            this.label5.TabIndex = 2712;
            this.label5.Text = "歷史資料";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(1520, 636);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 31);
            this.label6.TabIndex = 2713;
            this.label6.Text = "維護參數";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label7.Location = new System.Drawing.Point(1520, 570);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 31);
            this.label7.TabIndex = 2714;
            this.label7.Text = "機台參數";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label8.Location = new System.Drawing.Point(1520, 504);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 31);
            this.label8.TabIndex = 2715;
            this.label8.Text = "氣流系統圖";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label9.Location = new System.Drawing.Point(1521, 439);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 31);
            this.label9.TabIndex = 2716;
            this.label9.Text = "製程流程參數";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label10.Location = new System.Drawing.Point(1521, 373);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(88, 31);
            this.label10.TabIndex = 2717;
            this.label10.Text = "製程系統控制";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label11.Location = new System.Drawing.Point(1515, 307);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 31);
            this.label11.TabIndex = 2718;
            this.label11.Text = "製程系統圖";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label90
            // 
            this.label90.AutoSize = true;
            this.label90.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label90.ForeColor = System.Drawing.Color.Maroon;
            this.label90.Location = new System.Drawing.Point(1389, 934);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(170, 31);
            this.label90.TabIndex = 2725;
            this.label90.Text = "製程 匯入 配方";
            this.label90.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TxtRecipeName
            // 
            this.TxtRecipeName.BackColor = System.Drawing.SystemColors.Control;
            this.TxtRecipeName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtRecipeName.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.TxtRecipeName.ForeColor = System.Drawing.Color.DarkGreen;
            this.TxtRecipeName.Location = new System.Drawing.Point(1377, 967);
            this.TxtRecipeName.Name = "TxtRecipeName";
            this.TxtRecipeName.ReadOnly = true;
            this.TxtRecipeName.Size = new System.Drawing.Size(137, 39);
            this.TxtRecipeName.TabIndex = 2720;
            this.TxtRecipeName.Text = "N/A";
            this.TxtRecipeName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statusPictureBox1
            // 
            this.statusPictureBox1._CurrentStatus = false;
            this.statusPictureBox1._EqBase = null;
            this.statusPictureBox1._PlcDevice = "IsConnect";
            this.statusPictureBox1._Reverse = false;
            this.statusPictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.statusPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("statusPictureBox1.Image")));
            this.statusPictureBox1.Location = new System.Drawing.Point(1648, 183);
            this.statusPictureBox1.Name = "statusPictureBox1";
            this.statusPictureBox1.Size = new System.Drawing.Size(32, 32);
            this.statusPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.statusPictureBox1.TabIndex = 24;
            this.statusPictureBox1.TabStop = false;
            // 
            // currentDateTime1
            // 
            this.currentDateTime1.AutoSize = true;
            this.currentDateTime1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentDateTime1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.currentDateTime1.Location = new System.Drawing.Point(1520, 75);
            this.currentDateTime1.Margin = new System.Windows.Forms.Padding(4);
            this.currentDateTime1.Name = "currentDateTime1";
            this.currentDateTime1.Size = new System.Drawing.Size(173, 73);
            this.currentDateTime1.TabIndex = 0;
            // 
            // dataGrdAlarm
            // 
            this.dataGrdAlarm.AllowUserToAddRows = false;
            this.dataGrdAlarm.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGrdAlarm.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("標楷體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGrdAlarm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGrdAlarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrdAlarm.Location = new System.Drawing.Point(12, 920);
            this.dataGrdAlarm.Name = "dataGrdAlarm";
            this.dataGrdAlarm.ReadOnly = true;
            this.dataGrdAlarm.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.dataGrdAlarm.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.dataGrdAlarm.RowTemplate.Height = 24;
            this.dataGrdAlarm.RowTemplate.ReadOnly = true;
            this.dataGrdAlarm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGrdAlarm.Size = new System.Drawing.Size(1238, 92);
            this.dataGrdAlarm.TabIndex = 2726;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1703, 1019);
            this.Controls.Add(this.label90);
            this.Controls.Add(this.dataGrdAlarm);
            this.Controls.Add(this.TxtRecipeName);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnMaintenance);
            this.Controls.Add(this.btnDeviceConstant);
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
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnOverview1);
            this.Controls.Add(this.btnSysPara);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnRecipe);
            this.Controls.Add(this.btnHistoryLog);
            this.Controls.Add(this.currentDateTime1);
            this.Font = new System.Drawing.Font("標楷體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hongyu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusPictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrdAlarm)).EndInit();
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
        private System.Windows.Forms.Button btnDeviceConstant;
        private System.Windows.Forms.Button btnMaintenance;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        internal System.Windows.Forms.Label label90;
        internal System.Windows.Forms.TextBox TxtRecipeName;
        private System.Windows.Forms.DataGridView dataGrdAlarm;
    }
}

