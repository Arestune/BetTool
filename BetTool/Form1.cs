using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BetTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Reset()
        {
            lbProfitRatio.Text = "";
            lbPayMoney.Text = "";
            lbGainMoney.Text = "";
            lbProfitMoney.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Reset();

            if(tbOddsP1.Text == "" || tbOddsP2.Text == "")
            {
                 MessageBox.Show("请输入正确的赔率！");
                 return;
            }
            
            float fOdds1 = 0.0f;
            float fOdds2 = 0.0f;
            if (!float.TryParse(tbOddsP1.Text, out fOdds1))
            {
                MessageBox.Show("平台1赔率格式不正确！");
                return;
            } 
            if (!float.TryParse(tbOddsP2.Text, out fOdds2))
            {
                MessageBox.Show("平台2赔率格式不正确！");
                return;
            }

            float fProfitPer = (fOdds1 * fOdds2) / (fOdds1 + fOdds2);
            float fBet1 = 0.0f;
            float fBet2 = 0.0f;
            if (!float.TryParse(tbBet1.Text, out fBet1))
            {
                fBet1 = 0;
            } 
            if (!float.TryParse(tbBet2.Text, out fBet2))
            {
                fBet2 = 0;
            } 
            if(fProfitPer > 1.0f)
            {
                lbProfitRatio.Text = string.Format("恭喜您，可以打水！收益率为{0}%", ((fProfitPer - 1.0f) * 100).ToString("F2")); 
                if(fBet1 > 0 || fBet2 > 0)
                {
                    fBet2 = fBet1 * fOdds1 / fOdds2;
                    tbBet2.Text = fBet2.ToString("F1");
                    lbPayMoney.Text = string.Format("您的投注额为:￥{0}+￥{1}=￥{2}", fBet1.ToString("F1"), fBet2.ToString("F1"), (fBet1 + fBet2).ToString("F1"));
                    lbGainMoney.Text = string.Format("预估到帐:￥{0}", (fBet1 * fOdds1).ToString("F1"));
                    lbProfitMoney.Text = string.Format("收益额为:￥{0}", (fBet1 * fOdds1 - (fBet1 + fBet2)).ToString("F1"));
                }
            }
            else 
            {
                lbProfitRatio.Text = "很抱歉，无法打水！";
            }
        }

        private void btnBet1_Click(object sender, EventArgs e)
        {
            float fBet1 = 0.0f;
            if (!float.TryParse(tbBet1.Text, out fBet1))
            {
                MessageBox.Show("平台1投注格式不正确！");
                return;
            } 

            float fOdds1 = 0.0f;
            float fOdds2 = 0.0f;
            if (!float.TryParse(tbOddsP1.Text, out fOdds1))
            {
                MessageBox.Show("平台1赔率格式不正确！");
                return;
            } 
            if (!float.TryParse(tbOddsP2.Text, out fOdds2))
            {
                MessageBox.Show("平台2赔率格式不正确！");
                return;
            }

            tbBet2.Text = ((fOdds1 * fBet1 / fOdds2)).ToString("F1");
        }

        private void btnBet2_Click(object sender, EventArgs e)
        {
            float fBet2 = 0.0f;
            if (!float.TryParse(tbBet2.Text, out fBet2))
            {
                MessageBox.Show("平台1投注格式不正确！");
                return;
            }

            float fOdds1 = 0.0f;
            float fOdds2 = 0.0f;
            if (!float.TryParse(tbOddsP1.Text, out fOdds1))
            {
                MessageBox.Show("平台1赔率格式不正确！");
                return;
            }
            if (!float.TryParse(tbOddsP2.Text, out fOdds2))
            {
                MessageBox.Show("平台2赔率格式不正确！");
                return;
            }

            tbBet1.Text = ((fOdds2 * fBet2 / fOdds1)).ToString("F1");
        }
    }
}
