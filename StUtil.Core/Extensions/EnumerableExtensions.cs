using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StUtil.Extensions
{
    /// <summary>
    /// Extensions for Enumerables
    /// </summary>
    /// <remarks>
    /// 2013-06-26  - Initial version
    /// 2013-06-28  - Added TopN function
    /// </remarks>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Selects the last N items from an enumerable
        /// </summary>
        /// <typeparam name="T">The type of the enumerable</typeparam>
        /// <param name="collection">The enumerable object to take the last n from</param>
        /// <param name="n">The number of elements to take from the end of the collection</param>
        /// <returns>The last N items from the collection</returns>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> collection, int n)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (n < 0)
                throw new ArgumentOutOfRangeException("n", "n must be 0 or greater");

            LinkedList<T> temp = new LinkedList<T>();

            foreach (var value in collection)
            {
                temp.AddLast(value);
                if (temp.Count > n)
                    temp.RemoveFirst();
            }

            return temp;
        }

        /// <summary>
        /// Run an action on each of the items in a collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">The collection to enumerate</param>
        /// <param name="action">The action to run on each element</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }

        /// <summary>
        /// Run an action on each of the items in a collection
        /// </summary>
        /// <typeparam name="T">The type of the items in the collection</typeparam>
        /// <param name="collection">The collection to enumerate</param>
        /// <param name="action">The action to run on each element with the index of the element passed in</param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            for (int i = 0; i < collection.Count(); i++)
            {
                action(collection.ElementAt(i), i);
            }
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
        /// <![CDATA[From: http://code.google.com/p/morelinq/source/browse/MoreLinq/MinBy.cs]]>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>
        /// or <paramref name="comparer"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
        /// <![CDATA[From: http://code.google.com/p/morelinq/source/browse/MoreLinq/MinBy.cs]]>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            if (comparer == null) throw new ArgumentNullException("comparer");
            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        /// <summary>
        /// Returns the maximum element of the given sequence, based on
        /// the given projection.
        /// </summary>
        /// <remarks>
        /// If more than one element has the maximum projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current maximum element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The maximum element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns the maximum element of the given sequence, based on
        /// the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        /// If more than one element has the maximum projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current maximum element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The maximum element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/>
        /// or <paramref name="comparer"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            if (comparer == null) throw new ArgumentNullException("comparer");
            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) > 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        /// <summary>
        /// Returns the most frequently seen element in the collection
        /// </summary>
        /// <typeparam name="TSource">The type of the collection</typeparam>
        /// <param name="source">The collection to search</param>
        /// <returns>The most frequently seen element in the collection</returns>
        public static TSource MostFrequent<TSource>(this IEnumerable<TSource> source)
        {
            Dictionary<TSource, int> counts = new Dictionary<TSource, int>();
            foreach (TSource a in source)
            {
                if (counts.ContainsKey(a))
                {
                    counts[a] = counts[a] + 1;
                }
                else
                {
                    counts.Add(a, 1);
                }
            }

            TSource result = default(TSource);
            int max = int.MinValue;
            foreach (TSource key in counts.Keys)
            {
                if (counts[key] > max)
                {
                    max = counts[key];
                    result = key;
                }
            }
            return result;
        }

        /// <summary>
        /// Select the top N elements from a list using the itemLength function to return the "score" for the item
        /// </summary>
        /// <typeparam name="TSource">The type of parameter</typeparam>
        /// <param name="source">The source array to enumerate</param>
        /// <param name="count">The number of items to return</param>
        /// <param name="itemLength">A function that takes an item as input and returns the score (size, length, etc) of the object</param>
        /// <returns>The top N elements from a list</returns>
        public static TSource[] TopN<TSource>(this IEnumerable<TSource> source, int count, Func<TSource, int> itemLength)
        {
            //Store the items
            TSource[] items = new TSource[count];
            //Store the items lengths
            int?[] lengths = new int?[count];

            //Loop through each item
            foreach (TSource item in source)
            {
                //Compute the length of the item
                int length = itemLength(item);

                //Store previous 
                TSource prev = default(TSource);
                int prevLength = -1;
                bool added = false;
                //Loop through each item we have stored
                for (int i = 0; i < items.Length; i++)
                {
                    //If we havn't added an item
                    if (!added)
                    {
                        //If we havn't added an item at this index yet
                        if (lengths[i] == null)
                        {
                            //Just add it and move on
                            items[i] = item;
                            lengths[i] = length;
                            break;
                        }
                        else if (lengths[i] < length)
                        {
                            //Else add it and set the prev vars to the existing values to shift them down
                            prev = items[i];
                            prevLength = lengths[i].Value;
                            items[i] = item;
                            lengths[i] = length;
                            added = true;
                        }
                    }
                    else
                    {
                        //Else if we are shifting values
                        //If there is no value here currently
                        if (lengths[i] == null)
                        {
                            //Just add it and move on
                            lengths[i] = prevLength;
                            items[i] = prev;
                            break;
                        }
                        else
                        {
                            //Otherwise we need to keep shifting
                            TSource tmp = items[i];
                            int tmpLen = lengths[i].Value;
                            lengths[i] = prevLength;
                            items[i] = prev;
                            prev = tmp;
                            prevLength = tmpLen;
                        }
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Get the first element of the sequence
        /// </summary>
        /// <typeparam name="T">The type of the sequence</typeparam>
        /// <param name="source">The sequence to get the first element from</param>
        /// <returns>The first element in the sequence</returns>
        public static T FirstElement<T>(this IEnumerable source)
        {
            return (T)FirstElement(source);
        }
        /// <summary>
        /// Get the first element of the sequence
        /// </summary>
        /// <typeparam name="T">The type of the sequence</typeparam>
        /// <param name="source">The sequence to get the first element from</param>
        /// <returns>The first element in the sequence</returns>
        public static object FirstElement(this IEnumerable source)
        {
            if (source == null)
            {
                return new ArgumentNullException("source");
            }
            IEnumerator v = source.GetEnumerator();
            if (v.MoveNext())
            {
                return v.Current;
            }
            throw new InvalidOperationException("The source sequence is empty.");
        }
    }
}
