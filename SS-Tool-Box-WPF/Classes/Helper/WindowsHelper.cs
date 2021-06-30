using SS_Tool_Box.Classes.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            private SolidColorBrush brush = (SolidColorBrush)Application.Current.FindResource("colorMainBlue");

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

            public bool ChangeDark(bool isDark)
            {
                List<string[]> colorUI;
                if (isDark)
                {
                    colorUI = colorUIDark;
                }
                else
                {
                    colorUI = colorUILight;
                }
                foreach (string[] info in colorUI)
                {
                    Application.Current.Resources.Remove(info[0]);
                    Application.Current.Resources.Add(info[0],
                        new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(info[1])));
                }
                return false;
            }
        }
    }
}
