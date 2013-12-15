using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace StUtil.Imaging
{
    public static class Utilities
    {
        public enum ImageFileFormat
        {
            Unknown,
            Bmp,
            Emf,
            Wmf,
            Gif,
            Jpeg,
            Png,
            Tiff,
            Icon
        }

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

        public static ImageFileFormat ImageFormat(System.Drawing.Image Image)
        {
            try
            {
                return (ImageFileFormat)Enum.Parse(typeof(ImageFileFormat), System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders().ToList().Single(ImageCodecInfo => ImageCodecInfo.FormatID == Image.RawFormat.Guid).FormatDescription, true);
            }
            catch (Exception)
            {
            }
            return ImageFileFormat.Unknown;
        }

        public static ImageFileFormat ImageFormat(string file, bool useFileExtFallback = true)
        {
            byte[] buffer;
            using (Stream s = System.IO.File.OpenRead(file))
            {
                using (BinaryReader br = new BinaryReader(s))
                {
                    buffer = br.ReadBytes(10);
                }
            }

            if (buffer[0] == 0x42 && buffer[1] == 0x4d)
            {
                return ImageFileFormat.Bmp;
            }
            else if (buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x38)
            {
                return ImageFileFormat.Gif;
            }
            else if (buffer[0] == 0xff && buffer[1] == 0xd8 && buffer[2] == 0xff && buffer[3] == 0xe0)
            {
                return ImageFileFormat.Jpeg;
            }
            else if (buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4e && buffer[3] == 0x47)
            {
                return ImageFileFormat.Png;
            }
            else if ((buffer[0] == 0x4d && buffer[1] == 0x4d && buffer[2] == 0x00 && buffer[3] == 0x2a) | (buffer[0] == 0x49 && buffer[1] == 0x49 && buffer[2] == 0x2a && buffer[3] == 0x00))
            {
                return ImageFileFormat.Tiff;
            }
            else if (buffer[0] == 0x01 && buffer[1] == 0x00 && buffer[2] == 0x00 && buffer[3] == 0x00)
            {
                return ImageFileFormat.Emf;
            }
            else if (buffer[0] == 0x01 && buffer[1] == 0x00 && buffer[2] == 0x09 && buffer[3] == 0x00 && buffer[4] == 0x00 && buffer[5] == 0x03)
            {
                return ImageFileFormat.Wmf;
            }
            else if (buffer[0] == 0x00 && buffer[1] == 0x00 && buffer[2] == 0x01 && buffer[3] == 0x00)
            {
                return ImageFileFormat.Icon;
            }

            if (useFileExtFallback)
            {
                try
                {
                    return (ImageFileFormat)Enum.Parse(typeof(ImageFileFormat), System.IO.Path.GetExtension(file).Substring(1).Replace("jpg", "jpeg").Replace("ico", "icon"), true);
                }
                catch (Exception)
                {
                }
            }
            return ImageFileFormat.Unknown;
        }

        public static string ImageFormatExtension(ImageFileFormat format, bool includeDot = true)
        {
            string ext = "";
            if (format == ImageFileFormat.Jpeg)
            {
                ext = "jpg";
            }
            else if (format == ImageFileFormat.Icon)
            {
                ext = "ico";
            }
            else if (format == ImageFileFormat.Unknown)
            {
                return null;
            }
            else
            {
                ext = format.ToString().ToLower();
            }
            if (includeDot)
            {
                ext = "." + ext;
            }
            return ext;
        }
    }
}
