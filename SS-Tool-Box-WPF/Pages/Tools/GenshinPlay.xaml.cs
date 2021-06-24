using Microsoft.Win32;
using SS_Tool_Box.Classes.Helper;
using SS_Tool_Box.Function;
using SS_Tool_Box.Helper;
using SS_Tool_Box.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;
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
        List<RunKeyVer> runList = new List<RunKeyVer>();
        private StreamReader inReader = null;

        private bool isRun = false;

        class RunKeyVer
        {
            public RunKeyVer(double time, List<KeyVer> key)
            {
                runTime = time;
                runKey = key;
            }

            public double runTime { get; set; }
            public List<KeyVer> runKey { get; set; }
        }

        public GenshinPlay()
        {
            InitializeComponent();
        }

        #region 事件 | 提权

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mainScroll.Width = scrollGrid.Width;
#if DEBUG
            Open.Visibility = Visibility.Collapsed;
#endif
            fullButton.Visibility = Visibility.Visible;
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
                WindowsHelper.backHome();
            }
        }

        public void runExit()
        {
            WindowsHelper.backHome();
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
            WindowsHelper.changePage(new Full(), MainWindow.main.MainTitle.Text);
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
            songName.Text = "SongName";
            songName.Visibility = Visibility.Collapsed;
            inReader = null;
            loadedFile = "";
            tes = new TESVer();
            runList = new List<RunKeyVer>();
        }

        Thread thread = null;
        private void play_Click(object sender, RoutedEventArgs e)
        {
            if(!isRun)
            {
                goRun.Visibility = Visibility.Collapsed;
                goStop.Visibility = Visibility.Visible;
                thread = new Thread(run);
                MainWindow.threads.Push(thread);
                thread.Start();
                isRun = true;
            }
            else
            {
                goRun.Visibility = Visibility.Visible;
                goStop.Visibility = Visibility.Collapsed;
                thread.Abort();
                isRun = false;
            }
        }

        private void showDemo_Click(object sender, RoutedEventArgs e)
        {
            Stream src = Application.GetResourceStream(new Uri("Resources/demo.txt", UriKind.Relative)).Stream;
            inReader = new StreamReader(src, Encoding.UTF8);

            // 读取初始化铺面
            Thread thread = new Thread(loadTES);
            MainWindow.threads.Push(thread);
            thread.Start();
        }

        #endregion

        #region 操作 | 加载
        class TESVer
        {
            public string tesType { get; set; }                 // 类型
            public string tesName { get; set; }                 // 名字
            public double tesStep { get; set; }                 // 单位长度
            public double tesTime { get; set; }                 // 总长度
            public List<List<string[]>> tesKeys { get; set; }   // 键轴列表
        }

        /// <summary>
        /// 加载谱面
        /// </summary>
        private void loadTES()
        {
            Log.AddLog("genshinp", "正在加载谱面……");
            try
            {
                tes.tesKeys = new List<List<string[]>>();
                tes.tesTime = 0;
                // 逐行读取文件
                int num = 0;
                int lineNum = 0;
                List<string[]> nowLine = new List<string[]>();
                double nowLineTime = 0;

                string line;
                StreamReader file = null;
                if (inReader != null)
                {
                    file = inReader;
                }
                else
                {
                    file = new StreamReader(@loadedFile);
                }
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
                        tes.tesStep = long.Parse(line.Split('-')[1].Substring(0, line.Split('-')[1].Length - 1));
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
                            if(nowLineTime > tes.tesTime)
                            {
                                tes.tesTime = nowLineTime;
                            }
                            nowLineTime = 0;
                        }
                        else
                        {
                            string[] key = line.Split('-');
                            nowLineTime += double.Parse(key[2]) * tes.tesStep;
                            nowLine.Add(key);
                        }

                    }
                }
                // 保存键轴
                tes.tesKeys.Add(nowLine);
                if (nowLineTime > tes.tesTime)
                {
                    tes.tesTime = nowLineTime;
                }
                file.Close();

                Log.AddLog("genshinp", "加载谱面完成：lines-" + tes.tesKeys.Count +"，长度：" + tes.tesTime + " ms");
                Log.AddLog("genshinp", "正在解析为运行谱……");

                foreach(string[] key in tes.tesKeys[0])
                {
                    RunKeyVer addKey = new RunKeyVer(0, new List<KeyVer>());
                    addKey.runTime = double.Parse(key[2]) * tes.tesStep;
                    addKey.runKey.Add(findKey(int.Parse(key[0]), key[1]));
                    runList.Add(addKey);
                }

                for(int i = 1;i<tes.tesKeys.Count; i++)
                {

                    List<string[]> info = tes.tesKeys[i];
                    double nowTime = 0;
                    for(int j=0; j<info.Count; j++)
                    {
                        string[] key = info[j];

                        RunKeyVer addKey = new RunKeyVer(0, new List<KeyVer>());
                        addKey.runKey.Add(findKey(int.Parse(key[0]), key[1]));
                        nowTime += double.Parse(key[2]) * tes.tesStep;

                        int addIndex = 0;
                        double mainTime = 0;
                        foreach(RunKeyVer allKey in runList)
                        {
                            addIndex++;
                            mainTime += allKey.runTime;
                            if(mainTime == nowTime)
                            {
                                runList[addIndex - 1].runKey.Add(findKey(int.Parse(key[0]), key[1]));
                                addIndex = -1;
                                break;
                            }
                            else if(mainTime > nowTime)
                            {
                                break;
                            }
                        }
                        if(addIndex != -1)
                        {
                            addKey.runTime = nowTime - mainTime;
                            if(addKey.runTime < 0)
                            {
                                addKey.runTime = runList[addIndex - 1].runTime + addKey.runTime;
                                runList[addIndex - 1].runTime = runList[addIndex - 1].runTime - addKey.runTime;
                                runList.Insert(addIndex - 1, addKey);
                            }
                            else
                            {
                                runList.Insert(addIndex, addKey);
                            }
                        }
                    }
                    nowTime = 0;
                }

                string runk = "";
                int index = 0;
                foreach(RunKeyVer key in runList)
                {
                    index++;
                    foreach(KeyVer x in key.runKey)
                    {
                        runk += x.keyName;
                    }
                    runk += "(" + key.runTime + ")";
                    if (index == 5)
                    {
                        runk += "\n\t";
                        index = 0;
                    }
                    else
                    {
                        runk += " -> ";
                    }
                }

                if (index % 5 == 0)
                {
                    Log.AddLog("genshinp", "解析运行谱完成：\n\t" + runk.Substring(0, runk.Length - 2));
                }
                else
                {
                    Log.AddLog("genshinp", "解析运行谱完成：\n\t" + runk.Substring(0, runk.Length - 4));
                }

                // 切换界面
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    Open.Visibility = Visibility.Collapsed;
                    songName.Text = tes.tesName;
                    songName.Visibility = Visibility.Visible;
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

        class KeyVer
        {
            public KeyVer(string name, VirtualKeyCode key)
            {
                keyName = name;
                keyVirtual = key;
            }

            public string keyName { get; }
            public VirtualKeyCode keyVirtual { get; }
        }
        private KeyVer[][] keyList = new KeyVer[][]
        {
            new KeyVer[]{
                new KeyVer("Q", VirtualKeyCode.VK_Q),
                new KeyVer("W", VirtualKeyCode.VK_W),
                new KeyVer("E", VirtualKeyCode.VK_E),
                new KeyVer("R", VirtualKeyCode.VK_R),
                new KeyVer("T", VirtualKeyCode.VK_T),
                new KeyVer("Y", VirtualKeyCode.VK_Y),
                new KeyVer("U", VirtualKeyCode.VK_U),
            },
            new KeyVer[]{
                new KeyVer("A", VirtualKeyCode.VK_A),
                new KeyVer("S", VirtualKeyCode.VK_S),
                new KeyVer("D", VirtualKeyCode.VK_D),
                new KeyVer("F", VirtualKeyCode.VK_F),
                new KeyVer("G", VirtualKeyCode.VK_G),
                new KeyVer("H", VirtualKeyCode.VK_H),
                new KeyVer("J", VirtualKeyCode.VK_J),
            },
            new KeyVer[]{
                new KeyVer("Z", VirtualKeyCode.VK_Z),
                new KeyVer("X", VirtualKeyCode.VK_X),
                new KeyVer("C", VirtualKeyCode.VK_C),
                new KeyVer("V", VirtualKeyCode.VK_V),
                new KeyVer("B", VirtualKeyCode.VK_B),
                new KeyVer("N", VirtualKeyCode.VK_N),
                new KeyVer("M", VirtualKeyCode.VK_M),
            },
        };

        /// <summary>
        /// 获取键位信息
        /// </summary>
        /// <param name="line"></param>
        /// <param name="sound"></param>
        /// <returns></returns>
        private KeyVer findKey(int line, string sound)
        {
            string[] soundList = { "do", "re", "mi", "fa", "so", "la", "ti" };
            int soundnum = 0;
            foreach(string soundname in soundList)
            {
                if(soundname == sound)
                {
                    break;
                }
                soundnum++;
            }
            if(soundnum > 6)
            {
                throw new MainWindow.MyException("谱面错误，音符不存在。");
            }
            return keyList[line - 1][soundnum];
        }

        #endregion
        #region 操作 | 执行

        private void run()
        {
            try
            {
                Thread.Sleep(2000);

                InputSimulator keyIn = new InputSimulator();
                // keyIn.Keyboard.KeyPress(VirtualKeyCode.VK_E);

                Log.AddLog("genshinp", "开始播放铺面，共 " + tes.tesTime + " ms.");
                string all = "";
                foreach (RunKeyVer keys in runList)
                {
                    try
                    {
                        Thread.Sleep((int)keys.runTime);
                        foreach (KeyVer key in keys.runKey)
                        {
                            all += key.keyName;
                            keyIn.Keyboard.KeyPress(key.keyVirtual);
                        }
                        all += ",";
                    }
                    catch (Exception ex)
                    {
                        Log.AddErr("genshinp", "运行时长错误：" + keys.runKey[0].keyName + " -> " + (int)keys.runTime);
                        all = "Err    ";
                    }
                }
                Log.AddLog("genshinp", "播放结束，输出统计：\n\t" + all.Substring(0, all.Length - 1));
            }
            catch(Exception ex)
            {
                Log.AddErr("genshinp", "运行统计错误：" + ex);
            }

            Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                goRun.Visibility = Visibility.Visible;
                goStop.Visibility = Visibility.Collapsed;
            });

            Helper.Toast.Add("播放完成");
        }

        #endregion
    }
}