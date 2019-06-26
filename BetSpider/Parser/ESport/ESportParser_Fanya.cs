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
using BetSpider.Item;
using BetSpider.Parser;
using BetSpider.Tool;
namespace BetSpider.Parser.ESport
{
    class ESportParser_Fanya:ESportParser
    {
        protected override void Init()
        {
            webID = WebID.WID_FANYA;
            base.Init();
        }
        public override void LoadStaticData()
        {
            //GameIds
            int index = 0;
            var eItem = IniUtil.GetString(StaticData.SN_GAME_ID, string.Format("G{0}", index), configFile);
            while (!string.IsNullOrEmpty(eItem))
            {
                index++;
                gameIds.Add(Util.GetCommentString(eItem).ToLower());
                eItem = IniUtil.GetString(StaticData.SN_GAME_ID, string.Format("G{0}", index), configFile);
            }

            //GameNames
            index = 0;
            var gameName = IniUtil.GetString(StaticData.SN_GAME_NAME, string.Format("G{0}", index), configFile);
            while (!string.IsNullOrEmpty(gameName))
            {
                index++;
                gameNames.Add(gameName);
                gameName = IniUtil.GetString(StaticData.SN_GAME_NAME, string.Format("G{0}", index), configFile);
            }

            //Teams
            for (int i = 0; i < gameIds.Count; i++)
            {
                int teamIndex = 0;
                string teamAndId = IniUtil.GetString(i.ToString(), string.Format("T{0}", teamIndex), configFile);
                while (!string.IsNullOrEmpty(teamAndId))
                {
                    var array = teamAndId.Split(',');
                    var teamName = array[0].Trim();
                    var id = Convert.ToInt32(array[1].Trim());
                    if (!teamIds.ContainsKey(i))
                    {
                        Dictionary<string, int> teamList = new Dictionary<string, int>();
                        teamList.Add(teamName, id);
                        teamIds.Add(i, teamList);
                    }
                    else
                    {
                        if (!teamIds[i].ContainsKey(teamName))
                        {
                            teamIds[i].Add(teamName, id);
                        }
                    }
                    teamIndex++;
                    teamAndId = IniUtil.GetString(i.ToString(), string.Format("T{0}", teamIndex), configFile);
                }
            }
        }
        public override void GrabAndParseHtml()
        {
          
            base.GrabAndParseHtml();

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
        protected override string GetGameName(string str)
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
            Match match = Regex.Match(str, @"\[BO(\d+)\]\w");
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
            try
            {
                if (string.IsNullOrEmpty(html))
                {
                    return 0;
                }
                JObject main = JObject.Parse(html);
                var infos = main["info"];
                foreach (var info in infos)
                {
                    try
                    {
                        var title = info["Title"].ToString();
                        var status = info["Status"].ToString();
                        if (title.Contains("时时乐") || title.Contains("英雄乐") || title.Contains("三分乐") || status == "进行中")
                        {
                            continue;
                        }
                        var leagueName = info["League"].ToString();
                        var gameTime = GetGameTime(info["StartAt"].ToString());
                        var gameName = GetGameName(info["Category"].ToString());
                        var bo = GetBO(info["Title"].ToString());
                        var gameIndex = GetGameIndex(gameName);
                        var player = JArray.Parse(info["Player"].ToString());
                        if(player.Count == 0) continue;
                        var team_name1 = player[0]["Name"].ToString().Trim();
                        var team_name2 = player[1]["Name"].ToString().Trim();
                        var team_id1 = GetTeamIndex(gameIndex, team_name1);
                        var team_id2 = GetTeamIndex(gameIndex, team_name2);

                        var bet = JArray.Parse(info["Bet"].ToString());
                        var items = bet[0]["Items"];
                        var odds1 = Convert.ToDouble(items[0]["Odds"].ToString());
                        var odds2 = Convert.ToDouble(items[1]["Odds"].ToString());

                        BetItem b = new BetItem();
                        b.sportID = sportID;
                        b.webID = webID;
                        b.type = BetType.BT_TEAM;
                        b.pID1 = team_id1;
                        b.pID2 = team_id2;
                        b.pName1 = team_name1;
                        b.pName2 = team_name2;
                        b.odds1 = odds1;
                        b.odds2 = odds2;
                        b.gameID = gameIndex;
                        b.leagueName1 = leagueName;
                        b.handicap = 0;
                        b.gameName = gameNames[gameIndex];
                        b.time = gameTime;
                        betItems.Add(b);
                        //if(betItems.Count == 53)
                        //{
                        //    int a = 1;
                        //}
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

            return betItems.Count;
        }
    }    
}
