using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Shopify web client throttle wait cancled exception
    /// </summary>
    [Serializable]
    public class ShopifyWebClientThrottleWaitCancelException : ShopifyException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyWebClientThrottleWaitCancelException() :
            base("Waiting for Shopify to stop throttling was canceled.", null)
        {

        }
    }
}
