using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SS_Tool_Box
{
    /// <summary>
    /// 这是SS的用户工具类——
    /// </summary>
    public class SSUserClass
    {
        public static string SSUCVersion = "1.0.3";

        #region Reg | 注册表操作
        /// <summary>
        /// 注册表操作
        /// </summary>
        public class Reg
        {
            /// <summary>
            /// 在用户目录下判断注册表项是否存在
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
            /// 在用户目录下创建注册表项
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
            /// 在用户目录下判断注册表项的键值是否存在
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
            /// 在用户目录下创建键
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
            /// 获取在用户目录下的键值
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
                    return "Err";
                }
            }
        }
        #endregion

        #region Proc | 进程操作
        /// <summary>
        /// 进程操作
        /// </summary>
        public class Proc
        {
            /// <summary>
            /// 获取进程
            /// </summary>
            /// <param name="strProcName">进程名</param>
            /// <returns></returns>
            public static Process GetProc(string strProcName)
            {
                //精确进程名  用GetProcessesByName
                Process[] processList = Process.GetProcesses();
                foreach (Process process in processList)
                {
                    if (process.ProcessName.ToLower() == strProcName.ToLower())
                    {
                        return process;
                    }
                }
                return new Process();
            }

            /// <summary>
            /// 获取进程（模糊）
            /// </summary>
            /// <param name="strProcName">进程名</param>
            /// <param name="noTrue">是否为模糊搜索</param>
            /// <returns></returns>
            public static List<Process> GetProc(string strProcName, bool noTrue)
            {
                //模糊进程名  枚举
                List<Process> processes = new List<Process>();
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.ProcessName.IndexOf(strProcName) > -1)  //第一个字符匹配的话为0，这与VB不同
                    {
                        processes.Add(p);
                    }
                }
                return processes;
            }

            /// <summary>
            /// 结束进程
            /// </summary>
            /// <param name="p">进程</param>
            public static void KillProc(Process p)
            {
                try
                {
                    p.Kill();
                }
                catch { }
            }

            /// <summary>
            /// 判断进程是否存在
            /// </summary>
            /// <param name="name">进程名</param>
            /// <returns></returns>
            public static bool HasProc(string name)
            {
                Process[] processList = Process.GetProcesses();
                foreach (Process process in processList)
                {
                    if (process.ProcessName.ToLower() == name.ToLower())
                    {
                        return true;
                    }
                }
                return false;
            }
        }
        #endregion

        #region Net | 网络操作
        /// <summary>
        /// 网络操作
        /// </summary>
        public class Net
        {
            /// <summary>
            /// Ping IP （详细用法参见方法内注明）
            /// </summary>
            /// <param name="ipStr">IP地址</param>
            /// <param name="data">Ping数据</param>
            /// <returns></returns>
            public static PingReply GetPing(string ipStr, string data)
            {
                //构造Ping实例
                Ping pingSender = new Ping();
                //Ping 选项设置
                PingOptions options = new PingOptions();
                options.DontFragment = true;
                //测试数据
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                //设置超时时间
                int timeout = 120;
                //调用同步 send 方法发送数据,将返回结果保存至PingReply实例
                try
                {
                    PingReply reply = pingSender.Send(ipStr, timeout, buffer, options);
                    return reply;
                }
                catch
                {
                    //构造Ping实例
                    pingSender = new Ping();
                    //Ping 选项设置
                    options = new PingOptions();
                    options.DontFragment = true;
                    //测试数据
                    buffer = Encoding.ASCII.GetBytes(data);
                    //设置超时时间
                    timeout = 120;
                    //调用同步 send 方法发送数据,将返回结果保存至PingReply实例
                    PingReply reply = pingSender.Send("localhost", timeout, buffer, options);
                    return reply;
                }
                /*
                reply.Status == IPStatus.Success
                答复的主机地址：                  reply.Address.ToString()
                往返时间：                       reply.RoundtripTime
                生存时间( TTL ):                 reply.Options.Ttl
                是否控制数据包的分段：             reply.Options.DontFragment
                缓冲区大小:                      reply.Buffer.Length
                */
            }
        }
        #endregion

        #region Win | 窗口操作
        /// <summary>
        /// 窗口操作
        /// </summary>
        public class Win
        {
            public static class win32
            {
                [DllImport("user32.dll")]
                public static extern long GetWindowLong(IntPtr hWnd, int nlndex);
                [DllImport("user32.dll")]
                public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
                [DllImport("user32.dll")]
                public static extern long SetWindowLong(IntPtr hWnd, int nlndex, long dwNewLong);
                [DllImport("user32.dll")]
                public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            }
            /// <summary>
            /// 通过窗口名或类名寻找窗口
            /// </summary>
            /// <param name="lpClassName">窗口名</param>
            /// <param name="lpWindowName">类名</param>
            public static IntPtr FindWindow(string lpClassName, string lpWindowName)
            {
                return win32.FindWindow(lpClassName, lpWindowName);
            }
            /// <summary>
            /// 操作对应句柄的窗口任务栏图标(暂时只支持隐藏，另一个要用再写 = =)
            /// </summary>
            /// <param name="hWnd">句柄</param>
            /// <param name="isHide">是否隐藏</param>
            public static void SetTaskbarIcon(IntPtr hWnd, bool isHide)
            {
                if (isHide)
                {
                    long style = win32.GetWindowLong(hWnd, -16);
                    style &= ~(0x10000000L);
                    style |= 0x00000080L;
                    style &= ~(0x00040000L);
                    win32.ShowWindow(hWnd, 0);
                    win32.SetWindowLong(hWnd, -16, style);
                    win32.ShowWindow(hWnd, 5);
                    win32.ShowWindow(hWnd, 0);
                }
            }
        }
        #endregion

        #region Anim | UI 动画
        /// <summary>
        /// UI 动画
        /// </summary>
        public class Anim
        {
            /// <summary>
            /// 将滚动条平滑滚动到指定位置（注意：这是个动画循环，请新建线程使用）
            /// </summary>
            /// <param name="scroll">ScrollViewer</param>
            /// <param name="to">位置</param>
            /// <param name="window">UI线程对应的窗口</param>
            /// <param name="speed">速度</param>
            /// <param name="where">方向</param>
            public static void ScrollRollTo(MainWindow window, ScrollViewer scroll, int to, int speed, bool where)
            {
                if (where)
                {
                    for (int i = Convert.ToInt32(scroll.VerticalOffset); i <= to; i = i + 2)
                    {
                        window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            scroll.ScrollToVerticalOffset(i);
                        });
                        Thread.Sleep(speed);
                    }
                }
                else
                {
                    for (int i = to; i >= Convert.ToInt32(scroll.VerticalOffset); i = i - 2)
                    {
                        window.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            scroll.ScrollToVerticalOffset(i);
                        });
                        Thread.Sleep(speed);
                    }

                }
            }
        }
        #endregion

        #region SystemGet | 系统咨询
        /// <summary>
        /// 系统咨询
        /// </summary>
        public class SystemGet
        {
            /// <summary>
            /// 获取系统颜色模式（Windows10有效，其他默认返回false）
            /// </summary>
            /// <returns>颜色模式（true深色，false浅色）</returns>
            public static bool GetDarkMode()
            {
                bool isDarkMode = false;
                try
                {
                    string dark = SSUserClass.Reg.GetRegKey(Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme");
                    if (dark.Equals("Err"))      // 读取注册表出错（可能是不存在）
                    {
                        // 检查系统颜色模式失败，初始化为浅色");
                        isDarkMode = false;
                    }
                    else
                    {
                        if (dark.Equals("1"))
                        {
                            isDarkMode = false;
                        }
                        else
                        {
                            isDarkMode = true;
                        }
                    }
                }
                catch
                {
                    isDarkMode = false;
                }
                return isDarkMode;
            }
        }

        #endregion

        #region Other | 杂项
        /// <summary>
        /// 杂项
        /// </summary>
        public class Other
        {
            /// <summary>
            /// 计算字符串在特定字体大小，字体下实际占用的长度。
            /// </summary>
            /// <param name="text">字符串</param>
            /// <param name="fontSize">字体大小</param>
            /// <param name="fontFamily">字体</param>
            /// <returns></returns>
            public static double MeasureTextWidth(string text, double fontSize, string fontFamily)
            {
                FormattedText formattedText = new FormattedText(
                text,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                new Typeface(fontFamily.ToString()),
                fontSize,
                Brushes.Black
                );
                return formattedText.WidthIncludingTrailingWhitespace;
            }
        }
        #endregion
    }
}
