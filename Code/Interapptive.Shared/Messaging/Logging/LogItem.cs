using System;
using System.Diagnostics;
using Interapptive.Shared.Messaging.TrackedObservable;
using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Basic data for an item that can be logged
    /// </summary>
    public class LogItem : ILogItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LogItem(IMessageTracker tracker)
        {
            Timestamp = Stopwatch.GetTimestamp();
            Reference = tracker.Message.MessageId;
            TrackingPath = tracker.TrackingPath;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LogItem(IShipWorksMessage message)
        {
            Timestamp = Stopwatch.GetTimestamp();
            Reference = message.MessageId;
        }

        /// <summary>
        /// Reference for this message through the chain
        /// </summary>
        public Guid Reference { get; }

        /// <summary>
        /// Id to help track multiple paths a message can take
        /// </summary>
        public Guid? TrackingPath { get; }

        /// <summary>
        /// Timestamp of the message
        /// </summary>
        public long Timestamp { get; }

        /// <summary>
        /// Endpoint that should be used
        /// </summary>
        [JsonIgnore]
        public string Endpoint { get; protected set; }
    }
}