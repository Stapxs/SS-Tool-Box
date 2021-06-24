using SS_Tool_Box.Windows;
using System;
using System.Collections;
using System.Threading;
using System.Windows;

namespace SS_Tool_Box.Helper
{
    class Toast
    {
        public static Queue toastList = new Queue();
        static Windows.Toast toast;

        /// <summary>
        /// 吐司管理器
        /// </summary>
        public void StartShower()
        {
            // 运行吐司显示
            Thread rToast = new Thread(runToast);
            MainWindow.threads.Push(rToast);
            rToast.Start();
        }

        private void runToast()
        {
            while (!MainWindow.readyExit)
            {
                while (toastList.Count != 0)
                {
                    Show(toastList.Dequeue().ToString());
                }
            }
        }

        public void Show(string says)
        {
            Windows.Toast.says = says;
            Log.AddLog("ui", "显示了一条吐司：" + Windows.Toast.says);
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                toast = new Windows.Toast();
                toastList.TrimToSize();
                toast.Owner = Application.Current.MainWindow;
                toast.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                toast.Show();
            });
            Thread.Sleep(2000);
            if (!MainWindow.readyExit)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    toast.Close();
                });
            }
        }

        public static void Add(string says)
        {
            toastList.Enqueue(says);
        }
    }
}
