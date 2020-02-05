using System;
using System.Windows;
using System.Windows.Controls;
using Panuon.UI.Silver;
using SS_Tool_Box.Classes;
using SS_Tool_Box_By_WPF;

namespace SS_Tool_Box
{
    /// <summary>
    /// Page3.xaml 的交互逻辑
    /// </summary>
    public partial class PageHD2 : Page
    {
        public static bool opin = false;

        public Main ParentWindow { get; set; }

        Error error = new Error();
        BaseColor baseColora = Main.baseColor;
        String str = "";
        string strin = "";
        bool loading = true;
        bool fist = true;

        public PageHD2()
        {
            InitializeComponent();

            this.Height = 500;

            this.RunCard.Background = baseColora.Card;
            this.IconCard.Background = baseColora.Card;


            this.Percent.Background = baseColora.DBg;
            this.Percent.Foreground = baseColora.Main;

            CMD.Foreground = baseColora.Font;
            Errorsay.Foreground = baseColora.Font;
            Errorsay.Text = "初始化启动成功！";
            ProgressBarHelper.SetAnimateTo(Percent, 100);

            CMD.Text = "Stapx Steve Tool Box [ 版本 1.0.16.33 ] \n(c) Stapx Steve Team.  保留所有权利。\n\n注意事项：\n使用任何删除操作或者使用输入法将会使当前行失效。\n输入Y继续>";
            loading = false;
        }


        private void CMD_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (loading)
            {
                return;
            }
            str = CMD.Text;
            if (fist)
            {
                if (str.Substring(str.Length - 1, 1) == "Y" || str.Substring(str.Length - 1, 1) == "y")
                {
                    loading = true;
                    CMD.Text = "Stapx Steve Tool Box [ 版本 1.0.16.33 ] \n(c) Stapx Steve Team.  保留所有权利。\n\nX:\\User\\Admin>";
                    CMD.SelectionStart = CMD.Text.Length;
                    fist = false;
                    loading = false;
                    return;
                }
                else
                {
                    loading = true;
                    CMD.Text = Addtext("\n输入Y继续>");
                    CMD.SelectionStart = CMD.Text.Length;
                    loading = false;
                    return;
                }
            }
            try
            {
                if (str.Substring(str.Length - 1, 1) == "\n")
                {
                    bool runpass = false;
                    if (strin.Equals("help"))
                    {
                        loading = true;
                        CMD.Text = Addtext("cls\t\t清屏\ndev\t\t开发者选项\nexit\t\t退出\nhelp\t\t查看所有的帮助\nrun\t\t运行程式\n\n");
                        loading = false;
                        runpass = true;
                    }
                    if (strin.Equals("cls"))
                    {
                        loading = true;
                        CMD.Text = "Stapx Steve Tool Box [ 版本 1.0.16.33 ] \n(c) Stapx Steve Team.  保留所有权利。\n\n";
                        loading = false;
                        runpass = true;
                    }
                    if (strin.Equals("run"))
                    {
                        loading = true;
                        CMD.Text = Addtext("run -<appname> -[parameter]\n运行一个程式，这个程式可以是工具箱内含的工具。\n-<appname> \t\t程式名：程式的名字或者工具箱工具的全称。\n-[parameter]\t\t运行参数：传递给程式的参数。\n\n");
                        loading = false;
                        runpass = true;
                    }
                    else if (strin.Equals("run -kill -STL"))
                    {
                        opin = false;
                        Run();
                        runpass = true;
                    }
                    else if (strin.Equals("kill -S"))
                    {
                        if (Main.Settings["Features"]["Privacy"]["Password"].ToString() != "NULL")
                        {
                            PassWordEnterF7 EP = new PassWordEnterF7();
                            ParentWindow.IsMaskVisible = true;
                            EP.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                            EP.Owner = ParentWindow;
                            EP.ShowDialog();
                            ParentWindow.IsMaskVisible = false;
                        }
                        else
                        {
                            LoadingSetter.PasswordPass = true;
                        }
                        if (LoadingSetter.PasswordPass)
                        {
                            LoadingSetter.PasswordPass = false;
                            opin = true;
                            Run();
                            runpass = true;
                        }
                        else
                        {
                            ProgressBarHelper.SetAnimateTo(Percent, 50);
                            Errorsay.Text = "指令 " + strin + " 执行异常。";
                        }
                    }
                    if (strin.Equals("exit"))
                    {
                        loading = true;
                        CMD.Text = "";
                        error.ErrorTo("线程已关闭。", Percent, Errorsay);
                        ProgressBarHelper.SetAnimateTo(Percent, 0);
                        this.Errorsay.Visibility = Visibility.Visible;
                        Errorsay.Text = "线程已关闭。";
                        return;
                    }
                    if(runpass)
                    {
                        ProgressBarHelper.SetAnimateTo(Percent, 100);
                        Errorsay.Text = "指令 " + strin + " 执行成功！";
                    }
                    else
                    {
                        ProgressBarHelper.SetAnimateTo(Percent, 50);
                        Errorsay.Text = "指令 " + strin + " 执行异常。";
                    }
                    loading = true;
                    strin = "";
                    CMD.Text = CMD.Text.Substring(0, CMD.Text.Length - 1);
                    CMD.Text = Addtext("\nX:\\User\\Admin>");
                    loading = false;
                    CMD.SelectionStart = CMD.Text.Length;

                }
                else
                {
                    strin = strin + (str.Substring(str.Length - 1, 1));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        private void Run()
        {
            loading = true;
            CMD.Text = Addtext("\n正在初始化线程……");
            CMD.Text = Addtext("\n开始加载资源……");
            CMD.Text = Addtext("\n载入文件X:\\MainUI\\Admin\\UserData");
            for (int i = 0; i <= 20; i++)
            {
                CMD.Text = Addtext("\n载入文件X:\\MainUI\\Admin\\GameData\\" + i);
            }
            CMD.Text = Addtext("\n开始运行。");
            CMD.Text = Addtext("\n线程已结束。\n\n");
            loading = false;
            KillSTL openInWindow = new KillSTL();
            openInWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            openInWindow.ShowDialog();
            Errorsay.Text = "线程已关闭。";
        }

        private String Addtext(String addtext)
        {
            return CMD.Text + addtext;
        }
    }
}
