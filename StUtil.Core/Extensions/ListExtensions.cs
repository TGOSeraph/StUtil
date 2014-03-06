using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    public static class ListExtensions
    {
        public static List<T> Replace<T>(this List<T> list, T item, params T[] items)
        {
            return Replace(list, list.IndexOf(item), items);
        }
        public static List<T> Replace<T>(this List<T> list, int index, params T[] items)
        {
            list.RemoveAt(index);
            for (int i = 0; i < items.Length; i++)
            {
                list.Insert(index + i, items[i]);
            }
            return list;
        }

        public static T AddReturnItem<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return item;
        }
        public static IList<T> AddReturnList<T>(this IList<T> list, T item)
        {
            list.Add(item);
            return list;
        }
    }
}
