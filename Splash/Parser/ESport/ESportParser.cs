using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splash.Parser;
using Splash.Item;
using Splash.Tool;
namespace Splash.Parser.ESport
{
    class ESportParser:BaseParser
    {
        //动态数据
        protected List<string> gameIds = new List<string>();
        protected Dictionary<int, Dictionary<string, int>> teamIds = new Dictionary<int, Dictionary<string, int>>();
        //主电竞数据--亚博
        protected static string staticConfigFile = null;
        protected static List<string> gameStaticNames = new List<string>();

        protected static Dictionary<int, Dictionary<string, int>> mainTeamIds = new Dictionary<int, Dictionary<string, int>>();
        protected override void Init()
        {
            sportID = SportID.SID_ESPORT;
            base.Init();
        }
        public static void LoadMainData()
        {
            try
            {
                staticConfigFile = System.IO.Directory.GetCurrentDirectory() + @"\Config\电竞\亚博.ini";
                //GameNames
                int index = 0;
                gameStaticNames.Clear();
                var gameName = Config.GetString(StaticData.SN_GAME_NAME, string.Format("G{0}", index), staticConfigFile);
                while (!string.IsNullOrEmpty(gameName))
                {
                    index++;
                    gameStaticNames.Add(gameName);
                    gameName = Config.GetString(StaticData.SN_GAME_NAME, string.Format("G{0}", index), staticConfigFile);
                }

                var fileNames = Util.GetFileNames(System.IO.Directory.GetCurrentDirectory() + "\\Config\\" + StaticData.sportNames[(int)SportID.SID_ESPORT], "*.ini");
                //Teams create empty
                mainTeamIds.Clear();
                for (int i = 0; i < gameStaticNames.Count; i++)
                {
                    Dictionary<string, int> teamList = new Dictionary<string, int>();
                    mainTeamIds.Add(i, teamList);
                }
                //匹配
                for (int fileIndex = 0; fileIndex < fileNames.Count; fileIndex++)
                {
                    for (int i = 0; i < gameStaticNames.Count; i++)
                    {
                        int teamIndex = 0;
                        string teamAndId = Config.GetString(i.ToString(), string.Format("T{0}-{1}",i, teamIndex), fileNames[fileIndex]);
                        while (!string.IsNullOrEmpty(teamAndId))
                        {
                            var array = teamAndId.Split(',');
                            var teamName = array[0].Trim().ToLower();
                            var id = Convert.ToInt32(array[1].Trim());
                            if (!mainTeamIds[i].ContainsKey(teamName) && id != INVALID_INDEX)
                            {
                                mainTeamIds[i].Add(teamName, id);  //这边都要小写
                            }
                            teamIndex++;
                            teamAndId = Config.GetString(i.ToString(), string.Format("T{0}-{1}", i, teamIndex), fileNames[fileIndex]);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
            }
        }
        public override void LoadStaticData()
        {
            try
            {
                base.LoadStaticData();

                //GameID
                int index = 0;
                var eItem = Config.GetString(StaticData.SN_GAME_ID, string.Format("G{0}", index), configFile);
                while (!string.IsNullOrEmpty(eItem))
                {
                    index++;
                    gameIds.Add(eItem.ToLower());
                    eItem = Config.GetString(StaticData.SN_GAME_ID, string.Format("G{0}", index), configFile);
                }

                //Teams create empty
                for (int i = 0; i < gameStaticNames.Count; i++)
                {
                    Dictionary<string, int> teamList = new Dictionary<string, int>();
                    teamIds.Add(i, teamList);
                }

                //Teams
                for (int i = 0; i < gameIds.Count; i++)
                {
                    int teamIndex = 0;
                    string teamAndId = Config.GetString(i.ToString(), string.Format("T{0}-{1}", i, teamIndex), configFile);
                    while (!string.IsNullOrEmpty(teamAndId))
                    {
                        var array = teamAndId.Split(',');
                        //
                        if (array.Length != 2)
                        {
                            bIsEffect = false;
                            return;
                            //teamIds[i].Add("Tmp_"+teamIndex.ToString(),-1);
                            //teamIndex++;
                            //teamAndId = Config.GetString(i.ToString(), string.Format("T{0}-{1}", i, teamIndex), configFile);
                            //continue;
                        }
                        var teamName = array[0].Trim();
                        var teamLowerName = teamName.ToLower();
                        var id = Convert.ToInt32(array[1].Trim());

                        if (!teamIds[i].ContainsKey(teamLowerName))
                        {
                            teamIds[i].Add(teamLowerName, id);
                        }
                        else
                        {
                            teamIds[i].Add("Tmp_" + teamIndex.ToString(), -1);
                        }
                        //如果是-1，扫描主列表
                        if (id == INVALID_INDEX)
                        {
                            if (i < mainTeamIds.Count && mainTeamIds[i].ContainsKey(teamLowerName))
                            {
                                var value = teamName + "," + mainTeamIds[i][teamLowerName].ToString();
                                Config.WriteString(i.ToString(), string.Format("T{0}-{1}",i, teamIndex), value, configFile);
                                ShowLog(string.Format("识别[{0}]队伍:{1},ID:{2}！", i, teamName, mainTeamIds[i][teamLowerName]), ErrorLevel.EL_NORMAL);
                            }
                        }
                        teamIndex++;
                        teamAndId = Config.GetString(i.ToString(), string.Format("T{0}-{1}",i, teamIndex), configFile);
                    }
                }
            }
            catch (Exception exp)
            {
                DebugLog error = new DebugLog();
                error.webID = webID;
                error.level = ErrorLevel.EL_ERROR;
                error.message = exp.Message;
                ShowLog(error);
            }
        }
        protected virtual int GetBO(string str)
        {
            return 1;
        }
        protected override string GetGameID(string str)
        {
            return null;
        }
        protected override string GetGameName(int index)
        {
            if(index < gameStaticNames.Count )
            {
                return gameStaticNames[index];
            }
            return null;
        }
        protected override int GetGameIndex(string gameId)
        {
            gameId = gameId.ToLower();
            if (gameIds.Contains(gameId.ToLower()))
            {
                return gameIds.IndexOf(gameId);
            }
            //gameIds.Add(gameId);
            return INVALID_INDEX;
        }
        protected override int GetTeamIndex(int gameIndex, string strTeam)
        {
            if (gameIndex >= teamIds.Count) return INVALID_INDEX;
            string strLowerTeam = strTeam.ToLower().Trim();

            if (teamIds[gameIndex].ContainsKey(strLowerTeam))
            {
                return teamIds[gameIndex][strLowerTeam];
            }
            else
            {
                int curId = GetMainTeamIndex(gameIndex, strLowerTeam);
                teamIds[gameIndex].Add(strLowerTeam, curId);
                Config.WriteString(gameIndex.ToString(), string.Format("T{0}-{1}", gameIndex, teamIds[gameIndex].Count - 1), string.Format("{0},{1}",
                    strTeam, curId), configFile);
                if (curId == INVALID_INDEX)
                {
                    ShowLog(string.Format("新增[{0}]队伍:{1},请在配置文件配置ID！", gameIndex, strTeam), ErrorLevel.EL_NORMAL);
                }
                else
                {
                    ShowLog(string.Format("匹配[{0}]队伍:{1},ID:{2}！", gameIndex, strTeam, curId), ErrorLevel.EL_NORMAL);
                }
                return curId;
            }
          
        }
        protected virtual int GetMainTeamIndex(int gameIndex, string strTeam)
        {
            strTeam = strTeam.ToLower().Trim();
            if (mainTeamIds[gameIndex].ContainsKey(strTeam))
            {
                return mainTeamIds[gameIndex][strTeam];
            }
            return INVALID_INDEX;
        }
        protected virtual BetCompare GetBetCompare(string parseString)
        {
            return BetCompare.Larger;
        }
        protected virtual double GetBetValue(string parseString)
        {
            return 0.0;
        }
        protected virtual int GetPlayerIndex(string parseString)
        {
            return -1;
        }
    }
}
