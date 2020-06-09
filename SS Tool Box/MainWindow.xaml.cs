using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using SS_Tool_Box.Pages;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Walterlv.Windows.Effects;

namespace SS_Tool_Box
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isDarkMode = false;

        public MainWindow()
        {
            #region 1 - 初始化 LOG 文件

            // 初始化 LOG 文件
            if (!Directory.Exists(FilePath.log))
            {
                Directory.CreateDirectory(FilePath.log);
            }
            if (File.Exists(FilePath.log + "log.log"))
            {
                bool[] has = new bool[3];
                has[0] = has[1] = has[2] = false;
                if (File.Exists(FilePath.log + "log - 1.log"))
                {
                    has[0] = true;
                }
                if (File.Exists(FilePath.log + "log - 2.log"))
                {
                    has[1] = true;
                }
                if (File.Exists(FilePath.log + "log - 3.log"))
                {
                    has[2] = true;
                }
                if (!has[0] && !has[1] && !has[2])
                {
                    File.Move(FilePath.log + "log.log", FilePath.log + "log - 1.log");
                }
                else if (has[0] && !has[1] && !has[2])
                {
                    File.Move(FilePath.log + "log - 1.log", FilePath.log + "log - 2.log");
                    File.Move(FilePath.log + "log.log", FilePath.log + "log - 1.log");
                }
                else if (has[0] && has[1] && !has[2])
                {
                    File.Move(FilePath.log + "log - 2.log", FilePath.log + "log - 3.log");
                    File.Move(FilePath.log + "log - 1.log", FilePath.log + "log - 2.log");
                    File.Move(FilePath.log + "log.log", FilePath.log + "log - 1.log");
                }
                else
                {
                    File.Delete(FilePath.log + "log - 3.log");
                    File.Move(FilePath.log + "log - 2.log", FilePath.log + "log - 3.log");
                    File.Move(FilePath.log + "log - 1.log", FilePath.log + "log - 2.log");
                    File.Move(FilePath.log + "log.log", FilePath.log + "log - 1.log");
                }
            }
            Log.Info("初始化 LOG 成功");

            #endregion
            #region 2 - 窗口基础

            // 开始加载
            Log.Info("开始启动！");
            InitializeComponent();

            #endregion
            #region 3 - 初始化设置

            // 初始化设置
            Log.Info("开始载入设置……");
            if(!Save.SetReader())
            {
                if(Save.SetJson["Error"].ToString() == "设置文件版本不符合")
                {

                }
                else if(Save.SetJson["Error"].ToString() == "未发现设置文件版本")
                {

                }
                else if (Save.SetJson["Error"].ToString() == "读取设置文件错误")
                {

                }
            }

            #endregion

            Log.Info("开始载入主界面……");
            #region 4 - 判断系统主题颜色

            // 判断系统主题颜色
            try
            {
                if (Convert.ToBoolean(Save.SetJson["Exterior"]["Themes"]["DMFromSys"]) == false)
                {
                    if (Convert.ToBoolean(Save.SetJson["Exterior"]["Themes"]["DarkMode"]) == true)
                    {
                        isDarkMode = true;
                    }
                    else
                    {
                        isDarkMode = false;
                    }
                }
                else
                {
                    isDarkMode = SSUserClass.SystemGet.GetDarkMode();
                }
            }
            catch
            {
                Log.Error("未找到主题颜色设置", "提醒");

                isDarkMode = SSUserClass.SystemGet.GetDarkMode();

                JObject Themes = new JObject { { "DarkMode", isDarkMode }, { "DMFromSys", false } };
                JObject Exterior = new JObject();   //外观设置
                Exterior.Add("Themes", Themes);
                Save.SetJson.Add("Exterior", Exterior);
                Save.SetWritter();
            }
            if (isDarkMode)
            {
                Log.Info("系统颜色模式：深色");
                SolidColorBrush brush = new SolidColorBrush();
                SolidColorBrush mainbrush = new SolidColorBrush();
                SolidColorBrush iconbrush = new SolidColorBrush();
                brush.Color = Color.FromArgb(110, 0, 0, 0);
                traBackground.Background = brush;
                mainbrush.Color = Color.FromArgb(255, 0, 0, 0);
                mainBackground.Background = mainbrush;
                iconbrush.Color = Color.FromArgb(255, 255, 255, 255);
                closeicon.Brush = iconbrush;
                smallicon.Brush = iconbrush;
            }
            else
            {
                Log.Info("系统颜色模式：浅色");
            }

            #endregion
            #region 4 - 初始化样式

            Ver.

            #endregion
            #region 4 - 初始化窗口透明
            try
            {
                if (Convert.ToBoolean(Save.SetJson["Exterior"]["Themes"]["Transparent"]) == true)
                {

                }
            }
            catch
            {

            }

            var compositor = new WindowAccentCompositor(this);
            compositor.Composite(Color.FromArgb(200, 255, 255, 255));

            #endregion

            #region 加载主页

            // 加载主页
            PageHome main = new PageHome();
            Page.Content = new Frame()
            {
                Content = main
            };

            #endregion
        }

        #region 基本窗口样式和交互
        private void Onsizechanged(object sender, SizeChangedEventArgs e)
        {
            // 窗口圆角
            System.Windows.Rect r = new System.Windows.Rect(e.NewSize);
            int radius = 5;
            RectangleGeometry gm = new RectangleGeometry(r, radius, radius);
            ((UIElement)sender).Clip = gm;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            // 失去焦点事件
            SolidColorBrush brush = new SolidColorBrush();
            if (isDarkMode)
            {
                brush.Color = Color.FromArgb(230, 31, 31, 31);
            }
            else
            {
                brush.Color = Color.FromArgb(200, 230, 230, 230);
            }
            traBackground.Background = brush;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            // 获得焦点事件
            SolidColorBrush brush = new SolidColorBrush();
            if (isDarkMode)
            {
                brush.Color = Color.FromArgb(110, 0, 0, 0);
            }
            else
            {
                brush.Color = Color.FromArgb(110, 255, 255, 255);
            }
            traBackground.Background = brush;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 关闭按钮
            Application.Current.Shutdown();
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
                Log.Error("鼠标拖拽执行异常！", "崩溃");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // 最小化
            WindowState = WindowState.Minimized;
        }
        #endregion
    }
}
