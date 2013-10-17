using System;
using System.Drawing;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Points
    /// </summary>
    /// <remarks>
    /// 2013-06-26  - Initial version
    /// </remarks>
    public static class PointExtensions
    {
        /// <summary>
        /// Calculates the Euclidean distance between two points
        /// </summary>
        /// <param name="pt1">The starting point</param>
        /// <param name="pt2">The ending point</param>
        /// <returns>The Euclidean distance between the two points</returns>
        public static double Distance(this Point pt1, Point pt2)
        {
            return (Math.Sqrt(Math.Pow(Math.Abs(pt1.X - pt2.X), 2) + Math.Pow(Math.Abs(pt1.Y - pt2.Y), 2)));
        }

        /// <summary>
        /// Calculates the heading as an angle between two points
        /// </summary>
        /// <param name="pt1">The first point</param>
        /// <param name="pt2">The second point</param>
        /// <returns>The angle, in degrees between the two points</returns>
        public static double Heading(this Point pt1, Point pt2)
        {
            return Math.Atan2(pt1.Y - pt2.Y, pt1.X - pt2.X) * 180.0 / Math.PI;
        }

        /// <summary>
        /// Convert a pointf to a point
        /// </summary>
        /// <param name="pt">The pointf to convert</param>
        /// <returns>The pointf floored</returns>
        public static Point ToPoint(this PointF pt)
        {
            return new Point((int)pt.X, (int)pt.Y);
        }
    }
}
