using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using StUtil.Extensions;

namespace StUtil.Debugging.Testing
{
    public class TestResult
    {
        public object ActualResult { get; set; }
        public object ExpectedResult { get; set; }

        public bool Pass { get; set; }
        public MethodInfo Method { get; set; }
        public TimeSpan TimeTaken { get; set; }
        public int Iterations { get; set; }
        public string ValidationFunction { get; set; }

        public override string ToString()
        {
            return Method.Name + " (" + Iterations.ToString() + " iteration" + (Iterations == 1 ? "" : "s") + ")\n"
                + (ValidationFunction == null
                    ? "  Expected Value   : " + (ExpectedResult ?? "").ToString() + " [" + ExpectedResult.GetHashCode() + "]"
                    : "  Validate Method  : " + ValidationFunction) + "\n"
                + "  Return Value     : " + (ActualResult ?? "").ToString() + " [" + ActualResult.GetHashCode() + "]\n"
                + "  Time taken " + (Iterations > 1 ? "(avg)" : "") + " : " + TimeTaken.ToReadableString() + "\n"
                + "  Result           : " + (Pass ? "Pass" : "Fail");
        }
    }
}
