using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// BigCommerce web client throttle wait cancled exception
    /// </summary>
    [Serializable]
    public class BigCommerceWebClientThrottleWaitCancelException : BigCommerceException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceWebClientThrottleWaitCancelException() : 
            base("Waiting for BigCommerce to stop throttling was canceled.", null)
        {

        }
    }
}
