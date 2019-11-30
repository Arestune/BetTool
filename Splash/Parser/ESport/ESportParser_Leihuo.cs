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
    class ESportParser_Leihuo:ESportParser
    {
        protected override void Init()
        {
            webID = WebID.WID_Leihuo;
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
                string auth = Config.GetString(StaticData.SN_URL, "Authorization", configFile, "");
                op.WebHeader.Add(string.Format("authorization:Token {0}", auth)) ;
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

                //早盘
                string uri2 = Config.GetString(StaticData.SN_URL, "Uri2", configFile, "Uri2");
                string tomorrow = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                string tomUrl = string.Format(uri2, tomorrow);
                op = new RequestOptions(tomUrl);
                op.Method = Config.GetString(StaticData.SN_URL, "Method", configFile, "GET");
                op.Accept = Config.GetString(StaticData.SN_URL, "Accept", configFile, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                op.Referer = Config.GetString(StaticData.SN_URL, "Referer", configFile, "");
                op.RequestCookies = Config.GetString(StaticData.SN_URL, "Cookie", configFile, "");
                op.WebHeader.Add(string.Format("authorization:Token {0}", auth));
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
            //JObject main = JObject.Parse(html);
            JArray array = JArray.Parse(html);

            foreach (var record in array)
            {
                try
                {
                    var gameId = record["game_name"].ToString();
                    var league = record["competition_name"].ToString();
                    var gameIndex = GetGameIndex(gameId);
                    var bo = 1;
                    var gameTime = Convert.ToDateTime(record["start_datetime"].ToString());
                    JToken team1 = record["home"];
                    JToken team2 = record["away"];
                    var team1_abbr = team1["team_name"].ToString().Trim();
                    var team2_abbr = team2["team_name"].ToString().Trim();
                    var team1_name = team1["team_name"].ToString().Trim();
                    var team2_name = team2["team_name"].ToString().Trim();
                    var team1_index = GetTeamIndex(gameIndex, team1_name);
                    var team2_index = GetTeamIndex(gameIndex, team2_name);
                    JArray oddsMarket = JArray.Parse(record["markets"].ToString());
                    JToken odds = oddsMarket[0];
                    JArray selection = JArray.Parse(odds["selection"].ToString());
                    JToken odds1Market = selection[0];
                    JToken odds2Market = selection[1];
                    var odd1 = Convert.ToDouble(odds1Market["euro_odds"]);
                    var odd2 = Convert.ToDouble(odds2Market["euro_odds"]);
                    var betLimit1 = Convert.ToDouble(odds["max_bet_amount"]);
                    var betLimit2 = betLimit1;
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
