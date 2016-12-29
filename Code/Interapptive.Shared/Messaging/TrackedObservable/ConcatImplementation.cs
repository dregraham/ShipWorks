using System;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Interapptive.Shared.Messaging.TrackedObservable
{
    /// <summary>
    /// Implementation of the trackable concat statement
    /// </summary>
    public static class ConcatImplementation
    {
        /// <summary>
        /// Implementation of the trackable concat statement
        /// </summary>
        public static IObservable<IMessageTracker<TSource>> Concat<TSource>(this IObservable<IMessageTracker<IObservable<IMessageTracker<TSource>>>> sources,
            object listener, [CallerMemberName] string callerName = "")
        {
            return sources.Select(x => x.Value).Concat();
        }
    }
}
