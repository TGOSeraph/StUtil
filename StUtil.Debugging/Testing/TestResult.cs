using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using StUtil.Extensions;

namespace StUtil.Debugging.Testing
{
    /// <summary>
    /// Class used to store and display test results
    /// </summary>
    /// <remarks>
    /// 2013-10-18  - Initial version
    /// </remarks>
    public class TestResult
    {
        /// <summary>
        /// The result that was returned from the method
        /// </summary>
        public object ActualResult { get; set; }
        /// <summary>
        /// The result that was expected from the method
        /// </summary>
        public object ExpectedResult { get; set; }

        /// <summary>
        /// If the test passed
        /// </summary>
        public bool Pass { get; set; }
        /// <summary>
        /// The method that was tested
        /// </summary>
        public MethodInfo Method { get; set; }
        /// <summary>
        /// The time the method took to run
        /// </summary>
        public TimeSpan TimeTaken { get; set; }
        /// <summary>
        /// How many iterations were performed
        /// </summary>
        public int Iterations { get; set; }
        /// <summary>
        /// The name of the validation function that was used
        /// </summary>
        public string ValidationFunction { get; set; }

        /// <summary>
        /// Convert the object to a string representation
        /// </summary>
        /// <returns>The formatted test results</returns>
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
