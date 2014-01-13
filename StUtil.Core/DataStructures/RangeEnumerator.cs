using System;
using System.Collections.Generic;

namespace StUtil.DataStructures
{
    class RangeEnumerator<T> : IEnumerator<T> where T : struct, IConvertible, IComparable<T>
    {
        private int index = -1;
        private readonly Range<T> range;

        public RangeEnumerator(Range<T> range)
        {
            this.range = range;
        }

        public T Current
        {
            get 
            {
                if (index == -1)
                    throw new IndexOutOfRangeException();
                return (dynamic)this.range.Minimum + index; 
            }
        }

        public void Dispose()
        {
            return;
        }

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        public bool MoveNext()
        {
            dynamic num = (dynamic)this.range.Minimum + this.index + 1;
            if (num > this.range.Maximum)
                return false;
            this.index++;
            return true;
        }

        public void Reset()
        {
            this.index = -1;
        }
    }
}
