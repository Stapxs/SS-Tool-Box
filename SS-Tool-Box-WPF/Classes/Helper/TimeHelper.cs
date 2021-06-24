using System;

namespace SS_Tool_Box
{
    static partial class Features
    {
        /// <summary>
        /// 时间相关
        /// Add by 成长的小猪（Jason.Song） on 2018/05/10
        /// http://blog.csdn.net/jasonsong2008
        /// </summary>
        public static class TimeHelper
        {
            /// <summary>
            /// 获取当前时间戳
            /// Add by 成长的小猪（Jason.Song） on 2019/05/07
            /// http://blog.csdn.net/jasonsong2008
            /// </summary>
            /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
            /// <returns></returns>
            public static string GetCurrentTimestamp(bool millisecond = true)
            {
                return ToTimestamp(DateTime.Now, millisecond);
            }

            /// <summary>
            /// 转换指定时间得到对应的时间戳
            /// Add by 成长的小猪（Jason.Song） on 2019/05/07
            /// http://blog.csdn.net/jasonsong2008
            /// </summary>
            /// <param name="dateTime"></param>
            /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
            /// <returns>返回对应的时间戳</returns>
            public static string ToTimestamp(DateTime dateTime, bool millisecond = true)
            {
                return ToTimestampLong(dateTime, millisecond).ToString();
            }

            /// <summary>
            /// 转换指定时间得到对应的时间戳
            /// Add by 成长的小猪（Jason.Song） on 2019/05/07
            /// http://blog.csdn.net/jasonsong2008
            /// </summary>
            /// <param name="dateTime"></param>
            /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
            /// <returns>返回对应的时间戳</returns>
            public static long ToTimestampLong(DateTime dateTime, bool millisecond = true)
            {
                var ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return millisecond ? Convert.ToInt64(ts.TotalMilliseconds) : Convert.ToInt64(ts.TotalSeconds);
            }

            /// <summary>
            /// 转换指定时间戳到对应的时间
            /// Add by 成长的小猪（Jason.Song） on 2019/05/07
            /// http://blog.csdn.net/jasonsong2008
            /// </summary>
            /// <param name="timestamp">（10位或13位）时间戳</param>
            /// <returns>返回对应的时间</returns>
            public static DateTime ToDateTime(string timestamp)
            {
                var tz = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
                return timestamp.Length == 13
                    ? tz.AddMilliseconds(Convert.ToInt64(timestamp))
                    : tz.AddSeconds(Convert.ToInt64(timestamp));
            }
        }
    }
}
