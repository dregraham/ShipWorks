using System;
using System.Runtime.Serialization;

namespace ShipWorks.Actions.Scheduling
{
    [Serializable]
    public class SchedulingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingException"/> class.
        /// </summary>
        public SchedulingException() 
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public SchedulingException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SchedulingException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public SchedulingException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected SchedulingException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        { }
    }
}
