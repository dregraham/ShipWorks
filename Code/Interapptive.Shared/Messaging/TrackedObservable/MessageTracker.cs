using System;
using System.Diagnostics;
using Interapptive.Shared.Messaging.Logging;

namespace Interapptive.Shared.Messaging.TrackedObservable
{
    /// <summary>
    /// Static class that makes creating message trackers easier
    /// </summary>
    public static class MessageTracker
    {
        /// <summary>
        /// Create a message tracker from an IShipWorksMessage
        /// </summary>
        public static MessageTracker<T> Create<T>(T value) where T : IShipWorksMessage
        {
            return new MessageTracker<T>(value, value, Guid.NewGuid());
        }

        /// <summary>
        /// Create a message tracker from an existing tracker and value
        /// </summary>
        public static MessageTracker<T> Create<T>(IMessageTracker tracker, T value)
        {
            return new MessageTracker<T>(tracker.Message, value, tracker.TrackingPath);
        }
    }

    /// <summary>
    /// Provide tracking information for a message
    /// </summary>
    public class MessageTracker<T> : IMessageTracker<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MessageTracker(IShipWorksMessage message, T value, Guid trackingPath)
        {
            Message = message;
            Value = value;
            TrackingPath = trackingPath;
        }

        /// <summary>
        /// Message that started the observable stream
        /// </summary>
        public IShipWorksMessage Message { get; }

        /// <summary>
        /// Current value of the stream
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Track an individual path from the stream
        /// </summary>
        public Guid TrackingPath { get; }

        public IMessageTracker<T> Do(Action<T> operation, object listener, string callerName)
        {
            long startingTimestamp = Stopwatch.GetTimestamp();
            operation(Value);
            MessageLogger.Current.Log(new DoOperation(this, Value, listener, callerName, startingTimestamp));
            return this;
        }

        /// <summary>
        /// Track a Where call
        /// </summary>
        public bool Where(Func<T, bool> predicate, object listener, string callerName)
        {
            bool value = predicate(Value);
            MessageLogger.Current.Log(new WhereOperation(this, value, listener, callerName));
            return value;
        }

        /// <summary>
        /// Track a select call
        /// </summary>
        public IMessageTracker<TReturn> Select<TReturn>(Func<T, TReturn> operation, object listener, string callerName)
        {
            var value = operation(Value);
            MessageLogger.Current.Log(new SelectOperation(this, value, listener, callerName));
            return MessageTracker.Create(this, value);
        }

        /// <summary>
        /// Track a subscription
        /// </summary>
        public void Subscribe(Action<T> operation, object listener, string callerName)
        {
            MessageLogger.Current.Log(new SubscribeOperation(this, listener, callerName));
            operation(Value);
        }
    }
}