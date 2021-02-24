using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using SS_Tool_Box.Pages.SortPages;
using System.Windows.Media;
using SS_Tool_Box.Function;
using SS_Tool_Box.Controls;

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

        #endregion

        // 程序基本信息
        public class verInfo
        {
            public static string ver = "Dev-0.2.5";         // 版本号
            public static int verBulidTimes = 7;            // 编译编号
        }

        public MainWindow()
        {
            Log.AddLog("app", "感谢使用 SS Tool Box，当前版本为" + verInfo.ver + " build " + verInfo.verBulidTimes);
            Log.AddLog("main", "开始初始化界面……");
            startRun = DateTime.Now;

            InitializeComponent();

            main = this;

            // 亚克力模糊
            // WindowBlur.SetIsEnabled(this, true);

            #region 0 - 初始化基础服务

            Log.StartLogOut();
            UI.ToastHelper.StartShower();

            #endregion

            #region 4 - 初始化页面

            // 版本号
            viewVersion.Text = verInfo.ver;

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

            #endregion


            Log.AddLog("main", "加载完毕，耗时" + DateTime.Now.Subtract(startRun).TotalSeconds + "秒");
        }

        #region 事件 | 主按钮

        private void B_Exit(object sender, RoutedEventArgs e)
        {
            // 关闭
            readyExit = true;
            Log.AddLog("main", "正在等待所有辅助线程退出……");
            // 等待线程退出（超时2秒）
            long nowTime = long.Parse(Features.TimeHelper.GetCurrentTimestamp());
            while (long.Parse(Features.TimeHelper.GetCurrentTimestamp()) - nowTime <= 2) { }
            // 超时强制退出
            while (threads.Count != 0)
            {
                threads.Pop().Abort();
            }
            // 退出日志系统
            Log.exit();
            // 退出主程序
            Application.Current.Shutdown();
        }

        private void B_SmallWin(object sender, RoutedEventArgs e)
        {
            // 最小化
            WindowState = WindowState.Minimized;
        }

        private void B_Back(object sender, RoutedEventArgs e)
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
                if (pageStack.Count > 1)
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
            if (MainTitle.Text != "林槐工具箱 - SS Tool Box")
            {
                // 加载主页
                Home page = new Home();
                Log.AddLog("ui", "切换窗口到" + "林槐工具箱 - SS Tool Box" + "（ " + page + " ）");
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

        private void B_Flash(object sender, RoutedEventArgs e)
        {
            // 刷新页面
            flashPage();
        }

        private void B_More(object sender, RoutedEventArgs e)
        {
            // 设置
        }

        #endregion
        #region 事件 | 主窗口

        private void mainTab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TabControl tab = (TabControl)sender;
            //TabItem selected = (TabItem)tab.SelectedItem;
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
                //Log.Error("鼠标拖拽执行异常！", "崩溃");
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

        private void SeachBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // 失去焦点
            // 关闭搜索框
            panSeach.Visibility = Visibility.Collapsed;
            // 清空输入框
            SeachBox.Text = "";
        }

        private void Seach_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 搜索框
            TextBox box = (TextBox)sender;
            // 删除已经创建过的结果控件
            StackPanel sp = Features.GetChildObject<StackPanel>(listSeach, "seachoutpan");
            if(sp != null)
            {
                listSeach.Children.Remove(sp);
            }
            // 检索工具目录
            List<UI.Tools.ToolVer> toolist = new List<UI.Tools.ToolVer>();
            foreach(UI.Tools.ToolVer info in UI.Tools.List)
            {
                if(info.cardInfo[0].IndexOf(box.Text) >= 0)
                {
                    toolist.Add(info);
                }
            }
            if(toolist.Count != 0)
            {
                // 隐藏没有结果
                seachNone.Visibility = Visibility.Collapsed;
            }
            StackPanel stack = new StackPanel();
            stack.Name = "seachoutpan";
            if (toolist.Count <= 5)
            {
                foreach(UI.Tools.ToolVer info in toolist)
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
                if (pageStack.Count > 1)
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

        #endregion
    }
}
