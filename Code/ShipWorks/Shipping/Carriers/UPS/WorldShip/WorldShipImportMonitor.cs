using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Interapptive.Shared.Utility;
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
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.ServiceManager;
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
        /// Package type names that come back from WorldShip
        /// Pattern from http://msdn.microsoft.com/en-us/library/ms182275(v=vs.100).aspx
        /// </summary>
        static Dictionary<string, UpsPackagingType> upsPackageTypeNames = InitializeWorldShipPackageTypes();

        /// <summary>
        /// Initializes the UPS Service Names dictionary
        /// </summary>
        private static Dictionary<string, UpsPackagingType> InitializeWorldShipPackageTypes()
        {
            // WS returns the names of the packages differently, so create a mapping of WS package type names 
            // to our UpsPackagingType
            upsPackageTypeNames = new Dictionary<string, UpsPackagingType>();
            upsPackageTypeNames["BPM FLATS"] = UpsPackagingType.BPMFlats;
            upsPackageTypeNames["BPM PARCEL"] = UpsPackagingType.BPMParcels;
            upsPackageTypeNames["BPM"] = UpsPackagingType.BPM;
            upsPackageTypeNames["FIRST CLASS"] = UpsPackagingType.FirstClassMail;
            upsPackageTypeNames["FLATS"] = UpsPackagingType.Flats;
            upsPackageTypeNames["IRREGULARS"] = UpsPackagingType.Irregulars;
            upsPackageTypeNames["MACHINABLES"] = UpsPackagingType.Machinables;
            upsPackageTypeNames["MEDIA MAIL"] = UpsPackagingType.MediaMail;
            upsPackageTypeNames["PACKAGE"] = UpsPackagingType.Custom;
            upsPackageTypeNames["PARCEL POST"] = UpsPackagingType.ParcelPost;
            upsPackageTypeNames["PARCELS"] = UpsPackagingType.Parcels;
            upsPackageTypeNames["PRIORITY"] = UpsPackagingType.PriorityMail;
            upsPackageTypeNames["STANDARD FLATS"] = UpsPackagingType.StandardFlats;
            upsPackageTypeNames["UPS 10 KG BOX"] = UpsPackagingType.Box10Kg;
            upsPackageTypeNames["UPS 25 KG BOX"] = UpsPackagingType.Box25Kg;
            upsPackageTypeNames["UPS EXPRESS BOX LARGE"] = UpsPackagingType.BoxExpressLarge;
            upsPackageTypeNames["UPS EXPRESS BOX MEDIUM"] = UpsPackagingType.BoxExpressMedium;
            upsPackageTypeNames["UPS EXPRESS BOX SMALL"] = UpsPackagingType.BoxExpressSmall;
            upsPackageTypeNames["UPS LETTER"] = UpsPackagingType.Letter;
            upsPackageTypeNames["UPS PAK"] = UpsPackagingType.Pak;
            upsPackageTypeNames["UPS TUBE"] = UpsPackagingType.Tube;

            // Canada package types
            upsPackageTypeNames["UPS EXPRESS"] = UpsPackagingType.BoxExpress;
            upsPackageTypeNames["UPS EXPRESS PAK"] = UpsPackagingType.Pak;
            upsPackageTypeNames["UPS EXPRESS TUBE"] = UpsPackagingType.Tube;
            upsPackageTypeNames["UPS EXPRESS ENVELOPE"] = UpsPackagingType.ExpressEnvelope;

            return upsPackageTypeNames;
        }

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
                    if (importList.Count() >= 0)
                    {
                        List<WorldShipProcessedEntity> toImport = importList.ToList();
                        WorldShipUtility.FixInvalidShipmentIDs(toImport);

                        // We called FixNullShipmentIDs, so if there are any, ignore them.
                        toImport = toImport.Where(i => !string.IsNullOrWhiteSpace(i.ShipmentID)).ToList();

                        // Get a list of ws processed entries that are NOT Voids
                        // To support the old mappings, include any where the VoidIndicator is null.  And if it is not null, then check that it is "N"
                        var worldShipShipments = toImport.Where(i => (i.VoidIndicator == null) || (i.VoidIndicator != null && i.VoidIndicator.ToUpperInvariant() == "N"));

                        List<WorldShipProcessedGrouping> worldShipProcessedGroupings = 
                            worldShipShipments.GroupBy(import => long.Parse(import.ShipmentID),
                                (shipmentId, importEntries) =>
                                    new WorldShipProcessedGrouping(shipmentId,
                                        importEntries.Where(i => (i.ShipmentIdCalculated == shipmentId || i.ShipmentID == shipmentId.ToString()) && ((i.VoidIndicator == null) || (i.VoidIndicator != null && i.VoidIndicator.ToUpperInvariant() == "N"))).ToList())
                                ).ToList();

                        // Process each shipped entry
                        foreach (WorldShipProcessedGrouping worldShipProcessGroup in worldShipProcessedGroupings)
                        {
                            ProcessEntry(worldShipProcessGroup);
                        }

                        // Now Process Voids
                        // The old mappings did not support voids, so only include entries where VoidIndicator is not null and equals "Y"
                        var voidedWorldShipShipments = toImport.Where(i => i.VoidIndicator != null && i.VoidIndicator.ToUpperInvariant() == "Y").GroupBy(import => import.ShipmentIdCalculated,
                            (shipmentId, importEntries) =>
                                new WorldShipProcessedGrouping(shipmentId, importEntries.Where(i => i.ShipmentIdCalculated == shipmentId && i.VoidIndicator != null && i.VoidIndicator.ToUpperInvariant() == "Y").ToList())
                            );

                        // Process each voided entry
                        foreach (WorldShipProcessedGrouping worldShipProcessGroup in voidedWorldShipShipments)
                        {
                            ProcessVoidEntry(worldShipProcessGroup);
                        }
                    }
                }

                // Delete any abandoned WorldShipProcessedEntities
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

            // Done, release the lock
            lock (busyLock)
            {
                ApplicationBusyManager.OperationComplete(busyToken);
                busyToken = null;
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

                    foreach (WorldShipProcessedEntity import in worldShipProcessedGrouping.OrderedWorldShipProcessedEntries)
                    {
                        try
                        {
                            // Try to find the package by import.UpsPackageID
                            // Will not find a package if import.UpsPackageID is blank
                            UpsPackageEntity upsPackage = upsShipment.Packages.FirstOrDefault(p => !string.IsNullOrWhiteSpace(import.UpsPackageID) && p.UpsPackageID.ToString() == import.UpsPackageID);

                            if (upsPackage == null)
                            {
                                // In case after the upgrade, WS still had entries with no UpsPackageId, we need to support them
                                // See if we can find a package that does not yet have a tracking number

                                upsPackage = upsShipment.Packages.FirstOrDefault(p => (string.IsNullOrEmpty(SafeTrim(p.TrackingNumber)) && !string.IsNullOrEmpty(SafeTrim(import.TrackingNumber))) ||
                                                                                      (string.IsNullOrEmpty(SafeTrim(p.UspsTrackingNumber)) && !string.IsNullOrEmpty(SafeTrim(import.UspsTrackingNumber))));
                            }

                            // This is the case where the user created a new package in WS, so create a new one and add to the shipment
                            if (upsPackage == null)
                            {
                                upsPackage = UpsUtility.CreateDefaultPackage();
                                upsShipment.Packages.Add(upsPackage);
                            }

                            // If we found the ups package, update it's properties
                            if (upsPackage != null)
                            {
                                // To support old mappings, make sure we have a non null ServiceType
                                if (import.ServiceType != null)
                                {
                                    // The user may have changed the service type and package type in WS, update locally so we are in sync.
                                    string worldShipServiceType = SafeTrim(import.ServiceType).ToUpperInvariant();

                                    UpsServiceManagerFactory upsServiceManagerFactory = new UpsServiceManagerFactory(upsShipment.Shipment);
                                    IUpsServiceManager upsServiceManager = upsServiceManagerFactory.Create(upsShipment.Shipment);
                                    UpsServiceMapping upsServiceMapping = null;
                                    try
                                    {
                                        upsServiceMapping = upsServiceManager.GetServicesByWorldShipDescription(worldShipServiceType, upsShipment.Shipment.AdjustedShipCountryCode());
                                    }
                                    catch (UpsException)
                                    {
                                        // The description WorldShip returned isn't one we know about.  We don't want to crash SW, so we'll just leave the service type
                                        // as is.  Just log and continue on.
                                        log.ErrorFormat("WorldShip returned an unknown description: '{0}'", worldShipServiceType);
                                    }

                                    if (upsServiceMapping != null)
                                    {
                                        upsShipment.Service = (int)upsServiceMapping.UpsServiceType;
                                    }
                                }

                                // To support old mappings, make sure we have a non null PackageType
                                if (import.PackageType != null)
                                {
                                    // Update the package type sent from WS
                                    string worldShipPackageType = SafeTrim(import.PackageType).ToUpperInvariant();
                                    if (upsPackageTypeNames.ContainsKey(worldShipPackageType))
                                    {
                                        UpsPackagingType packageType = upsPackageTypeNames[worldShipPackageType];
                                        upsPackage.PackagingType = (int)packageType;
                                    }
                                    else
                                    {
                                        // WorldShip sent us a package type that doesn't match what we expected...log it and move on
                                        log.WarnFormat("WorldShip exported an unknown Package Type, '{0}', for ShipmentID '{1}'.", worldShipPackageType, import.ShipmentID);
                                    }
                                }

                                // To support old mappings, make sure we have a non null DeclaredValueOption
                                if (import.DeclaredValueOption != null)
                                {
                                    // Update the declared value
                                    if (import.DeclaredValueAmount.HasValue && SafeTrim(import.DeclaredValueOption).ToUpperInvariant() == "Y")
                                    {
                                        upsPackage.DeclaredValue = (decimal)import.DeclaredValueAmount.Value;
                                    }
                                }

                                // Update the tracking numbers (Do this AFTER we update the service/package types)
                                SetTrackingNumbers(shipment, upsShipment, upsPackage, import);
                            }

                            // Save the published charges
                            upsShipment.PublishedCharges = (decimal)import.PublishedCharges;

                            // Figure out if there was a negotiated rate
                            if (import.NegotiatedCharges > 0)
                            {
                                upsShipment.NegotiatedRate = true;

                                shipment.ShipmentCost = (decimal)import.NegotiatedCharges;
                            }
                            else
                            {
                                upsShipment.NegotiatedRate = false;
                                shipment.ShipmentCost = upsShipment.PublishedCharges;
                            }

                            adapter.SaveAndRefetch(shipment);
                        }
                        catch (ObjectDeletedException)
                        {
                            log.WarnFormat("Shipment {0} has gone away since WorldShip processing.", import.ShipmentID);
                        }
                        catch (SqlForeignKeyException)
                        {
                            log.WarnFormat("Shipment {0} has gone away since WorldShip processing.", import.ShipmentID);
                        }
                        catch (ORMConcurrencyException ormConcurrencyException)
                        {
                            string ormConcurrencyExceptionMessage = LogExceptionUtility.LogOrmConcurrencyException(import, 
                                                                                        string.Format("ShipmentID:{0}", import.ShipmentID), 
                                                                                        ormConcurrencyException);

                            throw new UpsException(ormConcurrencyExceptionMessage, ormConcurrencyException);
                        }
                    }

                    // Mark the shipment as completed
                    upsShipment.WorldShipStatus = (int)WorldShipStatusType.Completed;

                    // Save the updated ups world ship status
                    adapter.SaveAndRefetch(shipment);

                    // Dispatch the shipment processed event
                    ActionDispatcher.DispatchShipmentProcessed(shipment, adapter);

                    // Ensure the original exported records are deleted
                    adapter.DeleteEntity(new WorldShipShipmentEntity(shipment.ShipmentID));

                    adapter.Commit();

                    // Set the shipmentId so we can load up a shipment for Tango, OUTSIDE of the SqlAdapter transaction.
                    shipmentId = shipment.ShipmentID;
                }
            }
            catch (ORMConcurrencyException ormConcurrencyException)
            {
                string ormConcurrencyExceptionMessage = LogExceptionUtility.LogOrmConcurrencyException((IEntity2)ormConcurrencyException.EntityWhichFailed,
                                                                            string.Format("ShipmentID:{0}", shipmentId),
                                                                            ormConcurrencyException);
                throw new UpsException(ormConcurrencyExceptionMessage, ormConcurrencyException);
            }

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
                        shipmentForTango.OnlineShipmentID = TangoWebClient.LogShipment(store, shipmentForTango); ;

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
        /// Determines tracking numbers for the shipment package
        /// </summary>
        /// <param name="shipment">The shipment entity</param>
        /// <param name="upsShipment">The UPS Shipment entity</param>
        /// <param name="upsPackage">The UPS Package upon which to set tracking number</param>
        /// <param name="import">The WorldShipProcessed row with tracking information for this UPS Package</param>
        private static void SetTrackingNumbers(ShipmentEntity shipment, UpsShipmentEntity upsShipment, UpsPackageEntity upsPackage, WorldShipProcessedEntity import)
        {
            string trackingNumber;
            string uspsTrackingNumber;
            string leadTrackingNumber;

            // If we are mail innovations, set the tracking number to what WS set UspsTrackingNumber to.  
            // But if that is blank, we use ReferenceNumber (1)
            // WS does not provide a LeadTrackingNumber for MI, so we'll use the UspsTrackingNumber
            if (UpsUtility.IsUpsMiService((UpsServiceType)upsShipment.Service))
            {
                uspsTrackingNumber = !string.IsNullOrWhiteSpace(import.UspsTrackingNumber) ? import.UspsTrackingNumber : (upsShipment.ReferenceNumber ?? string.Empty);
                leadTrackingNumber = uspsTrackingNumber;
                trackingNumber = string.Empty;
            }
            else if (UpsUtility.IsUpsSurePostService((UpsServiceType)upsShipment.Service)) 
            {
                // SurePost provides both UPS and USPS tracking numbers, so we'll use the UPS tracking numbers
                // so that tracking info is available as soon as possible for customers
                // Also save the USPS tracking number so that it is available via templates
                uspsTrackingNumber = import.UspsTrackingNumber ?? string.Empty;
                trackingNumber = import.TrackingNumber ?? uspsTrackingNumber;
                leadTrackingNumber = import.LeadTrackingNumber ?? trackingNumber;
            }
            else
            {
                // We aren't mail innovations or sure post, so just get the regular tracking number
                trackingNumber = import.TrackingNumber ?? string.Empty;
                leadTrackingNumber = import.LeadTrackingNumber ?? trackingNumber;
                uspsTrackingNumber = string.Empty;
            }

            upsPackage.UspsTrackingNumber = uspsTrackingNumber;
            upsShipment.UspsTrackingNumber = uspsTrackingNumber;
            upsPackage.TrackingNumber = trackingNumber;
            shipment.TrackingNumber = leadTrackingNumber;
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
                if (!shipment.Voided && shipment.Ups != null && shipment.Ups.WorldShipStatus != (int)WorldShipStatusType.Voided)
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
                                new SqlAdapterRetry<SqlAppResourceLockException>(5, -5, string.Format("WorldShipImportMonitor.ProcessVoidEntry for ShipmentID {0}", shipment.ShipmentID));

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

        /// <summary>
        /// Helper method to safely trim a possibly null string
        /// </summary>
        private static string SafeTrim(string stringToTrim)
        {
            if (!string.IsNullOrWhiteSpace(stringToTrim))
            {
                return stringToTrim.Trim();
            }
             
            return string.Empty;
        }
    }
}

