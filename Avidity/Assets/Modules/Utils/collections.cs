#nullable enable

using System.Collections.Generic;


namespace Avidity
{
    public static partial class Utils
    {
        public static Dictionary<K,V> AddMaybe<K,V>(Dictionary<K,V> dict, K key, V? value)
        {
            if (value is not null) {
                dict.Add(key, value);
            }

            return dict;
        }
    }
}
