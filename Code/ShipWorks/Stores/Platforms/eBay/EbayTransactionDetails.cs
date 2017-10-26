using ShipWorks.Stores.Platforms.Ebay.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Represents an ebay transaction
    /// </summary>
    public struct EbayTransactionDetails
    {
        /// <summary>
        /// Token for the store
        /// </summary>
        public EbayToken Token { get; set; }

        /// <summary>
        /// The Item ID
        /// </summary>
        public long ItemID { get; set; }

        /// <summary>
        /// The TransactionID
        /// </summary>
        public long TransactionID { get; set; }

        /// <summary>
        /// The BuyerID
        /// </summary>
        public string BuyerID { get; set; }
    }
}
