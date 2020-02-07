using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Panuon.UI.Silver;
using System.Net;
using SS_Tool_Box.Classes;
using Newtonsoft.Json.Linq;
using SS_Tool_Box_By_WPF;

namespace SS_Tool_Box
{
    /// <summary>
    /// Page6.xaml 的交互逻辑
    /// </summary>
    public partial class Page6 : Page
    {

        BaseColor baseColora = Main.baseColor;

        public Page6()
        {
            InitializeComponent();

            this.Height = 477;

            String stTitle = "QQ 消息生成";
            this.Title.Foreground = baseColora.Fg;
            this.Title.FontFamily = baseColora.Fonts; ;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            String stSays = "生成一份由控件拼接而成的QQ聊天内容。";
            this.Says.Foreground = baseColora.Fg;
            this.Says.FontFamily = baseColora.Fonts;
            this.Says.FontSize = 14;
            Says.Text = stSays;

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

            this.QQ.Background = baseColora.Bg;
            this.QQ.Foreground = baseColora.Fg;
            this.QQName.Background = baseColora.Bg;
            this.QQName.Foreground = baseColora.Fg;
            this.QQSay.Background = baseColora.Bg;
            this.QQSay.Foreground = baseColora.Fg;

            this.RunCard.Visibility = Visibility.Collapsed;
            this.Errorsay.Visibility = Visibility.Collapsed;
        }

        private void IsMain_Checked(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void PopSet_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void RunButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
