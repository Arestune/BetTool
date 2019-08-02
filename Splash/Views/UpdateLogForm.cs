using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Splash.Log;
namespace Splash.Views
{
  
    public partial class UpdateLogForm : Form
    {
        
        public UpdateLogForm()
        {
            InitializeComponent();
            UpdateLog();
        }
        public void AddTitle(string version, string time)
        {
            tbLog.AppendText("---------------------------------------------------------\r\n");
            string title = string.Format("{0} @ {1}\r\n", version, time);
            tbLog.SelectionColor = Color.Blue;
            tbLog.AppendText(title);
            tbLog.SelectionColor = Color.Black;
        }
        public void AddTail()
        {
            tbLog.AppendText("\r\n");
            tbLog.AppendText("\r\n");
            tbLog.AppendText("\r\n");
        }
        public void AddLog(UpdateLog log)
        {
            if (log.type == LogType.LT_NEW)
            {
                tbLog.SelectionColor = Color.Green;
                tbLog.AppendText("新增：\r\n");
               // tbLog.SelectionColor = Color.Black;  
            }
            else if(log.type == LogType.LT_MODIFY)
            {
                tbLog.SelectionColor = Color.OrangeRed;
                tbLog.AppendText("修复：\r\n");
              //  tbLog.SelectionColor = Color.Black;  
            }
            else if (log.type == LogType.LT_OPTIMIZE)
            {
                tbLog.SelectionColor = Color.Purple;
                tbLog.AppendText("优化：\r\n");
             //   tbLog.SelectionColor = Color.Black;
            }
            else if (log.type == LogType.LT_DELETE)
            {
                tbLog.SelectionColor = Color.Gray;
                tbLog.AppendText("删除：\r\n");
             //   tbLog.SelectionColor = Color.Black;
            }

            for (int i = 0; i < log.logs.Count;i++ )
            {
                tbLog.AppendText(string.Format("{0}.{1}。\r\n",i+1,log.logs[i]));
            }
         
        }
        
        public void UpdateLog()
        {
            //v1.6
            AddTitle("v1.6", "2019-07-10");
            AddLog(new UpdateLog(LogType.LT_NEW, new List<string>{
                "日期筛选条件",
                "默认日期为今天到一周后，可以在筛选结果界面里修改",
                "投注单增加比赛场次BO显示"
            }));
            AddLog(new UpdateLog(LogType.LT_MODIFY, new List<string>{
                "亚游爬到的分数和奇偶的盘口被当成胜负盘的BUG"
            }));
            AddLog(new UpdateLog(LogType.LT_OPTIMIZE, new List<string>{
                "工具栏增加高度，图标增大BUG",
                "优化一些Label控件的显示"
            }));
            AddTail();

            //v1.7
            AddTitle("v1.7", "2019-07-23");
            AddLog(new UpdateLog(LogType.LT_NEW, new List<string>{
                "UWin网站爬取解析",
            }));
            AddLog(new UpdateLog(LogType.LT_MODIFY, new List<string>{
                "修复判断时间从两者小时的差值转换为两者的时差BUG"
            }));
            AddTail();

            //v1.7.1
            AddTitle("v1.7.1", "2019-07-24");
            AddLog(new UpdateLog(LogType.LT_MODIFY, new List<string>{
                 "修复UWin网站没有抓取时间的BUG",
                 "修复UWin网站没有设置SportID的BUG"
            }));
            AddLog(new UpdateLog(LogType.LT_OPTIMIZE, new List<string>{
                "UTF8配置文件如果读取异常，当前网站不会计入打水网站"
            }));
            AddTail();

            //v1.7.2
            AddTitle("v1.7.2", "2019-07-30");
            AddLog(new UpdateLog(LogType.LT_NEW, new List<string>{
                 "新增盈利率筛选",
            }));
            AddLog(new UpdateLog(LogType.LT_MODIFY, new List<string>{
                "修复UWin爬取某一天时遇到错误就不接着爬取当天数据的BUG"
            }));
            AddTail();

            //v1.8
            AddTitle("v1.8", "2019-08-02");
            AddLog(new UpdateLog(LogType.LT_NEW, new List<string>{
                 "新增平博体育的电竞爬取",
            }));
        }
    }
}
