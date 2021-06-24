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
