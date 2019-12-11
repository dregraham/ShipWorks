using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Rakuten.Warehouse
{
    class RakutenWarehouseStore
    {
        /// <summary>
        /// Rakuten Store AuthKey
        /// </summary>
        public string AuthKey { get; set; }

        /// <summary>
        /// Rakuten Store Marketplace ID
        /// </summary>
        public string MarketplaceID { get; set; }

        /// <summary>
        /// Rakuten Store Shop URL
        /// </summary>
        public string ShopURL { get; set; }

        /// <summary>
        /// The date to start downloading from
        /// </summary>
        public DateTime DownloadStartDate { get; set; }
    }
}
