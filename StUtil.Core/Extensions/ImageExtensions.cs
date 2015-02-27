using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace StUtil.Extensions
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Duplicates the specified image.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <returns></returns>
        public static T Duplicate<T>(this T img) where T : Image
        {
            ImageConverter converter = new ImageConverter();
            return (T)converter.ConvertFrom((byte[])converter.ConvertTo(img, typeof(byte[])));
        }

        /// <summary>
        /// Fill an image with a specific color
        /// </summary>
        /// <param name="img">The image to fill</param>
        /// <param name="c">The color to fill with</param>
        public static void Clear(this Image img, Color c)
        {
            using (Graphics g = img.GetGraphics())
            {
                g.Clear(c);
            }
        }

        /// <summary>
        /// Fill an image with a transparent color
        /// </summary>
        /// <param name="img">The image to fill</param>
        public static void Clear(this Image img)
        {
            Clear(img, Color.Transparent);
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

        /// <summary>
        /// Create a greyscale image
        /// </summary>
        /// <param name="original">The image to convert  to greyscale</param>
        /// <returns>A greyscale version of the input image</returns>
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
        /// Draw an image onto an image
        /// </summary>
        /// <param name="img">The image to draw onto</param>
        /// <param name="draw">The image to draw</param>
        public static void DrawImage(this Image img, Image draw)
        {
            DrawImage(img, draw, 0, 0, draw.Width, draw.Height);
        }

        /// <summary>
        /// Draw an image onto an image in a specific position
        /// </summary>
        /// <param name="img">The image to draw onto</param>
        /// <param name="draw">The image to draw</param>
        /// <param name="x">The x coordinate to draw the image at</param>
        /// <param name="y">The y coordinate to draw the image at</param>
        public static void DrawImage(this Image img, Image draw, int x, int y)
        {
            DrawImage(img, draw, x, y, draw.Width, draw.Height);
        }

        /// <summary>
        /// Draw an image onto an image in a specific position and size
        /// </summary>
        /// <param name="img">The image to draw onto</param>
        /// <param name="draw">The image to draw</param>
        /// <param name="x">The x coordinate to draw the image at</param>
        /// <param name="y">The y coordinate to draw the image at</param>
        /// <param name="width">The width of the image to draw</param>
        /// <param name="height">The height of the image to draw</param>
        public static void DrawImage(this Image img, Image draw, int x, int y, int width, int height)
        {
            using (Graphics g = img.GetGraphics())
            {
                g.DrawImage(draw, x, y, width, height);
            }
        }

        /// <summary>
        /// Get a graphics object from the image
        /// </summary>
        /// <param name="img">The image to get the graphics from</param>
        /// <returns></returns>
        public static Graphics GetGraphics(this Image img)
        {
            return Graphics.FromImage(img);
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
        /// Creats a new Bitmap with the brightness adjusted
        /// </summary>
        /// <param name="original">The image to adjust the brightness of</param>
        /// <param name="brightness">A value between -255 and 255 stating the increase in brightness of each pixel</param>
        /// <returns></returns>
        public static Bitmap AdjustBrightness(this Bitmap original, int brightness)
        {

            float finalValue = (float)brightness / 255.0f;

            ColorMatrix colorMatrix = new ColorMatrix(new float[][]{
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {finalValue, finalValue, finalValue, 1, 1}
            });


            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

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

        public static Bitmap Tint(this Image original, Color color)
        {
            return Tint(original, color.R, color.G, color.B);
        }

        public static Bitmap Tint(this Image original, float red, float green, float blue)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
		        new float[] {1,0,0,0,0},
		        new float[] {0,1,0,0,0},
		        new float[] {0,0,1,0,0},
		        new float[] {0,0,0,1,0},
		        new float[] {red / 255,green / 255,blue / 255,0,0}
	        });
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);
                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            }
            return newBitmap;
        }
    }



}