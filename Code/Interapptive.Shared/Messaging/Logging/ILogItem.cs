using System;
using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Basic data for an item that can be logged
    /// </summary>
    public interface ILogItem
    {
        /// <summary>
        /// Reference for this message through the chain
        /// </summary>
        Guid Reference { get; }

        /// <summary>
        /// Id to help track multiple paths a message can take
        /// </summary>
        Guid? TrackingPath { get; }

        /// <summary>
        /// Timestamp of the message
        /// </summary>
        long Timestamp { get; }

        /// <summary>
        /// Frequency of the timestamp value
        /// </summary>
        long TimestampFrequency { get; }

        /// <summary>
        /// Endpoint that should be used
        /// </summary>
        [JsonIgnore]
        string Endpoint { get; }
    }
}