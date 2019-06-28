namespace Splash
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lbLabel = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.RichTextBox();
            this.list = new System.Windows.Forms.ListView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRun = new System.Windows.Forms.ToolStripButton();
            this.gbBetForm = new System.Windows.Forms.GroupBox();
            this.lbBetWin = new System.Windows.Forms.Label();
            this.lbGameName = new System.Windows.Forms.Label();
            this.panelBet2 = new System.Windows.Forms.Panel();
            this.lbHandicap2 = new System.Windows.Forms.Label();
            this.lbPName2 = new System.Windows.Forms.Label();
            this.lbOdds2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbBet2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lbWebName2 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnBet2 = new System.Windows.Forms.Button();
            this.panelBet1 = new System.Windows.Forms.Panel();
            this.lbHandicap1 = new System.Windows.Forms.Label();
            this.lbPName1 = new System.Windows.Forms.Label();
            this.lbOdds1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbBet1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lbWebName1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnBet1 = new System.Windows.Forms.Button();
            this.tbClearData = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbSleepTime = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.gbBetForm.SuspendLayout();
            this.panelBet2.SuspendLayout();
            this.panelBet1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLabel
            // 
            this.lbLabel.AutoSize = true;
            this.lbLabel.Location = new System.Drawing.Point(12, 42);
            this.lbLabel.Name = "lbLabel";
            this.lbLabel.Size = new System.Drawing.Size(67, 15);
            this.lbLabel.TabIndex = 3;
            this.lbLabel.Text = "打水结果";
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.BackColor = System.Drawing.Color.OldLace;
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox.Location = new System.Drawing.Point(1461, 393);
            this.textBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(383, 241);
            this.textBox.TabIndex = 4;
            this.textBox.Text = "";
            // 
            // list
            // 
            this.list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.list.BackColor = System.Drawing.Color.AliceBlue;
            this.list.BackgroundImageTiled = true;
            this.list.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.list.FullRowSelect = true;
            this.list.Location = new System.Drawing.Point(13, 68);
            this.list.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(1441, 566);
            this.list.TabIndex = 5;
            this.list.TileSize = new System.Drawing.Size(1, 1);
            this.list.UseCompatibleStateImageBehavior = false;
            this.list.View = System.Windows.Forms.View.Details;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 637);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 13, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1857, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRun});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1857, 27);
            this.toolStrip1.TabIndex = 13;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnRun
            // 
            this.btnRun.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRun.Image = global::Splash.Properties.Resources.run;
            this.btnRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(24, 24);
            this.btnRun.Text = "运行";
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // gbBetForm
            // 
            this.gbBetForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbBetForm.Controls.Add(this.lbBetWin);
            this.gbBetForm.Controls.Add(this.lbGameName);
            this.gbBetForm.Controls.Add(this.panelBet2);
            this.gbBetForm.Controls.Add(this.panelBet1);
            this.gbBetForm.Location = new System.Drawing.Point(1461, 58);
            this.gbBetForm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbBetForm.Name = "gbBetForm";
            this.gbBetForm.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gbBetForm.Size = new System.Drawing.Size(384, 302);
            this.gbBetForm.TabIndex = 14;
            this.gbBetForm.TabStop = false;
            this.gbBetForm.Text = "投注单";
            // 
            // lbBetWin
            // 
            this.lbBetWin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBetWin.BackColor = System.Drawing.Color.YellowGreen;
            this.lbBetWin.Location = new System.Drawing.Point(16, 268);
            this.lbBetWin.Name = "lbBetWin";
            this.lbBetWin.Size = new System.Drawing.Size(353, 28);
            this.lbBetWin.TabIndex = 11;
            this.lbBetWin.Text = "预估盈利：40 盈利率 3,4%";
            this.lbBetWin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbGameName
            // 
            this.lbGameName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbGameName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lbGameName.Location = new System.Drawing.Point(16, 20);
            this.lbGameName.Name = "lbGameName";
            this.lbGameName.Size = new System.Drawing.Size(353, 28);
            this.lbGameName.TabIndex = 0;
            this.lbGameName.Text = "英雄联盟(LOL夏季联赛)@";
            this.lbGameName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelBet2
            // 
            this.panelBet2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBet2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelBet2.Controls.Add(this.lbHandicap2);
            this.panelBet2.Controls.Add(this.lbPName2);
            this.panelBet2.Controls.Add(this.lbOdds2);
            this.panelBet2.Controls.Add(this.label9);
            this.panelBet2.Controls.Add(this.tbBet2);
            this.panelBet2.Controls.Add(this.label10);
            this.panelBet2.Controls.Add(this.lbWebName2);
            this.panelBet2.Controls.Add(this.label12);
            this.panelBet2.Controls.Add(this.btnBet2);
            this.panelBet2.Location = new System.Drawing.Point(207, 51);
            this.panelBet2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelBet2.Name = "panelBet2";
            this.panelBet2.Size = new System.Drawing.Size(163, 214);
            this.panelBet2.TabIndex = 10;
            // 
            // lbHandicap2
            // 
            this.lbHandicap2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbHandicap2.Location = new System.Drawing.Point(91, 78);
            this.lbHandicap2.Name = "lbHandicap2";
            this.lbHandicap2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbHandicap2.Size = new System.Drawing.Size(45, 26);
            this.lbHandicap2.TabIndex = 9;
            this.lbHandicap2.Text = "-44";
            this.lbHandicap2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbPName2
            // 
            this.lbPName2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPName2.Location = new System.Drawing.Point(3, 48);
            this.lbPName2.Name = "lbPName2";
            this.lbPName2.Size = new System.Drawing.Size(156, 25);
            this.lbPName2.TabIndex = 8;
            this.lbPName2.Text = "For The Dream";
            this.lbPName2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbOdds2
            // 
            this.lbOdds2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbOdds2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbOdds2.Location = new System.Drawing.Point(92, 109);
            this.lbOdds2.Name = "lbOdds2";
            this.lbOdds2.Size = new System.Drawing.Size(45, 22);
            this.lbOdds2.TabIndex = 7;
            this.lbOdds2.Text = "3.33";
            this.lbOdds2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(23, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 15);
            this.label9.TabIndex = 5;
            this.label9.Text = "盘口：";
            // 
            // tbBet2
            // 
            this.tbBet2.Location = new System.Drawing.Point(88, 138);
            this.tbBet2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbBet2.Name = "tbBet2";
            this.tbBet2.Size = new System.Drawing.Size(59, 25);
            this.tbBet2.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(23, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(0, 15);
            this.label10.TabIndex = 4;
            // 
            // lbWebName2
            // 
            this.lbWebName2.AutoSize = true;
            this.lbWebName2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbWebName2.Location = new System.Drawing.Point(57, 16);
            this.lbWebName2.Name = "lbWebName2";
            this.lbWebName2.Size = new System.Drawing.Size(39, 20);
            this.lbWebName2.TabIndex = 0;
            this.lbWebName2.Text = "亚博";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(23, 112);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 15);
            this.label12.TabIndex = 3;
            this.label12.Text = "赔率:";
            // 
            // btnBet2
            // 
            this.btnBet2.Location = new System.Drawing.Point(11, 138);
            this.btnBet2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBet2.Name = "btnBet2";
            this.btnBet2.Size = new System.Drawing.Size(61, 25);
            this.btnBet2.TabIndex = 1;
            this.btnBet2.Text = "投注";
            this.btnBet2.UseVisualStyleBackColor = true;
            this.btnBet2.Click += new System.EventHandler(this.btnBet2_Click);
            // 
            // panelBet1
            // 
            this.panelBet1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelBet1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelBet1.Controls.Add(this.lbHandicap1);
            this.panelBet1.Controls.Add(this.lbPName1);
            this.panelBet1.Controls.Add(this.lbOdds1);
            this.panelBet1.Controls.Add(this.label6);
            this.panelBet1.Controls.Add(this.tbBet1);
            this.panelBet1.Controls.Add(this.label5);
            this.panelBet1.Controls.Add(this.lbWebName1);
            this.panelBet1.Controls.Add(this.label4);
            this.panelBet1.Controls.Add(this.btnBet1);
            this.panelBet1.Location = new System.Drawing.Point(16, 51);
            this.panelBet1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelBet1.Name = "panelBet1";
            this.panelBet1.Size = new System.Drawing.Size(163, 214);
            this.panelBet1.TabIndex = 5;
            // 
            // lbHandicap1
            // 
            this.lbHandicap1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbHandicap1.Location = new System.Drawing.Point(91, 78);
            this.lbHandicap1.Name = "lbHandicap1";
            this.lbHandicap1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbHandicap1.Size = new System.Drawing.Size(45, 25);
            this.lbHandicap1.TabIndex = 9;
            this.lbHandicap1.Text = "-44";
            this.lbHandicap1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbPName1
            // 
            this.lbPName1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPName1.Location = new System.Drawing.Point(3, 44);
            this.lbPName1.Name = "lbPName1";
            this.lbPName1.Size = new System.Drawing.Size(156, 29);
            this.lbPName1.TabIndex = 8;
            this.lbPName1.Text = "For The Dream";
            this.lbPName1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbOdds1
            // 
            this.lbOdds1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbOdds1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbOdds1.Location = new System.Drawing.Point(92, 109);
            this.lbOdds1.Name = "lbOdds1";
            this.lbOdds1.Size = new System.Drawing.Size(45, 22);
            this.lbOdds1.TabIndex = 7;
            this.lbOdds1.Text = "3.33";
            this.lbOdds1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 15);
            this.label6.TabIndex = 5;
            this.label6.Text = "盘口：";
            // 
            // tbBet1
            // 
            this.tbBet1.Location = new System.Drawing.Point(88, 138);
            this.tbBet1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbBet1.Name = "tbBet1";
            this.tbBet1.Size = new System.Drawing.Size(59, 25);
            this.tbBet1.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 15);
            this.label5.TabIndex = 4;
            // 
            // lbWebName1
            // 
            this.lbWebName1.AutoSize = true;
            this.lbWebName1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbWebName1.Location = new System.Drawing.Point(57, 16);
            this.lbWebName1.Name = "lbWebName1";
            this.lbWebName1.Size = new System.Drawing.Size(39, 20);
            this.lbWebName1.TabIndex = 0;
            this.lbWebName1.Text = "亚博";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "赔率:";
            // 
            // btnBet1
            // 
            this.btnBet1.Location = new System.Drawing.Point(11, 138);
            this.btnBet1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBet1.Name = "btnBet1";
            this.btnBet1.Size = new System.Drawing.Size(61, 25);
            this.btnBet1.TabIndex = 1;
            this.btnBet1.Text = "投注";
            this.btnBet1.UseVisualStyleBackColor = true;
            this.btnBet1.Click += new System.EventHandler(this.btnBet1_Click);
            // 
            // tbClearData
            // 
            this.tbClearData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbClearData.Location = new System.Drawing.Point(1338, 30);
            this.tbClearData.Name = "tbClearData";
            this.tbClearData.Size = new System.Drawing.Size(116, 33);
            this.tbClearData.TabIndex = 15;
            this.tbClearData.Text = "清空数据";
            this.tbClearData.UseVisualStyleBackColor = true;
            this.tbClearData.Click += new System.EventHandler(this.tbClearData_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1460, 370);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 16;
            this.label1.Text = "日志窗口";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1192, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 15);
            this.label2.TabIndex = 17;
            this.label2.Text = "定时器(秒)";
            // 
            // tbSleepTime
            // 
            this.tbSleepTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSleepTime.Location = new System.Drawing.Point(1273, 35);
            this.tbSleepTime.Name = "tbSleepTime";
            this.tbSleepTime.Size = new System.Drawing.Size(48, 25);
            this.tbSleepTime.TabIndex = 18;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1857, 659);
            this.Controls.Add(this.tbSleepTime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbClearData);
            this.Controls.Add(this.gbBetForm);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.list);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.lbLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Splash v1.2";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbBetForm.ResumeLayout(false);
            this.panelBet2.ResumeLayout(false);
            this.panelBet2.PerformLayout();
            this.panelBet1.ResumeLayout(false);
            this.panelBet1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbLabel;
        private System.Windows.Forms.RichTextBox textBox;
        private System.Windows.Forms.ListView list;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnRun;
        private System.Windows.Forms.GroupBox gbBetForm;
        private System.Windows.Forms.TextBox tbBet1;
        private System.Windows.Forms.Button btnBet1;
        private System.Windows.Forms.Label lbWebName1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelBet1;
        private System.Windows.Forms.Label lbHandicap1;
        private System.Windows.Forms.Label lbPName1;
        private System.Windows.Forms.Label lbOdds1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panelBet2;
        private System.Windows.Forms.Label lbHandicap2;
        private System.Windows.Forms.Label lbPName2;
        private System.Windows.Forms.Label lbOdds2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbBet2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbWebName2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnBet2;
        private System.Windows.Forms.Label lbBetWin;
        private System.Windows.Forms.Label lbGameName;
        private System.Windows.Forms.Button tbClearData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbSleepTime;
    }
}