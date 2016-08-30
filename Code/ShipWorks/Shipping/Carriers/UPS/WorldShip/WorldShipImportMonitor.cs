using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Stores;
using ShipWorks.Users;
using log4net;
using ShipWorks.Users.Audit;

namespace ShipWorks.Shipping.Carriers.UPS.WorldShip
{
    /// <summary>
    /// Monitors for WorldShip shipments that need to be imported after they were processed in WS
    /// </summary>
    public static class WorldShipImportMonitor
    {
        static readonly ILog log = LogManager.GetLogger(typeof(WorldShipImportMonitor));

        // Indicates if we are current in the process of checking
        static ApplicationBusyToken busyToken = null;
        static object busyLock = new object();

        // Indicates if the monitor has been started
        static bool started = false;

        /// <summary>
        /// Starts monitoring for WorldShip shipments processed from WorldShip
        /// </summary>
        public static void Start()
        {
            started = true;
        }

        /// <summary>
        /// Check for shipments that need imported into WorldShip.  If the monitor has not been started nothing is done.
        /// </summary>
        public static void CheckForShipments()
        {
            if (!started)
            {
                return;
            }

            // See if we are already doing this, and if not, start it now
            lock (busyLock)
            {
                if (busyToken != null)
                {
                    return;
                }

                // If we are in a context sensitive scope, we have to wait until next time.  If we are on the UI, we'll always get it.
                // We only may not if we are running in the background.
                if (!ApplicationBusyManager.TryOperationStarting("importing from WorldShip", out busyToken))
                {
                    return;
                }
            }

            int importListCount = 0;
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                using (WorldShipProcessedCollection importList = GetWorldShipProcessedEntitiesForProcessing(adapter))
                {
                    importListCount = importList.Count;
                }
            }

            if (importListCount > 0)
            {
                ThreadPool.QueueUserWorkItem(ExceptionMonitor.WrapWorkItem(ImportFromWorldShip));
            }
            else
            {
                lock (busyLock)
                {
                    ApplicationBusyManager.OperationComplete(busyToken);
                    busyToken = null;
                }
            }
        }

        /// <summary>
        /// Gets a list of WorldShipProcessed entities that need to be processed on this Computer
        /// </summary>
        private static WorldShipProcessedCollection GetWorldShipProcessedEntitiesForProcessing(SqlAdapter adapter)
        {
            long thisComputerID = UserSession.Computer.ComputerID;
            WorldShipProcessedCollection importList = new WorldShipProcessedCollection();

            // Fetch the records to import
            IRelationPredicateBucket bucket = new RelationPredicateBucket();
            bucket.Relations.Add(WorldShipProcessedEntity.Relations.WorldShipShipmentEntityUsingShipmentIdCalculated, "", "", JoinHint.Left);
            bucket.PredicateExpression.AddWithAnd(WorldShipShipmentFields.ShipmentProcessedOnComputerID == thisComputerID | WorldShipShipmentFields.ShipmentProcessedOnComputerID == System.DBNull.Value);
            adapter.FetchEntityCollection(importList, bucket, 0);

            return importList;
        }

        /// <summary>
        /// Gets a list of WorldShipProcessed entities that need to be deleted
        /// </summary>
        private static WorldShipProcessedCollection GetAbandonedWorldShipProcessedEntitiesForDeletion(SqlAdapter adapter)
        {
            WorldShipProcessedCollection importList = new WorldShipProcessedCollection();

            // Fetch the records to delete
            IRelationPredicateBucket bucket = new RelationPredicateBucket();
            bucket.PredicateExpression.AddWithAnd(WorldShipProcessedFields.ShipmentIdCalculated == System.DBNull.Value);
            adapter.FetchEntityCollection(importList, bucket, 0);

            return importList;
        }

        /// <summary>
        /// Runs on the background thread to import shipments that had been processed in WorldShip
        /// </summary>
        private static void ImportFromWorldShip(object state)
        {
            // Don't do in a transaction, we'll get each row in a transaction later
            using (SqlAdapter adapter = new SqlAdapter(false))
            {
                using (WorldShipProcessedCollection importList = GetWorldShipProcessedEntitiesForProcessing(adapter))
                {
                    // Only do the work if we have something to process
                    if (importList.Any())
                    {
                        ImportShipments(importList);
                    }
                }

                // Delete any abandoned WorldShipProcessedEntities
                DeleteAbandonedWorldShipProcessedEntities(adapter);
            }

            // Done, release the lock
            lock (busyLock)
            {
                ApplicationBusyManager.OperationComplete(busyToken);
                busyToken = null;
            }
        }

