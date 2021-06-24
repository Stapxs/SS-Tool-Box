using SS_Tool_Box.Classes.Structure;
using SS_Tool_Box.Controls;
using SS_Tool_Box.Helper;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

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
            DoubleAnimation opacAnimation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3)
            };
            ThicknessAnimation topAnimation = new ThicknessAnimation
            {
                From = new Thickness(0, -2, 0, 15),
                To = new Thickness(0, 0, 0, 15),
                Duration = TimeSpan.FromSeconds(0.3)
            };
            top.BeginAnimation(MarginProperty, topAnimation);

            ToolList toolHelper = new ToolList();
            Cards.Children.Clear();
            int num = 0;
            int stakpNum = -1;
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
            main_title.BeginAnimation(OpacityProperty, opacAnimation);

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
                        stakpNum++;
                    }
                    SortCard card = new SortCard(info.Info, info.Page, MainWindow.main)
                    {
                        Margin = new Thickness(0, 0, 17, 15)
                    };

                    ThicknessAnimation marginAnimation = new ThicknessAnimation
                    {
                        From = new Thickness(5),
                        To = new Thickness(0, 0, 17, 15),
                        Duration = TimeSpan.FromSeconds(0.2 * (num + 1))
                    };
                    opacAnimation.Duration = TimeSpan.FromSeconds(0.2 * (num + 1));
                    card.BeginAnimation(OpacityProperty, opacAnimation);
                    card.BeginAnimation(MarginProperty, marginAnimation);

                    nowStakp.Children.Add(card);
                    num++;
                }
            }
        }
    }
}
