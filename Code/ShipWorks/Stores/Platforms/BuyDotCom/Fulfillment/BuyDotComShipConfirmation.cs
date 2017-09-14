using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.BuyDotCom.Fulfillment;

namespace ShipWorks.Stores.Platforms.BuyDotCom.Fulfillment
{
    /// <summary>
    /// Stores information required by ShipConfirmation
    /// </summary>
    public class BuyDotComShipConfirmation
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public BuyDotComShipConfirmation()
        {
            OrderLines = new List<BuyDotComShipConfirmationLine>();
        }

        public string ReceiptID
        {
            get; set;
        }

        public List<BuyDotComShipConfirmationLine> OrderLines
        {
            get; private set;
        }
        
        public BuyDotComTrackingType TrackingType
        {
            get; set;
        }
        
        public string TrackingNumber
        {
            get; set;
        }
        
        public DateTime ShipDate
        {
            get; set;
        }
    }
}