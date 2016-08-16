using System;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Interapptive.Shared.Messaging.TrackedObservable
{
    /// <summary>
    /// Logging implementation of Select methods
    /// </summary>
    public static class SelectImpl
    {
        /// <summary>
        /// Select a new value from an existing one
        /// </summary>
        public static IObservable<IMessageTracker<TReturn>> Select<T, TReturn>(this IObservable<IMessageTracker<T>> source,
            object listener, Func<T, TReturn> func, [CallerMemberName] string callerName = "")
        {
            return source.Select(x => x.Select(func, listener, callerName));
        }
    }
}
