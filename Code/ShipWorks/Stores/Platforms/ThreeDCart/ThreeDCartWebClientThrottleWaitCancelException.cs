using System;
using System.Runtime.Serialization;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// 3D Cart web client throttle wait cancled exception
    /// </summary>
    [Serializable]
    public sealed class ThreeDCartWebClientThrottleWaitCancelException : ThreeDCartException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartWebClientThrottleWaitCancelException() : 
            base("Waiting for 3D Cart to stop throttling was canceled.")
        {

        }

        /// <summary>
        /// Serialization constructor
        /// </summary>
        private ThreeDCartWebClientThrottleWaitCancelException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {

        }
    }
}
