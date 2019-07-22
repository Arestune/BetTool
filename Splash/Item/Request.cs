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

namespace Splash.Item
{
    public class RequestOptions
    {
        public RequestOptions(string url,bool isPost = false)
        {
            Uri = new Uri(url);
            if(isPost)
            {
                Method = "POST";
            }
            else
            {
                Method = "GET";
            }
            KeepAlive = false;
            ContentType = "application/json";
            WebHeader.Add("Accept-Language", "zh-CN,zh;q=0.9");
            WebHeader.Add("Accept-Encoding", "gzip, deflate, br");
        }
        /// <summary>
        /// 请求方式，GET或POST
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public Uri Uri { get; set; }
        /// <summary>
        /// 上一级历史记录链接
        /// </summary>
        public string Referer { get; set; }
        /// <summary>
        /// 超时时间（毫秒）
        /// </summary>
        public int Timeout = 3000;
        /// <summary>
        /// 启用长连接
        /// </summary>
        public bool KeepAlive = true;
        /// <summary>
        /// 禁止自动跳转
        /// </summary>
        public bool AllowAutoRedirect = false;
        /// <summary>
        /// 定义最大连接数
        /// </summary>
        public int ConnectionLimit = int.MaxValue;
        /// <summary>
        /// 请求次数
        /// </summary>
        public int RequestNum = 3;
        /// <summary>
        /// 可通过文件上传提交的文件类型
        /// </summary>
        public string Accept = "application/json, text/plain, */*";
        /// <summary>
        /// 内容类型
        /// </summary>
        public string ContentType = "text/html; charset=UTF-8 ; text/json";//"application/x-www-form-urlencoded";
        /// <summary>
        /// 实例化头部信息
        /// </summary>
        private WebHeaderCollection header = new WebHeaderCollection();
        /// <summary>
        /// 头部信息
        /// </summary>
        public WebHeaderCollection WebHeader
        {
            get { return header; }
            set { header = value; }
        }
        /// <summary>
        /// 定义请求Cookie字符串
        /// </summary>
        public string RequestCookies { get; set; }
        /// <summary>
        /// 异步参数数据
        /// </summary>
        public string XHRParams { get; set; }

    }

}
