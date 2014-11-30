using System;
using System.Collections.Generic;

namespace StUtil.Data.Specialised
{
    /// <summary>
    /// Reperesents an item that is dependant on other items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Dependacy<T>
    {
        /// <summary>
        /// Gets the item that has dependancies.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public T Item { get; private set; }

        /// <summary>
        /// Gets the items that this item depends on.
        /// </summary>
        /// <value>
        /// The depends on.
        /// </value>
        public List<T> DependsOn { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dependacy{T}"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="dependsOn">The items it depends on.</param>
        public Dependacy(T item, List<T> dependsOn = null)
        {
            DependsOn = dependsOn ?? new List<T>();
            Item = item;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Item.ToString() + " -> {" + String.Join(",", DependsOn) + "}";
        }
    }
}