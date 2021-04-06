using SS_Tool_Box.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
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
using WindowsInput;
using WindowsInput.Native;

namespace SS_Tool_Box.Pages.Tools
{
    /// <summary>
    /// GenshinPlay.xaml 的交互逻辑
    /// </summary>
    public partial class GenshinPlay : Page
    {
        public GenshinPlay()
        {
            InitializeComponent();
        }

        #region 事件 | 提权

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            fullButton.Visibility = Visibility.Visible;
            //// 判断权限
            //WindowsIdentity current = WindowsIdentity.GetCurrent();
            //WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
            //if (!windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
            //{
            //    // 提权
            //    MainWindow.main.MsgAdd(
            //        new List<string> { "自动奏谱", "由于原神窗口拥有管理员权限，故本程序将重启并提高权限用于向原神窗口发送按键事件。" },
            //        new List<string> { "知道了" }, null);
            //    ProcessStartInfo psi = new ProcessStartInfo();
            //    psi.FileName = Process.GetCurrentProcess().MainModule.FileName;
            //    psi.Verb = "runas";
            //    try
            //    {
            //        Process.Start(psi);
            //        Application.Current.Shutdown();
            //    }
            //    catch (Exception ex)
            //    {
            //        MainWindow.main.MsgAdd(
            //            new List<string> { "自动奏谱", "提权失败：" + ex },
            //            new List<string> { "知道了" }, null);
            //    }
            //}
        }

        #endregion

        private void run()
        {
            InputSimulator keyIn = new InputSimulator();
            keyIn.Keyboard.KeyPress(VirtualKeyCode.VK_E);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        class keyVer
        {
            private string name;
            private VirtualKeyCode key;
        }

        private void Button_Click_Full(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            button.Visibility = Visibility.Collapsed;
            NewWindow newWindow = new NewWindow();
            newWindow.page = MainWindow.main.MainCol.Content;
            newWindow.Show();
            MainWindow.main.changePage(new Full(), MainWindow.main.MainTitle.Text);
            MainWindow.main.WindowState = WindowState.Minimized;
        }
    }
}