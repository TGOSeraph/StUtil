using Microsoft.VisualStudio.TestTools.UnitTesting;
using StUtil.Extensions;
using System;

namespace StUtil.Core.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TimeSpanFormatter()
        {
            TimeSpan current = new TimeSpan(1, 1, 1);
            TimeSpan end = new TimeSpan(2, 2, 2);

            string f = StUtil.Formatting.TimeSpanFormatter.ProgressFormat(current, end, true);
        }
    }
}