        /// <summary>
        /// Imports the shipments.
        /// </summary>
        /// <param name="importList">The import list.</param>
        private static void ImportShipments(WorldShipProcessedCollection importList)
        {
            List<WorldShipProcessedEntity> toImport = importList.ToList();
            WorldShipUtility.FixInvalidShipmentIDs(toImport);

            // We called FixNullShipmentIDs, so if there are any, ignore them.
            toImport = toImport.Where(i => !string.IsNullOrWhiteSpace(i.ShipmentID)).ToList();

            // Get a list of ws processed entries that are NOT Voids
            // To support the old mappings, include any where the VoidIndicator is null.  And if it is not null, then check that it is "N"
            var worldShipShipments =
                toImport.Where(
                    i => (i.VoidIndicator == null) || (i.VoidIndicator != null && i.VoidIndicator.ToUpperInvariant() == "N"));

            List<WorldShipProcessedGrouping> worldShipProcessedGroupings =
                worldShipShipments.GroupBy(import => long.Parse(import.ShipmentID),
                    (shipmentId, importEntries) =>
                        new WorldShipProcessedGrouping(shipmentId,
                            importEntries.Where(
                                i =>
                                    (i.ShipmentIdCalculated == shipmentId || i.ShipmentID == shipmentId.ToString()) &&
                                    ((i.VoidIndicator == null) ||
                                     (i.VoidIndicator != null && i.VoidIndicator.ToUpperInvariant() == "N"))).ToList())
                    ).ToList();

            // Process each shipped entry
            foreach (WorldShipProcessedGrouping worldShipProcessGroup in worldShipProcessedGroupings)
            {
                ProcessEntry(worldShipProcessGroup);
            }

            // Now Process Voids
            // The old mappings did not support voids, so only include entries where VoidIndicator is not null and equals "Y"
            var voidedWorldShipShipments =
                toImport.Where(i => i.VoidIndicator != null && i.VoidIndicator.ToUpperInvariant() == "Y")
                    .GroupBy(import => import.ShipmentIdCalculated,
                        (shipmentId, importEntries) =>
                            new WorldShipProcessedGrouping(shipmentId,
                                importEntries.Where(
                                    i =>
                                        i.ShipmentIdCalculated == shipmentId && i.VoidIndicator != null &&
                                        i.VoidIndicator.ToUpperInvariant() == "Y").ToList()));

            // Process each voided entry
            foreach (WorldShipProcessedGrouping worldShipProcessGroup in voidedWorldShipShipments)
            {
                ProcessVoidEntry(worldShipProcessGroup);
            }
        }

        /// <summary>
        /// Deletes the abandoned world ship processed entities.
        /// </summary>
        /// <param name="adapter">The adapter.</param>
        private static void DeleteAbandonedWorldShipProcessedEntities(SqlAdapter adapter)
        {
            using (WorldShipProcessedCollection deleteList = GetAbandonedWorldShipProcessedEntitiesForDeletion(adapter))
            {
                if (deleteList.Count > 0)
                {
                    try
                    {
                        adapter.DeleteEntityCollection(deleteList);
                    }
                    catch (ORMConcurrencyException ex)
                    {
                        // This means that the processed entry was already deleted, so just eat the error and we'll delete any
                        // remaining abandoned entities the next time we check for updates
                        log.Warn("Abandoned WorldShip processed entity was already deleted", ex);
                    }
                }
            }
        }

