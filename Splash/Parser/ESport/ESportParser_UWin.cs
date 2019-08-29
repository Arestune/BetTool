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
    class ESportParser_UWin:ESportParser
    {
        string dateTime = "1";
        protected override void Init()
        {
            webID = WebID.WID_UWIN;
            base.Init();
        }
        public override void GrabAndParseHtml()
        {
            dateTime = "1";
            int grabDay = Convert.ToInt32(Config.GetString(StaticData.SN_URL, "Days", configFile, "4"));
            //今日
            int i = 0;
            do
            {
                int nTryCount = 0;
                string uriFormat = Config.GetString(StaticData.SN_URL, "Uri", configFile, "Uri");
                string uri = string.Format(uriFormat, dateTime);
                RequestOptions op = new RequestOptions(uri);
                op.Method = Config.GetString(StaticData.SN_URL, "Method", configFile, "GET");
                op.Accept = Config.GetString(StaticData.SN_URL, "Accept", configFile, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                op.Referer = Config.GetString(StaticData.SN_URL, "Referer", configFile, "");
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
                i++;
            } while (i < grabDay);

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
            JToken data = main["data"];
            dateTime = data["next"].ToString();
            JToken matches = data["matches"];
            JToken records = data["records"];
            foreach (var record in records)
            {
                try
                {
                    string recordId = record.ToString();
                    JToken match = matches[recordId];
                    // listRecords.Add(record.ToString());
                    //}
                    //foreach(var match in matches)
                    //{
                    var bo = int.Parse(match["BO"].ToString());
                    var gameId = match["Cid"].ToString();
                    var league = match["LeagueName"].ToString();
                    var gameIndex = GetGameIndex(gameId);
                    if (gameIndex == INVALID_INDEX)
                    {
                        continue;
                    }
                    //过滤掉延期的比赛
                    if (!Boolean.Parse(match["Visible"].ToString()))
                    {
                        continue;
                    }
                    //teams
                    var team1_name = match["HomeTeamName"].ToString().Trim();
                    var team2_name = match["AwayTeamName"].ToString().Trim();
                    var team1_index = GetTeamIndex(gameIndex, team1_name);
                    var team2_index = GetTeamIndex(gameIndex, team2_name);
                    var timestamp = Convert.ToInt64(match["StartTimeInt"].ToString());
                    var time = new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(timestamp);
                    //odds
                    JToken match_winner = match["match_winner"];
                    if (match_winner != null)
                    {
                        JArray match_winner_records = JArray.Parse(match_winner["records"].ToString());
                        if(match_winner_records.Count == 0)
                        {
                            continue;
                        }
                        JToken match_winner_records_array_0 = match_winner_records[0];
                        JArray match_winner_records_array_0_0_data = JArray.Parse(match_winner_records_array_0["data"].ToString());
                        foreach (var market in match_winner_records_array_0_0_data)
                        {
                            string match_winner_data = market.ToString();
                            JToken odds = match_winner["markets"][match_winner_data]["odds"];
                            if (odds["left"] == null)
                            {
                                continue;
                            }
                            JArray leftArray = JArray.Parse(odds["left"].ToString());
                            JArray rightArray = JArray.Parse(odds["right"].ToString());
                            if(leftArray.Count == 0 || rightArray.Count == 0)
                            {
                                continue;
                            }
                            string left = leftArray[0].ToString();
                            string right = rightArray[0].ToString();
                            JToken odd_lists = match_winner["odd_lists"];
                            if (odd_lists != null)
                            {
                                var odd1 = Convert.ToDouble(odd_lists[left]["Value"]);
                                var odd2 = Convert.ToDouble(odd_lists[right]["Value"]);
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
                                b.odds1 = odd1;
                                b.odds2 = odd2;
                                b.gameID = gameIndex;
                                b.gameName = gameStaticNames[gameIndex];
                                b.leagueName1 = league;
                                b.leagueName2 = league;
                                b.bo = bo;
                                b.time = time;
                                betItems.Add(b);
                                if (betItems.Count == 37)
                                {
                                    int a = 0;
                                }
                            }
                        }
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
                //ShowLog(string.Format("页面解析成功，解析个数：{0}！", betItems.Count));
            }
            return betItems.Count;
        }
    }
     
}
