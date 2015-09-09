using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    /// <summary>
    /// Buy.Com specific Exception
    /// </summary>
    [Serializable]
    public class BuyDotComException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComException()
        {

        }
        
        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComException(string message, Exception inner)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected BuyDotComException(SerializationInfo serializationInfo, StreamingContext streamingContext) : 
            base(serializationInfo, streamingContext)
        {

        }
    }
}