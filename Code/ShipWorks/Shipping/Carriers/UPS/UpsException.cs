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
        public UpsException()
        {

        }

        public UpsException(string message)
            : base(message)
        {

        }

        public UpsException(string message, Exception innerEx)
            : base(message, innerEx)
        {

        }

        public virtual string ErrorCode
        {
            get { return "0"; }
        }

        protected UpsException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {

        }
    }
}
