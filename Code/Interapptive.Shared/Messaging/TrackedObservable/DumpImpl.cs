using System;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Interapptive.Shared.Messaging.TrackedObservable
{
    /// <summary>
    /// Implementation of the dump operation
    /// </summary>
    public static class DumpImpl
    {
        /// <summary>
        /// Dumps the current value to the log
        /// </summary>
        public static IObservable<IMessageTracker<TSource>> Dump<TSource>(this IObservable<IMessageTracker<TSource>> source,
            object listener, [CallerMemberName] string callerName = "")
        {
            return source.Do(x => x.Dump(listener, callerName));
        }
    }
}
