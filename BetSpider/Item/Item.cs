using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetSpider.Item
{
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
        public WebID webID
        {
            set;
            get;
        }
        public BetType type
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
        public double value
        {
            get;
            set;
        }
        public double odd1
        {
            get;
            set;
        }
        public double odd2
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
