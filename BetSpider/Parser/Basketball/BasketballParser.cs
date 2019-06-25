using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetSpider.Parser;
using BetSpider.Item;
namespace BetSpider.Parser.Basketball
{
    class BasketballParser:BaseParser
    {
        protected List<string> effectItems = new List<string>();
        protected List<string> playerNames = new List<string>();
        protected List<string> largers = new List<string>();
        protected List<string> smallers = new List<string>();
        protected override void Init()
        {
            sportID = SportID.SID_BASKETBALL;
            base.Init();
        }
        public override void Parse()
        {
        }
        private static List<BetWinPair> ParseBetWin(List<BetItem> bs1, List<BetItem> bs2)
        {
            List<BetWinPair> listPair = new List<BetWinPair>();
            foreach (var b1 in bs1)
            {
                foreach (var b2 in bs2)
                {
                    if (b1.itemID == b2.itemID && b1.pID1 == b2.pID2)
                    {
                        if ((b1.compare == BetCompare.Larger && b2.compare == BetCompare.Smaller && b1.handicap <= b2.handicap) ||
                            (b1.compare == BetCompare.Smaller && b2.compare == BetCompare.Larger && b1.handicap >= b2.handicap))
                        {
                            if (CanMustWin(b1.odds1, b2.odds1))
                            {
                                BetWinPair pair = new BetWinPair();
                                pair.b1 = b1;
                                pair.b2 = b2;
                                listPair.Add(pair);
                            }
                        }
                    }
                }
            }
            return listPair;
        }
        protected virtual BetCompare GetBetCompare(string parseString)
        {
            return BetCompare.Larger;
        }
        protected virtual double GetBetHandicap(string parseString)
        {
            return 0.0;
        }
        protected virtual int GetPlayerIndex(string parseString)
        {
            return -1;
        }
    }
}
