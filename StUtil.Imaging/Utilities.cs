using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace StUtil.Imaging
{
    public static class Utilities
    {
        public static Bitmap MergeImages(int w, int h, params IntPtr[] scan0s)
        {
            int c = scan0s.Length;
            Rectangle rect = new Rectangle(0, 0, w, h);

            Bitmap newImage = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            BitmapData newData = newImage.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* baserow = (byte*)newData.Scan0;

                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        int r = 0, g = 0, b = 0;
                        for (int i = 0; i < c; i++)
                        {
                            byte* ptr = (byte*)scan0s[i];
                            r += (int)ptr[2];
                            g += (int)ptr[1];
                            b += (int)ptr[0];
                            scan0s[i] = new IntPtr(scan0s[i].ToInt32() + 3);
                        }
                        baserow[2] = (byte)(r / c);
                        baserow[1] = (byte)(g / c);
                        baserow[0] = (byte)(b / c);

                        baserow += 3;
                    }
                }
            }

            newImage.UnlockBits(newData);

            return newImage;
        }


        public static void ThresholdSubtractImages(Bitmap b1, Bitmap b2, int differenceThreshold, int colorThreshold)
        {
            int h = b1.Height;
            int w = b1.Width;

            BitmapData data1 = b1.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData data2 = b2.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            long yOffset = 0;
            int xOffset = 0;

            unsafe
            {
                for (int y = 0; y < h; y++)
                {
                    byte* mergerow = (byte*)data1.Scan0 + yOffset;
                    byte* baserow = (byte*)data2.Scan0 + yOffset;
                    xOffset = 0;
                    for (int x = 0; x < w; x++)
                    {
                        int grayScale1 = (int)((baserow[xOffset + 2] * 0.3) + (baserow[xOffset + 1] * 0.59) + (baserow[xOffset] * 0.11));
                        int grayScale2 = (int)((mergerow[xOffset + 2] * 0.3) + (mergerow[xOffset + 1] * 0.59) + (mergerow[xOffset] * 0.11));

                        if (Math.Abs(grayScale1 - grayScale2) > differenceThreshold && grayScale1 > colorThreshold)
                        {
                            baserow[xOffset + 2] = 255;
                            baserow[xOffset + 1] = 255;
                            baserow[xOffset] = 255;
                        }
                        else
                        {
                            baserow[xOffset + 2] = 0;
                            baserow[xOffset + 1] = 0;
                            baserow[xOffset] = 0;
                        }

                        xOffset += 3;
                    }
                    yOffset += data1.Stride;
                }
            }

            b1.UnlockBits(data1);
            b2.UnlockBits(data2);
        }


        public static Image OpenFile(string fileName)
        {
            Image img;
            using (var bmpTemp = new Bitmap(fileName))
            {
                img = new Bitmap(bmpTemp);
            }
            return img;
        }

    }
}
