using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


// Shader-Based-Image-Processing (SBIP)
// http://code.google.com/p/sbip/
//
// Copyright © Frank Nagl, 2009-2011
// admin@franknagl.de
//
// ColorSpace namespace copyright © Tobias Bindel, 2011
//
namespace StUtil.Imaging.ColorSpaces
{
    /// <summary>
    /// A struct which defines the HSL color space.
    /// </summary>
    public struct HSL : IColorSpace
    {
        /// <summary>
        /// Gets an empty <see cref="HSL"/> structure;
        /// </summary>
        public static readonly HSL Empty;

        #region Accessors

        /// <summary>
        /// Gets or sets triple of color channels.
        /// </summary>
        /// <value>The color triple.</value>
        public ColorTriple Color
        {
            get
            {
                return new ColorTriple(H, S, L);
            }
            set
            {
                H = value.A;
                S = value.B;
                L = value.C;
            }
        }

        /// <summary>
        /// Gets or sets the hue channel.
        /// </summary>
        /// <value>The hue.</value>
        public double Hue
        {
            get { return H; }
            set { H = value; }
        }

        /// <summary>
        /// Gets or sets saturation channel.
        /// </summary>
        /// <value>The saturation.</value>
        public double Saturation
        {
            get { return S; }
            set { S = value; }
        }

        /// <summary>
        /// Gets or sets the luminance channel.
        /// </summary>
        /// <value>The luminance.</value>
        public double Luminance
        {
            get { return L; }
            set { L = value; }
        }

        /// <summary>
        /// Short accessor for the hue channel.
        /// </summary>
        /// <value>The hue channel.</value>
        public double H { get; set; }

        /// <summary>
        /// Short accessor for the saturation channel.
        /// </summary>
        /// <value>The saturation channel.</value>
        public double S { get; set; }

        /// <summary>
        /// Short accessor for the luminance channel.
        /// </summary>
        /// <value>The luminance channel.</value>
        public double L { get; set; }

        #endregion

        ///// <summary>
        ///// Initializes a new instance of the <see cref="HSL"/> struct.
        ///// </summary>
        //public HSL()
        //{
        //}

