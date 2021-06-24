using Microsoft.Win32;
using SS_Tool_Box.Function;
using SS_Tool_Box.Pages.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WebSocketSharp;
using WebSocketSharp.Net;

namespace SS_Tool_Box.Windows
{
    /// <summary>
    /// N2MessageView.xaml 的交互逻辑
    /// </summary>
    public partial class N2MessageView : Window
    {
        public static N2MessageView main = null;

        public N2Tools toolPage { get; set; }
        public string roomID { get; set; }
        public bool isExit { get; set; }

        private bool connected = false;
        private WebSocket ws = null;

        public N2MessageView()
        {
            InitializeComponent();

            isExit = false;
            main = this;

            MainWindow.main.WindowState = WindowState.Minimized;


            if (N2Tools.isYKLMH)
            {
                WindowBlur.SetIsEnabled(this, true);
            }

            // 开始创建 WebSocket
            Thread thread = new Thread(runMain);
            MainWindow.threads.Push(thread);
            thread.Start();

            // 置顶
            Topmost = true;

            // 设置窗口大小
            Height = SystemParameters.WorkArea.Height;
            Width = SystemParameters.WorkArea.Width * 0.15;

            msgList.Width = SystemParameters.WorkArea.Width * 0.15 - 30;

            // 移动位置
            Left = SystemParameters.WorkArea.Width - SystemParameters.WorkArea.Width * 0.15;
            Top = 0;
        }

        #region 事件 | 窗口事件

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            isExit = true;
            MainWindow.main.WindowState = WindowState.Normal;
            this.Close();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isExit = true;
            MainWindow.main.WindowState = WindowState.Normal;
            toolPage.viewerLoading = true;
            toolPage.panViewerSet.Visibility = Visibility.Collapsed;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            // 处理窗口字体颜色
            Thread threadc = new Thread(runForColor);
            MainWindow.threads.Push(threadc);
            // threadc.Start();
        }

