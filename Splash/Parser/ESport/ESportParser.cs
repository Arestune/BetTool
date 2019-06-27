using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splash.Parser;
using Splash.Item;
using Splash.Tool;
namespace Splash.Parser.ESport
{
    class ESportParser:BaseParser
    {
       
        protected override void Init()
        {
            sportID = SportID.SID_ESPORT;
            base.Init();
        }
 
        protected virtual BetCompare GetBetCompare(string parseString)
        {
            return BetCompare.Larger;
        }
        protected virtual double GetBetValue(string parseString)
        {
            return 0.0;
        }
        protected virtual int GetPlayerIndex(string parseString)
        {
            return -1;
        }
    }
}
