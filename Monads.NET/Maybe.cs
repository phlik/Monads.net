using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monads.NET
{
    public static class Maybe
    {
        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class 
        {
            return (o == null) ? null : evaluator(o) ? o : null;
        }

        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
               where TInput : class 
        {
            return (o == null) ? null : evaluator(o) ? null : o;
        }

        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class 
        {
            return (o == null) ? null : evaluator(o);
        }

        public static TResult With<TKey, TResult>(this Dictionary<TKey, TResult> o, TKey key)
            where TResult : class 
        {
            return (o == null || !o.ContainsKey(key)) ? null : o[key];
        }

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failureValue) where TInput : class 
        {
            return (o == null) ? failureValue : evaluator(o);
        }

        public static TResult Return<TKey, TResult>(this Dictionary<TKey, TResult> o, TKey key, TResult failureValue) 
        {
            return (o == null || !o.ContainsKey(key)) ? failureValue : o[key];
        }

        public static TInput Do<TInput>(this TInput o, Action<TInput> action) where TInput : class 
        {
            if (o == null) return null;
            action(o);
            return o;
        }
    }
}
