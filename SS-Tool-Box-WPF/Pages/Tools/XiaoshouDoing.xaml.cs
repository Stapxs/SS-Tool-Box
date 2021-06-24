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

namespace SS_Tool_Box.Pages.Tools
{
    /// <summary>
    /// XiaoshouDoing.xaml 的交互逻辑
    /// </summary>
    public partial class XiaoshouDoing : Page
    {

        private string version { get; set; } = "1.7.0";

        public XiaoshouDoing()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            viewVersion.Text = version;
        }
    }
}
