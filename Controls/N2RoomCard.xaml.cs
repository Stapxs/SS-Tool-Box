using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using SS_Tool_Box.Function;
using SS_Tool_Box.Pages.Tools;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SS_Tool_Box.Controls
{
    /// <summary>
    /// N2RoomCard.xaml 的交互逻辑
    /// </summary>
    public partial class N2RoomCard : UserControl
    {
        public N2RoomCard()
        {
            InitializeComponent();

            warntext.Text = "此控件不支持无参调用，请传递正确的参数创建";
        }

        private bool _isOpen = false;
        private string apiURL = "https://n2station.live:8443";
        private string stopIcon = "M400 32H48C21.5 32 0 53.5 0 80v352c0 26.5 21.5 48 48 48h352c26.5 0 48-21.5 48-48V80c0-26.5-21.5-48-48-48z";
        private string runIcon = "M424.4 214.7L72.4 6.6C43.8-10.3 0 6.1 0 47.9V464c0 37.5 40.7 60.1 72.4 41.3l352-208c31.4-18.5 31.5-64.1 0-82.6z";
        SolidColorBrush openBg = new SolidColorBrush(Color.FromArgb(255, 255, 84, 57));
        SolidColorBrush closeBg = new SolidColorBrush(Color.FromArgb(255, 76, 110, 245));

        public N2RoomCard(List<string> info, List<string> tags, string token, bool isOpen)
        {
            InitializeComponent();

            warning.Visibility = Visibility.Collapsed;

            id.Text = info[0];
            title.Text = info[1];
            subtitle.Text = info[2];

            foreach (string tag in tags)
            {
                TextBlock text = new TextBlock();
                text.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                text.Text = tag;
                text.Background = new SolidColorBrush(Color.FromArgb(255, 76, 110, 245));
                text.FontSize = 10;
                text.Margin = new Thickness(15, 5, 15, 5);

                Button button = new Button();
                button.SetResourceReference(Button.StyleProperty, "TextButtonEx");
                button.Background = new SolidColorBrush(Color.FromArgb(255, 76, 110, 245));
                button.Margin = new Thickness(0, 0, 3, 3);
                button.Height = 25;

                button.Content = text;

                tagView.Children.Add(button);
            }

            _isOpen = isOpen;

            if(isOpen)
            {
                opentext.Text = "关闭房间";
                openbutton.Background = openBg;
                icon.Data = Geometry.Parse(stopIcon);
            }
        }

        private void openbutton_Click(object sender, RoutedEventArgs e)
        {
            // 房间切换事件
            if(_isOpen)
            {
                // 关闭房间
                change.Visibility = Visibility.Collapsed;
                wait.Visibility = Visibility.Visible;

                Thread thread = new Thread(runChange);
                MainWindow.threads.Push(thread);
                thread.Start();
            }
            else
            {
                // 打开房间
                if (!N2Tools.hasOpendRoom)
                {
                    opentext.Text = "关闭房间";

                    change.Visibility = Visibility.Collapsed;
                    wait.Visibility = Visibility.Visible;

                    Thread thread = new Thread(runChange);
                    MainWindow.threads.Push(thread);
                    thread.Start();
                }
                else
                {
                    // 显示提示
                    MainWindow.main.MsgAdd(new List<string> { "N2 Station Tools", "请先关闭已打开的房间！" },
                        new List<string> { "知道了" }, null);
                }
            }
        }

        /// <summary>
        /// 切换房间状态
        /// </summary>
        private void runChange()
        {
            DateTime startRun = DateTime.Now;
            try
            {
                string id = "";
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    id = this.id.Text;
                });
                while (id == "") ;
                // 写个 JB 睡觉
                if (_isOpen)
                {
                    Log.AddLog("N2", "正在修改房间状态……（关闭）");
                    // 关
                    string back = HttpUitls.Post(apiURL + "/api/user/closeRoom", "{ \"id\": \"" + id + "\" }", "DEFAULT", "Cookie",
                        "Authorization=" + Features.Reg.GetRegKey(Registry.CurrentUser, @"SOFTWARE\SSTeam\SS-Tool-Box", "N2Token"));
                    if (JObject.Parse(back)["status"].ToString() == "0")
                    {
                        _isOpen = false;
                        N2Tools.hasOpendRoom = false;
                        Log.AddLog("N2", "切换完成，耗时：" + DateTime.Now.Subtract(startRun).TotalSeconds + "秒");
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            change.Visibility = Visibility.Visible;
                            wait.Visibility = Visibility.Collapsed;
                            opentext.Text = "开启房间";
                            openbutton.Background = closeBg;
                            icon.Data = Geometry.Parse(runIcon);
                        });
                        return;
                    }
                    else
                    {
                        UI.ToastHelper.Add("处理失败：" + JObject.Parse(back)["status"].ToString());
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            change.Visibility = Visibility.Visible;
                            wait.Visibility = Visibility.Collapsed;
                        });
                        return;
                    }
                }
                else
                {
                    Log.AddLog("N2", "正在修改房间状态……（打开）");
                    // 开
                    string back = HttpUitls.Post(apiURL + "/api/user/openRoom", "{ \"id\": \"" + id + "\" }", "DEFAULT", "Cookie",
                        "Authorization=" + Features.Reg.GetRegKey(Registry.CurrentUser, @"SOFTWARE\SSTeam\SS-Tool-Box", "N2Token"));
                    if (JObject.Parse(back)["status"].ToString() == "0")
                    {
                        _isOpen = true;
                        N2Tools.hasOpendRoom = true;
                        Log.AddLog("N2", "切换完成，耗时：" + DateTime.Now.Subtract(startRun).TotalSeconds + "秒");
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            change.Visibility = Visibility.Visible;
                            wait.Visibility = Visibility.Collapsed;
                            opentext.Text = "关闭房间";
                            openbutton.Background = openBg;
                            icon.Data = Geometry.Parse(stopIcon);

                            N2Tools.openRoomID = this.id.Text;
                        });
                        return;
                    }
                    else
                    {
                        UI.ToastHelper.Add("处理失败：" + JObject.Parse(back)["status"].ToString());
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            change.Visibility = Visibility.Visible;
                            wait.Visibility = Visibility.Collapsed;
                        });
                        return;
                    }
                }
            }
            catch(Exception e)
            {
                UI.ToastHelper.Add("处理失败：" + e.Message);
                Log.AddErr("N2", "切换房间状态失败：" + e.ToString());
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    change.Visibility = Visibility.Visible;
                    wait.Visibility = Visibility.Collapsed;
                });
            }
        }
    }
}
