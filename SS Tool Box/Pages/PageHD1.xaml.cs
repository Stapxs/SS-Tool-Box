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
using System.Net;
using SS_Tool_Box.Classes;
using Newtonsoft.Json.Linq;
using System.IO;
using SS_Tool_Box_By_WPF;

namespace SS_Tool_Box
{
    /// <summary>
    /// Page3.xaml 的交互逻辑
    /// </summary>
    public partial class PageHD1 : Page
    {
        bool fist = true;
        String[] names;
        Error error = new Error();
        DateTime loadingtimes;
        BaseColor baseColora = Main.baseColor;

        public PageHD1()
        {
            loadingtimes = DateTime.Now;

            InitializeComponent();

            this.Height = 500;

            String stTitle = "Smartisan OS 图标获取";
            this.Title.Foreground = baseColora.Fg;
            this.Title.FontFamily = baseColora.Fonts;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            String stSays = "通过POST网页获取地址的方法从锤科服务器上获取重绘过的APP图标。";
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

            this.MT2.Foreground = baseColora.Fg;
            this.MT2.FontFamily = baseColora.Fonts;
            this.MT2.FontSize = 15;

            this.RunCard.Background = baseColora.Card;
            this.CD1.Background = baseColora.Card;
            this.IconCard.Background = baseColora.Card;

            this.PackageIn.Background = baseColora.Bg;

            this.Percent.Background = baseColora.DBg;

            this.RunButton.Foreground = baseColora.Fg;
            this.RunButton.Background = baseColora.Tran;

            ButtonHelper.SetHoverBrush(RunButton, baseColora.DBg);

            this.RunCard.Visibility = Visibility.Collapsed;
            this.Errorsay.Visibility = Visibility.Collapsed;
            this.IconCard.Visibility = Visibility.Collapsed;

            error.logWriter("加载工具完成，耗时:" + (DateTime.Now - loadingtimes).ToString(), false);
        }

        //运行
        private void RunTool(object sender, RoutedEventArgs e)
        {
            this.IconCard.Visibility = Visibility.Collapsed;
            ButtonHelper.SetIsWaiting(RunButton, true);
            if (String.IsNullOrWhiteSpace(PackageIn.Text))
            {
                error.ErrorTo("发现错误（SOS - 001）：输入为空。", Percent, Errorsay);
                ButtonHelper.SetIsWaiting(RunButton, false);
                this.RunCard.Visibility = Visibility.Visible;
                return;
            }
            
            try
            {
                string url = "https://setting.smartisan.com/app/icon";
                string packageof = "[{\"package\":\"" + PackageIn.Text + "\"}]";

                string GetJson = HttpUitls.Post(url, packageof, "", "application/json");

                JObject obj = JObject.Parse(GetJson);

                JToken record = obj["body"]["app_icon"][PackageIn.Text];

                JToken recorda = obj["body"]["app_icon"][PackageIn.Text];
                int num = -1;
                int numa = 0;
                if (!Directory.Exists("SSTB/Files/Logo"))
                {
                    Directory.CreateDirectory("SSTB/Files/Logo");
                }
                if (!fist)
                {
                    Grid grida = new Grid();
                    for (int i=0; i<names.Length; i++)
                    {
                        grida = Iconall.FindName(names[i]) as Grid;
                        Iconall.Children.Remove(grida);
                        Iconall.UnregisterName(names[i]);
                    }
                }
                fist = false;
                foreach (JObject jpa in recorda)
                {
                    numa++;
                }
                names = new String[numa];
                    foreach (JObject jp in record)
                {
                    num++;
                    if (!Directory.Exists("SSTB/Files/Logo/" + num))
                    {
                        Directory.CreateDirectory("SSTB/Files/Logo/" + num);
                    }
                    string filepath = "SSTB/Files/Logo/" + num + "/logo.png";
                    string filepatha = "SSTB/Files/Logo/" + num + "/md5.xml";
                    string filepathb = "SSTB/Files/Logo/logo" + num + ".png";

                    WebClient mywebclient = new WebClient();

                    string urlPng = jp["logo"].ToString();
                    string urlmd5 = jp["md5"].ToString();

                    mywebclient.DownloadFile(urlPng, filepath);
                    mywebclient.DownloadFile(urlmd5, filepatha);
                    mywebclient.DownloadFile(urlPng, filepathb);

                    Image img = new Image();
                    Grid grid = new Grid();

                    Iconall.Children.Add(grid);
                    grid.Children.Add(img);

                    RegisterName("grid" + num, grid);
                    names[num] = "grid" + num;

                    img.Height = 256;
                    img.Width = 540;
                    Thickness thickness = new Thickness();
                    thickness.Left = 0;
                    thickness.Top = 18;
                    thickness.Right = 0;
                    thickness.Bottom = 15;
                    img.Margin = thickness;
                    img.Source = new BitmapImage(new Uri(urlPng));
                    //this.T.Source = new BitmapImage(new Uri(urlPng));
                }
                T5.Text = "原图以及MD5检验文件已保存在程序所在目录的File/Logo文件夹下";
                this.IconCard.Visibility = Visibility.Visible;

                this.RunCard.Visibility = Visibility.Collapsed;
            }
            catch(Exception ex)
            {
                error.ErrorTo("发现错误（SOS - 003）：请求图标失败， 错误原因：", "" + ex, Percent, Errorsay);
                this.RunCard.Visibility = Visibility.Visible;
                ButtonHelper.SetIsWaiting(RunButton, false);
                return;
            }
            ButtonHelper.SetIsWaiting(RunButton, false);
        }

        //添加
        private void Add(object sender, RoutedEventArgs e)
        {

        }
    }
}
