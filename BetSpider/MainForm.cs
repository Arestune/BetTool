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
using Commons;
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
        public static RichTextBox tb;
        int time = 0;
        int index = 0;
        public MainForm()
        {
            InitializeComponent();
            tb = textBox;
            this.list.Columns.Add("Index", 60, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Web", 150, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Game", 150, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("League", 200, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Team1", 150, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Team2", 150, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Odds1", 120, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Odds2", 120, HorizontalAlignment.Center); //一步添加
            this.list.Columns.Add("Time", 175, HorizontalAlignment.Center); //一步添加

            //ShowLog("");
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
                if(pair.Count > 0 )
                {
                    MessageBox.Show("打到水了！");
                }
                ShowLog(string.Format("分析完毕，打水个数：{0}", pair.Count));
                this.list.BeginUpdate();   //数据更新，UI暂时挂起，直到EndUpdate绘制控件，可以有效避免闪烁并大大提高加载速度
                for (int i = 0; i < pair.Count; i++)   //添加10行数据
                {
                    BetTeamItem p1 = pair[i].team1;
                    BetTeamItem p2 = pair[i].team2;
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = (index++).ToString();
                    lvi.SubItems.Add(StaticData.webNames[(int)p1.webID] + "|" + StaticData.webNames[(int)p2.webID]);
                    lvi.SubItems.Add(p1.gameName);
                    lvi.SubItems.Add(p1.leagueName1);
                    lvi.SubItems.Add(p1.team1_name);
                    lvi.SubItems.Add(p1.team2_name);
                    lvi.SubItems.Add(p1.odd1.ToString() + "|" + p1.odd2.ToString());
                    lvi.SubItems.Add(p2.odd1.ToString() + "|" + p2.odd2.ToString());
                    lvi.SubItems.Add(DateTime.Now.ToString());
                    this.list.Items.Add(lvi);
                }
                this.list.EndUpdate();  //结束数据处理，UI界面一次性绘制。
            }
        }
        private void Start()
        {
            while(true)
            {
                ShowLog("-------------------------------");
                ShowLog(string.Format("循环第{0}次",++time));

                BaseParser bp1 = new ESportParser_Yabo();
                bp1.showLogEvent = ShowLog;
                ShowLog(string.Format("爬取分析网站:{0}",  StaticData.webNames[(int)bp1.webID]));
                bp1.GrabAndParseHtml();

                BaseParser bp2 = new ESportParser_Yayou();
                bp2.showLogEvent = ShowLog;
                ShowLog(string.Format("爬取分析网站:{0}", StaticData.webNames[(int)bp2.webID]));
                bp2.GrabAndParseHtml();

                var pair = ESportParser.ParseBetWin(bp1.betItems, bp2.betItems);

                ShowResult(pair);
                Thread.Sleep(30000);
            }
        }
        private void btnRun_Click(object sender, EventArgs e)
        {
            index = 0;
            Thread[] threads = new Thread[1];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ThreadStart(Start));
                threads[i].Name = (i + 1).ToString();
                threads[i].IsBackground = true;
                threads[i].Start();
                ShowLog(string.Format("启动线程{0}", i));
            }
            // BaseParser p = new YayouParser();
            // p.LoadStaticData();
            // p.GrabAndParseHtml("https://b79.lpxin.cn/server/rest/event-tree/prelive/specials?lang=zh&currency=CNY&duration=&marketUID=hdp-ou");
            // List<BetItem> items1 = p.Parse();
        }

    }
}
