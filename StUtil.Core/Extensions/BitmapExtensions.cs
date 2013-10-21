using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for the System.Drawing.Bitmap class
    /// </summary>
    /// <remarks>
    /// 2013-10-18  - Initial version
    /// </remarks>
    public static partial class BitmapExtensions
    {
        /// <summary>
        /// Get a graphics object from the bitmap
        /// </summary>
        /// <param name="img">The bitmap to get the graphics from</param>
        /// <returns></returns>
        public static Graphics GetGraphics(this Bitmap img)
        {
            return Graphics.FromImage(img);
        }
        /// <summary>
        /// Fill a bitmap with a transparent color
        /// </summary>
        /// <param name="img">The bitmap to fill</param>
        public static void Clear(this Bitmap img)
        {
            Clear(img, Color.Transparent);
        }
        /// <summary>
        /// Fill a bitmap with a specific color
        /// </summary>
        /// <param name="img">The bitmap to fill</param>
        /// <param name="c">The color to fill with</param>
        public static void Clear(this Bitmap img, Color c)
        {
            using (Graphics g = img.GetGraphics())
            {
                g.Clear(c);
            }
        }
        /// <summary>
        /// Draw an image onto a bitmap
        /// </summary>
        /// <param name="img">The image to draw onto</param>
        /// <param name="draw">The image to draw</param>
        public static void DrawImage(this Bitmap img, Bitmap draw)
        {
            DrawImage(img, draw, 0, 0, draw.Width, draw.Height);
        }
        /// <summary>
        /// Draw an image onto a bitmap in a specific position
        /// </summary>
        /// <param name="img">The image to draw onto</param>
        /// <param name="draw">The image to draw</param>
        /// <param name="x">The x coordinate to draw the image at</param>
        /// <param name="y">The y coordinate to draw the image at</param>
        public static void DrawImage(this Bitmap img, Bitmap draw, int x, int y)
        {
            DrawImage(img, draw, x, y, draw.Width, draw.Height);
        }
        /// <summary>
        /// Draw an image onto a bitmap in a specific position and size
        /// </summary>
        /// <param name="img">The image to draw onto</param>
        /// <param name="draw">The image to draw</param>
        /// <param name="x">The x coordinate to draw the image at</param>
        /// <param name="y">The y coordinate to draw the image at</param>
        /// <param name="width">The width of the image to draw</param>
        /// <param name="height">The height of the image to draw</param>
        public static void DrawImage(this Bitmap img, Bitmap draw, int x, int y, int width, int height)
        {
            using (Graphics g = img.GetGraphics())
            {
                g.DrawImage(draw, x, y, width, height);
            }
        }
    }
}
