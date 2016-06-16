using System;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Interapptive.Shared.Messaging.TrackedObservable
{
    public static class SwitchImpl
    {
        public static IObservable<IMessageTracker<TSource>> Switch<TSource>(this IObservable<IMessageTracker<IObservable<IMessageTracker<TSource>>>> sources,
            object listener, [CallerMemberName] string callerName = "")
        {
            return sources.Select(x => x.Value).Switch();
        }
    }
}
