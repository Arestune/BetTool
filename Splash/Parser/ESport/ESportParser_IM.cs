﻿using System;
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
    class ESportParser_IM:ESportParser
    {
        protected override void Init()
        {
            webID = WebID.WID_IM;
            base.Init();
        }
        protected override int GetTeamIndex(int gameIndex, string strTeam)
        {
            strTeam = Util.GetFilterString(strTeam);
            string strLowerTeam = strTeam.ToLower();
            if (teamIds[gameIndex].ContainsKey(strLowerTeam))
            {
                return teamIds[gameIndex][strLowerTeam];
            }
            else
            {
                int curId = teamIds[gameIndex].Count;
                teamIds[gameIndex].Add(strLowerTeam, curId);
                Config.WriteString(gameIndex.ToString(), string.Format("T{0}-{1}", gameIndex, curId), string.Format("{0},{1}",
                    strTeam, curId), configFile);
                ShowLog(string.Format("新增主列表[{0}]队伍:{1}", gameIndex, strTeam), ErrorLevel.EL_NORMAL);
                return curId;
            }
        }

        protected override int GetBO(string str)
        {
            Match match = Regex.Match(str, @"BO(\d+)", RegexOptions.IgnoreCase);
            if (match != null && match.Groups[1] != null)
            {
                int bo = 0;
                if (int.TryParse(match.Groups[1].ToString(), out bo))
                {
                    return bo;
                }
            }
            return 1;
        }
        public override int Parse()
        {
            if (string.IsNullOrEmpty(html))
            {
                return 0;
            }
            JObject mObj = JObject.Parse(html);
            JToken d = mObj["d"];
            foreach (var b1 in d)
            {
               
                    var gameIndex = GetGameIndex(b1[8].ToString());
                    if(gameIndex == INVALID_INDEX)
                    {
                        continue;
                    }
                    var leagueName1 = b1[5][1].ToString();
                    var leagueName2 = b1[1][1].ToString();
                    //IniUtil.WriteString("Game", string.Format("G{0}", index),gameName, configFile);
                    foreach (var b2 in b1[10])
                    {
                        try
                        {
                            var team1_name = b2[5][0].ToString().Trim();
                            var team2_name = b2[6][0].ToString().Trim();
                            var europeTime = DateTime.Parse(b2[7].ToString());
                            var chinaMatchTime = europeTime.AddHours(8);
                            var localTime = DateTime.Now;
                            //比赛已经开始
                            if (DateTime.Compare(localTime, chinaMatchTime) > 0)
                            {
                                continue;
                            }
                            //赔率不正常，封盘的比赛
                            int x = Convert.ToInt32(b2[35]);
                            if (x == 1)
                            {
                                continue;
                            }
                            //bo
                            int bo = GetBO(b2[33].ToString());
                            double betValue = 0.0;
                            if (bo % 2 == 0)
                            {
                                var winhandcap = b2[28].ToString();
                                int v = Convert.ToInt32(winhandcap.Split('-')[1]);
                                if (winhandcap.Contains("A"))
                                {
                                    betValue = -v / 10.0f;
                                }
                                else if (winhandcap.Contains("B"))
                                {
                                    betValue = v / 10.0f;
                                }
                            }
                            var team1_abbr = b2[37].ToString();
                            var team2_abbr = b2[38].ToString();
                            //if(Convert.ToInt32(b2[50].ToString()) == 0 && Convert.ToInt32(b2[52].ToString())  == 0)
                            //{
                            //    continue;
                            //}
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
                            b.pAbbr1 = team1_abbr;
                            b.pAbbr2 = team2_abbr;
                            b.odds1 = odds1;
                            b.odds2 = odds2;
                            b.gameID = gameIndex;
                            b.gameName = gameStaticNames[gameIndex];
                            b.leagueName1 = leagueName1;
                            b.leagueName2 = leagueName2;
                            b.handicap = betValue;
                            b.time = chinaMatchTime;
                            b.bo = bo;
                            betItems.Add(b);

                            if (betItems.Count == 81)
                            {
                                int a = 1;
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
            }
            ShowLog(string.Format("页面解析成功，解析个数：{0}！", betItems.Count));
            return betItems.Count;
        }
    }
     
}
