using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Panuon.UI.Silver;
using SS_Tool_Box.Classes;

namespace SS_Tool_Box_By_WPF
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : WindowX
    {

        BaseColor baseColora = Main.baseColor;

        public About()
        {
            InitializeComponent();

            WindowXCaption.SetBackground(this, baseColora.Main);
            WindowXCaption.SetForeground(this, baseColora.FontM);

            BG.BeginInit();
            BG.Source = baseColora.Bgpa;
            BG.EndInit();
            BG1.BeginInit();
            BG1.Source = baseColora.Bgpa;
            BG1.EndInit();
            BG2.BeginInit();
            BG2.Source = baseColora.Bgpa;
            BG2.EndInit();

            this.MainTab.Background = baseColora.Bg;
            this.MainTab.Foreground = baseColora.Fg;
            TabControlHelper.SetHeaderPanelBackground(MainTab, baseColora.DBg);
            TabControlHelper.SetItemIcon(MainTab, baseColora.DBg);
            TabControlHelper.SetSelectedBackground(MainTab, baseColora.Bg);
            TabControlHelper.SetSelectedForeground(MainTab, baseColora.Fg);

            ButtonHelper.SetHoverBrush(B1, baseColora.Bg);
            IconHelper.SetForeground(B1, baseColora.Fg);
            this.B1.Foreground = baseColora.Fg;
            this.B1.Background = baseColora.Tran;
            ButtonHelper.SetHoverBrush(B2, baseColora.Fg);
            IconHelper.SetForeground(B2, baseColora.Fg);
            this.B2.Foreground = baseColora.Fg;
            this.B2.Background = baseColora.Tran;
            ButtonHelper.SetHoverBrush(B3, baseColora.Bg);
            IconHelper.SetForeground(B3, baseColora.Fg);
            this.B3.Foreground = baseColora.Fg;
            this.B3.Background = baseColora.Tran;
            ButtonHelper.SetHoverBrush(B4, baseColora.Bg);
            IconHelper.SetForeground(B4, baseColora.Fg);
            this.B4.Foreground = baseColora.Fg;
            this.B4.Background = baseColora.Tran;
            ButtonHelper.SetHoverBrush(B5, baseColora.Bg);
            IconHelper.SetForeground(B5, baseColora.Fg);
            this.B5.Foreground = baseColora.Fg;
            this.B5.Background = baseColora.Tran;
            ButtonHelper.SetHoverBrush(B6, baseColora.Bg);
            IconHelper.SetForeground(B6, baseColora.Fg);
            this.B6.Foreground = baseColora.Fg;
            this.B6.Background = baseColora.Tran;
            ButtonHelper.SetHoverBrush(B7, baseColora.Bg);
            IconHelper.SetForeground(B7, baseColora.Fg);
            this.B7.Foreground = baseColora.Fg;
            this.B7.Background = baseColora.Tran;

            this.CD1.Background = baseColora.Bg;
            this.CD2.Background = baseColora.Bg;
            this.CD3.Background = baseColora.Bg;
            this.CD4.Background = baseColora.Bg;

            //SS列表样式
            this.SS.Foreground = baseColora.Fg;
            this.SS.FontFamily = baseColora.Fonts;
            this.SS.FontSize = 22;
            this.BLC.Foreground = baseColora.Fg;
            this.BLC.FontFamily = baseColora.Fonts;
            this.BLC.FontSize = 22;

            this.WhoSS.Text = "       这是一只喜欢做UI排版的SS，喜欢折腾。有事没事就想做点什么……然而大半都做不完的说，米粉、谷歌粉，会卖萌/大雾，没什么事的话可以陪聊(？) ，QQ , Twitter都行/逃。";
            this.WhoBLC.Text = "       [ 这个人什么都没留下 ]";
            this.WhoSS.Foreground = baseColora.Fg;
            this.WhoSS.FontFamily = baseColora.Fonts;
            this.WhoSS.FontSize = 14;
            this.WhoBLC.Foreground = baseColora.Fg;
            this.WhoBLC.FontFamily = baseColora.Fonts;
            this.WhoBLC.FontSize = 14;

            this.MT1.Foreground = baseColora.Fg;
            this.MT1.FontFamily = baseColora.Fonts;
            this.MT1.FontSize = 15;
            this.MT2.Foreground = baseColora.Fg;
            this.MT2.FontFamily = baseColora.Fonts;
            this.MT2.FontSize = 15;
            this.MT3.Foreground = baseColora.Fg;
            this.MT3.FontFamily = baseColora.Fonts;
            this.MT3.FontSize = 15;
            this.MT4.Foreground = baseColora.Fg;
            this.MT4.FontFamily = baseColora.Fonts;
            this.MT4.FontSize = 15;

            this.S1.Foreground = baseColora.Fg;
            this.S1.FontFamily = baseColora.Fonts;
            this.S1.FontSize = 13;
            this.S2.Foreground = baseColora.Fg;
            this.S2.FontFamily = baseColora.Fonts;
            this.S2.FontSize = 13;
            this.S3.Foreground = baseColora.Fg;
            this.S3.FontFamily = baseColora.Fonts;
            this.S3.FontSize = 13;
            this.S4.Foreground = baseColora.Fg;
            this.S4.FontFamily = baseColora.Fonts;
            this.S4.FontSize = 13;
        }

        private void OwnWeb(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ssteamcommunity.wordpress.com/");
        }

        private void Twitter(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitter.com/Stapx_Steve");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://space.bilibili.com/100705534");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.newtonsoft.com/json");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Panuon/PanuonUI.Silver");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.fontawesome.com.cn/");
        }
    }
}
