using System;

namespace Commonality.Main.Extensions.Functional
{
    public static class MonadicObjectExtensions
    {
        public static TResult Get<TInput, TResult>(this TInput input, Func<TInput, TResult> evaluator)
            where TResult : class where TInput : class
        {
            if (input == null)
            {
                return null;
            }

            return evaluator(input);
        }

        public static TResult CastTo<TResult>(this object input) where TResult : class
        {
            if (input == null)
            {
                return null;
            }

            return input as TResult;
        }

        public static TInput Do<TInput>(this TInput input, Action<TInput> action) where TInput : class
        {
            if (input == null)
            {
                return null;
            }

            action(input);
            return input;
        }

        public static TResult Return<TInput, TResult>(
            this TInput input, 
            Func<TInput, TResult> evaluator, 
            TResult failureValue) where TInput : class
        {
            if (input == null)
            {
                return failureValue;
            }

            return evaluator(input);
        }
    }
}