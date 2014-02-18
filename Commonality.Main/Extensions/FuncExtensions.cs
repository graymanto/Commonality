using System;
using System.Collections.Concurrent;

using Commonality.Main.Contracts;

namespace Commonality.Main.Extensions
{
    public static class FuncExtensions
    {
        public static Func<T1, TResult> Memoize<T1, TResult>(this Func<T1, TResult> instance)
        {
            Require.ArgumentNotNull(() => instance);

            var cache = new ConcurrentDictionary<Tuple<T1>, TResult>();

            return t1 =>
                {
                    var key = Tuple.Create(t1);

                    return cache.GetOrAdd(key, x => instance(t1));
                };
        }
    }
}