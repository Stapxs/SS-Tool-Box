using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS_Tool_Box.Classes
{
    public class SSUserClass
    {
        #region Reg类 | 注册表操作
        /// <summary>
        /// 注册表类提供的所有操作均在 CurrentUser 主键下
        /// </summary>
        public class Reg
        {
            /// <summary>
            /// 在用户目录下判断注册表项是否存在
            /// </summary>
            /// <param name="way">注册表项父级路径</param>
            /// <param name="name">注册表项名</param>
            /// <returns>是否存在</returns>
            public static bool IsRegeditItemExist(string way, string name)
            {
                string[] subkeyNames;
                RegistryKey hkml = Registry.CurrentUser;
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
            /// 在用户目录下创建注册表项
            /// </summary>
            /// <param name="way">注册表项路径</param>
            /// <returns>是否成功</returns>
            public static bool CreateRegItem(String way)
            {
                try
                {
                    RegistryKey key = Registry.CurrentUser;
                    RegistryKey software = key.CreateSubKey(way);
                }
                catch
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 在用户目录下判断注册表项的键值是否存在
            /// </summary>
            /// <param name="way">注册表项路径</param>
            /// <param name="name">键名</param>
            /// <returns>是否存在</returns>
            public static bool IsRegeditKeyExit(String way, String name)
            {
                string[] subkeyNames;
                RegistryKey hkml = Registry.CurrentUser;
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
            /// 在用户目录下创建键
            /// </summary>
            /// <param name="way">键的父路径</param>
            /// <param name="name">键名</param>
            /// <param name="value">键值</param>
            /// /// <returns>是否成功</returns>
            public static bool CreateRegKey(String way, String name, String value)
            {
                try
                {
                    RegistryKey key = Registry.CurrentUser;
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
            /// 获取在用户目录下的键值
            /// </summary>
            /// <param name="way">键的父路径</param>
            /// <param name="name">键名</param>
            /// <returns></returns>
            public static String GetRegKey(String way,String name)
            {
                try
                {
                    RegistryKey key = Registry.CurrentUser;
                    RegistryKey software = key.OpenSubKey(way, true); //该项必须已存在
                    return software.GetValue(name).ToString();
                }
                catch
                {
                    return "Err";
                }
            }
        }
        #endregion
    }
}
