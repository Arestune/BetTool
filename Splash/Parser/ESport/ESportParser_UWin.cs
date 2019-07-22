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
        protected override void Init()
        {
            webID = WebID.WID_UWIN;
            base.Init();
        }
        public override void GrabAndParseHtml()
        {
            //今日
            int i = 0;
            do
            {
                long dateTime = 1563811200;
                //long dateTime = Util.GetCurrentTimeStamp();
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
            } while (i== 1);

            //赛前


            if (betItems.Count > 0)
            {
                ShowLog(string.Format("页面解析成功，解析个数：{0}！", betItems.Count));
            }
        }
        public override int Parse()
        {
            try
            {
                //string fullName = "D:\\1.json";
                //System.IO.StreamReader sr1 = new StreamReader(fullName);
                //html = sr1.ReadToEnd();
                if(string.IsNullOrEmpty(html))
                {
                    return 0;
                }
                JObject main = JObject.Parse(html);
                JToken data = main["data"];
                int size = Convert.ToInt32(data["size"]);
                JToken records = data["records"];
                foreach(var record in records)
                {
                    var id = record["id"].ToString();
                    var gameId = record["gameId"].ToString();
                    var league = record["league"]["name"].ToString();
                    var gameIndex = GetGameIndex(gameId);
                    //过滤掉进行中的比赛
                    if (record["status"].ToString() == "ongoing")
                    {
                        continue;
                    }
                    //teams
                    JToken teams = record["teams"];
                    var team1_name = teams[0]["name"].ToString().Trim();
                    var team2_name = teams[1]["name"].ToString().Trim();
                    var team1_index = GetTeamIndex(gameIndex, team1_name);
                    var team2_index = GetTeamIndex(gameIndex, team2_name);

                    //若有新的队发现，则暂时不做处理
                    //if (team1_index == INVALID_ID || team2_index == INVALID_ID)
                    //{
                    //    continue;
                    //}
                    //odds
                    JToken odds = record["odds"];
                    if(odds != null)
                    {
                        JToken betOptions = odds["betOptions"];
                        if(betOptions != null)
                        {
                            var odd1 = Convert.ToDouble(betOptions[0]["odd"]);
                            var odd2 = Convert.ToDouble(betOptions[1]["odd"]);
                            BetItem b = new BetItem();
                            b.webID = webID;
                            b.type = BetType.BT_TEAM;
                            b.pID1 = team1_index;
                            b.pID2 = team2_index;
                            b.pName1 = team1_name;
                            b.pName2 = team2_name;
                            b.odds1 = odd1;
                            b.odds2 = odd2;
                            b.gameID = gameIndex;
                            b.gameName = gameStaticNames[gameIndex];
                            b.leagueName1 = league;
                            b.leagueName2 = league;
                            betItems.Add(b);
                        }
                    }
                }
                ShowLog(string.Format("页面解析成功，解析个数：{0}！", betItems.Count));
            }
            catch(Exception e)
            {
                LogInfo error = new LogInfo();
                error.webID = webID;
                error.level = ErrorLevel.EL_WARNING;
                error.message = e.Message;
                ShowLog(error);
            }

            return betItems.Count;
        }
    }
     
}
