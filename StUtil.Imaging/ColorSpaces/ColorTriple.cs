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
    /// A struct which defines a combination of three color channels.
    /// </summary>
    public struct ColorTriple
    {
        /// <summary>
        /// Gets or sets the A channel.
        /// </summary>
        public double A { get; set; }

        /// <summary>
        /// Gets or sets the B channel.
        /// </summary>
        public double B { get; set; }

        /// <summary>
        /// Gets or sets the C channel.
        /// </summary>
        public double C { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorTriple"/> struct.
        /// </summary>
        /// <param name="a">The A channel.</param>
        /// <param name="b">The B channel.</param>
        /// <param name="c">The C channel.</param>
        public ColorTriple(double a, double b, double c)
            : this()
        {
            A = a;
            B = b;
            C = c;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorTriple"/> struct.
        /// </summary>
        /// <param name="color">The <see cref="ColorTriple"/>.</param>
        public ColorTriple(ColorTriple color)
            : this()
        {
            A = color.A;
            B = color.B;
            C = color.C;
        }
    }

}
