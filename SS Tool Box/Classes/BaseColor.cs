using SS_Tool_Box.Classes.BaseUIColor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SS_Tool_Box.Classes
{
    public class BaseColor
    {
        public SolidColorBrush Tran = new SolidColorBrush();      //纯透明
        public SolidColorBrush FontM = new SolidColorBrush();     //主题字体颜色
        public SolidColorBrush Main = new SolidColorBrush();      //主题颜色
        public SolidColorBrush Card = new SolidColorBrush();      //卡片颜色
        public SolidColorBrush Font = new SolidColorBrush();      //字体颜色
        public SolidColorBrush Fg = new SolidColorBrush();        //前景色
        public SolidColorBrush Bg = new SolidColorBrush();        //背景色
        public SolidColorBrush DBg = new SolidColorBrush();       //深背景色
        public BitmapImage Bgp;                                   //主背景图
        public BitmapImage Bgpa;                                  //背景图
        public BitmapImage Bgps;                                  //主背景小图
        public FontFamily Fonts;                                  //字体

        public bool DarkMode;
        public int Theme;

        public bool setColor(int ColorID, bool DarkMode)
        {
            this.DarkMode = DarkMode;
            this.Theme = ColorID;

            this.Tran.Color = Color.FromArgb(0, 0, 0, 0);
            switch (ColorID)
            {
                case 1:
                default:
                    {
                        DefaultColor defaultColor = new DefaultColor(DarkMode);
                        Card = defaultColor.Cardbrush;
                        Main = defaultColor.Mainbrush;
                        Font = defaultColor.Fontbrush;
                        FontM = defaultColor.FontMbrush;
                        Fg = defaultColor.Fgbrush;
                        Bg = defaultColor.Bgbrush;
                        Fonts = defaultColor.font;
                        DBg = defaultColor.DBgbrush;
                        Bgp = defaultColor.Bgp;
                        Bgpa = defaultColor.Bgpa;
                        Bgps = defaultColor.Bgps;
                    }
                    break;

                case 2:
                    {
                        BLCOrange defaultColor = new BLCOrange(DarkMode);
                        Card = defaultColor.Cardbrush;
                        Main = defaultColor.Mainbrush;
                        Font = defaultColor.Fontbrush;
                        FontM = defaultColor.FontMbrush;
                        Fg = defaultColor.Fgbrush;
                        Bg = defaultColor.Bgbrush;
                        Fonts = defaultColor.font;
                        DBg = defaultColor.DBgbrush;
                        Bgp = defaultColor.Bgp;
                        Bgpa = defaultColor.Bgpa;
                        Bgps = defaultColor.Bgps;
                    }
                    break;

                case 3:
                    {
                        LTCatBlue defaultColor = new LTCatBlue(DarkMode);
                        Card = defaultColor.Cardbrush;
                        Main = defaultColor.Mainbrush;
                        Font = defaultColor.Fontbrush;
                        FontM = defaultColor.FontMbrush;
                        Fg = defaultColor.Fgbrush;
                        Bg = defaultColor.Bgbrush;
                        Fonts = defaultColor.font;
                        DBg = defaultColor.DBgbrush;
                        Bgp = defaultColor.Bgp;
                        Bgpa = defaultColor.Bgpa;
                        Bgps = defaultColor.Bgps;
                    }
                    break;
            }
            return true;
        }

        public bool setColor(int r, int g, int b, String bgpa, String bgps, bool DarkMode)
        {
            this.DarkMode = DarkMode;
            this.Theme = 4;

            this.Tran.Color = Color.FromArgb(0, 0, 0, 0);
            DefaultColor defaultColor = new DefaultColor(DarkMode);
            if(DarkMode)
            {
                Card.Color = Color.FromArgb(204, 62, 62, 66);
            }
            else
            {
                Card.Color = Color.FromArgb(204, 255, 255, 255);
            }
            Main.Color = Color.FromArgb(255, byte.Parse(r.ToString()), byte.Parse(g.ToString()), byte.Parse(b.ToString()));
            if (r * 0.299 + g * 0.578 + b * 0.114 >= 192)
            { //浅色
                FontM.Color = Color.FromArgb(255, 0, 0, 0);
            }
            else
            {  //深色
                FontM.Color = Color.FromArgb(255, 255, 255, 255);
            }
            if (DarkMode)
            {
                Font.Color = Color.FromArgb(255, 255, 255, 255);
            }
            else
            {
                Font.Color = Color.FromArgb(255, 0, 0, 0);
            }
            Fg = defaultColor.Fgbrush;
            Bg = defaultColor.Bgbrush;
            Fonts = defaultColor.font;
            DBg = defaultColor.DBgbrush;
            //Bgp = defaultColor.Bgp;
            //Bgpa = bgpa;
            //Bgps = bgps;
            return true;
        }

        public BaseColor()
        {
            DefaultColor defaultColor = new DefaultColor(false);
            Card = defaultColor.Cardbrush;
            Font = defaultColor.Fontbrush;
            Fg = defaultColor.Fgbrush;
            Bg = defaultColor.Bgbrush;
            Fonts = defaultColor.font;
            DBg = defaultColor.DBgbrush;
        }
    }
}
