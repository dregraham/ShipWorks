using System;
using System.Runtime.Serialization;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Base for all handleable ups exceptions
    /// </summary>
    [Serializable]
    public class UpsException : CarrierException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsException()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsException(string message, Exception innerEx)
            : base(message, innerEx)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected UpsException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {

        }

        /// <summary>
        /// Gets the error code
        /// </summary>
        public virtual string ErrorCode
        {
            get { return "0"; }
        }
    }
}
