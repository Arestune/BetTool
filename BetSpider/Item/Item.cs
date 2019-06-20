using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetSpider.Item
{
    public enum BetCompare
    {
        Larger = 0,
        Smaller = 1
    }
    public class BetPlayerItem
    {
        public int itemIndex
        {
            get;
            set;
        }
        public int playerIndex
        {
            get;
            set;
        }
        public string playerName
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
        public double odds
        {
            get;
            set;
        }
    }

    public class BetTeamItem
    {
        public WebID webID
        {
            set;
            get;
        }
        public int gameIndex
        {
            get;
            set;
        }
        public string gameName
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
        public int team1_index
        {
            get;
            set;
        }
        public int team2_index
        {
            get;
            set;
        }
        public string team1_name
        {
            get;
            set;
        }
        public string team2_name
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
    }

    public class BetWinPair
    {
        public BetPlayerItem item1
        {
            get;
            set;
        }
        public BetPlayerItem item2
        {
            get;
            set;
        }

        public BetTeamItem team1
        {
            get;
            set;
        }

        public BetTeamItem team2
        {
            get;
            set;
        }
    }
}
