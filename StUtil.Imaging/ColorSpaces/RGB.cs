using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// A struct which defines the RGB color space.
    /// </summary>
    public struct RGB : IColorSpace
    {
        /// <summary>
        /// Gets an empty <see cref="RGB"/> structure;
        /// </summary>
        public static readonly RGB Empty;

        #region Accessors

        /// <summary>
        /// Gets or sets triple of color channels.
        /// </summary>
        /// <value>The color triple.</value>
        public ColorTriple Color
        {
            get
            {
                return new ColorTriple(R, G, B);
            }
            set
            {
                R = (int)value.A;
                G = (int)value.B;
                B = (int)value.C;
            }
        }

        /// <summary>
        /// Gets or sets the red channel.
        /// </summary>
        /// <value>The red channel.</value>
        public int Red
        {
            get { return R; }
            set { R = value; }
        }

        /// <summary>
        /// Gets or sets the green channel.
        /// </summary>
        /// <value>The green channel.</value>
        public int Green
        {
            get { return G; }
            set { G = value; }
        }

        /// <summary>
        /// Gets or sets the blue channel.
        /// </summary>
        /// <value>The blue channel.</value>
        public int Blue
        {
            get { return B; }
            set { B = value; }
        }

        /// <summary>
        /// Short accessor for the channel.
        /// </summary>
        /// <value>The red channel.</value>
        public int R { get; set; }

        /// <summary>
        /// Short accessor for the green channel.
        /// </summary>
        /// <value>The green channel.</value>
        public int G { get; set; }

        /// <summary>
        /// Short accessor for the blue channel.
        /// </summary>
        /// <value>The blue channel.</value>
        public int B { get; set; }

        #endregion

        ///// <summary>
        ///// Initializes a new instance of the <see cref="RGB"/> struct.
        ///// </summary>
        //public RGB()
        //{
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="RGB"/> struct.
        /// </summary>
        /// <param name="r">The red channel.</param>
        /// <param name="g">The green channel.</param>
        /// <param name="b">The blue channel.</param>
        public RGB(int r, int g, int b)
            : this()
        {
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RGB"/> struct.
        /// </summary>
        /// <param name="rgb">The color in <see cref="RGB"/> color space.</param>
        public RGB(RGB rgb)
            : this()
        {
            R = rgb.R;
            G = rgb.G;
            B = rgb.B;
        }

        #region convert

        /// <summary>
        /// Converts the instance into the target <see cref="IColorSpace"/>.
        /// </summary>
        /// <typeparam name="T">The target <see cref="IColorSpace"/> in which the instance should be converted.</typeparam>
        /// <returns>
        /// The converted <see cref="IColorSpace"/> instance.
        /// </returns>
        public T To<T>() where T : IColorSpace, new()
        {
            if (typeof(T) == typeof(RGB))
            {
                // just return a new RGB
                return new T { Color = Color };
            }

            T target = new T();

            // convert from RGB to the target color space
            target.From<RGB>(this);

            return target;
        }

        /// <summary>
        /// Converts a source <see cref="IColorSpace"/> into this <see cref="IColorSpace"/>.
        /// </summary>
        /// <typeparam name="T">The source <see cref="IColorSpace"/> from which this instance should be converted.</typeparam>
        /// <param name="color">The <see cref="IColorSpace"/> to be converted.</param>
        public void From<T>(T color) where T : IColorSpace, new()
        {
            if (typeof(T) == typeof(RGB))
            {
                // just take the RGB values
                Color = color.Color;
                return;
            }

            // convert to RGB
            Color = color.To<RGB>().Color;
        }

        public static RGB FromColor(Color color)
        {
            return new RGB()
            {
                Color = new ColorTriple
                {
                    A = color.R,
                    B = color.G,
                    C = color.B
                }
            };
        }

        public Color ToColor()
        {
            return System.Drawing.Color.FromArgb(R, G, B);
        }

        #endregion
    }
}
