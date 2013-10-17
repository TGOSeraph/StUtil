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
    /// An interface which defines a color space with an <see cref="ColorTriple"/> and convert methods.
    /// </summary>
    public interface IColorSpace
    {
        /// <summary>
        /// Gets or sets triple of color channels.
        /// </summary>
        /// <value>The color triple.</value>
        ColorTriple Color { get; set; }

        /// <summary>
        /// Converts the instance into the target <see cref="IColorSpace"/>.
        /// </summary>
        /// <typeparam name="T">The target <see cref="IColorSpace"/> in which the instance should be converted.</typeparam>
        /// <returns>
        /// The converted <see cref="IColorSpace"/> instance.
        /// </returns>
        T To<T>() where T : IColorSpace, new();

        /// <summary>
        /// Converts a source <see cref="IColorSpace"/> into this <see cref="IColorSpace"/>.
        /// </summary>
        /// <typeparam name="T">The source <see cref="IColorSpace"/> from which this instance should be converted.</typeparam>
        /// <param name="color">The <see cref="IColorSpace"/> to be converted.</param>
        void From<T>(T color) where T : IColorSpace, new();
    }

}
