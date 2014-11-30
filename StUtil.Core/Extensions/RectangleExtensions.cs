using System.Drawing;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Rectangles
    /// </summary>
    public static class RectangleExtensions
    {
        /// <summary>
        /// Calculates the area of the rectangle
        /// </summary>
        /// <param name="rect">The rectangle to calculate the area of</param>
        /// <returns>The width of the rectangle, multiplied by the height</returns>
        public static int Area(this Rectangle rect)
        {
            return rect.Width * rect.Height;
        }

        /// <summary>
        /// Returns the midpoint of the rectangle
        /// </summary>
        /// <param name="rect">The rectangle to get the midpoint from</param>
        /// <returns>The midpoint of the rectangle</returns>
        public static Point Middle(this Rectangle rect)
        {
            return new Point(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
        }

        /// <summary>
        /// Returns the midpoint of the rectangle
        /// </summary>
        /// <param name="rect">The rectangle to get the midpoint from</param>
        /// <returns>The midpoint of the rectangle</returns>
        public static PointF Middle(this RectangleF rect)
        {
            return new PointF(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
        }

        /// <summary>
        /// Sets the bottom value.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static Rectangle SetBottom(this Rectangle rect, int val)
        {
            rect.Height = val - rect.Y;
            return rect;
        }

        /// <summary>
        /// Sets the right value.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="val">The value.</param>
        /// <returns></returns>
        public static Rectangle SetRight(this Rectangle rect, int val)
        {
            rect.Width = val - rect.X;
            return rect;
        }

        /// <summary>
        /// Converts a RectangleF to a Rectangle.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <returns></returns>
        public static Rectangle ToRectangle(this RectangleF rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }
    }
}