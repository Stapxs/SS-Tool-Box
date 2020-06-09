using MCServerPing;
using Newtonsoft.Json.Linq;
using Panuon.UI.Silver;
using SS_Tool_Box.Classes;
using SS_Tool_Box_By_WPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SS_Tool_Box
{
    /// <summary>
    /// Page8.xaml 的交互逻辑
    /// </summary>
    public partial class Page8 : Page
    {

        Error error = new Error();
        BaseColor baseColora = Main.baseColor;

        public Main ParentWindow { get; set; }

        public Page8()
        {
            InitializeComponent();

            this.Height = 477;

            String stTitle = "Minecraft 服务器信息查询（Bata）";
            this.Title.Foreground = baseColora.Fg;
            this.Title.FontFamily = baseColora.Fonts;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            String stSays = "一个查询 Minecraft 服务器信息的工具。";
            this.Says.Foreground = baseColora.Fg;
            this.Says.FontFamily = baseColora.Fonts;
            this.Says.FontSize = 14;
            Says.Text = stSays;

            this.MT1.Foreground = baseColora.Fg;
            this.MT1.FontFamily = baseColora.Fonts;
            this.MT1.FontSize = 15;
            this.MT2.Foreground = baseColora.Fg;
            this.MT2.FontFamily = baseColora.Fonts;
            this.MT2.FontSize = 15;
            this.MT3.Foreground = baseColora.Fg;
            this.MT3.FontFamily = baseColora.Fonts;
            this.MT3.FontSize = 15;

            this.T1.Foreground = baseColora.Fg;
            this.T1.FontFamily = baseColora.Fonts;
            this.T1.FontSize = 13;
            this.T2.Foreground = baseColora.Fg;
            this.T2.FontFamily = baseColora.Fonts;
            this.T2.FontSize = 13;
            this.T3.Foreground = baseColora.Fg;
            this.T3.FontFamily = baseColora.Fonts;
            this.T3.FontSize = 13;
            this.T4.Foreground = baseColora.Fg;
            this.T4.FontFamily = baseColora.Fonts;
            this.T4.FontSize = 13;
            this.T5.Foreground = baseColora.Fg;
            this.T5.FontFamily = baseColora.Fonts;
            this.T5.FontSize = 13;
            this.T6.Foreground = baseColora.Fg;
            this.T6.FontFamily = baseColora.Fonts;
            this.T6.FontSize = 13;
            this.T7.Foreground = baseColora.Fg;
            this.T7.FontFamily = baseColora.Fonts;
            this.T7.FontSize = 13;
            this.T8.Foreground = baseColora.Fg;
            this.T8.FontFamily = baseColora.Fonts;
            this.T8.FontSize = 13;
            this.T9.Foreground = baseColora.Fg;
            this.T9.FontFamily = baseColora.Fonts;
            this.T9.FontSize = 13;

            this.Tips.Foreground = baseColora.Fg;
            this.Tips.FontFamily = baseColora.Fonts;
            this.Tips.FontSize = 13;

            this.Load.Foreground = baseColora.Fg;

            this.CD1.Background = baseColora.Card;
            this.CD2.Background = baseColora.Card;
            this.CD3.Background = baseColora.Card;
            this.CD4.Background = baseColora.Card;
            this.CD5.Background = baseColora.Card;
            this.CD6.Background = baseColora.Card;

            this.RunButton.Foreground = baseColora.Fg;
            this.ClearButton.Foreground = baseColora.Fg;
            this.RunButton.Background = baseColora.Tran;
            this.ClearButton.Background = baseColora.Tran;

            this.RunCard.Visibility = Visibility.Collapsed;
            this.Errorsay.Visibility = Visibility.Collapsed;

            this.IP.Background = baseColora.Bg;
            this.PORT.Background = baseColora.Bg;
            this.IP.Foreground = baseColora.Fg;
            this.PORT.Foreground = baseColora.Fg;

            this.Percent.Background = baseColora.DBg;
        }

        private void RunTool(object sender, RoutedEventArgs e)
        {
            Maina.Visibility = Visibility.Visible;

            CD3.Visibility = Visibility.Visible;
            CD4.Visibility = Visibility.Collapsed;
            CD5.Visibility = Visibility.Collapsed;

            T7.Visibility = Visibility.Visible;
            PlayerList.Visibility = Visibility.Visible;

            ButtonHelper.SetIsWaiting(RunButton, true);
            Thread thread = new Thread(GetInfo);
            thread.Start();
        }

        private void GetInfo()
        {
            //开始获取
            string ip = null;
            string post = null;
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                ip = IP.Text;
                post = PORT.Text;
            }), DispatcherPriority.SystemIdle, null);
            //等待从UI获取数据完成
            while (ip == null || post == null) ;
            //域名解析
            try
            {
                IPAddress[] ipadd = Dns.GetHostAddresses(ip);
                for(int i = 0; i<= ipadd.Count() - 1; i++)
                {
                    ip = ipadd[i].ToString();
                }
            } catch(Exception ex)
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    T4.Text = "ERR - 域名解析错误。" + ex;
                }), DispatcherPriority.SystemIdle, null);

                MessageBox.Show(ex.ToString());
                return;
            }
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                T4.Text = "正在获取……";
            }), DispatcherPriority.SystemIdle, null);
            String ping = ServerPing.GetSeverInfo(ip, Convert.ToInt32(post));
            if (!string.IsNullOrWhiteSpace(ping) && ping.IndexOf("ERR") < 0)
            {
                JObject info = new JObject();
                try
                {
                    info = JObject.Parse(ping);
                }
                catch
                {
                    goto END;
                }
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    T4.Text = "正在分析……";
                }), DispatcherPriority.SystemIdle, null);
                #if DEBUG
                //写入文本
                using (StreamWriter sw = new StreamWriter("SSTB/test.txt"))
                {
                    sw.WriteLine(info.ToString());
                    sw.Close();
                }
