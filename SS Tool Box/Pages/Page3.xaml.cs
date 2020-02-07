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

namespace SS_Tool_Box
{
    /// <summary>
    /// Page3.xaml 的交互逻辑
    /// </summary>
    public partial class Page3 : Page
    {
        public Page3()
        {
            InitializeComponent();

            this.Height = 477;

            SolidColorBrush GetColor = new SolidColorBrush();
            GetColor.Color = System.Windows.Media.Color.FromArgb(255, 100, 100, 100);

            FontFamily font = new FontFamily("方正兰亭简黑");

            SolidColorBrush solidColorBrush = new SolidColorBrush();
            solidColorBrush.Color = System.Windows.Media.Color.FromArgb(255, 100, 100, 100);

            String stTitle = "快速启动";
            this.Title.Foreground = solidColorBrush;
            this.Title.FontFamily = font;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            String stSays = "一个启动器功能而已，帮你启动各种乱七八糟的东西。";
            this.Says.Foreground = solidColorBrush;
            this.Says.FontFamily = font;
            this.Says.FontSize = 14;
            Says.Text = stSays;

            this.T4.Foreground = solidColorBrush;
            this.T4.FontFamily = font;
            this.T4.FontSize = 13;

            this.MT1.Foreground = solidColorBrush;
            this.MT1.FontFamily = font;
            this.MT1.FontSize = 15;
        }

        private void RunTool(object sender, RoutedEventArgs e)
        {
            
        }

        private void Clear(object sebder, RoutedEventArgs s)
        {
            //打开窗口About
            /*
            AddIcons add = new AddIcons();
            add.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            add.Owner = SS_Tool_Box_By_WPF.Main;
            SS_Tool_Box_By_WPF.Main.IsMaskVisible = true;
            add.ShowDialog();
            IsMaskVisible = false;
            */
        }
    }
}
