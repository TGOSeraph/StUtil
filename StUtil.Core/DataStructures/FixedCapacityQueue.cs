using System;
using System.Collections.Concurrent;

namespace StUtil.DataStructures
{
    public class FixedCapacityQueue<T> : ConcurrentQueue<T>
    {
        public class ItemAutoDequeuedEventArg : EventArgs
        {
            public T Item { get; private set; }
            public ItemAutoDequeuedEventArg(T item)
            {
                this.Item = item;
            }
        }

        public event EventHandler<ItemAutoDequeuedEventArg> ItemAutoDequeued;

        public int Capacity { get; private set; }
        public bool IsFull
        {
            get
            {
                return this.Capacity == this.Count;
            }
        }

        public FixedCapacityQueue(int capacity)
        {
            this.Capacity = capacity;
        }

        public new void Enqueue(T item)
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

        public T Dequeue()
        {
            T obj;
            if (base.TryDequeue(out obj))
            {
                return obj;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
