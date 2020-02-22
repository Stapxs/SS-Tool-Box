using Panuon.UI.Silver;
using SS_Tool_Box.Classes;
using SS_Tool_Box_By_WPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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
using System.Windows.Threading;
using static SS_Tool_Box.SSMessageBox;

namespace SS_Tool_Box
{
    /// <summary>
    /// Updater.xaml 的交互逻辑
    /// </summary>
    public partial class Updater : WindowX
    {
        BaseColor baseColora = Main.baseColor;
        Error error = new Error();

        public String url { get; set; }
        public Main ParentWindow { get; set; }

        public Updater()
        {
            InitializeComponent();

            BG.BeginInit();
            BG.Source = baseColora.Bgpa;
            BG.EndInit();
            this.Background = baseColora.Tran;
            WindowXCaption.SetHeight(this, 0);

            this.MT1.Foreground = baseColora.Fg;
            this.MT1.FontFamily = baseColora.Fonts;
            this.MT1.FontSize = 15;

            this.T1.Foreground = baseColora.Fg;
            this.T1.FontFamily = baseColora.Fonts;
            this.T1.FontSize = 13;
            this.T2.Foreground = baseColora.Fg;
            this.T2.FontFamily = baseColora.Fonts;
            this.T2.FontSize = 13;

            this.ICON.Foreground = baseColora.Fg;
            ButtonHelper.SetHoverBrush(ICON, baseColora.Fg);

            this.pbDown.Background = baseColora.DBg;
            this.pbDown.Foreground = baseColora.Main;
            ProgressBarHelper.SetCornerRadius(pbDown, 0);

            Thread thread = new Thread(Start);
            thread.Start();
        }

        private long Max = 0;
        private void Start()
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse respone = request.GetResponse();
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                pbDown.Maximum = respone.ContentLength;
                Max = respone.ContentLength;
            });
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                Stream netStream = respone.GetResponseStream();
                Stream fileStream = new FileStream(@"[Upd]SSTB.exe", FileMode.Create);
                byte[] read = new byte[1024];
                long progressBarValue = 0;
                int realReadLen = netStream.Read(read, 0, read.Length);
                while (realReadLen > 0)
                {
                    fileStream.Write(read, 0, realReadLen);
                    progressBarValue += realReadLen;
                    pbDown.Dispatcher.BeginInvoke(new ProgressBarSetter(SetProgressBar), progressBarValue);
                    realReadLen = netStream.Read(read, 0, read.Length);
                }
                netStream.Close();
                fileStream.Close();
            }, null);
        }

        public delegate void ProgressBarSetter(double value);
        public void SetProgressBar(double value)
        {
            ProgressBarHelper.SetAnimateTo(pbDown, value);
            T2.Text = Convert.ToInt32(value / Max * 100) + " %";
            if(value < Max)
            {
                T1.Text = "正在下载……";
            }
            if(Convert.ToInt32(value / Max * 100) >= 100)
            {
                T1.Text = "正在生成更新脚本……";
                Thread thread = new Thread(RunUpdate);
                thread.Start();
            }
        }

        private void RunUpdate()
        {
            Thread.Sleep(2000);
            FileInfo fileInfo = null;
            try
            {
                fileInfo = new FileInfo(@"[Upd]SSTB.exe");
            }
            catch (Exception ex)
            {
                error.logWriter("检查更新文件大小错误：" + ex, false);
                ErrorMSG(ex.ToString(), "在检查文件大小时出错。");
                return;
            }
            if (fileInfo != null && fileInfo.Exists)
            {
                if(fileInfo.Length != Max)
                {
                    error.logWriter("更新文件大小不符。", false);
                    ErrorMSG("", "\n下载完成的文件与目标大小不一致。");
                    return;
                }
                else
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if(File.Exists("UpdateBash.bat"))
                        {
                            File.Delete("UpdateBash.bat");
                        }
                        try
                        {
                            String MyName = Assembly.GetEntryAssembly().Location.Substring(Assembly.GetEntryAssembly().Location.LastIndexOf("\\") + 1);
                            File.AppendAllText("UpdateBash.bat", ":: 这是 SS Tool Box 自动生成的更新批处理");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "@echo off");
                            File.AppendAllText("UpdateBash.bat", "if \" % 1\" == \"h\" goto begin");
                            File.AppendAllText("UpdateBash.bat", "mshta vbscript:createobject(\"wscript.shell\").run(\" % ~nx0 h\",0)(window.close)&&exit");
                            File.AppendAllText("UpdateBash.bat", ":begin");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "del \"" + MyName + "\"");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "ping 127.1 -n 2");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "ren [Upd]SSTB.exe \"" + MyName + "\"");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "ping 127.1 -n 2");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "start \"\" \"" + MyName + "\"");
                        }
                        catch (Exception ex)
                        {
                            error.logWriter("生成更新脚本错误。" + ex, false);
                            ErrorMSG(ex.ToString(), "生成更新脚本错误。");
                            return;
                        }
                        if (File.Exists("run.vbs"))
                        {
                            File.Delete("run.vbs");
                        }
                        try
                        {
                            File.AppendAllText("run.vbs", "set ws=WScript.CreateObject(\"WScript.Shell\")");
                            File.AppendAllText("run.vbs", "\r\n" + "ws.Run \"UpdateBash.bat\", 0");
                        }
                        catch (Exception ex)
                        {
                            error.logWriter("生成更新脚本错误。" + ex, false);
                            ErrorMSG(ex.ToString(), "生成更新脚本错误。");
                            return;
                        }
                        Main.UpdateFin = true;
                        ForceClose();
                    }), DispatcherPriority.SystemIdle, null);
                }
            }
            else
            {
                error.logWriter("检查更新文件路径不存在。", false);
                ErrorMSG("", "未找到下载完成的更新文件。");
                return;
            }
        }

        private void ErrorMSG(String ex, String say)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                SSMessageHelper.noNo = false;
                ButtonHelper.SetIcon(SSMessageHelper.Icon, "");
                SSMessageHelper.Title = "更新失败";
                SSMessageHelper.bNOtext = "不用不用";
                SSMessageHelper.bOKtext = "打开日志";
                SSMessageHelper.Says = "在更新时发生了错误，更新失败。" + say + "\n" + ex;
                SSMessageBox MB = new SSMessageBox();
                ParentWindow.IsMaskVisible = true;
                MB.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                MB.Owner = ParentWindow;
                MB.ShowDialog();
                ParentWindow.IsMaskVisible = false;

                if (SSMessageHelper.buttonOK)
                {
                    string where = Directory.GetCurrentDirectory();
                    where = where + @"\SSTB\Log\log.log";
                    System.Diagnostics.Process process;
                    process = System.Diagnostics.Process.Start(@where);
                }
                ForceClose();
            }), DispatcherPriority.SystemIdle, null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ForceClose();
        }

        private void Onsizechanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Rect r = new System.Windows.Rect(e.NewSize);
            int radius = 5;
            RectangleGeometry gm = new RectangleGeometry(r, radius, radius);
            ((UIElement)sender).Clip = gm;
        }
    }
}
