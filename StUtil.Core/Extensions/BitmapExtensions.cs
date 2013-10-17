using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StUtil.Extensions
{
    public static partial class BitmapExtensions
    {
        public static Graphics GetGraphics(this Bitmap img)
        {
            return Graphics.FromImage(img);
        }
        public static void Clear(this Bitmap img)
        {
            Clear(img, Color.Transparent);
        }
        public static void Clear(this Bitmap img, Color c)
        {
            using (Graphics g = img.GetGraphics())
            {
                g.Clear(c);
            }
        }
        public static void DrawImage(this Bitmap img, Bitmap draw)
        {
            DrawImage(img, draw, 0, 0, draw.Width, draw.Height);
        }
        public static void DrawImage(this Bitmap img, Bitmap draw, int x, int y)
        {
            DrawImage(img, draw, x, y, draw.Width, draw.Height);
        }
        public static void DrawImage(this Bitmap img, Bitmap draw, int x, int y, int width, int height)
        {
            using (Graphics g = img.GetGraphics())
            {
                g.DrawImage(draw, x, y, width, height);
            }
        }
    }
}
