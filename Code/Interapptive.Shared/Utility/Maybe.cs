using System;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Create a result of an operation
    /// </summary>
    public struct Maybe
    {
        /// <summary>
        /// Get a Maybe with a value
        /// </summary>
        public static Maybe<T> Value<T>(T value) =>
            new Maybe<T>(value);

        /// <summary>
        /// Get a Maybe without a value
        /// </summary>
        public static Maybe<T> Empty<T>() => Maybe<T>.Empty;
    }

    /// <summary>
    /// Generic Class that can be used to return an object or message
    /// </summary>
    /// <remarks>
    /// Used to return an object or message
    /// </remarks>
    public struct Maybe<T>
    {
        private readonly static Maybe<T> empty = new Maybe<T>();
        private readonly T value;
        private readonly bool hasValue;

        /// <summary>
        /// Constructor
        /// </summary>
        internal Maybe(T value)
        {
            this.value = value;
            this.hasValue = true;
        }

        /// <summary>
        /// Empty Maybe
        /// </summary>
        internal static Maybe<T> Empty => empty;

        /// <summary>
        /// Bind the value of the result
        /// </summary>
        /// <param name="func">Func that will be bound with the value of the initial result</param>
        /// <returns>
        /// A result containing the mapped value or its error, or the original error
        /// </returns>
        public Maybe<TResult> Bind<TResult>(Func<T, Maybe<TResult>> map) =>
            Match(map, () => Maybe<TResult>.Empty);

        /// <summary>
        /// Filter results by the given predicate
        /// </summary>
        /// <returns>Value if true, otherwise an error result</returns>
        public Maybe<T> Filter(Func<T, bool> predicate) =>
            Bind(x => predicate(x) ? x : Maybe<T>.Empty);

        /// <summary>
        /// Map the value of the result
        /// </summary>
        /// <param name="func">Map the value to another value</param>
        /// <returns>
        /// A result containing the mapped value, or the original error
        /// </returns>
        public Maybe<TResult> Map<TResult>(Func<T, TResult> map) =>
            Match(x => Maybe.Value(map(x)),
                () => Maybe<TResult>.Empty);

        /// <summary>
        /// Perform an operation on the value
        /// </summary>
        /// <param name="action">Action to perform on the value</param>
        /// <returns>
        /// A result containing the original value, or the original error
        /// </returns>
        public Maybe<T> Do(Action<T> map) =>
            Match(x => { map(x); return x; },
                () => Maybe<T>.Empty);

        /// <summary>
        /// Provide a value when there is none
        /// </summary>
        public T OrElse(Func<T> onEmpty) =>
            Match(x => x, onEmpty);

        /// <summary>
        /// Match on the result, calling the first method on success and the second on failure
        /// </summary>
        /// <param name="onSuccess">Map the successful value</param>
        /// <param name="onError">Map the exception value</param>
        /// <returns>
        /// The result of the called function
        /// </returns>
        public TResult Match<TResult>(Func<T, TResult> onValue, Func<TResult> onEmpty) =>
            hasValue ?
                onValue(value) :
                onEmpty();

        /// <summary>
        /// Convert from a value of the result type to a successful result
        /// </summary>
        public static implicit operator Maybe<T>(T value) =>
            Maybe.Value(value);
    }
}
