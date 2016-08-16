using System;
using System.Runtime.Serialization;

namespace ShipWorks.ApplicationCore.ExecutionMode
{
    [Serializable]
    public class UserInterfaceAlreadyOpenException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceAlreadyOpenException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UserInterfaceAlreadyOpenException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInterfaceAlreadyOpenException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public UserInterfaceAlreadyOpenException(string message, Exception innerException)
            : base (message, innerException)
        { }
        
        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected UserInterfaceAlreadyOpenException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        { }
    }
}
