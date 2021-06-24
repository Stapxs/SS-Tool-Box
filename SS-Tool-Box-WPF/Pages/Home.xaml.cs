using SS_Tool_Box.Classes.Structure;
using SS_Tool_Box.Controls;
using SS_Tool_Box.Helper;
using System.Windows;
using System.Windows.Controls;

namespace SS_Tool_Box
{
    /// <summary>
    /// Home.xaml 的交互逻辑
    /// </summary>
    public partial class Home : Page
    {
        public MainWindow ParentWindow { get; set; }
        private int SortIndex;

        public Home(int SortIndex)
        {
            InitializeComponent();

            this.SortIndex = SortIndex;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ToolHelper toolHelper = new ToolHelper();
            Cards.Children.Clear();
            int num = 0;
            string type = "";

            // 获取当前显示的类型
            foreach(SortInfo sort in toolHelper.Sorts)
            {
                if(SortIndex == sort.index)
                {
                    type = sort.name;
                    break;
                }
            }
            main_title.Text = (string)Application.Current.FindResource("sort_type_" + type);

            // 创建卡片
            StackPanel nowStakp = new StackPanel();
            foreach (ToolInfo info in toolHelper.Tools)
            {
                Application app = Application.Current;
                if ((string)app.Resources["tool_title_" + info.Name] != null)
                {
                    info.Info.Name = (string)app.Resources["tool_title_" + info.Name];
                    info.Info.Info = (string)app.Resources["tool_sub_" + info.Name];
                }

                if (info.Type == type)
                {
                    if (num % 3 == 0)
                    {
                        nowStakp = new StackPanel();
                        nowStakp.Orientation = Orientation.Horizontal;
                        Cards.Children.Add(nowStakp);
                    }
                    SortCard card = new SortCard(info.Info, info.Page, MainWindow.main)
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
