using System;
using System.Drawing;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for PictureBoxes
    /// </summary>
    /// <remarks>
    /// 2013-06-26  - Initial version
    /// </remarks>
    public static class PictureBoxExtensions
    {
        /// <summary>
        /// Gets a point in the displayed image from a point on the picture box taking into account Zooming
        /// </summary>
        /// <param name="pb">The picturebox to get the point from</param>
        /// <param name="pt">The point to convert to image coordinates</param>
        /// <returns>The image-relative point from the control-relative point</returns>
        public static Point GetZoomedPoint(this PictureBox pb, Point pt)
        {
            if (pb.SizeMode != PictureBoxSizeMode.Zoom)
            {
                throw new InvalidOperationException("PictureBox must be set to SizeMode = Zoom");
            }

            Point p = pb.PointToClient(pt);
            Point unscaled_p = new Point();

            // image and container dimensions
            int w_i = pb.Image.Width;
            int h_i = pb.Image.Height;
            int w_c = pb.Width;
            int h_c = pb.Height;

            float imageRatio = w_i / (float)h_i; // image W:H ratio
            float containerRatio = w_c / (float)h_c; // container W:H ratio

            if (imageRatio >= containerRatio)
            {
                // horizontal image
                float scaleFactor = w_c / (float)w_i;
                float scaledHeight = h_i * scaleFactor;
                // calculate gap between top of container and top of image
                float filler = Math.Abs(h_c - scaledHeight) / 2;
                unscaled_p.X = (int)(p.X / scaleFactor);
                unscaled_p.Y = (int)((p.Y - filler) / scaleFactor);
            }
            else
            {
                // vertical image
                float scaleFactor = h_c / (float)h_i;
                float scaledWidth = w_i * scaleFactor;
                float filler = Math.Abs(w_c - scaledWidth) / 2;
                unscaled_p.X = (int)((p.X - filler) / scaleFactor);
                unscaled_p.Y = (int)(p.Y / scaleFactor);
            }

            return unscaled_p;
        }

        /// <summary>
        /// Get the point on the image that the cursor is currently over
        /// </summary>
        /// <param name="pb">The picturebox to get the point from</param>
        /// <returns>The image-relative point from the control-relative point</returns>
        public static Point GetCursorLocation(this PictureBox pb)
        {
            return GetZoomedPoint(pb, Cursor.Position);
        }
    }
}
