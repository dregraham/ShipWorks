using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ShipWorks.Shipping.Profiles
{
    /// <summary>
    /// Thrown when an error occurs applying a profile
    /// </summary>
    [Description("ProfileException")]
    public class ProfileException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProfileException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProfileException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProfileException(Exception inner)
            : base(inner.Message, inner)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProfileException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected ProfileException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {

        }
    }
}
