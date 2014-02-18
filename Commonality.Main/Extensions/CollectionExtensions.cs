using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Commonality.Main.Extensions
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Sorts the enumerable sequence randomly.
        /// </summary>
        /// <typeparam name="T">The type of enumerable.</typeparam>
        /// <param name="input">The enumerable sequence.</param>
        /// <returns>A randomly sorted enumerable.</returns>
        [DebuggerStepThrough]
        public static IEnumerable<T> OrderByRandom<T>(this IEnumerable<T> input)
        {
            var sortOrder = new Random();
            return input.Select(i => new { Sort = sortOrder.Next(), Item = i }).OrderBy(i => i.Sort).Select(i => i.Item);
        }

        /// <summary>
        /// Implements IsNullOrEmpty for an <see cref="IEnumerable"/>
        /// </summary>
        /// <param name="array">
        /// The array to test
        /// </param>
        /// <returns>
        /// If the enumerable sequence is null or empty.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> array)
        {
            return array == null || !array.Any();
        }

        /// <summary>
        /// Implements IsNullOrEmpty for an array
        /// </summary>
        /// <param name="array">
        /// The array to test
        /// </param>
        /// <returns>
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        [DebuggerStepThrough]
        public static IDictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            var newCollection = new Dictionary<string, string>();

            collection.Count.Times(
                x => newCollection.Add(collection.GetKey(x) ?? string.Empty, collection.Get(x) ?? string.Empty));

            return newCollection;
        }

        [DebuggerStepThrough]
        public static ICollection<T> FluentAdd<T>(this ICollection<T> list, T valueToAdd)
        {
            list.Add(valueToAdd);
            return list;
        }

        /// <summary>
        /// Functional foreach loop.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="func"></param>
        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> func)
        {
            foreach (var item in collection ?? Enumerable.Empty<T>())
            {
                func(item);
            }
        }

        [DebuggerStepThrough]
        public static IEnumerable<T> ForAll<T>(this IEnumerable<T> collection, Action<T> func)
        {
            foreach (var item in collection ?? Enumerable.Empty<T>())
            {
                func(item);
            }

            return collection;
        }

        [DebuggerStepThrough]
        public static void ParallelForEach<T>(this IEnumerable<T> collection, Action<T> func)
        {
            Parallel.ForEach(collection, func);
        }

        [DebuggerStepThrough]
        public static bool IsEmpty<T>(this IEnumerable<T> input)
        {
            return !input.Any();
        }

        /// <summary>
        /// Breaks an IEnumerable down into smaller batches
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> input, int batchSize)
        {
            if (input == null)
                input = Enumerable.Empty<T>();

            var temp = new List<T>(batchSize);
            foreach (var item in input)
            {
                temp.Add(item);

                if (temp.Count == batchSize)
                {
                    yield return temp;
                    temp = new List<T>(batchSize);
                }
            }
            if (temp.Count > 0)
                yield return temp;
        }

        [DebuggerStepThrough]
        public static IList<TInput> AddAll<TInput>(this IList<TInput> input, IEnumerable<TInput> toAdd)
        {
            toAdd.ForEach(input.Add);
            return input;
        }
    }
}