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
    /// A struct which defines the CIE Luv color space.
    /// </summary>
    public struct LUV : IColorSpace
    {
        /// <summary>
        /// Gets an empty <see cref="LUV"/> structure.
        /// </summary>
        public static readonly LUV Empty;

        /// <summary>
        /// The constant E.
        /// </summary>
        public static readonly double E = 0.008856;

        /// <summary>
        /// The constant K.
        /// </summary>
        public static readonly double K = 903.3;

        #region Accessors

        /// <summary>
        /// Gets or sets triple of color channels.
        /// </summary>
        /// <value>The color triple.</value>
        public ColorTriple Color
        {
            get
            {
                return new ColorTriple(L, U, V);
            }
            set
            {
                L = value.A;
                U = value.B;
                V = value.C;
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
        /// Gets or sets the U channel.
        /// </summary>
        public double U { get; set; }

        /// <summary>
        /// Gets or sets the V channel.
        /// </summary>
        public double V { get; set; }

        #endregion

        ///// <summary>
        ///// Initializes a new instance of the <see cref="LUV"/> struct.
        ///// </summary>
        //public LUV()
        //{
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="LUV"/> struct.
        /// </summary>
        /// <param name="l">The L channel.</param>
        /// <param name="u">The U channel.</param>
        /// <param name="v">The V channel.</param>
        public LUV(double l, double u, double v)
            : this()
        {
            L = l;
            U = u;
            V = v;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LUV"/> struct.
        /// </summary>
        /// <param name="luv">The color in <see cref="LUV"/> color space.</param>
        public LUV(LUV luv)
            : this()
        {
            L = luv.L;
            U = luv.U;
            V = luv.V;
        }

        #region convert

        /// <summary>
        /// Converts from <see cref="LUV"/> to <see cref="XYZ"/> color space.
        /// </summary>
        /// <param name="l">The L channel.</param>
        /// <param name="u">The U channel.</param>
        /// <param name="v">The V channel.</param>
        /// <returns>
        /// The color in <see cref="LUV"/> color space.
        /// </returns>
        public static XYZ ToXYZ(double l, double u, double v)
        {
            var ur = 4 * XYZ.D65.X / (XYZ.D65.X + 15 * XYZ.D65.Y + 3 * XYZ.D65.Z);
            var vr = 9 * XYZ.D65.Y / (XYZ.D65.X + 15 * XYZ.D65.Y + 3 * XYZ.D65.Z);

            var a = 1.0 / 3.0 * (52 * l / (u + 13 * l * ur) - 1);
            var c = -1.0 / 3.0;

            var y = (l > K * E) ? Math.Pow((l + 16) / 116, 3) : l / K;

            var b = -5 * y;
            var d = y * (39 * l / (v + 13 * l * vr) - 5);

            var x = (d - b) / (a - c);
            var z = x * a + b;

            return new XYZ
            {
                X = x,
                Y = y,
                Z = z
            };
        }

        /// <summary>
        /// Converts from <see cref="LUV"/> to <see cref="XYZ"/> color space.
        /// </summary>
        /// <param name="luv">The source color in <see cref="LUV"/> color space.</param>
        /// <returns>
        /// The color in <see cref="XYZ"/> color space.
        /// </returns>
        public static XYZ ToXYZ(LUV luv)
        {
            return ToXYZ(luv.L, luv.U, luv.V);
        }

        /// <summary>
        /// Converts from <see cref="LUV"/> to <see cref="XYZ"/> color space.
        /// </summary>
        /// <returns>
        /// The color in <see cref="XYZ"/> color space.
        /// </returns>
        public XYZ ToXYZ()
        {
            return ToXYZ(L, U, V);
        }

        /// <summary>
        /// Converts from <see cref="XYZ"/> to <see cref="LUV"/> color space.
        /// </summary>
        /// <param name="x">The X channel.</param>
        /// <param name="y">The Y channel.</param>
        /// <param name="z">The Z channel.</param>
        /// <returns>
        /// The color in <see cref="XYZ"/> color space.
        /// </returns>
        public static LUV FromXYZ(double x, double y, double z)
        {
            var ur = 4 * XYZ.D65.X / (XYZ.D65.X + 15 * XYZ.D65.Y + 3 * XYZ.D65.Z);
            var vr = 9 * XYZ.D65.Y / (XYZ.D65.X + 15 * XYZ.D65.Y + 3 * XYZ.D65.Z);

            var u = 0.0;
            var v = 0.0;

            // prevent NaN if x, y and z are zero
            if (x != 0 && y != 0 && z != 0)
            {
                u = 4 * x / (x + 15 * y + 3 * z);
                v = 9 * y / (x + 15 * y + 3 * z);
            }

            var yr = y / XYZ.D65.Y;

            var l = (yr > E) ? (116 * Math.Pow(yr, 1.0 / 3.0) - 16) : K * yr;

            u = 13 * l * (u - ur);
            v = 13 * l * (v - vr);

            return new LUV
            {
                L = l,
                U = u,
                V = v
            };
        }

        /// <summary>
        /// Converts from <see cref="XYZ"/> to <see cref="LUV"/> color space.
        /// </summary>
        /// <param name="xyz">The source color in <see cref="XYZ"/> color space.</param>
        /// <returns>
        /// The color in <see cref="LUV"/> color space.
        /// </returns>
        public static LUV FromXYZ(XYZ xyz)
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
            if (typeof(T) == typeof(LUV))
            {
                // just return a new LUV
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
            if (typeof(T) == typeof(LUV))
            {
                // just return a new LUV
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
    } 
}
