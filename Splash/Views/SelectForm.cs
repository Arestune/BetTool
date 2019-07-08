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
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DynamicData.bFresh = cbRefresh.Checked;
            Close();
        }
    }
}
