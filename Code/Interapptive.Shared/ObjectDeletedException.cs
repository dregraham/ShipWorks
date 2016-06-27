using System;
using System.Runtime.Serialization;

namespace Interapptive.Shared.Utility
{
    public class ObjectDeletedException : Exception
    {
        public ObjectDeletedException()
        {

        }

        public ObjectDeletedException(string message)
            : base(message)
        {

        }

        public ObjectDeletedException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected ObjectDeletedException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        {

        }
    }
}
