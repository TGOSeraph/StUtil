using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    /// <summary>
    /// TODO: Finish documentation
    /// </summary>
    /// <remarks>
    /// Capsule and Rounded Rectangles from: https://code.google.com/p/moo-plus/
    /// 
    /// 2013-06-23  - Initial version
    /// </remarks>
    public static class GraphicsExtensions
    {
        /// <summary>
        /// Creates a graphics path for a rounded rectangle
        /// </summary>
        /// <param name="graphics">The graphics object to use to create the path</param>
        /// <param name="rectangle">The rectangle to create the path at</param>
        /// <param name="radius">The radius of the rounded corners</param>
        /// <returns>A graphics path representing the rounded rectangle</returns>
        public static GraphicsPath GenerateRoundedRectangle(this Graphics graphics, RectangleF rectangle, float radius)
        {
            float diameter;
            GraphicsPath path = new GraphicsPath();
            if (radius <= 0.0F)
            {
                path.AddRectangle(rectangle);
                path.CloseFigure();
                return path;
            }
            else
            {
                if (radius >= (Math.Min(rectangle.Width, rectangle.Height)) / 2.0)
                    return GenerateCapsule(graphics, rectangle);
                diameter = radius * 2.0F;
                SizeF sizeF = new SizeF(diameter, diameter);
                RectangleF arc = new RectangleF(rectangle.Location, sizeF);
                path.AddArc(arc, 180, 90);
                arc.X = rectangle.Right - diameter;
                path.AddArc(arc, 270, 90);
                arc.Y = rectangle.Bottom - diameter;
                path.AddArc(arc, 0, 90);
                arc.X = rectangle.Left;
                path.AddArc(arc, 90, 90);
                path.CloseFigure();
            }
            return path;
        }
        /// <summary>
        /// Creates a graphics path for a capsule
        /// </summary>
        /// <param name="graphics">The graphics object to use to create the path</param>
        /// <param name="baseRect">The rectangle to create the path at</param>
        /// <returns>A graphics path representing the capsule</returns>
        private static GraphicsPath GenerateCapsule(this Graphics graphics, RectangleF baseRect)
        {
            float diameter;
            RectangleF arc;
            GraphicsPath path = new GraphicsPath();
            try
            {
                if (baseRect.Width > baseRect.Height)
                {
                    diameter = baseRect.Height;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 90, 180);
                    arc.X = baseRect.Right - diameter;
                    path.AddArc(arc, 270, 180);
                }
                else if (baseRect.Width < baseRect.Height)
                {
                    diameter = baseRect.Width;
                    SizeF sizeF = new SizeF(diameter, diameter);
                    arc = new RectangleF(baseRect.Location, sizeF);
                    path.AddArc(arc, 180, 180);
                    arc.Y = baseRect.Bottom - diameter;
                    path.AddArc(arc, 0, 180);
                }
                else path.AddEllipse(baseRect);
            }
            catch { path.AddEllipse(baseRect); }
            finally { path.CloseFigure(); }
            return path;
        }

        /// <summary>
        /// Draws a rounded rectangle specified by a pair of coordinates, a width, a height and the radius 
        /// for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="graphics">The graphics object used to draw</param>
        /// <param name="pen">System.Drawing.Pen that determines the color, width and style of the rectangle.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">Width of the rectangle to draw.</param>
        /// <param name="height">Height of the rectangle to draw.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, float x, float y, float width, float height, float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawPath(pen, path);
            graphics.SmoothingMode = old;
        }

        /// <summary>
        /// Draws a rounded rectangle specified by a pair of coordinates, a width, a height and the radius 
        /// for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="pen">System.Drawing.Pen that determines the color, width and style of the rectangle.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to draw.</param>
        /// <param name="width">Width of the rectangle to draw.</param>
        /// <param name="height">Height of the rectangle to draw.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, int x, int y, int width, int height, int radius)
        {
            graphics.DrawRoundedRectangle(
                pen,
                Convert.ToSingle(x),
                Convert.ToSingle(y),
                Convert.ToSingle(width),
                Convert.ToSingle(height),
                Convert.ToSingle(radius));
        }

        /// <summary>
        /// Draws a rounded rectangle at the specified rectangle with the supplied radius
        /// </summary>
        /// <param name="graphics">The graphics object used to draw</param>
        /// <param name="pen">The color to draw the rounded rectangle</param>
        /// <param name="rect">The location and size of the rounded rectangle</param>
        /// <param name="radius">The radius of the arcs used for the rounded corners</param>
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle rect, int radius)
        {
            graphics.DrawRoundedRectangle(
                pen,
                Convert.ToSingle(rect.X),
                Convert.ToSingle(rect.Y),
                Convert.ToSingle(rect.Width),
                Convert.ToSingle(rect.Height),
                Convert.ToSingle(radius));
        }

        /// <summary>
        /// Fills the interior of a rounded rectangle specified by a pair of coordinates, a width, a height
        /// and the radius for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="graphics">The graphics object used to draw</param>
        /// <param name="brush">System.Drawing.Brush that determines the characteristics of the fill.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="width">Width of the rectangle to fill.</param>
        /// <param name="height">Height of the rectangle to fill.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, float x, float y, float width, float height, float radius)
        {
            RectangleF rectangle = new RectangleF(x, y, width, height);
            GraphicsPath path = graphics.GenerateRoundedRectangle(rectangle, radius);
            SmoothingMode old = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.FillPath(brush, path);
            graphics.SmoothingMode = old;
        }

        /// <summary>
        /// Fills the interior of a rounded rectangle
        /// </summary>
        /// <param name="graphics">The graphics object used to draw</param>
        /// <param name="brush">System.Drawing.Brush that determines the characteristics of the fill.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle rect, int radius)
        {
            graphics.FillRoundedRectangle(
                brush,
                Convert.ToSingle(rect.X),
                Convert.ToSingle(rect.Y),
                Convert.ToSingle(rect.Width),
                Convert.ToSingle(rect.Height),
                Convert.ToSingle(radius));
        }

        /// <summary>
        /// Fills the interior of a rounded rectangle specified by a pair of coordinates, a width, a height
        /// and the radius for the arcs that make the rounded edges.
        /// </summary>
        /// <param name="graphics">The graphics object used to draw</param>
        /// <param name="brush">System.Drawing.Brush that determines the characteristics of the fill.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the rectangle to fill.</param>
        /// <param name="width">Width of the rectangle to fill.</param>
        /// <param name="height">Height of the rectangle to fill.</param>
        /// <param name="radius">The radius of the arc used for the rounded edges.</param>
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, int x, int y, int width, int height, int radius)
        {
            graphics.FillRoundedRectangle(
                brush,
                Convert.ToSingle(x),
                Convert.ToSingle(y),
                Convert.ToSingle(width),
                Convert.ToSingle(height),
                Convert.ToSingle(radius));
        }

        /// <summary>
        /// Fills a triangle with the specified brush with the triangle pointing in the specified direction
        /// </summary>
        /// <param name="graphics">The graphics object used to draw</param>
        /// <param name="brush">System.Drawing.Brush that determines the characteristics of the fill.</param>
        /// <param name="direction">The direction the point of the arrow should face</param>
        /// <param name="rect">The rectangle containing the triangle</param>
        /// <param name="addLineBase">If a base line should be added to the triangle</param>
        public static void FillTriangle(this Graphics g, Brush brush, ArrowDirection direction, Rectangle rect, bool addLineBase = false)
        {
            FillTriangle(g, brush, direction, rect.X, rect.Y, rect.Width, rect.Height, addLineBase);
        }

        /// <summary>
        /// Fills a triangle with the specified brush with the triangle pointing in the specified direction
        /// </summary>
        /// <param name="graphics">The graphics object used to draw</param>
        /// <param name="brush">System.Drawing.Brush that determines the characteristics of the fill.</param>
        /// <param name="direction">The direction the point of the arrow should face</param>
        /// <param name="x">The X coordinate of the rectangle containin the arrow</param>
        /// <param name="y">The Y coordinate of the rectangle containin the arrow</param>
        /// <param name="w">The width of the rectangle containin the arrow</param>
        /// <param name="h">The height of the rectangle containin the arrow</param>
        /// <param name="addLineBase"></param>
        public static void FillTriangle(this Graphics g, Brush brush, ArrowDirection direction, int x, int y, int w, int h, bool addLineBase = false)
        {
            PointF[] path = null;
            switch (direction)
            {
                case ArrowDirection.Up:
                    if (addLineBase)
                    {
                        path = new PointF[] { new PointF(x, y + h - 1), new PointF(x, y + h), new PointF(x + w, y + h), new PointF(x + w, y + h - 1), new PointF(x + (w / 2), y) };
                    }
                    else
                    {
                        path = new PointF[] { new PointF(x, y + h), new PointF(x + w, y + h), new PointF(x + (w / 2), y) };
                    }
                    break;
                case ArrowDirection.Down:
                    path = new PointF[] { new PointF(x, y), new PointF(x + w, y), new PointF(x + (w / 2), y + h) };
                    break;
                case ArrowDirection.Left:
                    if (addLineBase)
                    {
                        path = new PointF[] { new PointF(x, y + (h / 2)), new PointF(x + w, y), new PointF(x + w, y + h) };
                    }
                    else
                    {
                        path = new PointF[] { new PointF(x, y + (h / 2)), new PointF(x + w - 1, y), new PointF(x + w, y), new PointF(x + w, y + h), new PointF(x + w - 1, y + h) };
                    }
                    break;
                case ArrowDirection.Right:
                    path = new PointF[] { new PointF(x, y), new PointF(x, y + h), new PointF(x + w, y + (h / 2)) };
                    break;
            }
            g.FillPolygon(brush, path);
        }

    }
}
