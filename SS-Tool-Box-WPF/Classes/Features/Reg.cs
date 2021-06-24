using Microsoft.Win32;
using System;

namespace SS_Tool_Box
{
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
        public bool IsRegeditItemExist(RegistryKey hkml, string way, string name)
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
        public bool CreateRegItem(RegistryKey key, String way)
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
        public bool IsRegeditKeyExit(RegistryKey hkml, String way, String name)
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
        public bool CreateRegKey(RegistryKey key, String way, String name, String value)
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

        public bool DelRegKey(RegistryKey key, String way, String name)
        {
            try
            {
                RegistryKey software = key.OpenSubKey(way, true);
                software.DeleteValue(name);
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
        public string GetRegKey(RegistryKey key, String way, String name)
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
}
