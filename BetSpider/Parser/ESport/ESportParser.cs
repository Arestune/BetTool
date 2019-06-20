using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetSpider.Parser;
using BetSpider.Item;
namespace BetSpider.Parser.ESport
{
    class ESportParser:BaseParser
    {
        protected List<int> gameIds = new List<int>();
        protected List<string> gameNames = new List<string>();
        protected Dictionary<int, Dictionary<string, int>> teamIds = new Dictionary<int, Dictionary<string, int>>();
       
        
        protected override void Init()
        {
            sportID = SportID.SID_ESPORT;
            base.Init();
        }
        public static List<BetWinPair> ParseBetWin(List<BetTeamItem> teams1, List<BetTeamItem> teams2)
        {
            List<BetWinPair> listPair = new List<BetWinPair>();
            foreach (var team1 in teams1)
            {
                foreach (var team2 in teams2)
                {
                    double odd1 = 0.0f;
                    double odd2 = 0.0f;
                    double odd3 = 0.0f;
                    double odd4 = 0.0f;
                    if (team1.gameIndex == team2.gameIndex )
                    {
                        if((team1.team1_index == team2.team1_index) && (team1.team2_index == team2.team2_index))
                        {
                            odd1 = team1.odd1;
                            odd2 = team2.odd2;
                            odd3 = team1.odd2;
                            odd4 = team2.odd1;
                            if (team1.value >= team2.value)
                            {
                                if (CanMustWin(odd3, odd4))
                                {
                                    BetWinPair pair = new BetWinPair();
                                    pair.team1 = team1;
                                    pair.team2 = team2;
                                    listPair.Add(pair);
                                }
                            }
                            if (team1.value <= team2.value)
                            {
                                if (CanMustWin(odd1, odd2))
                                {
                                    BetWinPair pair = new BetWinPair();
                                    pair.team1 = team1;
                                    pair.team2 = team2;
                                    listPair.Add(pair);
                                }
                            }
                        }
                        else if ((team1.team1_index == team2.team2_index) && (team1.team2_index == team2.team1_index))
                        {
                            odd1 = team1.odd1;
                            odd2 = team2.odd1;
                            odd3 = team1.odd2;
                            odd4 = team2.odd2;
                            team2.value = -team2.value;
                            if (team1.value >= team2.value)
                            {
                                if (CanMustWin(odd3, odd4))
                                {
                                    BetWinPair pair = new BetWinPair();
                                    pair.team1 = team1;
                                    pair.team2 = team2;
                                    listPair.Add(pair);
                                }
                            }
                            if (team1.value <= team2.value)
                            {
                                if (CanMustWin(odd1, odd2))
                                {
                                    BetWinPair pair = new BetWinPair();
                                    pair.team1 = team1;
                                    pair.team2 = team2;
                                    listPair.Add(pair);
                                }
                            }
                        }
                       
                         
                    }
                }
            }
            return listPair;
        }
        public override void Parse()
        {
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
