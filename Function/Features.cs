using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace SS_Tool_Box
{
    /// <summary>
    /// 模块化功能
    /// </summary>
    static class Features
    {
        #region Reg | 注册表操作
        /// <summary>
        /// 注册表操作
        /// </summary>
        public class Reg
        {
            /// <summary>
            /// 判断注册表项是否存在
            /// </summary>
            /// <param name="way">注册表项父级路径</param>
            /// <param name="name">注册表项名</param>
            /// <returns>是否存在</returns>
            public static bool IsRegeditItemExist(RegistryKey hkml, string way, string name)
            {
                string[] subkeyNames;
                RegistryKey software = hkml.OpenSubKey(way, true);
                subkeyNames = software.GetSubKeyNames();
                //取得该项下所有子项的名称的序列，并传递给预定的数组中
                foreach (string keyName in subkeyNames) //遍历整个数组
                {
                    if (keyName.Equals(name)) //判断子项的名称
                    {
                        hkml.Close();
                        return true;
                    }
                }
                hkml.Close();
                return false;
            }

            /// <summary>
            /// 创建注册表项
            /// </summary>
            /// <param name="way">注册表项路径</param>
            /// <returns>是否成功</returns>
            public static bool CreateRegItem(RegistryKey key, String way)
            {
                try
                {
                    RegistryKey software = key.CreateSubKey(way);
                }
                catch
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 判断注册表项的键值是否存在
            /// </summary>
            /// <param name="way">注册表项路径</param>
            /// <param name="name">键名</param>
            /// <returns>是否存在</returns>
            public static bool IsRegeditKeyExit(RegistryKey hkml, String way, String name)
            {
                string[] subkeyNames;
                RegistryKey software = hkml.OpenSubKey(way, true);
                subkeyNames = software.GetValueNames();
                //取得该项下所有键值的名称的序列，并传递给预定的数组中
                foreach (string keyName in subkeyNames)
                {
                    if (keyName.Equals(name)) //判断键值的名称
                    {
                        hkml.Close();
                        return true;
                    }
                }
                hkml.Close();
                return false;
            }


            /// <summary>
            /// 创建键
            /// </summary>
            /// <param name="way">键的父路径</param>
            /// <param name="name">键名</param>
            /// <param name="value">键值</param>
            /// /// <returns>是否成功</returns>
            public static bool CreateRegKey(RegistryKey key, String way, String name, String value)
            {
                try
                {
                    RegistryKey software = key.OpenSubKey(way, true); //该项必须已存在
                    software.SetValue(name, value);
                }
                catch
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 获取键值
            /// </summary>
            /// <param name="way">键的父路径</param>
            /// <param name="name">键名</param>
            /// <returns></returns>
            public static String GetRegKey(RegistryKey key, String way, String name)
            {
                try
                {
                    RegistryKey software = key.OpenSubKey(way, true); //该项必须已存在
                    return software.GetValue(name).ToString();
                }
                catch
                {
                    return "ERR";
                }
            }
        }
        #endregion

        #region Time | 时间戳操作

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

        #endregion

        #region UI | 控件操作

        /// <summary>
        /// 根据名字获取子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        #endregion

    }
}
