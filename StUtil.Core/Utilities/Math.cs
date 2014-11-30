using StUtil.Data.Generic;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace StUtil.Utilities
{
    public static class Math
    {
        /// <summary>
        /// Clamps the value to a byte. If value is greater than 255, then it will be 255. If value
        /// is less than 0 it will be 0.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value clamped to the range of a byte</returns>
        public static byte ClampByte(double value)
        {
            return (byte)value > byte.MaxValue
                ? byte.MaxValue
                : (byte)value < byte.MinValue
                    ? byte.MinValue
                    : (byte)value;
        }

        /// <summary>
        /// Clamps the value to a byte. If value is greater than 255, then it will be 255. If value
        /// is less than 0 it will be 0.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value clamped to the range of a byte</returns>
        public static byte ClampByte(int value)
        {
            return ClampByte((double)value);
        }

        /// <summary>
        /// Clamps the value to a byte. If value is greater than 255, then it will be 255. If value
        /// is less than 0 it will be 0.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value clamped to the range of a byte</returns>
        public static byte ClampByte(uint value)
        {
            return ClampByte((double)value);
        }

        /// <summary>
        /// Clamps the value to a byte. If value is greater than 255, then it will be 255. If value
        /// is less than 0 it will be 0.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value clamped to the range of a byte</returns>
        public static byte ClampByte(long value)
        {
            return ClampByte((double)value);
        }

        /// <summary>
        /// Clamps the value to a byte. If value is greater than 255, then it will be 255. If value
        /// is less than 0 it will be 0.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value clamped to the range of a byte</returns>
        public static byte ClampByte(ulong value)
        {
            return ClampByte((double)value);
        }

        /// <summary>
        /// Clamps the value to a byte. If value is greater than 255, then it will be 255. If value
        /// is less than 0 it will be 0.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value clamped to the range of a byte</returns>
        public static byte ClampByte(short value)
        {
            return ClampByte((double)value);
        }

        /// <summary>
        /// Clamps the value to a byte. If value is greater than 255, then it will be 255. If value
        /// is less than 0 it will be 0.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value clamped to the range of a byte</returns>
        public static byte ClampByte(ushort value)
        {
            return ClampByte((double)value);
        }

        /// <summary>
        /// Clamps the value to a byte. If value is greater than 255, then it will be 255. If value
        /// is less than 0 it will be 0.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value clamped to the range of a byte</returns>
        public static byte ClampByte(sbyte value)
        {
            return ClampByte((double)value);
        }

        /// <summary>
        /// Clamps the value to a byte. If value is greater than 255, then it will be 255. If value
        /// is less than 0 it will be 0.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value clamped to the range of a byte</returns>
        public static byte ClampByte(float value)
        {
            return ClampByte((double)value);
        }

        /// <summary>
        /// Clamps the value to a byte. If value is greater than 255, then it will be 255. If value
        /// is less than 0 it will be 0.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value clamped to the range of a byte</returns>
        public static byte ClampByte(decimal value)
        {
            return ClampByte((double)value);
        }

        /// <summary>
        /// Gets the factors of the specified number.
        /// </summary>
        /// <param name="n">The number to get the factors of.</param>
        /// <returns></returns>
        public static List<Pair<int, int>> GetFactors(int n)
        {
            if (n == 0)
                return null;

            List<Pair<int, int>> ret = new List<Pair<int, int>>();
            if (n == 1)
            {
                ret.Add(new Pair<int, int>(1, 1));
                return ret;
            }
            int i = 1;
            double sqrt = System.Math.Sqrt(n);
            while (i <= sqrt)
            {
                if (n % i == 0)
                {
                    int v = Convert.ToInt32(n / i);
                    ret.Add(new Pair<int, int>(i, v));
                    ret.Add(new Pair<int, int>(v, i));
                }
                i += 1;
            }
            ret.Sort((Pair<int, int> s1, Pair<int, int> s2) => { return s1.First.CompareTo(s2.First); });

            return ret;
        }

        /// <summary>
        /// Gets the GCD of the two numbers, the largest positive integer that divides the numbers
        /// without a remainder.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns></returns>
        public static double GreatestCommonDivisor(double a, double b)
        {
            if (b == 0)
                return a;
            return GreatestCommonDivisor(b, a % b);
        }

        /// <summary>
        /// Reduces the ratio to the smallest form.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="denominator">The denominator.</param>
        /// <returns></returns>
        public static Size ReduceRatio(double numerator, double denominator)
        {
            bool swapped = false;
            if (numerator == denominator)
            {
                return new Size(1, 1);
            }
            if ((swapped == (numerator < denominator)) == true)
            {
                double tmp = numerator;
                numerator = denominator;
                denominator = tmp;
            }
            double Divisor = GreatestCommonDivisor(numerator, denominator);
            if (swapped)
            {
                return new Size(Convert.ToInt32((numerator / Divisor)), Convert.ToInt32((denominator / Divisor)));
            }
            return new Size(Convert.ToInt32((denominator / Divisor)), Convert.ToInt32((numerator / Divisor)));
        }
    }
}