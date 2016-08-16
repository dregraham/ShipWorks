namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Release mode version of the message logger that does nothing
    /// </summary>
    public class ReleaseMessageLogger : IMessageLogger
    {
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
