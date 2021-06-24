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
    /// SeachToolView.xaml 的交互逻辑
    /// </summary>
    public partial class SeachToolView : UserControl
    {

        public string Title
        {
            set => title.Text = value;
        }

        public string Svg
        {
            set => SvgPic.Data = Geometry.Parse(value);
        }

        public SeachToolView()
        {
            InitializeComponent();
        }

        private object mpage = null;
        private string mmtitle = "";
        private MainWindow ParentWindow = null;

        public SeachToolView(string mtitle, string msvg, object page, MainWindow main)
        {
            InitializeComponent();

            mpage = page;
            mmtitle = mtitle;
            ParentWindow = main;

            title.Text = mtitle;
            SvgPic.Data = Geometry.Parse(msvg);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(mpage != null)
            {
                ParentWindow.changePage(mpage, mmtitle);
            }
        }
    }
}
