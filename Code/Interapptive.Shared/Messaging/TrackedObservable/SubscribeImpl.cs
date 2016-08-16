using System;
using System.Runtime.CompilerServices;

namespace Interapptive.Shared.Messaging.TrackedObservable
{
    /// <summary>
    /// Logging implementation of Subscribe methods
    /// </summary>
    public static class SubscribeImpl
    {
        /// <summary>
        /// Subscribe to an observable stream
        /// </summary>
        public static IDisposable Subscribe<T>(this IObservable<IMessageTracker<T>> source, object listener,
            Action<T> onNext, [CallerMemberName] string callerName = "")
        {
            return source.Subscribe(x => x.Subscribe(onNext, listener, callerName));
        }
    }
}
