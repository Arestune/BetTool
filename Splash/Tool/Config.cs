using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace Splash.Tool
{
    /*
    * 姓名：叶重阳
    * 时间：2017.4.8
    * 邮箱：yechongyang@manmanbuy.com
    */

    /// <summary>
    /// ini文件操作类
    /// </summary>
    public sealed class Config
    {
        /*
        /// <summary>
        /// windows api 对ini文件写方法
        /// </summary>
        /// <param name="lpapplicationname">要在其中写入新字串的小节名称。这个字串不区分大小写</param>
        /// <param name="lpkeyname">要设置的项名或条目名。这个字串不区分大小写。用null可删除这个小节的所有设置项</param>
        /// <param name="lpstring">指定为这个项写入的字串值。用null表示删除这个项现有的字串</param>
        /// <param name="lpfilename">初始化文件的名字。假如没有指定完整路径名，则windows会在windows目录查找文件。假如文件没有找到，则函数会创建它</param>
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool WritePrivateProfileString(
            string lpAppName, string lpKeyName, string lpString, string lpFileName);
        /// <summary>
        /// windows api 对ini文件读方法
        /// </summary>
        /// <param name="lpapplicationname">欲在其中查找条目的小节名称。这个字串不区分大小写。如设为null，就在lpreturnedstring缓冲区内装载这个ini文件所有小节的列表</param>
        /// <param name="lpkeyname">欲获得的项名或条目名。这个字串不区分大小写。如设为null，就在lpreturnedstring缓冲区内装载指定小节所有项的列表</param>
        /// <param name="lpdefault">指定的条目没有找到时返回的默认值。可设为空（""）</param>
        /// <param name="lpreturnedstring">指定一个字串缓冲区，长度至少为nsize</param>
        /// <param name="nsize">指定装载到lpreturnedstring缓冲区的最大字符数量</param>
        /// <param name="lpfilename">初始化文件的名字。如没有指定一个完整路径名，windows就在windows目录中查找文件</param>
        /// 注重：如lpkeyname参数为null，那么lpreturnedstring缓冲区会载入指定小节所有设置项的一个列表。
        /// 每个项都用一个null字符分隔，最后一个项用两个null字符中止。也请参考getprivateprofileint函数的注解
        /// <returns></returns>
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetPrivateProfileString(
            string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString,
            int nSize, string lpFileName);
        /// <summary>
        /// 向ini文件中写入值
        /// </summary>
        /// <param name="section">小节的名称</param>
        /// <param name="key">键的名称</param>
        /// <param name="value">键的值</param>
        /// <returns>执行成功为true，失败为false。</returns>
        public static bool WriteString(string section, string key, string value, string filename)
        {
            if (string.IsNullOrEmpty(value)) return false;
            if (section.Trim().Length <= 0 || key.Trim().Length <= 0 ||
            value.Trim().Length <= 0) return false;
            return WritePrivateProfileString(section, key, value, filename);
        }
        /// <summary>
        /// 删除指定小节中的键
        /// </summary>
        /// <param name="section">小节的名称</param>
        /// <param name="key">键的名称</param>
        /// <returns>执行成功为true，失败为false。</returns>

        public static bool DeleteKey(string section, string key, string fileName)
        {
            if (section.Trim().Length <= 0 || key.Trim().Length <= 0) return false;
            return WritePrivateProfileString(section, key, null, fileName);
        }
        /// <summary>
        /// 删除指定的小节（包括这个小节中所有的键）
        /// </summary>
        /// <param name="section">小节的名称</param>
        /// <returns>执行成功为true，失败为false。</returns>
        public static bool DeleteSection(string section, string fileName)
        {
            if (section.Trim().Length <= 0) return false;
            return WritePrivateProfileString(section, null, null, fileName);
        }
        /// <summary>
        /// 获得指定小节中键的值
        /// </summary>
        /// <param name="section">小节的名称</param>
        /// <param name="key">键的名称</param>
        /// <param name="defaultvalue">假如键值为空，或没找到，返回指定的默认值。</param>
        /// <param name="capacity">缓冲区初始化大小。</param>
        /// <returns>键的值，string类型</returns>
        public static string GetString(string section, string key, string fileName, string defaultvalue = "", int capacity = 1000)
        {
            if (section.Trim().Length <= 0 || key.Trim().Length <= 0) return null;
            System.Text.StringBuilder strTemp = new System.Text.StringBuilder(capacity);
            long returnvalue = GetPrivateProfileString(section, key, defaultvalue, strTemp, capacity, fileName);
            return strTemp.ToString().Trim();
        }
        /// <summary>
        /// 获得指定小节中键的值
        /// </summary>
        /// <param name="section">小节的名称</param>
        /// <param name="key">键的名称</param>
        /// <param name="defaultvalue">假如键值为空，或没找到，返回指定的默认值。</param>
        /// <param name="capacity">缓冲区初始化大小。</param>
        /// <returns>键的值，int类型</returns>
        public static int GetInt(string section, string key, string fileName, int defaultvalue = 0)
        {
            string strTemp = GetString(section, key, fileName, defaultvalue.ToString());
            int nRes = 0;
            if (!int.TryParse(strTemp, out nRes))
            {
                return defaultvalue;
            }
            return nRes;
        }
        */



        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(byte[] section, byte[] key, byte[] val, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(byte[] section, byte[] key, byte[] def, byte[] retVal, int size, string filePath);
        //与ini交互必须统一编码格式
        private static byte[] getBytes(string s, string encodingName)
        {
            return null == s ? null : Encoding.GetEncoding(encodingName).GetBytes(s);
        }
        public static string GetString(string section, string key, string fileName, string def = "", string encodingName = "utf-8", int size = 1024)
        {
            byte[] buffer = new byte[size];
            int count = GetPrivateProfileString(getBytes(section, encodingName), getBytes(key, encodingName), getBytes(def, encodingName), buffer, size, fileName);
            return Encoding.GetEncoding(encodingName).GetString(buffer, 0, count).Trim();
        }
        public static bool WriteString(string section, string key, string value, string fileName, string encodingName = "utf-8")
        {
            return WritePrivateProfileString(getBytes(section, encodingName), getBytes(key, encodingName), getBytes(value, encodingName), fileName);
        }
    }

}
