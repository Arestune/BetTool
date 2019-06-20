namespace BetTool
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbOddsP1 = new System.Windows.Forms.TextBox();
            this.tbOddsP2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbResult = new System.Windows.Forms.Label();
            this.lbProfitRatio = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbPayMoney = new System.Windows.Forms.Label();
            this.tbBet1 = new System.Windows.Forms.TextBox();
            this.tbBet2 = new System.Windows.Forms.TextBox();
            this.btnBet1 = new System.Windows.Forms.Button();
            this.btnBet2 = new System.Windows.Forms.Button();
            this.lbGainMoney = new System.Windows.Forms.Label();
            this.lbProfitMoney = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 127);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "计算";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "平台1赔率";
            // 
            // tbOddsP1
            // 
            this.tbOddsP1.Location = new System.Drawing.Point(32, 31);
            this.tbOddsP1.Name = "tbOddsP1";
            this.tbOddsP1.Size = new System.Drawing.Size(100, 21);
            this.tbOddsP1.TabIndex = 2;
            // 
            // tbOddsP2
            // 
            this.tbOddsP2.Location = new System.Drawing.Point(32, 84);
            this.tbOddsP2.Name = "tbOddsP2";
            this.tbOddsP2.Size = new System.Drawing.Size(100, 21);
            this.tbOddsP2.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "平台2赔率";
            // 
            // lbResult
            // 
            this.lbResult.AutoSize = true;
            this.lbResult.Location = new System.Drawing.Point(30, 62);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(0, 12);
            this.lbResult.TabIndex = 5;
            // 
            // lbProfitRatio
            // 
            this.lbProfitRatio.AutoSize = true;
            this.lbProfitRatio.Location = new System.Drawing.Point(36, 172);
            this.lbProfitRatio.Name = "lbProfitRatio";
            this.lbProfitRatio.Size = new System.Drawing.Size(0, 12);
            this.lbProfitRatio.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(196, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "平台2投注";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(196, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "平台1投注";
            // 
            // lbPayMoney
            // 
            this.lbPayMoney.AutoSize = true;
            this.lbPayMoney.Location = new System.Drawing.Point(36, 199);
            this.lbPayMoney.Name = "lbPayMoney";
            this.lbPayMoney.Size = new System.Drawing.Size(0, 12);
            this.lbPayMoney.TabIndex = 11;
            // 
            // tbBet1
            // 
            this.tbBet1.Location = new System.Drawing.Point(198, 31);
            this.tbBet1.Name = "tbBet1";
            this.tbBet1.Size = new System.Drawing.Size(100, 21);
            this.tbBet1.TabIndex = 12;
            // 
            // tbBet2
            // 
            this.tbBet2.Location = new System.Drawing.Point(198, 84);
            this.tbBet2.Name = "tbBet2";
            this.tbBet2.Size = new System.Drawing.Size(100, 21);
            this.tbBet2.TabIndex = 13;
            // 
            // btnBet1
            // 
            this.btnBet1.Location = new System.Drawing.Point(304, 31);
            this.btnBet1.Name = "btnBet1";
            this.btnBet1.Size = new System.Drawing.Size(75, 23);
            this.btnBet1.TabIndex = 14;
            this.btnBet1.Text = "投注";
            this.btnBet1.UseVisualStyleBackColor = true;
            this.btnBet1.Click += new System.EventHandler(this.btnBet1_Click);
            // 
            // btnBet2
            // 
            this.btnBet2.Location = new System.Drawing.Point(304, 84);
            this.btnBet2.Name = "btnBet2";
            this.btnBet2.Size = new System.Drawing.Size(75, 23);
            this.btnBet2.TabIndex = 15;
            this.btnBet2.Text = "投注";
            this.btnBet2.UseVisualStyleBackColor = true;
            this.btnBet2.Click += new System.EventHandler(this.btnBet2_Click);
            // 
            // lbGainMoney
            // 
            this.lbGainMoney.AutoSize = true;
            this.lbGainMoney.Location = new System.Drawing.Point(36, 228);
            this.lbGainMoney.Name = "lbGainMoney";
            this.lbGainMoney.Size = new System.Drawing.Size(0, 12);
            this.lbGainMoney.TabIndex = 6;
            // 
            // lbProfitMoney
            // 
            this.lbProfitMoney.AutoSize = true;
            this.lbProfitMoney.Location = new System.Drawing.Point(36, 255);
            this.lbProfitMoney.Name = "lbProfitMoney";
            this.lbProfitMoney.Size = new System.Drawing.Size(0, 12);
            this.lbProfitMoney.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 281);
            this.Controls.Add(this.btnBet2);
            this.Controls.Add(this.btnBet1);
            this.Controls.Add(this.tbBet2);
            this.Controls.Add(this.tbBet1);
            this.Controls.Add(this.lbProfitMoney);
            this.Controls.Add(this.lbPayMoney);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbGainMoney);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbProfitRatio);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.tbOddsP2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbOddsP1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "打水辅助工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbOddsP1;
        private System.Windows.Forms.TextBox tbOddsP2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbResult;
        private System.Windows.Forms.Label lbProfitRatio;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbPayMoney;
        private System.Windows.Forms.TextBox tbBet1;
        private System.Windows.Forms.TextBox tbBet2;
        private System.Windows.Forms.Button btnBet1;
        private System.Windows.Forms.Button btnBet2;
        private System.Windows.Forms.Label lbGainMoney;
        private System.Windows.Forms.Label lbProfitMoney;
    }
}

