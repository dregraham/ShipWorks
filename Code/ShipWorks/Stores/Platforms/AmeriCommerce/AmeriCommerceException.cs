using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// Exception to wrap error messages and codes coming back from failed CA calls
    /// </summary>
    [Serializable]
    public class AmeriCommerceException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceException(string message, Exception inner)
            : base(message, inner)
        {
        }


        /// <summary>
        /// Constructor for just message
        /// </summary>
        public AmeriCommerceException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected AmeriCommerceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Deserilization
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
