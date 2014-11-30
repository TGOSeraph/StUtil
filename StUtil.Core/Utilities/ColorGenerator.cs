using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Utilities
{
    /// <summary>
    /// Class to generate colors
    /// </summary>
    public static class ColorGenerator
    {
        /// <summary>
        /// The random number generator
        /// </summary>
        [ThreadStatic]
        private static Random random = new Random();

        /// <summary>
        /// Generates a new random color.
        /// </summary>
        /// <param name="alpha">The alpha value. If -1 it will be generated at random</param>
        /// <returns></returns>
        public static Color Generate(int alpha = -1)
        {
            if (random == null)
            {
                random = new Random();
            }
            return Color.FromArgb(alpha == -1 ? random.Next(255) : alpha, random.Next(255), random.Next(255), random.Next(255));
        }
    }
}