using SS_Tool_Box.Controls;
using SS_Tool_Box.Function;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SS_Tool_Box.Pages
{
    /// <summary>
    /// None.xaml 的交互逻辑
    /// </summary>
    public partial class None : Page
    {

        public None()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            pageName.Text = MainWindow.main.MainTitle.Text;

            // 随机抽取三个工具
            Random rd = new Random();
            int[] nums = new int[] { -1, -1, -1 };
            UI.Tools tools = new UI.Tools();
            int isSInt = rd.Next(0, 10);
            bool isShowHidden = isSInt == 0;
            Log.AddLog("None", "隐藏常数：" + isSInt);
            for (int i=0; i<3; i++)
            {
                nums[i] = rd.Next(0, tools.List.Count);
                for (int j=0; j<3; j++)
                {
                    if ((nums[j] == nums[i] && i != j) || tools.List[nums[i]].page == null || (tools.List[nums[i]].type == "Hidden" && !isShowHidden))
                    {
                        if(tools.List[nums[i]].type == "Hidden" && !isShowHidden)
                        {
                            Log.AddLog("None", "抽到了一个隐藏工具哦，但是很可惜没有抽到允许显示 XD");
                        }
                        i--;
                        break;
                    }
                }
            }
            for (int i = 0; i < 3; i++)
            {
                SortCard card = new SortCard(tools.List[nums[i]].cardInfo, tools.List[nums[i]].page, MainWindow.main)
                {
                    Margin = new Thickness(0, 0, 17, 15)
                };
                CardSt.Children.Add(card);
            }
        }
    }
}
