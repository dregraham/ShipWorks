using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Content.CombineOrderActions
{
    /// <summary>
    /// Merge order charges from a list of orders to the combined order
    /// </summary>
    public class CombineOrderMergeChargesAction : ICombineOrderAction
    {
        /// <summary>
        /// Perform the merge of order charges
        /// </summary>
        public async Task Perform(OrderEntity combinedOrder, long survivingOrderID, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            EntityQuery<OrderChargeEntity> query = new QueryFactory().OrderCharge
                .Where(OrderChargeFields.OrderID.In(orders.Select(x => x.OrderID)));

            IEnumerable<OrderChargeEntity> orderCharges = (await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false)).Cast<OrderChargeEntity>();

            if (orderCharges.None())
            {
                return;
            }

            // Group the charges by Type and Description so that we can sum the Amounts into a single charge
            IEnumerable<OrderChargeEntity> groupedCharges = orderCharges
                                                                .GroupBy(c => new { c.Type, c.Description },
                                                                  (key, g) => new OrderChargeEntity(0)
                                                                      {
                                                                          OrderID = combinedOrder.OrderID,
                                                                          Type = key.Type,
                                                                          Description = key.Description,
                                                                          Amount = g.Sum(g2 => g2.Amount)
                                                                      });
            
            combinedOrder.OrderCharges.AddRange(groupedCharges);

            await sqlAdapter.SaveEntityCollectionAsync(combinedOrder.OrderCharges);
        }
    }
}
