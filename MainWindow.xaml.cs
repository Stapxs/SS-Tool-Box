﻿using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using SS_Tool_Box.Controls;
using SS_Tool_Box.Function;
using SS_Tool_Box.Pages.SortPages;
using SS_Tool_Box.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SS_Tool_Box
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 定义 | 全局程序变量

        // 初始化开始时间
        DateTime startRun;

        // 退出标记
        public static bool readyExit = false;

        // 页面切换堆栈
        public class pageInfo
        {
            public object lastPage;
            public string lastPageName;

            public pageInfo(object page, string name)
            {
                lastPage = page;
                lastPageName = name;
            }
        }
        public Stack<pageInfo> pageStack = new Stack<pageInfo>();

        // 线程堆栈
        public static Stack<Thread> threads = new Stack<Thread>();

        // MainWindow
        public static MainWindow main = null;

        // 检查更新数据
        public static List<string> back = new List<string>();

        #endregion

        // 加载完成标记
        private bool loadDone = false;

        // 程序基本信息
        public class verInfo
        {
            public static string verStr = "Dev-0.4.1";      // 版本号
            public static int verBulidTimes = 13;            // 编译编号
            public static double verNum = 41.113;             // 版本号数字
            #if DEBUG
            public static string verType = "Debug";         // 版本类型
            #else
            public static string verType = "Dev";
            #endif
        }

        public MainWindow()
        {
            Log.AddLog("app", "感谢使用 SS Tool Box，当前版本为" + verInfo.verStr + " build " + verInfo.verBulidTimes);
            Log.AddLog("main", "开始初始化界面……");
            startRun = DateTime.Now;

            InitializeComponent();

            main = this;

            // 亚克力模糊
            // WindowBlur.SetIsEnabled(this, true);

            #region 0 - 初始化基础服务

            Log.StartLogOut();                      // 日志
            Options.ReadOpt();                      // 设置
            // UI.ToastHelper.StartShower();        // 吐司

            // 颜色模式切换事件
            SystemEvents.UserPreferenceChanged +=
                new UserPreferenceChangedEventHandler(Event_UserPreferenceChanged);

#endregion
            #region 1 - 初始化颜色主题

            if (Options.GetOpt("autoDarkMode")[0] == "true")
            {
                // 判断颜色模式
                string isOpen = Features.Reg.GetRegKey(Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme");
                if (isOpen == "1")
                {
                    UI.Color.ChangeDark(false);
                }
            }
            else
            {
                if ((Options.GetOpt("darkMode"))[0] == "false")
                {
                    UI.Color.ChangeDark(false);
                }
            }
            string langValue = "en_US";
            if (Options.GetOpt("language")[0][0] != '~')
            {
                foreach (UI.Localization.localVer info in UI.Localization.indexLocals)
                {
                    if (info.value + ".xaml" == Options.GetOpt("language")[0])
                    {
                        langValue = info.value;
                        break;
                    }
                }
                UI.Localization.ChangeLanguage(langValue + ".xaml", true);
            }
            else
            {
                UI.Localization.ChangeLanguage(Options.GetOpt("language")[0].Substring(1), true, true);
            }

#endregion
            #region 4 - 初始化页面

            // 版本号
            viewVersion.Text = verInfo.verStr;
#if DEBUG
            viewVersion.Text = verInfo.verStr + " DBuild " + verInfo.verBulidTimes;
#endif

            // 加载主页
            Home page = new Home();
            page.ParentWindow = this;
            MainCol.Content = new Frame()
            {
                Content = page
            };

            // 初始化 Tab
            ColorTools colors = new ColorTools();
            colors.ParentWindow = this;
            conColorTools.Content = new Frame()
            {
                Content = colors
            };
            OtherTools others = new OtherTools();
            others.ParentWindow = this;
            conOtherTools.Content = new Frame()
            {
                Content = others
            };

            // 初始化 Home 按钮
            if(Options.GetOpt("alwaysShowHome")[0] == "true")
            {
                Home.Visibility = Visibility.Visible;
                Title.Margin = new Thickness(10, 0, 0, 0);
            }
#endregion
            #region 5 - 其他操作

            // 检查更新
            Thread thread = new Thread(getUpdate);
            MainWindow.threads.Push(thread);
            thread.Start();

#endregion

            Log.AddLog("main", "加载完毕，耗时" + DateTime.Now.Subtract(startRun).TotalSeconds + "秒");
            loadDone = true;
        }

        #region 事件 | 主按钮
        private void B_Exit(object sender, RoutedEventArgs e)
        {
            // 关闭
            Log.AddLog("main", "正在等待所有辅助线程退出……");
            // 等待线程退出（超时0.5秒）
            Thread thread = new Thread(runWaitExit);
            thread.Start();
        }

        private void runWaitExit()
        {
            exitMain("default");
        }

        private void B_SmallWin(object sender, RoutedEventArgs e)
        {
            // 最小化
            WindowState = WindowState.Minimized;
        }

        private void B_Back(object sender, RoutedEventArgs e)
        {
            backPage();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.XButton1)
            {
                backPage();
            }
        }

        private void backPage()
        {
            // 返回上级
            if (MainTitle.Text != "林槐工具箱 - SS Tool Box")
            {
                // 加载上一个界面
                Log.AddLog("ui", "返回窗口到" + pageStack.Peek().lastPageName);
                MainCol.Content = new Frame()
                {
                    Content = pageStack.Peek().lastPage
                };
                MainTitle.Text = pageStack.Peek().lastPageName;
                // 出栈
                pageStack.Pop();
                // 判断是否显示回到主页按钮
                if (pageStack.Count > 1 || Options.GetOpt("alwaysShowHome")[0] == "true")
                {
                    Home.Visibility = Visibility.Visible;
                }
                else
                {
                    Home.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void B_Home(object sender, RoutedEventArgs e)
        {
            // 返回主页
            backHome();
        }

        private void B_Flash(object sender, RoutedEventArgs e)
        {
            // 刷新页面
            flashPage();
        }

        private void B_More(object sender, RoutedEventArgs e)
        {
            // 设置
            Pages.Options opt = new Pages.Options();
            Application app = Application.Current;
            changePage(opt, (string)app.Resources["options"]);
        }

#endregion
        #region 事件 | 主窗口
        public void exitMain(string mode)
        {
            if (mode == "default")
            {
                if (!readyExit)
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        WindowState = WindowState.Minimized;
                        this.ShowInTaskbar = false;
                    });
                    readyExit = true;
                    Thread.Sleep(500);
                    Log.AddLog("main", "正在结束所有辅助线程……（ " + threads.Count + " 个）");
                    // 超时强制退出
                    while (threads.Count != 0)
                    {
                        threads.Pop().Abort();
                    }
                    // 退出日志系统
                    Log.exit();
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                    // 退出主程序
                    Application.Current.Shutdown();
                    });
                }
            }
            else if(mode == "update")
            {
                Log.AddLog("update", "正在重启更新。");
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    WindowState = WindowState.Minimized;
                    this.ShowInTaskbar = false;
                });
                readyExit = true;
                Thread.Sleep(500);
                Log.AddLog("main", "正在结束所有辅助线程……（ " + threads.Count + " 个）");
                // 超时强制退出
                while (threads.Count != 0)
                {
                    threads.Pop().Abort();
                }
                // 退出日志系统
                Log.exit();
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    // 退出主程序
                    string where = Directory.GetCurrentDirectory();
                    where = where + @"\run.vbs";
                    System.Diagnostics.Process process;
                    process = System.Diagnostics.Process.Start(@where);
                    Application.Current.Shutdown();
                });
            }
        }

        private void Event_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (Options.GetOpt("autoDarkMode")[0] == "true")
            {
                // 判断颜色模式
                string isOpen = Features.Reg.GetRegKey(Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme");
                if (isOpen == "1")
                {
                    UI.Color.ChangeDark(false);
                }
                else
                {
                    UI.Color.ChangeDark(true);
                }
            }
        }

        private void mainTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tab = (TabControl)sender;
            // 返回主页
            backHome();
        }

        public void backHome(bool showLog = true)
        {
            if (loadDone && MainTitle.Text != "林槐工具箱 - SS Tool Box")
            {
                // 加载主页
                Home page = new Home();
                if (showLog)
                {
                    Log.AddLog("ui", "切换窗口到" + "林槐工具箱 - SS Tool Box" + "（ " + page + " ）");
                }
                page.ParentWindow = this;
                MainCol.Content = new Frame()
                {
                    Content = page
                };
                MainTitle.Text = "林槐工具箱 - SS Tool Box";
                // 隐藏回到主页按钮
                Home.Visibility = Visibility.Collapsed;
                Title.Margin = new Thickness(0, 0, 0, 0);
                // 清空堆栈
                pageStack.Clear();
            }
        }

        private void Drag_MouseMove(object sender, MouseEventArgs e)
        {
            // 鼠标拖动事件
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            }
            catch
            {
                Log.AddErr("main", "鼠标拖拽执行异常！");
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // TODO 完成加载
        }

