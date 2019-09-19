namespace Splash.Views
{
    partial class SelectForm
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
            this.dtStart = new System.Windows.Forms.DateTimePicker();
            this.cbRefresh = new System.Windows.Forms.CheckBox();
            this.dtEnd = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbProfitsLow = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbProfitsHigh = new System.Windows.Forms.TextBox();
            this.tbWarning = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbSound = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // dtStart
            // 
            this.dtStart.Location = new System.Drawing.Point(124, 57);
            this.dtStart.Name = "dtStart";
            this.dtStart.Size = new System.Drawing.Size(200, 21);
            this.dtStart.TabIndex = 0;
            // 
            // cbRefresh
            // 
            this.cbRefresh.AutoSize = true;
            this.cbRefresh.Location = new System.Drawing.Point(47, 23);
            this.cbRefresh.Name = "cbRefresh";
            this.cbRefresh.Size = new System.Drawing.Size(72, 16);
            this.cbRefresh.TabIndex = 1;
            this.cbRefresh.Text = "刷新结果";
            this.cbRefresh.UseVisualStyleBackColor = true;
            // 
            // dtEnd
            // 
            this.dtEnd.Location = new System.Drawing.Point(370, 57);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Size = new System.Drawing.Size(200, 21);
            this.dtEnd.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(331, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "结束";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "日期开始";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "盈利区间(%)";
            // 
            // tbProfitsLow
            // 
            this.tbProfitsLow.Location = new System.Drawing.Point(124, 99);
            this.tbProfitsLow.Name = "tbProfitsLow";
            this.tbProfitsLow.Size = new System.Drawing.Size(45, 21);
            this.tbProfitsLow.TabIndex = 6;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(511, 203);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(174, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "-";
            // 
            // tbProfitsHigh
            // 
            this.tbProfitsHigh.Location = new System.Drawing.Point(191, 99);
            this.tbProfitsHigh.Name = "tbProfitsHigh";
            this.tbProfitsHigh.Size = new System.Drawing.Size(45, 21);
            this.tbProfitsHigh.TabIndex = 9;
            // 
            // tbWarning
            // 
            this.tbWarning.Location = new System.Drawing.Point(124, 138);
            this.tbWarning.Name = "tbWarning";
            this.tbWarning.Size = new System.Drawing.Size(45, 21);
            this.tbWarning.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 143);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "盈利提醒(%)";
            // 
            // cbSound
            // 
            this.cbSound.AutoSize = true;
            this.cbSound.Location = new System.Drawing.Point(136, 23);
            this.cbSound.Name = "cbSound";
            this.cbSound.Size = new System.Drawing.Size(48, 16);
            this.cbSound.TabIndex = 12;
            this.cbSound.Text = "声音";
            this.cbSound.UseVisualStyleBackColor = true;
            // 
            // SelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 238);
            this.Controls.Add(this.cbSound);
            this.Controls.Add(this.tbWarning);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbProfitsHigh);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbProfitsLow);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtEnd);
            this.Controls.Add(this.cbRefresh);
            this.Controls.Add(this.dtStart);
            this.Name = "SelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "筛选条件";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtStart;
        private System.Windows.Forms.CheckBox cbRefresh;
        private System.Windows.Forms.DateTimePicker dtEnd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbProfitsLow;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbProfitsHigh;
        private System.Windows.Forms.TextBox tbWarning;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbSound;
    }
}