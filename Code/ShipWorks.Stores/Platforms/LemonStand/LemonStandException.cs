using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    [Serializable]
    public class LemonStandException : Exception
    {
        public LemonStandException()
        {
        }

        public LemonStandException(string message)
            : base(message)
        {
        }

        public LemonStandException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected LemonStandException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {
        }
    }
}