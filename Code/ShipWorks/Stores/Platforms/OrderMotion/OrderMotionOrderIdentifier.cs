using System;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.OrderMotion
{
    /// <summary>
    /// Order Identifier that identifies orders by Order Number and Order Number Postfix
    /// </summary>
    public class OrderMotionOrderIdentifier : OrderIdentifier
    {
        // order number
        long orderNumber;

        // OrderMotion shipment Id
        int orderMotionShipmentId;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderMotionOrderIdentifier(long orderNumber, int orderMotionShipmentId)
        {
            this.orderNumber = orderNumber;
            this.orderMotionShipmentId = orderMotionShipmentId;
        }

        /// <summary>
        /// Applies the identifier to another order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            OrderMotionOrderEntity omOrder = order as OrderMotionOrderEntity;
            if (omOrder == null)
            {
                throw new InvalidOperationException("A non-OrderMotion order was passed to the OrderMotion order identifier.");
            }

            omOrder.OrderNumber = orderNumber;
            omOrder.OrderMotionShipmentID = orderMotionShipmentId;
        }

        /// <summary>
        /// Apply the identifying information for an order to the Download Detail
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.OrderNumber = orderNumber;
            downloadDetail.ExtraBigIntData1 = orderMotionShipmentId;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            CreateCombinedSearchQueryInternal(factory,
                factory.OrderMotionOrderSearch,
                OrderMotionOrderSearchFields.OriginalOrderID,
                OrderMotionOrderSearchFields.OrderMotionShipmentID == orderMotionShipmentId & OrderSearchFields.OrderNumber == orderNumber);

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return string.Format("OrderMotion:{0} - {1}", orderNumber, orderMotionShipmentId);
        }
    }
}
