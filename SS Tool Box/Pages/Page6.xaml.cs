using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Panuon.UI.Silver;
using System.Net;
using SS_Tool_Box.Classes;
using Newtonsoft.Json.Linq;
using SS_Tool_Box_By_WPF;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;

namespace SS_Tool_Box
{
    /// <summary>
    /// Page6.xaml 的交互逻辑
    /// </summary>
    public partial class Page6 : Page
    {

        BaseColor baseColora = Main.baseColor;

        bool hasCard = false;
        int allCards = 0;

        public Page6()
        {
            InitializeComponent();

            this.Height = 477;

            String stTitle = "QQ 消息生成";
            this.Title.Foreground = baseColora.Fg;
            this.Title.FontFamily = baseColora.Fonts; ;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            String stSays = "生成一份由控件拼接而成的QQ聊天内容。";
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
            this.T5.Foreground = baseColora.Fg;
            this.T5.FontFamily = baseColora.Fonts;
            this.T5.FontSize = 13;

            this.CD1.Background = baseColora.Card;
            this.CD2.Background = baseColora.Card;
            this.RunCard.Background = baseColora.Card;

            this.BG.Background = baseColora.Bg;
            this.BG.Foreground = baseColora.Fg;
            this.QQ.Background = baseColora.Bg;
            this.QQ.Foreground = baseColora.Fg;
            this.QQName.Background = baseColora.Bg;
            this.QQName.Foreground = baseColora.Fg;
            this.QQSay.Background = baseColora.Bg;
            this.QQSay.Foreground = baseColora.Fg;

            this.RunCard.Visibility = Visibility.Collapsed;
            this.Errorsay.Visibility = Visibility.Collapsed;

            this.RunButton.Foreground = baseColora.Fg;
            this.RunButton.Background = baseColora.Tran;

            this.PopSet.Background = baseColora.Bg;
            this.PopSet.Foreground = baseColora.Fg;
            ComboBoxHelper.SetShadowColor(PopSet, baseColora.Bg.Color);
            ComboBoxHelper.SetHoverBackground(PopSet, baseColora.Bg);
            ComboBoxHelper.SetHoverForeground(PopSet, baseColora.Fg);
            ComboBoxHelper.SetSelectedBackground(PopSet, baseColora.Bg);
            ComboBoxHelper.SetSelectedForeground(PopSet, baseColora.Fg);
        }

        private void IsMain_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void PopSet_DropDownClosed(object sender, EventArgs e)
        {

        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if(CD2.Visibility == Visibility.Collapsed)
            {
                CD2.Visibility = Visibility.Visible;
            }
            else
            {
                CD2.Visibility = Visibility.Collapsed;
                //新建卡片
                if (!hasCard)
                {
                    hasCard = true;
                    Card card = new Card();
                    card.Margin = new Thickness(5, 10, 20, 25);
                    card.Width = 540;
                    card.Background = baseColora.Card;
                    StackPanel stack = new StackPanel();
                    stack.Margin = new Thickness(10);
                    stack.Name = "MainList";
                    RegisterName("MainList", stack);
                    card.Content = stack;
                }
                //新建对话
                WrapPanel wrap = new WrapPanel();       //头像和名字
                wrap.Margin = new Thickness(36);
                Image image = new Image();              //头像
                RectangleGeometry rectangle = new RectangleGeometry();
                rectangle.RadiusX = rectangle.RadiusY = 15;
                rectangle.Rect = new Rect(6,6,0,0);
                image.Clip = rectangle;
                string urls = "http://q1.qlogo.cn/g?b=qq&nk=" + QQ.Text + "&s=3";
                image.Source = new BitmapImage(new Uri(urls));
                image.Width = image.Height = 30;
                image.Margin = new Thickness(6, 6, 0, 0);
                image.Stretch = Stretch.Fill;
                image.HorizontalAlignment = HorizontalAlignment.Left;
                wrap.Children.Add(image);
                WrapPanel wrapName = new WrapPanel();   //名字
                TextBlock text = new TextBlock();
                text.Margin = new Thickness(15, 8, 0, 0);
                text.VerticalAlignment = VerticalAlignment.Top;
                Action action = new Action(() =>
                {
                    String iduri = "https://api.toubiec.cn/qq?qq=" + QQ.Text + "&size=300";
                    string GetJson = HttpUitls.Get(iduri, "DEFALT");
                    JObject obj = JObject.Parse(GetJson);
                    text.Text = obj["name"].ToString();
                });
                action.BeginInvoke(null, null);
                wrap.Children.Add(text);
                WrapPanel pop = new WrapPanel();      //气泡
                Image imagePopL = new Image();          //左
                image.Width = 17;
                imagePopL.Height = 43;
                imagePopL.Margin = new Thickness(40, -15, 0, 0);
                imagePopL.Source = new BitmapImage(new Uri("/SS Tool Box;component/Resource/Pops/温柔卷纸 - L.png"));
                pop.Children.Add(imagePopL);
                TextBlock textSay = new TextBlock();    //中
                ImageBrush bg = new ImageBrush(); 
                bg.ImageSource = new BitmapImage(new Uri("/SS Tool Box;component/Resource/Pops/温柔卷纸 - M.png"));
                text.Background = bg;
                text.MaxWidth = 200;
                text.Text = QQSay.Text;
                text.FontSize = 17;
                text.Margin = new Thickness(0, -15, 0, 0);
                text.Padding = new Thickness(0, 11, 0, 0);
                pop.Children.Add(text);
                Image imagePopR = new Image();          //右
                imagePopR.Width = 17;
                imagePopR.Height = 49;
                imagePopR.Margin = new Thickness(0, -15, 0, 0);
            }
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
