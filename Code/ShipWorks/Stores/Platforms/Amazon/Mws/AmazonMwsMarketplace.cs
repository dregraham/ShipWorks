using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Data container for a marketplace
    /// </summary>
    public class AmazonMwsMarketplace
    {
        public string MarketplaceID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string DomainName
        {
            get; 
            set;
        }
    }
}
