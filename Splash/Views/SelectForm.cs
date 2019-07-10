using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Splash.Tool;
using Splash.Item;
using Splash.Parser;

namespace Splash.Views
{
    public partial class SelectForm : Form
    {
        public SelectForm()
        {
            InitializeComponent();
            cbRefresh.Checked = DynamicData.bFresh;
            dtStart.Value = DynamicData.startDate;
            dtEnd.Value = DynamicData.endDate;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DynamicData.bFresh = cbRefresh.Checked;
            DynamicData.startDate = dtStart.Value;
            DynamicData.endDate = dtEnd.Value;
            Close();
        }
    }
}
