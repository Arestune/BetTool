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
    class ESportParser_188:ESportParser
    {
        protected override void Init()
        {
            webID = WebID.WID_188;
            base.Init();
        }
        public override void GrabAndParseHtml()
        {
            //今日
            base.GrabAndParseHtml();

            //早盘
            int nTryCount = 0;
            string uri = Config.GetString(StaticData.SN_URL, "Uri", configFile, "Uri");
            RequestOptions op = new RequestOptions(uri);
            op.Method = Config.GetString(StaticData.SN_URL, "Method", configFile, "GET");
            op.Accept = Config.GetString(StaticData.SN_URL, "Accept", configFile, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            op.Referer = Config.GetString(StaticData.SN_URL, "Referer", configFile, "");
            op.RequestCookies = Config.GetString(StaticData.SN_URL, "Cookie", configFile, "");
            op.XHRParams = Config.GetString(StaticData.SN_URL, "XHRParams1", configFile, "");
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

            if(betItems.Count> 0)
            {
                ShowLog(string.Format("页面解析成功，解析个数：{0}！", betItems.Count));
            }

            if (!string.IsNullOrEmpty(responseCookie) && responseCookie.Contains("Xauth"))
            {
                Config.WriteString(StaticData.SN_URL, "Cookie",responseCookie,configFile);
            }
        }
        protected override string GetLeague1Name(string str)
        {
            int first = str.IndexOf('(');
            if(first < 0)
            {
                return str;
            }
            return str.Substring(0,first);
        }
        protected override string GetGameID(string str)
        {
            str = str.ToLower();
            for(int i =0;i<gameIds.Count;i++)
            {
                if (str.Contains(gameIds[i].ToLower()))
                {
                    return gameIds[i];
                }
            }
            if (str.Contains("绝地求生"))
            {
                return "绝地求生";
            }
            if (str.Contains("cs:go"))
            {
                return "cs:go";
            }
            return "NULL";
        }
        protected override int GetBO(string str)
        {
            int first_l = str.LastIndexOf('(');
            int first_r = str.IndexOf('场');
            if(first_l < 0 || first_r < 0)
            {
                return 1;
            }
            return int.Parse(str.Substring(first_l+1, first_r - first_l-1));
        }
        protected override int GetGameIndex(string gameName)
        {
            gameName = gameName.ToLower();
            if (gameIds.Contains(gameName.ToLower()))
            {
                return gameIds.IndexOf(gameName);
            }
            if (gameName.Contains("绝地求生"))
            {
                return 4;
            }
            if (gameName.Contains("cs:go"))
            {
                return 2;
            }
            return INVALID_INDEX;
        }
        public override int Parse()
        {

            if (string.IsNullOrEmpty(html))
            {
                return 0;
            }
            JObject main = JObject.Parse(html);
            JToken n_ot = main["n-ot"];
            if (n_ot == null)
            {
                return 0;
            }
            if (n_ot["egs"] == null)
            {
                return 0;
            }
            JArray egs = JArray.Parse(n_ot["egs"].ToString());

            foreach (var eg in egs)
            {
                try
                {
                    var c = eg["c"];
                    var n = c["n"];
                    var leagueName = GetLeague1Name(n.ToString());
                    var gameName = GetGameID(n.ToString());
                    var bo = GetBO(n.ToString());
                    var gameIndex = GetGameIndex(gameName);
                    var es = JArray.Parse(eg["es"].ToString());
                    foreach (var esSingle in es)
                    {
                        var i = esSingle["i"];
                        var team1_name = i[0].ToString().Trim();
                        var team2_name = i[1].ToString().Trim();
                        var team1_index = GetTeamIndex(gameIndex, team1_name);
                        var team2_index = GetTeamIndex(gameIndex, team2_name);
                        var monthDay = i[4];
                        string date = null;
                        string strTime = null;
                        DateTime gameTime;
                        if (monthDay != null)
                        {
                            var strMonthDay = monthDay.ToString().Split('/');
                            var month = strMonthDay[1].Trim();
                            var day = strMonthDay[0].Trim();
                            date = DateTime.Now.Year + "-" + month + "-" + day;
                        }
                        var time = i[5];
                        if (time != null)
                        {
                            strTime = i[5].ToString() + ":00";
                        }
                        string dateTime = date + " " + strTime;
                        gameTime = Convert.ToDateTime(dateTime);

                        var o = esSingle["o"];
                        var ml = o["ml"];
                        if (ml == null)
                        {
                            continue;
                        }
                        var odds1 = Convert.ToDouble(ml[1]);
                        var odds2 = Convert.ToDouble(ml[3]);

                        BetItem b = new BetItem();
                        b.sportID = sportID;
                        b.webID = webID;
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
                        b.handicap = 0;
                        b.gameName = gameStaticNames[gameIndex];
                        b.leagueName1 = leagueName;
                        b.time = gameTime;
                        b.bo = bo;
                        betItems.Add(b);
                        if (betItems.Count == 9)
                        {
                            int a = 1;
                        }
                    }
                }
                catch (Exception e)
                {
                    DebugLog error = new DebugLog();
                    error.webID = webID;
                    error.level = ErrorLevel.EL_WARNING;
                    error.message = e.Message;
                    ShowLog("解析第" + betItems.Count + "个Error:" + error.message);
                }
            }
            return betItems.Count;
        }
    }
     
}
