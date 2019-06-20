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
        public List<BetPlayerItem> betItems = new List<BetPlayerItem>();
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
        private static List<BetWinPair> ParseBetWin(List<BetPlayerItem> items1, List<BetPlayerItem> items2)
        {
            List<BetWinPair> listPair = new List<BetWinPair>();
            foreach (var item1 in items1)
            {
                foreach (var item2 in items2)
                {
                    if (item1.itemIndex == item2.itemIndex && item1.playerIndex == item2.playerIndex)
                    {
                        if ((item1.compare == BetCompare.Larger && item2.compare == BetCompare.Smaller && item1.value <= item2.value) ||
                            (item1.compare == BetCompare.Smaller && item2.compare == BetCompare.Larger && item1.value >= item2.value))
                        {
                            if (CanMustWin(item1.odds, item2.odds))
                            {
                                BetWinPair pair = new BetWinPair();
                                pair.item1 = item1;
                                pair.item2 = item2;
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
