using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monads.NET
{
    public static class Maybe
    {

        /// <summary>                
        /// Will take in an object and if it's not null run a Boolean evaluation on the object if the result is true returns the object if false returns null.
        /// </summary>
        /// <typeparam name="TInput">Reference type of object being extended</typeparam>
        /// <param name="o">Object being extended</param>
        /// <param name="evaluator">Evaluation Function</param>
        /// <returns>Same instance of object that was passed in if true other wise null</returns>
        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            return (o == null) ? null : evaluator(o) ? o : null;
        }

        /// <summary>
        /// Will take in an object and if it is not null run a Boolean evaluation on the object if the result is false returns the object if true returns null
        /// </summary>
        /// <typeparam name="TInput">Reference type of object being extended</typeparam>
        /// <param name="o">Object being extended</param>
        /// <param name="evaluator">Evaluation Function</param>
        /// <returns>Same instance of object that was passed in if false other wise null</returns>
        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
               where TInput : class
        {
            return (o == null) ? null : evaluator(o) ? null : o;
        }

        /// <summary>
        /// Will take in an object and if it is not null run a Boolean evaluation on the object if the result is true it will run an action on the object and returns original object
        /// </summary>
        /// <typeparam name="TInput">Reference type of object being extended</typeparam>
        /// <param name="o">Object being extended</param>
        /// <param name="evaluator">Evaluation Function</param>
        /// <param name="action">function that will act on the object</param>
        /// <returns>Same instance of object that was passed in</returns>
        public static TInput IfDo<TInput>(this TInput o, Func<TInput, bool> evaluator, Action<TInput> action)
            where TInput : class
        {
            if (o == null) return null;

            if (evaluator(o))
            {
                action(o);
            }

            return o;
        }

        /// <summary>
        /// Allows for a null safe accessing of an item
        /// </summary>
        /// <typeparam name="TInput">Reference type of object being extended</typeparam>
        /// <typeparam name="TResult">Reference type of object being returned</typeparam>
        /// <param name="o">instance of object being extended</param>
        /// <param name="evaluator">function that acts on object to return a result</param>
        /// <returns>null if object is null or an instance of TResult</returns>
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            return (o == null) ? null : evaluator(o);
        }

        /// <summary>
        /// Allows for a null safe accessing of items
        /// </summary>
        /// <typeparam name="TInput">Reference type of object being extended</typeparam>
        /// <typeparam name="TResult">Reference type of object being returned</typeparam>
        /// <param name="o">instance of collections of objects being extended</param>
        /// <param name="evaluator">function that acts on object to return a result</param>
        /// <returns>null if object is null or an instance of TResult</returns>
        public static IEnumerable<TResult> With<TInput, TResult>(this IEnumerable<TInput> o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null) return null;

            return o.Select(e => evaluator(e));
        }

        /// <summary>
        /// Allows for null safe and no key accessing of an item
        /// </summary>
        /// <typeparam name="TKey">Reference type of key used to access an item in a dictionary</typeparam>
        /// <typeparam name="TResult">Reference type of object stored in dictionary</typeparam>
        /// <param name="o">The Dictionary that you will be accessing</param>
        /// <param name="key">The key you will be using to access the dictionary.</param>
        /// <returns>if the key exists in the dictionary the item if the dictionary is null or doesn't have the key null</returns>
        public static TResult With<TKey, TResult>(this IDictionary<TKey, TResult> o, TKey key)
            where TResult : class
        {
            TResult value;
            return (o == null || !o.TryGetValue(key, out value)) ? null : value;
        }

        /// <summary>
        /// Allows for a null safe accessing of an item
        /// </summary>
        /// <typeparam name="TInput">Reference type of object being extended</typeparam>
        /// <typeparam name="TResult">Reference type of object being returned</typeparam>
        /// <param name="o">instance of object being extended</param>
        /// <param name="evaluator">function that acts on object to return a result</param>
        /// <returns>null if object is null or an instance of TResult</returns>
        public static Task<TResult> WithAsync<TInput, TResult>(this TInput o, Func<TInput, Task<TResult>> evaluator)
            where TResult : class
            where TInput : class
        {
            return (o == null) ? TaskEx.FromResult<TResult>(null) : evaluator(o);
        }
        /// <summary>
        /// Allows for a null safe accessing of an item
        /// </summary>
        /// <typeparam name="TInput">Reference type of object being extended</typeparam>
        /// <typeparam name="TResult">Reference type of object being returned</typeparam>
        /// <param name="o">instance of object being extended</param>
        /// <param name="evaluator">function that acts on object to return a result</param>
        /// <returns>null if object is null or an instance of TResult</returns>
        public static Task WithAsync<TInput>(this TInput o, Func<TInput, Task> evaluator)
            where TInput : class
        {
            return (o == null) ? TaskEx.FromResult<object>(null) : evaluator(o);
        }


        /// <summary>
        /// Allows for a null safe accessing of items
        /// </summary>
        /// <typeparam name="TInput">Reference type of object being extended</typeparam>
        /// <typeparam name="TResult">Reference type of object being returned</typeparam>
        /// <param name="o">instance of collections of objects being extended</param>
        /// <param name="evaluator">function that acts on object to return a result</param>
        /// <returns>null if object is null or an instance of TResult</returns>
        public async static Task<IEnumerable<TResult>> WithAsync<TInput, TResult>(this IEnumerable<TInput> o, Func<TInput, Task<TResult>> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null) return null;
            
            return await TaskEx.WhenAll(o.Select(async e => await evaluator(e)));
        }


        /// <summary>
        ///  Used for null safe accessing of an item with a default value in the case of a null object
        /// </summary>
        /// <typeparam name="TInput">Type of instance to be acted on</typeparam>
        /// <typeparam name="TResult">Type of value to be returned</typeparam>
        /// <param name="o">Object to be extended</param>
        /// <param name="evaluator">Function to act on the object</param>
        /// <param name="failureValue">Default value to return if o is null</param>
        /// <returns></returns>
        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failureValue) where TInput : class
        {
            return (o == null) ? failureValue : evaluator(o);
        }

        /// <summary>
        /// Used for null safe accessing of a dictionary where you want a default value returned when dictionary is null or key doesn't exist.
        /// </summary>
        /// <typeparam name="TKey">type of key used in dictionary</typeparam>
        /// <typeparam name="TResult">type of item to be used as failureValue and stored in dictionary </typeparam>
        /// <param name="o">Dictionary to be acted on by function</param>
        /// <param name="key">Value to be looked up in dictionary</param>
        /// <param name="failureValue">return value if no key is found in dictionary or dictionary is null</param>
        /// <returns>failureValue if dictionary is null or key is not found otherwise the value as stored in the dictionary for that key.</returns>             
        public static TResult Return<TKey, TResult>(this IDictionary<TKey, TResult> o, TKey key, TResult failureValue)
        {
            return (o == null || !o.ContainsKey(key)) ? failureValue : o[key];
        }

        /// <summary>
        /// Allows for a null safe action on a object.
        /// </summary>
        /// <typeparam name="TInput">Instance of object to be acted on</typeparam>
        /// <param name="o">object to be extended</param>
        /// <param name="action">function that will act on the object</param>
        /// <returns>object that was acted on</returns>
        public static TInput Do<TInput>(this TInput o, Action<TInput> action) where TInput : class
        {
            if (o == null) return null;
            action(o);
            return o;
        }

        /// <summary>
        /// Allows for a null safe action on a IEnumerable of objects.
        /// </summary>
        /// <typeparam name="TInput">Instance of object to be acted on</typeparam>
        /// <param name="o">collection to be extended</param>
        /// <param name="action">function that will act on each object</param>
        /// <returns>object that was acted on</returns>
        public static IEnumerable<TInput> Do<TInput>(this IEnumerable<TInput> o, Action<TInput> action) where TInput : class
        {
            if (o == null) return null;

            foreach (var c in o)
            {
                action(c);
            }
            return o;
        }

        /// <summary>
        /// Allows for a function to run when an object is null.   
        /// </summary>
        /// <typeparam name="TInput">type of object to extend</typeparam>
        /// <param name="o">object to be extended</param>
        /// <param name="evaluator">function that will run if o is null</param>
        /// <returns></returns>
        public static TInput Recover<TInput>(this TInput o, Func<TInput> evaluator)
            where TInput : class
        {
            return o ?? evaluator();
        }

        /// <summary>
        /// Allows for a an alternative value to be used when an object is null.   
        /// </summary>
        /// <typeparam name="TInput">type of object to extend</typeparam>
        /// <param name="o">object to be extended</param>
        /// <param name="recover">value that will be used if o is null</param>
        /// <returns></returns>
        public static TInput Recover<TInput>(this TInput o, TInput recover)
            where TInput : class
        {
            return o ?? recover;
        }

        /// <summary>
        /// Allows for a function to run on an object when it is not null otherwise returns a default value.
        /// </summary>
        /// <typeparam name="TInput">type of object to be extended</typeparam>
        /// <typeparam name="TResult">type of object to be returned</typeparam>
        /// <param name="o">object to be extended</param>
        /// <param name="evaluator">function to act on the object</param>
        /// <param name="failValue">value to return if o is null</param>
        /// <returns>fialvalue if o is null otherwise the value returned by byt the evaluator</returns>
        public static TResult Let<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failValue)
        {
            return (o == null) ? failValue : evaluator(o);
        }

        /// <summary>
        /// Exception safe version of do.
        /// </summary>
        /// <typeparam name="TInput">type of object to be extended</typeparam>
        /// <param name="o">object to be extended</param>
        /// <param name="evaluator">function to act on object</param>
        /// <returns>
        /// There are three possible returns a Tuple[TInput, Exception](null, null) happens when o is null
        /// Tuple[TInput, Exception](o, null) happens when the evaluator executes with no error.
        /// Tuple[TInput, Exception](o, exception) happens when the evaluator throws an exception.
        /// </returns>
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

        /// <summary>
        /// Exception safe version of Let.
        /// </summary>
        /// <typeparam name="TInput">type of object to be extended</typeparam>
        /// <typeparam name="TResult">type of object of be returned in Tuple</typeparam>
        /// <param name="o">object to be extended</param>
        /// <param name="evaluator">function to act on o</param>
        /// <returns>
        /// There are three possible returns a Tuple[TResult, Exception](null, null) happens when o is null
        /// Tuple[TResult, Exception](TResult, null) happens when the evaluator executes with no error.
        /// Tuple[TResult, Exception](null, exception) happens when the evaluator throws an exception.
        /// </returns>
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


        /// <summary>
        /// function used with TryDo to handle the exceptions that are possibly retuned
        /// </summary>
        /// <typeparam name="TInput">Type of item to be returned form function</typeparam>
        /// <param name="input">Tuple to be extended by function</param>
        /// <param name="log">function to run if there is an exception</param>
        /// <returns>TInput form the Tuple</returns>
        public static TInput Handle<TInput>(this Tuple<TInput, Exception> input, Action<TInput, Exception> log)
            where TInput : class
        {
            if (input.Item2 != null)
                log(input.Item1, input.Item2);

            return input.Item1;
        }

        /// <summary>
        /// function used with TryLet and TryDo to handle the exceptions that are possibly retuned
        /// </summary>
        /// <typeparam name="TInput">Type of item to be returned form function</typeparam>
        /// <param name="input">Tuple to be extended by function</param>
        /// <param name="log">function to run if there is an exception</param>
        /// <returns>TInput form the Tuple</returns>
        public static TInput Handle<TInput>(this Tuple<TInput, Exception> input, Action<Exception> log)
            where TInput : class
        {
            if (input.Item2 != null)
                log(input.Item2);

            return input.Item1;
        }

        /// <summary>
        /// function used with TryLet and TryDo to handle the exceptions that are possibly retuned
        /// </summary>
        /// <typeparam name="TInput">type of Item to be returned by function</typeparam>
        /// <param name="input">tuple to be extended by function</param>
        /// <returns>TInput from tuple </returns>
        public static TInput Ignore<TInput>(this Tuple<TInput, Exception> input)
            where TInput : class
        {
            return input.Item1;
        }
    }
}
