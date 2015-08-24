using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Holds Amazon Mws connection information to be used
    /// with web clients
    /// </summary>
    public class AmazonMwsConnection : IAmazonMwsConnection
    {
        /// <summary>
        /// Amazon account Merchant ID (aka Seller ID)
        /// </summary>
        public string MerchantId { get; set; }

        /// <summary>
        /// Amazon account authentication token
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Amazon Api Region
        /// </summary>
        public string AmazonApiRegion { get; set; }
    }
}
