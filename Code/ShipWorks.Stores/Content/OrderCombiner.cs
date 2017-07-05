using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content.OrderCombinerActions;
using ShipWorks.Users.Audit;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Combine multiple orders into a new, single order
    /// </summary>
    [Component]
    public class OrderCombiner : IOrderCombiner
    {
        private readonly IOrderManager orderManager;
        private readonly IDeletionService deletionService;
        readonly IConfigurationData configurationData;
        readonly IEnumerable<IOrderCombinerAction> combinationActions;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderCombiner(IOrderManager orderManager,
            IDeletionService deletionService,
            IConfigurationData configurationData,
            IEnumerable<IOrderCombinerAction> combinationActions)
        {
            this.combinationActions = combinationActions;
            this.configurationData = configurationData;
            this.deletionService = deletionService;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Perform the actual combination
        /// </summary>
        /// <remarks>
        /// I've purposely left the excessive spacing in this method to help extract each section into its own class
        /// </remarks>
        public async Task<GenericResult<long>> Combine(long survivingOrderID, IEnumerable<IOrderEntity> orders, string newOrderNumber)
        {
            using (AuditBehaviorScope scope = new AuditBehaviorScope(configurationData.FetchReadOnly().AuditDeletedOrders ? AuditState.Enabled : AuditState.NoDetails))
            {
                using (SqlAdapter sqlAdapter = SqlAdapter.Create(true))
                {
                    OrderEntity combinedOrder = await CreateCombinedOrder(survivingOrderID, sqlAdapter);

                    bool saveResult = await sqlAdapter.SaveEntityAsync(combinedOrder, true).ConfigureAwait(false);

                    if (!saveResult)
                    {
                        sqlAdapter.Rollback();
                        return GenericResult.FromError<long>("Save failed");
                    }

                    foreach (IOrderCombinerAction action in combinationActions)
                    {
                        await action.Perform(combinedOrder, orders, sqlAdapter).ConfigureAwait(false);
                    }

                    DeleteOriginalOrders(orders);

                    await sqlAdapter.SaveEntityAsync(combinedOrder).ConfigureAwait(false);

                    sqlAdapter.Commit();
                    return GenericResult.FromSuccess(combinedOrder.OrderID);
                }
            }
        }

        /// <summary>
        /// Create the combined order from the surviving order id
        /// </summary>
        private async Task<OrderEntity> CreateCombinedOrder(long survivingOrderID, ISqlAdapter sqlAdapter)
        {
            OrderEntity combinedOrder = await orderManager.LoadOrderAsync(survivingOrderID, sqlAdapter).ConfigureAwait(false);

            combinedOrder.IsNew = true;
            combinedOrder.OrderID = 0;
            combinedOrder.ApplyOrderNumberPostfix("-C");

            foreach (IEntityFieldCore field in combinedOrder.Fields)
            {
                field.IsChanged = true;
            }

            return combinedOrder;
        }

        /// <summary>
        /// Delete the original orders
        /// </summary>
        private void DeleteOriginalOrders(IEnumerable<IOrderEntity> orders)
        {
            foreach (IOrderEntity order in orders)
            {
                deletionService.DeleteOrder(order.OrderID);
            }
        }
    }
}
