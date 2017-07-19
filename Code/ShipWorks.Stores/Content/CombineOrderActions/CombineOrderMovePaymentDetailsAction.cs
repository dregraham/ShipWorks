using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Content.CombineOrderActions
{
    /// <summary>
    /// Move payment details from a list of orders to the combined order
    /// </summary>
    public class MovePaymentDetailsAction : ICombineOrderAction
    {
        /// <summary>
        /// Perform the combination of payment details
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IRelationPredicateBucket paymentDetailsBucket = new RelationPredicateBucket(OrderPaymentDetailFields.OrderID.In(orders.Select(x => x.OrderID)));
            return sqlAdapter.UpdateEntitiesDirectlyAsync(new OrderPaymentDetailEntity { OrderID = combinedOrder.OrderID }, paymentDetailsBucket);
        }
    }
}
