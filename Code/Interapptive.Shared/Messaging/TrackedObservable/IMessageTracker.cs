using System;

namespace Interapptive.Shared.Messaging.TrackedObservable
{
    /// <summary>
    /// Basic message tracker data
    /// </summary>
    public interface IMessageTracker
    {
        /// <summary>
        /// Message that is being tracked
        /// </summary>
        IShipWorksMessage Message { get; }

        /// <summary>
        /// Track an individual path from the stream
        /// </summary>
        Guid TrackingPath { get; }
    }

    /// <summary>
    /// Message tracker that contains a value
    /// </summary>
    public interface IMessageTracker<T> : IMessageTracker
    {
        /// <summary>
        /// Value being tracked
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Track a Do call
        /// </summary>
        IMessageTracker<T> Do(Action<T> value, object listener, string callerName);

        /// <summary>
        /// Track a Where call
        /// </summary>
        bool Where(Func<T, bool> value, object listener, string callerName);

        /// <summary>
        /// Track a subscription
        /// </summary>
        void Subscribe(Action<T> operation, object listener, string callerName);

        /// <summary>
        /// Track a select call
        /// </summary>
        IMessageTracker<TReturn> Select<TReturn>(Func<T, TReturn> value, object listener, string callerName);
    }
}