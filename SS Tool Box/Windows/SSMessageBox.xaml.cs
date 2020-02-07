using Panuon.UI.Silver;
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
using System.Windows.Shapes;

namespace SS_Tool_Box
{
    /// <summary>
    /// SSMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class SSMessageBox : WindowX
    {
        BaseColor baseColora = Main.baseColor;

        public class SSMessageHelper
        {
            public static bool buttonOK = false;
            public static bool buttonNO = false;
            public static Button Icon = new Button();
            public static string Title = "主标题";
            public static string Says = "内容，初始化内容得长到足以可以让我看见自动换行的效果才行，看起来应该够长了吧……emmm再长一点好了，就再长亿点。";
            public static string bOKtext = "好的";
            public static string bNOtext = "不要";
            public static bool noNo = false;
        }

        public SSMessageBox()
        {
            InitializeComponent();

            SSMessageHelper.buttonOK = false;
            SSMessageHelper.buttonNO = false;

            BG.BeginInit();
            BG.Source = baseColora.Bgpa;
            BG.EndInit();
            this.Background = baseColora.Tran;
            Border.Background = baseColora.DBg;
            WindowXCaption.SetHeight(this, 0);

            this.ICON.Foreground = baseColora.Fg;
            ButtonHelper.SetHoverBrush(ICON, baseColora.Fg);
            ButtonHelper.SetIcon(ICON, ButtonHelper.GetIcon(SSMessageHelper.Icon));
            this.OK.Content = SSMessageHelper.bOKtext;
            this.OK.Foreground = baseColora.Fg;
            this.OK.Background = baseColora.Bg;
            ButtonHelper.SetHoverBrush(OK, baseColora.Main);
            ButtonHelper.SetCornerRadius(OK, new CornerRadius(3, 3, 3, 3));
            this.CANC.Content = SSMessageHelper.bNOtext;
            this.CANC.Foreground = baseColora.Fg;
            this.CANC.Background = baseColora.Bg;
            ButtonHelper.SetHoverBrush(CANC, baseColora.Main);
            ButtonHelper.SetCornerRadius(CANC, new CornerRadius(3, 3, 3, 3));

            this.TITLE.Text = SSMessageHelper.Title;
            this.TITLE.Foreground = baseColora.Fg;
            this.TITLE.Background = baseColora.Tran;
            this.TITLE.FontFamily = baseColora.Fonts;
            this.TITLE.FontSize = 25;

            this.SAY.Text = "       " + SSMessageHelper.Says;
            this.SAY.Foreground = baseColora.Fg;
            this.SAY.Background = baseColora.Tran;
            this.SAY.FontFamily = baseColora.Fonts;
            this.SAY.FontSize = 16;

            if(SSMessageHelper.noNo)
            {
                this.CANC.Visibility = Visibility.Collapsed;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            SSMessageHelper.buttonOK = true;
            ForceClose();
        }

        private void CANC_Click(object sender, RoutedEventArgs e)
        {
            SSMessageHelper.buttonNO = true;
            ForceClose();
        }
    }
}
