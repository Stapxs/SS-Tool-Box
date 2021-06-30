using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using Panuon.UI.Silver;
using SS_Tool_Box.Classes.Helper;
using SS_Tool_Box.Classes.Structure;
using SS_Tool_Box.Function;
using SS_Tool_Box.Helper;
using SS_Tool_Box.Pages.AboutPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace SS_Tool_Box.Pages
{
    /// <summary>
    /// Options.xaml 的交互逻辑
    /// </summary>
    public partial class Options : Page
    {

        private bool load = true;
        private bool isUpd = false;
        private bool isUpding = false;

        public Options()
        {
            InitializeComponent();
        }

        #region 事件 | 主题与颜色

        private void darkButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!load)
            {
                new WindowsHelper.Color().ChangeDark(true);
                SS_Tool_Box.Options.SetOpt("darkMode", "true");
            }
        }
        private void darkButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!load)
            {
                new WindowsHelper.Color().ChangeDark(false);
                SS_Tool_Box.Options.SetOpt("darkMode", "false");
            }
        }

        private void autoDarkButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!load)
            {
                darkButton.IsEnabled = false;
                SS_Tool_Box.Options.SetOpt("autoDarkMode", "true");
                // 判断颜色模式
                string isOpen = new Reg().GetRegKey(Registry.CurrentUser, @"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme");
                if (isOpen == "1")
                {
                    new WindowsHelper.Color().ChangeDark(false);
                }
                else
                {
                    new WindowsHelper.Color().ChangeDark(true);
                }
            }
            else
            {
                darkButton.IsEnabled = false;
            }
        }
        private void autoDarkButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!load)
            {
                darkButton.IsEnabled = true;
                SS_Tool_Box.Options.SetOpt("autoDarkMode", "false");
                if (SS_Tool_Box.Options.GetOpt("darkMode")[0] == "false")
                {
                    new WindowsHelper.Color().ChangeDark(false);
                }
                else
                {
                    new WindowsHelper.Color().ChangeDark(true);
                }
            }
            else
            {
                darkButton.IsEnabled = true;
            }
        }

        private void rollButton_Checked(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.Remove("IsInertiaEnabled");
            Application.Current.Resources.Add("IsInertiaEnabled", false);

            SS_Tool_Box.Options.SetOpt("nonLinearScrolling", "true");
        }

        private void rollButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.Remove("IsInertiaEnabled");
            Application.Current.Resources.Add("IsInertiaEnabled", true);

            SS_Tool_Box.Options.SetOpt("nonLinearScrolling", "false");
        }

        private void homeButton_Checked(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Home.Visibility = Visibility.Visible;
            MainWindow.main.Title.Margin = new Thickness(10, 0, 0, 0);
            SS_Tool_Box.Options.SetOpt("alwaysShowHome", "true");
        }

        private void homeButton_Unchecked(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Home.Visibility = Visibility.Collapsed;
            MainWindow.main.Title.Margin = new Thickness(0, 0, 0, 0);
            SS_Tool_Box.Options.SetOpt("alwaysShowHome", "false");
        }

        private void Color_Click(object sender, RoutedEventArgs e)
        {
            if (!load)
            {
                // 处理颜色修改
                RadioButton button = (RadioButton)sender;
                if (button.Name == "FollowSysColor")
                {
                    RegistryKey colorKey = new Reg().GetRegKey(Registry.CurrentUser, @"Software\Microsoft\Windows\DWM", "AccentColor", true);
                    if (colorKey != null)
                    {
                        int accentColor = (int)colorKey.GetValue("AccentColor");
                        Color colorMain = Color.FromArgb(
                            180,
                            (byte)(accentColor & 0xFF),
                            (byte)((accentColor >> 8) & 0xFF),
                            (byte)((accentColor >> 16) & 0xFF));
                        Application.Current.Resources["colorMainBlue"] = new SolidColorBrush(colorMain);
                        Application.Current.Resources["colorSystem"] = new SolidColorBrush(colorMain);
                        SS_Tool_Box.Options.SetOpt("autoColor", "true");
                    }
                }
                else
                {
                    Application.Current.Resources["colorMainBlue"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(button.Background.ToString()));

                    SS_Tool_Box.Options.SetOpt("autoColor", "false");
                    SS_Tool_Box.Options.SetOpt("seleColor", button.ToolTip.ToString());
                }
            }
        }

        #endregion
        #region 事件 | 本土化

        private void LanguageBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!load)
            {
                string selectedName = ((ComboBox)sender).SelectedValue.ToString() + ".xaml";
                string back = "ERR - NONE";

                if (selectedName == "diy.xaml")
                {
                    Application app = Application.Current;
                    OpenFileDialog dialog = new OpenFileDialog();
                    dialog.Multiselect = false;  //该值确定是否可以选择多个文件
                    dialog.Title = app.Resources["options_language_choice"].ToString();
                    dialog.Filter = app.Resources["options_language_file_type"].ToString() + "|*.xaml";
                    dialog.ShowDialog();
                    string file = dialog.FileName;
                    if (file == null || file == "")
                    {
                        return;
                    }
                    file = file.Substring(file.IndexOf("Lang") + 5);
                    SS_Tool_Box.Options.SetOpt("language", "~" + file);
                    selectedName = file;
                    back = new LocalHelper().ChangeLanguage(selectedName, false, true);
                }
                else
                {
                    SS_Tool_Box.Options.SetOpt("language", selectedName);
                    back = new LocalHelper().ChangeLanguage(selectedName, false);
                }
                if (back.IndexOf("ERR") >= 0)
                {
                    Toast.Add(back);
                }
            }
        }


        #endregion
        #region 事件 | 常规

        private void updButton_Click(object sender, RoutedEventArgs e)
        {
            if(isUpding)
            {
                return;
            }
            if(isUpd)
            {
                updUpdate.Visibility = Visibility.Collapsed;
                updWait.Visibility = Visibility.Visible;
                pbDown.Visibility = Visibility.Visible;

                Thread thread = new Thread(Start);
                MainWindow.threads.Push(thread);
                thread.Start();

                Log.AddLog("update", "开始下载更新……");
            }
        }

        private void checkupdButton_Unchecked(object sender, RoutedEventArgs e)
        {
            SS_Tool_Box.Options.SetOpt("stopUpd", "false");
        }

        private void checkupdButton_Checked(object sender, RoutedEventArgs e)
        {
            SS_Tool_Box.Options.SetOpt("stopUpd", "true");
        }

        private void UpdBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox box = (ComboBox)sender;
            SS_Tool_Box.Options.SetOpt("updLink", box.SelectedIndex.ToString());
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            WindowsHelper.changePage(typeof(About), Application.Current.Resources["about_sstb"].ToString());
        }

        #endregion
        #region 事件 | 其他

        private void ShowTost_Click(object sender, RoutedEventArgs e)
        {
            // 随机吐司
            List<string> list = new List<string>(){
                "这个吐司看起来有好好初始化哦。",
                "¯\\_(ツ)_/¯",
                "来个吐司？",
                "吐司超好吃 ——",
                "这是个没有感情的吐司。",
                "Also try LMB Music Box!",
                "Toast",
                "不想说话。",
                "SS!",
                "隔壁小孩都馋哭了。",
                "正在烤吐司。",
                "超文本烤箱控制协议",
                "419 I'm a Microwave Oven.",
                "{\"status\":200, \"message\":\"Bake Finished.\"}"
            };

            Random rd = new Random();

            Toast.Add(list[rd.Next(0, list.Count)]);
        }


        private void ShowMsg_Click(object sender, RoutedEventArgs e)
        {
            string txt = new NetHelper.HttpUitls().Get("https://api.stapxs.cn/SS-Ana/Get/");
            new ToastContentBuilder()
                .AddText("林槐语录")
                .AddText(txt)
                .Show();
        }

        private void LoadMod_Click(object sender, RoutedEventArgs e)
        {
            // 装载 spk 插件包
            Application app = Application.Current;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;  //该值确定是否可以选择多个文件
            dialog.Title = app.Resources["options_mod_choice"].ToString();
            dialog.Filter = app.Resources["options_mod_file_type"].ToString() + "|*.spk";
            dialog.ShowDialog();

            // 解压 spk
            modLoader.Install(dialog.FileName.ToString());

            // 刷新 mod
            modLoader.Reload();
        }

        #endregion


        #region 事件 | 检查更新

        private long Max = 0;
        private void Start()
        {
            WebRequest request = WebRequest.Create(MainWindow.back[2]);
            WebResponse respone = request.GetResponse();
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
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
        private bool downFin = false;
        public void SetProgressBar(double value)
        {
            if(downFin)
            {
                return;
            }
            ProgressBarHelper.SetAnimateTo(pbDown, value);
            if (Convert.ToInt32(value / Max * 100) >= 100)
            {
                downFin = true;
                FileInfo fileInfo = null;
                try
                {
                    fileInfo = new FileInfo(@"[Upd]SSTB.exe");
                }
                catch (Exception ex)
                {
                    Log.AddLog("update", "检查更新文件大小错误：" + ex);
                }
                Log.AddLog("update", "下载完成，大小：" + fileInfo.Length);

                Thread thread = new Thread(RunUpdate);
                MainWindow.threads.Push(thread);
                thread.Start();

                Log.AddLog("update", "开始执行更新……");
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
                Log.AddLog("update", "检查更新文件大小错误：" + ex);
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    MainWindow.main.MsgAdd(
                    new List<string> { "更新", "检查更新文件大小错误。" },
                    new List<string> { "知道了" }, null);
                });
            }
            if (fileInfo != null && fileInfo.Exists)
            {
                if (fileInfo.Length != Max)
                {
                    Log.AddLog("update", "更新文件大小不符。");
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                    {
                        MainWindow.main.MsgAdd(
                        new List<string> { "更新", "更新文件大小不符。" },
                        new List<string> { "知道了" }, null);
                    });
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (File.Exists("UpdateBash.bat"))
                        {
                            File.Delete("UpdateBash.bat");
                        }
                        if (File.Exists("run.vbs"))
                        {
                            File.Delete("run.vbs");
                        }
                        try
                        {
                            Log.AddLog("update", "正在生成更新脚本。");
                            string MyName = Assembly.GetEntryAssembly().Location.Substring(Assembly.GetEntryAssembly().Location.LastIndexOf("\\") + 1);
                            File.AppendAllText("UpdateBash.bat", ":: 这是 SS Tool Box 自动生成的更新批处理");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "@echo off");
                            File.AppendAllText("UpdateBash.bat", "if \" % 1\" == \"h\" goto begin");
                            File.AppendAllText("UpdateBash.bat", "mshta vbscript:createobject(\"wscript.shell\").run(\" % ~nx0 h\",0)(window.close)&&exit");
                            File.AppendAllText("UpdateBash.bat", ":begin");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "ping 127.1 -n 2");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "del \"" + MyName + "\"");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "ping 127.1 -n 2");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "ren [Upd]SSTB.exe \"" + MyName + "\"");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "ping 127.1 -n 2");
                            File.AppendAllText("UpdateBash.bat", "\r\n" + "start \"\" \"" + MyName + "\"");

                            File.AppendAllText("run.vbs", "set ws=WScript.CreateObject(\"WScript.Shell\")");
                            File.AppendAllText("run.vbs", "\r\n" + "ws.Run \"UpdateBash.bat\", 0");

                            MainWindow.readyExit = true;
                            MainWindow.main.exitMain("update");
                        }
                        catch (Exception ex)
                        {
                            Log.AddLog("update", "生成更新脚本错误：" + ex);
                            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                            {
                                MainWindow.main.MsgAdd(
                                new List<string> { "更新", "生成更新脚本错误。" },
                                new List<string> { "知道了" }, null);
                            });
                        }
                    }), DispatcherPriority.SystemIdle, null);
                }
            }
            else
            {
                Log.AddLog("update", "检查更新文件路径不存在。");
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    MainWindow.main.MsgAdd(
                    new List<string> { "更新", "检查更新文件路径不存在。" },
                    new List<string> { "知道了" }, null);
                });
            }

            downFin = false;

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                updUpdate.Visibility = Visibility.Visible;
                updWait.Visibility = Visibility.Collapsed;
                pbDown.Visibility = Visibility.Collapsed;
            });

        }

        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            #region 初始化常规

            updTitle.Text = AppInfo.verType + " Version";
            nowVersion.Text = AppInfo.verStr + "（" + AppInfo.verNum + "）";

            if (MainWindow.back.Count != 0 && double.Parse(MainWindow.back[0]) > AppInfo.verNum)
            {
                isUpd = true;

                updUpdate.Visibility = Visibility.Visible;
                updWait.Visibility = Visibility.Collapsed;
                updNew.Visibility = Visibility.Visible;
                updNewF.Visibility = Visibility.Visible;
                updOK.Visibility = Visibility.Collapsed;

                newVersion.Text = MainWindow.back[0];
            }
            else
            {
                updButton.Visibility = Visibility.Collapsed;
            }
            checkupdButton.IsChecked = SS_Tool_Box.Options.GetOpt("stopUpd")[0] == "true" ? true : false;
            UpdBox.ItemsSource = MainWindow.linkList;
            UpdBox.DisplayMemberPath = "name";
            UpdBox.SelectedIndex = int.Parse(SS_Tool_Box.Options.GetOpt("updLink")[0] == "ERR" ? "0" : SS_Tool_Box.Options.GetOpt("updLink")[0]);

            #endregion
            #region 初始化外观
            darkButton.IsChecked = SS_Tool_Box.Options.GetOpt("darkMode")[0] == "true" ? true : false;
            autoDarkButton.IsChecked = SS_Tool_Box.Options.GetOpt("autoDarkMode")[0] == "true" ? true : false;
            rollButton.IsChecked = SS_Tool_Box.Options.GetOpt("nonLinearScrolling")[0] == "true" ? true : false;
            homeButton.IsChecked = SS_Tool_Box.Options.GetOpt("alwaysShowHome")[0] == "true" ? true : false;

            // 语言相关
            LanguageBox.ItemsSource = new LocalHelper().indexLocals;
            LanguageBox.DisplayMemberPath = "Name";
            LanguageBox.SelectedValuePath = "Value";
            string langValue = "en_US.xaml";
            if (SS_Tool_Box.Options.GetOpt("language")[0][0] != '~')
            {
                foreach (LangInfo info in new LocalHelper().indexLocals)
                {
                    if (info.Value + ".xaml" == SS_Tool_Box.Options.GetOpt("language")[0] && SS_Tool_Box.Options.GetOpt("language")[0] != "diy")
                    {
                        langValue = info.Value;
                        break;
                    }
                }
            }
            else
            {
                LanguageBox.SelectedValue = "diy";
            }
            LanguageBox.SelectedValue = langValue;

            // 主题色相关
            foreach(ColorInfo color in new WindowsHelper.Color().colors)
            {
                RadioButton radioButton = new RadioButton();
                radioButton.Resources = demoRadio.Resources;
                radioButton.Click += Color_Click;
                radioButton.ToolTip = color.name;
                radioButton.GroupName = "MainColor";
                radioButton.Background = new SolidColorBrush(color.color);
                radioButton.BorderThickness = new Thickness(0);
                radioButton.Width = 25;
                radioButton.Height = 25;
                radioButton.Margin = new Thickness(10, 0, 0, 0);

                RadioButtonHelper.SetCheckedBackground(radioButton, new SolidColorBrush(color.color));
                RadioButtonHelper.SetBoxHeight(radioButton, 25);
                RadioButtonHelper.SetBoxWidth(radioButton, 25);

                if(color.name == SS_Tool_Box.Options.GetOpt("seleColor")[0] && SS_Tool_Box.Options.GetOpt("autoColor")[0] != "true")
                {
                    radioButton.IsChecked = true;
                }

                colorsPan.Children.Add(radioButton);
            }
            if(SS_Tool_Box.Options.GetOpt("autoColor")[0] == "true")
            {
                FollowSysColor.IsChecked = true;
            }

            #endregion

            load = false;
        }
    }
}
