using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using SS_Tool_Box.Controls;
using SS_Tool_Box.Function;
using SS_Tool_Box.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SS_Tool_Box.Pages.Tools
{
    /// <summary>
    /// N2Tools.xaml 的交互逻辑
    /// </summary>
    public partial class N2Tools : Page
    {
        public static bool hasOpendRoom = false;

        private bool isLogined = false;
        private string apiURL = "https://n2station.live:8443";
        private string version = "1.0.5";

        public N2Tools()
        {
            InitializeComponent();

            viewVersion.Text = version;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            isLogined = false;
            panRoomCard.Children.Clear();
            panRoomLoading.Visibility = Visibility.Visible;
            panRomeError.Visibility = Visibility.Collapsed;
            try
            {
                // 检查是否存在 Token
                if (Features.Reg.IsRegeditKeyExit(Registry.CurrentUser, @"SOFTWARE\SSTeam\SS-Tool-Box", "N2Token"))
                {
                    panLogin.Visibility = Visibility.Collapsed;
                    // 开始验证登录
                    Thread thread = new Thread(runtrustLogin);
                    MainWindow.threads.Push(thread);
                    thread.Start();
                }
                else
                {
                    panRoomLoading.Visibility = Visibility.Collapsed;
                }
            }
            catch { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isLogined)
            {
                // 登录
                if (email.Text == null || password.Password == null)
                {
                    UI.ToastHelper.Show("请输入内容！");
                    return;
                }

                Login.Visibility = Visibility.Collapsed;
                LoginWait.Visibility = Visibility.Visible;
                LoginOut.Visibility = Visibility.Collapsed;

                Button box = (Button)sender;
                box.IsEnabled = false;

                // 开始登录
                Thread thread = new Thread(runLogin);
                MainWindow.threads.Push(thread);
                thread.Start();
            }
            else
            {
                LoginOut.Visibility = Visibility.Collapsed;
                LoginWait.Visibility = Visibility.Visible;
                // 登出
                Thread thread = new Thread(runLoginOut);
                MainWindow.threads.Push(thread);
                thread.Start();
            }
        }

        /// <summary>
        /// 执行登出流程
        /// </summary>
        private void runLoginOut()
        {
            Log.AddLog("N2", "开始登出……");
            DateTime startRun = DateTime.Now;
            string rooms = HttpUitls.Get(apiURL + "/api/auth/destroyToken", "DEFAULT", "Cookie",
                "Authorization=" + Features.Reg.GetRegKey(Registry.CurrentUser, @"SOFTWARE\SSTeam\SS-Tool-Box", "N2Token"));
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                panRoomCard.Children.Clear();
                panRoomLoading.Visibility = Visibility.Collapsed;
                panRomeError.Visibility = Visibility.Collapsed;
                panLoginInouts.Visibility = Visibility.Visible;
                Login.Visibility = Visibility.Visible;
                LoginOut.Visibility = Visibility.Collapsed;
                LoginWait.Visibility = Visibility.Collapsed;
            });
            Features.Reg.DelRegKey(Registry.CurrentUser, @"SOFTWARE\SSTeam\SS-Tool-Box", "N2Token");
            isLogined = false;
            Log.AddLog("N2", "登出完成，耗时：" + DateTime.Now.Subtract(startRun).TotalSeconds + "秒");
        }

        /// <summary>
        /// 执行登录流程
        /// </summary>
        private void runLogin()
        {
            Log.AddLog("N2", "开始登录……");
            DateTime startRun = DateTime.Now;
            // 获取文本
            string email = "";
            string pass = "";
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                email = this.email.Text;
                pass = password.Password;
            });
            while (pass == "") ;
            // 拼接 JSON，请求
            string loginInfo = "{\"email\": \"" + email + "\",\"pass\": \"" + pass + "\"}";
            List<string> back = LoginN2(loginInfo);
            if(back[0] == "Err")
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    Login.Visibility = Visibility.Visible;
                    LoginWait.Visibility = Visibility.Collapsed;

                    LoginButton.IsEnabled = true;
                });
                UI.ToastHelper.Show("登录失败：" + back[2]);
                return;
            }
            if(back[1].IndexOf("Authorization") < 0)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    Login.Visibility = Visibility.Visible;
                    LoginWait.Visibility = Visibility.Collapsed;

                    LoginButton.IsEnabled = true;
                });
                UI.ToastHelper.Show("登录失败：" + JObject.Parse(back[2])["status"]);
                return;

            }
            string token = back[1].Substring(back[1].IndexOf("Authorization") + "Authorization".Length + 1);
            token = token.Substring(0, token.IndexOf(";"));
            if(!Features.Reg.IsRegeditItemExist(Registry.CurrentUser, @"SOFTWARE\SSTeam", @"SS-Tool-Box"))
            {
                if(!Features.Reg.CreateRegItem(Registry.CurrentUser, @"SOFTWARE\SSTeam\SS-Tool-Box"))
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        Login.Visibility = Visibility.Visible;
                        LoginWait.Visibility = Visibility.Collapsed;

                        LoginButton.IsEnabled = true;
                    });
                    UI.ToastHelper.Show("注册表操作失败！");
                    return;
                }
            }
            if(!Features.Reg.CreateRegKey(Registry.CurrentUser, @"SOFTWARE\SSTeam\SS-Tool-Box", "N2Token", token))
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    Login.Visibility = Visibility.Visible;
                    LoginWait.Visibility = Visibility.Collapsed;

                    LoginButton.IsEnabled = true;
                });
                UI.ToastHelper.Show("注册表操作失败！");
                return;
            }
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                Login.Visibility = Visibility.Collapsed;
                LoginWait.Visibility = Visibility.Collapsed;
                LoginOut.Visibility = Visibility.Visible;

                panLoginInouts.Visibility = Visibility.Collapsed;
                panLoginTitle.Text = "N2 Station 账户";

                panRoomLoading.Visibility = Visibility.Visible;

                // 显示房间卡片
                panRoom.Visibility = Visibility.Visible;

                LoginButton.IsEnabled = true;
            });
            UI.ToastHelper.Show("登陆成功！");
            Log.AddLog("N2", "登录完成，耗时：" + DateTime.Now.Subtract(startRun).TotalSeconds + "秒");
            isLogined = true;
            // 开始获取后续数据
            getingStart();
        }

        /// <summary>
        /// 执行验证登陆流程
        /// </summary>
        private void runtrustLogin()
        {
            Log.AddLog("N2", "开始验证登录……");
            DateTime startRun = DateTime.Now;
            try
            {
                // 执行验证登陆（随便请求个需要验证的 API）
                string rooms = HttpUitls.Get(apiURL + "/api/user/rooms", "DEFAULT", "Cookie",
                    "Authorization=" + Features.Reg.GetRegKey(Registry.CurrentUser, @"SOFTWARE\SSTeam\SS-Tool-Box", "N2Token"));
                if (rooms != null && JObject.Parse(rooms)["status"].ToString() == "0")
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                    // 验证登录成功，切换到登录状态
                    isLogined = true;
                        panLogin.Visibility = Visibility.Collapsed;
                        Login.Visibility = Visibility.Collapsed;
                        LoginWait.Visibility = Visibility.Collapsed;
                        panLoginInouts.Visibility = Visibility.Collapsed;

                        panLoginTitle.Text = "N2 Station 账户";
                        LoginOut.Visibility = Visibility.Visible;
                    // 显示房间卡片
                    panRoom.Visibility = Visibility.Visible;
                        panLogin.Visibility = Visibility.Visible;
                        LoginButton.IsEnabled = true;
                    });
                    UI.ToastHelper.Show("验证登录成功！");
                    Log.AddLog("N2", "验证登录完成，耗时：" + DateTime.Now.Subtract(startRun).TotalSeconds + "秒");
                    // 获取后续数据
                    getingStart();
                }
                else
                {
                    UI.ToastHelper.Show("验证登录失败！");
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        panLogin.Visibility = Visibility.Visible;
                    });
                }
            }
            catch(Exception e)
            {
                UI.ToastHelper.Show("验证登录失败：" + e.Message);
                Log.AddErr("验证登录失败：" + e.Message);
            }
        }

        /// <summary>
        /// 获取后续信息
        /// </summary>
        /// <returns></returns>
        private bool getingStart()
        {
            // 开始获取房间信息
            Thread thread = new Thread(runGetRooms);
            MainWindow.threads.Push(thread);
            thread.Start();
            return true;
        }

        /// <summary>
        /// 获取我的房间
        /// </summary>
        private void runGetRooms()
        {
            Log.AddLog("N2", "开始获取房间信息……");
            DateTime startRun = DateTime.Now;
            try
            {
                string rooms = HttpUitls.Get(apiURL + "/api/user/rooms", "DEFAULT", "Cookie",
                    "Authorization=" + Features.Reg.GetRegKey(Registry.CurrentUser, @"SOFTWARE\SSTeam\SS-Tool-Box", "N2Token"));
                JObject obj = JObject.Parse(rooms);
                if(obj["status"].ToString() != "0")
                {
                    panRomeError.Visibility = Visibility.Visible;
                    panRoomErrorText.Text = obj["status"].ToString();
                    return;
                }
                foreach (var item in obj["data"])
                {
                    string json = item.ToString().Substring(item.ToString().IndexOf("{"));
                    JObject itemobj = JObject.Parse(json);
                    List<string> info = new List<string>()
                    {
                        itemobj["_id"].ToString(),
                        itemobj["title"].ToString(),
                        itemobj["desc"].ToString(),
                    };
                    JArray tagArray = JArray.Parse(itemobj["tag"].ToString());
                    List<string> tags = new List<string>();
                    for (int i = 0; i < tagArray.Count; i++)
                    {
                        tags.Add(tagArray[i].ToString());
                    }
                    if(itemobj["status"].ToString() == "open")
                    {
                        hasOpendRoom = true;
                    }
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        N2RoomCard card = new N2RoomCard(info, tags,
                        Features.Reg.GetRegKey(Registry.CurrentUser, @"SOFTWARE\SSTeam\SS-Tool-Box", "N2Token"),
                        itemobj["status"].ToString() == "open" ? true : false);
                        card.Margin = new Thickness(0, 0, 0, 15);
                        panRoomCard.Children.Add(card);
                        panRoomLoading.Visibility = Visibility.Collapsed;
                    });
                }
                Log.AddLog("N2", "获取完成，耗时：" + DateTime.Now.Subtract(startRun).TotalSeconds + "秒");
            }
            catch(Exception e)
            {
                UI.ToastHelper.Show("获取失败：" + e.Message);
                Log.AddErr("获取失败：" + e.Message);
            }
        }

        /// <summary>
        /// 进行请求
        /// </summary>
        /// <param name="info">请求 JSON</param>
        /// <returns>结果列表</returns>
        private List<string> LoginN2(string info)
        {
            try
            {
                string url = apiURL + "/api/auth/getToken";

                byte[] byteArray = Encoding.Default.GetBytes(info); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(url));
                webReq.Method = "POST";
                webReq.ContentType = "application/json";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                // 获取页面输出
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                string ret = sr.ReadToEnd();
                // 获取 Header Cookie
                string cookies = response.Headers["Set-Cookie"];

                return new List<string>() { "OK", cookies, ret };
            }
            catch (Exception e)
            {
                return new List<String>() { "Err", e.ToString(), e.Message };
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            N2MessageView view = new N2MessageView();
            view.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            view.Show();
        }
    }
}
