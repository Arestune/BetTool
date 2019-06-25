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
    class ESportParser_Yabo:ESportParser
    {
        protected override void Init()
        {
            webID = WebID.WID_YABO;
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
            for(int i =0 ;i<gameIds.Count;i++)
            {
                int teamIndex = 0;
                string teamAndId = IniUtil.GetString(i.ToString(), string.Format("T{0}", teamIndex), configFile);
                while (!string.IsNullOrEmpty(teamAndId))
                {
                     var array = teamAndId.Split(',');
                     var teamName = array[0].Trim();
                    var id = Convert.ToInt32(array[1].Trim());
                    if(!teamIds.ContainsKey(i))
                    {
                        Dictionary<string, int> teamList = new Dictionary<string, int>();
                        teamList.Add(teamName,id);
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
        protected int GetTeamIndex(int gameIndex,string strTeam)
        {
            if (teamIds.ContainsKey(gameIndex))
            {
                if (teamIds[gameIndex].ContainsKey(strTeam))
                {
                    return teamIds[gameIndex][strTeam];
                }
                else
                {
                    int curId = teamIds[gameIndex].Count;
                    teamIds[gameIndex].Add(strTeam, curId);
                    IniUtil.WriteString(gameIndex.ToString(), string.Format("T{0}", curId), string.Format("{0},{1}",
                        strTeam, curId), configFile);
                    return curId;
                }
            }
            else
            {
                int curId = 0;
                var teamList = new Dictionary<string, int>();
                teamList.Add(strTeam, curId);
                teamIds.Add(gameIndex, teamList);
                IniUtil.WriteString(gameIndex.ToString(), string.Format("T{0}", curId), string.Format("{0},{1}",
                        strTeam, curId), configFile);
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
                if (string.IsNullOrEmpty(html))
                {
                    return;
                }
                JObject mObj = JObject.Parse(html);
                JToken d = mObj["d"];
                int index = 0;
                foreach(var b1 in d)
                {
                    var gameIndex= GetGameIndex(b1[8].ToString());
                    var leagueName1 = b1[5][1].ToString();
                    var leagueName2 = b1[1][1].ToString();
                    //IniUtil.WriteString("Game", string.Format("G{0}", index),gameName, configFile);
                    foreach(var b2 in b1[10])
                    {
                        var team1_name = Util.GetGBKString(b2[5][0].ToString());
                        var team2_name = Util.GetGBKString(b2[6][0].ToString());
                        var europeTime = DateTime.Parse(b2[7].ToString());
                        var chinaMatchTime = europeTime.AddHours(8);
                        var localTime = DateTime.Now; 
                        //over time
                        if (DateTime.Compare(localTime, chinaMatchTime) > 0)
                        {
                            continue;
                        }
                        var Bo2 = b2[33].ToString() ;
                        bool isBo2 = Bo2 == "BO2";
                        double betValue = 0.0;
                        if(isBo2)
                        {
                            var winhandcap = b2[28].ToString();
                            int v = Convert.ToInt32(winhandcap.Split('-')[1]);
                            if(winhandcap.Contains("A"))
                            {
                                betValue = -v / 10.0f;
                            }
                            else if(winhandcap.Contains("B"))
                            {
                                betValue = v / 10.0f;
                            }
                        }
                        var team1_index = GetTeamIndex(gameIndex, team1_name);
                        var team2_index = GetTeamIndex(gameIndex, team2_name);
                        JArray b2_10 = JArray.Parse(b2[10].ToString());
                        JToken b2_10_0 = b2_10[0];
                        JToken b2_10_0_6 = b2_10_0[6];
                        JToken b2_10_0_6_4 = b2_10_0_6[4];
                        JToken b2_10_0_6_7 = b2_10_0_6[7];
                        var odds1 = Convert.ToDouble(b2_10_0_6_4);
                        var odds2 = Convert.ToDouble(b2_10_0_6_7);

                        BetItem b = new BetItem();
                        b.webID = webID;
                        b.sportID = sportID;
                        b.type = BetType.BT_TEAM;
                        b.pID1 = team1_index;
                        b.pID2 = team2_index;
                        b.pName1 = team1_name;
                        b.pName2 = team2_name;
                        b.odds1 = odds1;
                        b.odds2 = odds2;
                        b.gameID = gameIndex;
                        b.gameName = gameNames[gameIndex];
                        b.leagueName1 = leagueName1;
                        b.leagueName2 = leagueName2;
                        b.handicap = betValue;
                        betItems.Add(b);
                    }
                    index++;
                }
                ShowLog(string.Format("页面解析成功，解析个数：{0}！",betItems.Count));
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
