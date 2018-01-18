using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Extensions;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Class for splitting order charges between two orders
    /// </summary>
    public class OrderChargeSplitter : IOrderDetailSplitter
    {
        /// <summary>
        /// Split the charges
        /// </summary>
        /// <param name="orderSplitDefinition">OrderSplitDefinition</param>
        /// <param name="originalOrder">The source order that is being split in two</param>
        /// <param name="splitOrder">The new order created from originalOrder</param>
        public void Split(OrderSplitDefinition orderSplitDefinition, OrderEntity originalOrder, OrderEntity splitOrder)
        {
            // Go through each new order charges 
            foreach (KeyValuePair<long, decimal> orderChargesDefinition in orderSplitDefinition.ChargeAmounts)
            {
                // Find the new order charge based on OrderChargeID
                OrderChargeEntity newOrderChargeEntity = splitOrder.OrderCharges.First(oi => oi.OrderChargeID == orderChargesDefinition.Key);

                // Find the original charge based on OrderChargeID
                OrderChargeEntity originalOrderChargeEntity = originalOrder.OrderCharges.First(oi => oi.OrderChargeID == orderChargesDefinition.Key);

                // Update the new Amount to be the split order defined Amount
                newOrderChargeEntity.Amount = orderChargesDefinition.Value;

                // Update the original charge Amount to be the difference of the two
                originalOrderChargeEntity.Amount = originalOrderChargeEntity.Amount - newOrderChargeEntity.Amount;
            }

            splitOrder.OrderCharges
                .RemoveWhere(x => x.Amount == 0 || !orderSplitDefinition.ChargeAmounts.ContainsKey(x.OrderChargeID));

            originalOrder.OrderCharges
                .RemoveWhere(x => x.Amount == 0 &&
                    orderSplitDefinition.ChargeAmounts.TryGetValue(x.OrderChargeID, out decimal chargeAmount) &&
                    chargeAmount != 0);
        }
    }
}
