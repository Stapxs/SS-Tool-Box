using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Panuon.UI.Silver;
using System.Net;
using SS_Tool_Box.Classes;
using Newtonsoft.Json.Linq;
using SS_Tool_Box_By_WPF;
using System.Threading;
using System.Windows.Input;

namespace SS_Tool_Box
{
    /// <summary>
    /// Page3.xaml 的交互逻辑
    /// </summary>
    public partial class Page5 : Page
    {
        BaseColor baseColora = Main.baseColor;

        int err = -1;

        public Page5()
        {
            InitializeComponent();

            this.Height = 477;

            String stTitle = "Fuck QQ";
            this.Title.Foreground = baseColora.Fg;
            this.Title.FontFamily = baseColora.Fonts;;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            String stSays = "这儿是所有QQ相关功能的集合。某些操作需要使用到你的QQ号，请保证此QQ在登录状态。";
            this.Says.Foreground = baseColora.Fg;
            this.Says.FontFamily = baseColora.Fonts;
            this.Says.FontSize = 14;
            Says.Text = stSays;

            this.T1.Foreground = baseColora.Fg;
            this.T1.FontFamily = baseColora.Fonts;
            this.T1.FontSize = 13;
            this.T4.Foreground = baseColora.Fg;
            this.T4.FontFamily = baseColora.Fonts;
            this.T4.FontSize = 13;
            this.T5.Foreground = baseColora.Fg;
            this.T5.FontFamily = baseColora.Fonts;
            this.T5.FontSize = 13;
            this.T3.Foreground = baseColora.Fg;
            this.T3.FontFamily = baseColora.Fonts;
            this.T3.FontSize = 13;

            this.MT2.Foreground = baseColora.Fg;
            this.MT2.FontFamily = baseColora.Fonts;
            this.MT2.FontSize = 15;
            this.MT3.Foreground = baseColora.Fg;
            this.MT3.FontFamily = baseColora.Fonts;
            this.MT3.FontSize = 15;

            this.CD1.Background = baseColora.Card;
            this.RunCard.Background = baseColora.Card;
            this.HandCard.Background = baseColora.Card;
            this.ColCard.Background = baseColora.Card;

            this.RunButton.Foreground = baseColora.Fg;
            this.RunButton.Background = baseColora.Tran;
            this.About.Foreground = baseColora.Fg;
            this.About.Background = baseColora.Tran;
            this.Chat.Foreground = baseColora.Fg;
            this.Chat.Background = baseColora.Tran;

            this.QQ.Background = baseColora.Bg;
            this.QQY.Background = baseColora.Bg;
            this.QQ.Foreground = baseColora.Fg;
            this.QQY.Foreground = baseColora.Fg;


            ButtonHelper.SetHoverBrush(RunButton, baseColora.DBg);

            this.Percent.Background = baseColora.DBg;

            this.RunCard.Visibility = Visibility.Collapsed;
            this.Errorsay.Visibility = Visibility.Collapsed;
            this.HandCard.Visibility = Visibility.Collapsed;
            this.ColCard.Visibility = Visibility.Collapsed;
        }

        //运行
        private void RunTool(object sender, RoutedEventArgs e)
        {
            err = -1;
            this.HandCard.Visibility = Visibility.Collapsed;
            this.ColCard.Visibility = Visibility.Collapsed;
            Error error = new Error();

            Action action = new Action(() => {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (String.IsNullOrWhiteSpace(QQ.Text) || String.IsNullOrWhiteSpace(QQY.Text))
                    {
                        error.ErrorTo("发现错误（EQQ - 002）：输入为空。", Percent, Errorsay);
                        this.RunCard.Visibility = Visibility.Visible;
                        return;
                    }
                }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
                try
                {
                    err = 0;
                    string QQa = "NULL";
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        QQa = QQ.Text;
                    }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
                    Thread.Sleep(3000);
                    string url = "http://q1.qlogo.cn/g?b=qq&nk=" + QQa + "&s=5";
                    string filepath = "SSTB/Files/Head/Head" + QQa + ".jpg";
                    WebClient mywebclient = new WebClient();
                    mywebclient.DownloadFile(url, filepath);

                    string urls = "http://q1.qlogo.cn/g?b=qq&nk=" + QQa + "&s=3";
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        this.Hand.Source = new BitmapImage(new Uri(urls));
                        T5.Text = "高清头像已保存到程序File所在位置";
                        this.HandCard.Visibility = Visibility.Visible;
                    }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        err = 1;
                        String errorString = ex.ToString();
                        error.ErrorTo("发现错误（EQQ - 001）：请求头像失败， 错误原因：" + errorString.Substring(0, errorString.IndexOf("在")), Percent, Errorsay);
                    }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
                }
            });
            action.BeginInvoke(null, null);

            Action actiona = new Action(() => {
                try
                {
                    err = 0;
                    string QQa = "NULL";
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        QQa = QQ.Text;
                    }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
                    Thread.Sleep(3000);
                    String iduri = "https://api.toubiec.cn/qq?qq=" + QQa + "&size=300";
                    string GetJson = HttpUitls.Get(iduri, "DEFALT");
                    JObject obj = JObject.Parse(GetJson);
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if(err == 1)
                        {
                            return;
                        }
                        MT3.Text = "其他操作" + " —— " + obj["name"].ToString();
                        this.ColCard.Visibility = Visibility.Visible;
                    }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
                }
                catch (Exception ex)
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        err = 1;
                        String errorString = ex.ToString();
                        error.ErrorTo("发现错误（EQQ - 003）：请求昵称失败， 错误原因：" + errorString.Substring(0, errorString.IndexOf("在")), Percent, Errorsay);
                    }), System.Windows.Threading.DispatcherPriority.SystemIdle, null);
                }
            });
            actiona.BeginInvoke(null, null);
            if(err == 1)
            {
                this.RunCard.Visibility = Visibility.Visible;
            }
            else if(err == 0)
            {
                this.RunCard.Visibility = Visibility.Collapsed;
            }
        }

        //添加
        private void Add(object sender, RoutedEventArgs e)
        {
            
        }

        //和他聊天
        private void Chat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebBrowser wBrowser = new WebBrowser();
                wBrowser.Source = new Uri("tencent://Message/?uin=" + QQ.Text);
            }
            catch
            {

            }
        }

        //显示资料卡片
        private void About_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebBrowser wBrowser = new WebBrowser();
                wBrowser.Source = new Uri("tencent://snsapp/?cmd=2&ver=1&uin=" + QQ.Text + "&fuin=" + QQY.Text);
            }
            catch
            {

            }
        }

        void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scroll = (ScrollViewer)sender;
            if (e.Delta > 0)
            {
                // 向上滚动
                scroll.LineUp();
                scroll.LineUp();
            }
            if (e.Delta < 0)
            {
                // 向下滚动
                scroll.LineDown();
                scroll.LineDown();
            }
        }

    }
}
