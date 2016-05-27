namespace Interapptive.Shared.Messaging.Logging
{
    /// <summary>
    /// Log message related information
    /// </summary>
    public interface IMessageLogger
    {
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