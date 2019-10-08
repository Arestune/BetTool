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
    class ESportParser_BTI:ESportParser
    {
        protected override void Init()
        {
            webID = WebID.WID_BTI;
            base.Init();
        }
        protected override string GetGameID(string str)
        {
            string[] strArray= str.Split('-');
            return strArray[0].Trim();
        }
        protected  string GetGameLeague(string str)
        {
            string[] strArray = str.Split('-');
            return strArray[1].Trim();
        }
        protected override int GetBO(string str)
        {
            return 1;
        }
        protected override int GetGameIndex(string gameId)
        {
            gameId = gameId.ToLower();
            if (gameIds.Contains(gameId.ToLower()))
            {
                return gameIds.IndexOf(gameId);
            }
            //gameIds.Add(gameId);
            return INVALID_INDEX;
        }
        public override void GrabAndParseHtml()
        {
            try
            {
                int nTryCount = 0;
                string uri = Config.GetString(StaticData.SN_URL, "Uri", configFile, "Uri");
                RequestOptions op = new RequestOptions(uri);
                op.Method = Config.GetString(StaticData.SN_URL, "Method", configFile, "GET");
                op.Accept = Config.GetString(StaticData.SN_URL, "Accept", configFile, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                op.Referer = Config.GetString(StaticData.SN_URL, "Referer", configFile, "");
                op.RequestCookies = Config.GetString(StaticData.SN_URL, "Cookie", configFile, "");
                op.XHRParams = Config.GetString(StaticData.SN_URL, "XHRParams", configFile, "");
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

                //赛前
                if (betItems.Count > 0)
                {
                    ShowLog(string.Format("页面解析成功，解析个数：{0}！", betItems.Count));
                }
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
            JToken data = main["data"];
            JToken today = main["today"];
            foreach (var record in today)
            {
                try
                {
                    string title = record[37].ToString();
                    string gameName = GetGameID(title);
                    int gameIndex = GetGameIndex(gameName);
                    string team1_name = record[38].ToString();
                    string team2_name = record[39].ToString();
                    if(gameIndex == INVALID_INDEX)
                    {
                        continue;
                    }
                    if(team1_name.Contains('(') && team2_name.Contains(')'))
                    {
                        continue;
                    }
                    string gameLeague = GetGameLeague(title);
                    int team1_index = GetTeamIndex(gameIndex , team1_name);
                    int team2_index = GetTeamIndex(gameIndex , team2_name);

                    double odds1 = Convert.ToDouble(record[17].ToString());
                    double odds2 = Convert.ToDouble(record[18].ToString());
                    if(odds1 < 0 || odds2 < 0)
                    {
                        continue;
                    }
                    //时间
                    var monthDay = record[56];
                    string date = null;
                    string strTime = null;
                    DateTime gameTime;
                    if (monthDay != null)
                    {
                        var strMonthDay = monthDay.ToString().Split('/');
                        var month = strMonthDay[0].Trim();
                        var day = strMonthDay[1].Trim();
                        date = DateTime.Now.Year + "-" + month + "-" + day;
                    }
                    var time = record[53];
                    if (time != null)
                    {
                        strTime = time.ToString() + ":00";
                    }
                    string dateTime = date + " " + strTime;
                    gameTime = Convert.ToDateTime(dateTime);

                    //bo
                    // listRecords.Add(record.ToString());
                    //}
                    //foreach(var match in matches)
                    //{
                    BetItem b = new BetItem();
                    b.webID = webID;
                    b.sportID = sportID;
                    b.type = BetType.BT_TEAM;
                    b.pID1 = team1_index;
                    b.pID2 = team2_index;
                    b.pName1 = team1_name;
                    b.pName2 = team2_name;
                    b.pAbbr1 = team1_name;
                    b.pAbbr2 = team2_name;
                    b.odds1 = odds1;
                    b.odds2 = odds2;
                    b.gameID = gameIndex;
                    b.gameName = gameStaticNames[gameIndex];
                    b.leagueName1 = gameLeague;
                    b.leagueName2 = gameLeague;
                    b.bo = 1;
                    b.time = gameTime;
                    betItems.Add(b);
                    if (betItems.Count == 37)
                    {
                        int a = 0;
                    }    
                    
                }
                catch (Exception e)
                {
                    DebugLog error = new DebugLog();
                    error.webID = webID;
                    error.level = ErrorLevel.EL_WARNING;
                    error.message = betItems.Count + ":" + e.Message;
                    ShowLog(error);
                }
            }

            foreach (var record in data)
            {
                try
                {
                    string title = record[37].ToString();
                    string gameName = GetGameID(title);
                    int gameIndex = GetGameIndex(gameName);
                    string team1_name = record[38].ToString();
                    string team2_name = record[39].ToString();
                    if (gameIndex == INVALID_INDEX)
                    {
                        continue;
                    }
                    if (team1_name.Contains('(') && team2_name.Contains(')'))
                    {
                        continue;
                    }
                    string gameLeague = GetGameLeague(title);
                    int team1_index = GetTeamIndex(gameIndex, team1_name);
                    int team2_index = GetTeamIndex(gameIndex, team2_name);

                    double odds1 = Convert.ToDouble(record[17].ToString());
                    double odds2 = Convert.ToDouble(record[18].ToString());
                    if (odds1 < 0 || odds2 < 0)
                    {
                        continue;
                    }
                    //时间
                    var monthDay = record[56];
                    string date = null;
                    string strTime = null;
                    DateTime gameTime;
                    if (monthDay != null)
                    {
                        var strMonthDay = monthDay.ToString().Split('/');
                        var month = strMonthDay[0].Trim();
                        var day = strMonthDay[1].Trim();
                        date = DateTime.Now.Year + "-" + month + "-" + day;
                    }
                    var time = record[53];
                    if (time != null)
                    {
                        strTime = time.ToString() + ":00";
                    }
                    string dateTime = date + " " + strTime;
                    gameTime = Convert.ToDateTime(dateTime);

                    //bo
                    // listRecords.Add(record.ToString());
                    //}
                    //foreach(var match in matches)
                    //{
                    BetItem b = new BetItem();
                    b.webID = webID;
                    b.sportID = sportID;
                    b.type = BetType.BT_TEAM;
                    b.pID1 = team1_index;
                    b.pID2 = team2_index;
                    b.pName1 = team1_name;
                    b.pName2 = team2_name;
                    b.pAbbr1 = team1_name;
                    b.pAbbr2 = team2_name;
                    b.odds1 = odds1;
                    b.odds2 = odds2;
                    b.gameID = gameIndex;
                    b.gameName = gameStaticNames[gameIndex];
                    b.leagueName1 = gameLeague;
                    b.leagueName2 = gameLeague;
                    b.bo = 1;
                    b.time = gameTime;
                    betItems.Add(b);
                    if (betItems.Count == 37)
                    {
                        int a = 0;
                    }

                }
                catch (Exception e)
                {
                    DebugLog error = new DebugLog();
                    error.webID = webID;
                    error.level = ErrorLevel.EL_WARNING;
                    error.message = betItems.Count + ":" + e.Message;
                    ShowLog(error);
                }
            }
            return betItems.Count;
        }
    }
     
}
