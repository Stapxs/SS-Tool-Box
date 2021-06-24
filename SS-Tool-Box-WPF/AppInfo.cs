using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_Tool_Box
{
    public class AppInfo
    {
        public static string verStr = "Dev-0.4.1";      // 版本号
        public static int verBulidTimes = 16;           // 编译编号
        public static double verNum = 41.116;           // 版本号数字
#if DEBUG
        public static string verType = "Debug";         // 版本类型
#else
        public static string verType = "Dev";
#endif
    }
}
