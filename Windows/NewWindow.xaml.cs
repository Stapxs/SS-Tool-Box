using SS_Tool_Box.Pages;
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
using System.Windows.Shapes;

namespace SS_Tool_Box.Windows
{
    /// <summary>
    /// NewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewWindow : Window
    {

        public object page { get; set; }

        public NewWindow()
        {
            InitializeComponent();
        }

        #region 事件 | 按钮

        private void B_Exit(object sender, RoutedEventArgs e)
        {
            // 关闭
            MainWindow.main.WindowState = WindowState.Normal;
            string title = MainWindow.main.MainTitle.Text;
            MainWindow.main.backHome(false);
            MainWindow.main.changePage(MainCol.Content, title);
            Close();
        }

        private void B_SmallWin(object sender, RoutedEventArgs e)
        {
            // 最小化
            WindowState = WindowState.Minimized;
        }

        private void Drag_MouseMove(object sender, MouseEventArgs e)
        {
            // 鼠标拖动事件
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
            }
            catch
            {
                Log.AddErr("main", "鼠标拖拽执行异常！");
            }
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (page == null)
            {
                None none = new None();
                MainCol.Content = new Frame()
                {
                    Content = none
                };
            }
            else
            {
                MainCol.Content = new Frame()
                {
                    Content = page
                };
            }
        }
    }
}
