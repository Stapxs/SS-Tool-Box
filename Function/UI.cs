using SS_Tool_Box.Pages.Tools;
using SS_Tool_Box.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

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
                public ToolVer(object page, string type, List<string> cardInfo)
                {
                    this.type = type;
                    this.cardInfo = cardInfo;
                    this.page = page;
                }

                public string type { get; }
                public List<string> cardInfo { get; }
                public object page { get; }
            }

            public static List<ToolVer> List = new List<ToolVer>() {
                new ToolVer(null, "ColorTools", new List<string>(){"调色板", "调色板和格式转换", "#FFBE4BDB", "M204.3 5C104.9 24.4 24.8 104.3 5.2 203.4c-37 187 131.7 326.4 258.8 306.7 41.2-6.4 61.4-54.6 42.5-91.7-23.1-45.4 9.9-98.4 60.9-98.4h79.7c35.8 0 64.8-29.6 64.9-65.3C511.5 97.1 368.1-26.9 204.3 5zM96 320c-17.7 0-32-14.3-32-32s14.3-32 32-32 32 14.3 32 32-14.3 32-32 32zm32-128c-17.7 0-32-14.3-32-32s14.3-32 32-32 32 14.3 32 32-14.3 32-32 32zm128-64c-17.7 0-32-14.3-32-32s14.3-32 32-32 32 14.3 32 32-14.3 32-32 32zm128 64c-17.7 0-32-14.3-32-32s14.3-32 32-32 32 14.3 32 32-14.3 32-32 32z"}),
                new ToolVer(new ColorCard(), "ColorTools", new List<string>(){"色卡", "一些好康的颜色们", "#FF15AABF", "M434.66,167.71h0L344.5,77.36a31.83,31.83,0,0,0-45-.07h0l-.07.07L224,152.88V424L434.66,212.9A32,32,0,0,0,434.66,167.71ZM480,320H373.09L186.68,506.51c-2.06,2.07-4.5,3.58-6.68,5.49H480a32,32,0,0,0,32-32V352A32,32,0,0,0,480,320ZM192,32A32,32,0,0,0,160,0H32A32,32,0,0,0,0,32V416a96,96,0,0,0,192,0ZM96,440a24,24,0,1,1,24-24A24,24,0,0,1,96,440Zm32-184H64V192h64Zm0-128H64V64h64Z"}),
                new ToolVer(new XiaoshouDoing(), "OtherTools", new List<string>(){"Doing", "晓狩 Doing 客户端", "#FFFD7E14", "M8 256C8 119 119 8 256 8s248 111 248 248-111 248-248 248S8 393 8 256zm143.6 28.9l72.4-75.5V392c0 13.3 10.7 24 24 24h16c13.3 0 24-10.7 24-24V209.4l72.4 75.5c9.3 9.7 24.8 9.9 34.3.4l10.9-11c9.4-9.4 9.4-24.6 0-33.9L273 107.7c-9.4-9.4-24.6-9.4-33.9 0L106.3 240.4c-9.4 9.4-9.4 24.6 0 33.9l10.9 11c9.6 9.5 25.1 9.3 34.4-.4z"}),
                new ToolVer(new N2Tools(), "OtherTools", new List<string>(){"N2 Tools", "N2 Station Tools", "#FF4C6EF5", "M512 128V32c0-17.67-14.33-32-32-32h-96c-17.67 0-32 14.33-32 32H160c0-17.67-14.33-32-32-32H32C14.33 0 0 14.33 0 32v96c0 17.67 14.33 32 32 32v192c-17.67 0-32 14.33-32 32v96c0 17.67 14.33 32 32 32h96c17.67 0 32-14.33 32-32h192c0 17.67 14.33 32 32 32h96c17.67 0 32-14.33 32-32v-96c0-17.67-14.33-32-32-32V160c17.67 0 32-14.33 32-32zm-96-64h32v32h-32V64zM64 64h32v32H64V64zm32 384H64v-32h32v32zm352 0h-32v-32h32v32zm-32-96h-32c-17.67 0-32 14.33-32 32v32H160v-32c0-17.67-14.33-32-32-32H96V160h32c17.67 0 32-14.33 32-32V96h192v32c0 17.67 14.33 32 32 32h32v192z"}),
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
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    toast.Close();
                });
            }

            public static void Add(string says)
            {
                toastList.Enqueue(says);
            }
        }

        /// <summary>
        /// 弹窗相关
        /// </summary>
        public class MessageBoxHelper
        {
            public static void Show(List<string> info)
            {

            }
        }

    }
}
