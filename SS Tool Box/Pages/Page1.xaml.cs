using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Panuon.UI.Silver;
using SS_Tool_Box.Classes;
using SS_Tool_Box_By_WPF;

namespace SS_Tool_Box
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class Page1 : Page
    {
        BaseColor baseColora = Main.baseColor;
        Error error = new Error();
        int PercentP = 0;
        Brush ColorFst;

        public Page1()
        {
            InitializeComponent();
            this.Height = 500;

            String stTitle = "批量打开文件";
            this.Title.Foreground = baseColora.Fg;
            this.Title.FontFamily = baseColora.Fonts;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            String stSays = "批量打开文件，打开的方式为它的默认打开方式。";
            this.Says.Foreground = baseColora.Fg;
            this.Says.FontFamily = baseColora.Fonts;
            this.Says.FontSize = 14;
            Says.Text = stSays;

            this.T1.Foreground = baseColora.Fg;
            this.T1.FontFamily = baseColora.Fonts;
            this.T1.FontSize = 13;
            this.T2.Foreground = baseColora.Fg;
            this.T2.FontFamily = baseColora.Fonts;
            this.T2.FontSize = 13;
            this.T4.Foreground = baseColora.Fg;
            this.T4.FontFamily = baseColora.Fonts;
            this.T4.FontSize = 13;

            this.MT1.Foreground = baseColora.Fg;
            this.MT1.FontFamily = baseColora.Fonts;
            this.MT1.FontSize = 15;

            this.RunCard.Background = baseColora.Card;
            this.CD1.Background = baseColora.Card;
            this.CD2.Background = baseColora.Card;

            this.Address.Background = baseColora.Bg;
            this.AddressAdd.Background = baseColora.Bg;
            this.Address.Foreground = baseColora.Fg;
            this.AddressAdd.Foreground = baseColora.Fg;

            this.Percent.Background = baseColora.DBg;
            
            IconHelper.SetForeground(RunButton, baseColora.Fg);
            IconHelper.SetForeground(ClearButton, baseColora.Fg);

            this.RunButton.Foreground = baseColora.Fg;
            this.ClearButton.Foreground = baseColora.Fg;
            this.RunButton.Background = baseColora.Tran;
            this.ClearButton.Background = baseColora.Tran;

            this.RunCard.Visibility = Visibility.Collapsed;
            ColorFst = this.Percent.Foreground;
        }

        private void RunTool(object sebder, RoutedEventArgs s)
        {
            this.RunCard.Visibility = Visibility.Visible;
            ProgressBarHelper.SetAnimateTo(Percent, 0);
            this.Errorsay.Visibility = Visibility.Collapsed;
            bool Error;
            ButtonHelper.SetIsWaiting(RunButton, true);
            Error = SeachFile(Address.Text);
            ButtonHelper.SetIsWaiting(RunButton, false);
            if (Error)
            {
                this.Percent.Foreground = ColorFst;
                ProgressBarHelper.SetAnimateTo(Percent, 100);
            }
            Error = false;
        }

        private bool SeachFile(string Sdir)
        {
            DirectoryInfo dir = new DirectoryInfo("C:");
            try
            {
                dir = new DirectoryInfo(Sdir);
            }
            catch (Exception ex)
            {
                error.ErrorTo("发现错误（OPF - 003）：在检索文件时发生未知错误,错误原因为文件或目录格式有误。", Percent, Errorsay);
                return false;
            }
            try
            {
                //判断所指的文件夹/文件是否存在  
                if (!dir.Exists)
                {
                    error.ErrorTo("发现错误（OPF - 002）：在检索文件时发生未知错误,错误原因为文件或目录不存在。", Percent, Errorsay);
                    return false;
                }
                int filenum = 0;
                DirectoryInfo dirD = dir as DirectoryInfo;
                FileSystemInfo[] files = dirD.GetFileSystemInfos();//获取文件夹下所有文件和文件夹  
                //对单个FileSystemInfo进行判断,统计个数
                String add = "";
                foreach (FileSystemInfo FSys in files)
                {
                    FileInfo fileInfo = FSys as FileInfo;
                    if (fileInfo != null)
                    {
                        if (String.IsNullOrWhiteSpace(AddressAdd.Text))
                        {
                            filenum++;
                        }
                        else if (!(String.IsNullOrWhiteSpace(AddressAdd.Text)))
                        {
                            if (AddressAdd.Text.LastIndexOf("*.") != -1)
                            {
                                string[] sArray = AddressAdd.Text.Split('*');
                                add = sArray[1];
                            }
                            else
                            {
                                error.ErrorTo("发现错误（OPF - 004）：在检索文件时发生未知错误,错误原因为附加后缀格式有误。", Percent, Errorsay);
                                return false;
                            }
                            if (fileInfo.Name.LastIndexOf(add) != -1)
                            {
                                filenum++;
                            }
                        }
                    }
                }

                foreach (FileSystemInfo FSys in files)
                {
                    FileInfo fileInfo = FSys as FileInfo;

                    if (fileInfo != null)
                    {
                        //如果是文件，进行文件操作  
                        String Address = "";
                        FileInfo SFInfo = new FileInfo(fileInfo.DirectoryName + "\\" + fileInfo.Name);//获取文件所在原始路径
                        if (String.IsNullOrWhiteSpace(AddressAdd.Text))
                        {
                            Address = fileInfo.DirectoryName + "\\" + fileInfo.Name;
                        }
                        else if (!(String.IsNullOrWhiteSpace(AddressAdd.Text)))
                        {
                            if(fileInfo.Name.LastIndexOf(add) != -1)
                            {
                                Address = fileInfo.DirectoryName + "\\" + fileInfo.Name;
                            }
                            else
                            {
                                continue;
                            }

                        }
                        
                        Process process;

                        Errorsay.Visibility = Visibility.Collapsed;
                        Errorsay.Text = "打开成功,共打开了 " + filenum + " 个文件。";
                        PercentP = PercentP + 100 / filenum;
                        ProgressBarHelper.SetAnimateTo(Percent, PercentP);

                        process = System.Diagnostics.Process.Start(@Address);

                        System.Windows.Media.SolidColorBrush ColorBrush = new SolidColorBrush();
                        ColorBrush.Color = System.Windows.Media.Color.FromArgb(255, 100, 100, 100);
                        FontFamily font = new FontFamily("方正兰亭简黑");

                        Percent.Foreground = ColorFst;
                        Errorsay.Foreground = ColorBrush;
                        Errorsay.FontFamily = font;
                        Errorsay.FontSize = 13;

                        Errorsay.Visibility = Visibility.Visible;
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                error.ErrorTo("发现错误（OPF - 001）：在检索文件时发生未知错误,错误内容为\n" + ex + "\n错误原因未知，请及时反馈。", Percent, Errorsay);
                MessageBoxX.Show("发现错误（OPF - 001），是否反馈。", "错误", Application.Current.MainWindow);
                return false;
            }
        }

        private void Clear(object sebder, RoutedEventArgs s)
        {
            this.AddressAdd.Text = "";
            this.Address.Text = "";

            this.Errorsay.Visibility = Visibility.Collapsed;
            this.RunCard.Visibility = Visibility.Collapsed;
            this.Percent.Foreground = ColorFst;
            ProgressBarHelper.SetAnimateTo(Percent, 0);
        }

        private void FileRun(object sebder, RoutedEventArgs s)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Title = "选择要打开的文件类型所对应的文件以及位置";
            dialog.Filter = "所有文件(*.*)|*.*";
            if (dialog.ShowDialog() == true)
            {
                Address.Text = dialog.FileName.Substring(0, dialog.FileName.LastIndexOf("\\"));
                string AddAddPass = dialog.FileName.Substring(dialog.FileName.LastIndexOf("\\") + 1, dialog.FileName.Length - dialog.FileName.LastIndexOf("\\") - 1);
                string[] sArray = AddAddPass.Split('.');
                AddressAdd.Text = "*." + sArray[1];
            }
        }
    }
}
