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
    /// A struct which defines the CIE Lab color space.
    /// </summary>
    public struct LAB : IColorSpace
    {
        /// <summary>
        /// Gets an empty <see cref="LAB"/> structure.
        /// </summary>
        public static readonly LAB Empty;

        #region Accessors

        /// <summary>
        /// Gets or sets triple of color channels.
        /// </summary>
        /// <value>The color triple.</value>
        public ColorTriple Color
        {
            get
            {
                return new ColorTriple(L, A, B);
            }
            set
            {
                L = value.A;
                A = value.B;
                B = value.C;
            }
        }

        /// <summary>
        /// Gets or sets the lightness (L) channel.
        /// </summary>
        /// <value>The lightness (L) channel.</value>
        public double Lightness
        {
            get { return L; }
            set { L = value; }
        }

        /// <summary>
        /// Gets or sets the L channel.
        /// </summary>
        /// <value>The L channel.</value>
        public double L { get; set; }

        /// <summary>
        /// Gets or sets the A channel.
        /// </summary>
        /// <value>The A channel.</value>
        public double A { get; set; }

        /// <summary>
        /// Gets or sets the B channel.
        /// </summary>
        /// <value>The the B channel.</value>
        public double B { get; set; }

        #endregion

        ///// <summary>
        ///// Initializes a new instance of the <see cref="LAB"/> struct.
        ///// </summary>
        //public LAB()
        //{
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="LAB"/> struct.
        /// </summary>
        /// <param name="l">The L channel.</param>
        /// <param name="a">The A channel.</param>
        /// <param name="b">The B channel.</param>
        public LAB(double l, double a, double b)
            : this()
        {
            L = l;
            A = a;
            B = b;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LAB"/> struct.
        /// </summary>
        /// <param name="lab">The color in <see cref="LAB"/> color space.</param>
        public LAB(LAB lab)
            : this()
        {
            L = lab.L;
            A = lab.A;
            B = lab.B;
        }

        #region convert

        /// <summary>
        /// Converts from <see cref="LAB"/> to <see cref="XYZ"/> color space.
        /// </summary>
        /// <param name="l">The L channel.</param>
        /// <param name="a">The A channel.</param>
        /// <param name="b">The B channel.</param>
        /// <returns>
        /// The color in <see cref="XYZ"/> color space.
        /// </returns>
        public static XYZ ToXYZ(double l, double a, double b)
        {
            const double Theta = 6.0 / 29.0;

            var fy = (l + 16) / 116.0;
            var fx = fy + (a / 500.0);
            var fz = fy - (b / 200.0);

            return new XYZ
            {
                X = fx > Theta ? XYZ.D65.X * fx * fx * fx : (fx - 16.0 / 116.0) * 3 * (Theta * Theta) * XYZ.D65.X,
                Y = fy > Theta ? XYZ.D65.Y * fy * fy * fy : (fy - 16.0 / 116.0) * 3 * (Theta * Theta) * XYZ.D65.Y,
                Z = fz > Theta ? XYZ.D65.Z * fz * fz * fz : (fz - 16.0 / 116.0) * 3 * (Theta * Theta) * XYZ.D65.Z
            };
        }

        /// <summary>
        /// Converts from <see cref="LAB"/> to <see cref="XYZ"/> color space.
        /// </summary>
        /// <param name="lab">The source color in <see cref="LAB"/> color space.</param>
        /// <returns>
        /// The color in <see cref="XYZ"/> color space.
        /// </returns>
        public static XYZ ToXYZ(LAB lab)
        {
            return ToXYZ(lab.L, lab.A, lab.B);
        }

        /// <summary>
        /// Converts from <see cref="LAB"/> to <see cref="XYZ"/> color space.
        /// </summary>
        /// <returns>
        /// The color in <see cref="XYZ"/> color space.
        /// </returns>
        public XYZ ToXYZ()
        {
            return ToXYZ(L, A, B);
        }

        /// <summary>
        /// Converts from <see cref="XYZ"/> to <see cref="LAB"/> color space.
        /// </summary>
        /// <param name="x">The X channel.</param>
        /// <param name="y">The Y channel.</param>
        /// <param name="z">The Z channel.</param>
        /// <returns>
        /// The color in <see cref="LAB"/> color space.
        /// </returns>
        public static LAB FromXYZ(double x, double y, double z)
        {
            return new LAB
            {
                L = 116.0 * Transform(y / XYZ.D65.Y) - 16,
                A = 500.0 * (Transform(x / XYZ.D65.X) - Transform(y / XYZ.D65.Y)),
                B = 200.0 * (Transform(y / XYZ.D65.Y) - Transform(z / XYZ.D65.Z))
            };
        }

        /// <summary>
        /// Converts from <see cref="XYZ"/> to <see cref="LAB"/> color space.
        /// </summary>
        /// <param name="xyz">The source color in <see cref="XYZ"/> color space.</param>
        /// <returns>
        /// The color in <see cref="LAB"/> color space.
        /// </returns>
        public static LAB FromXYZ(XYZ xyz)
        {
            return FromXYZ(xyz.X, xyz.Y, xyz.Z);
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
            if (typeof(T) == typeof(LAB))
            {
                // just return a new LAB
                return new T { Color = Color };
            }

            if (typeof(T) == typeof(XYZ))
            {
                // conversion to XYZ is supported
                return new T { Color = ToXYZ().Color };
            }

            XYZ xyz = ToXYZ();

            // convert from XYZ to the target color space
            return xyz.To<T>();
        }

        /// <summary>
        /// Converts a source <see cref="IColorSpace"/> into this <see cref="IColorSpace"/>.
        /// </summary>
        /// <typeparam name="T">The source <see cref="IColorSpace"/> from which this instance should be converted.</typeparam>
        /// <param name="color">The <see cref="IColorSpace"/> to be converted.</param>
        public void From<T>(T color) where T : IColorSpace, new()
        {
            if (typeof(T) == typeof(LAB))
            {
                // just return a new LAB
                Color = color.Color;
                return;
            }

            if (typeof(T) == typeof(XYZ))
            {
                // conversion from XYZ is supported
                Color = FromXYZ(color.Color.A, color.Color.B, color.Color.C).Color;
                return;
            }

            // try to convert to XYZ
            XYZ xyz = color.To<XYZ>();

            // convert from XYZ to the target color space
            Color = FromXYZ(xyz).Color;
        }

        #endregion

        /// <summary>
        /// <see cref="XYZ"/> to <see cref="LAB"/> transformation function.
        /// </summary>
        /// <param name="t">The x, y or z channel.</param>
        /// <returns>The transformed x, y or z channel.</returns>
        private static double Transform(double t)
        {
            return t > 0.008856 ? Math.Pow(t, 1.0 / 3.0) : 7.787 * t + 16.0 / 116.0;
        }
    } 
}
