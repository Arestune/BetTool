using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;
using System.IO.Compression;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Splash.Item;
using Splash.Tool;
namespace Splash.Parser
{
   
    class BaseParser
    {
        protected const int MAX_TRY_COUNT = 3;
        protected const int INVALID_INDEX = -1;
        protected const string INVALID_VALUE = "NULL";
        protected  string configFile = null;
        protected  string urlFormat = null;
        protected  string html = null;
        protected string responseCookie = null;

        public SportID sportID;
        public WebID webID;
        public List<BetItem> betItems = new List<BetItem>();

        public delegate void ShowLogDelegate(LogInfo message);
        public ShowLogDelegate showLogEvent;
        public  BaseParser()
        {
            Init();
            LoadStaticData();
        }
        protected virtual void Init()
        {
            configFile = System.IO.Directory.GetCurrentDirectory() + "\\Config\\" + StaticData.sportNames[(int)sportID] + "\\" + StaticData.webNames[(int)webID] + ".ini";
        }

        public virtual void LoadStaticData()
        {

        }
        public virtual int Parse()
        {
            return 0;
        }
        protected virtual string GetLeague1Name(string str)
        {
            return null;
        }
        protected virtual string GetLeague2Name(string str)
        {
            return null;
        }
        protected virtual int GetBO(string str)
        {
            return INVALID_INDEX;
        }
        protected virtual string GetGameID(string str)
        {
            return null;
        }
        protected virtual int GetGameIndex(string gameId)
        {
            return INVALID_INDEX;
        }
        protected virtual string GetGameName(int index)
        {
            return null;
        }
        protected virtual int GetTeamIndex(int gameIndex, string strTeam)
        {
             return INVALID_INDEX;
        }
        protected virtual DateTime GetGameTime(string strTime)
        {
            DateTime time = Convert.ToDateTime("1970-1-1");
            return time;
        }
        protected virtual IWebProxy GetProxy()
        {
            return null;
        }
        protected virtual SecurityProtocolType GetSecurityProtocal()
        {
            return SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }
        public virtual void GrabAndParseHtml()
        {
            try
            {
                int nTryCount = 0;
                string uri = Config.GetString(StaticData.SN_URL, "Uri", configFile, "Uri");
                RequestOptions op = new RequestOptions(uri);
                op.Method = Config.GetString(StaticData.SN_URL, "Method", configFile, "GET");
                op.Accept = Config.GetString(StaticData.SN_URL, "Accept", configFile, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                op.Referer = Config.GetString(StaticData.SN_URL, "Referer", configFile, "");
                op.RequestCookies = Config.GetString(StaticData.SN_URL, "Cookie", configFile, "");
                op.XHRParams = Config.GetString(StaticData.SN_URL, "XHRParams", configFile, "");
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
            }
            catch(Exception e)
            {
                LogInfo error = new LogInfo();
                error.webID = webID;
                error.level = ErrorLevel.EL_ERROR;
                error.message = e.Message;
                ShowLog(error);
            }
        }

        protected string RequestAction(RequestOptions options)
        {
            string result = string.Empty;
            HttpWebResponse response = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(options.Uri);
                IWebProxy proxy = GetProxy();
                if (proxy != null)
                {
                    request.Proxy = proxy;//设置代理服务器IP，伪装请求地址
                }
                request.Accept = options.Accept;
                ServicePointManager.SecurityProtocol = GetSecurityProtocal();
                //在使用curl做POST的时候, 当要POST的数据大于1024字节的时候, curl并不会直接就发起POST请求, 而是会分为俩步,
                //发送一个请求, 包含一个Expect: 100 -continue, 询问Server使用愿意接受数据
                //接收到Server返回的100 - continue应答以后, 才把数据POST给Server
                //并不是所有的Server都会正确应答100 -continue, 比如lighttpd, 就会返回417 “Expectation Failed”, 则会造成逻辑出错.
                request.Headers.Add("x-requested-with", "XMLHttpRequest");
                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.UseNagleAlgorithm = false;//禁止Nagle算法加快载入速度
                request.ServicePoint.ConnectionLimit = options.ConnectionLimit;//定义最大连接数
                if (!string.IsNullOrEmpty(options.XHRParams))
                {
                    request.AllowWriteStreamBuffering = true;
                }
                else
                {
                    request.AllowWriteStreamBuffering = false;
                }//禁止缓冲加快载入速度
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate,br");//定义gzip压缩页面支持
                request.ContentType = options.ContentType;//定义文档类型及编码
                request.AllowAutoRedirect = options.AllowAutoRedirect;//禁止自动跳转
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";//设置User-Agent，伪装成Google Chrome浏览器
                request.Timeout = options.Timeout;//定义请求超时时间为5秒
                request.KeepAlive = options.KeepAlive;//启用长连接
                if (!string.IsNullOrEmpty(options.Referer))
                {
                    request.Referer = options.Referer;//返回上一级历史链接
                }
                request.Method = options.Method;//定义请求方式为GET
                if (!string.IsNullOrEmpty(options.RequestCookies))
                {
                    request.Headers[HttpRequestHeader.Cookie] = options.RequestCookies;
                }
                if (options.WebHeader != null && options.WebHeader.Count > 0)
                {
                    request.Headers.Add(options.WebHeader);//添加头部信息
                }
                if (request.Method == "POST")//如果是POST请求，加入POST数据
                {
                    if(!string.IsNullOrEmpty(options.XHRParams))
                    {
                        byte[] buffer = Encoding.UTF8.GetBytes(options.XHRParams);
                        if (buffer != null)
                        {
                            request.ContentLength = buffer.Length;
                            request.GetRequestStream().Write(buffer, 0, buffer.Length);
                        }
                    }
                    else
                    {
                        request.ContentLength = 0;
                    }
                }
                
                using (response = (HttpWebResponse)request.GetResponse())
                {
                    responseCookie = response.Headers.Get("Set-Cookie");
                    ////获取请求响应
                    //foreach (Cookie cookie in response.Cookies)
                    //    options.CookiesContainer.Add(cookie);//将Cookie加入容器，保存登录状态
                    if (response.ContentEncoding.ToLower().Contains("gzip"))//解压
                    {
                        using (GZipStream stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                result = reader.ReadToEnd();
                            }
                        }
                    }
                    else if (response.ContentEncoding.ToLower().Contains("deflate"))//解压
                    {
                        using (DeflateStream stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress))
                        {
                            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                result = reader.ReadToEnd();
                            }
                        }
                    }
                    else
                    {
                        using (Stream stream = response.GetResponseStream())//原始
                        {
                            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                            {
                                result = reader.ReadToEnd();
                            }
                        }
                    }
                }
                request.Abort();
            }
            catch (WebException exp)
            {
                LogInfo error = new LogInfo();  
                error.webID = webID;
                error.level = ErrorLevel.EL_WARNING;
                error.message = exp.Message;
                ShowLog(error);
            }
            catch(Exception exp)
            {
                LogInfo error = new LogInfo();
                error.webID = webID;
                error.level = ErrorLevel.EL_ERROR;
                error.message = exp.Message;
                ShowLog(error);
            }
            return result;
        }

        public static List<BetWinPair> ParseBetWin(List<BetItem> bs)
        {
            List<BetWinPair> listPair = new List<BetWinPair>();
            if (bs.Count < 2) return listPair;
            for (int i = 0; i < bs.Count;i++ )
            {
                BetItem b1 = bs[i];
                for(int j = i+1;j< bs.Count;j++)
                {
                    BetItem b2 = bs[j];

                    //过滤同一个网站
                    if (b1.webID == b2.webID)
                    {
                        continue;
                    }
                    //过滤不同的体育项目
                    if (b1.sportID != b2.sportID)
                    {
                        continue;
                    }
                    //过滤不同的类型
                    if (b1.type != b2.type)
                    {
                        continue;
                    }
                    //过滤没有注册的teamID
                    if (b1.pID1 == INVALID_INDEX || b1.pID2 == INVALID_INDEX || b2.pID1 == INVALID_INDEX || b2.pID2 == INVALID_INDEX)
                    {
                        continue;
                    }
                    //过滤bo不同的
                    if(b1.bo != b2.bo)
                    {
                        continue;
                    }

                    if (b1.type == BetType.BT_TEAM)
                    {
                        double odd1 = 0.0f;
                        double odd2 = 0.0f;
                        double odd3 = 0.0f;
                        double odd4 = 0.0f;
                        if (b1.gameID == b2.gameID)
                        {
                            if ((b1.pID1 == b2.pID1) && (b1.pID2 == b2.pID2))
                            {
                                odd1 = b1.odds1;
                                odd2 = b2.odds2;
                                odd3 = b1.odds2;
                                odd4 = b2.odds1;
                                //B1让B2分
                                if (b1.handicap >= b2.handicap)
                                {
                                    if (CanMustWin(odd1, odd2))
                                    {
                                        BetWinPair pair = new BetWinPair();
                                        pair.b1 = b1;
                                        pair.b2 = b2;
                                        pair.orderForward = true;
                                        pair.odds1 = odd1;
                                        pair.odds2 = odd2;
                                        pair.pName1 = b1.pName1;
                                        pair.pName2 = b2.pName2;
                                        pair.pAbbr1 = b1.pAbbr1;
                                        pair.pAbbr2 = b2.pAbbr2;
                                        pair.handicap1 = b1.handicap;
                                        pair.handicap2 = -b2.handicap;
                                        listPair.Add(pair);
                                    }
                                }
                                if (b1.handicap <= b2.handicap)
                                {
                                    if (CanMustWin(odd3, odd4))
                                    {
                                        BetWinPair pair = new BetWinPair();
                                        pair.b1 = b1;
                                        pair.b2 = b2;
                                        pair.orderForward = false;
                                        pair.odds1 = odd3;
                                        pair.odds2 = odd4;
                                        pair.pName1 = b1.pName2;
                                        pair.pName2 = b2.pName1;
                                        pair.pAbbr1 = b1.pAbbr2;
                                        pair.pAbbr2 = b2.pAbbr1;
                                        pair.handicap1 = -b1.handicap;
                                        pair.handicap2 = b2.handicap;
                                        listPair.Add(pair);
                                    }
                                }
                            }
                            else if ((b1.pID1 == b2.pID2) && (b1.pID2 == b2.pID1))
                            {
                                odd1 = b1.odds1;
                                odd2 = b2.odds1;
                                odd3 = b1.odds2;
                                odd4 = b2.odds2;
                                double tmpHandicap = -b2.handicap;
                                if (b1.handicap >= tmpHandicap)
                                {
                                    if (CanMustWin(odd1, odd2))
                                    {
                                        BetWinPair pair = new BetWinPair();
                                        pair.b1 = b1;
                                        pair.b2 = b2;
                                        pair.orderForward = true;
                                        pair.odds1 = odd1;
                                        pair.odds2 = odd2;
                                        pair.pName1 = b1.pName1;
                                        pair.pName2 = b2.pName1;
                                        pair.pAbbr1 = b1.pAbbr1;
                                        pair.pAbbr2 = b2.pAbbr1;
                                        pair.handicap1 = b1.handicap;
                                        pair.handicap2 = b2.handicap;
                                        listPair.Add(pair);
                                    }
                                }
                                if (b1.handicap <= tmpHandicap)
                                {
                                    if (CanMustWin(odd3, odd4))
                                    {
                                        BetWinPair pair = new BetWinPair();
                                        pair.b1 = b1;
                                        pair.b2 = b2;
                                        pair.orderForward = false;
                                        pair.odds1 = odd3;
                                        pair.odds2 = odd4;
                                        pair.pName1 = b1.pName2;
                                        pair.pName2 = b2.pName2;
                                        pair.pAbbr1 = b1.pAbbr2;
                                        pair.pAbbr2 = b2.pAbbr2;
                                        pair.handicap1 = -b1.handicap;
                                        pair.handicap2 = -b2.handicap;
                                        listPair.Add(pair);
                                    }
                                }
                            }
                        }
                    }
                    else if (b1.type == BetType.BT_SOLO)
                    {
                        if (b1.itemID == b2.itemID && b1.pID1 == b2.pID1)
                        {
                            if ((b1.compare == BetCompare.Larger && b2.compare == BetCompare.Smaller && b1.handicap <= b2.handicap) ||
                                (b1.compare == BetCompare.Smaller && b2.compare == BetCompare.Larger && b1.handicap >= b2.handicap))
                            {
                                if (CanMustWin(b1.odds1, b2.odds1))
                                {
                                    BetWinPair pair = new BetWinPair();
                                    pair.b1 = b1;
                                    pair.b2 = b2;
                                    listPair.Add(pair);
                                }
                            }
                        }
                    }
                }
            }
            return listPair;
        }
        public static bool CanMustWin(double odds1, double odds2)
        {
            if (odds1 == 0 || odds2 == 0)
            {
                return false;
            }
            return (odds1 * odds2) / (odds1 + odds2) > 1;
        }
      
 
        public virtual void ShowLog(LogInfo log)
        {
            if (showLogEvent != null)
            {
                showLogEvent(log);
            }
        }
        public virtual void ShowLog(string log,ErrorLevel level = ErrorLevel.EL_NORMAL)
        {
            if (showLogEvent != null)
            {
                LogInfo error = new LogInfo();
                error.webID = webID;
                error.level = level;
                error.message = log;
                ShowLog(error);
            }
        }
    }
}