#endif
                try
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        CD3.Visibility = Visibility.Collapsed;
                    }), DispatcherPriority.SystemIdle, null);

                    //解析图片
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        if (ping.IndexOf("favicon") >= 0)
                        {
                            string base64 = info["favicon"].ToString();
                            string imagebase64 = base64.Substring(base64.IndexOf(",") + 1);
                            byte[] streamBase = Convert.FromBase64String(imagebase64);
                            BitmapImage bi = new BitmapImage();
                            bi.BeginInit();
                            bi.StreamSource = new MemoryStream(streamBase);
                            bi.EndInit();
                            Hd.Source = bi;
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                CD4.Visibility = Visibility.Visible;
                            }), DispatcherPriority.SystemIdle, null);
                        }
                        else
                        {
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                CD4.Visibility = Visibility.Collapsed;
                            }), DispatcherPriority.SystemIdle, null);
                        }
                    }));
                    //服务器名
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            CD5.Visibility = Visibility.Visible;
                        }), DispatcherPriority.SystemIdle, null);
                        try
                        {
                            T5.Text = "显示标题：" + info["description"]["translate"].ToString();
                        }
                        catch
                        {
                            try
                            {
                                string namestr = "";
                                int i = ping.IndexOf("extra");       //标题列表位置
                                if (i >= 0)
                                {
                                    string extra = ping;
                                    extra = extra.Substring(i);
                                    i = extra.IndexOf("[");
                                    extra = extra.Substring(i + 1);
                                    i = extra.IndexOf("]");
                                    extra = extra.Substring(0, i);
                                    i = extra.IndexOf("\"text\":\"");
                                    while (i >= 0)
                                    {
                                        extra = extra.Substring(i + "\"text\":\"".Length);
                                        i = extra.IndexOf("\"");
                                        string str = extra;
                                        str = str.Substring(0, i);
                                        namestr += str;
                                        i = extra.IndexOf("\"text\":\"");
                                    }
                                    T5.Text = "显示标题：" + namestr;
                                }
                            }
                            catch
                            {

                            }
                        }
                    }));
                    //游戏版本
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        T8.Text = "游戏版本：" + info["version"]["name"].ToString() + " (" + info["version"]["protocol"].ToString() + ") ";
                    }), DispatcherPriority.SystemIdle, null);

                    //玩家数量
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        T6.Text = "在线玩家：" + info["players"]["online"].ToString() + " / " + info["players"]["max"].ToString();
                    }), DispatcherPriority.SystemIdle, null);
                    //服务器玩家列表
                    int j = ping.IndexOf("sample");       //玩家列表位置
                    if (j >= 0)
                    {
                        string sample = ping;
                        sample = sample.Substring(j);
                        j = sample.IndexOf("[");
                        sample = sample.Substring(j + 1);
                        j = sample.IndexOf("]");
                        sample = sample.Substring(0, j);
                        j = sample.IndexOf("\"name\":\"");
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            while (j >= 0)
                            {
                                sample = sample.Substring(j + "\"name\":\"".Length);
                                j = sample.IndexOf("\"");
                                string str = sample;
                                str = str.Substring(0, j);

                                /*
                                /* TextBlock
                                * HorizontalAlignment="Left"
                                * Margin="44,0,10,5"
                                * MaxWidth="540"
                                */
                                TextBlock textBlock = new TextBlock();
                                textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                                textBlock.Margin = new Thickness(0, 0, 0, 5);
                                textBlock.MaxWidth = 540;
                                textBlock.Foreground = baseColora.Font;
                                textBlock.FontSize = 13;
                                textBlock.FontFamily = baseColora.Fonts;
                                textBlock.Text = str;
                                PlayerList.Children.Add(textBlock);

                                j = sample.IndexOf("\"name\":\"");
                            }
                        }), DispatcherPriority.SystemIdle, null);
                    }
                    else
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            T7.Visibility = Visibility.Collapsed;
                            PlayerList.Visibility = Visibility.Collapsed;
                        }), DispatcherPriority.SystemIdle, null);
                    }
                    //Mod列表
                    int k = ping.IndexOf("modList");       //MOD列表位置
                    if (k >= 0)
                    {
                        string modList = ping;
                        modList = modList.Substring(k);
                        k = modList.IndexOf("[");
                        modList = modList.Substring(k + 1);
                        k = modList.IndexOf("]");
                        modList = modList.Substring(0, k);
                        k = modList.IndexOf("\"modid\":\"");
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if(k < 0)
                            {
                                T7.Visibility = Visibility.Collapsed;
                                PlayerList.Visibility = Visibility.Collapsed;
                            }
                            while (k >= 0)
                            {
                                modList = modList.Substring(k + "\"modid\":\"".Length);
                                k = modList.IndexOf("\"");
                                string str = modList;
                                str = str.Substring(0, k);
                                    /*
                                    /* TextBlock
                                    * HorizontalAlignment="Left"
                                    * Margin="44,0,10,5"
                                    * MaxWidth="540"
                                    */
                                TextBlock textBlock = new TextBlock();
                                textBlock.HorizontalAlignment = HorizontalAlignment.Left;
                                textBlock.Margin = new Thickness(0, 0, 0, 5);
                                textBlock.MaxWidth = 540;
                                textBlock.Foreground = baseColora.Font;
                                textBlock.FontSize = 13;
                                textBlock.FontFamily = baseColora.Fonts;
                                textBlock.Text = str;
                                ModList.Children.Add(textBlock);

                                k = modList.IndexOf("\"modid\":\"");
                            }
                        }), DispatcherPriority.SystemIdle, null);
                        k = modList.IndexOf("forge");
                        if (k >= 0)
                        {
                            string forge = modList;
                            forge = forge.Substring(k);
                            k = forge.IndexOf("\version\": \"");
                            forge = forge.Substring(k + "\version\": \"".Length + 8);
                            k = forge.IndexOf("\"");
                            forge = forge.Substring(0, k);
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                T9.Text = "模组列表： (Forge - " + forge + ") ";
                            }), DispatcherPriority.SystemIdle, null);
                        }
                    }
                    else
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            T7.Visibility = Visibility.Collapsed;
                            PlayerList.Visibility = Visibility.Collapsed;
                        }), DispatcherPriority.SystemIdle, null);
                    }
                }
                catch (Exception ex)
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        CD3.Visibility = Visibility.Visible;
                        T4.Text = "ERR - " + ex;
                    }), DispatcherPriority.SystemIdle, null);
                }
            }
            else
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (string.IsNullOrWhiteSpace(ping))
                    {
                        T4.Text = "ERR - 服务器返回了一个空消息，它可能不想理你= =";
                    }
                    else
                    {
                        T4.Text = ping;
                    }
                }), DispatcherPriority.SystemIdle, null);
            }
            END: this.Dispatcher.BeginInvoke(new Action(() =>
            {
                ButtonHelper.SetIsWaiting(RunButton, false);
            }), DispatcherPriority.SystemIdle, null);
        }

        private void Clear(object sender, RoutedEventArgs e)
        {

        }
    }
}
