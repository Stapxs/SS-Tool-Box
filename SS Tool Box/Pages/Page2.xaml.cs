using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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
using System.Windows.Threading;
using Panuon.UI.Silver;
using SS_Tool_Box.Classes;
using SS_Tool_Box_By_WPF;

namespace SS_Tool_Box
{
    /// <summary>
    /// Page2.xaml 的交互逻辑
    /// </summary>
    public partial class Page2 : Page
    {
        Brush ColorFst;
        Error error = new Error();
        BaseColor baseColora = Main.baseColor;

        public Page2()
        {
            InitializeComponent();

            this.Height = 477;

            ButtonHelper.SetIsWaiting(RunButton, false);
            this.Color.Fill = baseColora.Card;

            String stTitle = "调色板";
            this.Title.Foreground = baseColora.Fg;
            this.Title.FontFamily = baseColora.Fonts;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            String stSays = "一个显示RGBA颜色的工具，并且可以转换成十六进制。";
            this.Says.Foreground = baseColora.Fg;
            this.Says.FontFamily = baseColora.Fonts;
            this.Says.FontSize = 14;
            Says.Text = stSays;

            this.T1.Foreground = baseColora.Fg;
            this.T1.FontFamily = baseColora.Fonts;
            this.T1.FontSize = 13;
            this.T4.Foreground = baseColora.Fg;
            this.T4.FontFamily = baseColora.Fonts;
            this.T4.FontSize = 13;
            this.T6.Foreground = baseColora.Fg;
            this.T6.FontFamily = baseColora.Fonts;
            this.T6.FontSize = 13;
            this.T7.Foreground = baseColora.Fg;
            this.T7.FontFamily = baseColora.Fonts;
            this.T7.FontSize = 13;
            this.T8.Foreground = baseColora.Fg;
            this.T8.FontFamily = baseColora.Fonts;
            this.T8.FontSize = 13;
            this.T9.Foreground = baseColora.Fg;
            this.T9.FontFamily = baseColora.Fonts;
            this.T9.FontSize = 13;
            this.T10.Foreground = baseColora.Fg;
            this.T10.FontFamily = baseColora.Fonts;
            this.T10.FontSize = 13;

            this.From.Foreground = baseColora.Fg;
            this.From.FontFamily = baseColora.Fonts;

            this.Load.Foreground = baseColora.Fg;

            this.MT1.Foreground = baseColora.Fg;
            this.MT1.FontFamily = baseColora.Fonts;
            this.MT1.FontSize = 15;
            this.MT2.Foreground = baseColora.Fg;
            this.MT2.FontFamily = baseColora.Fonts;
            this.MT2.FontSize = 15;
            this.MT3.Foreground = baseColora.Fg;
            this.MT3.FontFamily = baseColora.Fonts;
            this.MT3.FontSize = 15;

            this.S1.Background = baseColora.DBg;
            this.S1.Foreground = baseColora.Fg;
            SliderHelper.SetThemeBrush(S1, baseColora.Main);
            this.S2.Background = baseColora.DBg;
            this.S2.Foreground = baseColora.Fg;
            SliderHelper.SetThemeBrush(S2, baseColora.Main);
            this.S3.Background = baseColora.DBg;
            this.S3.Foreground = baseColora.Fg;
            SliderHelper.SetThemeBrush(S3, baseColora.Main);

            this.CD1.Background = baseColora.Card;
            this.CD2.Background = baseColora.Card;
            this.CD3.Background = baseColora.Card;
            this.CD4.Background = baseColora.Card;
            this.CD5.Background = baseColora.Card;

            this.J16.Background = baseColora.Bg;
            this.RGBA.Background = baseColora.Bg;
            this.J16.Foreground = baseColora.Fg;
            this.RGBA.Foreground = baseColora.Fg;

            this.Percent.Background = baseColora.DBg;

            this.RunCard.Background = baseColora.Card;
            IconHelper.SetForeground(RunButton, baseColora.Fg);
            IconHelper.SetForeground(ClearButton, baseColora.Fg);

            this.RunButton.Foreground = baseColora.Fg;
            this.ClearButton.Foreground = baseColora.Fg;
            this.RunButton.Background = baseColora.Tran;
            this.ClearButton.Background = baseColora.Tran;

            this.RunCard.Visibility = Visibility.Collapsed;
            this.Errorsay.Visibility = Visibility.Collapsed;

            ColorFst = this.Percent.Foreground;

            //开始获取 Sukazyo 收集颜色表
            Thread thread = new Thread(Sukazyo);
            thread.Start();

        }

