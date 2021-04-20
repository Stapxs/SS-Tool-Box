using SS_Tool_Box.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
using System.Windows.Threading;

namespace SS_Tool_Box.Pages.AboutPages
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : Page
    {
        public About()
        {
            InitializeComponent();

            // 初始化 Debug 信息
            Thread thread = new Thread(run);
            MainWindow.threads.Push(thread);
            thread.Start();
        }

        private void run()
        {
            string mainStr = "1 - " + MainWindow.verInfo.verStr + "\n" +
                             "2 - " + MainWindow.verInfo.verNum + "\n" +
                             "3 - " + MainWindow.verInfo.verBulidTimes + "\n" +
                             "4 - " + MainWindow.verInfo.verType;
            string mainStr1 = "";
            // Ping Blog
            string ip = null;
            try
            {
                IPAddress[] ipadd = Dns.GetHostAddresses("stapxs.cn");
                for (int i = 0; i <= ipadd.Count() - 1; i++)
                {
                    ip = ipadd[i].ToString();
                }
            }
            catch
            {
                UI.ToastHelper.Add("域名解析错误！");
            }
            if(ip != null)
            {
                PingReply rep = Features.PingTest(ip);
                mainStr1 += "5 - " + rep.Status + "(" + rep.RoundtripTime + ")\n";
            }
            else
            {
                mainStr1 += "5 - err\n";
            }
            // Ping Github Raw
            ip = null;
            try
            {
                IPAddress[] ipadd = Dns.GetHostAddresses("raw.githubusercontent.com");
                for (int i = 0; i <= ipadd.Count() - 1; i++)
                {
                    ip = ipadd[i].ToString();
                }
            }
            catch
            {
                UI.ToastHelper.Add("域名解析错误！");
            }
            if (ip != null)
            {
                PingReply rep = Features.PingTest(ip);
                mainStr1 += "6 - " + rep.Status + "(" + rep.RoundtripTime + ")\n";
            }
            else
            {
                mainStr1 += "6 - err\n";
            }
            // 显示结果
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                InfoBox.Text = mainStr;
                InfoBox1.Text = mainStr1;
            }), DispatcherPriority.SystemIdle, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main.changePage(new license(), App.Current.Resources["about_LICENSE"].ToString());
        }
    }
}
