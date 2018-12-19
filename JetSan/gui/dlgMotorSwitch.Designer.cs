namespace HyTemplate.gui
{
    partial class dlgMotorSwitch
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.inputTextBox_MotoSpeed = new HyTemplate.components.InputTextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.No;
            this.button1.Location = new System.Drawing.Point(12, 56);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 39);
            this.button1.TabIndex = 0;
            this.button1.Text = "Back";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.button2.Location = new System.Drawing.Point(12, 11);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(162, 39);
            this.button2.TabIndex = 1;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.button3.Location = new System.Drawing.Point(96, 56);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(78, 39);
            this.button3.TabIndex = 2;
            this.button3.Text = "Forward";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(96, 101);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(78, 39);
            this.button4.TabIndex = 4;
            this.button4.Text = "JOG +";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button4_MouseDown);
            this.button4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button4_MouseUp);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 101);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(78, 39);
            this.button5.TabIndex = 3;
            this.button5.Text = "JOG -";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button5_MouseDown);
            this.button5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button5_MouseUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(12, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "速度";
            // 
            // inputTextBox_MotoSpeed
            // 
            this.inputTextBox_MotoSpeed._Division = 1;
            this.inputTextBox_MotoSpeed._DoubleWord = false;
            this.inputTextBox_MotoSpeed._EqBase = null;
            this.inputTextBox_MotoSpeed._FloatNumber = false;
            this.inputTextBox_MotoSpeed._MaxLimit = 3000D;
            this.inputTextBox_MotoSpeed._MinLimit = 0D;
            this.inputTextBox_MotoSpeed._Multiplication = 1;
            this.inputTextBox_MotoSpeed._NumberOnly = false;
            this.inputTextBox_MotoSpeed._PlcDevice = "";
            this.inputTextBox_MotoSpeed.BackColor = System.Drawing.Color.White;
            this.inputTextBox_MotoSpeed.Location = new System.Drawing.Point(55, 143);
            this.inputTextBox_MotoSpeed.Name = "inputTextBox_MotoSpeed";
            this.inputTextBox_MotoSpeed.Size = new System.Drawing.Size(119, 25);
            this.inputTextBox_MotoSpeed.TabIndex = 6;
            this.inputTextBox_MotoSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.inputTextBox_MotoSpeed.KeyUp += new System.Windows.Forms.KeyEventHandler(this.inputTextBox_MotoSpeed_KeyUp);
            // 
            // dlgMotorSwitch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(187, 185);
            this.Controls.Add(this.inputTextBox_MotoSpeed);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgMotorSwitch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Motor Switch";
            this.Shown += new System.EventHandler(this.dlgMotorSwitch_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label1;
        private components.InputTextBox inputTextBox_MotoSpeed;
    }
}