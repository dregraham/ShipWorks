using System;
using ShipWorks.Stores.Communication.Throttling;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// ThreeDCart web client throttled exception
    /// </summary>
    [Serializable]
    class ThreeDCartWebClientRequestThrottledException : RequestThrottledException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartWebClientRequestThrottledException() :
            base("3dcart has been throttled due to too many API calls.")
        {

        }
    }
}
