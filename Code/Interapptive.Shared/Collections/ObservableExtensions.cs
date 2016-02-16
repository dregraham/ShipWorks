using System;
using System.Reactive.Linq;

namespace Interapptive.Shared.Collections
{
    /// <summary>
    /// Extensions on the Observable type
    /// </summary>
    public static class ObservableExtensions
    {
        /// <summary>
        /// Catch a specified exception, log it, and continue
        /// </summary>
        public static IObservable<T> CatchAndContinue<T, TException>(this IObservable<T> source,
            Action<TException> handleException) where TException : Exception
        {
            return source.Catch<T, TException>(ex =>
            {
                handleException(ex);
                return source.CatchAndContinue(handleException);
            });
        }
    }
}
