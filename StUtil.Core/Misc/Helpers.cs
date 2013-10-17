using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace StUtil.Misc
{
    public static class Helpers
    {
        [ThreadStatic]
        private static Random random = new Random();

        public static Color RandomColor(int alpha = -1)
        {
            if (random == null)
            {
                random = new Random();
            }
            return Color.FromArgb(alpha == -1 ? random.Next(255) : alpha, random.Next(255), random.Next(255), random.Next(255));
        }
    }
}
