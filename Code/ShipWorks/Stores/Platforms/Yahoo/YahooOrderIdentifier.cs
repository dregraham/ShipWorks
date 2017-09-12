using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

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
            MethodConditions.EnsureArgumentIsNotNull(downloadDetail, nameof(downloadDetail));

            downloadDetail.ExtraStringData1 = yahooOrderID;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            CreateCombinedSearchQueryInternal(factory,
                factory.YahooOrderSearch,
                YahooOrderSearchFields.OriginalOrderID,
                YahooOrderSearchFields.YahooOrderID == yahooOrderID);

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString() => $"YahooOrderID:{yahooOrderID}";

        /// <summary>
        /// Value to use when auditing
        /// </summary>
        public override string AuditValue => yahooOrderID;
    }
}