#endregion
        #region 事件 | 搜索框

        private void Seach_GotFocus(object sender, RoutedEventArgs e)
        {
            // 得到焦点，显示浮动搜索框
            panSeach.Visibility = Visibility.Visible;
            // 删除已经创建过的结果控件
            StackPanel sp = Features.GetChildObject<StackPanel>(listSeach, "seachoutpan");
            if (sp != null)
            {
                listSeach.Children.Remove(sp);
            }
            // 显示没有结果
            seachNone.Visibility = Visibility.Visible;
        }

        private void Seach_Close(object sender, RoutedEventArgs e)
        {
            // 关闭搜索框
            panSeach.Visibility = Visibility.Collapsed;
            // 去除输入框焦点
            conColorTools.Focus();
            // 清空输入框
            SeachBox.Text = "";
        }

        private void Seach_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 搜索框
            TextBox box = (TextBox)sender;
            // 删除已经创建过的结果控件
            StackPanel sp = Features.GetChildObject<StackPanel>(listSeach, "seachoutpan");
            if (sp != null)
            {
                listSeach.Children.Remove(sp);
            }
            // 检索工具目录
            List<UI.Tools.ToolVer> toolist = new List<UI.Tools.ToolVer>();
            UI.Tools tools = new UI.Tools();
            foreach (UI.Tools.ToolVer info in tools.List)
            {
                Application app = Application.Current;
                info.cardInfo[0] = (string)app.Resources["tool_title_" + info.name];
                if (info.cardInfo[0].IndexOf(box.Text) >= 0 && info.type != "Hidden")
                {
                    toolist.Add(info);
                }
                if(info.cardInfo[0] == box.Text && info.type == "Hidden")
                {
                    toolist.Add(info);
                }
            }
            if (toolist.Count != 0)
            {
                // 隐藏没有结果
                seachNone.Visibility = Visibility.Collapsed;
            }
            StackPanel stack = new StackPanel();
            stack.Name = "seachoutpan";
            if (toolist.Count <= 5)
            {
                foreach (UI.Tools.ToolVer info in toolist)
                {
                    stack.Children.Add(new SeachToolView(info.cardInfo[0], info.cardInfo[3], info.page, main));
                }
            }
            else
            {
                // 大于 5 条生成 ScrollViewer
                ScrollViewer scroll = new ScrollViewer();
                scroll.Height = 150;
                scroll.Margin = new Thickness(0, 0, 3, 0);
                StackPanel stackin = new StackPanel();
                foreach (UI.Tools.ToolVer info in toolist)
                {
                    stackin.Children.Add(new SeachToolView(info.cardInfo[0], info.cardInfo[3], info.page, main));
                }
                scroll.Content = stackin;
                stack.Children.Add(scroll);
            }
            listSeach.Children.Add(stack);
        }

