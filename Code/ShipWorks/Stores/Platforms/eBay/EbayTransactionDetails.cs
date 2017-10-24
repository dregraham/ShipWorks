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
        public EbayToken Token { get; set; }
        public long ItemID { get; set; }
        public long TransactionID { get; set; }
        public string BuyerID { get; set; }
    }
}
