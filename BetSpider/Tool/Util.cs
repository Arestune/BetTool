using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace BetSpider.Tool
{
    class Util
    {
        public static int GetInt(string str)
        {
            Match match = Regex.Match(str, @"\.*(\d+)\.*");
            string strValue = match.Groups[1].ToString().Trim();
            int value = Convert.ToInt32(strValue);
            return value;
        }
        public static long GetCurrentTimeStamp()
        {
           return  (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
}
