using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Panuon.UI.Silver;
using SS_Tool_Box;

namespace SS_Tool_Box_By_WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowX
    {
        int NowPage = 0;

        public class Update
        {
            public string Line { get; set; }
        }

        public MainWindow()
        {
            //初始化窗口
            //添加程序集解析事件  
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();

            //版本号
            String stVersion = "Version - Dev.0.1.1";
            Versionon.Text = stVersion;

            //加载颜色笔刷
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, 100, 100, 100);

            //工具列表
            Update[] listall = new Update[]
            {
                new Update(){Line="    1 . 批量打开文件"},
                new Update(){Line="    2 . 颜色转换"},
            };
            UpdateList.ItemsSource = listall;

            //工具列表样式
            this.UpdateList.Foreground = mySolidColorBrush;
            this.UpdateList.FontFamily = new FontFamily("方正兰亭简黑");
            this.UpdateList.FontSize = 14;

            //版本号样式
            this.Versionon.Foreground = mySolidColorBrush;
            this.Versionon.FontFamily = new FontFamily("方正兰亭简黑");
            this.Versionon.FontSize = 12;

            //顶栏按钮样式
            this.Feedback.Foreground = mySolidColorBrush;
            this.Feedback.FontFamily = new FontFamily("方正兰亭简黑");
            this.Feedback.FontSize = 14;

            //加载主页
            PageMain main = new PageMain();
            Page.Content = new Frame()
            {
                Content = main
            };

            //加载完成
            ButtonHelper.SetIsWaiting(OpenButton, false);

        }

        private void ListMenu_Open(object sebder, RoutedEventArgs s)
        {
            Update upd = UpdateList.SelectedItem as Update;
            if (upd != null && upd is Update)
            {
                if (upd.Line == "    1 . 批量打开文件")
                {

                }
                //打开窗口OpenInWindow
                OpennInWindow openInWindow = new OpennInWindow();
                openInWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                openInWindow.ShowDialog();
            }
        }

        private void Button_Open(object sender, RoutedEventArgs e)
        {
            Update upd = UpdateList.SelectedItem as Update;
            if (upd != null && upd is Update)
            {
                if (upd.Line == "    1 . 批量打开文件")
                {
                    NowPage = 1;
                    Page1 page1 = new Page1();
                    Page.Content = new Frame()
                    {
                        Content = page1
                    };
                }
                else
                {
                    PageNone pageNone = new PageNone();
                    Page.Content = new Frame()
                    {
                        Content = pageNone
                    };
                }
            }
        }

        private void Button_About(object sender, RoutedEventArgs e)
        {
            //打开窗口About
            About about = new About();
            about.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            about.Owner = this;
            IsMaskVisible = true;
            about.ShowDialog();
            IsMaskVisible = false;
        }
    }
}
