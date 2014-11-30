using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for ControlCollections
    /// </summary>
    public static class ControlCollectionExtensions
    {
        /// <summary>
        /// Add an item to the collection and return the added item
        /// </summary>
        /// <param name="list">The collection to add to</param>
        /// <param name="item">The item to add</param>
        /// <returns>The item that was added</returns>
        public static Control AddReturnItem(this Control.ControlCollection list, Control item)
        {
            list.Add(item);
            return item;
        }

        /// <summary>
        /// Add an item to the collection and return the collection
        /// </summary>
        /// <param name="list">The collection to add to</param>
        /// <param name="item">The item to add</param>
        /// <returns>The collection that was added to</returns>
        public static Control.ControlCollection AddReturnList(this Control.ControlCollection list, Control item)
        {
            list.Add(item);
            return list;
        }
    }
}