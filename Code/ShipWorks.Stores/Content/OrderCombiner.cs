using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
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

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderCombiner(IOrderManager orderManager, IDeletionService deletionService, IConfigurationData configurationData)
        {
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
                    OrderEntity combinedOrder = await orderManager.LoadOrderAsync(survivingOrderID, sqlAdapter).ConfigureAwait(false);

                    combinedOrder.IsNew = true;
                    combinedOrder.OrderID = 0;
                    combinedOrder.ApplyOrderNumberPostfix("-C");

                    foreach (IEntityFieldCore field in combinedOrder.Fields)
                    {
                        field.IsChanged = true;
                    }

                    bool saveResult = await sqlAdapter.SaveEntityAsync(combinedOrder, true).ConfigureAwait(false);

                    if (!saveResult)
                    {
                        sqlAdapter.Rollback();
                        return GenericResult.FromError<long>("Save failed");
                    }


                    QueryFactory queryFactory = new QueryFactory();


                    // Move items
                    IRelationPredicateBucket itemsBucket = new RelationPredicateBucket(OrderItemFields.OrderID.In(orders.Select(x => x.OrderID)));
                    await sqlAdapter.UpdateEntitiesDirectlyAsync(new OrderItemEntity { OrderID = combinedOrder.OrderID }, itemsBucket);


                    // Move notes
                    IRelationPredicateBucket notesBucket = new RelationPredicateBucket(NoteFields.EntityID.In(orders.Select(x => x.OrderID)));
                    await sqlAdapter.UpdateEntitiesDirectlyAsync(new NoteEntity { EntityID = combinedOrder.OrderID }, notesBucket);
                    DynamicQuery noteCountQuery = queryFactory.Note
                        .Select(NoteFields.NoteID.Count())
                        .Where(NoteFields.EntityID == combinedOrder.OrderID);

                    combinedOrder.RollupNoteCount = await sqlAdapter.FetchScalarAsync<int>(noteCountQuery);


                    // Move paymentDetails
                    IRelationPredicateBucket paymentDetailsBucket = new RelationPredicateBucket(OrderPaymentDetailFields.OrderID.In(orders.Select(x => x.OrderID)));
                    await sqlAdapter.UpdateEntitiesDirectlyAsync(new OrderPaymentDetailEntity { OrderID = combinedOrder.OrderID }, paymentDetailsBucket);


                    // Delete
                    foreach (IOrderEntity order in orders)
                    {
                        deletionService.DeleteOrder(order.OrderID);
                    }


                    // Add search records
                    IEnumerable<OrderSearchEntity> orderSearches = orders.Select(x => new OrderSearchEntity
                    {
                        OrderID = combinedOrder.OrderID,
                        StoreID = x.StoreID,
                        OrderNumber = x.OrderNumber,
                        OrderNumberComplete = x.OrderNumberComplete
                    });

                    await sqlAdapter.SaveEntityCollectionAsync(orderSearches.ToEntityCollection());



                    await sqlAdapter.SaveEntityAsync(combinedOrder);

                    sqlAdapter.Commit();
                    return GenericResult.FromSuccess(combinedOrder.OrderID);
                }
            }
        }
    }
}