        /// <summary>
        /// Creates an instance of a <see cref="HSL"/> structure.
        /// </summary>
        /// <param name="h">The hue channel.</param>
        /// <param name="s">The saturation channel.</param>
        /// <param name="l">The lightness channel.</param>
        public HSL(double h, double s, double l)
            : this()
        {
            H = h;
            S = s;
            L = l;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HSL"/> struct.
        /// </summary>
        /// <param name="hsl">The color in <see cref="HSL"/> color space.</param>
        public HSL(HSL hsl)
            : this()
        {
            H = hsl.H;
            S = hsl.S;
            L = hsl.L;
        }

        #region convert

        /// <summary>
        /// Converts from <see cref="HSL"/> to <see cref="RGB"/> color space.
        /// </summary>
        /// <param name="h">The hue channel, must be in [0, 360].</param>
        /// <param name="s">The saturation channel, must be in [0, 1].</param>
        /// <param name="l">The luminance channel, must be in [0, 1].</param>
        /// <returns>
        /// The color in <see cref="RGB"/> color space.
        /// </returns>
        public static RGB ToRGB(double h, double s, double l)
        {
            if (s == 0)
            {
                // achromatic color (gray scale)
                return new RGB
                {
                    R = (int)Math.Round(l * 255.0),
                    G = (int)Math.Round(l * 255.0),
                    B = (int)Math.Round(l * 255.0)
                };
            }
            else
            {
                var q = (l < 0.5) ? (l * (1.0 + s)) : (l + s - (l * s));
                var p = (2.0 * l) - q;

                var k = h / 360.0;
                var t = new double[3];

                t[0] = k + (1.0 / 3.0); // Tr
                t[1] = k;               // Tb
                t[2] = k - (1.0 / 3.0); // Tg

                for (int i = 0; i < 3; i++)
                {
                    if (t[i] < 0)
                    {
                        t[i] += 1.0;
                    }

                    if (t[i] > 1)
                    {
                        t[i] -= 1.0;
                    }

                    if ((t[i] * 6) < 1)
                    {
                        t[i] = p + ((q - p) * 6.0 * t[i]);
                    }
                    else if ((t[i] * 2.0) < 1)
                    {
                        // or ((1.0/6.0) <= t[i] && T[i] < 0.5)
                        t[i] = q;
                    }
                    else if ((t[i] * 3.0) < 2)
                    {
                        // or (0.5 <= T[i] && t[i] < (2.0/3.0))
                        t[i] = p + (q - p) * ((2.0 / 3.0) - t[i]) * 6.0;
                    }
                    else
                    {
                        t[i] = p;
                    }
                }

                return new RGB
                {
                    R = (int)Math.Round(t[0] * 255.0),
                    G = (int)Math.Round(t[1] * 255.0),
                    B = (int)Math.Round(t[2] * 255.0)
                };
            }
        }

        /// <summary>
        /// Converts from <see cref="HSL"/> to <see cref="RGB"/> color space.
        /// </summary>
        /// <param name="hsl">The source color in <see cref="HSL"/> color space.</param>
        /// <returns>
        /// The color in <see cref="RGB"/> color space.
        /// </returns>
        public static RGB ToRGB(HSL hsl)
        {
            return ToRGB(hsl.H, hsl.S, hsl.L);
        }

        /// <summary>
        /// Converts from <see cref="RGB"/> to <see cref="HSL"/> color space.
        /// </summary>
        /// <param name="r">The red channel, must be in [0, 255].</param>
        /// <param name="g">The green channel, must be in [0, 255].</param>
        /// <param name="b">The blue channel, must be in [0, 255].</param>
        /// <returns>
        /// The color in <see cref="HSL"/> color space.
        /// </returns>
        public static HSL FromRGB(double r, double g, double b)
        {
            var h = 0.0;
            var s = 0.0;
            var l = 0.0;

            // normalizes red-green-blue values
            var red = r / 255.0;
            var green = g / 255.0;
            var blue = b / 255.0;

            var max = Math.Max(red, Math.Max(green, blue));
            var min = Math.Min(red, Math.Min(green, blue));

            // hue
            if (max == min)
            {
                h = 0; // undefined
            }
            else if (max == red && green >= blue)
            {
                h = 60.0 * (green - blue) / (max - min);
            }
            else if (max == red && green < blue)
            {
                h = 60.0 * (green - blue) / (max - min) + 360.0;
            }
            else if (max == green)
            {
                h = 60.0 * (blue - red) / (max - min) + 120.0;
            }
            else if (max == blue)
            {
                h = 60.0 * (red - green) / (max - min) + 240.0;
            }

            // luminance
            l = (max + min) / 2.0;

            // saturation
            if (l == 0 || max == min)
            {
                s = 0;
            }
            else if (0 < l && l <= 0.5)
            {
                s = (max - min) / (max + min);
            }
            else if (l > 0.5)
            {
                s = (max - min) / (2 - (max + min));
            }

            return new HSL
            {
                H = h,
                S = s,
                L = l
            };
        }

        /// <summary>
        /// Converts from <see cref="RGB"/> to <see cref="HSL"/> color space.
        /// </summary>
        /// <param name="rgb">The source color in <see cref="RGB"/> color space.</param>
        /// <returns>
        /// The color in <see cref="HSL"/> color space.
        /// </returns>
        public static HSL FromRGB(RGB rgb)
        {
            return FromRGB(rgb.R, rgb.G, rgb.B);
        }

        /// <summary>
        /// Converts from <see cref="HSL"/> to <see cref="RGB"/> color space.
        /// </summary>
        /// <returns>
        /// The color in <see cref="RGB"/> color space.
        /// </returns>
        public RGB ToRGB()
        {
            return ToRGB(Hue, Saturation, Luminance);
        }

        /// <summary>
        /// Converts the instance into the target <see cref="IColorSpace"/>.
        /// </summary>
        /// <typeparam name="T">The target <see cref="IColorSpace"/> in which the instance should be converted.</typeparam>
        /// <returns>
        /// The converted <see cref="IColorSpace"/> instance.
        /// </returns>
        public T To<T>() where T : IColorSpace, new()
        {
            if (typeof(T) == typeof(HSL))
            {
                // just return a new HSL
                return new T { Color = Color };
            }

            if (typeof(T) == typeof(RGB))
            {
                // conversion to RGB is supported
                return new T { Color = ToRGB().Color };
            }

            RGB rgb = ToRGB();

            // convert from RGB to the target color space
            return rgb.To<T>();
        }

        /// <summary>
        /// Converts a source <see cref="IColorSpace"/> into this <see cref="IColorSpace"/>.
        /// </summary>
        /// <typeparam name="T">The source <see cref="IColorSpace"/> from which this instance should be converted.</typeparam>
        /// <param name="color">The <see cref="IColorSpace"/> to be converted.</param>
        public void From<T>(T color) where T : IColorSpace, new()
        {
            if (typeof(T) == typeof(HSL))
            {
                // just return a new HSL
                Color = color.Color;
                return;
            }

            if (typeof(T) == typeof(RGB))
            {
                // converting to RGB is supported
                Color = FromRGB(color.Color.A, color.Color.B, color.Color.C).Color;
                return;
            }

            // try to convert to RGB
            RGB rgb = color.To<RGB>();

            // convert from XYZ to the target color space
            Color = FromRGB(rgb).Color;
        }

        #endregion
    }

}
