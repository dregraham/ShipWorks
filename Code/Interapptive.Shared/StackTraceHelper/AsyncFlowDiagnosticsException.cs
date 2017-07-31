using System;

namespace WindowsFormsApp1.StackTraceHelper
{
    /// <summary>
    /// All-wrapping async diagnostics exception
    /// </summary>
    [Serializable]
    public class AsyncFlowDiagnosticsException : Exception
    {
        public AsyncFlowDiagnosticsException()
        {

        }

        public AsyncFlowDiagnosticsException(string message) : base(message)
        {

        }

        public AsyncFlowDiagnosticsException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}