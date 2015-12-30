using System;
using System.Data;
using Interapptive.Shared;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Policies;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Templates.Printing;
using ShipWorks.Users;
using ShipWorks.Users.Audit;
using ShipWorks.Users.Security;

namespace ShipWorks.Data
{
    /// <summary>
    /// Provides methods for deleting certain types of entities
    /// </summary>
    public static class DeletionService
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(DeletionService));

        // Global flag indicating if we are currently in the process of deleting the store
        static volatile bool deletingStore = false;

        /// <summary>
        /// Indicates if ShipWorks is currently in the process of deleting a store
        /// </summary>
        public static bool IsDeletingStore
        {
            get { return deletingStore; }
        }

        /// <summary>
        /// Delete the entity with the specified EntityID
        /// </summary>
        public static void DeleteEntity(long entityID)
        {
            switch (EntityUtility.GetEntityType(entityID))
            {
                case EntityType.OrderEntity:
                    DeleteOrder(entityID);
                    break;

                case EntityType.CustomerEntity:
                    DeleteCustomer(entityID);
                    break;

                case EntityType.AuditEntity:
                    DeleteAudit(entityID);
                    break;

                default:
                    throw new InvalidOperationException("Invalid entity type " + entityID);
            }
        }

        /// <summary>
        /// Delete the specified audit
        /// </summary>
        public static void DeleteAudit(long auditID)
        {
            DeleteWithCascade(EntityType.AuditEntity, auditID, SqlAdapter.Default);

            DataProvider.RemoveEntity(auditID);
        }

        /// <summary>
        /// Permanently delete the selected store from ShipWorks
        /// </summary>
        public static void DeleteStore(StoreEntity store, ISecurityContext securityContext)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            securityContext.DemandPermission(PermissionType.ManageStores, store.StoreID);

            deletingStore = true;

            try
            {
                SqlAdapterRetry<SqlDeadlockException> sqlDeadlockRetry = new SqlAdapterRetry<SqlDeadlockException>(5, -5, string.Format("DeletionService.DeleteStore for store {0}", store.StoreName));
                sqlDeadlockRetry.ExecuteWithRetry((SqlAdapter adapter) => DeleteStore(store, adapter));
            }
            finally
            {
                deletingStore = false;
            }

            // Refresh the nudges, just in case there were any that shouldn't be displayed now due to the deletion of this store.
            // Ask the store manager to check for changes so that it doesn't return the store we just deleted.  The heart beat may
            // not have run to force the check yet.
            StoreManager.CheckForChanges();
            NudgeManager.Refresh(StoreManager.GetAllStores());
        }

        /// <summary>
        /// Delete the specified order.
        /// </summary>
        public static void DeleteOrder(long orderID)
        {
            UserSession.Security.DemandPermission(PermissionType.OrdersModify, orderID);

            using (AuditBehaviorScope scope = new AuditBehaviorScope(ConfigurationData.Fetch().AuditDeletedOrders ? AuditState.Enabled : AuditState.NoDetails))
            {
                SqlAdapterRetry<SqlDeadlockException> sqlDeadlockRetry = new SqlAdapterRetry<SqlDeadlockException>(5, -5, string.Format("DeletionService.DeleteWithCascade for OrderID {0}", orderID));
                sqlDeadlockRetry.ExecuteWithRetry((SqlAdapter adapter) => DeleteWithCascade(EntityType.OrderEntity, orderID, adapter));
            }

            DataProvider.RemoveEntity(orderID);
        }

        /// <summary>
        /// Delete the specified order within an existing adapter/transaction.
        /// </summary>
        public static void DeleteOrder(long orderID, SqlAdapter adapter)
        {
            UserSession.Security.DemandPermission(PermissionType.OrdersModify, orderID);

            DeleteWithCascade(EntityType.OrderEntity, orderID, adapter);

            DataProvider.RemoveEntity(orderID);
        }

        /// <summary>
        /// Delete the specified customer
        /// </summary>
        public static void DeleteCustomer(long customerID)
        {
            UserSession.Security.DemandPermission(PermissionType.CustomersDelete, customerID);

            using (AuditBehaviorScope scope = new AuditBehaviorScope(ConfigurationData.Fetch().AuditDeletedOrders ? AuditState.Enabled : AuditState.NoDetails))
            {
                SqlAdapterRetry<SqlDeadlockException> sqlDeadlockRetry = new SqlAdapterRetry<SqlDeadlockException>(5, -5, string.Format("DeletionService.DeleteCustomer for CustomerID {0}", customerID));
                sqlDeadlockRetry.ExecuteWithRetry((SqlAdapter adapter) => DeleteWithCascade(EntityType.CustomerEntity, customerID, adapter));
            }

            DataProvider.RemoveEntity(customerID);
        }

        /// <summary>
        /// Delete the given store 
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void DeleteStore(StoreEntity store, SqlAdapter adapter)
        {
            // At first I was just doing the following:
            //   C#: DeleteWithCascade(EntityType.StoreEntity, store.StoreID);
            // However, this
            //  1) Does not delete any customers, so a lot of customers are left over with zero orders.
            //  2) Makes it hard to deal with removing notes.

            // So now the strategy is:
            //  1) Delete all customers that only have orders for the given store.
            //  2) Delete all orders in the store (which may leave customers remaining that have orders from other stores)

            // We can shortcut all the orders\customers deleting if the store isn't even setup yet
            if (store.SetupComplete)
            {
                ResultsetFields customerFields = new ResultsetFields(1);
                customerFields.DefineField(CustomerFields.CustomerID, 0, "CustomerID", "");

                // SELECT COUNT(OrderID) FROM Order WHERE Order.CustomerID = CustomerID
                EntityField2 customerOrdersAll = OrderFields.OrderID;
                customerOrdersAll.ExpressionToApply = new ScalarQueryExpression(OrderFields.OrderID.SetAggregateFunction(AggregateFunction.Count),
                    OrderFields.CustomerID == CustomerFields.CustomerID);

                // SELECT COUNT(OrderID) FROM Order WHERE Order.CustomerID = CustomerID AND Order.StoreID = @storeID
                EntityField2 customerOrdersStore = OrderFields.OrderID;
                customerOrdersStore.ExpressionToApply = new ScalarQueryExpression(OrderFields.OrderID.SetAggregateFunction(AggregateFunction.Count),
                    OrderFields.CustomerID == CustomerFields.CustomerID & OrderFields.StoreID == store.StoreID);

                //  Delete all customers who have orders that are only in the store being deleted, or that have zero orders.
                RelationPredicateBucket customerBucket = new RelationPredicateBucket(customerOrdersAll == customerOrdersStore);

                // Delete all notes and print results for customers and orders defined by this bucket
                NoteManager.DeleteNotesForDeletedStore(customerBucket, adapter);
                PrintResultLogger.DeleteForDeletedStore(customerBucket, adapter);

                // Now, delete all the customers
                DeleteChildRelations(customerBucket, EntityType.CustomerEntity, adapter);

                // Now find all leftover orders.  These will be the ones who have a customer that has an order from another store.  There shouldnt
                // be too many of these, so we should be ok to do these one at a time.
                ResultsetFields orderFields = new ResultsetFields(1);
                orderFields.DefineField(OrderFields.OrderID, 0, "OrderID", "");
                RelationPredicateBucket orderBucket = new RelationPredicateBucket(OrderFields.StoreID == store.StoreID);

                // Keep deleting order's until there are no more
                while (true)
                {
                    DataTable result = new DataTable();
                    adapter.FetchTypedList(orderFields, result, orderBucket, 1000, false);

                    if (result.Rows.Count == 0)
                    {
                        break;
                    }

                    foreach (DataRow row in result.Rows)
                    {
                        DeleteWithCascade(EntityType.OrderEntity, (long) row[0], adapter);
                    }
                }
            }

            // Allow the store to delete any additional FK data it may have
            StoreType storeType = StoreTypeManager.GetType(store);
            storeType.DeleteStoreAdditionalData(adapter);

            // Delete any actions that are specific only to the store
            ActionManager.DeleteStoreActions(store.StoreID);

            // We delete the clone, so the original store doesnt get marked as Deleted until the StoreManager updates itself.
            StoreEntity clone = (StoreEntity) GeneralEntityFactory.Create(EntityUtility.GetEntityType(store.GetType()));
            clone.Fields = store.Fields.Clone();

            ShippingPolicies.Unload(clone.StoreID);

            // Delete the store
            adapter.DeleteEntity(clone);
        }

        /// <summary>
        /// Delete the specified EntityType, as found using the given starting predicate (that must be PK based), while
        /// first deleting all child records. 
        /// </summary>
        private static void DeleteWithCascade(EntityType entityType, long id, SqlAdapter adapter)
        {
            log.InfoFormat("Cascade delete {0} {1}", EntityTypeProvider.GetEntityTypeName(entityType), id);

            // Cleanup notes for those entities that use them
            if (entityType == EntityType.OrderEntity || entityType == EntityType.CustomerEntity)
            {
                NoteManager.DeleteNotesForDeletedEntity(id, adapter);
                PrintResultLogger.DeleteForDeletedEntity(id, adapter);
            }

            IEntityField2 primaryKeyField = GeneralEntityFactory.Create(entityType).Fields.PrimaryKeyFields[0];

            // Create the base filter, based on ok
            RelationPredicateBucket bucket = new RelationPredicateBucket(new FieldCompareValuePredicate(primaryKeyField, null, ComparisonOperator.Equal, id));

            // Delete recursively, bottom up
            DeleteChildRelations(bucket, entityType, adapter);
        }

        /// <summary>
        /// Deletes all child relations recursively, bottom up, using the specified bucket as the base filter for deletion.
        /// </summary>
        private static void DeleteChildRelations(RelationPredicateBucket baseFilter, EntityType parentType, SqlAdapter adapter)
        {
            EntityBase2 parentEntity = (EntityBase2) GeneralEntityFactory.Create(parentType);

            foreach (IEntityRelation relation in parentEntity.GetAllRelations())
            {
                // If we are the PK, then we delete the children
                if (relation.StartEntityIsPkSide)
                {
                    // Add this relation
                    RelationPredicateBucket bucket = EntityUtility.ClonePredicateBucket(baseFilter);
                    bucket.Relations.Add(relation);

                    // We need the current entity type
                    EntityType childType = EntityTypeProvider.GetEntityType(relation.GetFKEntityFieldCore(0).ContainingObjectName);

                    // Delete child relations
                    DeleteChildRelations(bucket, childType, adapter);
                }
            }

            // Delete any inherited tables
            DeleteInheritanceSubTypes(baseFilter, parentType, adapter);

            // Now delete ourself
            adapter.DeleteEntitiesDirectly(EntityTypeProvider.GetSystemType(parentType), baseFilter);
        }

        /// <summary>
        /// Delete all rows related to the given entity type via inheritance, using the specified bucket as the starting filter point.
        /// </summary>
        private static void DeleteInheritanceSubTypes(RelationPredicateBucket baseFilter, EntityType entityType, SqlAdapter adapter)
        {
            EntityBase2 entity = (EntityBase2) GeneralEntityFactory.Create(entityType);
            IInheritanceInfo inheritanceInfo = entity.GetInheritanceInfo();

            if (inheritanceInfo == null)
            {
                return;
            }

            IRelationFactory relationFactory = EntityTypeProvider.GetInheritanceRelationFactory(entityType);

            // Go through each inherited leaf
            foreach (string subTypeEntityName in inheritanceInfo.EntityNamesOfPathsToLeafs)
            {
                IEntityRelation relation = relationFactory.GetSubTypeRelation(subTypeEntityName);

                // Add this relation
                RelationPredicateBucket bucket = EntityUtility.ClonePredicateBucket(baseFilter);
                bucket.Relations.Add(relation);

                adapter.DeleteEntitiesDirectly(EntityTypeProvider.GetSystemType(subTypeEntityName), bucket);
            }
        }
    }
}
