using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Amazon Connection information
    /// </summary>
    public interface IAmazonMwsConnection
    {
        /// <summary>
        /// Amazon account Merchant ID (aka Seller ID)
        /// </summary>
        string MerchantId { get; set; }

        /// <summary>
        /// Amazon account authentication token
        /// </summary>
        string AuthToken { get; set; }

        /// <summary>
        /// Amazon Api Region
        /// </summary>
        string AmazonApiRegion { get; set; }
    }
}
