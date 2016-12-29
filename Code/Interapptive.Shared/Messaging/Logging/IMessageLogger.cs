using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Log message related information
    /// </summary>
    public interface IMessageLogger
    {
        /// <summary>
        /// Add converters to use when logging
        /// </summary>
        void AddConverters(Func<IEnumerable<JsonConverter>> getConverters);

        /// <summary>
        /// Log an operation
        /// </summary>
        void Log(ILogItem logItem);

        /// <summary>
        /// Log a send message call
        /// </summary>
        void LogSend<T>(T message, string method) where T : IShipWorksMessage;
    }
}