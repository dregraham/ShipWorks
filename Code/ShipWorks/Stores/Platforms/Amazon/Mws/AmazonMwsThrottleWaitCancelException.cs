using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    [Serializable]
    public class AmazonMwsThrottleWaitCancelException : AmazonException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonMwsThrottleWaitCancelException() :
            base("Waiting for Amazon to stop throttling was canceled.", null)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected AmazonMwsThrottleWaitCancelException(SerializationInfo serializationInfo, StreamingContext streamingContext) :
            base(serializationInfo, streamingContext)
        {

        }
    }
}
