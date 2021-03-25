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
        /// 工具相关
        /// </summary>
        public class Tools
        {
            public class ToolVer
            {
                public ToolVer(string name, object page, string type, List<string> cardInfo)
                {
                    this.name = name;
                    this.type = type;
                    this.cardInfo = cardInfo;
                    this.page = page;
                }

                public string name { get; }
                public string type { get; }
                public List<string> cardInfo { get; }
                public object page { get; }
            }

            public List<ToolVer> List = new List<ToolVer>() {
                new ToolVer("color", null, "ColorTools", new List<string>(){"调色板", "调色板和格式转换", "#BE4BDB", "M204.3 5C104.9 24.4 24.8 104.3 5.2 203.4c-37 187 131.7 326.4 258.8 306.7 41.2-6.4 61.4-54.6 42.5-91.7-23.1-45.4 9.9-98.4 60.9-98.4h79.7c35.8 0 64.8-29.6 64.9-65.3C511.5 97.1 368.1-26.9 204.3 5zM96 320c-17.7 0-32-14.3-32-32s14.3-32 32-32 32 14.3 32 32-14.3 32-32 32zm32-128c-17.7 0-32-14.3-32-32s14.3-32 32-32 32 14.3 32 32-14.3 32-32 32zm128-64c-17.7 0-32-14.3-32-32s14.3-32 32-32 32 14.3 32 32-14.3 32-32 32zm128 64c-17.7 0-32-14.3-32-32s14.3-32 32-32 32 14.3 32 32-14.3 32-32 32z"}),
                new ToolVer("color_card", new ColorCard(), "ColorTools", new List<string>(){"色卡", "一些好康的颜色们", "#15AABF", "M434.66,167.71h0L344.5,77.36a31.83,31.83,0,0,0-45-.07h0l-.07.07L224,152.88V424L434.66,212.9A32,32,0,0,0,434.66,167.71ZM480,320H373.09L186.68,506.51c-2.06,2.07-4.5,3.58-6.68,5.49H480a32,32,0,0,0,32-32V352A32,32,0,0,0,480,320ZM192,32A32,32,0,0,0,160,0H32A32,32,0,0,0,0,32V416a96,96,0,0,0,192,0ZM96,440a24,24,0,1,1,24-24A24,24,0,0,1,96,440Zm32-184H64V192h64Zm0-128H64V64h64Z"}),
                new ToolVer("xiaoshou", new XiaoshouDoing(), "OtherTools", new List<string>(){"Doing", "晓狩 Doing 客户端", "#FD7E14", "M8 256C8 119 119 8 256 8s248 111 248 248-111 248-248 248S8 393 8 256zm143.6 28.9l72.4-75.5V392c0 13.3 10.7 24 24 24h16c13.3 0 24-10.7 24-24V209.4l72.4 75.5c9.3 9.7 24.8 9.9 34.3.4l10.9-11c9.4-9.4 9.4-24.6 0-33.9L273 107.7c-9.4-9.4-24.6-9.4-33.9 0L106.3 240.4c-9.4 9.4-9.4 24.6 0 33.9l10.9 11c9.6 9.5 25.1 9.3 34.4-.4z"}),

                new ToolVer("n2_tools", new N2Tools(), "OtherTools", new List<string>(){"N2 Tools", "N2 Station Tools", "#4C6EF5", "M512 128V32c0-17.67-14.33-32-32-32h-96c-17.67 0-32 14.33-32 32H160c0-17.67-14.33-32-32-32H32C14.33 0 0 14.33 0 32v96c0 17.67 14.33 32 32 32v192c-17.67 0-32 14.33-32 32v96c0 17.67 14.33 32 32 32h96c17.67 0 32-14.33 32-32h192c0 17.67 14.33 32 32 32h96c17.67 0 32-14.33 32-32v-96c0-17.67-14.33-32-32-32V160c17.67 0 32-14.33 32-32zm-96-64h32v32h-32V64zM64 64h32v32H64V64zm32 384H64v-32h32v32zm352 0h-32v-32h32v32zm-32-96h-32c-17.67 0-32 14.33-32 32v32H160v-32c0-17.67-14.33-32-32-32H96V160h32c17.67 0 32-14.33 32-32V96h192v32c0 17.67 14.33 32 32 32h32v192z"}),

                new ToolVer("kill_stl", new KTL(), "Hidden", new List<string>(){"Kill STL", "Do you wanna kill STL?", "#4C6EF5", "M207.9 15.2c.8 4.7 16.1 94.5 16.1 128.8 0 52.3-27.8 89.6-68.9 104.6L168 486.7c.7 13.7-10.2 25.3-24 25.3H80c-13.7 0-24.7-11.5-24-25.3l12.9-238.1C27.7 233.6 0 196.2 0 144 0 109.6 15.3 19.9 16.1 15.2 19.3-5.1 61.4-5.4 64 16.3v141.2c1.3 3.4 15.1 3.2 16 0 1.4-25.3 7.9-139.2 8-141.8 3.3-20.8 44.7-20.8 47.9 0 .2 2.7 6.6 116.5 8 141.8.9 3.2 14.8 3.4 16 0V16.3c2.6-21.6 44.8-21.4 48-1.1zm119.2 285.7l-15 185.1c-1.2 14 9.9 26 23.9 26h56c13.3 0 24-10.7 24-24V24c0-13.2-10.7-24-24-24-82.5 0-221.4 178.5-64.9 300.9z"}),

            };
        }

        /// <summary>
        /// 吐司相关
        /// </summary>
        public class ToastHelper
        {

            private static Queue toastList = new Queue();
            static Toast toast;

            /// <summary>
            /// 吐司管理器
            /// </summary>
            public static void StartShower()
            {
                // 运行吐司显示
                Thread rToast = new Thread(runToast);
                rToast.Start();
            }

            private static void runToast()
            {
                while (!MainWindow.readyExit)
                {
                    while (toastList.Count != 0)
                    {
                        Show(toastList.Dequeue().ToString());
                    }
                }
            }

            public static void Show(string says)
            {
                Toast.says = says;
                Log.AddLog("ui", "显示了一条吐司：" + Toast.says);
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    toast = new Toast();
                    toastList.TrimToSize();
                    toast.Owner = Application.Current.MainWindow;
                    toast.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    toast.Show();
                });
                Thread.Sleep(2000);
                if (!MainWindow.readyExit)
                {
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        toast.Close();
                    });
                }
            }

            public static void Add(string says)
            {
                toastList.Enqueue(says);
            }
        }

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
