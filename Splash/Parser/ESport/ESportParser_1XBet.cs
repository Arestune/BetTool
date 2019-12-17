using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Splash.Item;
using Splash.Parser;
using Splash.Tool;
namespace Splash.Parser.ESport
{
    class ESportParser_1XBet:ESportParser
    {
        protected override void Init()
        {
            webID = WebID.WID_1XBet;
            base.Init();
        }
        public override void GrabAndParseHtml()
        {
            try
            {
                //今日
                int nTryCount = 0;
                string uri = Config.GetString(StaticData.SN_URL, "Uri", configFile, "Uri");
                RequestOptions op = new RequestOptions(uri);
                op.Method = Config.GetString(StaticData.SN_URL, "Method", configFile, "GET");
                op.Accept = Config.GetString(StaticData.SN_URL, "Accept", configFile, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                op.Referer = Config.GetString(StaticData.SN_URL, "Referer", configFile, "");
                op.RequestCookies = Config.GetString(StaticData.SN_URL, "Cookie", configFile, "");
                //获取网页
                html = RequestAction(op);
                while (string.IsNullOrEmpty(html) && nTryCount < MAX_TRY_COUNT)
                {
                    html = RequestAction(op);
                    nTryCount++;
                }
                if (nTryCount == MAX_TRY_COUNT)
                {
                    ShowLog("抓取失败！");
                    html = "";
                }
                else
                {
                    Parse();
                }
                ShowLog(string.Format("页面解析成功，解析个数：{0}！", betItems.Count));
            }
            catch (Exception e)
            {
                DebugLog error = new DebugLog();
                error.webID = webID;
                error.level = ErrorLevel.EL_ERROR;
                error.message = e.Message;
                ShowLog(error);
            }
        }

        public override int Parse()
        {
            if (string.IsNullOrEmpty(html))
            {
                return 0;
            }
            JObject main = JObject.Parse(html);
            JArray array = JArray.Parse(main["Value"].ToString());
            foreach (var record in array)
            {
                try
                {
                    var gameId = record["SSI"].ToString();
                    var league = record["L"].ToString();
                    var gameIndex = GetGameIndex(gameId);
                    if(gameIndex == INVALID_INDEX)
                    {
                        continue;
                    }
                    var bo = 1;
                    var timeStamp =Convert.ToInt64(record["S"]);
                    var gameTime = new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(timeStamp);
                    var oddsMarket = JArray.Parse(record["E"].ToString());
                    if (oddsMarket.Count != 2)
                    {
                        continue;
                    }
                    var team1_abbr = record["O1E"].ToString().Trim();
                    var team2_abbr = record["O2E"].ToString().Trim();
                    var team1_name = record["O1E"].ToString().Trim();
                    var team2_name = record["O2E"].ToString().Trim();
                    var team1_index = GetTeamIndex(gameIndex, team1_name);
                    var team2_index = GetTeamIndex(gameIndex, team2_name);
                    var odd1 = Convert.ToDouble(oddsMarket[0]["C"]);
                    var odd2 = Convert.ToDouble(oddsMarket[1]["C"]);
                    var betLimit1 = 0;
                    var betLimit2 = 0;
                    BetItem b = new BetItem();
                    b.webID = webID;
                    b.sportID = sportID;
                    b.type = BetType.BT_TEAM;
                    b.pID1 = team1_index;
                    b.pID2 = team2_index;
                    b.pName1 = team1_name;
                    b.pName2 = team2_name;
                    b.pAbbr1 = team1_abbr;
                    b.pAbbr2 = team2_abbr;
                    b.odds1 = odd1;
                    b.odds2 = odd2;
                    b.gameID = gameIndex;
                    b.gameName = gameStaticNames[gameIndex];
                    b.leagueName1 = league;
                    b.leagueName2 = league;
                    b.handicap = 0;
                    b.time = gameTime;
                    b.bo = bo;
                    b.betLimit1 = (int)betLimit1;
                    b.betLimit2 = (int)betLimit2;
                    betItems.Add(b);   
                }
                catch (Exception e)
                {
                    DebugLog error = new DebugLog();
                    error.webID = webID;
                    error.level = ErrorLevel.EL_WARNING;
                    error.message = e.Message;
                    ShowLog(error);
                }
            }       
            return betItems.Count;
        }
    }
     
}
