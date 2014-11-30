using System.Collections;
using System.Collections.Generic;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extension methods for ILists
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// Adds the item and returns it.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static object AddReturnItem(this IList list, object item)
        {
            list.Add(item);
            return item;
        }

        /// <summary>
        /// Adds the item and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static T AddReturnItem<T>(this IList<T> list, T item)
        {
            return (T)AddReturnItem((IList)list, item);
        }

        /// <summary>
        /// Adds the item and returns the list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static IList AddReturnList(this IList list, object item)
        {
            list.Add(item);
            return list;
        }

        /// <summary>
        /// Adds the item and returns the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public static IList<T> AddReturnList<T>(this IList<T> list, T item)
        {
            return (IList<T>)AddReturnList((IList)list, item);
        }

        /// <summary>
        /// Replaces items following the specified start index with items from the specified replacement
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsNew">The new items.</param>
        /// <returns></returns>
        public static IList ReplaceMany(this IList list, int startIndex, params object[] itemsNew)
        {
            for (int i = 0; i < itemsNew.Length; i++)
            {
                list.RemoveAt(startIndex + i);
            }
            for (int i = 0; i < itemsNew.Length; i++)
            {
                list.Insert(startIndex + i, itemsNew[i]);
            }
            return list;
        }

        /// <summary>
        /// Replaces items following the specified start index with items from the specified replacement
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsNew">The new items.</param>
        public static IList<T> ReplaceMany<T>(this IList<T> list, int startIndex, params T[] itemsNew)
        {
            return (IList<T>)ReplaceMany(((IList)list), startIndex, itemsNew);
        }

        /// <summary>
        /// Replaces the specified object with a new object.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="itemOld">The old item.</param>
        /// <param name="itemNew">The new item.</param>
        /// <returns></returns>
        public static IList ReplaceWith(this IList list, object itemOld, object itemNew)
        {
            int pos = list.IndexOf(itemOld);
            list.RemoveAt(pos);
            list.Insert(pos, itemNew);
            return list;
        }

        /// <summary>
        /// Replaces the specified object with a new object.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="itemOld">The old item.</param>
        /// <param name="itemNew">The new item.</param>
        /// <returns></returns>
        public static IList<T> ReplaceWith<T>(this IList<T> list, T itemOld, T itemNew)
        {
            return (IList<T>)ReplaceWith(((IList)list), itemOld, itemNew);
        }

        /// <summary>
        /// Replaces the specified the with many items, inserting new items at the index of the old.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="itemOld">The old item.</param>
        /// <param name="itemsNew">The new items .</param>
        /// <returns></returns>
        public static IList ReplaceWithMany(this IList list, object itemOld, params object[] itemsNew)
        {
            int pos = list.IndexOf(itemOld);
            list.RemoveAt(pos);
            for (int i = 0; i < itemsNew.Length; i++)
            {
                list.Insert(pos + i, itemsNew[i]);
            }
            return list;
        }

        /// <summary>
        /// Replaces the specified the with many items, inserting new items at the index of the old.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="itemOld">The old item.</param>
        /// <param name="itemsNew">The new items .</param>
        /// <returns></returns>
        public static IList<T> ReplaceWithMany<T>(this IList<T> list, T itemOld, params T[] itemsNew)
        {
            return (IList<T>)ReplaceWithMany(((IList)list), itemOld, itemsNew);
        }
    }
}