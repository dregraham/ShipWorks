using System;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Interapptive.Shared.Messaging.TrackedObservable
{
    /// <summary>
    /// Logging implementation of SelectMany methods
    /// </summary>
    public static class SelectManyImpl
    {
        /// <summary>
        /// Select new values from existing collections
        /// </summary>
        public static IObservable<IMessageTracker<TReturn>> SelectMany<T, TReturn>(this IObservable<IMessageTracker<T>> source,
            object listener, Func<T, IObservable<TReturn>> func, [CallerMemberName] string callerName = "")
        {
            return source.SelectMany(x => x.SelectMany(func, listener, callerName));
        }
    }
}
