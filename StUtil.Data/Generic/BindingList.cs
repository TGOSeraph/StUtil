using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StUtil.Data.Generic
{
    [Serializable]
    public class BindingList<T> : System.ComponentModel.BindingList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingList{T}"/> class.
        /// </summary>
        public BindingList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingList{T}"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public BindingList(IList<T> list)
            : base(list)
        {
        }

        /// <summary>
        /// Removes the item.
        /// </summary>
        /// <param name="index">The index.</param>
        protected override void RemoveItem(int index)
        {
            T deleted_item = this[index];

            // take care of the event
            var deleted_arg = new ListChangedEventArgsWithRemovedItem<T>(deleted_item, index);
            OnListChanged(deleted_arg);

            // remove item without any duplicate event
            bool b = RaiseListChangedEvents;
            RaiseListChangedEvents = false;
            base.RemoveItem(index);
            RaiseListChangedEvents = b;
        }

        /// <summary>
        /// Adds the range.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                Add(item);
            }
        }
    }

    [Serializable]
    public class ListChangedEventArgsWithRemovedItem : System.ComponentModel.ListChangedEventArgs
    {
        /// <summary>Public constructor.</summary>
        /// <param name="item">The item being deleted</param>
        /// <param name="index">The index of the item being deleted, for backaward compatiblity.</param>
        internal ListChangedEventArgsWithRemovedItem(object item, int index)
            : base(ListChangedType.ItemDeleted, index, index)
        {
            if (item == null) throw new ArgumentNullException("item");
            Item = item;
        }

        /// <summary>This is the item that has been deleted from the collection.</summary>
        public virtual object Item { get; protected set; }
    }

    [Serializable]
    public class ListChangedEventArgsWithRemovedItem<T> : ListChangedEventArgsWithRemovedItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListChangedEventArgsWithRemovedItem{T}"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="index">The index.</param>
        internal ListChangedEventArgsWithRemovedItem(T item, int index)
            : base(item, index)
        {
        }

        /// <summary>This is the item that has been deleted from the collection.</summary>
        public new T Item { get { return (T)base.Item; } protected set { base.Item = value; } }
    }
}