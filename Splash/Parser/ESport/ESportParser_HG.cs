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
    class ESportParser_HG:ESportParser
    {
        public Dictionary<int, string> dicLeagues;
        protected override void Init()
        {
            webID = WebID.WID_HG;
            base.Init();
        }
        protected override DateTime GetGameTime(string d)
        {
            string[] s = new string[7];
            s[0] = d.Substring(0, 4);
            s[1] = d.Substring(4, 2);
            s[2] = d.Substring(6, 2);
            s[3] = d.Substring(8, 2);
            s[4] = d.Substring(10, 2);
            DateTime dt = Convert.ToDateTime(s[0] + "-" + s[1] + "-" + s[2]);
            dt = dt.AddHours(int.Parse(s[3]));
            dt = dt.AddMinutes(int.Parse(s[4]));
            return dt;
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
                op.XHRParams = Config.GetString(StaticData.SN_URL, "XHRParams", configFile, "");
                op.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
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

            dicLeagues = new Dictionary<int, string>();
            var main = JObject.Parse(html);
            var data = main["Data"];
            var lg = data["LG"];
            foreach(var league in lg)
            {
                foreach (JToken child in league.Children())
                {
                    int leagueId = Convert.ToInt32(child["LeagueID"].ToString());
                    string leagueName = child["LN"].ToString();
                    dicLeagues.Add(leagueId, leagueName);
                }
                
            }
            if(data["MH"] ==null)
            {
                throw  new Exception("MH：为空");
            }
            var mh =JArray.Parse(data["MH"].ToString());
            foreach (var match in mh)
            {
                var m = JArray.Parse(match["M"].ToString());
                var leagueId = Convert.ToInt32( m[2].ToString());
                if(dicLeagues.ContainsKey(leagueId))
                {
                    try
                    {
                        var title = dicLeagues[leagueId];
                        var titleArray = title.Split('-');
                        var gameId = titleArray[0].Trim();
                        var league = titleArray[1].Trim();
                        var gameIndex = GetGameIndex(gameId);
                        if (gameIndex == INVALID_INDEX)
                        {
                            continue;
                        }
                        var team1_abbr = m[4].ToString().Trim();
                        var team2_abbr = m[6].ToString().Trim();
                        var team1_name = m[4].ToString().Trim();
                        var team2_name = m[6].ToString().Trim();
                        var team1_index = GetTeamIndex(gameIndex, team1_name);
                        var team2_index = GetTeamIndex(gameIndex, team2_name);
                        var timeFormat = m[7].ToString();
                        var gameTime = GetGameTime(timeFormat);

                        var mk = match["MK"];
                        if (mk != null)
                        {
                            var seriesWin = mk["16"];
                            if (seriesWin != null)
                            {
                                var oddsArray = JArray.Parse(seriesWin.ToString());
                                var oddsCell = oddsArray[0];
                                var odds1 = Convert.ToDouble(oddsCell[1]);
                                var odds2 = Convert.ToDouble(oddsCell[2]);

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
                                b.odds1 = odds1;
                                b.odds2 = odds2;
                                b.gameID = gameIndex;
                                b.gameName = gameStaticNames[gameIndex];
                                b.leagueName1 = league;
                                b.leagueName2 = league;
                                b.handicap = 0;
                                b.time = gameTime;
                                b.bo = 0;
                                b.betLimit1 = 0;
                                b.betLimit2 = 0;
                                betItems.Add(b);
                            }

                            var handicapWin = mk["1"];
                            if (handicapWin != null)
                            {
                                var oddsArray = JArray.Parse(seriesWin.ToString());
                                var oddsCell = oddsArray[0];
                                var odds1 = Convert.ToDouble(oddsCell[1]);
                                var odds2 = Convert.ToDouble(oddsCell[2]);
                                var handicap = Convert.ToDouble(oddsCell[3]);

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
                                b.odds1 = odds1;
                                b.odds2 = odds2;
                                b.gameID = gameIndex;
                                b.gameName = gameStaticNames[gameIndex];
                                b.leagueName1 = league;
                                b.leagueName2 = league;
                                b.handicap = handicap;
                                b.time = gameTime;
                                b.bo = 0;
                                b.betLimit1 = 0;
                                b.betLimit2 = 0;
                                betItems.Add(b);
                            }
                        }
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
            }
            return betItems.Count;
        }
    }
     
}
