using System;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Interapptive.Shared.Messaging.TrackedObservable
{
    /// <summary>
    /// Implementation of Where extension methods
    /// </summary>
    public static class WhereImpl
    {
        /// <summary>
        /// Where method
        /// </summary>
        public static IObservable<IMessageTracker<T>> Where<T>(this IObservable<IMessageTracker<T>> source,
            object listener, Func<T, bool> func, [CallerMemberName] string callerName = "")
        {
            return Observable.Where(source, x => x.Where(func, listener, callerName));
        }
    }
}
