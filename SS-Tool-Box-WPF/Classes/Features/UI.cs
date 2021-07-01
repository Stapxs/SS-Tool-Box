using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SS_Tool_Box.Function
{
    partial class UI
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
            public Bitmap GetBitmap(FrameworkElement element, int width = 0, int height = 0, int x = 0, int y = 0)
            {
                if (width == 0) width = (int)element.ActualWidth;
                if (height == 0) height = (int)element.ActualHeight;

                var rtb = new RenderTargetBitmap(width, height, x, y, System.Windows.Media.PixelFormats.Default);
                rtb.Render(element);
                var bit = new ImageChange().BitmapSourceToBitmap(rtb);

                return bit;
            }

            /// <summary>
            /// 获取窗口截图，From https://www.codenong.com/5124825/
            /// </summary>
            /// <param name="target"></param>
            public Bitmap CreateBitmapFromVisual(Visual target)
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

                return new ImageChange().BitmapSourceToBitmap(bmp);
            }

            public Bitmap getScreen()
            {
                Bitmap bm = new Bitmap(Convert.ToInt32(SystemParameters.PrimaryScreenWidth), Convert.ToInt32(SystemParameters.PrimaryScreenHeight));
                Graphics g = Graphics.FromImage(bm);
                g.CopyFromScreen(0, 0, 0, 0, bm.Size);
                return bm;
            }

        }

        /// <summary>
        /// 图片处理相关
        /// </summary>
        public class ImageChange
        {
            /// <summary>
            /// BitmapSource转Bitmap
            /// </summary>
            /// <param name="source"></param>
            /// <returns></returns>
            public Bitmap BitmapSourceToBitmap(BitmapSource source)
            {
                return BitmapSourceToBitmap(source, source.PixelWidth, source.PixelHeight);
            }

            /// <summary>
            /// Convert BitmapSource to Bitmap
            /// </summary>
            /// <param name="source"></param>
            /// <returns></returns>
            public Bitmap BitmapSourceToBitmap(BitmapSource source, int width, int height)
            {
                Bitmap bmp = null;
                try
                {
                    System.Drawing.Imaging.PixelFormat format = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                    /*set the translate type according to the in param(source)*/
                    switch (source.Format.ToString())
                    {
                        case "Rgb24":
                        case "Bgr24": format = System.Drawing.Imaging.PixelFormat.Format24bppRgb; break;
                        case "Bgra32": format = System.Drawing.Imaging.PixelFormat.Format32bppPArgb; break;
                        case "Bgr32": format = System.Drawing.Imaging.PixelFormat.Format32bppRgb; break;
                        case "Pbgra32": format = System.Drawing.Imaging.PixelFormat.Format32bppArgb; break;
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
            /// 裁切 Bitmap
            /// </summary>
            /// <param name="src"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="width"></param>
            /// <param name="height"></param>
            /// <returns></returns>
            public Bitmap CutPic(Bitmap src, int x, int y, int width, int height)
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

        }

        /// <summary>
        /// 颜色相关处理
        /// </summary>
        public class ColorChange
        {
            /// <summary>
            /// RGB 转 HSL from https://blog.csdn.net/immotal_xiaoqiang/article/details/107498059
            /// </summary>
            /// <param name="AColor"></param>
            /// <param name="H"></param>
            /// <param name="S"></param>
            /// <param name="L"></param>
            public double[] RGBToHSL(System.Windows.Media.Color AColor)
            {
                double H = 0, L = 0, S = 0;
                double r = AColor.R;
                double g = AColor.G;
                double b = AColor.B;
                if (r < 0) r = 0;
                if (g < 0) g = 0;
                if (b < 0) b = 0;
                if (r > 255) r = 255;
                if (g > 255) g = 255;
                if (b > 255) b = 255;
                r = r / 255;
                g = g / 255;
                b = b / 255;
                double M = Math.Max(Math.Max(r, g), b);
                double m = Math.Min(Math.Min(r, g), b);
                double d = M - m;
                if (d == 0) H = 0;
                else if (M == r) H = ((g - b) / d) % 6;
                else if (M == g) H = (b - r) / d + 2;
                else H = (r - g) / d + 4;
                H *= 60;
                if (H < 0) H += 360;
                L = (M + m) / 2;
                if (d == 0)
                    S = 0;
                else
                    S = d / (1 - Math.Abs(2 * L - 1));  //如果放大或者缩小  乘以或者除以相应的倍数
                S = S * 255;
                L = L * 255;

                return new double[] { H, S, L };
            }

            /// <summary>
            /// HSL 转 RGB from https://blog.csdn.net/immotal_xiaoqiang/article/details/107498059
            /// </summary>
            /// <param name="Hue"></param>
            /// <param name="Saturation"></param>
            /// <param name="Lightness"></param>
            /// <returns></returns>
            public double[] HslToRgb(double Hue, double Saturation, double Lightness)
            {
                if (Hue < 0) Hue = 0.0;
                if (Saturation < 0) Saturation = 0.0;
                if (Lightness < 0) Lightness = 0.0;
                if (Hue >= 360) Hue = 359.0;
                if (Saturation > 255) Saturation = 255;
                if (Lightness > 255) Lightness = 255;
                Saturation = Saturation / 255.0;
                Lightness = Lightness / 255.0;
                double C = (1 - Math.Abs(2 * Lightness - 1)) * Saturation;
                double hh = Hue / 60.0;
                double X = C * (1 - Math.Abs(hh % 2 - 1));
                double r = 0, g = 0, b = 0;
                if (hh >= 0 && hh < 1)
                {
                    r = C;
                    g = X;
                }
                else if (hh >= 1 && hh < 2)
                {
                    r = X;
                    g = C;
                }
                else if (hh >= 2 && hh < 3)
                {
                    g = C;
                    b = X;
                }
                else if (hh >= 3 && hh < 4)
                {
                    g = X;
                    b = C;
                }
                else if (hh >= 4 && hh < 5)
                {
                    r = X;
                    b = C;
                }
                else
                {
                    r = C;
                    b = X;
                }
                double m = Lightness - C / 2;
                r += m;
                g += m;
                b += m;
                r = r * 255.0;
                g = g * 255.0;
                b = b * 255.0;
                r = Math.Round(r);
                g = Math.Round(g);
                b = Math.Round(b);

                return new double[] { r, g, b };
            }
        }

        /// <summary>
        /// 根据名字获取子控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
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

    }
}