        private void Title_Click(object sender, RoutedEventArgs e)
        {
            if(!connected && ws != null)
            {
                ws.Connect();
                MainTitle.Text = "正在重连";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (msgBox.Text != null && msgBox.Text != "")
            {
                if (msgBox.Text.Substring(0, 1) != "/")
                {
                    ws.Send("message " + msgBox.Text);
                }
                else
                {
                    string[] command = msgBox.Text.Substring(1).Split(' ');
                    switch(command[0])
                    {
                        case "ws":
                            {
                                switch(command[1])
                                {
                                    case "disconnect": ws.Close(); break;
                                    case "connect": ws.Connect(); break;
                                }
                                break;
                            }
                    }
                }
            }
            msgBox.Text = "";
        }

        #endregion

        #region 控件 | 添加控件

        private void MsgAddSystem(string msg, System.Windows.Media.SolidColorBrush background)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                ListBoxItem item = new ListBoxItem();
                item.Margin = new Thickness(0, 0, 0, 10);
                item.SetResourceReference(StyleProperty, "ItemWithoutAll");

                Button button = new Button();
                button.Height = 30;
                if (background == null)
                {
                    button.SetResourceReference(BackgroundProperty, "colorCard");
                }
                else
                {
                    button.Background = background;
                }
                button.SetResourceReference(StyleProperty, "SysMsgAdm");
                button.Visibility = Visibility.Collapsed;

                TextBlock textBlock = new TextBlock();
                textBlock.SetResourceReference(ForegroundProperty, "colorFont");
                textBlock.Text = msg;
                textBlock.FontSize = 12;
                textBlock.Padding = new Thickness(15, 0, 15, 0);
                textBlock.Width = msgList.Width - 15;
                textBlock.TextAlignment = TextAlignment.Center;

                button.Content = textBlock;
                item.Content = button;

                msgList.Items.Add(item);

                button.Visibility = Visibility.Visible;
            });
        }

        private void msgAddChat(string user, string msg)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                ListBoxItem item = new ListBoxItem();
                item.Margin = new Thickness(0, 0, 0, 10);
                item.Width = msgList.Width - 5;
                item.SetResourceReference(StyleProperty, "ItemWithoutAll");

                StackPanel stack = new StackPanel();
                stack.Orientation = Orientation.Horizontal;

                Button border = new Button();
                border.SetResourceReference(BackgroundProperty, "colorCard");
                border.Width = border.Height = 10;
                border.SetResourceReference(StyleProperty, "ChatMsgDot");
                border.VerticalAlignment = VerticalAlignment.Top;
                border.Margin = new Thickness(3, 12, 0, 0);

                Button button = new Button();
                button.SetResourceReference(StyleProperty, "ChatMsgText");
                Grid grid = new Grid();

                TextBlock says = new TextBlock();
                says.SetResourceReference(ForegroundProperty, "colorFont");
                says.Width = msgList.Width - 35;
                says.Text = user + "：" + msg;
                says.FontSize = 12;
                says.TextWrapping = TextWrapping.Wrap;
                says.Margin = new Thickness(10, 8, 0, 0);

                grid.Children.Add(says);
                button.Content = grid;

                stack.Children.Add(border);
                stack.Children.Add(button);

                item.Content = stack;

                msgList.Items.Add(item);
            });
        }

        #endregion

        class MessageVer
        {
            public MessageVer(string type, string user, string msg, string isSys)
            {
                Type = type;
                User = user;
                Msg = msg;
                IsSys = isSys;
            }

            public string Type { get; }
            public string User { get; }
            public string Msg { get; }
            public string IsSys { get; }
        }

        /// <summary>
        /// WebSocket 主流程
        /// </summary>
        private void runMain()
        {
            if (roomID != null && roomID != "")
            {
                using (ws = new WebSocket("wss://n2station.live:8443/api/chat/" + roomID))
                {
                    ws.OnOpen += (sender, e) => {
                        Log.AddLog("N2Viewer", "WebSocket 连接已连接");
                        connected = true;
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            Title.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 76, 110, 245));
                            MainTitle.Text = "连接成功";
                        });
                        };
                    ws.OnClose += (sender, e) => {
                        Log.AddLog("N2Viewer", "WebSocket 连接已断开");
                        connected = false;
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            Title.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 84, 57));
                            MainTitle.Text = "点击重连";
                            MsgAddSystem("连接意外断开", new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 84, 57)));

                        });
                    };
                    ws.OnMessage += (sender, e) =>
                    {
                        if (e.IsText)
                        {
                            ChangeMessage(e.Data);
                        }
                    };
                    ws.SetCookie(new Cookie("Authorization", new Reg().GetRegKey(Registry.CurrentUser, @"SOFTWARE\SSTeam\SS-Tool-Box", "N2Token")));
                    ws.Connect();
                    while (!isExit)
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            Topmost = true;
                        });
                        ws.Send("PING_PACK");
                        Thread.Sleep(5000);
                    }
                }
            }
        }

        /// <summary>
        /// 序列化消息
        /// </summary>
        /// <param name="msg"></param>
        private void ChangeMessage(string msg)
        {
            if(msg == "" || msg == null)
            {
                Log.AddLog("N2Viewer", "空消息！");
                return;
            }
            List<string> baseMsg = new List<string>();
            if (msg.IndexOf(";") >= 0)
            {
                baseMsg = new List<string>
                {
                    msg.Substring(0, msg.IndexOf(";")),
                    msg.Substring(msg.IndexOf(";") + 1)
                };
                
            }
            else
            {
                baseMsg = new List<string>
                {
                    msg,
                    ""
                };
            }
            baseMsg = new List<string>
            {
                baseMsg[0].Split(' ')[0],
                baseMsg[0].Split(' ')[1],
                baseMsg[1],
                "false"
            };
            if (baseMsg[1] == "0")
            {
                baseMsg[3] = "true";
            }
            MessageVer msgInfo = new MessageVer(
                baseMsg[0],
                baseMsg[1],
                baseMsg[2],
                baseMsg[3]
                );
            AddMessage(msgInfo, msg);
        }

        /// <summary>
        /// 添加消息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        private void AddMessage(MessageVer msg, string baseMsg)
        {
            switch (msg.Type)
            {
                case "chat":
                    {
                        msgAddChat(msg.User, msg.Msg);
                        break;
                    }
                case "auth":
                    {
                        MsgAddSystem("验证登录成功", null);
                        break;
                    }
                case "members":
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            Members.Visibility = Visibility.Visible;
                            MembersText.Text = (int.Parse(msg.User) - 1).ToString();
                        });
                        break;
                    }
                case "join":
                    {
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                        {
                            if (int.Parse(msg.Msg) >= int.Parse(MembersText.Text))
                            {
                                MsgAddSystem(msg.User + " 进入了房间", null);
                            }
                            else if(int.Parse(msg.Msg) < int.Parse(MembersText.Text))
                            {
                                MsgAddSystem(msg.User + " 退出了房间", null);
                            }
                            MembersText.Text = msg.Msg;
                        });
                        break;
                    }
                default:
                    {
                        Log.AddLog("N2Viewer", "未知消息类型：" + baseMsg);
                        break;
                    }
            }
        }

        /// <summary>
        /// 添加消息（不拆分）
        /// </summary>
        /// <param name="msg"></param>
        private void AddMessage(string msg)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                ListBoxItem item = new ListBoxItem();
                TextBlock text = new TextBlock();
                text.Text =  msg;
                item.Content = text;
                msgList.Items.Add(item);
            });
        }

        /// <summary>
        /// 透明模式字体颜色进程
        /// </summary>
        private void runForColor()
        {
            while (!isExit)
            {
                // 获取全屏截图
                bool finished = false;
                Bitmap map = null;
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    // WindowState = WindowState.Minimized;
                    map = new UI.Cutshot().getScreen();
                    // WindowState = WindowState.Normal;
                    // 裁剪窗口位置
                    map = new UI.ImageChange().CutPic(map,
                                         (int)PointToScreen(new System.Windows.Point(0, 0)).X,
                                         (int)PointToScreen(new System.Windows.Point(0, 0)).Y,
                                         (int)Width, (int)Height);
                    finished = true;
                });

                // 等待截图
                while (!finished) ;


                //计算平均亮度
                var sum = 0d;
                for (int h = 0; h < map.Height; h++)
                {
                    for (int w = 0; w < map.Width; w++)
                    {
                        var hue = map.GetPixel(w, h).GetBrightness();
                        sum += hue;
                    }
                }
                var avg = sum / (map.Width * map.Height);

                // 如果平均亮度在 [0, 0.6]   就用白色字体
                // 如果平均亮度在 (0.6, 1]   就用黑色字体


                //Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                //{
                //    if (avg >= 0.6)
                //        Demo.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 0));
                //    else
                //        Demo.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
                //});

                Thread.Sleep(1500);
            }
        }
    }
}
