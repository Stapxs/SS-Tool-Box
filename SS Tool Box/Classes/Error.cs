using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Panuon.UI.Silver;

namespace SS_Tool_Box.Classes
{
    public class Error
    {
        public void ErrorTo(String ErrorString, ProgressBar Percent, TextBlock Errorsay)
        {
            Errorsay.Visibility = Visibility.Collapsed;
            logWriter(ErrorString, false);
            Errorsay.Text = ErrorString + "\n(详细报错已写入log)";

            System.Windows.Media.SolidColorBrush ColorBrush = new SolidColorBrush();
            ColorBrush.Color = System.Windows.Media.Color.FromArgb(255, 209, 93, 93);
            FontFamily font = new FontFamily("方正兰亭简黑");

            Percent.Foreground = ColorBrush;
            Errorsay.Foreground = ColorBrush;
            Errorsay.FontFamily = font;
            Errorsay.FontSize = 13;

            Errorsay.Visibility = Visibility.Visible;
        }

        public void ErrorTo(String Hander, String ErrorString, ProgressBar Percent, TextBlock Errorsay)
        {
            Errorsay.Visibility = Visibility.Collapsed;
            logWriter(Hander + ErrorString, false);

            ErrorString = StringCut(ErrorString);

            Errorsay.Text = Hander + ErrorString + "\n(详细报错已写入log)";
            System.Windows.Media.SolidColorBrush ColorBrush = new SolidColorBrush();
            ColorBrush.Color = System.Windows.Media.Color.FromArgb(255, 209, 93, 93);
            FontFamily font = new FontFamily("方正兰亭简黑");

            Percent.Foreground = ColorBrush;
            Errorsay.Foreground = ColorBrush;
            Errorsay.FontFamily = font;
            Errorsay.FontSize = 13;

            Errorsay.Visibility = Visibility.Visible;
        }

        public bool logWriter(String logString, bool mode)
        {
            String nowTime = DateTime.Now.TimeOfDay.ToString();
            logString = "[" + nowTime.Substring(0, nowTime.IndexOf(".") + 3) + "] " + logString;
            if (!Directory.Exists("SSTB/Log"))
            {
                Directory.CreateDirectory("SSTB/Log");
            }
            try
            {
                File.AppendAllText("SSTB/Log/log.log", "\r\n" + logString);
            }
            catch(Exception ex)
            {
                MessageBox.Show("写入Log文件错误：" + ex);
                return false;
            }
            return true;
        }

        /*
         * 以下是功能无关的操作方法
         */
        public String StringCut(String ErrorString)
        {
            return ErrorString.Substring(0, ErrorString.IndexOf("在"));
        }
    }
}
