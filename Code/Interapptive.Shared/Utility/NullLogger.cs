using System;
using log4net.Core;
using log4net.Repository;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Logger that does nothing
    /// </summary>
    public class NullLogger : ILogger
    {
        /// <summary>
        /// Dummy implementation
        /// </summary>
        public string Name => "NullLogger";

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public ILoggerRepository Repository => null;

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public bool IsEnabledFor(Level level) => false;

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void Log(Type callerStackBoundaryDeclaringType, Level level, object message, Exception exception) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void Log(LoggingEvent logEvent) { }
    }
}
