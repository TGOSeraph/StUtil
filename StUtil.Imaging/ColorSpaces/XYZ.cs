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
    /// A struct which defines the CIE XYZ color space.
    /// </summary>
    public struct XYZ : IColorSpace
    {
        /// <summary>
        /// Gets an empty <see cref="XYZ"/> instance.
        /// </summary>
        public static readonly XYZ Empty;

        /// <summary>
        /// Gets the D65 (white) instance.
        /// </summary>
        public static readonly XYZ D65 = new XYZ(0.95047, 1.0, 1.08883);

        /// <summary>
        /// The transformation matrix from SRGB to CIE XYZ
        /// </summary>
        public static readonly double[,] D65ToXYZ =
        {
            { 0.4124564, 0.3575761, 0.1804375 },
            { 0.2126729, 0.7151522, 0.0721750 },
            { 0.0193339, 0.1191920, 0.9503041 },
        };

        /// <summary>
        /// The transformation matrix from CIE XYZ to SRGB
        /// </summary>
        public static readonly double[,] D65FromXYZ =
        {
            { 3.2404542, -1.5371385, -0.4985314 },
            { -0.9692660, 1.8760108, 0.0415560 },
            { 0.0556434, -0.2040259, 1.0572252 }
        };

        #region Accessors

        /// <summary>
        /// Gets or sets triple of color channels.
        /// </summary>
        /// <value>The color triple.</value>
        public ColorTriple Color
        {
            get
            {
                return new ColorTriple(X, Y, Z);
            }
            set
            {
                X = value.A;
                Y = value.B;
                Z = value.C;
            }
        }

        /// <summary>
        /// Gets or sets the X channel.
        /// </summary>
        /// <value>The X channel.</value>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y channel.
        /// </summary>
        /// <value>The Y channel.</value>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the Z channel.
        /// </summary>
        /// <value>The Z channel.</value>
        public double Z { get; set; }

        #endregion

        ///// <summary>
        ///// Initializes a new instance of the <see cref="XYZ"/> struct.
        ///// </summary>
        //public XYZ()
        //{
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="XYZ"/> struct.
        /// </summary>
        /// <param name="x">The X channel.</param>
        /// <param name="y">The Y channel.</param>
        /// <param name="z">The Z channel.</param>
        public XYZ(double x, double y, double z)
            : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XYZ"/> struct.
        /// </summary>
        /// <param name="xyz">The color in <see cref="XYZ"/> color space.</param>
        public XYZ(XYZ xyz)
            : this()
        {
            X = xyz.X;
            Y = xyz.Y;
            Z = xyz.Z;
        }

        #region convert

        /// <summary>
        /// Converts from <see cref="XYZ"/> to <see cref="RGB"/> color space.
        /// </summary>
        /// <param name="x">The X channel.</param>
        /// <param name="y">The Y channel.</param>
        /// <param name="z">The Z channel.</param>
        /// <returns>
        /// The color in <see cref="RGB"/> color space.
        /// </returns>
        public static RGB ToRGB(double x, double y, double z)
        {
            // convert from CIE XYZ (D65)
            var red = x * D65FromXYZ[0, 0] + y * D65FromXYZ[0, 1] + z * D65FromXYZ[0, 2];
            var green = x * D65FromXYZ[1, 0] + y * D65FromXYZ[1, 1] + z * D65FromXYZ[1, 2];
            var blue = x * D65FromXYZ[2, 0] + y * D65FromXYZ[2, 1] + z * D65FromXYZ[2, 2];

            // assume SRGB
            return SRGB.ToRGB(red, green, blue);
        }

        /// <summary>
        /// Converts from <see cref="XYZ"/> to <see cref="RGB"/> color space.
        /// </summary>
        /// <param name="xyz">The source color in <see cref="XYZ"/> color space.</param>
        /// <returns>
        /// The color in <see cref="RGB"/> color space.
        /// </returns>
        public static RGB ToRGB(XYZ xyz)
        {
            return ToRGB(xyz.X, xyz.Y, xyz.Z);
        }

        /// <summary>
        /// Converts from <see cref="RGB"/> to <see cref="XYZ"/> color space.
        /// </summary>
        /// <param name="r">The red channel.</param>
        /// <param name="g">The green channel.</param>
        /// <param name="b">The blue channel.</param>
        /// <returns>
        /// The color in <see cref="XYZ"/> color space.
        /// </returns>
        public static XYZ FromRGB(double r, double g, double b)
        {
            // assume SRGB
            var srgb = SRGB.FromRGB(r, g, b);

            // convert to CIE XYZ
            return new XYZ
            {
                X = srgb.R * D65ToXYZ[0, 0] + srgb.G * D65ToXYZ[0, 1] + srgb.B * D65ToXYZ[0, 2],
                Y = srgb.R * D65ToXYZ[1, 0] + srgb.G * D65ToXYZ[1, 1] + srgb.B * D65ToXYZ[1, 2],
                Z = srgb.R * D65ToXYZ[2, 0] + srgb.G * D65ToXYZ[2, 1] + srgb.B * D65ToXYZ[2, 2]
            };
        }

        /// <summary>
        /// Converts from <see cref="RGB"/> to <see cref="XYZ"/> color space.
        /// </summary>
        /// <param name="rgb">The source color in <see cref="RGB"/> color space.</param>
        /// <returns>
        /// The color in <see cref="XYZ"/> color space.
        /// </returns>
        public static XYZ FromRGB(RGB rgb)
        {
            return FromRGB(rgb.R, rgb.G, rgb.B);
        }

        /// <summary>
        /// Converts this <see cref="XYZ"/> to <see cref="RGB"/> color space.
        /// </summary>
        /// <returns>
        /// The color in <see cref="RGB"/> color space.
        /// </returns>
        public RGB ToRGB()
        {
            return ToRGB(X, Y, Z);
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
            if (typeof(T) == typeof(XYZ))
            {
                // just return a new XYZ
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
            if (typeof(T) == typeof(XYZ))
            {
                // just return a new XYZ
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

            // convert from RGB to the target color space
            Color = FromRGB(rgb).Color;
        }

        #endregion
    } 
}
