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
    class ESportParser_Pingbo:ESportParser
    {
        protected override void Init()
        {
            webID = WebID.WID_Pingbo;
            base.Init();
        }
        protected override string GetLeague1Name(string str)
        {
            string league = null;
            var pair = str.Split('-');
            if (pair.Length == 1)
            {
                return league;
            }
            else
            {
                league = pair[0].ToString().Trim();
            }
            league = pair[1].ToString().Trim();
            return league;
        }

        protected override string GetGameID(string str)
        {
            var pair = str.Split('-');
            string league = null;
            if(pair.Length == 1)
            {
                if(str.Contains("英雄联盟"))
                {
                    return "英雄联盟";
                }
            }
            else
            {
                league = pair[0].ToString().Trim();
            }
            return league;
        }
        protected override int GetGameIndex(string gameId)
        {
            gameId = gameId.ToLower();
            if (gameIds.Contains(gameId.ToLower()))
            {
                return gameIds.IndexOf(gameId);
            }
            else
            {
                if(gameId == "英雄联盟")
                {
                    return 0;
                }
                else if (gameId == "王者荣耀")
                {
                    return 3;
                }
                else if(gameId == "星际争霸2")
                {
                    return 6;
                }
                else if(gameId == "彩虹六号")
                {
                    return 8;
                }
                else if (gameId == "炉石传说")
                {
                    return 11;
                }
                else if (gameId == "火箭联盟")
                {
                    return 7;
                }
            }
            return INVALID_INDEX;
        }
        //public override void GrabAndParseHtml()
        //{
        //    //今日
        //    int nTryCount = 0;
        //    string dateTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        //    string uriFormat = Config.GetString(StaticData.SN_URL, "Uri", configFile, "Uri");
        //    string uri = string.Format(uriFormat, dateTime);
        //    RequestOptions op = new RequestOptions(uri);
        //    op.Method = Config.GetString(StaticData.SN_URL, "Method", configFile, "GET");
        //    //获取网页
        //    html = RequestAction(op);
        //    while (string.IsNullOrEmpty(html) && nTryCount < MAX_TRY_COUNT)
        //    {
        //        html = RequestAction(op);
        //        nTryCount++;
        //    }
        //    if (nTryCount == MAX_TRY_COUNT)
        //    {
        //        ShowLog("抓取失败！");
        //        html = "";
        //    }
        //    else
        //    {
        //        Parse();
        //    }
             
        //    //赛前
        //    if (betItems.Count > 0)
        //    {
        //        ShowLog(string.Format("页面解析成功，解析个数：{0}！", betItems.Count));
        //    }
        //}
        //public override int Parse()
        //{
        //    //string fullName = "D:\\1.json";
        //    // System.IO.StreamReader sr1 = new StreamReader(fullName);
        //    // html = sr1.ReadToEnd();
        //    if (string.IsNullOrEmpty(html))
        //    {
        //        return 0;
        //    }
        //    JObject main = JObject.Parse(html);
        //    JArray n =  JArray.Parse(main["n"].ToString());
        //    JToken n0 = n[0];
        //    JArray data = JArray.Parse(n0[2].ToString());
        //    foreach(var record in data)
        //    {
        //        try
        //        {
        //            string title = record[1].ToString();
        //            string gameId = GetGameID(title);
        //            string league = GetLeague1Name(title);
        //            int gameIndex = GetGameIndex(gameId);
        //            if(gameIndex == INVALID_INDEX)
        //            {
        //                continue;
        //            }
        //            var matches = record[2].ToArray();
        //            foreach (var match in matches)
        //            {
        //                string team_name1 = match[1].ToString();
        //                string team_name2 = match[2].ToString();
        //                if (team_name1.Contains("击杀数") || team_name2.Contains("击杀数") || team_name2.Contains("Kills") || team_name1.Contains("Kills"))
        //                {
        //                    continue;
        //                }
        //                int team_index1 = GetTeamIndex(gameIndex, team_name1);
        //                int team_index2 = GetTeamIndex(gameIndex, team_name2);
        //                long timeStamp1000 = Convert.ToInt64(match[4].ToString());
        //                long timeStamp = timeStamp1000 / 1000;
        //                DateTime time = new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(timeStamp);
        //                int bo = 1;
        //                var oddsString = match[8];
        //                int index = 0;
        //                JToken[] oddsItem = null;
        //                if (oddsString[index.ToString()] == null)
        //                {
        //                    continue;
        //                }
        //                oddsItem = oddsString[index.ToString()].ToArray();
        //                //独赢
        //                if(oddsItem[2] != null && oddsItem[2].ToString() != "" && oddsItem[2].ToArray() != null)
        //                {
        //                    var oddsStr = oddsItem[2].ToString();
        //                    var oddsPair = oddsItem[2].ToArray();
        //                    var odds1 = Convert.ToDouble(oddsPair[1]);
        //                    var odds2 = Convert.ToDouble(oddsPair[0]);
        //                    BetItem b = new BetItem();
        //                    b.webID = webID;
        //                    b.sportID = sportID;
        //                    b.type = BetType.BT_TEAM;
        //                    b.pID1 = team_index1;
        //                    b.pID2 = team_index2;
        //                    b.pName1 = team_name1;
        //                    b.pName2 = team_name2;
        //                    b.pAbbr1 = team_name1;
        //                    b.pAbbr2 = team_name2;
        //                    b.odds1 = odds1;
        //                    b.odds2 = odds2;
        //                    b.gameID = gameIndex;
        //                    b.gameName = gameStaticNames[gameIndex];
        //                    b.leagueName1 = league;
        //                    b.leagueName2 = league;
        //                    b.bo = bo;
        //                    b.time = time;
        //                    b.handicap = 0;
        //                    betItems.Add(b);
        //                }

        //                if (betItems.Count == 2)
        //                {
        //                    int a = 0;
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            DebugLog error = new DebugLog();
        //            error.webID = webID;
        //            error.level = ErrorLevel.EL_WARNING;
        //            error.message = betItems.Count + ":" + e.Message;
        //            ShowLog(error);
        //        }
        //    }
          
        //    return betItems.Count;
        //}
        public override void GrabAndParseHtml()
        {
            //int grabDay = Convert.ToInt32(Config.GetString(StaticData.SN_URL, "Days", configFile, "4"));
            ////今日
            //int i = 0;
            //do
            //{
            //    int nTryCount = 0;
            //    string uriFormat = Config.GetString(StaticData.SN_URL, "Uri", configFile, "Uri");
            //    // string uri = string.Format(uriFormat, dateTime);
            //    RequestOptions op = new RequestOptions(uriFormat);
            //    op.Method = Config.GetString(StaticData.SN_URL, "Method", configFile, "GET");
            //    //获取网页
            //    html = RequestAction(op);
            //    while (string.IsNullOrEmpty(html) && nTryCount < MAX_TRY_COUNT)
            //    {
            //        html = RequestAction(op);
            //        nTryCount++;
            //    }
            //    if (nTryCount == MAX_TRY_COUNT)
            //    {
            //        ShowLog("抓取失败！");
            //        html = "";
            //    }
            //    else
            //    {
            //        Parse();
            //    }
            //    i++;
            //} while (i < 1);

            ////赛前
            //if (betItems.Count > 0)
            //{
            //    ShowLog(string.Format("页面解析成功，解析个数：{0}！", betItems.Count));
            //}

            //今日
            int nTryCount = 0;
            string dateTime = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string uriFormat = Config.GetString(StaticData.SN_URL, "Uri", configFile, "Uri");
            string uri = string.Format(uriFormat, dateTime);
            RequestOptions op = new RequestOptions(uri);
            op.Method = Config.GetString(StaticData.SN_URL, "Method", configFile, "GET");
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
        public override int Parse()
        {
            //string fullName = "D:\\1.json";
            // System.IO.StreamReader sr1 = new StreamReader(fullName);
            // html = sr1.ReadToEnd();
            if (string.IsNullOrEmpty(html))
            {
                return 0;
            }
            JObject main = JObject.Parse(html);
            JArray n = JArray.Parse(main["n"].ToString());
            JToken n0 = n[0];
            JArray data = JArray.Parse(n0[2].ToString());
            foreach (var record in data)
            {
                try
                {
                    string title = record[1].ToString();
                    string gameId = GetGameID(title);
                    string league = GetLeague1Name(title);
                    int gameIndex = GetGameIndex(gameId);
                    if (gameIndex == INVALID_INDEX)
                    {
                        continue;
                    }
                    var matches = record[2].ToArray();
                    foreach (var match in matches)
                    {
                        string team_name1 = match[1].ToString();
                        string team_name2 = match[2].ToString();
                        if (team_name1.Contains("击杀数") || team_name2.Contains("击杀数") || team_name2.Contains("Kills") || team_name1.Contains("Kills"))
                        {
                            continue;
                        }
                        int team_index1 = GetTeamIndex(gameIndex, team_name1);
                        int team_index2 = GetTeamIndex(gameIndex, team_name2);
                        long timeStamp1000 = Convert.ToInt64(match[4].ToString());
                        long timeStamp = timeStamp1000 / 1000;
                        DateTime time = new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(timeStamp);
                        int bo = Convert.ToInt32(match[7]);
                        var oddsString = match[8];
                        int index = 0;
                        JToken[] oddsItem = null;
                        if (oddsString[index.ToString()] == null)
                        {
                            continue;
                        }
                        oddsItem = oddsString[index.ToString()].ToArray();
                        var oddsPair = oddsItem[0].ToArray();
                        var odds1 = Convert.ToDouble(oddsPair[1]);
                        var odds2 = Convert.ToDouble(oddsPair[0]);
                        var betLimit = Convert.ToInt32(oddsPair[5]);

                        BetItem b = new BetItem();
                        b.webID = webID;
                        b.sportID = sportID;
                        b.type = BetType.BT_TEAM;
                        b.pID1 = team_index1;
                        b.pID2 = team_index2;
                        b.pName1 = team_name1;
                        b.pName2 = team_name2;
                        b.pAbbr1 = team_name1;
                        b.pAbbr2 = team_name2;
                        b.odds1 = odds1;
                        b.odds2 = odds2;
                        b.gameID = gameIndex;
                        b.gameName = gameStaticNames[gameIndex];
                        b.leagueName1 = league;
                        b.leagueName2 = league;
                        b.bo = bo;
                        b.time = time;
                        betItems.Add(b);
                        if (betItems.Count == 36)
                        {
                            int a = 0;
                        }
                        // index++;
                        //do
                        //{

                        //}
                        //while (true);
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
