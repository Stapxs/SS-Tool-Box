using SS_Tool_Box.Controls;
using SS_Tool_Box.Function;
using SS_Tool_Box.Pages.Tools;
using System.Windows;
using System.Windows.Controls;

namespace SS_Tool_Box.Pages.SortPages
{
    /// <summary>
    /// ColorTools.xaml 的交互逻辑
    /// </summary>
    public partial class ColorTools : Page
    {

        public MainWindow ParentWindow { get; set; }

        public ColorTools()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Cards.Children.Clear();
            int num = 0;
            StackPanel nowStakp = new StackPanel();
            UI.Tools tools = new UI.Tools();
            foreach (UI.Tools.ToolVer info in tools.List)
            {
                Application app = Application.Current;
                if ((string)Application.Current.FindResource("tool_title_" + info.name) != null)
                {
                    info.cardInfo[0] = (string)app.Resources["tool_title_" + info.name];
                    info.cardInfo[1] = (string)app.Resources["tool_sub_" + info.name];
                }

                if (info.type == "ColorTools")
                {
                    if (num % 3 == 0)
                    {
                        nowStakp = new StackPanel();
                        nowStakp.Orientation = Orientation.Horizontal;
                        Cards.Children.Add(nowStakp);
                    }
                    SortCard card = new SortCard(info.cardInfo, info.page, MainWindow.main)
                    {
                        Margin = new Thickness(0, 0, 17, 15)
                    };
                    nowStakp.Children.Add(card);
                    num++;
                }
            }
        }
    }
}
