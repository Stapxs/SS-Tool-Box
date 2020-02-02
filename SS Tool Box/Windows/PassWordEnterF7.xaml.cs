using System.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Panuon.UI.Silver;
using SS_Tool_Box.Classes;
using System.Timers;
using System.Windows.Threading;
using SS_Tool_Box_By_WPF;
using System.Windows.Media;

namespace SS_Tool_Box
{
    /// <summary>
    /// PassWordEnterF7.xaml 的交互逻辑
    /// </summary>
    public partial class PassWordEnterF7 : WindowX
    {
        BaseColor baseColora = Main.baseColor;
        private delegate void TimerDispatcherDelegate();
        int timePass = 0;

        public PassWordEnterF7()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            BG.BeginInit();
            BG.Source = baseColora.Bgpa;
            BG.EndInit();
            this.Background = baseColora.Tran;
            WindowXCaption.SetHeight(this, 0);

            this.Time.Background = baseColora.DBg;
            this.Time.Foreground = baseColora.Main;
            ProgressBarHelper.SetCornerRadius(Time, 0);
            Border.Background = baseColora.DBg;

            this.Password.Background = baseColora.DBg;
            this.Password.Foreground = baseColora.Fg;

            ButtonHelper.SetHoverBrush(B1, baseColora.Tran);
            this.B1.Background = baseColora.Tran;

            Timer timer = new Timer();
            timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new TimerDispatcherDelegate(updateUI));
        }

        private void updateUI()
        {
            timePass++;
            ProgressBarHelper.SetAnimateTo(Time, timePass * 10);
            if(timePass == 11)
            {
                if (!String.IsNullOrWhiteSpace(Password.Password))
                {
                    if (Password.Password.Equals(Main.Settings["Features"]["Privacy"]["Password"].ToString()))
                    {
                        LoadingSetter.PasswordPass = true;
                    }
                    else
                    {
                        LoadingSetter.PasswordPass = false;
                    }
                }
                else
                {
                    LoadingSetter.PasswordPass = false;
                }
                ForceClose();
            }
        }

        private void Onsizechanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Rect r = new System.Windows.Rect(e.NewSize);
            int radius = 5;
            RectangleGeometry gm = new RectangleGeometry(r, radius, radius);
            ((UIElement)sender).Clip = gm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(Password.Password))
            {
                if(Password.Password.Equals(Main.Settings["Features"]["Privacy"]["Password"].ToString()))
                {
                    LoadingSetter.PasswordPass = true;
                    ForceClose();
                }
                else
                {
                    Password.Password = "";
                    LoadingSetter.PasswordPass = false;
                }
            }
            else
            {
                Password.Password = "";
                LoadingSetter.PasswordPass = false;
            }
        }
    }
}
