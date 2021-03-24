using SS_Tool_Box.Function;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SS_Tool_Box.Pages
{
    /// <summary>
    /// Options.xaml 的交互逻辑
    /// </summary>
    public partial class Options : Page
    {
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
            string langValue = "en_US";
            foreach(UI.Localization.localVer info in UI.Localization.indexLocals)
            {
                if(info.value == SS_Tool_Box.Options.GetOpt("language")[0])
                {
                    langValue = info.value;
                    break;
                }
            }
            LanguageBox.SelectedValue = langValue;
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
            string selectedName = ((ComboBox)sender).SelectedValue.ToString();
            SS_Tool_Box.Options.SetOpt("language", selectedName);
            UI.Localization.ChangeLanguage(selectedName, false);
        }

        #endregion


    }
}
