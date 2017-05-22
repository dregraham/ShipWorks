using System;
using ShipWorks.Stores.Communication.Throttling;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// BigCommerce web client throttled exception
    /// </summary>
    [Serializable]
    class BigCommerceWebClientRequestThrottledException : RequestThrottledException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceWebClientRequestThrottledException() :
            base("BigCommerce has been throttled due to too many API calls during the past rolling hour.")
        {

        }
    }
}
