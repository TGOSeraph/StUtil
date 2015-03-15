using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StUtil.Data.Generic
{
    public class Sides<T>
    {
        public T Top { get; set; }
        public T Bottom { get; set; }
        public T Left { get; set; }
        public T Right { get; set; }

        public Sides(T all)
            : this(all, all, all, all)
        {
        }

        public Sides(T left, T top, T right, T bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }
    }
}
