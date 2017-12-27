using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Class for splitting order charges between two orders
    /// </summary>
    [Component]
    public class OrderChargeSplitter : IOrderChargeSplitter
    {
        /// <summary>
        /// Split the charges
        /// </summary>
        /// <param name="newOrderChargeAmounts">Dictionary<OrderChargeID, Amount></OrderChargeID></param>
        /// <param name="originalOrder">The source order that is being split in two</param>
        /// <param name="splitOrder">The new order created from originalOrder</param>
        public void Split(IDictionary<long, decimal> newOrderChargeAmounts, OrderEntity originalOrder, OrderEntity splitOrder)
        {
            // Go through each new order charges 
            foreach (KeyValuePair<long, decimal> orderChargesDefinition in newOrderChargeAmounts)
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

            // Remove any charges from the split order that were not in the newOrderChargeAmounts
            // or had 0 Amounts
            IList<long> notPresentOrderChargeIDs = splitOrder.OrderCharges
                .Select(oi => oi.OrderChargeID)
                .Except(newOrderChargeAmounts.Keys)
                .Concat(splitOrder.OrderCharges.Where(oc => oc.Amount == 0).Select(oc => oc.OrderChargeID))
                .Distinct()
                .ToList();

            foreach (long orderChargeID in notPresentOrderChargeIDs)
            {
                splitOrder.OrderCharges.Remove(splitOrder.OrderCharges.First(oi => oi.OrderChargeID == orderChargeID));
            }

            // Remove any charges from the original order that have an Amount of 0
            IList<long> originalChargesToRemove = originalOrder.OrderCharges
                .Where(oc => oc.Amount == 0)
                .Select(oc => oc.OrderChargeID)
                .ToList();

            foreach (long orderChargeID in originalChargesToRemove)
            {
                originalOrder.OrderCharges.Remove(originalOrder.OrderCharges.First(oi => oi.OrderChargeID == orderChargeID));
            }
        }
    }
}
