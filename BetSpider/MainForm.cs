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
using BetSpider.Tool;
using BetSpider.Item;
using BetSpider.Parser;
using BetSpider.Parser.Basketball;
using BetSpider.Parser.ESport;

namespace BetSpider
{
    public partial class MainForm : Form
    {
        public delegate void ShowLogHandler(LogInfo log);
        public delegate void ShowResultHandler(List<BetWinPair> pairs);
        int time = 0;
        int index = 0;
        public MainForm()
        {
            InitializeComponent();
            this.list.Columns.Add("Index", 60, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Web", 150, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Game", 150, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("League", 200, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Team1", 150, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Team2", 150, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Odds1", 120, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Odds2", 120, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Time", 175, HorizontalAlignment.Center); //一步添加

        }
        public void ShowLog(string message,ErrorLevel level = ErrorLevel.EL_NORMAL)
        {
            LogInfo info = new LogInfo();
            info.message = message;
            info.level = level;
            info.webID = WebID.WID_NULL;
            ShowLog(info);
        }
        public  void ShowLog(LogInfo log)
        {
            if (textBox.InvokeRequired)
            {
                textBox.Invoke(new ShowLogHandler(ShowLog), log);
            }
            else
            {
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
        public void ShowResult( List<BetWinPair> pair)
        {
            if (list.InvokeRequired)
            {
                list.Invoke(new ShowResultHandler(ShowResult), pair);
            }
            else
            {
                //if(pair.Count > 0 )
                //{
                //    MessageBox.Show("打到水了！");
                //}
                ShowLog(string.Format("分析完毕，打水个数：{0}", pair.Count));
                //this.list.Clear();
             
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
                    lvi.SubItems.Add(p1.pName1);
                    lvi.SubItems.Add(p1.pName2);
                    lvi.SubItems.Add(p1.odds1.ToString() + "|" + p1.odds2.ToString());
                    lvi.SubItems.Add(p2.odds1.ToString() + "|" + p2.odds2.ToString());
                    lvi.SubItems.Add(DateTime.Now.ToString());
                    this.list.Items.Add(lvi);
                }
                this.list.EndUpdate();  //结束数据处理，UI界面一次性绘制。
            }
        }
        private void Start()
        {
            int webNum = 4;
            List<Thread> threads = new List<Thread>();
            while(true)
            {
                ShowLog("-------------------------------");
                ShowLog(string.Format("循环第{0}次",++time));

                List<BetItem> total = new List<BetItem>();
                threads.Clear();
                for (int i = 0; i < webNum; i++)
                {
                    int x = i;
                    Thread s = new Thread(() => {
                        BaseParser bp = ParseFactory.GetParser(SportID.SID_ESPORT, (WebID)x);
                        bp.showLogEvent = ShowLog;
                        ShowLog(string.Format("线程{0},爬取分析网站:{1}", x.ToString(), StaticData.webNames[(int)bp.webID]));
                        bp.GrabAndParseHtml();
                        total.AddRange(bp.betItems);
                    });
                    s.Name = StaticData.webNames[i];
                    s.IsBackground = true;
                    s.Start();
                    threads.Add(s);
                }

                for (int i = 0; i < webNum; i++)
                {
                    threads[i].Join();
                }
                
                var pair = BaseParser.ParseBetWin(total);
                ShowResult(pair);
                Thread.Sleep(30000);
            }
        }
        private void btnRun_Click(object sender, EventArgs e)
        {
            //TestWeb();
           // return;
            index = 0;
            Thread startThread = new Thread(Start);
            startThread.Name = "Main";
            startThread.IsBackground = true;
            startThread.Start();
            ShowLog("启动主抓取线程");
        }
        private void TestWeb()
        {
            BaseParser bp = ParseFactory.GetParser(SportID.SID_ESPORT, WebID.WID_FANYA);
            bp.showLogEvent = ShowLog;
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
            b1.pID1 = 3;
            b1.pID2 = 5;
            b1.odds1 = 2.2;
            b1.odds2 = 1.95;
            b1.handicap = 0;

            BetItem b2 = new BetItem();
            b2.webID = WebID.WID_YABO;
            b2.sportID = SportID.SID_ESPORT;
            b2.type = BetType.BT_TEAM;
            b2.pID1 = 3;
            b2.pID2 = 5;
            b2.odds1 = 2.1;
            b2.odds2 = 2;
            b2.handicap = 0;

            BetItem b3 = new BetItem();
            b3.webID = WebID.WID_188;
            b3.sportID = SportID.SID_ESPORT;
            b3.type = BetType.BT_TEAM;
            b3.pID1 = 3;
            b3.pID2 = 5;
            b3.odds1 = 2.2;
            b3.odds2 = 1.95;
            b3.handicap = 0;

            total.Add(b1);
            total.Add(b2);
            total.Add(b3);
            var pair = BaseParser.ParseBetWin(total);
            ShowResult(pair);
        }
    }
}
