using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Extensions
{
    public static class DictionaryExtensions
    {
        public static bool AddOrUpdate<T, U>(this Dictionary<T, U> dictionary, T key, U value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return true;
            }
            else
            {
                dictionary.Add(key, value);
                return false;
            }
        }

        public static U GetOrDefault<T, U>(this Dictionary<T, U> dictionary, T key, U defaultVal = default(U))
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                return defaultVal;
            }
        }
    }
}
