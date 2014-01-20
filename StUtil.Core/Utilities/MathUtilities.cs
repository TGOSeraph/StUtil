using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace StUtil.Utilities
{
    public static class MathUtilities
    {
        private static double GreatestCommonDivisor(double a, double b)
        {
            if (b == 0)
                return a;
            return GreatestCommonDivisor(b, a % b);
        }

        public static Size ReduceRatio(double Numerator, double Denominator)
        {
            bool swapped = false;
            if (Numerator == Denominator)
            {
                return new Size(1, 1);
            }
            if ((swapped == (Numerator < Denominator)) == true)
            {
                double tmp = Numerator;
                Numerator = Denominator;
                Denominator = tmp;
            }
            double Divisor = GreatestCommonDivisor(Numerator, Denominator);
            if (swapped)
            {
                return new Size(Convert.ToInt32((Numerator / Divisor)), Convert.ToInt32((Denominator / Divisor)));
            }
            return new Size(Convert.ToInt32((Denominator / Divisor)), Convert.ToInt32((Numerator / Divisor)));
        }

        public static List<Size> GetFactors(int n)
        {
            if (n == 0)
                return null;

            List<Size> ret = new List<Size>();
            if (n == 1)
            {
                ret.Add(new Size(1, 1));
                return ret;
            }
            int i = 1;
            double sqrt = System.Math.Sqrt(n);
            while (i <= sqrt)
            {
                if (n % i == 0)
                {
                    int v = Convert.ToInt32(n / i);
                    ret.Add(new Size(i, v));
                    ret.Add(new Size(v, i));
                }
                i += 1;
            }
            ret.Sort((Size s1, Size s2) => { return s1.Width.CompareTo(s2.Width); });

            return ret;
        }

        public static double[] Normalize(uint[] arr, uint count = 0)
        {
            if (count == 0)
                Array.ForEach<uint>(arr, e => { count += e; });
            double[] d = Array.ConvertAll<uint, double>(arr, new Converter<uint, double>(e => { return (double)e / count; }));
            return d;
        }

    }
}
