using System;
using System.Collections.Generic;
using System.Linq;
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
using Newtonsoft.Json.Linq;
using Panuon.UI.Silver;
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

        BaseColor baseColora = Main.baseColor;

        public Settings()
        {
            InitializeComponent();

            UpdateUI();

            //初始化主题
            IList<customer> customList = new List<customer>();
            customList.Add(new customer() { ID = 1, Name = "林槐白" });
            customList.Add(new customer() { ID = 2, Name = "坏猫橙" });
            customList.Add(new customer() { ID = 3, Name = "龙猫蓝" });
            Theams.ItemsSource = customList;
            Theams.DisplayMemberPath = "Name";
            Theams.SelectedValuePath = "ID";
            Theams.SelectedValue = baseColora.Theme;

            if(baseColora.DarkMode)
            {
                this.C11.IsChecked = true;
            }

            S11.Value = double.Parse(Main.Settings["Exterior"]["Themes"]["WindowTran"].ToString());
        }

        private void Save(object sebder, RoutedEventArgs s)
        {
            String DMIsclick = "False";
            if(this.C11.IsChecked == true)
            {
                DMIsclick = "True";
            }
            Main.Settings["Exterior"]["Themes"]["WindowTran"] = this.S11.Value;
            Main.Settings["Exterior"]["Themes"]["MainTheme"] = Theams.SelectedValue.ToString();
            Main.Settings["Exterior"]["Themes"]["DarkMode"] = DMIsclick;

            LoadingSetter setter = new LoadingSetter();
            setter.writeJsom(Main.Settings);
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
            this.T1.Foreground = baseColora.Fg;
            this.T1.FontFamily = baseColora.Fonts;
            this.T1.FontSize = 13;
            this.T11.Foreground = baseColora.Fg;
            this.T11.FontFamily = baseColora.Fonts;
            this.T11.FontSize = 13;
            this.T12.Foreground = baseColora.Fg;
            this.T12.FontFamily = baseColora.Fonts;
            this.T12.FontSize = 13;
            this.T13.Foreground = baseColora.Fg;
            this.T13.FontFamily = baseColora.Fonts;
            this.T13.FontSize = 13;
            this.T31.Foreground = baseColora.Fg;
            this.T31.FontFamily = baseColora.Fonts;
            this.T31.FontSize = 13;

            this.MT11.Foreground = baseColora.Fg;
            this.MT11.FontFamily = baseColora.Fonts;
            this.MT11.FontSize = 15;
            this.MT12.Foreground = baseColora.Fg;
            this.MT12.FontFamily = baseColora.Fonts;
            this.MT12.FontSize = 15;
            this.MT31.Foreground = baseColora.Fg;
            this.MT31.FontFamily = baseColora.Fonts;
            this.MT31.FontSize = 15;
            this.MT32.FontFamily = baseColora.Fonts;
            this.MT32.FontSize = 15;

            this.CD1.Background = baseColora.Bg;
            this.CD11.Background = baseColora.Bg;
            this.CD12.Background = baseColora.Bg;
            this.CD31.Background = baseColora.Bg;
            this.CD32.Background = baseColora.Bg;

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

            this.S11.Background = baseColora.DBg;
            this.S11.Foreground = baseColora.Fg;
            SliderHelper.SetThemeBrush(S11, baseColora.Main);

            this.TopIcon.Background = baseColora.Main;
            this.TopIcon.Foreground = baseColora.FontM;
            ButtonHelper.SetHoverBrush(TopIcon, baseColora.Main);
            ButtonHelper.SetClickCoverOpacity(TopIcon, 1);
            IconHelper.SetForeground(SaveButton, baseColora.Fg);
            this.SaveButton.Foreground = baseColora.Fg;
            this.SaveButton.Background = baseColora.Tran;

            return true;
        }
    }
}
