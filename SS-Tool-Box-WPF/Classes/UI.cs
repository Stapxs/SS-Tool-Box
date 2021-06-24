using SS_Tool_Box.Pages.Tools;
using SS_Tool_Box.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SS_Tool_Box.Function
{
    class UI
    {
        /// <summary>
        /// 截图相关
        /// </summary>
        public class Cutshot
        {
            /// <summary>
            /// 获取控件截图，From https://www.cnblogs.com/likui-bookHouse/p/11114924.html
            /// </summary>
            /// <param name="element"></param>
            /// <param name="width">默认控件宽度</param>
            /// <param name="height">默认控件高度</param>
            /// <param name="x">默认0</param>
            /// <param name="y">默认0</param>
            /// <returns></returns>
            public static Bitmap GetBitmap(FrameworkElement element, int width = 0, int height = 0, int x = 0, int y = 0)
            {
                if (width == 0) width = (int)element.ActualWidth;
                if (height == 0) height = (int)element.ActualHeight;

                var rtb = new RenderTargetBitmap(width, height, x, y, System.Windows.Media.PixelFormats.Default);
                rtb.Render(element);
                var bit = Features.BitmapSourceToBitmap(rtb);

                return bit;
            }

            /// <summary>
            /// 获取窗口截图，From https://www.codenong.com/5124825/
            /// </summary>
            /// <param name="target"></param>
            public static Bitmap CreateBitmapFromVisual(Visual target)
            {
                Rect bounds = VisualTreeHelper.GetDescendantBounds(target);

                RenderTargetBitmap renderTarget = new RenderTargetBitmap((Int32)bounds.Width, (Int32)bounds.Height, 96, 96, PixelFormats.Pbgra32);

                DrawingVisual visual = new DrawingVisual();

                using (DrawingContext context = visual.RenderOpen())
                {
                    VisualBrush visualBrush = new VisualBrush(target);
                    context.DrawRectangle(visualBrush, null, new Rect(new System.Windows.Point(), bounds.Size));
                }

                renderTarget.Render(visual);
                BitmapSource bmp = renderTarget;

                return Features.BitmapSourceToBitmap(bmp);
            }

            public static Bitmap getScreen()
            {
                Bitmap bm = new Bitmap(Convert.ToInt32(SystemParameters.PrimaryScreenWidth), Convert.ToInt32(SystemParameters.PrimaryScreenHeight));
                Graphics g = Graphics.FromImage(bm);
                g.CopyFromScreen(0, 0, 0, 0, bm.Size);
                return bm;
            }

        }

        /// <summary>
        /// 主题与颜色
        /// </summary>
        public class Color
        {
            private static SolidColorBrush brush = (SolidColorBrush)Application.Current.FindResource("colorMainBlue");

            private static List<string[]> colorUIDark = new List<string[]> {
                new string[] { "colorBg", "#2D2D2D"},
                new string[] { "colorCard", "#3A3A3A"},
                new string[] { "colorCard1", "#494949"},
                new string[] { "colorCard2", "#5F5F5F"},

                new string[] { "colorFont", "#FFFFFF"},
                new string[] { "colorFont1", "#CFCFCF"},
                new string[] { "colorFont2", "#B0B0B0"},
            };
            private static List<string[]> colorUI = new List<string[]> {
                new string[] { "colorBg", "#F2F4F3"},
                new string[] { "colorCard", "#FDFFFD"},
                new string[] { "colorCard1", "#F2F4F3"},
                new string[] { "colorCard2", "#E8EBE8"},

                new string[] { "colorFont", "#50534F"},
                new string[] { "colorFont1", "#969FB3"},
                new string[] { "colorFont2", "#969FB3"},
            };
            public static bool ChangeDark(bool isDark)
            {
                List<string[]> colorUI;
                if (isDark)
                {
                    colorUI = colorUIDark;
                }
                else
                {
                    colorUI = Color.colorUI;
                }
                foreach(string[] info in colorUI)
                {
                    Application.Current.Resources.Remove(info[0]);
                    Application.Current.Resources.Add(info[0],
                        new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(info[1])));
                }
                return false;
            }
        }

        /// <summary>
        /// 本土化
        /// </summary>
        public class Localization
        {

            public class localVer
            {

                public localVer(string v1, string v2)
                {
                    name = v1;
                    value = v2;
                }

                public string name { get; }
                public string value { get; }
            }

            public static List<localVer> indexLocals = new List<localVer>
            {
                new localVer("中文（简体）", "zh_CN"),
                new localVer("English", "en_US"),

                new localVer("Select ...", "diy"),
            };

            public static ResourceDictionary langUsed = null;
            private static string usedLang = @"Lang\en_US.xaml";

            public static string ChangeLanguage(string name, bool isFist, bool isOut = false)
            {
                usedLang = @"Lang\" + name;
                try
                {
                    if (!isOut)
                    {
                        langUsed = (ResourceDictionary)Application.LoadComponent(new Uri(@"Lang\" + name, UriKind.Relative));
                    }
                    else
                    {
                        FileStream fs = new FileStream(@"Lang\" + name, FileMode.Open);
                        langUsed = (ResourceDictionary)XamlReader.Load(fs);
                    }
                }
                catch (Exception e)
                {
                    return "ERR - " + e.Message;
                }
                if (langUsed != null)
                {
                    Application app = Application.Current;
                    // 如果已使用其他语言,先清空
                    if (app.Resources.MergedDictionaries.Count > 0)
                    {
                        List<ResourceDictionary> reses = new List<ResourceDictionary>();
                        foreach (ResourceDictionary res in app.Resources.MergedDictionaries)
                        {
                            if (res.Source != null && res.Source.ToString() == new Uri(usedLang, UriKind.Relative).ToString())
                            {
                                reses.Add(res);
                            }
                        }
                        foreach (ResourceDictionary res in reses)
                        {
                            app.Resources.MergedDictionaries.Remove(res);
                        }
                    }
                    app.Resources.MergedDictionaries.Add(langUsed);
                }
                if (!File.Exists(@"Lang\" + name + ".xaml"))
                {
                    return "ERR - 文件不存在";
                }
                return "OK";
            }
        }

    }
}
