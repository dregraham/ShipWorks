using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
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
        private readonly Dictionary<StoreTypeCode, Func<IJoinOperand, IPredicate, Tuple<IJoinOperand, IPredicate>>> storeSpecificSearches =
            new Dictionary<StoreTypeCode, Func<IJoinOperand, IPredicate, Tuple<IJoinOperand, IPredicate>>>
            {
                { StoreTypeCode.Amazon, GetAmazonSearch },
                { StoreTypeCode.Ebay, GetEbaySearch },
                { StoreTypeCode.ChannelAdvisor, GetChannelAdvisorSearch }
            };

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

                    // Force the correct sort order.
                    IEnumerable<IOrderEntity> orderedByIDList = from i in orderIDs
                                                                join o in orders
                                                                  on i equals o.OrderID
                                                                select o;

                    return GenericResult.FromSuccess(orderedByIDList);
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
        /// <remarks>
        /// This cannot be async because it is called from existing UI machinery that cannot be async
        /// </remarks>
        public bool CanCombine(IStoreEntity store, IEnumerable<long> orderIDs)
        {
            DynamicQuery query = CreateCanCombineQuery(store, orderIDs, new QueryFactory());

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                long invalidOrders = sqlAdapter.FetchScalar<long>(query);

                return invalidOrders == 0;
            }
        }

        /// <summary>
        /// Create the CanCombine query
        /// </summary>
        private DynamicQuery CreateCanCombineQuery(IStoreEntity store, IEnumerable<long> orderIDs, QueryFactory queryFactory)
        {
            IJoinOperand shipmentsJoin = Joins.Left(OrderEntity.Relations.ShipmentEntityUsingOrderID);
            IPredicate orPredicate = OrderFields.StoreID != store.StoreID;
            IPredicate andWherePredicate = ShipmentFields.Processed == true;

            Func<IJoinOperand, IPredicate, Tuple<IJoinOperand, IPredicate>> getStoreSpecificSearch;
            if (storeSpecificSearches.TryGetValue((StoreTypeCode) store.TypeCode, out getStoreSpecificSearch))
            {
                Tuple<IJoinOperand, IPredicate> searchDetails = getStoreSpecificSearch(shipmentsJoin, orPredicate);

                shipmentsJoin = searchDetails.Item1;
                orPredicate = searchDetails.Item2;
            }

            return queryFactory.Create()
                .From(shipmentsJoin)
                .Select(OrderFields.OrderID.Count())
                .Where(new FieldCompareRangePredicate(OrderFields.OrderID, null, orderIDs.ToArray()))
                .AndWhere(andWherePredicate
                    .Or(orPredicate));
        }

        /// <summary>
        /// Get Amazon search pieces
        /// </summary>
        private static Tuple<IJoinOperand, IPredicate> GetAmazonSearch(IJoinOperand joins, IPredicate predicate)
        {
            string entityName = EntityTypeProvider.GetEntityTypeName(EntityType.AmazonOrderEntity);

            IJoinOperand newJoin = joins.LeftJoin(OrderEntity.Relations.GetSubTypeRelation(entityName));
            IPredicate newPredicate = predicate
                .Or(AmazonOrderFields.IsPrime.In((int) AmazonMwsIsPrime.Yes, (int) AmazonMwsIsPrime.Unknown))
                .Or(AmazonOrderFields.FulfillmentChannel.In((int) AmazonMwsFulfillmentChannel.AFN, AmazonMwsFulfillmentChannel.Unknown));

            return Tuple.Create(newJoin, newPredicate);
        }

        /// <summary>
        /// Get Ebay search pieces
        /// </summary>
        private static Tuple<IJoinOperand, IPredicate> GetEbaySearch(IJoinOperand joins, IPredicate predicate)
        {
            string entityName = EntityTypeProvider.GetEntityTypeName(EntityType.EbayOrderEntity);

            IJoinOperand newJoin = joins.LeftJoin(OrderEntity.Relations.GetSubTypeRelation(entityName));
            IPredicate newPredicate = predicate.Or(EbayOrderFields.GspEligible == true);

            return Tuple.Create(newJoin, newPredicate);
        }

        /// <summary>
        /// Get ChannelAdvisor search pieces
        /// </summary>
        private static Tuple<IJoinOperand, IPredicate> GetChannelAdvisorSearch(IJoinOperand joins, IPredicate predicate)
        {
            string entityName = EntityTypeProvider.GetEntityTypeName(EntityType.ChannelAdvisorOrderEntity);
            string itemEntityName = EntityTypeProvider.GetEntityTypeName(EntityType.ChannelAdvisorOrderItemEntity);

            IJoinOperand newJoin = joins.LeftJoin(OrderEntity.Relations.GetSubTypeRelation(entityName))
                .LeftJoin(OrderEntity.Relations.OrderItemEntityUsingOrderID)
                .LeftJoin(OrderItemEntity.Relations.GetSubTypeRelation(itemEntityName));
            IPredicate newPredicate = predicate
                .Or(ChannelAdvisorOrderFields.IsPrime.In((int) AmazonMwsIsPrime.Yes, (int) AmazonMwsIsPrime.Unknown))
                .Or(ChannelAdvisorOrderItemFields.IsFBA == true);

            return Tuple.Create(newJoin, newPredicate);
        }
    }
}