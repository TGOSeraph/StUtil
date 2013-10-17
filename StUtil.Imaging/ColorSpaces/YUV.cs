using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace StUtil.Imaging.ColorSpaces
{
    public struct YUV : IColorSpace
    {
        /// <summary>
        /// Gets an empty <see cref="YUV"/> structure.
        /// </summary>
        public static readonly YUV Empty;

        #region Accessors

        /// <summary>
        /// Gets or sets triple of color channels.
        /// </summary>
        /// <value>The color triple.</value>
        public ColorTriple Color
        {
            get
            {
                return new ColorTriple(Y, U, V);
            }
            set
            {
                Y = value.A;
                U = value.B;
                V = value.C;
            }
        }

        /// <summary>
        /// Gets or sets the Y channel.
        /// </summary>
        /// <value>The Y channel.</value>
        public double Y { get; set; }

        /// <summary>
        /// Gets or sets the U channel.
        /// </summary>
        public double U { get; set; }

        /// <summary>
        /// Gets or sets the V channel.
        /// </summary>
        public double V { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LUV"/> struct.
        /// </summary>
        /// <param name="y">The Y channel.</param>
        /// <param name="u">The U channel.</param>
        /// <param name="v">The V channel.</param>
        public YUV(double y, double u, double v)
            : this()
        {
            Y = y;
            U = u;
            V = v;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YUV"/> struct.
        /// </summary>
        /// <param name="yuv">The color in <see cref="YUV"/> color space.</param>
        public YUV(YUV yuv)
            : this()
        {
            Y = yuv.Y;
            U = yuv.U;
            V = yuv.V;
        }

        #region convert

        public static RGB ToRGB(YUV yuv)
        {
            return ToRGB(yuv.Y, yuv.U, yuv.V);
        }

        public static RGB ToRGB(double y, double u, double v)
        {
            return new RGB
            {
                Red = Convert.ToInt32((y + 1.139837398373983740 * v) * 255),
                Green = Convert.ToInt32((y - 0.3946517043589703515 * u - 0.5805986066674976801 * v) * 255),
                Blue = Convert.ToInt32((y + 2.032110091743119266 * u) * 255)
            };
        }

        public RGB ToRGB()
        {
            return ToRGB(this);
        }

        public static YUV FromRGB(RGB rgb)
        {
            return FromRGB(rgb.R, rgb.G, rgb.B);
        }

        public static YUV FromRGB(double r, double g, double b)
        {
            r = r / 255.0;
            g = g / 255.0;
            b = b / 255.0;

            return new YUV
            {
                Y = 0.299 * r + 0.587 * g + 0.114 * b,
                U = -0.14713 * r - 0.28886 * g + 0.436 * b,
                V = 0.615 * r - 0.51499 * g - 0.10001 * b
            };
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
            if (typeof(T) == typeof(YUV))
            {
                // just return a new YUV
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
            if (typeof(T) == typeof(YUV))
            {
                // just return a new YUV
                Color = color.Color;
                return;
            }

            if (typeof(T) == typeof(RGB))
            {
                // conversion from XYZ is supported
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
