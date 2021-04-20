using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SS_Tool_Box
{
    /// <summary>
    /// 模块化功能
    /// </summary>
    static class Features
    {
        #region Reg | 注册表操作
        /// <summary>
        /// 注册表操作
        /// </summary>
        public class Reg
        {
            /// <summary>
            /// 判断注册表项是否存在
            /// </summary>
            /// <param name="way">注册表项父级路径</param>
            /// <param name="name">注册表项名</param>
            /// <returns>是否存在</returns>
            public static bool IsRegeditItemExist(RegistryKey hkml, string way, string name)
            {
                string[] subkeyNames;
                RegistryKey software = hkml.OpenSubKey(way, true);
                subkeyNames = software.GetSubKeyNames();
                //取得该项下所有子项的名称的序列，并传递给预定的数组中
                foreach (string keyName in subkeyNames) //遍历整个数组
                {
                    if (keyName.Equals(name)) //判断子项的名称
                    {
                        hkml.Close();
                        return true;
                    }
                }
                hkml.Close();
                return false;
            }

            /// <summary>
            /// 创建注册表项
            /// </summary>
            /// <param name="way">注册表项路径</param>
            /// <returns>是否成功</returns>
            public static bool CreateRegItem(RegistryKey key, String way)
            {
                try
                {
                    RegistryKey software = key.CreateSubKey(way);
                }
                catch
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 判断注册表项的键值是否存在
            /// </summary>
            /// <param name="way">注册表项路径</param>
            /// <param name="name">键名</param>
            /// <returns>是否存在</returns>
            public static bool IsRegeditKeyExit(RegistryKey hkml, String way, String name)
            {
                string[] subkeyNames;
                RegistryKey software = hkml.OpenSubKey(way, true);
                subkeyNames = software.GetValueNames();
                //取得该项下所有键值的名称的序列，并传递给预定的数组中
                foreach (string keyName in subkeyNames)
                {
                    if (keyName.Equals(name)) //判断键值的名称
                    {
                        hkml.Close();
                        return true;
                    }
                }
                hkml.Close();
                return false;
            }


            /// <summary>
            /// 创建键
            /// </summary>
            /// <param name="way">键的父路径</param>
            /// <param name="name">键名</param>
            /// <param name="value">键值</param>
            /// /// <returns>是否成功</returns>
            public static bool CreateRegKey(RegistryKey key, String way, String name, String value)
            {
                try
                {
                    RegistryKey software = key.OpenSubKey(way, true); //该项必须已存在
                    software.SetValue(name, value);
                }
                catch
                {
                    return false;
                }
                return true;
            }

            public static bool DelRegKey(RegistryKey key, String way, String name)
            {
                try
                {
                    RegistryKey software = key.OpenSubKey(way, true);
                    software.DeleteValue(name);
                }
                catch
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// 获取键值
            /// </summary>
            /// <param name="way">键的父路径</param>
            /// <param name="name">键名</param>
            /// <returns></returns>
            public static String GetRegKey(RegistryKey key, String way, String name)
            {
                try
                {
                    RegistryKey software = key.OpenSubKey(way, true); //该项必须已存在
                    return software.GetValue(name).ToString();
                }
                catch
                {
                    return "ERR";
                }
            }
        }
        #endregion

        #region Time | 时间戳操作

        /// <summary>
        /// 时间相关
        /// Add by 成长的小猪（Jason.Song） on 2018/05/10
        /// http://blog.csdn.net/jasonsong2008
        /// </summary>
        public static class TimeHelper
        {
            /// <summary>
            /// 获取当前时间戳
            /// Add by 成长的小猪（Jason.Song） on 2019/05/07
            /// http://blog.csdn.net/jasonsong2008
            /// </summary>
            /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
            /// <returns></returns>
            public static string GetCurrentTimestamp(bool millisecond = true)
            {
                return ToTimestamp(DateTime.Now, millisecond);
            }

            /// <summary>
            /// 转换指定时间得到对应的时间戳
            /// Add by 成长的小猪（Jason.Song） on 2019/05/07
            /// http://blog.csdn.net/jasonsong2008
            /// </summary>
            /// <param name="dateTime"></param>
            /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
            /// <returns>返回对应的时间戳</returns>
            public static string ToTimestamp(DateTime dateTime, bool millisecond = true)
            {
                return ToTimestampLong(dateTime, millisecond).ToString();
            }

            /// <summary>
            /// 转换指定时间得到对应的时间戳
            /// Add by 成长的小猪（Jason.Song） on 2019/05/07
            /// http://blog.csdn.net/jasonsong2008
            /// </summary>
            /// <param name="dateTime"></param>
            /// <param name="millisecond">精度（毫秒）设置 true，则生成13位的时间戳；精度（秒）设置为 false，则生成10位的时间戳；默认为 true </param>
            /// <returns>返回对应的时间戳</returns>
            public static long ToTimestampLong(DateTime dateTime, bool millisecond = true)
            {
                var ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return millisecond ? Convert.ToInt64(ts.TotalMilliseconds) : Convert.ToInt64(ts.TotalSeconds);
            }

            /// <summary>
            /// 转换指定时间戳到对应的时间
            /// Add by 成长的小猪（Jason.Song） on 2019/05/07
            /// http://blog.csdn.net/jasonsong2008
            /// </summary>
            /// <param name="timestamp">（10位或13位）时间戳</param>
            /// <returns>返回对应的时间</returns>
            public static DateTime ToDateTime(string timestamp)
            {
                var tz = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
                return timestamp.Length == 13
                    ? tz.AddMilliseconds(Convert.ToInt64(timestamp))
                    : tz.AddSeconds(Convert.ToInt64(timestamp));
            }
        }

        #endregion

        #region UI | 控件操作

        /// <summary>
        /// 根据名字获取子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= System.Windows.Media.VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = System.Windows.Media.VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }

        #endregion

        #region Bitmap | 图像处理

        /// <summary>
        /// BitmapSource转Bitmap
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Bitmap BitmapSourceToBitmap(BitmapSource source)
        {
            return BitmapSourceToBitmap(source, source.PixelWidth, source.PixelHeight);
        }

        /// <summary>
        /// Convert BitmapSource to Bitmap
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Bitmap BitmapSourceToBitmap(BitmapSource source, int width, int height)
        {
            Bitmap bmp = null;
            try
            {
                PixelFormat format = PixelFormat.Format24bppRgb;
                /*set the translate type according to the in param(source)*/
                switch (source.Format.ToString())
                {
                    case "Rgb24":
                    case "Bgr24": format = PixelFormat.Format24bppRgb; break;
                    case "Bgra32": format = PixelFormat.Format32bppPArgb; break;
                    case "Bgr32": format = PixelFormat.Format32bppRgb; break;
                    case "Pbgra32": format = PixelFormat.Format32bppArgb; break;
                }
                bmp = new Bitmap(width, height, format);
                BitmapData data = bmp.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size),
                    ImageLockMode.WriteOnly,
                    format);
                source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
                bmp.UnlockBits(data);
            }
            catch
            {
                if (bmp != null)
                {
                    bmp.Dispose();
                    bmp = null;
                }
            }

            return bmp;
        }

        /// <summary>
        /// 获取图片主题色，From https://bbs.csdn.net/topics/390773598
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Color getMajorColor(Bitmap bitmap)
        {
            //色调的总和
            var sum_hue = 0d;
            //色差的阈值
            var threshold = 30;
            //计算色调总和
            for (int h = 0; h < bitmap.Height; h++)
            {
                for (int w = 0; w < bitmap.Width; w++)
                {
                    var hue = bitmap.GetPixel(w, h).GetHue();
                    sum_hue += hue;
                }
            }
            var avg_hue = sum_hue / (bitmap.Width * bitmap.Height);

            //色差大于阈值的颜色值
            var rgbs = new List<Color>();
            for (int h = 0; h < bitmap.Height; h++)
            {
                for (int w = 0; w < bitmap.Width; w++)
                {
                    var color = bitmap.GetPixel(w, h);
                    var hue = color.GetHue();
                    //如果色差大于阈值，则加入列表
                    if (Math.Abs(hue - avg_hue) > threshold)
                    {
                        rgbs.Add(color);
                    }
                }
            }
            if (rgbs.Count == 0)
                return Color.Black;
            //计算列表中的颜色均值，结果即为该图片的主色调
            int sum_r = 0, sum_g = 0, sum_b = 0;
            foreach (var rgb in rgbs)
            {
                sum_r += rgb.R;
                sum_g += rgb.G;
                sum_b += rgb.B;
            }
            return Color.FromArgb(sum_r / rgbs.Count,
                sum_g / rgbs.Count,
                sum_b / rgbs.Count);
        }

        /// <summary>
        /// 裁切 Bitmap
        /// </summary>
        /// <param name="src"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap CutPic(Bitmap src, int x, int y, int width, int height)
        {
            Rectangle cropRect = new Rectangle(x, y, width, height);
            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, target.Width, target.Height),
                      cropRect,
                      GraphicsUnit.Pixel);
            }
            return target;
        }

        /// <summary>
        /// 获取最多的颜色，From https://www.codeproject.com/Questions/677506/Csharp-find-the-majority-color-of-an-image
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static List<Color> GetBaseColor(Bitmap bmp)
        {
            List<Color> TenMostUsedColors = new List<Color>();

            // does using Dictionary<int,int> here
            // really pay-off compared to using
            // Dictionary<Color, int> ?

            // would using a SortedDictionary be much slower, or ?

            Dictionary<int, int> dctColorIncidence = new Dictionary<int, int>();

            // this is what you want to speed up with unmanaged code
            for (int row = 0; row < bmp.Size.Width; row++)
            {
                for (int col = 0; col < bmp.Size.Height; col++)
                {
                    int pixelColor = bmp.GetPixel(row, col).ToArgb();

                    if (dctColorIncidence.Keys.Contains(pixelColor))
                    {
                        dctColorIncidence[pixelColor]++;
                    }
                    else
                    {
                        dctColorIncidence.Add(pixelColor, 1);
                    }
                }
            }

            // note that there are those who argue that a
            // .NET Generic Dictionary is never guaranteed
            // to be sorted by methods like this
            var dctSortedByValueHighToLow = dctColorIncidence.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

            // this should be replaced with some elegant Linq ?
            foreach (KeyValuePair<int, int> kvp in dctSortedByValueHighToLow.Take(10))
            {
                TenMostUsedColors.Add(Color.FromArgb(kvp.Key));
            }

            return TenMostUsedColors;
        }

        #endregion

        #region Net | 网络操作

        /// <summary>
        /// Ping 测试
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static PingReply PingTest(string ip)
        {
            PingReply reply = null;
            Ping pingSender = null;
            try
            {
                pingSender = new Ping();

                PingOptions options = new PingOptions();
                options.DontFragment = true;

                string data = "hello world";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 1000;

                IPAddress ipa = IPAddress.Parse(ip);
                PingReply replyPing = pingSender.Send(ip, timeout, buffer, options);
                reply = replyPing;
            }
            catch (Exception ex)
            {
                reply = null;
            }
            finally
            {
                pingSender.Dispose();
            }
            return reply;
        }

        #endregion

    }
}
