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
        protected class BTI_MATCH_INFO
        {
            public string matchItemId;
            public DateTime matchTime;
            public int gameIndex;
            public string gameLeague;
            public BTI_MATCH_INFO(string id,DateTime time, int index,string league)
            {
                matchItemId = id;
                matchTime = time;
                gameIndex = index;
                gameLeague = league;
            }
        }
        protected override void Init()
        {
            webID = WebID.WID_BTI;
            base.Init();
        }

        protected double GetOdds(int usaOddFormat)
        {
            if(usaOddFormat < 0)
            {
                return (-100.0f /(double) usaOddFormat) + 1;
            }
            return ( ((double)usaOddFormat)/100.0f) + 1;
        }

        protected bool GetGameIndexAndLeague(string title,out int gameIndex,out string gameLeague)
        {
            title = title.ToLower();
            title = title.Replace('-',' ');
            gameIndex = INVALID_INDEX;
            string gameFinalId = null;
            foreach (var gameId in gameIds)
            {
                if(title.Contains(gameId))
                {
                    gameIndex = gameIds.IndexOf(gameId);
                    gameFinalId = gameId;
                    break;
                }
            }
            if(gameIndex == INVALID_INDEX)
            {
                gameLeague = "";
                return false;
            }
            gameLeague = title.Substring(title.IndexOf(gameFinalId) + gameFinalId.Length + 1);
            return true;
        }
        public override void GrabAndParseHtml()
        {
            try
            {
                //今日
                int nTryCount = 0;
                string uri = Config.GetString(StaticData.SN_URL, "Uri", configFile, "");
                RequestOptions op = new RequestOptions(uri);
                op.Method = Config.GetString(StaticData.SN_URL, "Method", configFile, "GET");
                op.Accept = Config.GetString(StaticData.SN_URL, "Accept", configFile, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                op.Referer = Config.GetString(StaticData.SN_URL, "Referer", configFile, "");
                op.RequestCookies = Config.GetString(StaticData.SN_URL, "Cookie", configFile, "");
                op.XHRParams = Config.GetString(StaticData.SN_URL, "XHRParams", configFile, "");
                op.WebHeader.Add("requesttarget: AJAXService");
                //获取网页
                html = RequestAction(op);
                 while (string.IsNullOrEmpty(html) && nTryCount < MAX_TRY_COUNT)
                {
                    html = RequestAction(op);
                    nTryCount++;
                }
                if (nTryCount == MAX_TRY_COUNT)
                {
                    ShowLog("今日数据抓取失败！");
                    html = "";
                }
                else
                {
                    Parse();
                }

                //早盘
                nTryCount = 0;
                string uri2 = Config.GetString(StaticData.SN_URL, "Uri2", configFile, "");
                RequestOptions op2 = new RequestOptions(uri2);
                op2.Method = Config.GetString(StaticData.SN_URL, "Method", configFile, "GET");
                op2.Accept = Config.GetString(StaticData.SN_URL, "Accept", configFile, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                op2.Referer = Config.GetString(StaticData.SN_URL, "Referer", configFile, "");
                op2.RequestCookies = Config.GetString(StaticData.SN_URL, "Cookie", configFile, "");
                op2.XHRParams = Config.GetString(StaticData.SN_URL, "XHRParams", configFile, "");
                op2.WebHeader.Add("requesttarget: AJAXService");
                //获取网页
                html = RequestAction(op2);
                while (string.IsNullOrEmpty(html) && nTryCount < MAX_TRY_COUNT)
                {
                    html = RequestAction(op);
                    nTryCount++;
                }
                if (nTryCount == MAX_TRY_COUNT)
                {
                    ShowLog("早盘数据抓取失败！");
                    html = "";
                }
                else
                {
                    Parse();
                }
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
            string postQueryString = "requestString=";
            Dictionary<string, BTI_MATCH_INFO> listMatchItemIDs = new Dictionary<string, BTI_MATCH_INFO>();
            if (string.IsNullOrEmpty(html))
            {
                return 0;
            }
            JArray main = JArray.Parse(html);
            JArray matches = JArray.Parse(main[2].ToString());
            foreach(var match in matches)
            {
                //string team1_name = match[1].ToString();
                //string team2_name = match[2].ToString();
                string gameLeague;
                int gameIndex;
                DateTime time = Convert.ToDateTime(match[4].ToString()).AddHours(8);
                string title = match[11].ToString();
                if(GetGameIndexAndLeague(title,out gameIndex,out gameLeague))
                {
                    JArray matchItemArray = JArray.Parse(match[5].ToString());
                    if (matchItemArray != null)
                    {
                        foreach (var matchItems in matchItemArray)
                        {
                            string matchItemId = matchItems[0].ToString();
                            postQueryString += matchItemId + "%40";
                            listMatchItemIDs.Add(matchItemId, new BTI_MATCH_INFO(matchItemId, time, gameIndex, gameLeague));
                        }
                    }     
                }
            }
            postQueryString = postQueryString.Substring(0, postQueryString.Length - 3);

            //寻求赔率页面
            int nTryCount = 0;
            string uri = "https://jbosports.btisports.io/pagemethods.aspx/UpdateAsianSkinEvents";
            RequestOptions op = new RequestOptions(uri);
            op.Method = "POST";
            op.Accept = "*/*";
            op.XHRParams = postQueryString;
            op.WebHeader.Add("requesttarget: AJAXService");
            op.ContentType = "application/x-www-form-urlencoded";
            //获取网页
            string oddsHTML = RequestAction(op);
            while (string.IsNullOrEmpty(oddsHTML) && nTryCount < MAX_TRY_COUNT)
            {
                oddsHTML = RequestAction(op);
                nTryCount++;
            }
            if (nTryCount == MAX_TRY_COUNT)
            {
                ShowLog("赔率页面抓取失败！");
                oddsHTML = "";
                return 0;
            }
            else
            {
                if (string.IsNullOrEmpty(oddsHTML))
                {
                    return 0;
                }
                JArray matchItems = JArray.Parse(oddsHTML);
                foreach(var matchItem in matchItems)
                {
                    try
                    {
                        string curMap = matchItem[6].ToString();
                        if(curMap != "Full Time")
                        {
                            continue;
                        }
                        string matchItemId = matchItem[0].ToString();
                        int gameIndex = listMatchItemIDs[matchItemId].gameIndex;
                        string gameLeague = listMatchItemIDs[matchItemId].gameLeague;
                        DateTime gameTime = listMatchItemIDs[matchItemId].matchTime;
                        string team1_name = matchItem[4].ToString();
                        string team2_name = matchItem[5].ToString();
                        int team1_index = GetTeamIndex(gameIndex, team1_name);
                        int team2_index = GetTeamIndex(gameIndex, team2_name);
                        JArray oddsArray = JArray.Parse(matchItem[2].ToString());
                        if (oddsArray != null && oddsArray.Count > 0)
                        {
                            JArray oddsArray1 = JArray.Parse(oddsArray[0].ToString());
                            JArray oddsArray2 = JArray.Parse(oddsArray1[1].ToString());
                            if (oddsArray2 != null && oddsArray2.Count > 0)
                            {
                                string odds1String = oddsArray2[1].ToString();
                                string odds2String = oddsArray2[5].ToString();
                                double odds1 = GetOdds(int.Parse(odds1String));
                                double odds2 = GetOdds(int.Parse(odds2String));
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
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    catch(Exception e)
                    {
                        DebugLog error = new DebugLog();
                        error.webID = webID;
                        error.level = ErrorLevel.EL_WARNING;
                        error.message = betItems.Count + ":" + e.Message;
                        ShowLog(error);
                    }
                }
            }
            return betItems.Count;
        }
    }
     
}
