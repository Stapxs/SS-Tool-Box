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
using SS_Tool_Box;
using SS_Tool_Box.Classes;

namespace SS_Tool_Box_By_WPF
{
    /// <summary>
    /// OpennInWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OpennInWindow : WindowX
    {

        BaseColor baseColora = Main.baseColor;

        public OpennInWindow()
        {
            InitializeComponent();

            WindowXCaption.SetBackground(this, baseColora.Bg);
            WindowXCaption.SetForeground(this, baseColora.Fg);

            BG.BeginInit();
            BG.Source = baseColora.Bgp;
            BG.EndInit();

            switch (Main.NowChoice)
            {
                case 1:
                    {
                        Page1 page1 = new Page1();
                        Page.Content = new Frame()
                        {
                            Content = page1
                        };
                    }
                    break;
                case 2:
                    {
                        Page2 page2 = new Page2();
                        Page.Content = new Frame()
                        {
                            Content = page2
                        };
                    }
                    break;
                case 3:
                    {
                        Page3 page3 = new Page3();
                        Page.Content = new Frame()
                        {
                            Content = page3
                        };
                    }
                    break;
                case 4:
                    {
                        Page4 page4 = new Page4();
                        Page.Content = new Frame()
                        {
                            Content = page4
                        };
                    }
                    break;
                case 5:
                    {
                        Page5 page5 = new Page5();
                        Page.Content = new Frame()
                        {
                            Content = page5
                        };
                    }
                    break;
                case 6:
                    {
                        Page6 page6 = new Page6();
                        Page.Content = new Frame()
                        {
                            Content = page6
                        };
                    }
                    break;
                case 7:
                    {
                        Page7 page7 = new Page7();
                        Page.Content = new Frame()
                        {
                            Content = page7
                        };
                    }
                    break;
                case -1:
                    {
                        PageHD1 pageHD1 = new PageHD1();
                        Page.Content = new Frame()
                        {
                            Content = pageHD1
                        };
                    }
                    break;
                default:
                    {
                        PageNone pageNone = new PageNone();
                        Page.Content = new Frame()
                        {
                            Content = pageNone
                        };
                    }
                    break;
            }
            Topmost = true;
        }
    }
}