using SS_Tool_Box.Classes;
using SS_Tool_Box_By_WPF;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SS_Tool_Box
{
    /// <summary>
    /// PageNone.xaml 的交互逻辑
    /// </summary>
    public partial class PageNone : Page
    {
        BaseColor baseColora = Main.baseColor;

        public PageNone()
        {
            InitializeComponent();


            String stTitle = "哎呦喂！";
            this.Title.Foreground = baseColora.Fg;
            this.Title.FontFamily = baseColora.Fonts;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            String stSays = "没有找到这个界面欸……或者我还没有做完这个界面/小声，去看看别的？";
            this.Says.Foreground = baseColora.Fg;
            this.Says.FontFamily = baseColora.Fonts;
            this.Says.FontSize = 14;
            Says.Text = stSays;

            this.T1.Foreground = baseColora.Fg;
            this.T1.FontFamily = baseColora.Fonts;
            this.T1.FontSize = 12;

            this.CD1.Background = baseColora.Card;
            this.CD2.Background = baseColora.Card;

            this.Load.Foreground = baseColora.Fg;
        }
    }
}
