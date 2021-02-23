using Newtonsoft.Json.Linq;
using SS_Tool_Box.Classes;
using SS_Tool_Box.Controls;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SS_Tool_Box.Pages.Tools
{
    /// <summary>
    /// ColorCard.xaml 的交互逻辑
    /// </summary>
    public partial class ColorCard : Page
    {

        public MainWindow ParentWindow { get; set; }

        public ColorCard()
        {
            InitializeComponent();

            // 获取 Suk 色表
            Thread runGetSukColors = new Thread(GetSukColors);
            MainWindow.threads.Push(runGetSukColors);
            runGetSukColors.Start();
        }

        class colorInfoVer
        {
            public string name;
            public string color;

            public colorInfoVer(string name, string color)
            {
                this.name = name;
                this.color = color;
            }
        }

        private void GetSukColors()
        {
            try
            {
                string url = "https://srv.sukazyo.cc/api/color?version=1";
                string GetJson = HttpUitls.Get(url, "DEFALT");

                JObject obj = JObject.Parse(GetJson);
                JArray jList = JArray.Parse(obj["colors"].ToString());
                List<colorInfoVer> colors = new List<colorInfoVer>();

                for (int i = 0; i < jList.Count; ++i)
                {
                    JObject colorInfo = JObject.Parse(jList[i].ToString());
                    colors.Add(new colorInfoVer(colorInfo["name"].ToString(), colorInfo["color"].ToString()));
                }

                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    foreach (colorInfoVer colorInfo in colors)
                    {
                        ColorView view = new ColorView(colorInfo.name, "#" + colorInfo.color)
                        {
                            Margin = new Thickness(5, 15, 10, 0)
                        };
                        sukColors.Children.Add(view);
                    }

                    sukLoading.Visibility = Visibility.Collapsed;
                    sukColorsEx.IsExpanded = false;
                });
            }
            catch(Exception e)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    sukColorsEx.IsExpanded = true;
                    sukError.Visibility = Visibility.Visible;
                    sukLoading.Visibility = Visibility.Collapsed;
                    sukErrorText.Text = e.Message;
                    Log.AddErr(e.ToString());
                });
            }
        }

        private void sukLink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://srv.sukazyo.cc/style/color");
        }
    }
}
