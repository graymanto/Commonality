using System;

namespace Commonality.Main.Extensions.Functional
{
    public static class CurryingExtensions
    {
        public static Func<TArg1, Func<TArg2, TResult>> Curry<TArg1, TArg2, TResult>(
            this Func<TArg1, TArg2, TResult> func)
        {
            return a1 => a2 => func(a1, a2);
        }

        public static Func<TArg1, Action<TArg2>> Curry<TArg1, TArg2>(this Action<TArg1, TArg2> action)
        {
            return a1 => a2 => action(a1, a2);
        }
    }
}