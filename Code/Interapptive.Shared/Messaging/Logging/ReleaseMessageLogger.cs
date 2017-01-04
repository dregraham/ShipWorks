using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Release mode version of the message logger that does nothing
    /// </summary>
    public class ReleaseMessageLogger : IMessageLogger
    {
        /// <summary>
        /// Add a converter to the list
        /// </summary>
        public void AddConverters(Func<IEnumerable<JsonConverter>> getConverters)
        {
            // In release, AddConverter is a no-op
        }

        /// <summary>
        /// Log an operation
        /// </summary>
        public void Log(ILogItem logItem)
        {
            // In release, Log is a no-op
        }

        /// <summary>
        /// Log a send message call
        /// </summary>
        public void LogSend<T>(T message, string method) where T : IShipWorksMessage
        {
            // In release, LogSend is a no-op
        }
    }
}
