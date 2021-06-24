using SS_Tool_Box.Classes.Helper;
using SS_Tool_Box.Classes.Structure;
using SS_Tool_Box.Pages;
using SS_Tool_Box.Windows;
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

namespace SS_Tool_Box.Controls
{
    /// <summary>
    /// SortCard.xaml 的交互逻辑
    /// </summary>
    public partial class SortCard : UserControl
    {

        public String Title
        {
            set => MTitle.Text = value;
        }

        public String SubTitle
        {
            set => MSubTitle.Text = value;
        }

        public String Svg
        {
            set => SvgPic.Data = Geometry.Parse(value);
        }

        public SolidColorBrush ColorBrush
        {
            set => IconBg.Background = value;
        }

        private object pageOpen = null;
        private MainWindow ParentWindow = null;
        private ToolInfo.CardInfo Info;

        public SortCard(ToolInfo.CardInfo MInfo, object page, MainWindow Main)
        {
            InitializeComponent();

            // 执行初始化
            ParentWindow = Main;
            pageOpen = page;
            Info = MInfo;

            IconBg.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Info.Color));

            MTitle.Text = MInfo.Name;
            MSubTitle.Text = MInfo.Info;

            SvgPic.Data = Geometry.Parse(MInfo.Icon);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (pageOpen != null)
            {
                WindowsHelper.changePage(pageOpen, Info.Name);
            }
            else
            {
                WindowsHelper.changePage(typeof(None), Info.Name);
            }
        }

        private void Button_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (pageOpen != null)
            {
                NewWindow newWindow = new NewWindow();
                newWindow.page = pageOpen;
                newWindow.Show();
                WindowsHelper.changePage(typeof(Full), Info.Name);
                ParentWindow.WindowState = WindowState.Minimized;
            }
            else
            {
                Log.AddErr("card", "大窗口打开失败：page null");
            }
        }
    }
}
