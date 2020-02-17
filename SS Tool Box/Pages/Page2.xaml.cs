using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            this.CD1.Background = baseColora.Card;
            this.CD2.Background = baseColora.Card;

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
                            
                            if (this.J16.Text[0] != '#' || this.J16.Text.Length != 9)
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
                                if (int.Parse(sArray[0]) > 255 || int.Parse(sArray[1]) > 255 || int.Parse(sArray[2]) > 255 || int.Parse(sArray[3]) > 255)
                                {
                                    Error = true;
                                    error.ErrorTo("发现错误（ECC - 003）：输入内容无效，请检查。错误内容为RGBA值大于255。", Percent, Errorsay);
                                    break;
                                }
                                r = byte.Parse(sArray[0]);
                                g = byte.Parse(sArray[1]);
                                b = byte.Parse(sArray[2]);
                                a = byte.Parse(sArray[3]);

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
                        Color[0] = Convert.ToInt32(InPut.Substring(1, 2), 16);
                        Color[1] = Convert.ToInt32(InPut.Substring(3, 2), 16);
                        Color[2] = Convert.ToInt32(InPut.Substring(5, 2), 16);
                        Color[3] = Convert.ToInt32(InPut.Substring(7, 2), 16);
                        if (Color[0] > 255 && Color[1] > 255 && Color[2] > 255 && Color[3] > 255)
                        {
                            Color[4] = -1;
                            error.ErrorTo("发现错误（ECC - 003）：输入内容无效，请检查。错误内容为十六进制值大于F", Percent, Errorsay);
                            return Color;
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
                        a1 = Convert.ToString(a, 16);
                        r1 = Convert.ToString(r, 16);
                        g1 = Convert.ToString(g, 16);
                        b1 = Convert.ToString(b, 16);
                        if(a == 0)
                        {
                            a1 = new string(zero);
                        }
                        if (r == 0)
                        {
                            r1 = new string(zero);
                        }
                        if (g == 0)
                        {
                            g1 = new string(zero);
                        }
                        if (b == 0)
                        {
                            b1 = new string(zero);
                        }
                        return "#" + a1 + r1 + g1 + b1;
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

    }
}
