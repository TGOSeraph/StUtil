using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Debugging.Testing
{
    public class TestMethodAttribute : Attribute
    {
        public object ExpectedValue { get; set; }
        public int Iterations { get; set; }
        public string ValidationFunction { get; set; }

        public TestMethodAttribute()
        {
            Iterations = 1;
        }
    }
}
