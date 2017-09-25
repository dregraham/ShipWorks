using System.Collections.Generic;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Stores.Content.CombineOrderActions
{
    /// <summary>
    /// Move validated addresses from a list of orders to the combined order
    /// </summary>
    public class CombineOrderMoveValidatedAddressesAction : ICombineOrderAction
    {
        /// <summary>
        /// Perform the combination of validated addresses
        /// </summary>
        public Task Perform(OrderEntity combinedOrder, long survivingOrderID, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter)
        {
            IRelationPredicateBucket itemsBucket = new RelationPredicateBucket(ValidatedAddressFields.ConsumerID == survivingOrderID);
            return sqlAdapter.UpdateEntitiesDirectlyAsync(new ValidatedAddressEntity { ConsumerID = combinedOrder.OrderID }, itemsBucket);
        }
    }
}