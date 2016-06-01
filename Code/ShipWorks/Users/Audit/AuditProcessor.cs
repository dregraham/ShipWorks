using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;
using log4net;
using ShipWorks.Data.Model;
using ShipWorks.Data;
using System.Diagnostics;
using Interapptive.Shared;
using ShipWorks.SqlServer.Common.Data;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// The top-level Audit table contains an Action that describes the overall change of what happened
    /// during a transaction.  This is figured out after the transaction runs, and this is the class
    /// that figures it out.
    /// </summary>
    public static class AuditProcessor
    {
        static readonly ILog log = LogManager.GetLogger(typeof(AuditProcessor));

        private const string auditProcessingAppLockName = "AuditProcessorRunning";

        static Guid idleWorkGuid;

        // List of the change groups we support to what entities go in the group
        static Dictionary<AuditChangeGroup, List<EntityType>> changeGroups = new Dictionary<AuditChangeGroup, List<EntityType>>();

        /// <summary>
        /// One time initialization that should happen only once during app lifetime
        /// </summary>
        public static void InitializeForApplication()
        {
            idleWorkGuid = IdleWatcher.RegisterDatabaseDependentWork("AuditProcessor", AsyncProcessAudits, "processing audits", TimeSpan.FromMinutes(10));

            changeGroups.Add(AuditChangeGroup.Standalone, new List<EntityType>()
                {
                    EntityType.TemplateEntity
                });

            changeGroups.Add(AuditChangeGroup.Orders, new List<EntityType>()
                {
                    EntityType.CustomerEntity,
                    EntityType.OrderEntity,
                    EntityType.OrderItemEntity,
                    EntityType.OrderItemAttributeEntity,
                    EntityType.OrderChargeEntity,
                    EntityType.ShipmentEntity,
                    EntityType.NoteEntity
                });
        }

        /// <summary>
        /// Determine the action type and relevant object for each unknown audit.  Returns immediately and runs on a background thread.
        /// </summary>
        public static void ProcessAudits()
        {
            IdleWatcher.RunWorkNow(idleWorkGuid);
        }

        /// <summary>
        /// Process audits in a background thread
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void AsyncProcessAudits()
        {
            log.Info("Starting AsyncProcessAudits at " + DateTime.Now);
            bool lockAcquired = false;

            using (SqlConnection sqlConnection = SqlSession.Current.OpenConnection())
            {
                try
                {
                    lockAcquired = SqlAppLockUtility.AcquireLock(sqlConnection, auditProcessingAppLockName);
                    if (!lockAcquired)
                    {
                        log.Info("AsyncProcessAudits  was unable to acquire an app lock.  Processing must already be being done by someone else.");
                        return;
                    }

                    log.Info("Acquired AsyncProcessAudits app lock. ");

                    // If we skip any due to locking, then don't try them again during this pass.  Otherwise we could
                    // keep trying them over and over in quick succession while another SW was working on it.
                    List<long> skippedDueToLock = new List<long>();

                    while (true)
                    {
                        // Grab count at a time (arbitrary) number, but we keep looping until they are gone
                        AuditCollection audits = GetUndeterminedAudits(100);

                        // If there arent any left that we havn't already tried, get out. This would also return if there arent any in the first place
                        if (audits.Select(a => a.AuditID).Except(skippedDueToLock).Count() == 0)
                        {
                            break;
                        }

                        foreach (AuditEntity audit in audits)
                        {
                            try
                            {
                                if (!ProcessAudit(audit))
                                {
                                    skippedDueToLock.Add(audit.AuditID);
                                }
                            }
                            catch (AuditMissingObjectLabelException objectLabelMissingException)
                            {
                                HandleAuditMissingObjectLabelException(objectLabelMissingException);
                            }
                            catch (Exception ex)
                            {
                                // If an exception happens while processing audits, there's nothing the user can do about it...  Crashing doesn't help them either...
                                // So log it and carry on.
                                log.Error("An exception occurred while processing Audits.", ex);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // If an exception happens while processing audits, there's nothing the user can do about it...  Crashing doesn't help them either...
                    // So log it and carry on.
                    log.Error("An exception occurred while processing Audits.", ex);
                }
                finally
                {
                    if (lockAcquired)
                    {
                        log.Info("Releasing AsyncProcessAudits app lock. ");
                        SqlAppLockUtility.ReleaseLock(sqlConnection, auditProcessingAppLockName);
                    }
                }
            }

            log.Info("Leaving AsyncProcessAudits at " + DateTime.Now);
        }

        /// <summary>
        /// Handles logging and deletion of offending audit
        /// </summary>
        /// <param name="objectLabelMissingException"></param>
        private static void HandleAuditMissingObjectLabelException(AuditMissingObjectLabelException objectLabelMissingException)
        {
            // There's nothing we can do about these...the user will always crash until we remote in to delete the audit.  So just do that now.
            try
            {
                log.Error(objectLabelMissingException);
                DeletionService.DeleteAudit(objectLabelMissingException.AuditID);
            }
            catch (Exception ex)
            {
                log.Error("While attempting to delete an audit with missing object labels, an exception occurred.", ex);
            }
        }

        /// <summary>
        /// Get the next set of undetermined audits
        /// </summary>
        private static AuditCollection GetUndeterminedAudits(int max)
        {
            RelationPredicateBucket bucket = new RelationPredicateBucket(AuditFields.Action == (int) AuditActionType.Undetermined);
            
            using (SqlAdapter adapter = new SqlAdapter())
            {
                AuditCollection collection = new AuditCollection();
                adapter.FetchEntityCollection(collection, bucket, max, new SortExpression(AuditFields.TransactionID | SortOperator.Descending));

                return collection;
            }
        }

        /// <summary>
        /// Process the given audit.  Returns false if it is not processed due to being locked
        /// </summary>
        private static bool ProcessAudit(AuditEntity audit)
        {
            try
            {
                using (SqlEntityLock auditLock = new SqlEntityLock(audit.AuditID, "Audit Determination"))
                {
                    // We need all the child records to make our determination
                    AuditChangeCollection changes = AuditChangeCollection.Fetch(SqlAdapter.Default, AuditChangeFields.AuditID == audit.AuditID);

                    // If there are no changes, that means no details were logged, due to the "changed" columns actually just being changed
                    // to the exact same value.  Delete this audit record, its not needed.
                    if (changes.Count == 0)
                    {
                        SqlAdapter.Default.DeleteEntity(new AuditEntity(audit.AuditID));
                    }
                    else
                    {
                        // First thing to do is to condense all the changes that go to the same object.  This could happen in plenty of ways I guess,
                        // but two cases I know it happens are when:
                        //  1. When creating an order if the Local Status needs tokens, you first have to save the order, then process the tokens. So that
                        //     ends up with two entries for the order - one of the insert, one for the update.
                        //  2. When you make changes to a derived table and the normal table.  So you make a change to Shipment and FedEx shipment at the same
                        //     time, those will get condensed into a single AuditChange entry.
                        CondenseChangesForSameObject(changes);

                        // Build our catalog of changes for easier procesing
                        Dictionary<EntityType, Dictionary<long, AuditChangeType>> changeCatalog = BuildChangeCatalog(changes);

                        // Determine the change group
                        AuditChangeGroup? changeGroup = DetermineChangeGroup(changeCatalog);

                        // Process the catalog using the specified grouping strategy
                        ProcessChangeGroup(audit, changeGroup, changeCatalog);

                        // If we don't do this as serializable, we get deadlocks if the audit grid is open and the background entity fetcher
                        // is fetching at the same time we are saving
                        using (SqlAdapter adapter = new SqlAdapter(true, System.Transactions.IsolationLevel.Serializable))
                        {
                            adapter.SaveAndRefetch(audit);

                            adapter.Commit();
                        }
                    }
                }

                return true;
            }
            catch (SqlAppResourceLockException)
            {
                log.InfoFormat("Skipping audit {0} due to lock.", audit.AuditID);
                return false;
            }
        }

        /// <summary>
        /// Condense all changes that go with the same object into one change
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void CondenseChangesForSameObject(AuditChangeCollection changes)
        {
            var groups = changes.GroupBy(c => c.ObjectID).Where(g => g.Count() > 1).ToList();

            // Go through each group to consolidate
            foreach (List<AuditChangeEntity> group in groups.Select(g => g.OrderBy(a => a.AuditChangeID).ToList()))
            {
                AuditChangeEntity primary = group[0];
                AuditChangeDetailCollection primaryDetails = AuditChangeDetailCollection.Fetch(SqlAdapter.Default, AuditChangeDetailFields.AuditChangeID == primary.AuditChangeID);

                // Roll the additional group data into the primary group data
                foreach (AuditChangeEntity additional in group.Skip(1))
                {
                    // If there's a delete anywhere in there, it's a delete.  Otherwise,
                    // its going to stick with what the primary is.  If the primary was insert, and then
                    // the additional's updated it, it should still look like an insert.
                    if (additional.ChangeType == (int) AuditChangeType.Delete)
                    {
                        primary.ChangeType = (int) AuditChangeType.Delete;
                    }

                    // Get the details for this set
                    AuditChangeDetailCollection additionalDetails = AuditChangeDetailCollection.Fetch(SqlAdapter.Default, AuditChangeDetailFields.AuditChangeID == additional.AuditChangeID);

                    // Copy over all the additionals into the primary
                    foreach (AuditChangeDetailEntity detail in additionalDetails)
                    {
                        // See if there is an existing entry with the same name
                        AuditChangeDetailEntity existing = primaryDetails.FirstOrDefault(a => a.DisplayName == detail.DisplayName);

                        // If it's there, update it
                        if (existing != null)
                        {
                            existing.VariantNew = detail.VariantNew;
                            existing.VariantOld = detail.VariantOld;
                            existing.TextNew = detail.TextNew;
                            existing.TextOld = detail.TextOld;
                        }
                        // Otherwise add it in
                        else
                        {
                            detail.AuditChangeID = primary.AuditChangeID;
                            primaryDetails.Add(detail);
                        }
                    }
                }

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Save all the changes to the primary details
                    foreach (AuditChangeDetailEntity detail in primaryDetails)
                    {
                        if (adapter.SaveAndRefetch(detail))
                        {
                            DataProvider.RemoveEntity(detail.AuditChangeDetailID);
                        }
                    }

                    // Delete all the change entries we don't need anymore
                    foreach (AuditChangeEntity additional in group.Skip(1))
                    {
                        // Remove it from the main changes collection
                        changes.Remove(additional);

                        adapter.DeleteEntity(additional);
                    }

                    // Save the change to the primary change entity
                    adapter.SaveAndRefetch(primary);

                    adapter.Commit();
                }
            }
        }

        /// <summary>
        /// Process the changeCatalog using the specified grouping strategy
        /// </summary>
        private static void ProcessChangeGroup(AuditEntity audit, AuditChangeGroup? changeGroup, Dictionary<EntityType, Dictionary<long, AuditChangeType>> changeCatalog)
        {
            // If there are multiple groups, or if its standalone and there are multiple changes, then its "Various"
            if (changeGroup == null || (changeGroup == AuditChangeGroup.Standalone && changeCatalog.SelectMany(p => p.Value).Count() > 1))
            {
                audit.ObjectID = AuditUtility.VariousEntityID;
                audit.Action = (int) DetermineVariousGroupAction(changeCatalog);
            }
            else
            {
                switch (changeGroup.Value)
                {
                    case AuditChangeGroup.Standalone:
                        {
                            KeyValuePair<long, AuditChangeType> changeInfo = changeCatalog.Single().Value.Single();

                            audit.ObjectID = changeInfo.Key;
                            audit.Action = (int) GetActionFromChangeType(changeInfo.Value);
                        }
                        break;

                    case AuditChangeGroup.Orders:
                        {
                            ProcessChangeGroupOrders(audit, changeCatalog);
                        }
                        break;

                    default:
                        throw new InvalidOperationException("Unhandled ChangeGroup");
                }
            }
        }

        /// <summary>
        /// Determine the audit information for the given catalog which contains only changes related to the Orders change group.
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethod]
        private static void ProcessChangeGroupOrders(AuditEntity audit, Dictionary<EntityType, Dictionary<long, AuditChangeType>> changeCatalog)
        {
            int totalCount = changeCatalog.SelectMany(c => c.Value).Select(p => p.Value).Count();

            // A special case - a shipment on its own gets logged as the shipment.  Whereas multiple shipments will get rolledup into the order.
            if (totalCount == 1 && changeCatalog.First().Key == EntityType.ShipmentEntity)
            {
                // Don't condense
            }
            else
            {
                // Roll up all children below orders.  Attributes have to come first, since they have to be before OrderItem
                foreach (EntityType childType in new EntityType[] 
                    { 
                        EntityType.OrderItemAttributeEntity, 
                        EntityType.OrderItemEntity, 
                        EntityType.OrderChargeEntity, 
                        EntityType.ShipmentEntity,
                        EntityType.NoteEntity
                    })
                {
                    // Get the changs as related to the parent.  
                    Dictionary<long, AuditChangeType> rolledupChanges = RollupChildChanges(childType, changeCatalog, audit.AuditID);

                    // Remove this child set from the change catalog
                    changeCatalog.Remove(childType);

                    // Add in all the changes to the changeCatalog. 
                    if (rolledupChanges != null)
                    {
                        EntityType parentType = EntityUtility.GetEntityType(rolledupChanges.First().Key);

                        Dictionary<long, AuditChangeType> catalogParentChanges;
                        if (!changeCatalog.TryGetValue(parentType, out catalogParentChanges))
                        {
                            catalogParentChanges = new Dictionary<long, AuditChangeType>();
                            changeCatalog[parentType] = catalogParentChanges;
                        }

                        // Add in each change. This list will only contain parent keys that are not already in the changeCatalog.
                        foreach (KeyValuePair<long, AuditChangeType> pair in rolledupChanges)
                        {
                            catalogParentChanges.Add(pair.Key, pair.Value);
                        }
                    }
                }
            }

            // Take the count again
            int updatedCount = changeCatalog.SelectMany(c => c.Value).Select(p => p.Value).Count();

            //
            // At this point the catalog will contain Orders & Customers, or a single shipment.
            //

            // If its a single change, then we can log it
            if (updatedCount == 1)
            {
                KeyValuePair<long, AuditChangeType> changeInfo = changeCatalog.First().Value.First();

                audit.ObjectID = changeInfo.Key;
                audit.Action = (int) GetActionFromChangeType(changeInfo.Value);
            }

            else
            {
                Dictionary<long, AuditChangeType> customerChanges = null;
                changeCatalog.TryGetValue(EntityType.CustomerEntity, out customerChanges);

                Dictionary<long, AuditChangeType> orderChanges = null;
                changeCatalog.TryGetValue(EntityType.OrderEntity, out orderChanges);

                // If there are customer changes...
                if (customerChanges != null)
                {
                    // If there are multiple customer's, that's various
                    if (customerChanges.Count > 1)
                    {
                        ProcessChangeGroup(audit, null, changeCatalog);
                    }
                    else
                    {
                        KeyValuePair<long, AuditChangeType> customerChange = customerChanges.First();
                        
                        // If the customer was deleted, that's what it get's logged as
                        if (customerChange.Value == AuditChangeType.Delete)
                        {
                            audit.ObjectID = customerChange.Key;
                            audit.Action = (int) AuditActionType.Delete;
                        }

                        // If the customer is an insert or update, and there is a single order, that's a special case where
                        // the customer was created automatically when the order was created, or update automatically when the
                        // order was downloaded.
                        else
                        {
                            bool allOrdersThisCustomer = true;

                            // See if all the orders go with this customer
                            if (orderChanges != null)
                            {
                                foreach (long orderID in orderChanges.Select(p => p.Key))
                                {
                                    long? customerID = ObjectLabelManager.GetLabel(orderID).ParentID;
                                    if (customerID == null)
                                    {
                                        throw new AuditMissingObjectLabelException(string.Format("CustomerID was not logged to order {0} in ObjectLabel.", orderID), audit.AuditID);
                                    }

                                    if (customerID.Value != customerChange.Key)
                                    {
                                        allOrdersThisCustomer = false;
                                    }
                                }
                            }

                            // If the orders don't go to the customer, its various
                            if (!allOrdersThisCustomer)
                            {
                                ProcessChangeGroup(audit, null, changeCatalog);
                            }

                            // If there are no orders - or if there are multiple ones that all go to the same customer, log it to the customer
                            if (orderChanges == null || orderChanges.Count > 1)
                            {
                                audit.ObjectID = customerChange.Key;
                                audit.Action = (int) AuditActionType.Delete;
                            }

                            // At this point we know there is one order that does go with this customer
                            else
                            {
                                KeyValuePair<long, AuditChangeType> orderChange = orderChanges.Single();

                                audit.ObjectID = orderChange.Key;
                                audit.Action = (int) GetActionFromChangeType(orderChange.Value);
                            }
                        }
                    }
                }

                // There must only be order changes...
                else
                {
                    Debug.Assert(orderChanges != null);

                    // If there are multiple, then call it various
                    if (orderChanges.Count > 1)
                    {
                        ProcessChangeGroup(audit, null, changeCatalog);
                    }
                    // Otherwise use the single changed order
                    else
                    {
                        KeyValuePair<long, AuditChangeType> orderChange = orderChanges.Single();

                        audit.ObjectID = orderChange.Key;
                        audit.Action = (int) GetActionFromChangeType(orderChange.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Rollup the changes of the given child type into the set of changes for the parent type.  Children with the same parent will end up with only a parent entry.  If
        /// the parent already has its own change in the changeCatalog, then nothing is done.  If a change for the parent is rolled up, it always an update.  See content
        /// for why.
        /// </summary>
        private static Dictionary<long, AuditChangeType> RollupChildChanges(EntityType childType, Dictionary<EntityType, Dictionary<long, AuditChangeType>> changeCatalog, long auditID)
        {
            Dictionary<long, AuditChangeType> parentChanges = null;

            // First see if the original catalog has any changes of the specified child type.
            Dictionary<long, AuditChangeType> childChanges;
            if (changeCatalog.TryGetValue(childType, out childChanges))
            {
                // Next, rollup each of those changes into the parent changes table
                foreach (KeyValuePair<long, AuditChangeType> change in childChanges)
                {
                    long? parentID = ObjectLabelManager.GetLabel(change.Key).ParentID;
                    if (parentID == null)
                    {
                        throw new AuditMissingObjectLabelException(string.Format("Parent for {0} not set in ObjectLabel", change.Key), auditID);
                    }

                    EntityType parentType = EntityUtility.GetEntityType(parentID.Value);

                    // Only add this guy to the rolled up version if the parent wasnt directly changed too.  And it always gets rolled up as an 
                    // update.  So if the child was added\deleted for example, that actually shows up as an update to the parent.
                    // We are looking at the original catalog for this test, not the condensed one.
                    if (!changeCatalog.ContainsKey(parentType) || !changeCatalog[parentType].ContainsKey(parentID.Value))
                    {
                        if (parentChanges == null)
                        {
                            parentChanges = new Dictionary<long, AuditChangeType>();
                        }

                        parentChanges[parentID.Value] = AuditChangeType.Update;
                    }
                }
            }

            return parentChanges;
        }

        /// <summary>
        /// Determine what grouping of changes is affected by the contents of the change catalog.
        /// </summary>
        private static AuditChangeGroup? DetermineChangeGroup(Dictionary<EntityType, Dictionary<long, AuditChangeType>> changeCatalog)
        {
            AuditChangeGroup? changeGroup = null;

            foreach (EntityType entityType in changeCatalog.Select(p => p.Key))
            {
                AuditChangeGroup thisGroup = GetChangeGroup(entityType);

                // If we have not set it yet, set it to this one
                if (changeGroup == null)
                {
                    changeGroup = thisGroup;
                }
                else
                {
                    // If its different than we already set, then its undetermined, and we return null.
                    if (changeGroup != thisGroup)
                    {
                        return null;
                    }
                }
            }

            return changeGroup;
        }

        /// <summary>
        /// When a change group is determined to be null, this tries its best to determine the overall action that was taken
        /// </summary>
        private static AuditActionType DetermineVariousGroupAction(Dictionary<EntityType, Dictionary<long, AuditChangeType>> changeCatalog)
        {
            AuditChangeType? overallType = null;

            foreach (AuditChangeType changeType in changeCatalog.SelectMany(c => c.Value).Select(p => p.Value))
            {
                if (overallType == null)
                {
                    overallType = changeType;
                }

                else
                {
                    // If there is no agreement, its an update
                    if (overallType != changeType)
                    {
                        overallType = AuditChangeType.Update;
                    }
                }
            }

            if (overallType == null)
            {
                return AuditActionType.Update;
            }

            return GetActionFromChangeType(overallType.Value);
        }

        /// <summary>
        /// Get the action that corresponds to the given change type
        /// </summary>
        private static AuditActionType GetActionFromChangeType(AuditChangeType changeType)
        {
            switch (changeType)
            {
                case AuditChangeType.Update: return AuditActionType.Update;
                case AuditChangeType.Insert: return AuditActionType.Insert;
                case AuditChangeType.Delete: return AuditActionType.Delete;
            }

            throw new InvalidOperationException("Unhandled AuditChangeType");
        }

        /// <summary>
        /// Get the group that the specified EntityType goes in.  If not found an exception is thrown.
        /// </summary>
        private static AuditChangeGroup GetChangeGroup(EntityType entityType)
        {
            foreach (KeyValuePair<AuditChangeGroup, List<EntityType>> pair in changeGroups)
            {
                if (pair.Value.Contains(entityType))
                {
                    return pair.Key;
                }
            }

            throw new InvalidOperationException(string.Format("No change group found for entity type {0}", entityType));
        }

        /// <summary>
        /// Build the catalog of changes that break it down into a simplified form of what entity types were modified, and what changes where
        /// made to the entities within that type category.
        /// </summary>
        private static Dictionary<EntityType, Dictionary<long, AuditChangeType>> BuildChangeCatalog(AuditChangeCollection changes)
        {
            // A list of changes broken down by EntityType, and then mapped from ObjectID to how it changed.
            Dictionary<EntityType, Dictionary<long, AuditChangeType>> changeCatalog = new Dictionary<EntityType, Dictionary<long, AuditChangeType>>();

            // Go through each change to try to figure out what's going on
            foreach (AuditChangeEntity change in changes)
            {
                EntityType entityType = EntityUtility.GetEntityType(change.ObjectID);
                AuditChangeType changeType = (AuditChangeType) change.ChangeType;

                // Get the change map for this entity type
                Dictionary<long, AuditChangeType> entityChangeMap;
                if (!changeCatalog.TryGetValue(entityType, out entityChangeMap))
                {
                    entityChangeMap = new Dictionary<long, AuditChangeType>();
                    changeCatalog.Add(entityType, entityChangeMap);
                }

                // Should never already be there due to the condensing we have already done
                entityChangeMap.Add(change.ObjectID, changeType);
            }

            return changeCatalog;
        }
    }
}