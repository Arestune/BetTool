using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Splash.Parser
{
    class DynamicData
    {
        public static bool bFresh = false;
        public static DateTime startDate = DateTime.Now;
        public static DateTime endDate = DateTime.Now.AddDays(7);
        public static double profitLow = 0;
        public static double profitHigh = 5;
    }
}
