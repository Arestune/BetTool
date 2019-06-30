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
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Splash.Item;
using Splash.Tool;


namespace Splash.Parser.Basketball
{
    class BasketballParser_Yabo:BasketballParser
    {
        protected string xauth;
        protected override void Init()
        {
            webID = WebID.WID_YABO;
            base.Init();
        }
        protected override System.Net.IWebProxy GetProxy()
        {
            WebProxy proxy = null;
            return proxy;
        }
        public override void GrabAndParseHtml()
        {
            int nTryCount = 0;
            string url = "www.baidu.com";
            RequestOptions op = new RequestOptions(url, true);
            op.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
            op.Method = "GET";
            op.KeepAlive = true;
            op.Referer = "https://www.yabox10.com/app/sport";
            op.RequestCookies = Config.GetString("Cookie", "cookie", configFile);
            if(!string.IsNullOrEmpty(op.RequestCookies))
            {
                if(op.RequestCookies.Contains(".Xauth"))
                {
                    Match match = Regex.Match(op.RequestCookies, @"\.Xauth=(\w+)");
                    xauth = match.Groups[1].ToString().Trim();
                }
                else
                {
                    MessageBox.Show("授权码不存在，请配置授权码！");
                    return ;
                }
            }
            else
            {
                MessageBox.Show("授权码不存在，请配置授权码！");
                return;
            }
            

            //更新授权码
            html = RequestAction(op);
            if (!string.IsNullOrEmpty(responseCookie))
            {
                if (responseCookie.Contains("Xauth"))
                {
                    Config.WriteString("Cookie", "cookie", responseCookie, configFile);
                    Match m = Regex.Match(responseCookie, @"\.Xauth=(\w+)");
                    xauth = m.Groups[1].ToString().Trim();
                }
            }

            RequestOptions roFinal = new RequestOptions("https://xj-sb-asia-yabo.prdasbbwla1.com/zh-cn/serv/getamodds",true);
            roFinal.KeepAlive = true;
            roFinal.ContentType = "application/json;charset=utf-8";
            roFinal.Accept = "application/json, text/javascript, */*; q=0.01";
            roFinal.WebHeader.Add("Accept-Language", "zh-CN,zh;q=0.9");
            roFinal.Referer = "https://xj-sb-asia-yabo.prdasbbwla1.com/zh-cn/sports";
            roFinal.RequestCookies = ".Xauth=" + xauth;
            roFinal.XHRParams = "{\"eid\":2986041,\"ifl\":true,\"iip\":false,\"isp\":false,\"ot\":1,\"ubt\":\"am\",\"tz\":480,\"v\":0}";
            html = null;
            while (string.IsNullOrEmpty(html) && nTryCount < MAX_TRY_COUNT)
            {
                html = RequestAction(roFinal);
                nTryCount++;
                Thread.Sleep(1);
            }
            if (nTryCount == MAX_TRY_COUNT)
            {
                MessageBox.Show("抓取失败！");
                html = "";
            }
            else
            {
                Parse();
            }
        }
        public override void LoadStaticData()
        {
            
            //EffectiveItem
            int index = 0;
            var eItem = Config.GetString("EffectiveItem", string.Format("I{0}", index), configFile);
            while (!string.IsNullOrEmpty(eItem))
            {
                index++;
                effectItems.Add(eItem);
                eItem = Config.GetString("EffectiveItem", string.Format("I{0}", index), configFile);
            }

            //Player
            index = 0;
            var player = Config.GetString("Players", string.Format("P{0}", index), configFile);
            while (!string.IsNullOrEmpty(player))
            {
                index++;
                playerNames.Add(player);
                player = Config.GetString("Players", string.Format("P{0}", index), configFile);
            }

            //Equal
            var largerPair = Config.GetString("Compare", "C0", configFile).Split('|');
            var smallerPair = Config.GetString("Compare", "C1", configFile).Split('|');
            foreach (var larger in largerPair)
            {
                largers.Add(larger);
            }
            foreach (var smaller in smallerPair)
            {
                smallers.Add(smaller);
            }
        }
        protected int GetBetItemIndex(string parseString)
        {
            Match match = Regex.Match(parseString, @"(.*):(.*)");
            string strValue = match.Groups[2].ToString().Trim();
            if (effectItems.Contains(strValue))
            {
                return effectItems.IndexOf(strValue);
            }
            return -1 ;
        }
        protected override int GetPlayerIndex(string parseString)
        {
            Match match = Regex.Match(parseString, @"(.*):(.*)");
            string playerName = match.Groups[1].ToString().Trim();
            if (playerNames.Contains(playerName))
            {
                return playerNames.IndexOf(playerName);
            }
            playerNames.Add(playerName);
            Config.WriteString("Players", string.Format("P{0}", playerNames.Count - 1), playerName, configFile);
            return playerNames.Count - 1;
        }
        protected override BetCompare GetBetCompare(string parseString)
        {
            foreach(var larger in largers)
            {
                if(parseString.Contains(larger))
                {
                    return BetCompare.Larger;
                }
            }

            foreach (var smaller in smallers)
            {
                if (parseString.Contains(smaller))
                {
                    return BetCompare.Smaller;
                }
            }
            return BetCompare.Smaller;
        }
        protected override double GetBetHandicap(string parseString)
        {
            Match match = Regex.Match(parseString, @"[大|小| ](.*)");
            string strValue = match.Groups[1].ToString().Trim();
            double value = Convert.ToDouble(strValue);
            return value;
        }
        protected  double GetBetOdds(string parseString)
        {
            double value = Convert.ToDouble(parseString);
            return value;
        }

        public override int Parse()
        {
            betItems.Clear();

            //string fullName = "Yabo-NBA.json";
            //System.IO.StreamReader sr1 = new StreamReader(fullName);
            //string nextLine = sr1.ReadToEnd();
            JObject mObj = JObject.Parse(html);
            JToken eg = mObj["eg"];
            JToken es = eg["es"];
            JToken game = es[0];
            JToken po = game["p-o"];
            int index = 0;

            foreach (JToken baseJ in po)//遍历数组
            {
                var n = baseJ["n"];
                Config.WriteString("Items", string.Format("I{0}", index), n.ToString(), configFile);
                var itemIndex = GetBetItemIndex(n.ToString());
                if(itemIndex < 0)
                {
                    continue;
                }
                var playerIndex = GetPlayerIndex(n.ToString());
                var itemName = effectItems[itemIndex];
                for (int i = 0; i < effectItems.Count;i++ )
                {
                    if(itemName.ToString() == effectItems[i])
                    {
                        JToken array = baseJ["o"];
                        foreach (JToken eI in array)//遍历数组
                        {
                            BetItem b = new BetItem();
                            b.webID = webID;
                            b.type = BetType.BT_SOLO;
                            b.compare = GetBetCompare(eI[0].ToString());
                            b.pID1 = playerIndex;
                            b.pName1 = playerNames[playerIndex];
                            b.handicap = GetBetHandicap(eI[0].ToString());
                            b.odds1 = GetBetOdds(eI[2].ToString());
                            b.itemID = itemIndex;
                            betItems.Add(b);
                        }
                    }
                }
                index++;
            }
            return betItems.Count;
        }
    }
}
