using System;
using log4net;
using log4net.Core;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Logger that does nothing
    /// </summary>
    public class NullLog : ILog
    {
        private static readonly NullLogger nullLogger = new NullLogger();
        private static readonly NullLog defaultLog = new NullLog();

        /// <summary>
        /// Default null logger
        /// </summary>
        public static ILog Default => defaultLog;

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public bool IsDebugEnabled => false;

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public bool IsInfoEnabled => false;

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public bool IsWarnEnabled => false;

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public bool IsErrorEnabled => false;

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public bool IsFatalEnabled => false;

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public ILogger Logger => nullLogger;

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void Debug(object message) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void Debug(object message, Exception exception) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void DebugFormat(string format, params object[] args) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void DebugFormat(string format, object arg0) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void DebugFormat(string format, object arg0, object arg1) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void DebugFormat(string format, object arg0, object arg1, object arg2) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void DebugFormat(IFormatProvider provider, string format, params object[] args) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void Error(object message) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void Error(object message, Exception exception) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void ErrorFormat(string format, params object[] args) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void ErrorFormat(string format, object arg0) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void ErrorFormat(string format, object arg0, object arg1) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void ErrorFormat(string format, object arg0, object arg1, object arg2) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void Fatal(object message) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void Fatal(object message, Exception exception) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void FatalFormat(string format, params object[] args) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void FatalFormat(string format, object arg0) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void FatalFormat(string format, object arg0, object arg1) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void FatalFormat(string format, object arg0, object arg1, object arg2) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void FatalFormat(IFormatProvider provider, string format, params object[] args) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void Info(object message) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void Info(object message, Exception exception) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void InfoFormat(string format, params object[] args) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void InfoFormat(string format, object arg0) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void InfoFormat(string format, object arg0, object arg1) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void InfoFormat(string format, object arg0, object arg1, object arg2) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void InfoFormat(IFormatProvider provider, string format, params object[] args) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void Warn(object message) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void Warn(object message, Exception exception) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void WarnFormat(string format, params object[] args) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void WarnFormat(string format, object arg0) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void WarnFormat(string format, object arg0, object arg1) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void WarnFormat(string format, object arg0, object arg1, object arg2) { }

        /// <summary>
        /// Dummy implementation
        /// </summary>
        public void WarnFormat(IFormatProvider provider, string format, params object[] args) { }
    }
}
