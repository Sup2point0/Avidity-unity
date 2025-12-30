#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;


namespace Avidity
{
    public static partial class Utils
    {
        /// <summary> Batch the items of an iterable into chunks of `chunk_size`. </summary>
        /// <remarks>
        /// If a last chunk of exactly `chunk_size` cannot be produced, the array will have fewer items (instead of having <c>null</c> or default values).
        /// </remarks>
        public static IEnumerable<T[]> Chunked<T>(this IEnumerable<T> iterable, int chunk_size)
        {
            var length = iterable.Count();

            var i = 0;
            var idx = 0;
            var res = new T[Math.Min(chunk_size, length - i)];

            foreach (var item in iterable) {
                i++;
                res[idx] = item;
                idx++;

                if (idx == chunk_size) {
                    yield return res;
                    idx = 0;
                    res = new T[Math.Min(chunk_size, length - i)];
                }
            }

            if (res.Length > 0) yield return res;
        }

        public static Dictionary<K,V> AddMaybe<K,V>(Dictionary<K,V> dict, K key, V? value)
        {
            if (value is not null) {
                dict.Add(key, value);
            }

            return dict;
        }
    }
}
