using Panuon.UI.Silver;
using SS_Tool_Box.Classes;
using SS_Tool_Box_By_WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SS_Tool_Box
{
    /// <summary>
    /// KillSTL.xaml 的交互逻辑
    /// </summary>
    public partial class KillSTL : WindowX
    {
        public class customerHit
        {
            public int ID { get; set; }
            public string ShowName { get; set; }
            public string Name { get; set; }
            public int Hit { get; set; }
            public int HitMax { get; set; }
            public int Love { get; set; }
            public int LoveMax { get; set; }
            public int Hug { get; set; }
            public int HugMax { get; set; }
            public int Duang { get; set; }
            public int DuangMax { get; set; }
            public string Buff { get; set; }
            public int BuffPes { get; set; }
        }
        IList<customerHit> customHit = new List<customerHit>();

        public class customerGive
        {
            public int ID { get; set; }
            public string ShowName { get; set; }
            public string Name { get; set; }
            public int Hit { get; set; }
            public int HitMax { get; set; }
            public int Love { get; set; }
            public int LoveMax { get; set; }
            public int Hug { get; set; }
            public int HugMax { get; set; }
            public int Duang { get; set; }
            public int DuangMax { get; set; }
            public string Buff { get; set; }
            public int BuffPes { get; set; }
        }
        IList<customerGive> customGive = new List<customerGive>();

        public class customerDo
        {
            public int ID { get; set; }
            public string ShowName { get; set; }
            public string Name { get; set; }
            public int Hit { get; set; }
            public int HitMax { get; set; }
            public int Love { get; set; }
            public int LoveMax { get; set; }
            public int Hug { get; set; }
            public int HugMax { get; set; }
            public string Buff { get; set; }
            public int BuffPes { get; set; }
        }
        IList<customerDo> customDo = new List<customerDo>();

        BaseColor baseColora = Main.baseColor;
        string version = "Dev.0.1.5";

        //初始化数据
        int health = 100;
        int love = 40;
        int hug = 90;
        int duang = 10;

        int yhealth = 100;
        int ylove = 50;
        int yhug = 100;
        int yduang = 10;

        string logs = "ShanTianLun：你好！";
        string cmd = "Stapx Steve Tool Box [ 版本 1.0.16.33 ] \n(c) Stapx Steve Team.  保留所有权利。\n";
        string you = "Youself";
        int pass = 0;

        bool op = false;

        public KillSTL()
        {
            InitializeComponent();
            Height = 500;
            Topmost = true;

            this.T20.Text = "这就是一个超级简单的、超级普通的、超级正常的养成对战类回合制文字游戏 / ？，灵感来自STL。同时这也是SSTB内涵的第一个游戏为主的彩蛋。";
            this.T19.Text = "相杀象征相爱\nBy Stapx Steve [ 林槐 ] @2019 / 版权所有";

            setTitle(version);
            this.T2.Text = "Version - " + version;
            this.T18.Text = "Version - " + version;

            WindowXCaption.SetBackground(this, baseColora.Main);
            WindowXCaption.SetForeground(this, baseColora.FontM);

            BG.BeginInit();
            BG.Source = baseColora.Bgpa;
            BG.EndInit();
            BG1.BeginInit();
            BG1.Source = baseColora.Bgpa;
            BG1.EndInit();
            BG2.BeginInit();
            BG2.Source = baseColora.Bgpa;
            BG2.EndInit();

            this.TT.Foreground = baseColora.Main;

            this.T.Foreground = baseColora.Fg;
            this.T1.Foreground = baseColora.Fg;
            this.T2.Foreground = baseColora.Fg;
            this.T3.Foreground = baseColora.Fg;
            this.T4.Foreground = baseColora.Fg;
            this.T5.Foreground = baseColora.Fg;
            this.T6.Foreground = baseColora.Fg;
            this.T7.Foreground = baseColora.Fg;
            this.T8.Foreground = baseColora.Fg;
            this.T9.Foreground = baseColora.Fg;
            this.T10.Foreground = baseColora.Fg;
            this.T11.Foreground = baseColora.Fg;
            this.T12.Foreground = baseColora.Fg;
            this.T13.Foreground = baseColora.Fg;
            this.T14.Foreground = baseColora.Fg;
            this.T15.Foreground = baseColora.Fg;
            this.T16.Foreground = baseColora.Fg;
            this.T17.Foreground = baseColora.Fg;
            this.T18.Foreground = baseColora.Fg;
            this.T19.Foreground = baseColora.Fg;
            this.T20.Foreground = baseColora.Fg;

            this.TopIcon.Background = baseColora.Main;
            this.TopIcon.Foreground = baseColora.FontM;
            ButtonHelper.SetHoverBrush(TopIcon, baseColora.Main);
            ButtonHelper.SetClickCoverOpacity(TopIcon, 1);
            
            this.RunButton.Foreground = baseColora.Font;
            this.RunButton.Background = baseColora.Tran;
            ButtonHelper.SetHoverBrush(RunButton, baseColora.Main);
            this.Pass.Foreground = baseColora.Font;
            this.Pass.Background = baseColora.DBg;
            ButtonHelper.SetHoverBrush(Pass, baseColora.Main);
            this.Exit.Foreground = baseColora.Font;
            this.Exit.Background = baseColora.DBg;
            ButtonHelper.SetHoverBrush(Exit, baseColora.Main);
            this.Hits.Foreground = baseColora.Font;
            this.Hits.Background = baseColora.DBg;
            ButtonHelper.SetHoverBrush(Hits, baseColora.Main);

            this.MainTab.Background = baseColora.Bg;
            this.MainTab.Foreground = baseColora.Fg;
            TabControlHelper.SetHeaderPanelBackground(MainTab, baseColora.DBg);
            TabControlHelper.SetItemIcon(MainTab, baseColora.DBg);
            TabControlHelper.SetSelectedBackground(MainTab, baseColora.Bg);
            TabControlHelper.SetSelectedForeground(MainTab, baseColora.Fg);
            this.GameTab.Background = baseColora.Bg;
            this.GameTab.Foreground = baseColora.Fg;
            TabControlHelper.SetHeaderPanelBackground(GameTab, baseColora.DBg);
            TabControlHelper.SetItemIcon(GameTab, baseColora.DBg);
            TabControlHelper.SetSelectedBackground(GameTab, baseColora.Bg);
            TabControlHelper.SetSelectedForeground(GameTab, baseColora.Fg);

            this.Hit.Background = baseColora.DBg;
            this.Hit.Foreground = baseColora.Fg;
            ComboBoxHelper.SetShadowColor(Hit, baseColora.Bg.Color);
            ComboBoxHelper.SetHoverBackground(Hit, baseColora.Bg);
            ComboBoxHelper.SetHoverForeground(Hit, baseColora.Fg);
            ComboBoxHelper.SetSelectedBackground(Hit, baseColora.DBg);
            ComboBoxHelper.SetSelectedForeground(Hit, baseColora.Fg);
            this.Do.Background = baseColora.DBg;
            this.Do.Foreground = baseColora.Fg;
            ComboBoxHelper.SetShadowColor(Do, baseColora.Bg.Color);
            ComboBoxHelper.SetHoverBackground(Do, baseColora.Bg);
            ComboBoxHelper.SetHoverForeground(Do, baseColora.Fg);
            ComboBoxHelper.SetSelectedBackground(Do, baseColora.DBg);
            ComboBoxHelper.SetSelectedForeground(Do, baseColora.Fg);
            this.Give.Background = baseColora.DBg;
            this.Give.Foreground = baseColora.Fg;
            ComboBoxHelper.SetShadowColor(Give, baseColora.Bg.Color);
            ComboBoxHelper.SetHoverBackground(Give, baseColora.Bg);
            ComboBoxHelper.SetHoverForeground(Give, baseColora.Fg);
            ComboBoxHelper.SetSelectedBackground(Give, baseColora.DBg);
            ComboBoxHelper.SetSelectedForeground(Give, baseColora.Fg);

            this.BagBD.Background = baseColora.DBg;
            ListViewItem listViewItem = new ListViewItem();
            listViewItem.Name = "空";
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "  空";
            textBlock.Foreground = baseColora.Font;
            textBlock.FontSize = 14;
            listViewItem.Content = textBlock;
            Bag.Items.Add(listViewItem);

            this.Health.Background = baseColora.DBg;
            this.Health.Foreground = baseColora.Main;
            this.Love.Background = baseColora.DBg;
            this.Love.Foreground = baseColora.Main;
            this.Hug.Background = baseColora.DBg;
            this.Hug.Foreground = baseColora.Main;
            this.Duang.Background = baseColora.DBg;
            this.Duang.Foreground = baseColora.Main;

            this.yHealth.Background = baseColora.DBg;
            this.yHealth.Foreground = baseColora.Main;
            this.yLove.Background = baseColora.DBg;
            this.yLove.Foreground = baseColora.Main;
            this.yHug.Background = baseColora.DBg;
            this.yHug.Foreground = baseColora.Main;
            this.yDuang.Background = baseColora.DBg;
            this.yDuang.Foreground = baseColora.Main;

            this.Run.Background = baseColora.DBg;
            this.Run.Foreground = baseColora.Fg;

            CMD.Foreground = baseColora.Font;
            CMDCOL.Foreground = baseColora.Font;

            this.MainGame.Visibility = Visibility.Collapsed;

            CMDIN.Visibility = Visibility.Collapsed;

            if(PageHD2.opin)
            {
                you = "Stapx Steve";
                op = true;
            }

            customHit.Add(new customerHit() { ID = 0, Name = "  - 空 -", ShowName = "空", Hit = 0, HitMax = 0, Love = 0, LoveMax = 0, Hug = 0, HugMax = 0, Duang = 0, DuangMax = 0 });
            customHit.Add(new customerHit() { ID = 1, Name = "  - 轻拳 -", ShowName = "轻拳", Hit = -10, HitMax = -5, Love = -1, LoveMax = 0, Hug = -45, HugMax = -25, Duang = 1, DuangMax = 2 });
            Hit.ItemsSource = customHit;
            Hit.DisplayMemberPath = "Name";
            Hit.SelectedValuePath = "ID";
            Hit.SelectedValue = 0;

            customGive.Add(new customerGive() { ID = 0, Name = "  - 空 -", ShowName = "空", Hit = 0, HitMax = 0, Love = 0, LoveMax = 0, Hug = 0, HugMax = 0, Buff = "", BuffPes = 0 });
            customGive.Add(new customerGive() { ID = 1, Name = "  - 面包 -", ShowName = "面包", Hit = 0, HitMax = 0, Love = 0, LoveMax = 2, Hug = 10, HugMax = 10, Buff = "吃撑", BuffPes = 5 });
            Give.ItemsSource = customGive;
            Give.DisplayMemberPath = "Name";
            Give.SelectedValuePath = "ID";
            Give.SelectedValue = 0;

            customDo.Add(new customerDo() { ID = 0, Name = "  - 空 -", ShowName = "空", Hit = 0, HitMax = 0, Love = 0, LoveMax = 0, Hug = 0, HugMax = 0, Buff = "", BuffPes = 0 });
            customDo.Add(new customerDo() { ID = 1, Name = "  - 捏捏 -", ShowName = "捏捏", Hit = 0, HitMax = 0, Love = 0, LoveMax = 2, Hug = -5, HugMax = -2, Buff = "", BuffPes = 0 });
            Do.ItemsSource = customDo;
            Do.DisplayMemberPath = "Name";
            Do.SelectedValuePath = "ID";
            Do.SelectedValue = 0;
        }

        private bool workPass(int nMode)                        //游戏过程判定
        {
            //基础判定
            if(health > 30 && health < 85)          //血量在30-85每回合-1
            {
                health -= 1;
            }
            else if(health <= 30)                   //血量小于30每回合-3
            {
                health -= 3;
            }
            if (yhealth > 30 && yhealth < 85)
            {
                yhealth -= 1;
            }
            else if (yhealth <= 30)
            {
                yhealth -= 3;
            }
            hug -= 1;                               //每回合饥饿-1
            yhug -= 1;
            if(nMode == 0)
            {
                return true;
            }
            //攻击判定
            //A
            if(int.Parse(Hit.SelectedValue.ToString()) != 0)
            {
                foreach(customerHit hit in customHit)
                {
                    if(hit.ID == int.Parse(Hit.SelectedValue.ToString()))
                    {
                        if (yhug + hit.HugMax >= 0)
                        {
                            int hitrad = RandomInt(hit.Hit, hit.HitMax);
                            health += hitrad;
                            love += RandomInt(hit.Love, hit.LoveMax);
                            yhug += RandomInt(hit.Hug, hit.HugMax);
                            duang += RandomInt(hit.Duang, hit.DuangMax);
                            CMD.Text = addtext(" [ 系统 ] " + you + " 打出了一击 " + hit.ShowName + " 造成 " + hitrad + " 伤害。");
                        }
                        else
                        {
                            CMD.Text = addtext(" [ 系统 ] " + you + " 肚子饿的咕咕叫了QAQ，没有成功打出攻击。");
                        }
                    }
                }
            }
            else if (int.Parse(Do.SelectedValue.ToString()) != 0)
            {
                
            }
            else
            {
                CMD.Text = addtext(" [ 系统 ] " + you + " 什么都没做的说。");
            }
            return true;
        }
        private bool ClickPass(int nMode)
        {
            bool error = false;
            TPASS.Text = "第" + ++pass + "回合";         //刷新回合数
            CMD.Text = addtext("\n第" + pass + "回合");
            error = workPass(nMode);                    //游戏过程判定（ 数据判定 & BUFF获取判定 ）
            You.IsSelected = true;                      //跳转到信息界面
            updatePBar();                               //刷新数据条
            CMD.SelectionStart = CMD.Text.Length;       //刷新日志光标
            return error;
        }

        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            ClickPass(0);
        }

        private void Hit_Click(object sender, RoutedEventArgs e)
        {
            ClickPass(1);
        }

        private void updatePBar()            //刷新进度条
        {
            ProgressBarHelper.SetAnimateTo(Health, health);
            ProgressBarHelper.SetAnimateTo(yHealth, yhealth);
            ProgressBarHelper.SetAnimateTo(Hug, hug);
            ProgressBarHelper.SetAnimateTo(yHug, yhug);
            ProgressBarHelper.SetAnimateTo(Duang, duang);
            ProgressBarHelper.SetAnimateTo(yDuang, yduang);
            ProgressBarHelper.SetAnimateTo(Love, love);
            ProgressBarHelper.SetAnimateTo(yLove, ylove);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            ForceClose();
        }

        private void Hit_DropDownClosed(object sender, EventArgs e)
        {
            Do.SelectedValue = 0;
            Give.SelectedValue = 0;
        }

        private void Do_DropDownClosed(object sender, EventArgs e)
        {
            Hit.SelectedValue = 0;
            Give.SelectedValue = 0;
        }

        private void Give_DropDownClosed(object sender, EventArgs e)
        {
            Do.SelectedValue = 0;
            Hit.SelectedValue = 0;
        }

        private void setTitle(String title)
        {
            this.Title = "| Kill STL - " + title;
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            Set.Visibility = Visibility.Collapsed;
            Topmost = false;
            if (op)
            {
                CMDIN.Visibility = Visibility.Visible;
            }
            this.MainPage.Visibility = Visibility.Collapsed;        //隐藏主页
            try
            {
                string urls = "http://q1.qlogo.cn/g?b=qq&nk=1917934706&s=3";
                this.Hand.Source = new BitmapImage(new Uri(urls));
            }
            catch
            {
            }
            this.MainGame.Visibility = Visibility.Visible;           //显示主游戏页面

            ProgressBarHelper.SetAnimateTo(Health, health);
            ProgressBarHelper.SetAnimateTo(Love, love);
            ProgressBarHelper.SetAnimateTo(Hug, hug);
            ProgressBarHelper.SetAnimateTo(Duang, duang);

            ProgressBarHelper.SetAnimateTo(yHealth, yhealth);
            ProgressBarHelper.SetAnimateTo(yLove, ylove);
            ProgressBarHelper.SetAnimateTo(yHug, yhug);
            ProgressBarHelper.SetAnimateTo(yDuang, yduang);

            CMD.Text = logs;
            CMD.Text = addtext(you + "：你好！");
            CMDCOL.Text = cmd;
            TYN.Text = you;
            TPASS.Text = "第" + pass + "回合";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String runcmd = Run.Text;
            Run.Text = "";
            if (CMDIN.IsSelected)
            {
                CMDCOL.Text = addcmd(">" + runcmd);
                if (String.IsNullOrWhiteSpace(Run.Text))
                {
                    if (runcmd.Equals("/help"))
                    {
                        CMDCOL.Text = addcmd("/ui - UI相关操作");
                    }
                    else if (runcmd.Equals("/help ui"))
                    {
                        CMDCOL.Text = addcmd("/ui show [UI Name] - 显示UI\n    /ui hide [UI Name] - 隐藏UI");
                    }
                    else if (runcmd.Equals("/ui show \"Set\""))
                    {
                        Set.Visibility = Visibility.Visible;
                        CMDCOL.Text = addcmd("执行成功。(0)");
                    }
                    else if (runcmd.Equals("/ui hide \"Set\""))
                    {
                        Set.Visibility = Visibility.Collapsed;
                        CMDCOL.Text = addcmd("执行成功。(0)");
                    }
                    else if (runcmd.Equals("/ui show \"MainPage\""))
                    {
                        this.MainPage.Visibility = Visibility.Visible;
                        this.MainGame.Visibility = Visibility.Collapsed;
                        CMDCOL.Text = addcmd("执行成功。(0)");
                    }
                    else
                    {
                        CMDCOL.Text = addcmd("未知命令。");
                    }
                }
            }
        }


        /* 无关算法
         * 下面是功能无关的辅助算法
         */

        private int RandomInt(int Min, int Max)
        {
            Random random = new Random();
            if(Min > Max)
            {
                int p = Min;
                Min = Max;
                Max = p;
            }
            return random.Next(Min, Max);
        }

        private String addtext(String text)
        {
            return CMD.Text + "\n" + text;
        }

        private String addcmd(String text)
        {
            return CMDCOL.Text + "\n" + text;
        }
    }
}
