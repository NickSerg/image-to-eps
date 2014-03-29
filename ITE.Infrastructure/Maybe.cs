using System;
using System.Collections.Generic;

namespace ITE.Infrastructure
{
    public static class Maybe
    {
        public static TResult With<TInput, TResult>(this TInput obj, Func<TInput, TResult> evaluator)
            where TInput : class
            where TResult : class
        {
            return obj == null ? null : evaluator(obj);
        }

        public static TResult Return<TInput, TResult>(this TInput obj, Func<TInput, TResult> evaluator, TResult failureValue)
            where TInput : class
        {
            return obj == null ? failureValue : evaluator(obj);
        }

        public static bool ReturnSuccess<TInput>(this TInput obj) where TInput : class
        {
            return obj != null;
        }

        public static TInput If<TInput>(this TInput obj, Predicate<TInput> predicate) where TInput : class
        {
            if (obj == null)
                return null;

            return predicate(obj) ? obj : null;
        }

        public static TInput Do<TInput>(this TInput obj, Action<TInput> action) where TInput : class
        {
            if (obj == null)
                return null;

            action(obj);

            return obj;
        }

        public static bool IsNullOrEmpty<TInput>(this ICollection<TInput> obj)
        {
            return obj == null || obj.Count == 0;
        }
    }
}
