using SS_Tool_Box.Classes.Structure;
using SS_Tool_Box.Controls;
using SS_Tool_Box.Function;
using SS_Tool_Box.Helper;
using System;
using System.Collections.Generic;
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
            int isSInt = rd.Next(0, 10);
            bool isShowHidden = isSInt == 0;
            Log.AddLog("None", "隐藏常数：" + isSInt);

            ToolList toolHelper = new ToolList();
            for (int i = 0; i < 3; i++)
            {
                nums[i] = rd.Next(0, toolHelper.Tools.Count);

                for (int j = 0; j < 3; j++)
                {
                    if ((nums[j] == nums[i] && i != j) || toolHelper.Tools[nums[i]].Page == null)
                    {
                        i--;
                        break;
                    }
                    if (toolHelper.Tools[nums[i]].Type == "Hidden" && !isShowHidden)
                    {
                        Log.AddLog("None", "抽到了一个隐藏工具哦，但是很可惜没有抽到允许显示 XD");
                        i--;
                        break;
                    }
                }
            }
            for (int i = 0; i < 3; i++)
            {
                Application app = Application.Current;
                ToolInfo.CardInfo info = toolHelper.Tools[i].Info;
                info.Name = (string)app.Resources["tool_title_" + toolHelper.Tools[i].Name];
                info.Info = (string)app.Resources["tool_sub_" + toolHelper.Tools[i].Name];
                SortCard card = new SortCard(info, toolHelper.Tools[i].Page, MainWindow.main)
                {
                    Margin = new Thickness(0, 0, 17, 15)
                };
                CardSt.Children.Add(card);
            }
        }
    }
}
