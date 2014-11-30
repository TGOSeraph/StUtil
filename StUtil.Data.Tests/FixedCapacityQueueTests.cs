using Microsoft.VisualStudio.TestTools.UnitTesting;
using StUtil.Data.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StUtil.Data.Tests
{
    [TestClass]
    public class FixedCapacityQueueTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void InvalidCapacity()
        {
            FixedCapacityQueue<int> items = new FixedCapacityQueue<int>(-1);
        }

        [TestMethod]
        public void Enqueue()
        {
            FixedCapacityQueue<int> items = new FixedCapacityQueue<int>(10);
            items.Enqueue(10);
            Assert.AreEqual(1, items.Count, "Count did match expected value");
        }

        [TestMethod]
        public void EnqueueMany()
        {
            FixedCapacityQueue<int> items = new FixedCapacityQueue<int>(10);
            for (int i = 0; i < 20; i++)
            {
                items.Enqueue(i);
            }
            Assert.AreEqual(10, items.Count, "Count did match expected value");
        }

        [TestMethod]
        public void Dequeue()
        {
            const int value = 10;

            FixedCapacityQueue<int> items = new FixedCapacityQueue<int>(10);
            items.Enqueue(value);
            int i = items.Dequeue();
            Assert.AreEqual(value, i, "Dequeued value did not match expected value");
            Assert.AreEqual(0, items.Count, "Count did not meet expected value");
        }

        [TestMethod]
        public void DequeueMany()
        {
            FixedCapacityQueue<int> items = new FixedCapacityQueue<int>(10);
            for (int i = 0; i < 10; i++)
            {
                items.Enqueue(i);
            }
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(i, items.Dequeue(), "Dequeued value did not match expected value");
                Assert.AreEqual(9 - i, items.Count, "Count did not meet expected value");
            }
        }

        [TestMethod]
        public void AutoDequeue()
        {
            const int queueSize = 10;
            const int numberToAdd = 100;
            const int expectedDequeueCount = numberToAdd - queueSize;

            FixedCapacityQueue<int> items = new FixedCapacityQueue<int>(queueSize);
            int dequeueCount = 0;
            items.ItemAutoDequeued += (sender, e) =>
            {
                Assert.AreSame(items, sender, "Sender was not the queue");
                Assert.AreEqual(dequeueCount, e.Item, "Dequeue count did not meet expected value");
                Assert.AreEqual(10, items.Count, "Count did not meet expected value");
                dequeueCount++;
            };
            for (int i = 0; i < numberToAdd; i++)
            {
                items.Enqueue(i);
            }
            Assert.AreEqual(expectedDequeueCount, dequeueCount, "Dequeue count did not meet expected value");
            Assert.AreEqual(queueSize, items.Count, "Count did not meet expected value");
        }

        [TestMethod]
        public void BulkEnqueue()
        {
            FixedCapacityQueue<int> items = new FixedCapacityQueue<int>(10);
            for (int i = 0; i < 100000; i++)
            {
                items.Enqueue(i);
            }
        }

        [TestMethod]
        public void Multithreaded()
        {
            const int queueSize = 100000;

            FixedCapacityQueue<int> items = new FixedCapacityQueue<int>(queueSize);
            Parallel.For(0, queueSize, i =>
            {
                items.Enqueue(i);
            });

            Assert.AreEqual(queueSize, items.Count, "Count did not meet expected value");
            Assert.AreEqual(queueSize, items.Distinct().Count(), "Duplicate items were added");
        }
    }
}