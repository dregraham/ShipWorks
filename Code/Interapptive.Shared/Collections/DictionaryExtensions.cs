using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Extension methods on various dictionary classes
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Update the value of the given key, using the default if it doesn't already exist
        /// </summary>
        public static IImmutableDictionary<TKey, TValue> UpdateValue<TKey, TValue>(this IImmutableDictionary<TKey, TValue> source,
                TKey key, Func<TValue> defaultValue, Func<TValue, TValue> map) =>
            source.ContainsKey(key) ?
                source.SetItem(key, map(source[key])) :
                source.Add(key, map(defaultValue()));

        /// <summary>
        /// Update the value of the given key, or add it to the dictionary
        /// </summary>
        public static void UpdateValue<TKey, TValue>(this IDictionary<TKey, TValue> source,
            TKey key, TValue value)
        {
            if (source.ContainsKey(key))
            {
                source[key] = value;
            }
            else
            {
                source.Add(key, value);
            }
        }
    }
}
