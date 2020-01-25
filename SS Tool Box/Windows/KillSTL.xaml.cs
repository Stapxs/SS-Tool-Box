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
            public string Name { get; set; }
            public int Hit { get; set; }
            public int HitMax { get; set; }
        }

        IList<customerHit> customHit = new List<customerHit>();

        public class customerGive
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Hit { get; set; }
            public int HitMax { get; set; }
            public int Love { get; set; }
            public int LoveMax { get; set; }
            public int Hug { get; set; }
            public int HugMax { get; set; }
        }

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

        string logs = "STL：你好！\n";
        string cmd = "Stapx Steve Tool Box [ 版本 1.0.16.33 ] \n(c) Stapx Steve Team.  保留所有权利。\n";
        string you = "youself";
        int pass = 0;

        bool op = false;

        public KillSTL()
        {
            InitializeComponent();

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
                you = "SS";
                op = true;
            }

            customHit.Add(new customerHit() { ID = 0, Name = "  - 空 -", Hit = 0, HitMax = 0 });
            customHit.Add(new customerHit() { ID = 1, Name = "  - 轻拳 -", Hit = 5, HitMax = 10});
            Hit.ItemsSource = customHit;
            Hit.DisplayMemberPath = "Name";
            Hit.SelectedValuePath = "ID";
            Hit.SelectedValue = 0;
        }

        private void setTitle(String title)
        {
            this.Title = "| Kill STL - " + title;
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            if(op)
            {
                MessageBoxX.Show("你好！管理员权限已为你打开。", "你好");
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

        private String addtext(String addtext)
        {
            return CMD.Text + addtext;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            ForceClose();
        }

        private void Pass_Click(object sender, RoutedEventArgs e)
        {
            pass += 1;
            TPASS.Text = "第" + pass + "回合";
            //刷新血量
            health -= BuffHitPass(health) - HealthPass(health, 1);
            ProgressBarHelper.SetAnimateTo(Health, health);
        }

        private void Hit_Click(object sender, RoutedEventArgs e)
        {
            pass += 1;
            TPASS.Text = "第" + pass + "回合";

            //刷新血量
            health -= BuffHitPass(health) - HealthPass(health, 0);
            ProgressBarHelper.SetAnimateTo(Health, health);
        }

        private void Hit_DropDownClosed(object sender, EventArgs e)
        {

        }

        private int HealthPass(int health, int nMode)        //扣血判定算法
        {
            int rehealth = 0;
            switch (nMode)
            {
                //跳过回合
                case 1:
                    {
                        /* 逻辑
                         * buff 优先扣血
                         * 血量大于30小于80每回合定量扣血 -1
                         * 血量小于30每回合定量扣血 -3
                         */

                        //buff会单独写函数扣，所以这儿就用不着
                        if(health > 30 && health <= 80)
                        {
                            rehealth = health - 1;
                        }
                        else if(health <= 30)
                        {
                            rehealth = health - 3;
                        }
                    }
                    break;
                //操作计算
                case 0:
                    {
                        rehealth = health - HitPass(health, int.Parse(Hit.SelectedValue.ToString()));
                    }
                    break;
            }
            return rehealth;
        }

        public int BuffHitPass(int health)          //Buff判定算法
        {
            int rehealth = 0;
            rehealth = health;
            return rehealth;
        }

        public int HitPass(int health, int ID)          //攻击判定算法
        {
            int rehealth = 0;
            switch(ID)
            {
                case 1:
                    {
                        dynamic temp = customHit[1];
                        rehealth = RandomInt(temp.Hit, temp.HitMax);
                    }
                    break;
                default:
                    {
                        rehealth = health;
                    }
                    break;
            }
            return rehealth;
        }

        private int RandomInt(int Min, int Max)
        {
            Random random = new Random();
            return random.Next(Min, Max);
        }
    }
}
