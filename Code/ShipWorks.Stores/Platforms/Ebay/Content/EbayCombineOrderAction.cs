using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.Actions;

namespace ShipWorks.Stores.Platforms.Ebay.Content
{
    /// <summary>
    /// Combination action that is specific to Ebay
    /// </summary>
    [KeyedComponent(typeof(IStoreSpecificCombineOrderAction), StoreTypeCode.Ebay)]
    public class EbayCombineOrderAction : IStoreSpecificCombineOrderAction
    {
        private readonly static EntityField2[] rollupFields = new[] {
            EbayOrderItemFields.EffectivePaymentMethod,
            EbayOrderItemFields.EffectiveCheckoutStatus,
            EbayOrderItemFields.PayPalAddressStatus,
            EbayOrderItemFields.FeedbackLeftType,
            EbayOrderItemFields.FeedbackLeftComments,
            EbayOrderItemFields.FeedbackReceivedType,
            EbayOrderItemFields.FeedbackReceivedComments };

        /// <summary>
        /// Perform the platform specific action
        /// </summary>
        public async Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            // For some reason, EbayOrderItem has a FK to Order which also needs to be updated to point to the combined order id
            IRelationPredicateBucket itemsBucket = new RelationPredicateBucket(EbayOrderItemFields.LocalEbayOrderID.In(orders.Select(x => x.OrderID)));
            var itemTemplate = new EbayOrderItemEntity
            {
                LocalEbayOrderID = combinedOrder.OrderID,
            };

            // Setting these fields to themselves causes SQL to mark the fields as updated, which causes our triggers to recalculate the rollups
            foreach (var rollupField in rollupFields)
            {
                itemTemplate.Fields[rollupField.FieldIndex].ExpressionToApply = new Expression(rollupField);
            }

            await sqlAdapter.UpdateEntitiesDirectlyAsync(itemTemplate, itemsBucket);

            EbayOrderEntity order = (EbayOrderEntity) combinedOrder;

            order.GspEligible = orders
                .Where(o => o is EbayOrderEntity)
                .Cast<EbayOrderEntity>()
                .Any(o => o.GspEligible);

            var recordCreator = new SearchRecordMerger<IEbayOrderEntity>(combinedOrder, orders, sqlAdapter);

            await recordCreator.Perform(EbayOrderSearchFields.OrderID,
                x => new EbayOrderSearchEntity
                {
                    OrderID = combinedOrder.OrderID,
                    EbayOrderID = x.EbayOrderID,
                    EbayBuyerID = x.EbayBuyerID,
                    SellingManagerRecord = x.SellingManagerRecord,
                    OriginalOrderID = x.OrderID
                });
        }
    }
}