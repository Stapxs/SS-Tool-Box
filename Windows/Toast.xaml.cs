using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Shapes;

namespace SS_Tool_Box.Windows
{
    /// <summary>
    /// Toast.xaml 的交互逻辑
    /// </summary>
    public partial class Toast : Window
    {

        public static string says = "";
        public bool stop = false;

        public Toast()
        {
            InitializeComponent();
            Topmost = true;
            Says.Content = says;

            Thread thread = new Thread(runMoveControler);
            thread.Start();
        }

        private void runMoveControler()
        {
            Thread thread = new Thread(runMove);
            thread.Start();
            Thread.Sleep(2000);
            stop = true;
        }

        private void runMove()
        {
            try
            {
                while (!stop)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Top = Owner.Top;
                        Left = Owner.Left;
                    });
                }
            }
            catch(Exception e)
            {
                Log.AddErr("ui", "关闭吐司错误：" + e.ToString());
            }
        }
    }
}
