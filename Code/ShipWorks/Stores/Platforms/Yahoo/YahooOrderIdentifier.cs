using System;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Yahoo
{
    /// <summary>
    /// Custom order identifier for Yahoo! stores
    /// </summary>
    public class YahooOrderIdentifier : OrderIdentifier
    {
        // Yahoo's Order ID
        readonly string yahooOrderID;

        /// <summary>
        /// Constructor
        /// </summary>
        public YahooOrderIdentifier(string yahooOrderID)
        {
            this.yahooOrderID = yahooOrderID;
        }

        /// <summary>
        /// Apply the identifier to the yahoo order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            YahooOrderEntity yahooOrder = order as YahooOrderEntity;

            if (yahooOrder == null)
            {
                throw new YahooException("Attempted to apply a Yahoo Order ID to a null or non-Yahoo Order");
            }

            yahooOrder.YahooOrderID = yahooOrderID;
        }

        /// <summary>
        /// Apply the identifier to the given detail record for history tracking
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            MethodConditions.EnsureArgumentIsNotNull(downloadDetail, "downloadDetail");

            downloadDetail.ExtraStringData1 = yahooOrderID;
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return $"YahooOrderID:{yahooOrderID}";
        }
    }
}
