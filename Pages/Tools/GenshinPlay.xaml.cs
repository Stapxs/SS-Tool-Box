using Microsoft.Win32;
using SS_Tool_Box.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace SS_Tool_Box.Pages.Tools
{
    /// <summary>
    /// GenshinPlay.xaml 的交互逻辑
    /// </summary>
    public partial class GenshinPlay : Page
    {
        private string loadedFile = "";
        private TESVer tes = new TESVer();

        public GenshinPlay()
        {
            InitializeComponent();
        }

        #region 事件 | 提权

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // 判断权限
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
            if (!windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                // 提权
                MainWindow.main.MsgAdd(
                    new List<string> { "自动奏谱", "由于原神窗口拥有管理员权限，故本程序将重启并提高权限用于向原神窗口发送按键事件" },
                    new List<string> { "好的", "不要" },
                    new List<MainWindow.FunMsgButton> { runGet, runExit });
            }
        }

        public void runGet()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Process.GetCurrentProcess().MainModule.FileName;
            psi.Verb = "runas";
            try
            {
                Process.Start(psi);
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MainWindow.main.MsgAdd(
                    new List<string> { "自动奏谱", "提权失败：" + ex.Message },
                    new List<string> { "知道了" }, null);
                MainWindow.main.backHome();
            }
        }

        public void runExit()
        {
#if !DEBUG
            MainWindow.main.backHome();
#endif
        }

        #endregion
        #region 事件 | 按钮

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

        private void OpenPane_Click(object sender, RoutedEventArgs e)
        {
            Application app = Application.Current;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;  //该值确定是否可以选择多个文件
            dialog.Title = app.Resources["tool_genshin_open"].ToString();
            dialog.Filter = app.Resources["tool_genshin_open_file_type"].ToString() + "|*.tes";
            dialog.ShowDialog();
            loadedFile = dialog.FileName;
            if (loadedFile == null || loadedFile == "")
            {
                MainWindow.main.MsgAdd(
                       new List<string> { "自动奏谱", "请选择文件" },
                       new List<string> { "取消" }, null);
                return;
            }

            // 读取初始化铺面
            Thread thread = new Thread(loadTES);
            MainWindow.threads.Push(thread);
            thread.Start();

        }

        private void newPane_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main.MsgAdd(
                       new List<string> { "自动奏谱", "这功能还没做" },
                       new List<string> { "知道了" }, null);
        }

        private void mainClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main.MsgAdd(
                       new List<string> { "自动奏谱", "未保存的更改将被丢弃，确认退出？" },
                       new List<string> { "好的", "点错了" }, 
                       new List<MainWindow.FunMsgButton> { runClose, null } );
        }

        private void runClose()
        {
            Open.Visibility = Visibility.Visible;
            mainTab.Visibility = Visibility.Collapsed;
            loadedFile = "";
            tes = new TESVer();
        }

        #endregion

        #region 操作 | 加载
        class TESVer
        {
            public string tesType { get; set; }                 // 类型
            public string tesName { get; set; }                 // 名字
            public double tesLong { get; set; }                 // 单位长度
            public List<List<string[]>> tesKeys { get; set; }       // 键轴列表
        }

        class KeyVer
        {
            public KeyVer(string name, string sound, VirtualKeyCode key)
            {
                keyName = name;
                keySound = sound;
                keyVirtual = key;
            }

            public string keyName { get; }
            public string keySound { get; }
            public VirtualKeyCode keyVirtual { get; }
        }
        private KeyVer[][] keyList = new KeyVer[][]
        {
            new KeyVer[]{ 
                new KeyVer("Q", "do", VirtualKeyCode.VK_Q),
                new KeyVer("W", "re", VirtualKeyCode.VK_W),
                new KeyVer("E", "mi", VirtualKeyCode.VK_E),
                new KeyVer("R", "fa", VirtualKeyCode.VK_R),
                new KeyVer("T", "so", VirtualKeyCode.VK_T),
                new KeyVer("Y", "la", VirtualKeyCode.VK_Y),
                new KeyVer("U", "ti", VirtualKeyCode.VK_U), 
            },
            new KeyVer[]{
                new KeyVer("A", "do", VirtualKeyCode.VK_A),
                new KeyVer("S", "re", VirtualKeyCode.VK_S),
                new KeyVer("D", "mi", VirtualKeyCode.VK_D),
                new KeyVer("F", "fa", VirtualKeyCode.VK_F),
                new KeyVer("G", "so", VirtualKeyCode.VK_G),
                new KeyVer("H", "la", VirtualKeyCode.VK_H),
                new KeyVer("J", "ti", VirtualKeyCode.VK_J),
            },
            new KeyVer[]{
                new KeyVer("Z", "do", VirtualKeyCode.VK_Z),
                new KeyVer("X", "re", VirtualKeyCode.VK_X),
                new KeyVer("C", "mi", VirtualKeyCode.VK_C),
                new KeyVer("V", "fa", VirtualKeyCode.VK_V),
                new KeyVer("B", "so", VirtualKeyCode.VK_B),
                new KeyVer("N", "la", VirtualKeyCode.VK_N),
                new KeyVer("M", "ti", VirtualKeyCode.VK_M),
            },
        };

        private void loadTES()
        {
            try
            {
                tes.tesKeys = new List<List<string[]>>();
                // 逐行读取文件
                string line;
                int num = 0;
                int lineNum = 0;
                List<string[]> nowLine = new List<string[]>();
                StreamReader file = new StreamReader(@loadedFile);
                while ((line = file.ReadLine()) != null)
                {
                    num++;
                    if (num == 1)
                    {
                        // 铺面名
                        tes.tesName = line.Substring(1, line.Length - 2);
                    }
                    if (num == 2)
                    {
                        // 铺面参数
                        tes.tesType = line.Split('-')[0].Substring(1);
                        tes.tesLong = long.Parse(line.Split('-')[1].Substring(0, line.Split('-')[1].Length - 1));
                    }
                    if (num > 3)
                    {
                        // 键列表
                        if (line[0] == '[')
                        {
                            lineNum++;
                            int lnum = int.Parse(line.Split('-')[1].Substring(0, line.Split('-')[1].Length - 1));
                            if (lineNum == lnum)
                            {
                                if (nowLine.Count != 0)
                                {
                                    // 保存键轴
                                    tes.tesKeys.Add(nowLine);
                                }
                                nowLine = new List<string[]>();
                            }
                            else
                            {
                                throw new MainWindow.MyException("键轴错误，行数不正确。");
                            }
                        }
                        else
                        {
                            string[] key = line.Split('-');
                            nowLine.Add(key);
                        }

                    }
                }
                // 保存键轴
                tes.tesKeys.Add(nowLine);
                file.Close();

                // 切换界面
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    Open.Visibility = Visibility.Collapsed;
                    mainTab.Visibility = Visibility.Visible;
                });
            }
            catch(Exception ex)
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
#if DEBUG
                    MainWindow.main.MsgAdd(
                       new List<string> { "自动奏谱", "读取谱面错误：" + ex },
                       new List<string> { "取消" }, null);
#else
                MainWindow.main.MsgAdd(
                       new List<string> { "自动奏谱", "读取谱面错误：" + ex.Message },
                       new List<string> { "取消" }, null);
#endif
                });
            }
        }

        #endregion


        private void run()
        {
            InputSimulator keyIn = new InputSimulator();
            keyIn.Keyboard.KeyPress(VirtualKeyCode.VK_E);
        }

    }
}