        /// <summary>
        /// Gets a shipment entity by string shipmentID
        /// </summary>
        /// <param name="shipmentIdToTest">String representation of the shipmentID to find</param>
        /// <returns>ShipmentEntity if a shipment is found for shipmentIdToTest, otherwise null is returned. </returns>
        private static ShipmentEntity GetShipment(long? shipmentIdToTest)
        {
            // First we need to find the shipment
            long shipmentID = 0;
            ShipmentEntity shipment = null;

            // Try to convert the string shipment ID to a usable long
            if (shipmentIdToTest.HasValue)
            {
                shipmentID = shipmentIdToTest.Value;

                // test to make sure we have a real shipment id
                if (shipmentID % 1000 != EntityUtility.GetEntitySeed(EntityType.ShipmentEntity))
                {
                    log.ErrorFormat("Encountered invalid shipment ID '{0}' in WorldShipProcessed.ShipmentID.", shipmentID);
                    return null;
                }

                shipment = ShippingManager.GetShipment(shipmentID);
                if (shipment == null)
                {
                    log.WarnFormat("Shipment {0} has gone away since WorldShip processing.", shipmentIdToTest);
                    return null;
                }
            }
            else
            {
                log.WarnFormat("ShipmentID was null");
                return null;
            }

            return shipment;
        }

        /// <summary>
        /// Updates shipping information received from WorldShip for a shipment(s)
        /// </summary>
        /// <param name="worldShipProcessedGrouping">The WorldShipProcessedGrouping for a shipment</param>
        private static void ProcessEntry(WorldShipProcessedGrouping worldShipProcessedGrouping)
        {
            // The shipment id used for updating tango
            long shipmentId = 0;

            try
            {
                // Do in a transaction so only one shipworks can touch each one at a time
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    // Delete each import row to lock it
                    worldShipProcessedGrouping.OrderedWorldShipProcessedEntries.ForEach(worldShipProcessedEntry => adapter.DeleteEntity(new WorldShipProcessedEntity(worldShipProcessedEntry.WorldShipProcessedID)));

                    ShipmentEntity shipment = GetShipment(worldShipProcessedGrouping.ShipmentID);

                    if (shipment == null)
                    {
                        // The shipment went away...
                        // Ensure the original exported records are deleted
                        if (worldShipProcessedGrouping.ShipmentID.HasValue)
                        {
                            adapter.DeleteEntity(new WorldShipShipmentEntity(worldShipProcessedGrouping.ShipmentID.Value));
                        }

                        // Commit the delete and return.
                        adapter.Commit();

                        return;
                    }

                    // Set the shipmentId so we can load up a shipment for Tango, OUTSIDE of the SqlAdapter transaction.
                    shipmentId = shipment.ShipmentID;

                    // Now we need to make sure it is a UPS shipment
                    ShippingManager.EnsureShipmentLoaded(shipment);

                    // Get the ups entity
                    UpsShipmentEntity upsShipment = shipment.Ups;

                    // Not a UPS shipment? should not be possible
                    if (upsShipment == null)
                    {
                        throw new InvalidOperationException("How did it get processed by WorldShip if not a UPS shipment?");
                    }

                    WorldShipPackageImporter importer = new WorldShipPackageImporter(LogManager.GetLogger(typeof(WorldShipPackageImporter)));

                    foreach (WorldShipProcessedEntity import in worldShipProcessedGrouping.OrderedWorldShipProcessedEntries)
                    {
                        importer.ImportPackage(upsShipment, import, shipment, adapter);
                    }

                    SaveWorldShipStatus(upsShipment, adapter, shipment);
                }
            }
            catch (ORMConcurrencyException ormConcurrencyException)
            {
                string ormConcurrencyExceptionMessage = LogExceptionUtility.LogOrmConcurrencyException((IEntity2)ormConcurrencyException.EntityWhichFailed,
                    $"ShipmentID:{shipmentId}", ormConcurrencyException);
                throw new UpsException(ormConcurrencyExceptionMessage, ormConcurrencyException);
            }

