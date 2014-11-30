using System;
using System.Collections.Concurrent;

namespace StUtil.Data.Generic
{
    /// <summary>
    /// A queue that will automatically remove items when new ones are added to maintain a maximum capacity
    /// </summary>
    /// <typeparam name="TItem">The type of the items that will be queued</typeparam>
    public class FixedCapacityQueue<TItem> : ConcurrentQueue<TItem>
    {
        public event EventHandler<ItemAutoDequeuedEventArg> ItemAutoDequeued;

        /// <summary>
        /// The capacity of the queue
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// If the queue has reached its maximum capacity
        /// </summary>
        public bool IsFull
        {
            get
            {
                return this.Capacity == this.Count;
            }
        }

        /// <summary>
        /// Initializes a new instance of the class
        /// </summary>
        /// <param name="capacity">The maximum capacity of the queue</param>
        public FixedCapacityQueue(int capacity)
        {
            if (capacity < 1) throw new ArgumentOutOfRangeException("capacity", capacity, "Capacity must be greater than 0.");
            this.Capacity = capacity;
        }

        /// <summary>
        /// Remove an item from the queue
        /// </summary>
        /// <returns>The item that has been removed from the queue</returns>
        public TItem Dequeue()
        {
            TItem obj;
            if (base.TryDequeue(out obj))
            {
                return obj;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Add a new item to the queue
        /// </summary>
        /// <param name="item">The item to add</param>
        public new void Enqueue(TItem item)
        {
            base.Enqueue(item);
            lock (this)
            {
                while (base.Count > this.Capacity)
                {
                    base.TryDequeue(out item);
                    if (ItemAutoDequeued != null)
                        ItemAutoDequeued(this, new ItemAutoDequeuedEventArg(item));
                }
            }
        }

        public class ItemAutoDequeuedEventArg : EventArgs
        {
            /// <summary>
            /// The item  that has been dequeued
            /// </summary>
            public TItem Item { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="ItemAutoDequeuedEventArg"/> class.
            /// </summary>
            /// <param name="item">The item.</param>
            public ItemAutoDequeuedEventArg(TItem item)
            {
                this.Item = item;
            }
        }
    }
}