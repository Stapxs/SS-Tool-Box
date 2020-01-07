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

            String stTitle = "倒计时";
            this.Title.Foreground = baseColora.Fg;
            this.Title.FontFamily = baseColora.Fonts;;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            String stSays = "这儿会显示各种乱七八糟的倒计时，也可以自定义倒计时。";
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

            this.MT1.Foreground = baseColora.Fg;
            this.MT1.FontFamily = baseColora.Fonts;
            this.MT1.FontSize = 15;
            this.MT2.Foreground = baseColora.Fg;
            this.MT2.FontFamily = baseColora.Fonts;
            this.MT2.FontSize = 15;

            this.RunCard.Background = baseColora.Card;
            this.CD1.Background = baseColora.Card;
            this.CD2.Background = baseColora.Card;
            this.CD3.Background = baseColora.Card;

            this.Percent.Background = baseColora.DBg;
            this.R1.Background = baseColora.DBg;
            this.R2.Background = baseColora.DBg;
            this.R3.Background = baseColora.DBg;


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
            ProgressBarHelper.SetAnimateTo(R1, 0);
            bool error = UpdateData();
        }

        //添加
        private void Add(object sender, RoutedEventArgs e)
        {

        }

        //更新
        private bool UpdateData()
        {
            DateTime dt = DateTime.Now;
            DateTime ch = DateTime.Now;

            int[] dayinmon = { 0, 31, 0, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            int m = int.Parse(DateTime.Now.ToString("yyyy"));
            if (m % 4 == 0 && m % 100 != 0 || m % 400 == 0)
            {
                dayinmon[1] = 29;
            }
            else
            {
                dayinmon[1] = 28;
            }
            int dayinyear = 0;
            for(int i=0; i<13; i++)
            {
                dayinyear += dayinmon[i];
            }

            int year = int.Parse(DateTime.Now.ToString("yyyy"));
            int mon = int.Parse(DateTime.Now.ToString("MM"));
            int day = int.Parse(DateTime.Now.ToString("dd"));
            int hour = int.Parse(DateTime.Now.ToString("hh"));

            int dayofyear = int.Parse(DateTime.Now.DayOfYear.ToString());

            double dayyl = dayinyear - dayofyear + 0.0;
            T1.Text = "今年还有" + (dayinyear - dayofyear) + "天";
            ProgressBarHelper.SetAnimateTo(R1, dayyl / dayinyear * 100);
            double daywl = dayinmon[mon] - day + 0.0;
            T2.Text = "本月还有" + (dayinmon[mon] - day) + "天";
            ProgressBarHelper.SetAnimateTo(R2, daywl / dayinmon[mon] * 100);
            double hourl = 24.0 - hour;
            T3.Text = "今天还有" + (24 - hour) + "小时";
            ProgressBarHelper.SetAnimateTo(R3, hourl / 24 * 100);
            return true;
        }
    }
}
