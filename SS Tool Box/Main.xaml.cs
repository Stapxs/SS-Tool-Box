using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Panuon.UI.Silver;
using SS_Tool_Box;
using System.Runtime;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data;
using Windows.Foundation;
using Windows.UI;
using SS_Tool_Box.Classes;
using System.IO;
using Newtonsoft.Json.Linq;

namespace SS_Tool_Box_By_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>

    public partial class Main : WindowX
    {
        public static BaseColor baseColor = new BaseColor();
        public static JObject Settings;

        int NowPage = 0;
        public static int NowChoice = 0;
        public int WindowNew;
        Error error = new Error();
        DateTime loadingtime;

        public class Update
        {
            public string Line { get; set; }
        }


        //工具列表
        Update[] listall = new Update[]
        {
                new Update(){Line="    1 . 批量打开文件"},
                new Update(){Line="    2 . 调色板"},
                new Update(){Line="    3 . 快速启动器"},
                new Update(){Line="    4 . 倒计时"},
                new Update(){Line="    5 . Fuck QQ"},
        };
        //隐藏的工具列表
        Update[] listallHidden = new Update[]
        {
                new Update(){Line="    * . SOS 图标获取"},
                new Update(){Line="    回到主页"},
        };

        public Main()
        {
            //初始化颜色
            if(!LoadingMain())
            {
                Application.Current.Shutdown();
            }

            //写入log日期
            if (!Directory.Exists("SSTB/Log"))
            {
                Directory.CreateDirectory("SSTB/Log");
            }
            if (File.Exists("SSTB/Log/log.log"))
            {
                String line = "";
                try
                {
                    using (StreamReader sr = new StreamReader("SSTB/Log/log.log"))
                    {
                        line = sr.ReadLine();
                        sr.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("读取Log文件错误：" + ex);
                }
                try
                {
                    while (line != null)
                    {
                        File.Move("SSTB/Log/log.log", "SSTB/Log/log_" + line + ".log");
                    }
                }
                catch
                {

                }
            }
            try
            {
                using (StreamWriter sw = new StreamWriter("SSTB/Log/log.log"))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy_MM_dd") + DateTime.Now.ToString("_HH_ss"));
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("写入Log文件错误：" + ex);
            }

            error.logWriter("开始加载UI", false);
            loadingtime = DateTime.Now;

            //初始化窗口
            //添加程序集解析事件  
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            //版本号
            String stVersion = "Version - 1.0.8";
            Versionon.Text = stVersion;

            //刷新工具列表
            UpdateList.ItemsSource = listall;

            //工具列表样式
            this.UpdateList.Foreground = baseColor.Fg;
            this.UpdateList.FontFamily = baseColor.Fonts;
            this.UpdateList.FontSize = 14;

            //版本号样式
            this.Versionon.Foreground = baseColor.Fg;
            this.Versionon.FontFamily = baseColor.Fonts;
            this.Versionon.FontSize = 12;

            //按钮样式
            this.Feedback.Foreground = baseColor.FontM;
            this.Feedback.FontFamily = baseColor.Fonts;
            this.Feedback.FontSize = 14;
            this.OpenButton.Foreground = baseColor.FontM;
            this.OpenButton.Background = baseColor.Main;
            this.B1.Foreground = baseColor.Fg;
            this.B2.Foreground = baseColor.Fg;
            this.TopIcon.Background = baseColor.Main;
            ButtonHelper.SetHoverBrush(TopIcon, baseColor.Main);
            ButtonHelper.SetClickCoverOpacity(TopIcon, 1);

            //初始化卡片颜色
            WindowXCaption.SetBackground(this, baseColor.Main);
            WindowXCaption.SetForeground(this, baseColor.FontM);

            CD1.Background = baseColor.Card;
            CD2.Background = baseColor.Card;
            SH1.Background = baseColor.Card;

            SH1.Foreground = baseColor.Font;
            UpdateList.Background = baseColor.Tran;
            Versionon.Foreground = baseColor.Fg;
            UpdateList.ContextMenu.Background = baseColor.Card;
            UpdateList.ContextMenu.Foreground = baseColor.Fg;
            ContextMenuHelper.SetShadowColor(UpdateList.ContextMenu, baseColor.DBg.Color);

            BG.BeginInit();
            BG.Source = baseColor.Bgp;
            BG.EndInit();
            SBG.BeginInit();
            SBG.Source = baseColor.Bgps;
            SBG.EndInit();

            //加载主页
            PageMain main = new PageMain();
            Page.Content = new Frame()
            {
                Content = main
            };

            //加载完成
            error.logWriter("UI加载完成，耗时：" + (DateTime.Now - loadingtime).ToString(), false);
            ButtonHelper.SetIsWaiting(OpenButton, false);
        }

        private void ListMenu_Open(object sebder, RoutedEventArgs s)
        {
            Update upd = UpdateList.SelectedItem as Update;
            if (upd != null && upd is Update)
            {
                WindowState = WindowState.Minimized;
                if (upd.Line == "    1 . 批量打开文件")
                {
                    NowChoice = 1;
                }
                else if (upd.Line == "    2 . 调色板")
                {
                    NowChoice = 2;
                }
                //else if (upd.Line == "    3 . 快速启动器")
                //{
                //    NowChoice = 3;
                //}
                else if (upd.Line == "    4 . 倒计时")
                {
                    NowChoice = 4;
                }
                else if (upd.Line == "    5 . Fuck QQ")
                {
                    NowChoice = 5;
                }
                else if (upd.Line == "    * . SOS 图标获取")
                {
                    NowChoice = -1;
                }
                else if (upd.Line == "    回到主页")
                {
                    NowChoice = -2;
                }
                else
                {
                    NowChoice = -999;
                }
                //打开窗口OpenInWindow
                error.logWriter("加载工具：" + upd.Line, false);
                OpennInWindow openInWindow = new OpennInWindow();
                openInWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                openInWindow.ShowDialog();
                WindowState = WindowState.Normal;
            }
        }

        private void Button_Open(object sender, RoutedEventArgs e)
        {
            Update upd = UpdateList.SelectedItem as Update;
            error.logWriter("加载工具：" + upd.Line, false);
            if (upd != null && upd is Update)
            {
                if (upd.Line == "    1 . 批量打开文件")
                {
                    NowPage = 1;
                    Page1 page1 = new Page1();
                    Page.Content = new Frame()
                    {
                        Content = page1
                    };
                }
                else if (upd.Line == "    2 . 调色板")
                {
                    NowPage = 2;
                    Page2 page2 = new Page2();
                    Page.Content = new Frame()
                    {
                        Content = page2
                    };
                }
                /*
                else if (upd.Line == "    3 . 快速启动器")
                {
                    NowPage = 3;
                    Page3 page3 = new Page3();
                    Page.Content = new Frame()
                    {
                        Content = page3
                    };
                }
                */
                else if (upd.Line == "    4 . 倒计时")
                {
                    NowPage = 4;
                    Page4 page4 = new Page4();
                    Page.Content = new Frame()
                    {
                        Content = page4
                    };
                }
                else if (upd.Line == "    5 . Fuck QQ")
                {
                    NowPage = 5;
                    Page5 page5 = new Page5();
                    Page.Content = new Frame()
                    {
                        Content = page5
                    };
                }
                else if (upd.Line == "    * . SOS 图标获取")
                {
                    NowPage = -1;
                    PageHD1 pageHD1 = new PageHD1();
                    Page.Content = new Frame()
                    {
                        Content = pageHD1
                    };
                }
                else if (upd.Line == "    回到主页")
                {
                    NowPage = -2;
                    PageMain pageMain = new PageMain();
                    Page.Content = new Frame()
                    {
                        Content = pageMain
                    };
                }
                else
                {
                    NowPage = -1000;
                    PageNone pageNone = new PageNone();
                    Page.Content = new Frame()
                    {
                        Content = pageNone
                    };
                }
            }
        }

        private void Button_About(object sender, RoutedEventArgs e)
        {
            //打开窗口About
            About about = new About();
            about.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            about.Owner = this;
            IsMaskVisible = true;
            about.ShowDialog();
            IsMaskVisible = false;
        }

        private void Button_Set(object sender, RoutedEventArgs e)
        {
            //打开窗口About
            Settings about = new Settings();
            about.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            about.Owner = this;
            IsMaskVisible = true;
            about.ShowDialog();
            IsMaskVisible = false;
        }

        private void SearchBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            int nowget = 0;
            Update[] listallGet = new Update[10];
            Update[] listallnull = new Update[]
            {
                new Update(){Line=" "},
            };
            bool get = false;

            if (e.Key != Key.Enter)
            {
                return;
            }
            
            if(String.IsNullOrWhiteSpace(SH1.Text))
            {
                //刷新工具列表
                UpdateList.ItemsSource = listall;
                return;
            }
            for(int i=0; i< listall.Length; i++)
            {
                if (listall[i].Line.IndexOf(SH1.Text) != -1)
                {
                    get = true;
                    listallGet[nowget] = listall[i];
                    nowget++;
                }
            }
            for (int i = 0; i < listallHidden.Length; i++)
            {
                if (listallHidden[i].Line.IndexOf(SH1.Text) != -1)
                {
                    get = true;
                    listallGet[nowget] = listallHidden[i];
                    nowget++;
                }
            }
            Update[] listallGetEnd = new Update[nowget];
            for(int i=0; i<nowget; i++)
            {
                listallGetEnd[i] = listallGet[i];
            }
            if (!get)
            {
                listallGet = listallnull;
            }
            //刷新工具列表
            UpdateList.ItemsSource = listallGetEnd;
        }

        private bool LoadingMain()
        {
            LoadingSetter Set = new LoadingSetter();
            if (Set.fistUsed())
            {
                Set.newSetup();
            }
            else
            {
                Settings = Set.ReadSetup();
            }
            try
            {
                baseColor.setColor(int.Parse(Settings["Exterior"]["Themes"]["MainTheme"].ToString()), bool.Parse(Settings["Exterior"]["Themes"]["DarkMode"].ToString()));
            }
            catch(Exception ex)
            {
                MessageBox.Show("[ 崩溃 ] 初始化数据错误：" + ex);
                error.logWriter("[ 崩溃 ] 初始化数据错误：" + ex, false);
                return false;
            }
            return true;
        }
    }
}
