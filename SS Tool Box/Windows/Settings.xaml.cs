using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json.Linq;
using Panuon.UI.Silver;
using SS_Tool_Box;
using SS_Tool_Box.Classes;

namespace SS_Tool_Box_By_WPF
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class Settings : WindowX
    {
        public class customer
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public Main ParentWindow { get; set; }

        BaseColor baseColora = Main.baseColor;
        bool fistLoad = true;

        public Settings()
        {
            InitializeComponent();
            
            UpdateUI();
            
            //初始化主题
            IList<customer> customList = new List<customer>();
            customList.Add(new customer() { ID = 1, Name = "  林槐白" });
            customList.Add(new customer() { ID = 2, Name = "  坏猫黄" });
            customList.Add(new customer() { ID = 3, Name = "  龙猫蓝" });
            customList.Add(new customer() { ID = 4, Name = "  自定义" });
            Theams.ItemsSource = customList;
            Theams.DisplayMemberPath = "Name";
            Theams.SelectedValuePath = "ID";
            Theams.SelectedValue = baseColora.Theme;

            //初始化一句话推荐
            IList<customer> customListss = new List<customer>();
            customListss.Add(new customer() { ID = 1, Name = "  一言（Hit…" });
            customListss.Add(new customer() { ID = 2, Name = "  今日诗词" });
            whatsay.ItemsSource = customListss;
            whatsay.DisplayMemberPath = "Name";
            whatsay.SelectedValuePath = "ID";
            whatsay.SelectedValue = baseColora.Theme;

            if (baseColora.DarkMode)
            {
                this.C11.IsChecked = true;
            }

            if (!baseColora.Theme.ToString().Equals("4"))
            {
                this.CD12.Visibility = Visibility.Collapsed;
                this.CD13.Visibility = Visibility.Collapsed;
            }
            
            S11.Value = double.Parse(Main.Settings["Exterior"]["Themes"]["WindowTran"].ToString());
            if (baseColora.Theme.ToString().Equals("4"))
            {
                S12.Value = double.Parse(Main.Settings["Exterior"]["OwnColor"]["R"].ToString());
                S13.Value = double.Parse(Main.Settings["Exterior"]["OwnColor"]["G"].ToString());
                S14.Value = double.Parse(Main.Settings["Exterior"]["OwnColor"]["B"].ToString());
                fistLoad = false;
            }
            LoadingSetter setter = new LoadingSetter();
            try
            {
                string now = Main.Settings["Features"]["MainPage"]["SaysType"].ToString();
                if (now.Equals("Hitokoto"))
                {
                    whatsay.SelectedValue = 1;
                    T24.Visibility = Visibility.Collapsed;
                }
                else
                {
                    whatsay.SelectedValue = 2;
                    T24.Visibility = Visibility.Visible;
                    if (SSUserClass.Reg.IsRegeditItemExist("SOFTWARE", "SSTB"))
                    {
                        if (SSUserClass.Reg.IsRegeditKeyExit("SOFTWARE\\SSTB", "ShiciToken"))
                        {
                            T24.Text = "X-User-Token:" + SSUserClass.Reg.GetRegKey("SOFTWARE\\SSTB", "ShiciToken");
                        }
                        else
                        {
                            T24.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        T24.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch
            {
                //新建设置项
                JObject MainPage = new JObject { { "SaysType", "Hitokoto" } };
                Main.Settings["Features"]["MainPage"] = MainPage;
                setter.writeJsom(Main.Settings);
            }

            this.PassWord.Visibility = Visibility.Collapsed;
            this.UpdateLog.Visibility = Visibility.Collapsed;
        }

        private bool UpdateUI()
        {
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
            BG3.BeginInit();
            BG3.Source = baseColora.Bgpa;
            BG3.EndInit();

            //样式
            this.T11.Foreground = baseColora.Fg;
            this.T11.FontFamily = baseColora.Fonts;
            this.T11.FontSize = 13;
            this.T12.Foreground = baseColora.Fg;
            this.T12.FontFamily = baseColora.Fonts;
            this.T12.FontSize = 13;
            this.T13.Foreground = baseColora.Fg;
            this.T13.FontFamily = baseColora.Fonts;
            this.T13.FontSize = 13;
            this.T14.Foreground = baseColora.Fg;
            this.T14.FontFamily = baseColora.Fonts;
            this.T14.FontSize = 13;
            this.T15.Foreground = baseColora.Fg;
            this.T15.FontFamily = baseColora.Fonts;
            this.T15.FontSize = 13;
            this.T16.Foreground = baseColora.Fg;
            this.T16.FontFamily = baseColora.Fonts;
            this.T16.FontSize = 13;
            this.T17.Foreground = baseColora.Fg;
            this.T17.FontFamily = baseColora.Fonts;
            this.T17.FontSize = 13;
            this.T18.Foreground = baseColora.Fg;
            this.T18.FontFamily = baseColora.Fonts;
            this.T18.FontSize = 13;
            this.T21.Foreground = baseColora.Fg;
            this.T21.FontFamily = baseColora.Fonts;
            this.T21.FontSize = 13;
            this.T22.Foreground = baseColora.Fg;
            this.T22.FontFamily = baseColora.Fonts;
            this.T22.FontSize = 13;
            this.T23.Foreground = baseColora.Fg;
            this.T23.FontFamily = baseColora.Fonts;
            this.T23.FontSize = 13;
            this.T24.Foreground = baseColora.Fg;
            this.T24.FontFamily = baseColora.Fonts;
            this.T24.FontSize = 11;
            this.T31.Foreground = baseColora.Fg;
            this.T31.FontFamily = baseColora.Fonts;
            this.T31.FontSize = 13;
            this.T41.Foreground = baseColora.Fg;
            this.T41.FontFamily = baseColora.Fonts;
            this.T41.FontSize = 13;

            this.MT11.Foreground = baseColora.Fg;
            this.MT11.FontFamily = baseColora.Fonts;
            this.MT11.FontSize = 15;
            this.MT12.Foreground = baseColora.Fg;
            this.MT12.FontFamily = baseColora.Fonts;
            this.MT12.FontSize = 15;
            this.MT13.Foreground = baseColora.Fg;
            this.MT13.FontFamily = baseColora.Fonts;
            this.MT13.FontSize = 15;
            this.MT21.Foreground = baseColora.Fg;
            this.MT21.FontFamily = baseColora.Fonts;
            this.MT21.FontSize = 15;
            this.MT22.Foreground = baseColora.Fg;
            this.MT22.FontFamily = baseColora.Fonts;
            this.MT22.FontSize = 15;
            this.MT31.Foreground = baseColora.Fg;
            this.MT31.FontFamily = baseColora.Fonts;
            this.MT31.FontSize = 15;
            this.MT41.Foreground = baseColora.Fg;
            this.MT41.FontFamily = baseColora.Fonts;
            this.MT41.FontSize = 15;

            this.CD11.Background = baseColora.Bg;
            this.CD12.Background = baseColora.Bg;
            this.CD13.Background = baseColora.Bg;
            this.CD21.Background = baseColora.Bg;
            this.CD22.Background = baseColora.Bg;
            this.CD32.Background = baseColora.Bg;
            this.CD41.Background = baseColora.Bg;

            this.C11.Background = baseColora.DBg;
            this.C31.Background = baseColora.DBg;
            this.C32.Background = baseColora.DBg;
            CheckBoxHelper.SetCheckedBackground(C11, baseColora.DBg);
            CheckBoxHelper.SetCheckedBackground(C31, baseColora.DBg);
            CheckBoxHelper.SetCheckedBackground(C32, baseColora.DBg);
            CheckBoxHelper.SetGlyphBrush(C11, baseColora.DBg);
            CheckBoxHelper.SetGlyphBrush(C31, baseColora.DBg);
            CheckBoxHelper.SetGlyphBrush(C32, baseColora.DBg);
            CheckBoxHelper.SetCheckedGlyphBrush(C11, baseColora.Main);
            CheckBoxHelper.SetCheckedGlyphBrush(C31, baseColora.Main);
            CheckBoxHelper.SetCheckedGlyphBrush(C32, baseColora.Main);

            this.MainTab.Background = baseColora.Bg;
            this.MainTab.Foreground = baseColora.Fg;
            TabControlHelper.SetHeaderPanelBackground(MainTab, baseColora.DBg);
            TabControlHelper.SetItemIcon(MainTab, baseColora.DBg);
            TabControlHelper.SetSelectedBackground(MainTab, baseColora.Bg);
            TabControlHelper.SetSelectedForeground(MainTab, baseColora.Fg);

            this.Theams.Background = baseColora.DBg;
            this.Theams.Foreground = baseColora.Fg;
            ComboBoxHelper.SetShadowColor(Theams, baseColora.Bg.Color);
            ComboBoxHelper.SetHoverBackground(Theams, baseColora.Bg);
            ComboBoxHelper.SetHoverForeground(Theams, baseColora.Fg);
            ComboBoxHelper.SetSelectedBackground(Theams, baseColora.DBg);
            ComboBoxHelper.SetSelectedForeground(Theams, baseColora.Fg);
            this.whatsay.Background = baseColora.DBg;
            this.whatsay.Foreground = baseColora.Fg;
            ComboBoxHelper.SetShadowColor(whatsay, baseColora.Bg.Color);
            ComboBoxHelper.SetHoverBackground(whatsay, baseColora.Bg);
            ComboBoxHelper.SetHoverForeground(whatsay, baseColora.Fg);
            ComboBoxHelper.SetSelectedBackground(whatsay, baseColora.DBg);
            ComboBoxHelper.SetSelectedForeground(whatsay, baseColora.Fg);

            this.S11.Background = baseColora.DBg;
            this.S11.Foreground = baseColora.Fg;
            SliderHelper.SetThemeBrush(S11, baseColora.Main);
            this.S12.Background = baseColora.DBg;
            this.S12.Foreground = baseColora.Fg;
            SliderHelper.SetThemeBrush(S12, baseColora.Main);
            this.S13.Background = baseColora.DBg;
            this.S13.Foreground = baseColora.Fg;
            SliderHelper.SetThemeBrush(S13, baseColora.Main);
            this.S14.Background = baseColora.DBg;
            this.S14.Foreground = baseColora.Fg;
            SliderHelper.SetThemeBrush(S14, baseColora.Main);

            this.TopIcon.Background = baseColora.Main;
            this.TopIcon.Foreground = baseColora.FontM;
            ButtonHelper.SetHoverBrush(TopIcon, baseColora.Main);
            ButtonHelper.SetClickCoverOpacity(TopIcon, 1);
            this.MBG.Foreground = baseColora.Fg;
            this.MBG.Background = baseColora.DBg;
            this.SBG.Foreground = baseColora.Fg;
            this.SBG.Background = baseColora.DBg;
            ButtonHelper.SetHoverBrush(MBG, baseColora.DBg);
            ButtonHelper.SetHoverBrush(SBG, baseColora.DBg);
            this.B21.Foreground = baseColora.Fg;
            this.B21.Background = baseColora.DBg;
            ButtonHelper.SetHoverBrush(B21, baseColora.DBg);
            this.B41.Foreground = baseColora.Fg;
            this.B41.Background = baseColora.DBg;
            ButtonHelper.SetHoverBrush(B41, baseColora.DBg);

            this.Password.Background = baseColora.Bg;
            this.Password.Foreground = baseColora.Fg;
            this.UpdateLog.Background = baseColora.Bg;
            this.UpdateLog.Foreground = baseColora.Fg;

            return true;
        }

        void DackModeChanged(object sender, EventArgs e)
        {
            LoadingSetter setter = new LoadingSetter();
            SaveSet();
            JObject newSettings = setter.ReadSetup();
            if (!Theams.SelectedValue.ToString().Equals("4"))
            {
                baseColora.setColor(int.Parse(newSettings["Exterior"]["Themes"]["MainTheme"].ToString()), bool.Parse(newSettings["Exterior"]["Themes"]["DarkMode"].ToString()));
            }
            else
            {
                baseColora.setColor(int.Parse(this.S12.Value.ToString()), int.Parse(this.S13.Value.ToString()), int.Parse(this.S14.Value.ToString()), "", "", bool.Parse(this.C11.IsChecked.ToString()));
            }
            UpdateUI();
        }

        void ChangeTheams(object sender, EventArgs e)
        {
            LoadingSetter setter = new LoadingSetter();
            SaveSet();
            if(Theams.SelectedValue.ToString().Equals("4"))
            {
                this.CD12.Visibility = Visibility.Visible;
                this.CD13.Visibility = Visibility.Visible;
            }
            else
            {
                this.CD12.Visibility = Visibility.Collapsed;
                this.CD13.Visibility = Visibility.Collapsed;
            }
            JObject newSettings = setter.ReadSetup();
            if (!Theams.SelectedValue.ToString().Equals("4"))
            {
                baseColora.setColor(int.Parse(newSettings["Exterior"]["Themes"]["MainTheme"].ToString()), bool.Parse(newSettings["Exterior"]["Themes"]["DarkMode"].ToString()));
            }
            UpdateUI();
        }

        void UpdateColor(object sender, EventArgs e)
        {
            if (fistLoad)
            {
                return;
            }
            LoadingSetter setter = new LoadingSetter();
            SaveSet();
            baseColora.setColor(int.Parse(this.S12.Value.ToString()), int.Parse(this.S13.Value.ToString()), int.Parse(this.S14.Value.ToString()), "", "", bool.Parse(this.C11.IsChecked.ToString()));
        }

        void SaveSet()
        {
            String DMIsclick = "False";
            if (this.C11.IsChecked == true)
            {
                DMIsclick = "True";
            }
            Main.Settings["Exterior"]["Themes"]["WindowTran"] = this.S11.Value;
            Main.Settings["Exterior"]["Themes"]["MainTheme"] = Theams.SelectedValue.ToString();
            Main.Settings["Exterior"]["Themes"]["DarkMode"] = DMIsclick;
            Main.Settings["Exterior"]["OwnColor"]["R"] = this.S12.Value;
            Main.Settings["Exterior"]["OwnColor"]["G"] = this.S13.Value;
            Main.Settings["Exterior"]["OwnColor"]["B"] = this.S14.Value;

            LoadingSetter setter = new LoadingSetter();
            setter.writeJsom(Main.Settings);
        }

        private void MBG_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Title = "选择图片";
            dialog.Filter = "图片文件(*.jpg,*.jpeg,*.png,*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (dialog.ShowDialog() == true)
            {
                if (File.Exists(dialog.FileName))
                {
                    string[] sArray = dialog.FileName.Split('.');
                    File.Copy(dialog.FileName, "SSTB/Background/MBG" + sArray[1], true);
                }
            }
        }

        private void SBG_Click(object sender, RoutedEventArgs e)
        {

        }

        private void B21_Click(object sender, RoutedEventArgs e)
        {
            if (PassWord.Visibility == Visibility.Visible)
            {
                if (!String.IsNullOrWhiteSpace(Password.Password))
                {
                    if(Password.Password == "NULL" || Password.Password == "null")
                    {
                        Password.Password = "";
                        PasswordBoxHelper.SetWatermark(Password, " ( 这不是个合法的密码！ ) ");
                        return;
                    }
                    Main.Settings["Features"]["Privacy"]["Password"] = Password.Password;
                    SaveSet();
                    Password.Password = "";
                }
                this.PassWord.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (Main.Settings["Features"]["Privacy"]["Password"].ToString() != "NULL")
                {
                    PassWordEnterF7 EP = new PassWordEnterF7();
                    this.IsMaskVisible = true;
                    EP.Owner = this;
                    EP.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    EP.ShowDialog();
                    this.IsMaskVisible = false;
                }
                else
                {
                    LoadingSetter.PasswordPass = true;
                }
                if (!LoadingSetter.PasswordPass)
                {
                    return;
                }
                LoadingSetter.PasswordPass = false;
                this.PassWord.Visibility = Visibility.Visible;
                PasswordBoxHelper.SetWatermark(Password, " ( 再次点击设置按钮确定 ) ");
                if (Main.Settings["Features"]["Privacy"]["Password"].ToString() != "NULL")
                {
                    PasswordBoxHelper.SetWatermark(Password, " ( 空白将保持已设置的密码 ) ");
                }
                else
                {
                    MessageBoxX.Show("私隐密码为弱密码，以 <明文> 记录在设置文件中，这意味着这个密码仅用于警告，没有任何安全性，请不要把重要信息存储在 SSTB 内。", "提示");
                }
            }
        }

        private void B41_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateLog.Visibility == Visibility.Collapsed)
            {
                UpdateLog.Visibility = Visibility.Visible;
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(UpdateLog.Text))
                {
                    UpdateLog.Visibility = Visibility.Collapsed;
                    String Updtext = UpdateLog.Text;
                    UpdateLog.Text = "";
                    String url = "https://raw.githubusercontent.com/Stapxs/SS-Tool-Box/" + Main.szTree + "/SS%20Tool%20Box/bin/Release/SS%20Tool%20Box.exe";
                    JObject jObject = new JObject { { "Version", "2" }, { "MainVersion", Main.stVersion }, { "Time", DateTime.Now.ToString("yyyy/MM/dd") + " " + DateTime.Now.ToShortTimeString().ToString() }, { "Url", url }, { "Logs", Updtext } };

                    try
                    {
                        using (StreamWriter sw = new StreamWriter("SSTB/Update.txt"))
                        {
                            sw.WriteLine(jObject.ToString());
                            sw.Close();
                        }
                        string where = Directory.GetCurrentDirectory();
                        where = where + @"\SSTB";
                        System.Diagnostics.Process process;
                        process = System.Diagnostics.Process.Start(@where);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("写入文件错误：" + ex);
                    }
                }
            }
        }

        private void Whatsay_DropDownClosed(object sender, EventArgs e)
        {
            LoadingSetter setter = new LoadingSetter();
            try
            {
                string now = Main.Settings["Features"]["MainPage"]["SaysType"].ToString();
                if (whatsay.SelectedValue.ToString().Equals("1"))
                {
                    Main.Settings["Features"]["MainPage"]["SaysType"] = "Hitokoto";
                    T24.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Main.Settings["Features"]["MainPage"]["SaysType"] = "ShiCi";
                    T24.Visibility = Visibility.Visible;
                    if (SSUserClass.Reg.IsRegeditItemExist("SOFTWARE", "SSTB"))
                    {
                        if (SSUserClass.Reg.IsRegeditKeyExit("SOFTWARE\\SSTB", "ShiciToken"))
                        {
                            T24.Text = "X-User-Token:" + SSUserClass.Reg.GetRegKey("SOFTWARE\\SSTB", "ShiciToken");
                        }
                        else
                        {
                            T24.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        T24.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch
            {
                //新建设置项
                JObject MainPage = new JObject { { "SaysType", "Hitokoto" } };
                Main.Settings["Features"]["MainPage"] = MainPage;
                setter.writeJsom(Main.Settings);
            }
        SaveSet();
        }
    }
}
