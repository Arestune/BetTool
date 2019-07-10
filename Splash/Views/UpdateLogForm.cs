using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Splash.Views
{
    public partial class UpdateLogForm : Form
    {
        public UpdateLogForm()
        {
            InitializeComponent();
            tbLog.AppendText("---------------------------------------------------------\r\n");

            UpdateLog();
        }
        public void UpdateLog()
        {
            //v1.6
            tbLog.AppendText("v1.6 @ 2019-07-10\r\n");
            tbLog.AppendText("##新增：\r\n");
            tbLog.AppendText("1.日期筛选条件。\r\n");
            tbLog.AppendText("2.默认日期为今天到一周后，可以在筛选结果界面里修改。\r\n");
            tbLog.AppendText("3.投注单增加比赛场次BO显示。\r\n");
            tbLog.AppendText("##修复：\r\n");
            tbLog.AppendText("1.亚游爬到的分数和奇偶的盘口被当成胜负盘的BUG\r\n");
            tbLog.AppendText("##优化：\r\n");
            tbLog.AppendText("1.工具栏增加高度，图标增大\r\n");
            tbLog.AppendText("2.优化一些Label控件的显示\r\n");
            tbLog.AppendText("---------------------------------------------------------\r\n");
        }
    }
}
