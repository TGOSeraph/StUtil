using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StUtil.Extensions
{
    public static class ListExtensions
    {
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
