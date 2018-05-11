using System;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Overstock order identifier
    /// </summary>
    public class OverstockOrderIdentifier : OrderIdentifier
    {
        private readonly long overstockOrderID;

        /// <summary>
        /// Constructor
        /// </summary>
        public OverstockOrderIdentifier(long overstockOrderID)
        {
            this.overstockOrderID = overstockOrderID;
        }

        /// <summary>
        /// Apply the order identifier values to the order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            OverstockOrderEntity overstockOrder = order as OverstockOrderEntity;

            if (overstockOrder == null)
            {
                throw new InvalidOperationException("A non Overstock order was passed to the Overstock order identifier.");
            }

            overstockOrder.OverstockOrderID = overstockOrderID;
        }

        /// <summary>
        /// Apply the order identifier values to the download history entry
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.ExtraStringData1 = overstockOrderID.ToString();
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            CreateCombinedSearchQueryInternal(factory,
                factory.OverstockOrderSearch,
                OverstockOrderSearchFields.OriginalOrderID,
                OverstockOrderSearchFields.OverstockOrderID == overstockOrderID);

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString() => $"OverstockOrderId:{overstockOrderID}";
    }
}