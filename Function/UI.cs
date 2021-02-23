using SS_Tool_Box.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SS_Tool_Box.Function
{
    class UI
    {
        /// <summary>
        /// 吐司相关
        /// </summary>
        public class ToastHelper
        {

            private static Queue toastList = new Queue();
            static Toast toast;

            /// <summary>
            /// 吐司管理器
            /// </summary>
            public static void StartShower()
            {
                // 运行吐司显示
                Thread rToast = new Thread(runToast);
                rToast.Start();
            }

            private static void runToast()
            {
                while (!MainWindow.readyExit)
                {
                    while (toastList.Count != 0)
                    {
                        Toast.says = toastList.Dequeue().ToString();
                        Log.AddLog("ui", "显示了一条吐司：" + Toast.says);
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            toast = new Toast();
                            toastList.TrimToSize();
                            toast.Owner = Application.Current.MainWindow;
                            toast.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            toast.Show();
                        });
                        Thread.Sleep(2000);
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            toast.Close();
                        });
                    }
                }
            }

            public static void Add(string says)
            {
                toastList.Enqueue(says);
            }
        }
    }
}
