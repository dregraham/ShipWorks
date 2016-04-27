using System;
using System.Reactive.Linq;

namespace Interapptive.Shared.Messaging.TrackedObservable
{
    /// <summary>
    /// Logging implementation of Trackable
    /// </summary>
    public static class TrackableImpl
    {
        /// <summary>
        /// Convert Observable stream to a trackable observable stream
        /// </summary>
        public static IObservable<IMessageTracker<T>> Trackable<T>(this IObservable<T> source) where T : IShipWorksMessage
        {
            return source.Select(MessageTracker.Create);
        }
    }
}
