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
    /// LicenseCard.xaml 的交互逻辑
    /// </summary>
    public partial class LicenseCard : UserControl
    {
        public string Title
        {
            set => Name.Text = value;
        }

        public string Auther
        {
            set => By.Text = value;
        }

        public string License
        {
            set => lis.Text = value;
        }

        public string URL
        {
            set => but.Content = value;
        }

        public LicenseCard()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            System.Diagnostics.Process.Start(bt.Content.ToString());
        }
    }
}
