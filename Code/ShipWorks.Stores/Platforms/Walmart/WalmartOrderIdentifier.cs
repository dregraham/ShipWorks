using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Walmart
{
    /// <summary>
    /// Uniquely identifies an Walmart order in the database
    /// </summary>
    public class WalmartOrderIdentifier : OrderIdentifier
    {
        private readonly string purchaseOrderId;

        /// <summary>
        /// Constructor
        /// </summary>
        public WalmartOrderIdentifier(string purchaseOrderId)
        {
            this.purchaseOrderId = purchaseOrderId;
        }

        /// <summary>
        /// Apply the order identifier values to the order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            MethodConditions.EnsureArgumentIsNotNull(order);

            WalmartOrderEntity walmartOrder = order as WalmartOrderEntity;

            if (walmartOrder == null)
            {
                throw new InvalidOperationException("A non Walmart order was passed to the Walmart order identifier.");
            }

            walmartOrder.PurchaseOrderID = purchaseOrderId;
        }

        /// <summary>
        /// Apply the order identifier values to the download history entry
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            MethodConditions.EnsureArgumentIsNotNull(downloadDetail);

            downloadDetail.ExtraStringData1 = purchaseOrderId;
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return $"WalmartPurchaseOrderID:{purchaseOrderId}";
        }
    }
}