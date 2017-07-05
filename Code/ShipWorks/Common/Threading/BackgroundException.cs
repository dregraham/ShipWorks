using System;
using System.Diagnostics;

namespace ShipWorks.Common.Threading
{
    public class BackgroundException : Exception
    {
        // The Exception that was caught on the background thread.
        Exception backgroundEx;

        // The StackTrace from the invoking thread that led up to the starting of the background thread
        StackTrace invokingThreadTrace;

        public BackgroundException(Exception backgroundEx, StackTrace invokingThreadTrace)
            : base(backgroundEx.Message)
        {
            this.backgroundEx = backgroundEx;
            this.invokingThreadTrace = invokingThreadTrace;
        }

        /// <summary>
        /// The actual exception that was thrown on the background thread. This is NOT set as the Inner exception,
        // because when .NET does its UnhandledException handler, it throws out the outer exception (which would be this class)
        // and only uses the inner.  We don't want to be thrown out.
        /// </summary>
        public Exception ActualException
        {
            get { return backgroundEx; }
        }

        /// <summary>
        /// The StackTrace from the thread that started the background operation that led up to the execution
        /// of the background thread.
        /// </summary>
        public StackTrace InvokingThreadTrace
        {
            get { return invokingThreadTrace; }
        }
    }
}
