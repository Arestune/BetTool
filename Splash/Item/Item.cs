using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Item
{
    public enum SportID
    {
        SID_NULL = -1,
        SID_BASKETBALL = 0,
        SID_ESPORT
    }
    public enum WebID
    {
        WID_NULL = -1,
        WID_IM = 0,
        WID_YAYOU,
        WID_188,
        WID_FANYA,
        WID_RAY,
        WID_UWIN,
        WID_Pingbo,
        WID_CMD,
        WID_BTI,
        WID_Leihuo,
        WID_1XBet
    }

    public enum BetType
    {
        BT_TEAM = 0,
        BT_SOLO
    }
    public enum BetCompare
    {
        Larger = 0,
        Smaller = 1
    }

    public class BetItem
    {
        public BetItem()
        {
            handicap = 0;
            odds1 = 0;
            odds2 = 0;
        }
        public WebID webID
        {
            set;
            get;
        }
        public SportID sportID
        {
            get;
            set;
        }
        public BetType type
        {
            get;
            set;
        }
        public int bo
        {
            get;
            set;
        }
        public string leagueName1
        {
            get;
            set;
        }
        public string leagueName2
        {
            get;
            set;
        }
        public int gameID
        {
            get;
            set;
        }
        public string gameName
        {
            get;
            set;
        }
        public int matchID
        {
            get;
            set;
        }
        public int itemID
        {
            get;
            set;
        }
        public string itemName
        {
            get;
            set;
        }
        public int pID1
        {
            get;
            set;
        }
        public string pName1
        {
            get;
            set;
        }
        public string pAbbr1
        {
            get;
            set;
        }
        public int pID2
        {
            get;
            set;
        }
       
        public string pName2
        {
            get;
            set;
        }
        public string pAbbr2
        {
            get;
            set;
        }
        public BetCompare compare
        {
            get;
            set;
        }
        public double handicap
        {
            get;
            set;
        }
        public double odds1
        {
            get;
            set;
        }
        public double odds2
        {
            get;
            set;
        }
        public int betLimit1
        {
            get;
            set;
        }
        public int betLimit2
        {
            get;
            set;
        }
        public DateTime time
        {
            get;
            set;
        }
        public string reserve
        {
            get;
            set;
        }
    }

    public class BetWinPair
    {
        public bool orderForward
        {
            get;
            set;
        }
        public double odds1
        {
            get;
            set;
        }
        public double odds2
        {
            get;
            set;
        }
        public string pName1
        {
            get;
            set;
        }
        public string pName2
        {
            get;
            set;
        }
        public int betLimit1
        {
            get;
            set;
        }
        public int betLimit2
        {
            get;
            set;
        }
        public string pAbbr1
        {
            get;
            set;
        }

        public string pAbbr2
        {
            get;
            set;
        }
        public double handicap1
        {
            get;
            set;
        }
        public double handicap2
        {
            get;
            set;
        }

        public double profit
        {
            get;
            set;
        }

        public BetItem b1
        {
            get;
            set;
        }

        public BetItem b2
        {
            get;
            set;
        }
    }
}
