using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Item
{
    public enum ErrorLevel
    {
        EL_NORMAL,
        EL_WARNING,
        EL_ERROR
    }
    public class LogInfo
    {
        public WebID webID
        {
            get;
            set;
        }
        public ErrorLevel level
        {
            get;
            set;
        }
        public string message
        {
            get;
            set;
        }
    }
}
