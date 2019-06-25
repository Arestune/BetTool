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
    class ESportParser_Yayou:ESportParser
    {
        protected override void Init()
        {
            webID = WebID.WID_YAYOU;
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
                gameIds.Add(Util.GetCommentInt(eItem).ToString());
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
        protected override int GetGameIndex(string strGameId)
        {
            if (gameIds.Contains(strGameId))
            {
                return gameIds.IndexOf(strGameId);
            }
            gameIds.Add(strGameId);
            IniUtil.WriteString(StaticData.SN_GAME_ID, string.Format("G{0}", gameIds.Count - 1), strGameId, configFile);
            return INVALID_INDEX;
        }
        protected override int GetTeamIndex(int gameIndex, string strTeam)
        {
           
            if (teamIds.ContainsKey(gameIndex))
            {
                if (teamIds[gameIndex].ContainsKey(strTeam))
                {
                    return teamIds[gameIndex][strTeam];
                }
                else
                {
                    int curId = INVALID_INDEX;
                    teamIds[gameIndex].Add(strTeam, curId);
                    IniUtil.WriteString(gameIndex.ToString(), string.Format("T{0}", teamIds[gameIndex].Count - 1), string.Format("{0},{1}",
                        strTeam, curId), configFile);
                    ShowLog(string.Format("新增[{0}]队伍:{1},请在配置文件配置ID！",gameIndex, strTeam), ErrorLevel.EL_WARNING);
                    return curId;
                }
            }
            else
            {
                int curId = INVALID_INDEX;
                var teamList = new Dictionary<string, int>();
                teamList.Add(strTeam, curId);
                teamIds.Add(gameIndex, teamList);
                IniUtil.WriteString(gameIndex.ToString(), string.Format("T{0}", teamList.Count-1), string.Format("{0},{1}",
                        strTeam, curId), configFile);
                ShowLog(string.Format("新增[{0}]队伍:{1},请在配置文件配置ID！", gameIndex,strTeam), ErrorLevel.EL_WARNING);
                return curId;
            }
        }
        public override void Parse()
        {
            try
            {
                //string fullName = "D:\\1.json";
                //System.IO.StreamReader sr1 = new StreamReader(fullName);
                //html = sr1.ReadToEnd();
                if(string.IsNullOrEmpty(html))
                {
                    return;
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
                    var team1_name = Util.GetGBKString(teams[0]["name"].ToString());
                    var team2_name = Util.GetGBKString(teams[1]["name"].ToString());
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
                            b.sportID = sportID;
                            b.type = BetType.BT_TEAM;
                            b.pID1 = team1_index;
                            b.pID2 = team2_index;
                            b.pName1 = team1_name;
                            b.pName2 = team2_name;
                            b.odds1 = odd1;
                            b.odds2 = odd2;
                            b.gameID = gameIndex;
                            b.gameName = gameNames[gameIndex];
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
        }
    }
     
}
