using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content.CombineOrderActions
{
    /// <summary>
    /// Perform the merge of the search records
    /// </summary>
    public class SearchRecordMerger<TEntity> where TEntity : IOrderEntity
    {
        private readonly OrderEntity combinedOrder;
        private readonly IEnumerable<TEntity> orders;
        private readonly ISqlAdapter sqlAdapter;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchRecordMerger(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter) :
            this(combinedOrder, orders, sqlAdapter, false)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchRecordMerger(OrderEntity combinedOrder, IEnumerable<IOrderEntity> orders, ISqlAdapter sqlAdapter, bool includeManualOrders)
        {
            this.sqlAdapter = sqlAdapter;
            this.orders = orders.OfType<TEntity>().Where(x => includeManualOrders || !x.IsManual);
            this.combinedOrder = combinedOrder;
        }

        /// <summary>
        /// Perform the combine
        /// </summary>
        public async Task Perform<T>(EntityField2 orderIDField, Func<TEntity, T> orderCreator) where T : EntityBase2
        {
            await UpdateSearchRecords(orderIDField);
            await InsertSearchRecords(orderCreator);
        }

        /// <summary>
        /// Insert search records for orders that have not yet been combined
        /// </summary>
        private async Task InsertSearchRecords<T>(Func<TEntity, T> orderCreator) where T : EntityBase2
        {
            IEnumerable<T> orderSearches = orders
                .Where(x => x.CombineSplitStatus != CombineSplitStatusType.Combined)
                .Select(orderCreator);

            if (orderSearches.None())
            {
                return;
            }

            await sqlAdapter.SaveEntityCollectionAsync(orderSearches.ToEntityCollection());
        }

        /// <summary>
        /// Update search records for orders that were previously combined
        /// </summary>
        private async Task UpdateSearchRecords(EntityField2 orderIDField)
        {
            IEnumerable<long> preCombinedOrders = orders
                .Where(x => x.CombineSplitStatus == CombineSplitStatusType.Combined)
                .Select(x => x.OrderID);

            if (preCombinedOrders.None())
            {
                return;
            }

            IEntity2 entity = EntityTypeProvider.CreateEntity(orderIDField.ContainingObjectName);
            entity.Fields[orderIDField.Name].CurrentValue = combinedOrder.OrderID;

            IRelationPredicateBucket itemsBucket = new RelationPredicateBucket(orderIDField.In(preCombinedOrders));
            await sqlAdapter.UpdateEntitiesDirectlyAsync(entity, itemsBucket);
        }
    }
}
