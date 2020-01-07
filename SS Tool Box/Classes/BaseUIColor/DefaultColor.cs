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

namespace SS_Tool_Box.Classes.BaseUIColor
{
    class DefaultColor      //配色类：林槐白
    {
        public SolidColorBrush Cardbrush = new SolidColorBrush();      //卡片颜色
        public SolidColorBrush Fontbrush = new SolidColorBrush();      //字体颜色
        public SolidColorBrush Fgbrush = new SolidColorBrush();        //前景色
        public SolidColorBrush Bgbrush = new SolidColorBrush();        //背景色
        public SolidColorBrush DBgbrush = new SolidColorBrush();        //深背景色
        public BitmapImage Bgp;                                       //主背景图
        public BitmapImage Bgpa;                                       //背景图
        public FontFamily font;                                        //字体

        public DefaultColor(bool DarkMode)
        {
            if (DarkMode)
            {
                Cardbrush.Color = Color.FromArgb(204, 62, 62, 66);
                Fontbrush.Color = Color.FromArgb(255, 255, 255, 255);
                Fgbrush = Fontbrush;
                Bgbrush.Color = Color.FromArgb(255, 62, 62, 62);
                DBgbrush.Color = Color.FromArgb(255, 37, 37, 37);
                Bgp = new BitmapImage(new Uri("pack://application:,,,/Resource/BGDrck.png"));
                Bgpa = new BitmapImage(new Uri("pack://application:,,,/Resource/BGDrck.png"));
                font = new FontFamily("方正兰亭简黑");
            }
            else
            {
                Cardbrush.Color = Color.FromArgb(204, 255, 255, 255);
                Fontbrush.Color = Color.FromArgb(255, 100, 100, 100);
                Fgbrush = Fontbrush;
                Bgbrush.Color = Color.FromArgb(255, 255, 255, 255);
                DBgbrush.Color = Color.FromArgb(255, 234, 234, 234);
                Bgp = new BitmapImage(new Uri("pack://application:,,,/Resource/BG.png"));
                Bgpa = new BitmapImage(new Uri("pack://application:,,,/Resource/BGLight.png"));
                font = new FontFamily("方正兰亭简黑");
            }
        }
    }
}
