using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using SS_Tool_Box.Pages.SortPages;
using System.Windows.Media;

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

        #endregion

        // 程序基本信息
        public class verInfo
        {
            public static string ver = "Dev-0.1.1";         // 版本号
            public static int verBulidTimes = 1;            // 编译编号
        }

        public MainWindow()
        {
            Log.AddLog("app", "感谢使用 SS Tool Box，当前版本为" + verInfo.ver + " build " + verInfo.verBulidTimes);
            Log.AddLog("main", "开始初始化界面……");
            startRun = DateTime.Now;

            InitializeComponent();

            // 亚克力模糊
            // WindowBlur.SetIsEnabled(this, true);

            #region 0 - 初始化基础服务

            Log.StartLogOut();
            // UI.ToastHelper.StartShower();

            #endregion

            #region 4 - 初始化页面

            // 版本号
            viewVersion.Text = verInfo.ver;

            // 加载主页
            Home page = new SS_Tool_Box.Home();
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
                    Title.Margin = new Thickness(35, 0, 0, 0);
                }
                else
                {
                    Home.Visibility = Visibility.Collapsed;
                    Title.Margin = new Thickness(0, 0, 0, 0);
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
                // 记录上个界面（入栈）
                pageStack.Push(new pageInfo(MainCol.Content, MainTitle.Text));
                // 切换界面
                MainCol.Content = new Frame()
                {
                    Content = page
                };
                MainTitle.Text = pageTitle;
                // 判断是否显示回到主页按钮
                if (pageStack.Count > 1)
                {
                    Home.Visibility = Visibility.Visible;
                    Title.Margin = new Thickness(35, 0, 0, 0);
                }
                else
                {
                    Home.Visibility = Visibility.Collapsed;
                    Title.Margin = new Thickness(0, 0, 0, 0);
                }
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
