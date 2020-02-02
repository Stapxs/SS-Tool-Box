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
using SS_Tool_Box_By_WPF;

namespace SS_Tool_Box
{
    /// <summary>
    /// Page3.xaml 的交互逻辑
    /// </summary>
    public partial class Page4 : Page
    {
        BaseColor baseColora = Main.baseColor;

        public Page4()
        {
            InitializeComponent();

            this.Height = 441;

            String stTitle = "倒计时";
            this.Title.Foreground =
                baseColora.Fg;
            this.Title.FontFamily = baseColora.Fonts;;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            String stSays = "这儿会显示各种乱七八糟的倒计时，也可以自定义倒计时。";
            this.Says.Foreground = baseColora.Fg;
            this.Says.FontFamily = baseColora.Fonts;
            this.Says.FontSize = 14;
            Says.Text = stSays;

            this.T4.Foreground = baseColora.Fg;
            this.T4.FontFamily = baseColora.Fonts;
            this.T4.FontSize = 13;

            this.MT2.Foreground = baseColora.Fg;
            this.MT2.FontFamily = baseColora.Fonts;
            this.MT2.FontSize = 15;

            this.RunCard.Background = baseColora.Card;
            this.CD1.Background = baseColora.Card;
            this.CD2.Background = baseColora.Card;

            this.Percent.Background = baseColora.DBg;


            ButtonHelper.SetHoverBrush(RunButton, baseColora.DBg);
            ButtonHelper.SetHoverBrush(ClearButton, baseColora.Fg);

            this.RunButton.Foreground = baseColora.Fg;
            this.ClearButton.Foreground = baseColora.Fg;
            this.RunButton.Background = baseColora.Tran;
            this.ClearButton.Background = baseColora.Tran;

            this.RunCard.Visibility = Visibility.Collapsed;
            this.Errorsay.Visibility = Visibility.Collapsed;

            bool error = UpdateData();
        }

        //刷新
        private void RunTool(object sender, RoutedEventArgs e)
        {
            bool error = UpdateData();
        }

        //添加
        private void Add(object sender, RoutedEventArgs e)
        {

        }

        //更新
        private bool UpdateData()
        {

            return true;
        }
    }
}
