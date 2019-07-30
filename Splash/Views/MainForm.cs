using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Splash.Tool;
using Splash.Item;
using Splash.Parser;
using Splash.Parser.Basketball;
using Splash.Parser.ESport;
namespace Splash.Views
{
    public partial class MainForm : Form
    {
        public string configDir = null;
        public List<BetWinPair> listPairs = new List<BetWinPair>();
        public delegate void ShowLogHandler(DebugLog log);
        public delegate void ShowResultHandler(List<BetWinPair> pairs);
        int time = 0;
        int index = 0;
        

        public MainForm()
        {
            InitializeComponent();
            this.Text = StaticData.SW_Version;
            configDir = System.IO.Directory.GetCurrentDirectory() + "\\Config";
            ShowBetPanel(false);
            tbSleepTime.Text = "30";
            this.list.Click += listView_Click;
            this.tbBet1.KeyPress += tb_Keypressed;
            this.tbBet2.KeyPress += tb_Keypressed;
            this.tbSleepTime.KeyPress += tb_Keypressed;
            ClearList();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            
           // ESportParser.LoadMainData();
           // TestWeb();
          //  return;
           
           // ESportParser.LoadMainData();
            Thread startThread = new Thread(Start);
            startThread.Name = "Main";
            startThread.IsBackground = true;
            startThread.Start();
            ShowLog("启动主抓取线程");
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SelectForm select = new SelectForm();
            select.Show();
        }
        private void tbClearData_Click(object sender, EventArgs e)
        {
            listPairs.Clear();
            ClearList();
        }
        public void tb_Keypressed(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar >57 ) && e.KeyChar != 8)
            {
                e.Handled = true;
            }
        }
        public void listView_Click(object sender, EventArgs e)
        {
            ListView list = (ListView)sender;
            int index =  Convert.ToInt32(list.SelectedItems[0].Text);
            if (index < 0) return;
            tbBet1.Text = "";
            tbBet2.Text = "";
            ShowBetPanel(true, listPairs[index]);
        }
        private void btnBet1_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(tbBet1.Text))
            {
                int bet1 = Convert.ToInt32(tbBet1.Text);
                double odds1 = Convert.ToDouble(lbOdds1.Text);
                double odds2 = Convert.ToDouble(lbOdds2.Text);
                tbBet2.Text =((int) (bet1 * odds1 / odds2)).ToString();
                tbBet1.Focus();
                tbBet1.SelectAll();
                ShowBetWin(odds1, odds2);
            }
        }
        private void btnBet2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbBet2.Text))
            {
                int bet2 = Convert.ToInt32(tbBet2.Text);
                double odds1 = Convert.ToDouble(lbOdds1.Text);
                double odds2 = Convert.ToDouble(lbOdds2.Text);
                tbBet1.Text = ((int)(bet2 * odds2 / odds1)).ToString();
                tbBet2.Focus();
                tbBet2.SelectAll();
                ShowBetWin(odds1, odds2);
            }
        }


      
        public void ShowBetPanel(bool bShow,BetWinPair pair = null)
        {
            lbGameName.Visible = bShow;
            panelBet1.Visible = bShow;
            panelBet2.Visible = bShow;
            lbBetWin.Visible = bShow;
            if(bShow && pair != null && pair.b1 != null && pair.b2 != null)
            {
                lbGameName.Text = pair.b1.gameName;
                if(!string.IsNullOrEmpty(pair.b1.leagueName1))
                {
                    lbGameName.Text = lbGameName.Text + "(" + pair.b1.leagueName1 + ")";
                }
                else if(!string.IsNullOrEmpty(pair.b2.leagueName1))
                {
                    lbGameName.Text = lbGameName.Text + "(" + pair.b2.leagueName1 + ")";
                }
                lbWebName1.Text = StaticData.webNames[(int)pair.b1.webID]+"-[BO"+pair.b1.bo+"]";
                lbWebName2.Text = StaticData.webNames[(int)pair.b2.webID]+"-[BO"+pair.b2.bo+"]";
                lbOdds1.Text = pair.odds1.ToString("F3");
                lbOdds2.Text = pair.odds2.ToString("F3");
                lbPName1.Text = pair.pAbbr1;
                lbPName2.Text = pair.pAbbr2;
                lbTime1.Text = pair.b1.time.ToString();
                lbTime2.Text = pair.b2.time.ToString();
                lbLimit1.Text = pair.betLimit1 == 0?  "N/A" : pair.betLimit1.ToString();
                lbLimit2.Text = pair.betLimit2 == 0 ? "N/A" : pair.betLimit2.ToString();
                ShowBetWin(pair.odds1, pair.odds2);
            }
        }
        private void ClearList()
        {
            index = 0;
            this.list.Clear();
            this.list.Columns.Add("Index", 60, HorizontalAlignment.Center); //索引
            this.list.Columns.Add("Web", 100, HorizontalAlignment.Center); //网站
            this.list.Columns.Add("Game", 100, HorizontalAlignment.Center); //电竞名称
            this.list.Columns.Add("League", 170, HorizontalAlignment.Center); //联赛
            this.list.Columns.Add("Team1", 200, HorizontalAlignment.Center); //队伍1
            this.list.Columns.Add("Team2", 200, HorizontalAlignment.Center); //队伍2
            this.list.Columns.Add("Odds1", 120, HorizontalAlignment.Center); //赔率1
            this.list.Columns.Add("Odds2", 120, HorizontalAlignment.Center); //赔率2
            this.list.Columns.Add("GameTime", 175, HorizontalAlignment.Center); //比赛开始时间
            this.list.Columns.Add("GrabTime", 175, HorizontalAlignment.Center); //抓取数据时间
            this.list.Columns.Add("Profit", 100, HorizontalAlignment.Center); //抓取数据时间
        }
        public void ShowBetWin(double odds1,double odds2)
        {
            double winPer = (odds1*odds2)/(odds1+odds2)-1;
            if(winPer > 0)
            {
                if(!string.IsNullOrEmpty(tbBet1.Text) && !string.IsNullOrEmpty(tbBet2.Text))
                {
                    int bet1 = int.Parse(tbBet1.Text);
                    int bet2 = int.Parse(tbBet2.Text);
                    lbBetWin.Text = string.Format("盈利率:{0}%,奖金:{1},盈利:{2} ", (winPer*100).ToString("F2"),(bet1*odds1).ToString("F2"), (int)(winPer * (bet1 + bet2)));
                }
                else
                {
                    lbBetWin.Text = string.Format("盈利率:{0}%", (winPer * 100).ToString("F2"));
                }
            }
            else
            {
                lbBetWin.Text = string.Format("数据有误，无法打水！");
            }
                
        }
        public void ShowLog(string message,ErrorLevel level = ErrorLevel.EL_NORMAL)
        {
            DebugLog info = new DebugLog();
            info.message = message;
            info.level = level;
            info.webID = WebID.WID_NULL;
            ShowLog(info);
        }
        public  void ShowLog(DebugLog log)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new ShowLogHandler(ShowLog), log);
            }
            else
            {
                //设置光标的位置到文本尾   
                textBox.Select(textBox.TextLength, 0);
                //滚动到控件光标处   
                textBox.ScrollToCaret();
                if (log.webID != WebID.WID_NULL)
                {
                    textBox.AppendText(DateTime.Now.ToString("HH:mm:ss") + " " + StaticData.webNames[(int)log.webID] + ":" + log.message+"\r\n");
                }
                else
                {
                    textBox.AppendText(DateTime.Now.ToString("HH:mm:ss") + " " + log.message + "\r\n");
                }
            }
        }
        public List<BetWinPair>  FilterResult(List<BetWinPair> pairs)
        {
            List<BetWinPair> filter = new List<BetWinPair>();
            foreach(var pair in pairs)
            {
                //过滤时间
                if (DateTime.Compare(pair.b1.time, DynamicData.startDate) < 0 
                    || DateTime.Compare(pair.b2.time, DynamicData.startDate) < 0
                     || DateTime.Compare(pair.b1.time, DynamicData.endDate) > 0
                     || DateTime.Compare(pair.b2.time, DynamicData.endDate) > 0)
                {
                    continue;
                }

                //过滤盈利
                if (pair.profit < DynamicData.profitLow || pair.profit > DynamicData.profitHigh)
                {
                    continue;
                }

                filter.Add(pair);
            }
            return filter;
        }
        public void ShowResult( List<BetWinPair> pair)
        {
            if (list.InvokeRequired)
            {
                list.Invoke(new ShowResultHandler(ShowResult), pair);
            }
            else
            {
                ShowLog(string.Format("分析完毕，打水个数：{0}", pair.Count));

                if(DynamicData.bFresh)
                {
                    ClearList();
                    listPairs.Clear();
                }


                listPairs.AddRange(pair);
                this.list.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
                for (int i = 0; i < pair.Count; i++)   //添加10行数据
                {
                    BetItem p1 = pair[i].b1;
                    BetItem p2 = pair[i].b2;
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = (index++).ToString();
                    lvi.SubItems.Add(StaticData.webNames[(int)p1.webID] + "|" + StaticData.webNames[(int)p2.webID]);
                    lvi.SubItems.Add(p1.gameName);
                    lvi.SubItems.Add(p1.leagueName1);
                    lvi.SubItems.Add(p1.pName1 + "(" + p1.pAbbr1 + ")");
                    lvi.SubItems.Add(p1.pName2 + "(" + p1.pAbbr2 + ")");
                    lvi.SubItems.Add(p1.odds1.ToString() + "|" + p1.odds2.ToString());
                    lvi.SubItems.Add(p2.odds1.ToString() + "|" + p2.odds2.ToString());
                    lvi.SubItems.Add(p1.time.ToString());
                    lvi.SubItems.Add(DateTime.Now.ToString());
                    lvi.SubItems.Add(string.Format("{0}%", (pair[i].profit * 100).ToString("F2")));
                    this.list.Items.Add(lvi);
                }
                if (list.Items.Count > 0)
                {
                    list.Items[list.Items.Count - 1].EnsureVisible();
                }
                this.list.EndUpdate();  //结束数据处理，UI界面一次性绘制。 
            }
        }
        private void Start()
        {
            List<Thread> threads = new List<Thread>();
            while(true)
            {
                List<string> webs = Util.GetFilesCount(configDir+"\\"+StaticData.sportNames[(int)SportID.SID_ESPORT],"*.ini");
                ShowLog("-------------------------------");
                ShowLog(string.Format("循环第{0}次,爬取{1}个网站...",++time,webs.Count));
                List<BetItem> total = new List<BetItem>();
                threads.Clear();
                ESportParser.LoadMainData();
                ShowLog(string.Format("加载主列表队列..."));
                for (int i = 0; i < webs.Count; i++)
                {
                    int x = i;
                    Thread s = new Thread(() => {
                        BaseParser bp = ParseFactory.GetParser(SportID.SID_ESPORT, webs[x]);
                        bp.showLogEvent = ShowLog;
                        bp.LoadStaticData();
                        if(bp.bIsEffect)
                        {
                            ShowLog(string.Format("线程{0},爬取分析网站:{1}", x.ToString(), StaticData.webNames[(int)bp.webID]));
                            bp.GrabAndParseHtml();
                            total.AddRange(bp.betItems);
                        }
                        else
                        {
                            ShowLog(string.Format("线程{0},爬取分析网站:{1} 配置读取错误！", x.ToString(), StaticData.webNames[(int)bp.webID]));
                        }
                    });
                    s.Name = StaticData.webNames[i];
                    s.IsBackground = true;
                    s.Start();
                    threads.Add(s);
                }

                for (int i = 0; i < webs.Count; i++)
                {
                    threads[i].Join();
                }
                
                var pair = BaseParser.ParseBetWin(total);
                var finals = FilterResult(pair);
                ShowResult(finals);

                //延迟时间
                if(!string.IsNullOrEmpty(tbSleepTime.Text))
                {
                    Thread.Sleep(Convert.ToInt32(tbSleepTime.Text)*1000);
                }
            }
        }
       
  
        private void TestWeb()
        {
            BaseParser bp = ParseFactory.GetParser(SportID.SID_ESPORT,StaticData.webNames[(int)WebID.WID_UWIN]);
            bp.showLogEvent = ShowLog;
            bp.LoadStaticData();
            ShowLog(string.Format("爬取分析网站:{0}", StaticData.webNames[(int)bp.webID]));
            bp.GrabAndParseHtml();
        }
        private void Test()
        {
            List<BetItem> total = new List<BetItem>();
            BetItem b1 = new BetItem();
            b1.webID = WebID.WID_188;
            b1.sportID = SportID.SID_ESPORT;
            b1.type = BetType.BT_TEAM;
            b1.gameName = "SB联赛";
            b1.pID1 = 3;
            b1.pID2 = 5;
            b1.pName1 = "SB战队";
            b1.pName1 = "NB战队";
            b1.odds1 = 2.2;
            b1.odds2 = 1.95;
            b1.handicap = 0;
                
            BetItem b2 = new BetItem();
            b2.webID = WebID.WID_YABO;
            b2.sportID = SportID.SID_ESPORT;
            b2.type = BetType.BT_TEAM;
            b2.gameName = "SB联赛";
            b2.pID1 = 3;
            b2.pID2 = 5;
            b2.pName1 = "SB战队";
            b2.pName2 = "NB战队";
            b2.odds1 = 2.1;
            b2.odds2 = 2;
            b2.handicap = 0;

            total.Add(b1);
            total.Add(b2);

            var pair = BaseParser.ParseBetWin(total);
            ShowResult(pair);
        }

        private void btnUpdateLog_Click(object sender, EventArgs e)
        {
            UpdateLogForm logForm = new UpdateLogForm();
            logForm.Show();
        }

 
    }
}
