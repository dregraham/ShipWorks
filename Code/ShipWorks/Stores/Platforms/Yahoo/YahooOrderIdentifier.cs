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
        readonly string yahooOrderID = "";

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
            YahooOrderEntity yahoo = (YahooOrderEntity) order;
            yahoo.YahooOrderID = yahooOrderID;
        }

        /// <summary>
        /// Apply the identifier to the given detail record for history tracking
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
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
