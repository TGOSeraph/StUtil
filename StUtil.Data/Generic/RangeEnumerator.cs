using System;
using System.Collections.Generic;

namespace StUtil.Data.Generic
{
    /// <summary>
    /// Enumerates a range object
    /// </summary>
    /// <typeparam name="TNumeric">The numeric type of the elements to enumerate</typeparam>
    public class RangeEnumerator<TNumeric> : IEnumerator<TNumeric> where TNumeric : struct, IConvertible, IComparable<TNumeric>
    {
        /// <summary>
        /// The range object to enumerate
        /// </summary>
        private readonly Range<TNumeric> range;

        /// <summary>
        /// The current index of the enumeration
        /// </summary>
        private int index = -1;

        /// <summary>
        /// The current element in the enumeration
        /// </summary>
        public TNumeric Current
        {
            get
            {
                return (dynamic)this.range.Minimum + (index * (dynamic)range.Step);
            }
        }

        /// <summary>
        /// The current element in the enumeration
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="range">The range to enumerate</param>
        public RangeEnumerator(Range<TNumeric> range)
        {
            this.range = range;
        }

        /// <summary>
        /// Dispose the class
        /// </summary>
        public void Dispose()
        {
            return;
        }

        /// <summary>
        /// Move the the next number in the range
        /// </summary>
        /// <returns>If there are any items remaining in the range</returns>
        public bool MoveNext()
        {
            dynamic num = (dynamic)this.range.Minimum + ((index + 1) * (dynamic)range.Step);

            if ((dynamic)range.Step < 0)
            {
                if (num < this.range.Maximum)
                    return false;
            }
            else
            {
                if (num > this.range.Maximum)
                    return false;
            }
            this.index++;
            return true;
        }

        /// <summary>
        /// Reset the enumeration to the beginning of the range
        /// </summary>
        public void Reset()
        {
            this.index = -1;
        }
    }
}