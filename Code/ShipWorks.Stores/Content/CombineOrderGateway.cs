using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Order gateway for combining orders
    /// </summary>
    [Component]
    public class CombineOrderGateway : ICombineOrderGateway
    {
        private readonly IOrderManager orderManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrderGateway(IOrderManager orderManager, ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Load information needed for combining the orders
        /// </summary>
        /// <remarks>
        /// We should change the return type from IOrderEntity to an
        /// actual projection class, depending on our needs
        /// </remarks>
        public async Task<GenericResult<IEnumerable<IOrderEntity>>> LoadOrders(IEnumerable<long> orderIDs)
        {
            try
            {
                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
                {
                    IEnumerable<IOrderEntity> orders = await orderManager.LoadOrdersAsync(orderIDs, sqlAdapter);
                    return GenericResult.FromSuccess(orders);
                }
            }
            catch (ORMException ex)
            {
                return GenericResult.FromError<IEnumerable<IOrderEntity>>(ex);
            }
        }

        /// <summary>
        /// Can the given orders be combined
        /// </summary>
        public bool CanCombine(IStoreEntity store, IEnumerable<long> orderIDs)
        {
            IJoinOperand shipmentsJoin = Joins.Left(OrderEntity.Relations.ShipmentEntityUsingOrderID);
            IPredicate orPredicate = OrderFields.StoreID != store.StoreID;
            IPredicate andWherePredicate = ShipmentFields.Processed == true;

            if (store.TypeCode == (int) StoreTypeCode.Amazon)
            {
                shipmentsJoin = shipmentsJoin.LeftJoin(OrderEntity.Relations.GetSubTypeRelation("AmazonOrderEntity"));
                orPredicate = orPredicate.Or(AmazonOrderFields.IsPrime.In((int) AmazonMwsIsPrime.Yes, (int) AmazonMwsIsPrime.Unknown))
                    .Or(AmazonOrderFields.FulfillmentChannel.In((int) AmazonMwsFulfillmentChannel.AFN, AmazonMwsFulfillmentChannel.Unknown));
            }
            else if (store.TypeCode == (int) StoreTypeCode.Ebay)
            {
                shipmentsJoin = shipmentsJoin.LeftJoin(OrderEntity.Relations.GetSubTypeRelation("EbayOrderEntity"));
                orPredicate = orPredicate.Or(EbayOrderFields.GspEligible == true);
            }
            else if (store.TypeCode == (int) StoreTypeCode.ChannelAdvisor)
            {
                shipmentsJoin = shipmentsJoin.LeftJoin(OrderEntity.Relations.GetSubTypeRelation("ChannelAdvisorOrderEntity"))
                    .LeftJoin(OrderEntity.Relations.OrderItemEntityUsingOrderID)
                    .LeftJoin(OrderItemEntity.Relations.GetSubTypeRelation("ChannelAdvisorOrderItemEntity"));
                orPredicate = orPredicate.Or(ChannelAdvisorOrderFields.IsPrime.In((int) AmazonMwsIsPrime.Yes, (int) AmazonMwsIsPrime.Unknown))
                    .Or(ChannelAdvisorOrderItemFields.IsFBA == true);
            }

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                QueryFactory queryFactory = new QueryFactory();
                var query = queryFactory.Create()
                    .From(shipmentsJoin)
                    .Select(OrderFields.OrderID.Count())
                    .Where(new FieldCompareRangePredicate(OrderFields.OrderID, null, orderIDs.ToArray()))
                    .AndWhere(andWherePredicate
                        .Or(orPredicate));

                long invalidOrders = ((IDataAccessAdapter) sqlAdapter).FetchScalar<long>(query);

                return invalidOrders == 0;
            }
        }
    }
}