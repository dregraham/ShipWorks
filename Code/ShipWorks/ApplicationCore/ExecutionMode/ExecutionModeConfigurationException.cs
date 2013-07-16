using System;
using System.Runtime.Serialization;


namespace ShipWorks.ApplicationCore.ExecutionMode
{
    /// <summary>
    /// Thrown when an execution mode cannot execute because it has not been configured correctly.
    /// </summary>
    [Serializable]
    public class ExecutionModeConfigurationException : Exception
    {
        public ExecutionModeConfigurationException() { }

        public ExecutionModeConfigurationException(string message)
            : base(message) { }

        public ExecutionModeConfigurationException(string message, Exception inner)
            : base(message, inner) { }

        protected ExecutionModeConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
