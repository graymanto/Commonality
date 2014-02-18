using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Commonality.Main.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> OrderByRandom<T>(this IEnumerable<T> input)
        {
            var sortOrder = new Random();
            return input.Select(i => new { Sort = sortOrder.Next(), Item = i })
                .OrderBy(i => i.Sort)
                .Select(i => i.Item);
        }

        /// <summary>
        /// Implements IsNullOrEmpty for an <see cref="IEnumerable"/>
        /// </summary>
        /// <param name="array">The array to test</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> array)
        {
            return array == null || !array.Any();
        }

        /// <summary>
        /// Implements IsNullOrEmpty for an array
        /// </summary>
        /// <param name="array">The array to test</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        public static IDictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            var newCollection = new Dictionary<string, string>();

            collection.Count.Times(x => newCollection.Add(collection.GetKey(x) ?? string.Empty, collection.Get(x) ?? string.Empty));

            return newCollection;
        }

        public static ICollection<T> FluentAdd<T>(this ICollection<T> list, T valueToAdd)
        {
            list.Add(valueToAdd);
            return list;
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> func)
        {
            foreach (var item in collection ?? Enumerable.Empty<T>())
            {
                func(item);
            }
        }

        public static IEnumerable<T> FluentForEach<T>(this IEnumerable<T> collection, Action<T> func)
        {
            foreach (var item in collection ?? Enumerable.Empty<T>())
            {
                func(item);
            }
            return collection;
        }

        public static void ParallelForEach<T>(this IEnumerable<T> collection, Action<T> func)
        {
            Parallel.ForEach(collection, func);
        }

        public static bool IsEmpty<T>(this IEnumerable<T> input)
        {
            return !input.Any();
        }
    }
}