        private void Sukazyo()
        {
            bool download = true;
            if (File.Exists("SSTB/Files/Sukazyodata.txt"))
            {
                using (StreamReader sr = new StreamReader("SSTB/Files/Sukazyodata.txt", Encoding.Default))
                {
                    if(sr.Peek() > 0)
                    {
                        string lastdate = sr.ReadLine();
                        DateTime dt;
                        DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                        dtFormat.ShortDatePattern = "yyyy/MM/dd";
                        dt = Convert.ToDateTime(lastdate, dtFormat);
                        TimeSpan ts = DateTime.Now - dt;
                        if(ts.TotalDays > 10)
                        {
                            download = false;
                        }
                    }
                }
            }
            if (download)
            {
                if (File.Exists("SSTB/Files/Sukazyodata.txt"))
                {
                    File.Delete("SSTB/Files/Sukazyodata.txt");
                }
                using (FileStream fs = new FileStream("SSTB/Files/Sukazyodata.txt", FileMode.Append, FileAccess.Write))
                {
                    fs.Lock(0, fs.Length);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(DateTime.Now.ToShortDateString());
                    fs.Unlock(0, fs.Length);
                    sw.Flush();
                }
                try
                {
                    error.logWriter("尝试下载 Sukazyo 收集颜色表界面……", false);
                    string url = "https://srv.sukazyo.cc/color/";
                    string filepath = "SSTB/Files/Sukazyo.txt";
                    WebClient mywebclient = new WebClient();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    mywebclient.DownloadFile(url, filepath);
                    error.logWriter("下载 Sukazyo 收集颜色表界面完成。", false);
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        LoadColor.Visibility = Visibility.Collapsed;
                    }), DispatcherPriority.SystemIdle, null);
                }
                catch (Exception ex)
                {
                    error.logWriter("下载下载 Sukazyo 收集颜色表界面失败，" + ex, false);
                    return;
                }
            }
            try
            {
                List<Colors> colors = new List<Colors>();
                string content = File.ReadAllText("SSTB/Files/Sukazyo.txt");
                int i = content.IndexOf("h3");
                int j;
                while(i > 0)
                {
                    content = content.Substring(i);
                    i = content.IndexOf(">");       //标题开始位置
                    j = content.IndexOf("<");       //标题结束位置
                    string Name = content;
                    Name = Name.Substring(i + 1, j - i - 1);
                    content = content.Substring(j + 2);
                    i = content.IndexOf("<p");
                    content = content.Substring(i + 1);
                    i = content.IndexOf(">");       //颜色开始位置
                    content = content.Substring(i);
                    i = content.IndexOf("rgb");     //rgb开始位置
                    content = content.Substring(i);
                    i = content.IndexOf(")");       //rgb结束位置
                    string RGB = content;
                    RGB = RGB.Substring(0, i);
                    RGB = RGB.Substring(4);
                    i = RGB.IndexOf(",");
                    string d = RGB;
                    d = d.Substring(0, i);
                    RGB = RGB.Substring(i + 1);
                    int r = int.Parse(d);
                    i = RGB.IndexOf(",");
                    d = RGB;
                    d = d.Substring(0, i);
                    RGB = RGB.Substring(i + 1);
                    int g = int.Parse(d);
                    int b = int.Parse(RGB);
                    colors.Add(new Colors() { Name = Name, r = r, g = g, b = b});

                    i = content.IndexOf("h3");
                }
                CreateColorBoard(colors);
            }
            catch
            {
                
            }
        }

        class Colors
        {
            public string Name;
            public int r;
            public int g;
            public int b;
        }

        private void CreateColorBoard(List<Colors> colors)
        {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    for (int i = 0; i <= colors.Count - 1; i++)
                    {
                        SolidColorBrush ColorBg = new SolidColorBrush();
                        ColorBg.Color = System.Windows.Media.Color.FromArgb(Convert.ToByte(255), Convert.ToByte(colors[i].r), Convert.ToByte(colors[i].g), Convert.ToByte(colors[i].b));
                        SolidColorBrush ColorFg = new SolidColorBrush();
                        if (colors[i].r * 0.299 + colors[i].g * 0.578 + colors[i].b * 0.114 >= 192)
                        { //浅色
                            ColorFg.Color = System.Windows.Media.Color.FromArgb(255, 0, 0, 0);
                        }
                        else
                        {  //深色
                            ColorFg.Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
                        }

                        Grid grid = new Grid();
                        grid.Height = 60;
                        Grid gridBG = new Grid();
                        gridBG.Width = 4;
                        gridBG.Height = 60 * 0.6;
                        gridBG.HorizontalAlignment = HorizontalAlignment.Left;
                        gridBG.VerticalAlignment = VerticalAlignment.Center;
                        gridBG.Background = ColorBg;
                        Grid gridBGA = new Grid();
                        gridBGA.Width = 250;
                        gridBGA.Height = 60 * 0.6;
                        gridBGA.HorizontalAlignment = HorizontalAlignment.Right;
                        gridBGA.VerticalAlignment = VerticalAlignment.Center;
                        gridBGA.Background = ColorBg;
                        TextBlock title = new TextBlock();
                        title.FontSize = 13;
                        title.Text = colors[i].Name;
                        title.VerticalAlignment = VerticalAlignment.Top;
                        title.Margin = new Thickness(49, 10, 0, 0);
                        title.HorizontalAlignment = HorizontalAlignment.Left;
                        title.Foreground = ColorBg;
                        TextBlock things = new TextBlock();
                        things.FontSize = 13;
                        things.Text = RGBATo(colors[i].r, colors[i].g, colors[i].b, 255, 1) + " / " + colors[i].r + "," + colors[i].g + "," + colors[i].b;
                        things.VerticalAlignment = VerticalAlignment.Bottom;
                        things.Margin = new Thickness(49, 0, 0, 10);
                        things.Foreground = ColorBg;
                        grid.Children.Add(title);
                        grid.Children.Add(things);
                        grid.Children.Add(gridBG);
                        grid.Children.Add(gridBGA);
                        ColorsSukazyo.Children.Add(grid);
                    }
                }), DispatcherPriority.SystemIdle, null);
        }

        private void RunTool(object sebder, RoutedEventArgs s)
        {
            bool Error = false;
            bool[] InPutHas = new bool[4];
            String ErrorString = "";
            int which = 0;
            String[] ColorList = new string[2];

            ButtonHelper.SetIsWaiting(RunButton, true);

            this.Errorsay.Visibility = Visibility.Collapsed;
            this.RunCard.Visibility = Visibility.Visible;

            this.Percent.Foreground = ColorFst;
            ProgressBarHelper.SetAnimateTo(Percent, 0);

            for (int i = 0; i < 2; i++)
            {
                InPutHas[i] = true;
            }
            if (String.IsNullOrWhiteSpace(J16.Text))
            {
                InPutHas[0] = false;
            }
            if (String.IsNullOrWhiteSpace(RGBA.Text))
            {
                InPutHas[1] = false;
            }

            {
                bool has = false;
                for (int i = 0; i < 2; i++)
                {
                    if (InPutHas[i] && !has)
                    {
                        has = true;
                        which = i;
                        continue;
                    }
                    if (InPutHas[i] && has)
                    {
                        Error = true;
                        ErrorString = "发现错误（ECC - 002）：有多个无法统一的输入。";
                    }
                }
            }

            if ((InPutHas[0] || InPutHas[1]) == false)
            {
                Error = true;
                ErrorString = "发现错误（ECC - 001）：输入内容为空。";
            }
            ProgressBarHelper.SetAnimateTo(Percent, 100 / 3);

            if (Error)
            {
                error.ErrorTo(ErrorString, Percent, Errorsay);
            }
            else
            {
                SolidColorBrush ChangeColor = new SolidColorBrush();
                ChangeColor.Color = System.Windows.Media.Color.FromArgb(255, 209, 93, 93);

                switch (which + 1)
                {
                    case 1:
                        {
                            //十六进制
                            ColorList[0] = this.J16.Text;
                            int[] Color;
                            
                            if (!(this.J16.Text[0] == '#' && (this.J16.Text.Length == 9 || this.J16.Text.Length == 7)))
                            {
                                Error = true;
                                error.ErrorTo("发现错误（ECC - 003）：输入内容无效，请检查。错误内容为……你瞎几把输入了些什么玩意。", Percent, Errorsay);
                                break;
                            }
                            for (int i=1; i<=8; i++)
                            {
                                if(this.J16.Text[i] != '0' && this.J16.Text[i] != '1' && this.J16.Text[i] != '2' && this.J16.Text[i] != '3' && this.J16.Text[i] != '4' && this.J16.Text[i] != '5' && this.J16.Text[i] != '6' && this.J16.Text[i] != '7' && this.J16.Text[i] != '8' && this.J16.Text[i] != '9' && this.J16.Text[i] != 'A' && this.J16.Text[i] != 'B' && this.J16.Text[i] != 'C' && this.J16.Text[i] != 'D' && this.J16.Text[i] != 'E' && this.J16.Text[i] != 'F')
                                {
                                    Error = true;
                                    error.ErrorTo("发现错误（ECC - 003）：输入内容无效，请检查。错误内容为十六进制值大于F", Percent, Errorsay);
                                }
                                else
                                {
                                    goto A;
                                }
                            }
                            break;
                        A: Color = toRGBA(this.J16.Text, 1);
                            if(Color[4] == -1)
                            {
                                Error = true;
                                break;
                            }
                            ColorList[1] = Color[1] + "," + Color[2] + "," + Color[3] + "," + Color[0];
                            ChangeColor.Color = System.Windows.Media.Color.FromArgb(byte.Parse(Color[0].ToString()), byte.Parse(Color[1].ToString()), byte.Parse(Color[2].ToString()), byte.Parse(Color[3].ToString()));
                            SolidColorBrush CardColor = new SolidColorBrush();
                            if (Color[0] != 255)
                            {
                                CardColor.Color = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
                            }
                            else
                            {
                                CardColor.Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
                            }
                            this.ColorCard.Background = CardColor;
                            this.Color.Fill = ChangeColor;
                        }
                        break;
                    case 2:
                        {
                            //RGBA
                            ColorList[1] = this.RGBA.Text;
                            byte r, g, b, a;
                            string[] sArray = this.RGBA.Text.Split(',');
                            int row = sArray.GetLength(0);  //第一维的长度（即行数），结果为2
                            if (row == 4)
                            {
                                if (int.Parse(sArray[0]) > 255 || int.Parse(sArray[1]) > 255 || int.Parse(sArray[2]) > 255 || double.Parse(sArray[3]) <= 1? (double.Parse(sArray[3]) * 255) > 255 : int.Parse(sArray[3]) > 255)
                                {
                                    Error = true;
                                    error.ErrorTo("发现错误（ECC - 003）：输入内容无效，请检查。错误内容为RGBA值大于255。", Percent, Errorsay);
                                    break;
                                }
                                r = byte.Parse(sArray[0]);
                                g = byte.Parse(sArray[1]);
                                b = byte.Parse(sArray[2]);
                                if(double.Parse(sArray[3]) <= 1)
                                {
                                    a = Convert.ToByte(double.Parse(sArray[3]) * 255);
                                }
                                else
                                {
                                    a = byte.Parse(sArray[3]);
                                }

                                ChangeColor.Color = System.Windows.Media.Color.FromArgb(a, r, g, b);
                                SolidColorBrush CardColor = new SolidColorBrush();
                                if (a != 255)
                                {
                                    CardColor.Color = System.Windows.Media.Color.FromArgb(0, 255, 255, 255);
                                }
                                else
                                {
                                    CardColor.Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);
                                }
                                String aa = RGBATo(r, g, b, a, 1);
                                ColorList[0] = aa.ToUpper();
                                this.ColorCard.Background = CardColor;
                                this.Color.Fill = ChangeColor;
                            }
                            else
                            {
                                Error = true;
                                error.ErrorTo("发现错误（ECC - 003）：输入内容无效，请检查。错误内容为……你瞎几把输入了些什么玩意。", Percent, Errorsay);
                                break;
                            }
                        }
                        break;
                    default:
                        {
                            Error = true;
                            error.ErrorTo("发现错误（ECC - 004）：在判断输入时发生未知错误,错误变量为which，数值为" + which + "，错误类型为无法匹配。", Percent, Errorsay);
                            MessageBoxX.Show("发现错误（ECC - 004），是否反馈。", "错误", Application.Current.MainWindow);
                        }
                        break;
                }
                ProgressBarHelper.SetAnimateTo(Percent, 100 / 3 * 2);
            }
            ButtonHelper.SetIsWaiting(RunButton, false);
            this.J16.Text = ColorList[0];
            this.RGBA.Text = ColorList[1];
            if (!Error)
            {
                this.Errorsay.Visibility = Visibility.Collapsed;
                this.RunCard.Visibility = Visibility.Collapsed;
                this.Percent.Foreground = ColorFst;
                ProgressBarHelper.SetAnimateTo(Percent, 0);
            }
            Error = false;
            ErrorString = "";
            which = 0;
            ColorList = new string[2];
        }

        private void Clear(object sebder, RoutedEventArgs s)
        {
            this.J16.Text = "";
            this.RGBA.Text = "";

            this.Errorsay.Visibility = Visibility.Collapsed;
            this.RunCard.Visibility = Visibility.Collapsed;
            this.Percent.Foreground = ColorFst;
            ProgressBarHelper.SetAnimateTo(Percent, 0);
        }

        private int[] toRGBA(String InPut, int Mode)
        {
            int[] Color = new int[5];
            switch (Mode)
            {
                case 1:     //十六进制
                    {
                        if (InPut.Length == 9)
                        {
                            Color[0] = Convert.ToInt32(InPut.Substring(1, 2), 16);
                            Color[1] = Convert.ToInt32(InPut.Substring(3, 2), 16);
                            Color[2] = Convert.ToInt32(InPut.Substring(5, 2), 16);
                            Color[3] = Convert.ToInt32(InPut.Substring(7, 2), 16);
                        }
                        else
                        {
                            Color[0] = 255;
                            Color[1] = Convert.ToInt32(InPut.Substring(1, 2), 16);
                            Color[2] = Convert.ToInt32(InPut.Substring(3, 2), 16);
                            Color[3] = Convert.ToInt32(InPut.Substring(5, 2), 16);
                        }
                        if (Color[0] > 255 && Color[1] > 255 && Color[2] > 255)
                        {
                            Color[4] = -1;
                            error.ErrorTo("发现错误（ECC - 003）：输入内容无效，请检查。错误内容为十六进制值大于F", Percent, Errorsay);
                            return Color;
                        }
                        if(InPut.Length == 9)
                        {
                             if(Color[3] > 255)
                            {
                                Color[4] = -1;
                                error.ErrorTo("发现错误（ECC - 003）：输入内容无效，请检查。错误内容为十六进制值大于F", Percent, Errorsay);
                                return Color;
                            }
                        }
                        Color[4] = 1;
                    }
                    break;

                case 2:     //RGBA
                    {
                        Color[4] = -1;
                        error.ErrorTo("发现错误（ECC - 004）：在判断输入时发生未知错误,错误变量为Mode，数值为" + Mode + "，错误类型为无法匹配。", Percent, Errorsay);
                        return Color;
                    }
            }
            return Color;
        }

        private String RGBATo (int r, int g, int b, int a, int mode)
        {
            String RET = "";
            switch(mode)
            {
                case 1:     //十六进制
                    {
                        String a1, r1, g1, b1;
                        char[] zero = new char[2];
                        zero[0] = zero[1] = '0';
                        if(a <= 1)
                        {
                            a = 255 * a;
                        }
                        a1 = Convert.ToString(a, 16);
                        r1 = Convert.ToString(r, 16);
                        g1 = Convert.ToString(g, 16);
                        b1 = Convert.ToString(b, 16);
                        if(a == 0)
                        {
                            a1 = new string(zero);
                        }
                        else if(a < 16)
                        {
                            a1 = "0" + a1;
                        }
                        if (r == 0)
                        {
                            r1 = new string(zero);
                        }
                        else if (r < 16)
                        {
                            r1 = "0" + r1;
                        }
                        if (g == 0)
                        {
                            g1 = new string(zero);
                        }
                        else if (g < 16)
                        {
                            g1 = "0" + g1;
                        }
                        if (b == 0)
                        {
                            b1 = new string(zero);
                        }
                        else if (b < 16)
                        {
                            b1 = "0" + b1;
                        }
                        return "#" + a1.ToUpper() + r1.ToUpper() + g1.ToUpper() + b1.ToUpper();
                    }
            }
            return RET;
        }

        void UIElement_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scroll = (ScrollViewer)sender;
            if (e.Delta > 0)
            {
                // 向上滚动
                scroll.LineUp();
                scroll.LineUp();
            }
            if (e.Delta < 0)
            {
                // 向下滚动
                scroll.LineDown();
                scroll.LineDown();
            }
        }

        private void J16_GotFocus(object sender, RoutedEventArgs e)
        {
            RGBA.Text = "";
        }

        private void RGBA_GotFocus(object sender, RoutedEventArgs e)
        {
            J16.Text = "";
        }

        private void UpdateColor(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SolidColorBrush ChangeColor = new SolidColorBrush();
            ChangeColor.Color = System.Windows.Media.Color.FromArgb(Convert.ToByte(255), Convert.ToByte(S1.Value), Convert.ToByte(S2.Value), Convert.ToByte(S3.Value));
            Color.Fill = ChangeColor;
            RGBA.Text = Convert.ToInt32(S1.Value) + "," + Convert.ToInt32(S2.Value) + "," + Convert.ToInt32(S3.Value) + ",255";
            J16.Text = RGBATo(Convert.ToInt32(S1.Value), Convert.ToInt32(S2.Value), Convert.ToInt32(S3.Value), 255, 1);
        }
    }
}