            LogShipmentToTango(shipmentId);
        }

        /// <summary>
        /// Saves the world ship status.
        /// </summary>
        private static void SaveWorldShipStatus(UpsShipmentEntity upsShipment, SqlAdapter adapter, ShipmentEntity shipment)
        {
            // Mark the shipment as completed
            upsShipment.WorldShipStatus = (int) WorldShipStatusType.Completed;

            // Save the updated ups world ship status
            adapter.SaveAndRefetch(shipment);

            // Dispatch the shipment processed event
            ActionDispatcher.DispatchShipmentProcessed(shipment, adapter);

            // Ensure the original exported records are deleted
            adapter.DeleteEntity(new WorldShipShipmentEntity(shipment.ShipmentID));

            adapter.Commit();
        }

        /// <summary>
        /// Logs the shipment to tango.
        /// </summary>
        private static void LogShipmentToTango(long shipmentId)
        {
            // Load the shipment for sending to Tango
            ShipmentEntity shipmentForTango = ShippingManager.GetShipment(shipmentId);
            ShippingManager.EnsureShipmentLoaded(shipmentForTango);

            // If the shipment needed logged to tango do that now
            if (shipmentForTango != null)
            {
                StoreEntity store = StoreManager.GetStore(shipmentForTango.Order.StoreID);

                if (store != null)
                {
                    try
                    {
                        ITangoWebClient tangoWebClient = new TangoWebClientFactory().CreateWebClient();
                        shipmentForTango.OnlineShipmentID = tangoWebClient.LogShipment(store, shipmentForTango);

                        using (SqlAdapter adapter = new SqlAdapter())
                        {
                            adapter.SaveAndRefetch(shipmentForTango);
                            adapter.Commit();
                        }
                    }
                    catch (ShipWorksLicenseException ex)
                    {
                        throw new ShippingException(ex.Message, ex);
                    }
                    catch (TangoException ex)
                    {
                        throw new ShippingException(ex.Message, ex);
                    }
                }
            }
        }

        /// <summary>
        /// Voids WorldShipProcessed entries
        /// </summary>
        /// <param name="worldShipProcessedGrouping">The WorldShipProcessed row that is to be voided</param>
        private static void ProcessVoidEntry(WorldShipProcessedGrouping worldShipProcessedGrouping)
        {
            ShipmentEntity shipment = GetShipment(worldShipProcessedGrouping.ShipmentID);

            if (shipment != null)
            {
                // Now we need to make sure it is a UPS shipment
                ShippingManager.EnsureShipmentLoaded(shipment);

                // If it's already been voided, just skip voiding
                if (!shipment.Voided && shipment.Ups != null)
                {
                    // Since we pass the ShipmentID to ShippingManager.VoidShipment, we lose the status being set here,
                    // so we save the Ups entity.  This keeps ShippingManager.VoidShipment from calling the Ups Online Tools Void
                    // Transactionally this is OK because:
                    //   We are doing a void, so we need to mark the shipment as voided and set the worldship status and delete the
                    //   WorldShipProcessed entries.
                    //   If we commit the worldship status and ShippingManager.VoidShipment throws, the WorldShipProcessed entries still
                    //   exist and will get deleted on the next run of the importer.
                    shipment.Ups.WorldShipStatus = (int)WorldShipStatusType.Voided;
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        adapter.SaveEntity(shipment.Ups);
                        adapter.Commit();
                    }

                    using (new AuditBehaviorScope(AuditBehaviorUser.SuperUser))
                    {
                        try
                        {
                            SqlAdapterRetry<SqlAppResourceLockException> sqlAppResourceLockExceptionRetry =
                                new SqlAdapterRetry<SqlAppResourceLockException>(5, -5,
                                    $"WorldShipImportMonitor.ProcessVoidEntry for ShipmentID {shipment.ShipmentID}");

                            sqlAppResourceLockExceptionRetry.ExecuteWithRetry(() => ShippingManager.VoidShipment(shipment.ShipmentID));
                        }
                        catch (ShippingException ex)
                        {
                            // ShippingManager.VoidShipment translates a SqlAppResourceLockException to a ShippingException, so we check for that type.
                            if (!(ex.InnerException is SqlAppResourceLockException))
                            {
                                // It wasn't a SqlAppResourceLockException, so re-throw
                                throw;
                            }
                        }
                    }
                }
            }

            // Delete each of the WorldShipProcessed entries
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                foreach (WorldShipProcessedEntity import in worldShipProcessedGrouping.OrderedWorldShipProcessedEntries)
                {
                    // Delete the import row
                    adapter.DeleteEntity(new WorldShipProcessedEntity(import.WorldShipProcessedID));
                }

                adapter.Commit();
            }
        }
    }
}

