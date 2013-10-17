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
    /// A struct which defines the HSB color space.
    /// </summary>
    public struct HSB : IColorSpace
    {
        /// <summary>
        /// Gets an empty <see cref="HSB"/> structure;
        /// </summary>
        public static readonly HSB Empty;

        #region Accessors

        /// <summary>
        /// Gets or sets triple of color channels.
        /// </summary>
        /// <value>The color triple.</value>
        public ColorTriple Color
        {
            get
            {
                return new ColorTriple(H, S, B);
            }
            set
            {
                H = value.A;
                S = value.B;
                B = value.C;
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
        /// Gets or sets the brightness channel.
        /// </summary>
        /// <value>The brightness.</value>
        public double Brightness
        {
            get { return B; }
            set { B = value; }
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
        /// Short accessor for the blue channel.
        /// </summary>
        /// <value>The blue channel.</value>
        public double B { get; set; }

        #endregion

        ///// <summary>
        ///// Initializes a new instance of the <see cref="HSB"/> struct.
        ///// </summary>
        //public HSB()
        //{
        //}

        /// <summary>
        /// Creates an instance of a HSB structure.
        /// </summary>
        /// <param name="h">The hue value.</param>
        /// <param name="s">The saturation value.</param>
        /// <param name="b">The brightness value.</param>
        public HSB(double h, double s, double b)
            : this()
        {
            H = h;
            S = s;
            B = b;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HSB"/> struct.
        /// </summary>
        /// <param name="hsb">The color in <see cref="HSB"/> color space.</param>
        public HSB(HSB hsb)
            : this()
        {
            H = hsb.H;
            S = hsb.S;
            B = hsb.B;
        }

        #region convert HSB

        /// <summary>
        /// Converts from <see cref="HSB"/> to <see cref="RGB"/> color space.
        /// </summary>
        /// <param name="h">The hue channel.</param>
        /// <param name="s">The saturation channel.</param>
        /// <param name="b">The brightness channel.</param>
        /// <returns>
        /// The color in <see cref="RGB"/> color space.
        /// </returns>
        public static RGB ToRGB(double h, double s, double b)
        {
            var red = 0.0;
            var green = 0.0;
            var blue = 0.0;

            if (s == 0)
            {
                red = green = blue = b;
            }
            else
            {
                // the color wheel consists of 6 sectors. Figure out which sector you're in.
                var sectorPos = h / 60.0;
                var sectorNumber = (int)(Math.Floor(sectorPos));

                // get the fractional part of the sector
                var fractionalSector = sectorPos - sectorNumber;

                // calculate values for the three axes of the color.
                var p = b * (1.0 - s);
                var q = b * (1.0 - (s * fractionalSector));
                var t = b * (1.0 - (s * (1 - fractionalSector)));

                // assign the fractional colors to r, g, and b based on the sector the angle is in.
                switch (sectorNumber)
                {
                    case 0:
                        red = b;
                        green = t;
                        blue = p;
                        break;
                    case 1:
                        red = q;
                        green = b;
                        blue = p;
                        break;
                    case 2:
                        red = p;
                        green = b;
                        blue = t;
                        break;
                    case 3:
                        red = p;
                        green = q;
                        blue = b;
                        break;
                    case 4:
                        red = t;
                        green = p;
                        blue = b;
                        break;
                    case 5:
                        red = b;
                        green = p;
                        blue = q;
                        break;
                }
            }

            return new RGB
            {
                R = (int)Math.Round(red * 255.0),
                G = (int)Math.Round(green * 255.0),
                B = (int)Math.Round(blue * 255.0)
            };
        }

        /// <summary>
        /// Converts from <see cref="HSB"/> to <see cref="RGB"/> color space.
        /// </summary>
        /// <param name="hsb">The source color in<see cref="HSB"/> color space.</param>
        /// <returns>
        /// The color in <see cref="RGB"/> color space.
        /// </returns>
        public static RGB ToRGB(HSB hsb)
        {
            return ToRGB(hsb.H, hsb.S, hsb.B);
        }

        /// <summary>
        /// Converts from <see cref="RGB"/> to <see cref="HSB"/> color space.
        /// </summary>
        /// <param name="r">The red channel.</param>
        /// <param name="g">The green channel.</param>
        /// <param name="b">The blue channel.</param>
        /// <returns>
        /// The color in <see cref="HSB"/> color space.
        /// </returns>
        public static HSB FromRGB(double r, double g, double b)
        {
            var red = r / 255.0;
            var green = g / 255.0;
            var blue = b / 255.0;

            var max = Math.Max(red, Math.Max(green, blue));
            var min = Math.Min(red, Math.Min(green, blue));

            var h = 0.0;

            if (max == red && green >= blue)
            {
                if (max - min == 0)
                {
                    h = 0.0;
                }
                else
                {
                    h = 60 * (green - blue) / (max - min);
                }
            }
            else if (max == red && green < blue)
            {
                h = 60 * (green - blue) / (max - min) + 360;
            }
            else if (max == green)
            {
                h = 60 * (blue - red) / (max - min) + 120;
            }
            else if (max == blue)
            {
                h = 60 * (red - green) / (max - min) + 240;
            }

            var s = (max == 0) ? 0.0 : (1.0 - (min / max));

            return new HSB
            {
                H = h,
                S = s,
                B = max
            };
        }

        /// <summary>
        /// Converts from <see cref="RGB"/> to <see cref="HSB"/> color space.
        /// </summary>
        /// <param name="rgb">The source color in<see cref="RGB"/> color space.</param>
        /// <returns>
        /// The color in <see cref="HSB"/> color space.
        /// </returns>
        public static HSB FromRGB(RGB rgb)
        {
            return FromRGB(rgb.R, rgb.G, rgb.B);
        }

        /// <summary>
        /// Converts from <see cref="HSB"/> to <see cref="RGB"/> color space.
        /// </summary>
        /// <returns>
        /// The color in <see cref="RGB"/> color space.
        /// </returns>
        public RGB ToRGB()
        {
            return ToRGB(H, S, B);
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
            if (typeof(T) == typeof(HSB))
            {
                // just return a new HSB
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
            if (typeof(T) == typeof(HSB))
            {
                // just return a new HSB
                Color = color.Color;
                return;
            }

            if (typeof(T) == typeof(RGB))
            {
                // conversion from RGB is supported
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
