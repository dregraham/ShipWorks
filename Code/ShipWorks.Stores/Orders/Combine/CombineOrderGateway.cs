using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
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
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Orders.Combine
{
    /// <summary>
    /// Order gateway for combining orders
    /// </summary>
    [Component]
    public class CombineOrderGateway : ICombineOrderGateway
    {
        private readonly Dictionary<StoreTypeCode, Func<QueryFactory, IEnumerable<long>, IJoinOperand, IPredicate, (IJoinOperand, IPredicate)>> storeSpecificSearches =
            new Dictionary<StoreTypeCode, Func<QueryFactory, IEnumerable<long>, IJoinOperand, IPredicate, (IJoinOperand, IPredicate)>>
            {
                { StoreTypeCode.Amazon, GetAmazonSearch },
                { StoreTypeCode.Ebay, GetEbaySearch },
                { StoreTypeCode.ChannelAdvisor, GetChannelAdvisorSearch }
            };

        private readonly IOrderManager orderManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IStoreTypeManager storeTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrderGateway(IOrderManager orderManager, ISqlAdapterFactory sqlAdapterFactory, IStoreTypeManager storeTypeManager)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.storeTypeManager = storeTypeManager;
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
            // The store can be null when deleting all stores
            if (store == null)
            {
                return false;
            }

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

            // If it is a Generic Module based store type use the GenericModule behavior 
            if (storeTypeManager.IsStoreTypeCodeGenericModuleBased((StoreTypeCode) store.TypeCode))
            {
                (shipmentsJoin, orPredicate) = GetGenericModuleSearch(queryFactory, orderIDs, shipmentsJoin, orPredicate);
            }

            // we can override store specific behavior here
            Func<QueryFactory, IEnumerable<long>, IJoinOperand, IPredicate, (IJoinOperand, IPredicate)> getStoreSpecificSearch;
            if (storeSpecificSearches.TryGetValue((StoreTypeCode) store.TypeCode, out getStoreSpecificSearch))
            {
                (shipmentsJoin, orPredicate) = getStoreSpecificSearch(queryFactory, orderIDs, shipmentsJoin, orPredicate);
            }
            
            return queryFactory.Create()
                .From(shipmentsJoin)
                .Select(OrderFields.OrderID.Count())
                .Where(OrderFields.OrderID.In(orderIDs)
                    .And(andWherePredicate.Or(orPredicate)));
        }

        /// <summary>
        /// Get Amazon search pieces
        /// </summary>
        private static (IJoinOperand newJoin, IPredicate newPredicate) GetAmazonSearch(QueryFactory factory, IEnumerable<long> orderIDs, IJoinOperand joins, IPredicate predicate)
        {
            string entityName = EntityTypeProvider.GetEntityTypeName(EntityType.AmazonOrderEntity);

            IJoinOperand newJoin = joins.LeftJoin(OrderEntity.Relations.GetSubTypeRelation(entityName));
            IPredicate newPredicate = predicate
                .Or(AmazonOrderFields.IsPrime.In((int) AmazonIsPrime.Yes, (int) AmazonIsPrime.Unknown))
                .Or(AmazonOrderFields.FulfillmentChannel.In((int) AmazonMwsFulfillmentChannel.AFN, AmazonMwsFulfillmentChannel.Unknown));

            return (newJoin, newPredicate);
        }

        /// <summary>
        /// Get Ebay search pieces
        /// </summary>
        private static (IJoinOperand newJoin, IPredicate newPredicate) GetEbaySearch(QueryFactory factory, IEnumerable<long> orderIDs, IJoinOperand joins, IPredicate predicate)
        {
            string entityName = EntityTypeProvider.GetEntityTypeName(EntityType.EbayOrderEntity);

            IJoinOperand newJoin = joins.LeftJoin(OrderEntity.Relations.GetSubTypeRelation(entityName));
            IPredicate newPredicate = predicate
                .Or(EbayOrderFields.GspEligible == true)
                .OrNot(factory.EbayOrder.As("Inner")
                    .Where(EbayOrderFields.OrderID.Source("Inner") == orderIDs.First())
                    .Select(EbayOrderFields.RollupEffectiveCheckoutStatus.Source("Inner"))
                    .Limit(1)
                    .Contains(EbayOrderFields.RollupEffectiveCheckoutStatus));

            return (newJoin, newPredicate);
        }

        /// <summary>
        /// Get ChannelAdvisor search pieces
        /// </summary>
        private static (IJoinOperand newJoin, IPredicate newPredicate) GetChannelAdvisorSearch(QueryFactory factory, IEnumerable<long> orderIDs, IJoinOperand joins, IPredicate predicate)
        {
            string entityName = EntityTypeProvider.GetEntityTypeName(EntityType.ChannelAdvisorOrderEntity);
            string itemEntityName = EntityTypeProvider.GetEntityTypeName(EntityType.ChannelAdvisorOrderItemEntity);

            IJoinOperand newJoin = joins.LeftJoin(OrderEntity.Relations.GetSubTypeRelation(entityName))
                .LeftJoin(OrderEntity.Relations.OrderItemEntityUsingOrderID)
                .LeftJoin(OrderItemEntity.Relations.GetSubTypeRelation(itemEntityName));
            IPredicate newPredicate = predicate
                .Or(ChannelAdvisorOrderFields.IsPrime.In((int) AmazonIsPrime.Yes, (int) AmazonIsPrime.Unknown))
                .Or(ChannelAdvisorOrderItemFields.IsFBA == true);

            return (newJoin, newPredicate);
        }

        /// <summary>
        /// Get GenericModule search pieces
        /// </summary>
        private static (IJoinOperand newJoin, IPredicate newPredicate) GetGenericModuleSearch(QueryFactory factory, IEnumerable<long> orderIDs, IJoinOperand joins, IPredicate predicate)
        {
            string entityName = EntityTypeProvider.GetEntityTypeName(EntityType.GenericModuleOrderEntity);
            string itemEntityName = EntityTypeProvider.GetEntityTypeName(EntityType.GenericModuleOrderItemEntity);

            IJoinOperand newJoin = joins.LeftJoin(OrderEntity.Relations.GetSubTypeRelation(entityName))
                .LeftJoin(OrderEntity.Relations.OrderItemEntityUsingOrderID)
                .LeftJoin(OrderItemEntity.Relations.GetSubTypeRelation(itemEntityName));

            // Find orders that are prime or unknown OR FBA, these are the ones we dont now want to allow combining for
            IPredicate newPredicate = predicate
                .Or(GenericModuleOrderFields.IsPrime.In((int) AmazonIsPrime.Yes, (int) AmazonIsPrime.Unknown))
                .Or(GenericModuleOrderFields.IsFBA.Equal(true));

            return (newJoin, newPredicate);
        }
    }
}