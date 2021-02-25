using SS_Tool_Box.Controls;
using SS_Tool_Box.Function;
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

namespace SS_Tool_Box.Pages.SortPages
{
    /// <summary>
    /// OtherTools.xaml 的交互逻辑
    /// </summary>
    public partial class OtherTools : Page
    {

        public MainWindow ParentWindow { get; set; }

        public OtherTools()
        {
            InitializeComponent();

            int num = 0;
            StackPanel nowStakp = new StackPanel();
            foreach (UI.Tools.ToolVer info in UI.Tools.List)
            {
                if (info.type == "OtherTools")
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
