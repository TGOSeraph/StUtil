using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Images
    /// </summary>
    /// <remarks>
    /// 2013-06-23  - Initial version
    /// </remarks>
    public static class ImageExtensions
    {
        /// <summary>
        /// Crop an image
        /// </summary>
        /// <param name="b">The image to crop</param>
        /// <param name="x">The x location to start cropping from</param>
        /// <param name="y">The y location to start cropping from</param>
        /// <param name="w">The width of the cropped image</param>
        /// <param name="h">The height of the cropped image</param>
        /// <returns>The cropped image</returns>
        public static System.Drawing.Image Crop(this System.Drawing.Image b, int x, int y, int w, int h)
        {
            Bitmap bm = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(bm))
            {
                g.DrawImageUnscaled(b, -x, -y);
            }
            return bm;
        }

        /// <summary>
        /// Resize an image
        /// </summary>
        /// <param name="b">The image to resize</param>
        /// <param name="w">The width of the image to resize to</param>
        /// <param name="h">The height of the image to resize to</param>
        /// <returns>The resized image</returns>
        public static System.Drawing.Image ResizeImage(this System.Drawing.Image b, int w, int h)
        {
            Bitmap ret = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(ret))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(b, 0, 0, w, h);
            }
            return ret;
        }

        /// <summary>
        /// Resize an image whilst maintaining the aspect ratio (no stretching)
        /// </summary>
        /// <param name="b">The image to resize</param>
        /// <param name="maxW">The maximum allowed width</param>
        /// <param name="maxH">The maximum allowed height</param>
        /// <returns>The resized image</returns>
        public static System.Drawing.Image ResizeImageMaintainAspect(this System.Drawing.Image b, int maxW, int maxH)
        {
            Size sz = GetResizedMaintainedAspectRatio(b, maxW, maxH);
            return ResizeImage(b, sz.Width, sz.Height);
        }

        /// <summary>
        /// Get the scaled size while maintaining the aspect ratio
        /// </summary>
        /// <param name="b">The image to get the scaled dimensions of</param>
        /// <param name="maxW">The maximum width of the output size</param>
        /// <param name="maxH">The maximum height of the output size</param>
        /// <returns>The min(maxW,maxH) while scaled maintaining aspect ratio</returns>
        public static Size GetResizedMaintainedAspectRatio(this System.Drawing.Image b, int maxW, int maxH)
        {
            int w = b.Width;
            int h = b.Height;

            if (w <= maxW && h <= maxH) return b.Size;

            double sW = (double)w / maxW;
            double sH = (double)h / maxH;

            sW = (sW > sH) ? sW : sH;

            return new Size((int)(w / sW), (int)(h / sW));
        }

        /// <summary>
        /// Converts an image to greyscale using the ToolStipRenderer CreateDisabledImage method
        /// </summary>
        /// <param name="img">The image to create a greyscale equivalent of</param>
        /// <returns>The greyscale conversion of the original image</returns>
        public static System.Drawing.Image CreateDisabled(this System.Drawing.Image img)
        {
            return System.Windows.Forms.ToolStripRenderer.CreateDisabledImage(img);
        }

        public static Bitmap CreateGreyscale(this Image original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] 
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
    }
}
