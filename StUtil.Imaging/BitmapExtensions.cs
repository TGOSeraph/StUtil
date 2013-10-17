using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

using StUtil.Extensions;

namespace StUtil.Imaging
{
    public static class BitmapExtensions
    {
        public static BitmapData Lock(this Bitmap image)
        {
            return image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
        }

        public static BitmapAccessor GetAccessor(this Bitmap image, bool doLock = true)
        {
            BitmapAccessor accessor = new BitmapAccessor(image);
            if (doLock)
            {
                accessor.Lock();
            }
            return accessor;
        }

        public static int GetPixelSize(this Bitmap image)
        {
            if (image.PixelFormat == PixelFormat.Format24bppRgb)
                return 3;
            else if (image.PixelFormat == PixelFormat.Format32bppArgb
                || image.PixelFormat == PixelFormat.Format32bppPArgb
                || image.PixelFormat == PixelFormat.Format32bppRgb)
                return 4;
            return 0;
        }

        public static Bitmap ChangeContrast(this System.Drawing.Image Image, float Value)
        {
            Value = (100.0f + Value) / 100.0f;
            Value *= Value;
            Bitmap NewBitmap = (Bitmap)Image.Clone();
            BitmapData data = NewBitmap.LockBits(
                new Rectangle(0, 0, NewBitmap.Width, NewBitmap.Height),
                ImageLockMode.ReadWrite,
                NewBitmap.PixelFormat);
            int Height = NewBitmap.Height;
            int Width = NewBitmap.Width;

            unsafe
            {
                for (int y = 0; y < Height; ++y)
                {
                    byte* row = (byte*)data.Scan0 + (y * data.Stride);
                    int columnOffset = 0;
                    for (int x = 0; x < Width; ++x)
                    {
                        byte B = row[columnOffset];
                        byte G = row[columnOffset + 1];
                        byte R = row[columnOffset + 2];

                        float Red = R / 255.0f;
                        float Green = G / 255.0f;
                        float Blue = B / 255.0f;
                        Red = (((Red - 0.5f) * Value) + 0.5f) * 255.0f;
                        Green = (((Green - 0.5f) * Value) + 0.5f) * 255.0f;
                        Blue = (((Blue - 0.5f) * Value) + 0.5f) * 255.0f;

                        int iR = (int)Red;
                        iR = iR > 255 ? 255 : iR;
                        iR = iR < 0 ? 0 : iR;
                        int iG = (int)Green;
                        iG = iG > 255 ? 255 : iG;
                        iG = iG < 0 ? 0 : iG;
                        int iB = (int)Blue;
                        iB = iB > 255 ? 255 : iB;
                        iB = iB < 0 ? 0 : iB;

                        row[columnOffset] = (byte)iB;
                        row[columnOffset + 1] = (byte)iG;
                        row[columnOffset + 2] = (byte)iR;

                        columnOffset += 4;
                    }
                }
            }

            NewBitmap.UnlockBits(data);

            return NewBitmap;
        }

        public static unsafe Bitmap Invert(this System.Drawing.Image img)
        {
            Bitmap bmp = new Bitmap(img);
            BitmapAccessor acc = new BitmapAccessor(bmp);
            int w = bmp.Width;
            int h = bmp.Height;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    acc.SetPixel(x, y, acc.GetPixel(x, y).Invert());
                }
            }
            acc.Unlock();
            return bmp;
        }
    }
}
