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

        public static TInput Recover<TInput>(this TInput o, Func<TInput> evaluator)
            where TInput : class
        {
            return o ?? evaluator();
        }

        public static TResult Let<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failValue)
        {
            return (o == null) ? failValue : evaluator(o);
        }

        public static Tuple<TInput, Exception> TryDo<TInput>(this TInput o, Action<TInput> evaluator)
                where TInput : class
        {
            try
            {
                if (o == null)
                    return Tuple.Create<TInput, Exception>(null, null);

                evaluator(o);

                return Tuple.Create<TInput, Exception>(o, null);
            }
            catch (Exception ex)
            {
                return Tuple.Create<TInput, Exception>(o, ex);
            }
        }

        public static Tuple<TResult, Exception> TryLet<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TInput : class
            where TResult : class
        {
            try
            {
                if (o == null)
                    return Tuple.Create<TResult, Exception>(null, null);

                return Tuple.Create<TResult, Exception>(evaluator(o), null);
            }
            catch (Exception ex)
            {
                return Tuple.Create<TResult, Exception>(null, ex);
            }
        }

        public static TInput Handle<TInput>(this Tuple<TInput, Exception> input, Action<Exception> log)
            where TInput : class
        {
            if (input.Item2 != null)
                log(input.Item2);

            return input.Item1;
        }

        public static TInput Ignore<TInput>(this Tuple<TInput, Exception> input)
            where TInput : class
        {
            return input.Item1;
        }
    }
}
