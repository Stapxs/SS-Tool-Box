﻿using Newtonsoft.Json.Linq;
using SS_Tool_Box.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SS_Tool_Box_By_WPF
{
    /// <summary>
    /// PageMain.xaml 的交互逻辑
    /// </summary>
    public partial class PageMain : Page
    {

        Error error = new Error();
        BaseColor baseColora = Main.baseColor;

        DateTime loadingtime;
        String stSays = "你好丫，欢迎使用林槐工具箱！Hummm它就只是个工具箱而已。";

        public PageMain()
        {
            InitializeComponent();
            //title文本
            String stTitle;
            String UserName = Environment.UserName; 
            loadingtime = DateTime.Now;
            if (DateTime.Now.ToString("MM").Equals("01") && int.Parse(DateTime.Now.ToString("dd")) < 8)
            {
                stTitle = DateTime.Now.ToString("yyyy") + "年快乐，" + UserName + "。";
            }
            else
            {
                stTitle = "你好，" + UserName + "。";
            }

            /*
            try
            {
                string wnlurl = "https://sp0.baidu.com/8aQDcjqpAAV3otqbppnN2DJv/api.php?query=" + DateTime.Now.ToString("yyyy") + "年" + DateTime.Now.ToString("MM") + "月&resource_id=6018&format=json";
                string GetJsonwnl = HttpUitls.Get(wnlurl, "DEFALT");

                GetJsonwnl = HttpUtility.UrlEncode(GetJsonwnl, Encoding.UTF8);
                GetJsonwnl = HttpUtility.UrlDecode(GetJsonwnl);
                MessageBox.Show(GetJsonwnl);
            }
            catch(Exception ex)
            {
                MessageBox.Show("" + ex);
            }
            */

            //一言
            Action action = new Action(() => {
                error.logWriter("开始获取一言", false);
                try
                {
                    String saysuri = "https://v1.hitokoto.cn/";
                    string GetJson = HttpUitls.Get(saysuri, "DEFALT");
                    if (GetJson.IndexOf("hitokoto") != -1)
                    {
                        JObject obj = JObject.Parse(GetJson);
                        stSays = "     " + obj["hitokoto"].ToString() + " —— " + obj["from"].ToString();
                        error.logWriter("获取一言成功，耗时：" + (DateTime.Now - loadingtime).ToString(), false);
                    }
                    else
                    {
                        error.logWriter("发现错误（MAN - 001）：获取一言内容为空，耗时：" + (DateTime.Now - loadingtime).ToString(), false);
                        stSays = "你好丫，欢迎使用林槐工具箱！Hummm它就只是个工具箱而已。";
                    }
                }
                catch (Exception ex)
                {
                    error.logWriter("发现错误（MAN - 002）：获取一言失败，错误内容为：" + ex + "，耗时：" + (DateTime.Now - loadingtime).ToString(), false);
                    stSays = "你好丫，欢迎使用林槐工具箱！Hummm它就只是个工具箱而已。";
                }
                
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Says.Text = stSays;
                }), DispatcherPriority.SystemIdle, null);
            });
            action.BeginInvoke(null, null);

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

            BaseColor baseColor = new BaseColor();

            CD1.Background = baseColora.Card;
            CD2.Background = baseColora.Card;
            CD3.Background = baseColora.Card;

            Title.Foreground = baseColora.Fg;
            MT1.Foreground = baseColora.Fg;
            MT2.Foreground = baseColora.Fg;
            Says.Foreground = baseColora.Fg;
            Tips.Foreground = baseColora.Fg;

            this.UpdateList.ItemLineBrush = baseColora.Font;
            this.UpdateList.ItemToggleStroke = baseColora.Tran;
            this.UpdateList.ItemToggleFill = baseColora.Main;
            this.U0.Foreground = baseColora.Font;
            this.U1.Foreground = baseColora.Font;
            this.U2.Foreground = baseColora.Font;
            this.U3.Foreground = baseColora.Font;
            this.U4.Foreground = baseColora.Font;
            this.U5.Foreground = baseColora.Font;
            this.U6.Foreground = baseColora.Font;
            this.U7.Foreground = baseColora.Font;
            this.U8.Foreground = baseColora.Font;
            this.U9.Foreground = baseColora.Font;
            this.U10.Foreground = baseColora.Font;
            this.U11.Foreground = baseColora.Font;
            this.U12.Foreground = baseColora.Font;
            this.U13.Foreground = baseColora.Font;
            this.U14.Foreground = baseColora.Font;
            this.U15.Foreground = baseColora.Font;
            this.U16.Foreground = baseColora.Font;
            this.U17.Foreground = baseColora.Font;
            this.U18.Foreground = baseColora.Font;
            this.U19.Foreground = baseColora.Font;

            this.Height = 477;
        }

        /*
         * 以下是程序无关的辅助算法 
         */

        private string GB2312ToUTF8(string str)
        {
            try
            {
                Encoding utf8 = Encoding.UTF8;
                Encoding gb2312 = Encoding.GetEncoding("GB2312");
                byte[] unicodeBytes = gb2312.GetBytes(str);
                byte[] asciiBytes = Encoding.Convert(gb2312, utf8, unicodeBytes);
                char[] asciiChars = new char[utf8.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
                utf8.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);
                string result = new string(asciiChars);
                return result;
            }
            catch
            {
                return "Error";
            }
        }
    }
}
