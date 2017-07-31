using System;
using System.Diagnostics;
using WindowsFormsApp1.StackTraceHelper;

namespace ShipWorks.Common.Threading
{
    /// <summary>
    /// Exception caught on background thread
    /// </summary>
    public class BackgroundException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BackgroundException(Exception backgroundEx, StackTrace invokingThreadTrace)
            : base(backgroundEx.Message)
        {
            ActualException = backgroundEx;
            InvokingThreadTrace = invokingThreadTrace;
            CausalityChain = FlowReservoir.ExtendedStack;
        }

        /// <summary>
        /// The actual exception that was thrown on the background thread. This is NOT set as the Inner exception,
        // because when .NET does its UnhandledException handler, it throws out the outer exception (which would be this class)
        // and only uses the inner.  We don't want to be thrown out.
        /// </summary>
        public Exception ActualException { get; }

        /// <summary>
        /// The StackTrace from the thread that started the background operation that led up to the execution
        /// of the background thread.
        /// </summary>
        public StackTrace InvokingThreadTrace { get; }

        /// <summary>
        /// Chain of async method calls, if any
        /// </summary>
        public string CausalityChain { get; }
    }
}
