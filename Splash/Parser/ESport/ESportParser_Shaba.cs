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
    class ESportParser_Shaba:ESportParser
    {
        protected override void Init()
        {
            webID = WebID.WID_Shaba;
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
                op.XHRParams = Config.GetString(StaticData.SN_URL, "XHRParams", configFile, "");
                op.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                string verfCode = Config.GetString(StaticData.SN_URL, "VerfCode", configFile, "");
                op.WebHeader.Add(string.Format("__VerfCode:{0}", verfCode));
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
            html = html.Substring(7, html.Length - 8);
            JArray main = JArray.Parse(html);
            JArray esport = JArray.Parse(main[0].ToString());
            foreach(var leagueCell in esport)
            {
                var title = leagueCell[3].ToString();
                var titleArray = title.Split('-');
                var gameId = titleArray[0].Trim();
                var league = titleArray[1].Trim();
                var gameIndex = GetGameIndex(gameId);
                if(gameIndex == INVALID_INDEX)
                {
                    continue;
                }
                var matchCell = JArray.Parse(leagueCell[5].ToString());
                foreach (var record in matchCell)
                {
                    try
                    {
                        var team1_abbr = record[1].ToString().Trim();
                        var team2_abbr = record[2].ToString().Trim();
                        var team1_name = record[1].ToString().Trim();
                        var team2_name = record[2].ToString().Trim();
                        var team1_index = GetTeamIndex(gameIndex, team1_name);
                        var team2_index = GetTeamIndex(gameIndex, team2_name);
                        var timeString = record[3].ToString();
                        timeString = System.Text.RegularExpressions.Regex.Replace(timeString, @"[^0-9]+", "");
                        var timeStamp = Convert.ToInt64(timeString) / 1000;
                        var gameTime = new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(timeStamp);
                        var oddsCell = JArray.Parse(record[27].ToString())[0];
                        var odds1Cell = JArray.Parse(oddsCell[8][0].ToString());
                        var odds2Cell = JArray.Parse(oddsCell[8][1].ToString());
                        if(Convert.ToInt32(oddsCell[1].ToString()) != 20)
                        {
                            continue;
                        }
                        if (odds1Cell.Count != 2 || odds2Cell.Count != 2 || odds1Cell[0].ToString() != "h" || odds2Cell[0].ToString() != "a")
                        {
                            continue;
                        }
                        var odds1Format = odds1Cell[1].ToString();
                        var odds2Format = odds2Cell[1].ToString();
                        var odds1 = Convert.ToDouble(odds1Format);
                        var odds2 = Convert.ToDouble(odds2Format);
                        if (odds1 < 0)
                        {
                            odds1 = (1 / (-odds1)) + 1;
                        }
                        else
                        {
                            odds1 += 1;
                        }
                        if (odds2 < 0)
                        {
                            odds2 = (1 / (-odds2)) + 1;
                        }
                        else
                        {
                            odds2 += 1;
                        }

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
