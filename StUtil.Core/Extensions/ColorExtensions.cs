using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for the System.Drawing.Color class
    /// </summary>
    /// <remarks>
    /// 2013-06-23  - Initial version
    /// </remarks>
    public static class ColorExtensions
    {
        /// <summary>
        /// Invert the specified color
        /// </summary>
        /// <param name="color">The color to invert</param>
        /// <returns>The inverted color</returns>
        public static Color Invert(this Color color)
        {
            return Invert(color, false);
        }

        /// <summary>
        /// Invert the specified color
        /// </summary>
        /// <param name="color">The color to invert</param>
        /// <param name="alpha">If the alpha channel should also be inverted</param>
        /// <returns>The inverted color</returns>
        public static Color Invert(this Color color, bool alpha)
        {
            if (alpha)
            {
                return Color.FromArgb(255 - color.A, 255 - color.R, 255 - color.G, 255 - color.B);
            }
            return Color.FromArgb(color.A, 255 - color.R, 255 - color.G, 255 - color.B);
        }

        /// <summary>
        /// Approximately calculates the percieved brightness of a color
        /// </summary>
        /// <param name="color">The color to calculate the percieved brightness of</param>
        /// <returns>The percieved brightness of the color</returns>
        public static double PercievedBrightnessFast(this Color color)
        {
            return (0.299 * color.R + 0.587 * color.G + 0.114 * color.B);
        }
        /// <summary>
        /// Calculates the percieved brightness of a color
        /// </summary>
        /// <param name="color">The color to calculate the percieved brightness of</param>
        /// <returns>The percieved brightness of the color</returns>
        public static double PercievedBrightness(this Color color)
        {
            return Math.Sqrt(Math.Pow(0.241 * color.R, 2) + Math.Pow(0.691 * color.G, 2) + Math.Pow(0.068 * color.B, 2));
        }
        /// <summary>
        /// Calculates the brightness of a color
        /// </summary>
        /// <param name="color">The color to calculate the brightness of</param>
        /// <returns>The brightness of the color</returns>
        public static double StandardBrightness(this Color color)
        {
            return (0.2126 * color.R) + (0.7152 * color.G) + (0.0722 * color.B);
        }

        /// <summary>
        /// Calculates the average color from an array of colors
        /// </summary>
        /// <param name="colors">The array of colors to average</param>
        /// <returns>The average color of the array</returns>
        public static Color Average(this IEnumerable<Color> colors)
        {
            int l = colors.Count();
            int r = 0, g = 0, b = 0;
            foreach (Color c in colors)
            {
                r += c.R;
                g += c.G;
                b += c.B;
            }
            r /= l;
            g /= l;
            b /= l;
            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Subtract a value from each channel of a color
        /// </summary>
        /// <param name="c">The color to subtract from</param>
        /// <param name="val">The value to subtract</param>
        /// <returns>The color with the specified value subtracted</returns>
        public static Color Subtract(this Color c, byte val)
        {
            return Subtract(c, 0, val, val, val);
        }

        /// <summary>
        /// Subtract a value from each channel of a color
        /// </summary>
        /// <param name="c">The color to subtract from</param>
        /// <param name="r">The value to subtract from the R channel</param>
        /// <param name="g">The value to subtract from the G channel</param>
        /// <param name="b">The value to subtract from the B channel</param>
        /// <returns>The color with the specified values subtracted</returns>
        public static Color Subtract(this Color c, byte r, byte g, byte b)
        {
            return Subtract(c, 0, r, g, b);
        }

        /// <summary>
        /// Subtract a value from each channel of a color
        /// </summary>
        /// <param name="c">The color to subtract from</param>
        /// <param name="a">The value to subtract from the alpha channel</param>
        /// <param name="r">The value to subtract from the R channel</param>
        /// <param name="g">The value to subtract from the G channel</param>
        /// <param name="b">The value to subtract from the B channel</param>
        /// <returns>The color with the specified values subtracted</returns>
        /// <returns>The color with the specified values subtracted</returns>
        public static Color Subtract(this Color c, byte a, byte r, byte g, byte b)
        {
            return Color.FromArgb(c.A - a, c.R - r, c.G - g, c.B - b);
        }

        /// <summary>
        /// Subtract a value from each channel of a color
        /// </summary>
        /// <param name="c">The color to subtract from</param>
        /// <param name="val">The value to subtract</param>
        /// <returns>The color with the specified value subtracted</returns>
        public static Color Subtract(this Color c, sbyte val)
        {
            return Subtract(c, 0, val, val, val);
        }

        /// <summary>
        /// Subtract a value from each channel of a color
        /// </summary>
        /// <param name="c">The color to subtract from</param>
        /// <param name="r">The value to subtract from the R channel</param>
        /// <param name="g">The value to subtract from the G channel</param>
        /// <param name="b">The value to subtract from the B channel</param>
        /// <returns>The color with the specified values subtracted</returns>
        public static Color Subtract(this Color c, sbyte r, sbyte g, sbyte b)
        {
            return Subtract(c, 0, r, g, b);
        }

        /// <summary>
        /// Subtract a value from each channel of a color
        /// </summary>
        /// <param name="c">The color to subtract from</param>
        /// <param name="a">The value to subtract from the alpha channel</param>
        /// <param name="r">The value to subtract from the R channel</param>
        /// <param name="g">The value to subtract from the G channel</param>
        /// <param name="b">The value to subtract from the B channel</param>
        /// <returns>The color with the specified values subtracted</returns>
        /// <returns>The color with the specified values subtracted</returns>
        public static Color Subtract(this Color c, sbyte a, sbyte r, sbyte g, sbyte b)
        {
            return Color.FromArgb(c.A - a, c.R - r, c.G - g, c.B - b);
        }

        /// <summary>
        /// Add a value to each channel of a color
        /// </summary>
        /// <param name="c">The color to add to</param>
        /// <param name="val">The value to add</param>
        /// <returns>The color with the specified value added</returns>
        public static Color Add(this Color c, byte val)
        {
            return Add(c, 0, val, val, val);
        }

        /// <summary>
        /// Add a value to each channel of a color
        /// </summary>
        /// <param name="c">The color to add to</param>
        /// <param name="r">The value to add to the R channel</param>
        /// <param name="g">The value to add to the G channel</param>
        /// <param name="b">The value to add to the B channel</param>
        /// <returns>The color with the specified values added</returns>
        public static Color Add(this Color c, byte r, byte g, byte b)
        {
            return Add(c, 0, r, g, b);
        }

        /// <summary>
        /// Add a value to each channel of a color
        /// </summary>
        /// <param name="c">The color to add to</param>
        /// <param name="a">The value to add to the alpha channel</param>
        /// <param name="r">The value to add to the R channel</param>
        /// <param name="g">The value to add to the G channel</param>
        /// <param name="b">The value to add to the B channel</param>
        /// <returns>The color with the specified values added</returns>
        public static Color Add(this Color c, byte a, byte r, byte g, byte b)
        {
            return Color.FromArgb(c.A + a, c.R + r, c.G + g, c.B + b);
        }

        /// <summary>
        /// Add a value to each channel of a color
        /// </summary>
        /// <param name="c">The color to add to</param>
        /// <param name="val">The value to add</param>
        /// <returns>The color with the specified value added</returns>
        public static Color Add(this Color c, sbyte val)
        {
            return Add(c, 0, val, val, val);
        }

        /// <summary>
        /// Add a value to each channel of a color
        /// </summary>
        /// <param name="c">The color to add to</param>
        /// <param name="r">The value to add to the R channel</param>
        /// <param name="g">The value to add to the G channel</param>
        /// <param name="b">The value to add to the B channel</param>
        /// <returns>The color with the specified values added</returns>
        public static Color Add(this Color c, sbyte r, sbyte g, sbyte b)
        {
            return Add(c, 0, r, g, b);
        }

        /// <summary>
        /// Add a value to each channel of a color
        /// </summary>
        /// <param name="c">The color to add to</param>
        /// <param name="a">The value to add to the alpha channel</param>
        /// <param name="r">The value to add to the R channel</param>
        /// <param name="g">The value to add to the G channel</param>
        /// <param name="b">The value to add to the B channel</param>
        /// <returns>The color with the specified values added</returns>
        public static Color Add(this Color c, sbyte a, sbyte r, sbyte g, sbyte b)
        {
            return Color.FromArgb(c.A + a, c.R + r, c.G + g, c.B + b);
        }
    }
}
