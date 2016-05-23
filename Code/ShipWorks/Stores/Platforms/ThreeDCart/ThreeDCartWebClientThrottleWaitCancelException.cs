using System;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// 3dcart web client throttle wait cancled exception
    /// </summary>
    [Serializable]
    public sealed class ThreeDCartWebClientThrottleWaitCancelException : ThreeDCartException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartWebClientThrottleWaitCancelException() : 
            base("Waiting for 3dcart to stop throttling was canceled.")
        {

        }
    }
}
