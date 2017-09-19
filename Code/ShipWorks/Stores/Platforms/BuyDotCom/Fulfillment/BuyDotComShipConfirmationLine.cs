using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.BuyDotCom.Fulfillment
{
    /// <summary>
    /// Stores information about an Order Line
    /// </summary>
    public class BuyDotComShipConfirmationLine
    {
        public string ReceiptItemID
        {
            get; set;
        }

        public double Quantity
        {
            get; set;
        }
    }
}
