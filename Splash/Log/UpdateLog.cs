using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Splash.Log
{
    public enum LogType
    {
        LT_NEW,
        LT_MODIFY,
        LT_OPTIMIZE,
        LT_DELETE
    }
    public class UpdateLog
    {
        public LogType type;
        public List<string> logs = new List<string>();
        public UpdateLog(LogType type, List<string> logs)
        {
            this.type = type;
            this.logs = logs;
        }
    }
}
