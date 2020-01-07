using Newtonsoft.Json.Linq;
using SS_Tool_Box.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace SS_Tool_Box_By_WPF
{
    /// <summary>
    /// PageMain.xaml 的交互逻辑
    /// </summary>
    public partial class PageMain : Page
    {

        Error error = new Error();
        BaseColor baseColora = Main.baseColor;

        public PageMain()
        {
            InitializeComponent();

            String stTitle;
            String UserName = Environment.UserName;
            if (DateTime.Now.ToString("MM").Equals("01"))
            {
                stTitle = DateTime.Now.ToString("yyyy") + "年快乐，" + UserName + "。";
            }
            else
            {
                stTitle = "你好，" + UserName + "。";
            }
            String stSays = "你好丫，欢迎使用林槐工具箱！Hummm它就只是个工具箱而已。";

            try
            {
                String saysuri = "https://v1.hitokoto.cn/";
                string GetJson = HttpUitls.Get(saysuri);
                if (GetJson.IndexOf("hitokoto") != -1)
                {
                    JObject obj = JObject.Parse(GetJson);
                    stSays = "     " + obj["hitokoto"].ToString() + " —— " + obj["from"].ToString();
                }
                else
                {
                    error.logWriter("发现错误（MAN - 001）：获取一言内容为空。", false);
                    stSays = "你好丫，欢迎使用林槐工具箱！Hummm它就只是个工具箱而已。";
                }
            }
            catch(Exception ex)
            {
                error.logWriter("发现错误（MAN - 002）：获取一言失败，错误内容为：" + ex, false);
                stSays = "你好丫，欢迎使用林槐工具箱！Hummm它就只是个工具箱而已。";
            }
            this.Says.Foreground = baseColora.Fg;
            this.Says.FontFamily = baseColora.Fonts;
            this.Says.FontSize = 14;
            Says.Text = stSays;
            String stTips = "      1.这个程序还在开发哦\n      2.第一次使用一定要去“设置 > Q&A”去看看使用建议啦";
            this.Tips.Foreground = baseColora.Fg;
            this.Tips.FontFamily = baseColora.Fonts;
            this.Tips.FontSize = 14;
            Tips.Text = stTips;

            this.Title.Foreground = baseColora.Fg;
            this.Title.FontFamily = baseColora.Fonts;
            this.Title.FontSize = 17;
            Title.Text = stTitle;

            this.MT1.Foreground = baseColora.Fg;
            this.MT1.FontFamily = baseColora.Fonts;
            this.MT1.FontSize = 15;
            this.MT2.Foreground = baseColora.Fg;
            this.MT2.FontFamily = baseColora.Fonts;
            this.MT2.FontSize = 15;

            this.UpdateList.FontFamily = baseColora.Fonts;

            BaseColor baseColor = new BaseColor();

            CD1.Background = baseColora.Card;
            CD2.Background = baseColora.Card;
            CD3.Background = baseColora.Card;

            Title.Foreground = baseColora.Fg;
            MT1.Foreground = baseColora.Fg;
            MT2.Foreground = baseColora.Fg;
            Says.Foreground = baseColora.Fg;
            Tips.Foreground = baseColora.Fg;
            UpdateList.Foreground = baseColora.Fg;

            this.UpdateList.Foreground = baseColora.Fg;
            this.UpdateList.FontFamily = baseColora.Fonts;
            this.UpdateList.FontSize = 10;

        }
    }
}
