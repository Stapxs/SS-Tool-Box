using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SS_Tool_Box.Classes.BaseUIColor
{
    class DefaultColor      //配色类：林槐白
    {
        public SolidColorBrush Cardbrush = new SolidColorBrush();      //卡片颜色
        public SolidColorBrush Fontbrush = new SolidColorBrush();      //字体颜色
        public SolidColorBrush FontMbrush = new SolidColorBrush();     //主题字体颜色
        public SolidColorBrush Mainbrush = new SolidColorBrush();      //主题颜色
        public SolidColorBrush Fgbrush = new SolidColorBrush();        //前景色
        public SolidColorBrush Bgbrush = new SolidColorBrush();        //背景色
        public SolidColorBrush DBgbrush = new SolidColorBrush();       //深背景色
        public BitmapImage Bgp;                                        //主背景图
        public BitmapImage Bgps;                                        //主背景小图
        public BitmapImage Bgpa;                                       //背景图
        public FontFamily font;                                        //字体

        public DefaultColor(bool DarkMode)
        {
            if (DarkMode)
            {
                Cardbrush.Color = Color.FromArgb(204, 62, 62, 66);
                Fontbrush.Color = Color.FromArgb(255, 255, 255, 255);
                FontMbrush.Color = Fontbrush.Color;
                Fgbrush = Fontbrush;
                Bgbrush.Color = Color.FromArgb(255, 62, 62, 62);
                Mainbrush.Color = Bgbrush.Color;
                DBgbrush.Color = Color.FromArgb(255, 37, 37, 37);
                Bgp = new BitmapImage(new Uri("pack://application:,,,/Resource/BGDrck.png"));
                Bgpa = new BitmapImage(new Uri("pack://application:,,,/Resource/BGDrck.png"));
                Bgps = new BitmapImage(new Uri("pack://application:,,,/Resource/BGtop.bmp"));
                font = new FontFamily("方正兰亭简黑");
            }
            else
            {
                Cardbrush.Color = Color.FromArgb(190, 255, 255, 255);
                Fontbrush.Color = Color.FromArgb(255, 100, 100, 100);
                Mainbrush.Color = Fontbrush.Color;
                FontMbrush.Color = Color.FromArgb(255, 240, 240, 240);
                Fgbrush = Fontbrush;
                Bgbrush.Color = Color.FromArgb(255, 255, 255, 255);
                DBgbrush.Color = Bgbrush.Color;
                Bgp = new BitmapImage(new Uri("pack://application:,,,/Resource/BG.png"));
                Bgpa = new BitmapImage(new Uri("pack://application:,,,/Resource/BGLight.png"));
                Bgps = new BitmapImage(new Uri("pack://application:,,,/Resource/BGtop.bmp"));
                font = new FontFamily("方正兰亭简黑");
            }
        }
    }
}
