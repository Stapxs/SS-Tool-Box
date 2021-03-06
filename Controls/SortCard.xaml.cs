using SS_Tool_Box.Pages;
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

        public SortCard()
        {
            InitializeComponent();
        }

        private object pageOpen = null;
        private MainWindow ParentWindow = null;
        private List<string> Info;

        public SortCard(List<string> MInfo, object page, MainWindow Main)
        {
            InitializeComponent();

            if(!(MInfo.Count != 4))
            {
                // 执行初始化
                ParentWindow = Main;
                pageOpen = page;
                Info = MInfo;

                IconBg.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Info[2]));

                MTitle.Text = MInfo[0];
                MSubTitle.Text = MInfo[1];

                SvgPic.Data = Geometry.Parse(MInfo[3]);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (pageOpen != null)
            {
                ParentWindow.changePage(pageOpen, Info[0]);
            }
            else
            { 
                ParentWindow.changePage(new None(), Info[0]);
            }
        }
    }
}
