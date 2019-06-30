using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Splash.Item;
using Splash.Parser;
using Splash.Tool;
namespace Splash.Parser.ESport
{
    class ESportParser_Ray:ESportParser
    {
        const int PAGE_MAX_NUM = 30;
        protected override void Init()
        {
            webID = WebID.WID_RAY;
            base.Init();
        }
        protected override SecurityProtocolType GetSecurityProtocal()
        {
            //Get the assembly that contains the internal class

            System.Reflection.Assembly aNetAssembly = System.Reflection.Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if (aNetAssembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type aSettingsType = aNetAssembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (aSettingsType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created allready the property will create it for us.
                    object anInstance = aSettingsType.InvokeMember("Section",
                      System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.NonPublic, null, null, new object[] { });

                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                        System.Reflection.FieldInfo aUseUnsafeHeaderParsing = aSettingsType.GetField("useUnsafeHeaderParsing", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, true);
                        }
                    }
                }
            }
            return SecurityProtocolType.Tls12;
        }
        public override void GrabAndParseHtml()
        {
            //今日
            int pageNum = 0;
            int page = 1;
            int match_type = 2;
            do
            {
                int nTryCount = 0;
                string uriFormat = Config.GetString(StaticData.SN_URL, "Uri", configFile, "Uri");
                string uri = string.Format(uriFormat, page, match_type);
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
                    pageNum = Parse();
                }
                page++;
            } while (pageNum == PAGE_MAX_NUM);

            //赛前
            pageNum = 0;
            page = 1;
            match_type = 3;
            do
            {
                int nTryCount = 0;
                string uriFormat = Config.GetString(StaticData.SN_URL, "Uri", configFile, "Uri");
                string uri = string.Format(uriFormat, page, match_type);
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
                    pageNum = Parse();
                }
                page++;
            } while (pageNum == PAGE_MAX_NUM);

            if(betItems.Count> 0)
            {
                ShowLog(string.Format("页面解析成功，解析个数：{0}！", betItems.Count));
            }
        }
        protected override string GetLeague1Name(string str)
        {
            int first = str.IndexOf('(');
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
            return "NULL";
        }
        protected override DateTime GetGameTime(string strTime)
        {
            DateTime time = Convert.ToDateTime(strTime);
            return time;
        }
        protected override int GetBO(string str)
        {
            Match match = Regex.Match(str, @"\bo(\d+)",RegexOptions.IgnoreCase);
            if(match != null && match.Groups[1] != null)
            {
                int bo = 0;
                if (int.TryParse(match.Groups[1].ToString(),out bo))
                {
                    return bo;
                }
            }
            return 1;
        }
       
        public override int Parse()
        {
            int pageNum = 0;
            try
            {
                if (string.IsNullOrEmpty(html))
                {
                    return 0;
                }
                JObject main = JObject.Parse(html);
                var results = JArray.Parse(main["result"].ToString());
                pageNum = results.Count;
                foreach (var result in results)
                {
                    try
                    {
                        //odds
                        var odds = result["odds"];
                        if (odds == null)
                        {
                            continue;
                        }
                        var status = result["status"];
                        if(status != null && Convert.ToInt32(status.ToString()) == 2)
                        {
                            continue;
                        }
                        var odds1 = Convert.ToDouble(odds[0]["odds"].ToString());
                        var odds2 = Convert.ToDouble(odds[1]["odds"].ToString());
                       
                        var leagueName = result["tournament_name"].ToString();
                        var gameTime = GetGameTime(result["start_time"].ToString());
                        var gameName = GetGameID(result["game_name"].ToString());
                        var bo = GetBO(result["round"].ToString());
                        var gameIndex = GetGameIndex(gameName);
                        if(gameIndex >= gameStaticNames.Count || gameIndex < 0)
                        {
                            continue;
                        }
                        //team
                        var team = result["team"];
                        var team_name1 = odds[0]["name"].ToString();
                        var team_name2 = odds[1]["name"].ToString();
                        var team_short_name1 = team_name1;//Trim();
                        var team_short_name2 = team_name2;//Trim();
                        var team_id1 = GetTeamIndex(gameIndex, team_name1);
                        var team_id2 = GetTeamIndex(gameIndex, team_name2);

                        BetItem b = new BetItem();
                        b.sportID = sportID;
                        b.webID = webID;
                        b.type = BetType.BT_TEAM;
                        b.pID1 = team_id1;
                        b.pID2 = team_id2;
                        b.pName1 = team_name1;
                        b.pName2 = team_name2;
                        b.pAbbr1 = team_short_name1;
                        b.pAbbr2 = team_short_name2;
                        b.odds1 = odds1;
                        b.odds2 = odds2;
                        b.gameID = gameIndex;
                        b.handicap = 0;
                        b.gameName = gameStaticNames[gameIndex];
                        b.time = gameTime;
                        b.leagueName1 = leagueName;
                        betItems.Add(b);
                        if(betItems.Count == 135)
                        {
                            int a = 1;
                            a++;
                        }
                    }
                    catch (Exception e)
                    {
                        LogInfo error = new LogInfo();
                        error.webID = webID;
                        error.level = ErrorLevel.EL_WARNING;
                        error.message = e.Message;
                        ShowLog("解析第" + betItems.Count + "个Error:" + error.message);
                    }
                }
            }
            catch (Exception e)
            {
                LogInfo error = new LogInfo();
                error.webID = webID;
                error.level = ErrorLevel.EL_WARNING;
                error.message = e.Message;
                ShowLog("解析第" + betItems.Count + "个Error:" + error.message);
            }

            return pageNum;
        }
    }    
}
