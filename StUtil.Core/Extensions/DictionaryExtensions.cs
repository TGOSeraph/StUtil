using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for the Dictionary class
    /// </summary>
    /// <remarks>
    /// 2013-10-18  - Initial version
    /// </remarks>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Add an item to the dictionary, or if the key already exists, update the value
        /// </summary>
        /// <typeparam name="T">The type of the dictionarys key</typeparam>
        /// <typeparam name="U">The type of the dictionarys value</typeparam>
        /// <param name="dictionary">The dictionary to add to or update</param>
        /// <param name="key">The key to add or update</param>
        /// <param name="value">The value to add or update</param>
        /// <returns>True if the key already existed, else false</returns>
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

        /// <summary>
        /// Get an element from the dictionary, if the key does not exist return a defalt value
        /// </summary>
        /// <typeparam name="T">The type of the dictionarys key</typeparam>
        /// <typeparam name="U">The type of the dictionarys value</typeparam>
        /// <param name="dictionary">The dictionary to try and get the value from</param>
        /// <param name="key">The key to retrieve the value of</param>
        /// <param name="defaultVal">A default value for if the key does not exist</param>
        /// <returns>The value of the key or a default value</returns>
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
