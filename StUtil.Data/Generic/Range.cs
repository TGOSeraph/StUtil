using System;
using System.Collections.Generic;

namespace StUtil.Data.Generic
{
    /// <summary>
    /// A range between two numbers
    /// </summary>
    /// <typeparam name="TNumeric">The numeric type of the range</typeparam>
    public class Range<TNumeric> : IEnumerable<TNumeric> where TNumeric : struct, IConvertible, IComparable<TNumeric>
    {
        /// <summary>
        /// Maximum value of the range
        /// </summary>
        public TNumeric Maximum { get; set; }

        /// <summary>
        /// Minimum value of the range
        /// </summary>
        public TNumeric Minimum { get; set; }

        /// <summary>
        /// The step between each item in the range
        /// </summary>
        public TNumeric Step { get; set; }

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="min">The minum value in the range</param>
        /// <param name="max">The inclusive maximum value in the range</param>
        public Range(TNumeric min, TNumeric max)
            : this(min, max, (TNumeric)Convert.ChangeType(1, typeof(TNumeric)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="min">The minum value in the range</param>
        /// <param name="max">The inclusive maximum value in the range</param>
        /// <param name="step">The step between item in the range</param>
        public Range(TNumeric min, TNumeric max, TNumeric step)
        {
            if ((dynamic)min > max && (dynamic)step > 0)
            {
                throw new ArgumentException("Max cannot be less than min unless step is negative.", "max");
            }
            if ((dynamic)min < max && (dynamic)step < 0)
            {
                throw new ArgumentException("Step cannot be negative if min is less than max.", "max");
            }

            this.Minimum = min;
            this.Maximum = max;
            this.Step = step;
        }

        /// <summary>
        /// Determines if another range is inside the bounds of this range
        /// </summary>
        /// <param name="Range">The child range to test</param>
        /// <returns>True if range is inside, else false</returns>
        public Boolean ContainsRange(Range<TNumeric> Range)
        {
            return this.IsValid() && Range.IsValid() && this.ContainsValue(Range.Minimum) && this.ContainsValue(Range.Maximum);
        }

        /// <summary>
        /// Determines if the provided value is inside the range
        /// </summary>
        /// <param name="value">The value to test</param>
        /// <returns>True if the value is inside Range, else false</returns>
        public Boolean ContainsValue(TNumeric value)
        {
            return (Minimum.CompareTo(value) <= 0) && (value.CompareTo(Maximum) <= 0);
        }

        /// <summary>
        /// Gets the enumerator for the range
        /// </summary>
        /// <returns>The enumerator for the range</returns>
        public IEnumerator<TNumeric> GetEnumerator()
        {
            return (IEnumerator<TNumeric>)new RangeEnumerator<TNumeric>(this);
        }

        /// <summary>
        /// Determines if this Range is inside the bounds of another range
        /// </summary>
        /// <param name="Range">The parent range to test on</param>
        /// <returns>True if range is inclusive, else false</returns>
        public Boolean IsInsideRange(Range<TNumeric> Range)
        {
            return this.IsValid() && Range.IsValid() && Range.ContainsValue(this.Minimum) && Range.ContainsValue(this.Maximum);
        }

        /// <summary>
        /// Determines if the range is valid
        /// </summary>
        /// <returns>True if range is valid, else false</returns>
        public Boolean IsValid()
        {
            return Minimum.CompareTo(Maximum) <= 0;
        }

        /// <summary>
        /// Gets the enumerator for the range
        /// </summary>
        /// <returns>The enumerator for the range</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Presents the Range in readable format
        /// </summary>
        /// <returns>String representation of the Range</returns>
        public override string ToString()
        {
            return String.Format("[{0} - {1}]", Minimum, Maximum);
        }
    }
}