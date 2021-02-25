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
    /// N2RoomCard.xaml 的交互逻辑
    /// </summary>
    public partial class N2RoomCard : UserControl
    {
        public N2RoomCard()
        {
            InitializeComponent();
        }

        private bool _isOpen = false;

        public N2RoomCard(List<string> info, List<string> tags, string token, bool isOpen)
        {
            InitializeComponent();

            warning.Visibility = Visibility.Collapsed;

            id.Text = info[0];
            title.Text = info[1];
            subtitle.Text = info[2];

            foreach (string tag in tags)
            {
                TextBlock text = new TextBlock();
                text.SetResourceReference(TextBlock.ForegroundProperty, "colorFont");
                text.Text = tag;
                text.Background = new SolidColorBrush(Color.FromArgb(255, 76, 110, 245));
                text.FontSize = 10;
                text.Margin = new Thickness(15, 5, 15, 5);

                Button button = new Button();
                button.SetResourceReference(Button.StyleProperty, "TextButtonEx");
                button.Background = new SolidColorBrush(Color.FromArgb(255, 76, 110, 245));
                button.Margin = new Thickness(0, 0, 3, 3);

                button.Content = text;

                tagView.Children.Add(button);
            }

            _isOpen = isOpen;

            if(isOpen)
            {
                opentext.Text = "关闭房间";
                openbutton.Background = new SolidColorBrush(Color.FromArgb(255, 255, 84, 57)); ;
            }
        }

        private void openbutton_Click(object sender, RoutedEventArgs e)
        {
            // 房间切换事件

        }
    }
}
