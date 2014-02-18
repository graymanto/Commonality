using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Commonality.Main.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Gets the value from the dictionary associated with the given key or returns a default value.
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        /// <typeparam name="TU">
        /// </typeparam>
        /// <param name="dic">
        /// </param>
        /// <param name="key">
        /// </param>
        /// <returns>
        /// </returns>
        [DebuggerStepThrough]
        public static TU GetOrDefault<T, TU>(this IDictionary<T, TU> dic, T key)
        {
            TU possibleValue;
            return dic.TryGetValue(key, out possibleValue) ? possibleValue : default(TU);
        }

        /// <summary>
        /// Gets the value associated with the given key or invokes the func to add the value.
        /// </summary>
        /// <typeparam name="TKey">
        /// </typeparam>
        /// <typeparam name="TValue">
        /// </typeparam>
        /// <param name="input">
        /// The dictionary to search.
        /// </param>
        /// <param name="key">
        /// The key to search for.
        /// </param>
        /// <param name="valueFactory">
        /// </param>
        /// <returns>
        /// The added or already present value.
        /// </returns>
        [DebuggerStepThrough]
        public static TValue GetOrAdd<TKey, TValue>(
            this Dictionary<TKey, TValue> input, 
            TKey key, 
            Func<TKey, TValue> valueFactory)
        {
            TValue possibleValue;
            if (input.TryGetValue(key, out possibleValue))
            {
                return possibleValue;
            }

            possibleValue = valueFactory(key);
            return input[key] = possibleValue;
        }

        /// <summary>
        /// Gets the value associated with the given key or adds the given value.
        /// </summary>
        /// <typeparam name="TKey">
        /// </typeparam>
        /// <typeparam name="TValue">
        /// </typeparam>
        /// <param name="input">
        /// The dictionary to search.
        /// </param>
        /// <param name="key">
        /// The key to search for.
        /// </param>
        /// <param name="theValue">
        /// The value to add.
        /// </param>
        /// <returns>
        /// The added or already present value.
        /// </returns>
        [DebuggerStepThrough]
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> input, TKey key, TValue theValue)
        {
            TValue possibleValue;
            if (input.TryGetValue(key, out possibleValue))
            {
                return possibleValue;
            }

            return input[key] = theValue;
        }
    }
}