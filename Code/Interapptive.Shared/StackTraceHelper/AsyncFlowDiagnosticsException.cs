using System;
using System.Runtime.Serialization;

namespace Interapptive.Shared.StackTraceHelper
{
    /// <summary>
    /// All-wrapping async diagnostics exception
    /// </summary>
    /// <remarks>
    /// Translated from https://msdn.microsoft.com/en-us/magazine/jj891052.aspx
    /// </remarks>
    [Serializable]
    public class AsyncFlowDiagnosticsException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AsyncFlowDiagnosticsException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AsyncFlowDiagnosticsException(string message) : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AsyncFlowDiagnosticsException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected AsyncFlowDiagnosticsException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {

        }
    }
}