#endregion
        #region 事件 | 消息窗

        private void msgClose_Click(object sender, RoutedEventArgs e)
        {
            // 执行取消事件
            try
            {
                if (_clickFun != null && _clickFun[1] != null)
                {
                    _clickFun[1]();
                }
            }
            catch (Exception en)
            {
                Log.AddErr("sms", "弹窗点击事件无效：" + en.ToString());
            }
            isEnd = true;

            // 关闭窗口
            Shaker.IsEnabled = false;
            panMessageBox.Visibility = Visibility.Collapsed;

            Thread thread = new Thread(runCollButton);
            threads.Push(thread);
            thread.Start();
        }

        private void runCollButton()
        {
            Thread.Sleep(200);
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                Shaker.Visibility = Visibility.Collapsed;
            });
        }

#endregion

        #region 操作 | 主窗口

        /// <summary>
        /// 更改嵌入页面
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageTitle"></param>
        /// <returns>操作是否成功</returns>
        public bool changePage(object page, string pageTitle)
        {
            Log.AddLog("ui", "切换窗口到" + pageTitle + "（ " + page + " ）");
            try
            {
                // 关闭搜索框
                panSeach.Visibility = Visibility.Collapsed;
                SeachBox.Text = "";
                // 记录上个界面（入栈）
                pageStack.Push(new pageInfo(MainCol.Content, MainTitle.Text));
                // 切换界面
                MainCol.Content = new Frame()
                {
                    Content = page
                };
                MainTitle.Text = pageTitle;
                // 判断是否显示回到主页按钮和刷新按钮
                if (pageStack.Count > 1 || Options.GetOpt("alwaysShowHome")[0] == "true")
                {
                    Home.Visibility = Visibility.Visible;
                    // Flash.Visibility = Visibility.Visible;
                    Title.Margin = new Thickness(10, 0, 0, 0);
                }
                else
                {
                    Home.Visibility = Visibility.Collapsed;
                    // Flash.Visibility = Visibility.Collapsed;
                    Title.Margin = new Thickness(0, 0, 0, 0);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 刷新嵌入界面
        /// </summary>
        /// <returns></returns>
        public bool flashPage()
        {
            try
            {
                // 不计入堆栈
                object now = MainCol.Content;
                string title = MainTitle.Text;
                Home page = new Home();
                changePage(page, MainTitle.Text);
                changePage(now, title);
                Log.AddLog("ui", "刷新界面" + pageStack.Peek().lastPageName + "（ " + pageStack.Peek().lastPage + " ）");
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检查更新
        /// </summary>
        /// <returns></returns>
        private void getUpdate()
        {
            Log.AddLog("update", "正在检查更新……");

            if (File.Exists("UpdateBash.bat"))
            {
                File.Delete("UpdateBash.bat");
                if (File.Exists("run.vbs"))
                {
                    File.Delete("run.vbs");
                }
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    MsgAdd(
                        new List<string> { "更新完成", "我们成功更新了 Stapx Steve Tool Box！开始体验全新的功能吧！" },
                        new List<string> { "好" }, null);
                });
            }

            if (Options.GetOpt("stopUpd")[0] == "false" || Options.GetOpt("stopUpd")[0] == "ERR")
            {
                try
                {
                    string updateVersion = "1";

                    int updLink = Options.GetOpt("updLink")[0] == "ERR" ? 0 : int.Parse(Options.GetOpt("updLink")[0]);
                    string getStr = HttpUitls.Get(linkList[updLink].link);
                    try
                    {
                        JObject obj = JObject.Parse(getStr);
                        if (obj["Version"].ToString() == updateVersion)
                        {
                            back.Add(obj["MainVersion"].ToString());
                            back.Add(obj["Time"].ToString());
                            back.Add(obj["Url"].ToString());
                            back.Add(obj["Logs"].ToString());
                            Log.AddLog("update", "检查更新完成。");
                        }
                        else
                        {
                            Log.AddErr("update", "检查更新失败：更新文件版本不符合，更新失败。");
                        }
                    }
                    catch (Exception e)
                    {
                        Log.AddErr("update", "检查更新失败：" + e + "\n" + getStr.Replace("\n", "\\n").Replace("\t", "\\t"));
                        UI.ToastHelper.Add("检查更新失败");
                    }
                    if (back.Count != 0 && double.Parse(back[0]) <= verInfo.verNum)
                    {
                        Log.AddLog("update", "当前版本为最新。");
                    }
                    else
                    {
                        Log.AddLog("update", "最新版本为：" + back[0] + " > " + verInfo.verNum);
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                        // 切换到设置界面
                        Pages.Options opt = new Pages.Options();
                            Application app = Application.Current;
                            changePage(opt, app.Resources["options"] + " - " + app.Resources["options_main_title_def"]);
                        });
                    }
                }
                catch(Exception e)
                {
                    Log.AddErr("update", "检查更新失败：" + e);
                    UI.ToastHelper.Add("检查更新失败");
                }
            }
            else
            {
                Log.AddLog("update", "检查更新已关闭。");
            }
        }

        public class LinkVer
        {
            public string name { get; }
            public string link { get; }
            public string type { get; }

            public LinkVer(string v1, string v2, string v3)
            {
                type = v1;
                name = v2;
                link = v3;
            }
        }

        public static List<LinkVer> linkList = new List<LinkVer>() {
            new LinkVer("Dev", "Github Dev", "https://raw.githubusercontent.com/Stapxs/SS-Tool-Box/dev/Latest/LatestLog.txt"),
            new LinkVer("Release", "Blog Release", "https://stapx.chuhelan.com/api/SS-Tool-Box/getVersion.php?dev=False&type=Desktop"),
        };

#endregion
        #region 操作 | 消息窗
        public delegate void FunMsgButton();

        private bool isShowing = false;
        private bool isEnd = false;

        public class msgVer
        {
            public msgVer(List<string> info, List<string> button, List<FunMsgButton> clickFun)
            {
                Info = info;
                Button = button;
                ClickFun = clickFun;
            }

            public List<string> Info { get; }
            public List<string> Button { get; }
            public List<FunMsgButton> ClickFun { get; }
        }

        private Queue<msgVer> msgList = new Queue<msgVer>();
        List<FunMsgButton> _clickFun = new List<FunMsgButton>();

        private void MsgShow(List<string> info, List<string> button, List<FunMsgButton> clickFun)
        {
            isShowing = true;
            // 显示当前条目
            msgTitle.Text = info[0];
            msgText.Text = info[1];
            if(button.Count == 1)
            {
                msgBNO.Visibility = Visibility.Collapsed;
            }
            else
            {
                msgBNT.Text = button[1];
            }
            msgBOT.Text = button[0];
            _clickFun = clickFun;
            // 显示
            Shaker.Visibility = Visibility.Visible;
            Shaker.IsEnabled = true;
            panMessageBox.Visibility = Visibility.Visible;
            // 等待操作
            Thread thread = new Thread(runShowMsg);
            threads.Push(thread);
            thread.Start();
        }

        private void runShowMsg()
        {
            // 输出积压的全部消息
            while (msgList.Count != 0)
            {
                while (!isEnd)
                {
                    Thread.Sleep(500);
                }
                isEnd = false;
                msgVer infos = msgList.Dequeue();
                bool setFinished = false;
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    panMessageBox.Visibility = Visibility.Collapsed;
                    msgTitle.Text = infos.Info[0];
                    msgText.Text = infos.Info[1];
                    if (infos.Button.Count == 1)
                    {
                        msgBNO.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        msgBNT.Text = infos.Button[1];
                    }
                    msgBOT.Text = infos.Button[0];
                    _clickFun = infos.ClickFun;
                    setFinished = true;
                });
                while (!setFinished) ;
                Thread.Sleep(200);
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    panMessageBox.Visibility = Visibility.Visible;
                });
            }
            while (!isEnd)
            {
                Thread.Sleep(500);
            }
            isEnd = false;
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                panMessageBox.Visibility = Visibility.Collapsed;
                Shaker.IsEnabled = false;
            });
            Thread.Sleep(200);
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                Shaker.Visibility = Visibility.Collapsed;
            });
            isShowing = false;
        }

        public void MsgAdd(List<string> info, List<string> button, List<FunMsgButton> clickFun)
        {
            if (!isShowing)
            {
                MsgShow(info, button, clickFun);
            }
            else
            {
                msgList.Enqueue(new msgVer(info, button, clickFun));
            }
        }

        private void msgBOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_clickFun != null && _clickFun[0] != null)
                {
                    _clickFun[0]();
                }
            }
            catch(Exception eo)
            {
                Log.AddErr("sms", "弹窗点击事件无效：" + eo.ToString());
            }
            isEnd = true;
        }

        private void msgBNO_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_clickFun != null && _clickFun[1] != null)
                {
                    _clickFun[1]();
                }
            }
            catch(Exception en)
            {
                Log.AddErr("sms", "弹窗点击事件无效：" + en.ToString());
            }
            isEnd = true;
        }

#endregion

        public class MyException : ApplicationException
        {
            public MyException(string message) : base(message) { }

            public override string Message
            {
                get
                {
                    return base.Message;
                }
            }
        }
    }
}
