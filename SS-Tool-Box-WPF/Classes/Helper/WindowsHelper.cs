using Microsoft.Win32;
using SS_Tool_Box.Classes.Structure;
using SS_Tool_Box.Function;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static SS_Tool_Box.MainWindow;

namespace SS_Tool_Box.Classes.Helper
{
    public class WindowsHelper
    {
        /// <summary>
        /// 更改嵌入页面
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageTitle"></param>
        /// <returns>操作是否成功</returns>
        public static bool changePage(object page, string pageTitle)
        {
            Log.AddLog("ui", "切换窗口到" + pageTitle + "（ " + page + " ）");
            try
            {
                // 关闭搜索框
                main.panSeach.Visibility = Visibility.Collapsed;
                main.SeachBox.Text = "";
                // 记录上个界面（入栈）
                main.pageStack.Push(new pageInfo(main.MainCol.Content, main.MainTitle.Text));
                // 清空
                main.MainCol.Content = null;
                GC.Collect();
                // 切换界面
                Page changePage = Activator.CreateInstance(page as Type) as Page;
                main.MainCol.Content = new Frame()
                {
                    Content = changePage
                };
                main.MainTitle.Text = pageTitle;
                // 判断是否显示回到主页按钮和刷新按钮
                if (main.pageStack.Count > 1 || Options.GetOpt("alwaysShowHome")[0] == "true")
                {
                    main.Home.Visibility = Visibility.Visible;
                    // Flash.Visibility = Visibility.Visible;
                    main.Title.Margin = new Thickness(10, 0, 0, 0);
                }
                else
                {
                    main.Home.Visibility = Visibility.Collapsed;
                    // Flash.Visibility = Visibility.Collapsed;
                    main.Title.Margin = new Thickness(0, 0, 0, 0);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 回到主页
        /// </summary>
        /// <param name="showLog"></param>
        /// <param name="index"></param>
        public static void backHome(bool showLog = true, int index = 0)
        {
            if (main.loadDone && main.MainTitle.Text != "林槐工具箱 - SS Tool Box")
            {
                // 清空
                main.MainCol.Content = null;
                GC.Collect();
                // 加载主页
                Home page = new Home(index);
                if (showLog)
                {
                    Log.AddLog("ui", "切换窗口到" + "林槐工具箱 - SS Tool Box" + "（ " + page + " ）");
                }
                page.ParentWindow = main;
                main.MainCol.Content = new Frame()
                {
                    Content = page
                };
                main.MainTitle.Text = "林槐工具箱 - SS Tool Box";
                // 隐藏回到主页按钮
                main.Home.Visibility = Visibility.Collapsed;
                main.Title.Margin = new Thickness(0, 0, 0, 0);
                // 清空堆栈
                main.pageStack.Clear();
            }
        }

        /// <summary>
        /// 主题与颜色
        /// </summary>
        public class Color
        {
            public ColorInfo[] colors = new ColorInfo[]
            {
                new ColorInfo("林槐蓝", (System.Windows.Media.Color)ColorConverter.ConvertFromString("#4C6EF5")),
                new ColorInfo("坏猫黄", (System.Windows.Media.Color)ColorConverter.ConvertFromString("#FFCC00")),
                new ColorInfo("墨竹绿", (System.Windows.Media.Color)ColorConverter.ConvertFromString("#546E36")),
                new ColorInfo("天牧红", (System.Windows.Media.Color)ColorConverter.ConvertFromString("#DE373F")),
                new ColorInfo("玄素黑", (System.Windows.Media.Color)ColorConverter.ConvertFromString("#31343B")),
            };

            private SolidColorBrush brush = (SolidColorBrush)Application.Current.FindResource("colorMain");

            private List<string[]> colorUIDark = new List<string[]> {
                new string[] { "colorBg", "#2D2D2D"},
                new string[] { "colorCard", "#3A3A3A"},
                new string[] { "colorCard1", "#494949"},
                new string[] { "colorCard2", "#5F5F5F"},

                new string[] { "colorFont", "#FFFFFF"},
                new string[] { "colorFont1", "#CFCFCF"},
                new string[] { "colorFont2", "#B0B0B0"},
            };
            private List<string[]> colorUILight = new List<string[]> {
                new string[] { "colorBg", "#F2F4F3"},
                new string[] { "colorCard", "#FDFFFD"},
                new string[] { "colorCard1", "#F2F4F3"},
                new string[] { "colorCard2", "#E8EBE8"},

                new string[] { "colorFont", "#50534F"},
                new string[] { "colorFont1", "#969FB3"},
                new string[] { "colorFont2", "#969FB3"},
            };

            public static bool isDark = true;

            /// <summary>
            /// 切换颜色模式
            /// </summary>
            /// <param name="isDark"></param>
            /// <returns></returns>
            public bool ChangeDark(bool isDark)
            {
                if(Color.isDark == isDark)
                {
                    return false;
                }
                List<string[]> colorUI;
                if (isDark)
                {
                    Color.isDark = true;
                    colorUI = colorUIDark;
                    // 处理主题色
                    changeMainColorDark();
                }
                else
                {
                    Color.isDark = false;
                    colorUI = colorUILight;
                    // 处理主题色
                    if (Options.GetOpt("autoColor")[0] == "true")
                    {
                        GetASaveSysColor();
                    }
                    else
                    {
                        bool get = false;
                        foreach (ColorInfo colorInfo in colors)
                        {
                            if (colorInfo.name == Options.GetOpt("seleColor")[0])
                            {
                                get = true;
                                Application.Current.Resources["colorMain"] = new SolidColorBrush(ChangeColor(colorInfo.color, (int)(255 * 0.4)));
                            }
                        }
                        if (!get)
                        {
                            Options.SetOpt("autoColor", "true");
                            GetASaveSysColor();
                        }
                    }
                }
                // 刷新页面颜色
                foreach (string[] info in colorUI)
                {
                    Application.Current.Resources.Remove(info[0]);
                    Application.Current.Resources.Add(info[0],
                        new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(info[1])));
                }
                return true;
            }

            
            /// <summary>
            /// 修改颜色亮度
            /// </summary>
            /// <param name="addLightPer"></param>
            /// <returns></returns>
            public System.Windows.Media.Color ChangeColor(System.Windows.Media.Color nowColor, double addLightPer)
            {
                double[] hsl = new UI.ColorChange().RGBToHSL(nowColor);
                double[] rgb = new UI.ColorChange().HslToRgb(hsl[0], hsl[1], hsl[2] + ((255 - hsl[2]) * addLightPer));
                return System.Windows.Media.Color.FromArgb(255, (byte)(int)rgb[0], (byte)(int)rgb[1], (byte)(int)rgb[2]);
            }
            public System.Windows.Media.Color ChangeColor(System.Windows.Media.Color nowColor, int light)
            {
                double[] hsl = new UI.ColorChange().RGBToHSL(nowColor);
                double[] rgb = new UI.ColorChange().HslToRgb(hsl[0], hsl[1], light);
                return System.Windows.Media.Color.FromArgb(255, (byte)(int)rgb[0], (byte)(int)rgb[1], (byte)(int)rgb[2]);
            }
            public void changeMainColorDark()
            {
                if (Options.GetOpt("autoColor")[0] == "true")
                {
                    GetASaveSysColor();
                }
                else
                {
                    bool get = false;
                    foreach (ColorInfo colorInfo in colors)
                    {
                        if (colorInfo.name == Options.GetOpt("seleColor")[0])
                        {
                            get = true;
                            Application.Current.Resources["colorMain"] = new SolidColorBrush(ChangeColor(colorInfo.color, (int)(255 * 0.4)));
                        }
                    }
                    if (!get)
                    {
                        Options.SetOpt("autoColor", "true");
                        GetASaveSysColor();
                    }
                }
                System.Windows.Media.Color color = ChangeColor((System.Windows.Media.Color)ColorConverter.ConvertFromString(Application.Current.Resources["colorMain"].ToString()), 0.6);
                Application.Current.Resources["colorMain"] = new SolidColorBrush(color);
            }

            /// <summary>
            /// 获取系统主题颜色
            /// </summary>
            /// <returns></returns>
            private System.Windows.Media.Color GetSystemColor()
            {
                RegistryKey color = new Reg().GetRegKey(Registry.CurrentUser, @"Software\Microsoft\Windows\DWM", "AccentColor", true);
                if (color != null)
                {
                    int accentColor = (int)color.GetValue("AccentColor");
                    return System.Windows.Media.Color.FromArgb(
                        255,
                        (byte)(accentColor & 0xFF),
                        (byte)((accentColor >> 8) & 0xFF),
                        (byte)((accentColor >> 16) & 0xFF));
                }
                return System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
            }
            public void GetASaveSysColor()
            {
                // 系统主题色
                System.Windows.Media.Color sysColor = GetSystemColor();
                if(sysColor.R != 255 && sysColor.G != 255 && sysColor.B != 255)
                {
                    // 保存系统颜色
                    Application.Current.Resources["colorMainDef"] = new SolidColorBrush(sysColor);
                    // 对颜色进行平衡亮度（将亮度改为 40%）
                    if (Options.GetOpt("autoColor")[0] != "false")
                    {
                        // 应用颜色
                        Application.Current.Resources["colorMain"] = new SolidColorBrush(ChangeColor(sysColor, (int)(255 * 0.4)));
                    }
                }
                else
                {
                    Log.AddErr("main", "获取系统主题色错误！");
                }
            }

            public bool isNowDark()
            {
                string isOpen = new Reg().GetRegKey(Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme");
                if (Options.GetOpt("autoDarkMode")[0] == "true")
                {
                    if(isOpen != "1")
                    {
                        return true;
                    }
                }
                else
                {
                    if(Options.GetOpt("darkMode")[0] == "true")
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
