using Microsoft.Win32;
using SS_Tool_Box.Function;
using System.Windows;
using System.Windows.Controls;

namespace SS_Tool_Box.Pages
{
    /// <summary>
    /// Options.xaml 的交互逻辑
    /// </summary>
    public partial class Options : Page
    {

        private bool load = true;

        public Options()
        {
            InitializeComponent();

            // 初始化设置项目
            if (SS_Tool_Box.Options.GetOpt("darkMode")[0] == "false")
            {
                darkButton.IsChecked = false;
            }
            else
            {
                darkButton.IsChecked = true;

            }

            LanguageBox.ItemsSource = UI.Localization.indexLocals;
            LanguageBox.DisplayMemberPath = "name";
            LanguageBox.SelectedValuePath = "value";
            string langValue = "en_US.xaml";
            if (SS_Tool_Box.Options.GetOpt("language")[0][0] != '~')
            {
                foreach (UI.Localization.localVer info in UI.Localization.indexLocals)
                {
                    if (info.value + ".xaml" == SS_Tool_Box.Options.GetOpt("language")[0] && SS_Tool_Box.Options.GetOpt("language")[0] != "diy")
                    {
                        langValue = info.value;
                        break;
                    }
                }
            }
            else
            {
                LanguageBox.SelectedValue = "diy";
            }
            LanguageBox.SelectedValue = langValue;

            load = false;
        }

        #region 事件 | 主题与颜色

        private void darkButton_Checked(object sender, RoutedEventArgs e)
        {
            UI.Color.ChangeDark(true);
            SS_Tool_Box.Options.SetOpt("darkMode", "true");
        }
        private void darkButton_Unchecked(object sender, RoutedEventArgs e)
        {
            UI.Color.ChangeDark(false);
            SS_Tool_Box.Options.SetOpt("darkMode", "false");
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
                    back = UI.Localization.ChangeLanguage(selectedName, false, true);
                }
                else
                {
                    SS_Tool_Box.Options.SetOpt("language", selectedName);
                    back = UI.Localization.ChangeLanguage(selectedName, false);
                }
                if (back.IndexOf("ERR") >= 0)
                {
                    UI.ToastHelper.Show(back);
                }
            }
        }

        #endregion


    }
}
