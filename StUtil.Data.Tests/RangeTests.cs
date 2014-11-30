using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StUtil.Data.Generic;

namespace StUtil.Data.Tests
{
    [TestClass]
    public class RangeTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidRange1()
        {
            Range<int> range = new Range<int>(10, 5, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidRange2()
        {
            Range<int> range = new Range<int>(1, 5, -2);
        }

        [TestMethod]
        public void ContainsValue()
        {
            Range<int> range = new Range<int>(1, 5);
            Assert.IsTrue(range.ContainsValue(1));
            Assert.IsTrue(range.ContainsValue(2));
            Assert.IsTrue(range.ContainsValue(3));
            Assert.IsTrue(range.ContainsValue(4));
            Assert.IsTrue(range.ContainsValue(5));
            Assert.IsFalse(range.ContainsValue(0));
            Assert.IsFalse(range.ContainsValue(6));
        }

        [TestMethod]
        public void ContainsRange()
        {
            Range<int> range = new Range<int>(1, 5);
            Assert.IsTrue(range.ContainsRange(new Range<int>(1, 5)));
            Assert.IsTrue(range.ContainsRange(new Range<int>(2, 4)));
            Assert.IsTrue(range.ContainsRange(new Range<int>(1, 1)));
            Assert.IsFalse(range.ContainsRange(new Range<int>(0, 5)));
            Assert.IsFalse(range.ContainsRange(new Range<int>(0, 4)));
            Assert.IsFalse(range.ContainsRange(new Range<int>(4, 6)));
        }

        [TestMethod]
        public void IsInsideRange()
        {
            Range<int> range = new Range<int>(1, 5);
            Assert.IsTrue(range.IsInsideRange(new Range<int>(1, 5)));
            Assert.IsFalse(range.IsInsideRange(new Range<int>(2, 4)));
            Assert.IsFalse(range.IsInsideRange(new Range<int>(1, 1)));
            Assert.IsTrue(range.IsInsideRange(new Range<int>(0, 5)));
            Assert.IsFalse(range.IsInsideRange(new Range<int>(0, 4)));
            Assert.IsFalse(range.IsInsideRange(new Range<int>(4, 6)));
        }

        [TestMethod]
        public void Enumerable()
        {
            Range<int> range = new Range<int>(1, 100, 1);
            int step = 1;
            foreach (int r in range)
            {
                Assert.AreEqual(step++, r);
            }
            Assert.AreEqual(100, range.Count());

            range = new Range<int>(1, 10, 2);
            int[] rx = range.ToArray();
            Assert.AreEqual(5, rx.Length);
            for (int i = 1; i < 10; i += 2)
            {
                Assert.AreEqual(i, rx[(i-1) / 2]);
            }

            range = new Range<int>(30, 0, -3);
            rx = range.ToArray();

        }
    }
}
