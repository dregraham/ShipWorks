using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Extensions;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split.Hub
{
    /// <summary>
    /// Class for splitting order charges between two orders
    /// </summary>
    public class OrderChargeSplitterHub : IOrderDetailSplitterHub
    {
        /// <summary>
        /// Split the charges
        /// </summary>
        /// <param name="orderSplitDefinition">OrderSplitDefinition</param>
        /// <param name="originalOrder">The source order that is being split in two</param>
        public void Split(OrderSplitDefinition orderSplitDefinition, OrderEntity originalOrder)
        {
            // Go through each new order charges 
            foreach (KeyValuePair<long, decimal> orderChargesDefinition in orderSplitDefinition.ChargeAmounts)
            {
                // Find the original charge based on OrderChargeID
                OrderChargeEntity originalOrderChargeEntity = originalOrder.OrderCharges.First(oi => oi.OrderChargeID == orderChargesDefinition.Key);

                // Update the new Amount to be the split order defined Amount
                decimal newChargeAmount = orderChargesDefinition.Value;

                // Update the original charge Amount to be the difference of the two
                originalOrderChargeEntity.Amount = originalOrderChargeEntity.Amount - newChargeAmount;
            }

            originalOrder.OrderCharges
                .RemoveWhere(x => x.Amount == 0 &&
                    orderSplitDefinition.ChargeAmounts.TryGetValue(x.OrderChargeID, out decimal chargeAmount) &&
                    chargeAmount != 0);
        }
    }
}
