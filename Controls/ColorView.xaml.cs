using SS_Tool_Box.Function;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SS_Tool_Box.Controls
{
    /// <summary>
    /// ColorView.xaml 的交互逻辑
    /// </summary>
    public partial class ColorView : UserControl
    {

        public SolidColorBrush ColorBrush
        {
            set
            {
                left.Background = value;
                right.Background = value;
                color.Foreground = value;
                name.Foreground = value;

                string colorStr = "#" + new ColorConverter().ConvertToString(value.Color).Substring(3) + " / " +
                    value.Color.R + "," +
                    value.Color.G + "," +
                    value.Color.B;
                color.Text = colorStr;
            }
        }
        public string ColorName
        {
            set => name.Text = value;
        }

        public ColorView()
        {
            InitializeComponent();
        }

        public ColorView(string Name, string Color16)
        {
            InitializeComponent();

            left.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Color16));
            right.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Color16));
            color.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Color16));
            name.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Color16));
            name.Text = Name;
            SolidColorBrush colorSB = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Color16));
            string colorStr = "#" + new ColorConverter().ConvertToString(colorSB.Color).Substring(3) + " / " +
                    colorSB.Color.R + "," +
                    colorSB.Color.G + "," +
                    colorSB.Color.B;
            color.Text = colorStr;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(color.Text);
            UI.ToastHelper.Add("复制成功！");
        }
    }
}
