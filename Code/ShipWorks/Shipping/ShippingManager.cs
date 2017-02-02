using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Autofac;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.LicenseEnforcement;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Core.Messaging;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Utility;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Utility class for working with shipments
    /// </summary>
    [NDependIgnoreLongTypes]
    public static class ShippingManager
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ShippingManager));

        // Cache of sibling data, mapped from order -> shipment -> data
        static LruCache<long, List<long>> siblingData;

        /// <summary>
        /// Initialize whenever a new database is loaded
        /// </summary>
        public static void InitializeForCurrentDatabase()
        {
            siblingData = new LruCache<long, List<long>>(1000);
        }

        /// <summary>
        /// Get the list of shipments that correspond to the given order key.  If no shipment exists for the order,
        /// one will be created if autoCreate is true.  An OrderEntity will be attached to each shipment.
        /// </summary>
        public static List<ShipmentEntity> GetShipments(long orderID, bool createIfNone)
        {
            // Get all the shipments
            List<ShipmentEntity> shipments = DataProvider.GetRelatedEntities(orderID, EntityType.ShipmentEntity).Cast<ShipmentEntity>().ToList();

            // If there are none, create one
            if (shipments.Count == 0)
            {
                if (createIfNone)
                {
                    ShipmentEntity shipment = InternalCreateFirstShipment(orderID);
                    shipments.Add(shipment);
                }
            }
            else
            {
                // Get the order
                OrderEntity order = (OrderEntity) DataProvider.GetEntity(orderID);

                if (order == null)
                {
                    throw new SqlForeignKeyException("The order has been deleted.");
                }

                // Update the order to references all these shipments, and pull in custom's info
                foreach (ShipmentEntity shipment in shipments)
                {
                    shipment.Order = order;
                }
            }

            // Update the sibling data - we now know this is the most current list
            lock (siblingData)
            {
                siblingData[orderID] = shipments.Select(s => s.ShipmentID).OrderBy(k => k).ToList();
            }

            return shipments;
        }

        /// <summary>
        /// Create a new shipment for the order.
        /// </summary>
        public static ShipmentEntity CreateShipment(long orderID)
        {
            // First let's see if there are any shipments already for the order
            IEnumerable<ShipmentEntity> shipments = GetShipments(orderID, false);
            ShipmentEntity firstShipment = shipments.FirstOrDefault();

            // If there are none, then we can just create a new single shipment attached to the order,
            // and know that's all that needs attached
            return firstShipment == null ?
                InternalCreateFirstShipment(orderID) :
                CreateShipment(firstShipment);
        }

        /// <summary>
        /// Create a new shipment as a sibling of the given shipment
        /// </summary>
        public static ShipmentEntity CreateShipment(ShipmentEntity sibling) => CreateShipment(sibling.Order);

        /// <summary>
        /// Create what we know to be the first shipment for an order
        /// </summary>
        private static ShipmentEntity InternalCreateFirstShipment(long orderID)
        {
            // Get the order
            OrderEntity order = (OrderEntity) DataProvider.GetEntity(orderID);

            if (order == null)
            {
                throw new SqlForeignKeyException("The order has been deleted.");
            }

            return CreateShipment(order);
        }

        /// <summary>
        /// Create a shipment for the given order
        /// </summary>
        public static ShipmentEntity CreateShipment(OrderEntity order)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                return CreateShipment(order, lifetimeScope);
            }
        }

        /// <summary>
        /// Create a shipment for the given order.  The order\shipment reference is created between the two objects.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static ShipmentEntity CreateShipment(OrderEntity order, ILifetimeScope lifetimeScope)
        {
            lifetimeScope.Resolve<ISecurityContext>()
                .DemandPermission(PermissionType.ShipmentsCreateEditProcess, order.OrderID);

            // Create the shipment
            ShipmentEntity shipment = new ShipmentEntity();

            // It goes with the order
            shipment.Order = order;

            OrderUtility.PopulateOrderDetails(shipment);

            // Set some defaults
            shipment.ShipDate = lifetimeScope.Resolve<IDateTimeProvider>().Now.Date.AddHours(12);
            shipment.ShipmentType = (int) ShipmentTypeCode.None;
            shipment.Processed = false;
            shipment.Voided = false;
            shipment.ShipmentCost = 0;
            shipment.TrackingNumber = "";
            shipment.ResidentialDetermination = (int) ResidentialDeterminationType.CommercialIfCompany;
            shipment.ResidentialResult = true;
            shipment.ReturnShipment = false;
            shipment.Insurance = false;
            shipment.InsuranceProvider = (int) InsuranceProvider.ShipWorks;
            shipment.BestRateEvents = (int) BestRateEventTypes.None;
            shipment.ShipSenseStatus = (int) ShipSenseStatus.NotApplied;
            shipment.ShipSenseChangeSets = new XElement("ChangeSets").ToString();
            shipment.ShipSenseEntry = new byte[0];
            shipment.OnlineShipmentID = string.Empty;

            //TODO: Remove this once the profile copying is implemented.
            shipment.RequestedLabelFormat = (int) ThermalLanguage.None;

            // Set the initial weights
            shipment.ContentWeight = order.OrderItems.Sum(i => i.Quantity * i.Weight);
            shipment.TotalWeight = shipment.ContentWeight;

            // Set the rating billing info.
            shipment.BilledType = (int) BilledType.Unknown;
            shipment.BilledWeight = 0;

            // Content items aren't generated until they are needed
            shipment.CustomsGenerated = false;
            shipment.CustomsValue = 0;

            // Initialize the to address
            PersonAdapter.Copy(order, shipment, "Ship");
            AddressAdapter.Copy(order, shipment, "Ship");

            shipment.ShipAddressValidationError = order.ShipAddressValidationError;
            shipment.ShipAddressValidationStatus = order.ShipAddressValidationStatus;
            shipment.ShipAddressValidationSuggestionCount = order.ShipAddressValidationSuggestionCount;
            shipment.ShipResidentialStatus = order.ShipResidentialStatus;
            shipment.ShipPOBox = order.ShipPOBox;
            shipment.ShipUSTerritory = order.ShipUSTerritory;
            shipment.ShipMilitaryAddress = order.ShipMilitaryAddress;

            shipment.OriginOriginID = (int) ShipmentOriginSource.Store;

            // The from address will be dependent on the specific service type, but we'll default it to that of the store
            PersonAdapter.Copy(order.Store, "", shipment, "Origin");
            shipment.OriginFirstName = order.Store.StoreName;

            IShipmentTypeManager shipmentTypeManager = lifetimeScope.Resolve<IShipmentTypeManager>();

            ShipmentType shipmentType = shipmentTypeManager.InitialShipmentType(shipment);

            // Apply the determined shipment type
            shipment.ShipmentTypeCode = shipmentType.ShipmentTypeCode;

            // Apply the default values to the shipment
            shipmentType.ConfigureNewShipment(shipment);
            shipmentType.UpdateDynamicShipmentData(shipment);

            // Go ahead and create customs if needed
            lifetimeScope.Resolve<ICustomsManager>().GenerateCustomsItems(shipment);

            IEnumerable<ValidatedAddressEntity> addressSuggestions = lifetimeScope.Resolve<IValidatedAddressScope>()
                .LoadValidatedAddresses(shipment.OrderID, "Ship")
                .Select(EntityUtility.CloneAsNew);
            shipment.ValidatedAddress.AddRange(addressSuggestions);

            if (shipment.ShipSenseStatus != (int) ShipSenseStatus.NotApplied)
            {
                // Make sure the status is correct in case a rule/profile changed a shipment after ShipSense was applied
                KnowledgebaseEntry entry = new Knowledgebase().GetEntry(shipment.Order);
                shipment.ShipSenseStatus = entry.Matches(shipment) ? (int) ShipSenseStatus.Applied : (int) ShipSenseStatus.Overwritten;
            }

            // Explicitly save the shipment here to delete any entities in the Removed buckets of the
            // entity collections; after applying ShipSense (where customs items are first loaded in
            // this path), and entities were removed, they were still being persisted to the database.
            SaveShipment(shipment, lifetimeScope.Resolve<IOrderManager>(), shipmentTypeManager);

            return shipment;
        }

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        public static ShipmentEntity GetShipment(long shipmentID)
        {
            ShipmentEntity shipment = (ShipmentEntity) DataProvider.GetEntity(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Shipment {0} seems to be now deleted.", shipmentID);
                return null;
            }

            OrderEntity order = (OrderEntity) DataProvider.GetEntity(shipment.OrderID);
            if (order == null)
            {
                log.InfoFormat("Order {0} seems to be now deleted.", shipment.OrderID);
                return null;
            }

            shipment.Order = order;

            return shipment;
        }

        /// <summary>
        /// Refresh the data for the given shipment, including the carrier specific data.  The order and the other siblings are not touched.
        /// If the shipment has been deleted, an ObjectDeletedException is thrown.
        /// </summary>
        public static ShipmentEntity RefreshShipment(ShipmentEntity shipment)
        {
            if (shipment.DeletedFromDatabase)
            {
                throw new ObjectDeletedException();
            }

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Refresh the entity
                adapter.FetchEntity(shipment);

                // Check if its been deleted
                if (shipment.Fields.State == EntityState.Deleted ||
                    shipment.Fields.State == EntityState.OutOfSync)
                {
                    shipment.Fields.State = EntityState.Fetched;
                    shipment.DeletedFromDatabase = true;

                    throw new ObjectDeletedException();
                }

                // Refresh carrier specific
                ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
                shipmentType.LoadShipmentData(shipment, true);

                // Refresh customs (if it was loaded in the first place)
                if (shipment.CustomsItemsLoaded)
                {
                    CustomsManager.LoadCustomsItems(shipment, true, adapter);
                }

                adapter.Commit();
            }

            return shipment;
        }

        /// <summary>
        /// Creates a deep copy of a shipment, the resulting copy set to IsNew
        /// </summary>
        public static ShipmentEntity CreateShipmentCopy(ShipmentEntity shipment)
        {
            // load all carrier and customs data
            EnsureShipmentLoaded(shipment);

            // clone the entity tree
            ShipmentEntity clonedShipment = EntityUtility.CloneEntity(shipment, true);

            // No insurance policy yet.
            clonedShipment.InsurancePolicy = null;

            // this is now a new shipment to be inserted
            EntityUtility.MarkAsNew(clonedShipment);

            // all carrier data is New as well
            foreach (ShipmentCustomsItemEntity customsItem in clonedShipment.CustomsItems)
            {
                EntityUtility.MarkAsNew(customsItem);
            }

            clonedShipment.Processed = false;
            clonedShipment.ProcessedDate = null;
            clonedShipment.TrackingNumber = "";
            clonedShipment.Voided = false;
            clonedShipment.ActualLabelFormat = null;
            clonedShipment.ShipDate = DateTime.Now.Date.AddHours(12);
            clonedShipment.BestRateEvents = 0;
            clonedShipment.OnlineShipmentID = string.Empty;

            // Clear out post-processed data on a per shipment-type basis.
            ShipmentTypeManager.ShipmentTypes.ForEach(st => st.ClearDataForCopiedShipment(clonedShipment));

            return clonedShipment;
        }

        /// <summary>
        /// Save the given shipment.
        /// </summary>
        public static void SaveShipment(ShipmentEntity shipment)
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                SaveShipment(shipment,
                    lifetimeScope.Resolve<IOrderManager>(),
                    lifetimeScope.Resolve<IShipmentTypeManager>());
            }
        }

        /// <summary>
        /// Save the given shipment.
        /// </summary>
        [NDependIgnoreLongMethod]
        private static void SaveShipment(ShipmentEntity shipment, IOrderManager orderManager, IShipmentTypeManager shipmentTypeManager)
        {
            // Ensure the latest ShipSense data is recorded for this shipment before saving
            SaveShipSenseFieldsToShipment(shipment, orderManager, shipmentTypeManager);

            using (SqlAdapter adapter = SqlAdapter.Create(true))
            {
                bool rootDirty = shipment.IsDirty;

                // We only want the shipment on down saved... if the order was still attached, it would find
                // everything related to the order, including dirty sibling shipments.
                OrderEntity order = shipment.Order;
                shipment.Order = null;

                // Get all the recursive entities that have the potential to be saved
                List<IEntity2> saveList = new ObjectGraphUtils().ProduceTopologyOrderedList<IEntity2>(shipment);

                // Determine if any of them are dirty
                bool anyDirty = saveList.Any(e => e.IsDirty);

                // Check for any entities that need to be deleted
                List<IEntity2> deleteList = new List<IEntity2>();
                foreach (IEntity2 entity in saveList)
                {
                    foreach (IEntityCollection2 childCollection in entity.GetMemberEntityCollections())
                    {
                        if (childCollection.RemovedEntitiesTracker != null && childCollection.RemovedEntitiesTracker.Count > 0)
                        {
                            anyDirty = true;
                            deleteList.AddRange(childCollection.RemovedEntitiesTracker.Cast<IEntity2>().Where(e => !e.IsNew));

                            // Reset the tracker
                            childCollection.RemovedEntitiesTracker.Clear();
                        }
                    }
                }

                // If the root isn't dirty - but children are - we have to force the root to get dirty for concurrency
                if (!rootDirty && anyDirty)
                {
                    // Use a column we don't filter on so we don't falsely trigger filter recalculations.
                    shipment.Fields[(int) ShipmentFieldIndex.CustomsGenerated].IsChanged = true;
                    shipment.Fields.IsDirty = true;
                }

                // Recursively save, and see if anything was actually dirty
                try
                {
                    adapter.SaveAndRefetch(shipment);

                    // Delete everything that had been tracked to be deleted
                    foreach (IEntity2 entity in deleteList)
                    {
                        adapter.DeleteEntity(entity);
                    }
                }
                catch (ORMConcurrencyException ex)
                {
                    // Log this ORMConcurrencyException
                    LogExceptionUtility.LogOrmConcurrencyException(shipment, "Attempted to save shipment in ShippingManager.SaveShipment.", ex);

                    // See if we got this b\c the shipment was deleted
                    bool deleted = ShipmentCollection.GetCount(adapter, ShipmentFields.ShipmentID == shipment.ShipmentID) == 0;

                    if (deleted)
                    {
                        shipment.DeletedFromDatabase = true;

                        throw new ObjectDeletedException("The shipment has been deleted.", ex);
                    }
                    else
                    {
                        throw;
                    }
                }
                finally
                {
                    // Restore the order
                    shipment.Order = order;
                }

                adapter.Commit();
            }
        }

        /// <summary>
        /// Saves the ShipSense fields to shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private static void SaveShipSenseFieldsToShipment(ShipmentEntity shipment, IOrderManager orderManager,
            IShipmentTypeManager shipmentTypeManager)
        {
            if (!shipment.Processed)
            {
                // Ensure the order details are populated, so we get the correct hash key
                orderManager.PopulateOrderDetails(shipment);

                IEnumerable<IPackageAdapter> packageAdapters = shipmentTypeManager.Get(shipment).GetPackageAdapters(shipment);

                // Create a knowledge base entry that represents the current shipment/package and customs configuration
                KnowledgebaseEntry entry = new KnowledgebaseEntry();
                entry.ApplyFrom(packageAdapters, shipment.CustomsItems);

                // Now compress the entry so we can record the ShipSense data for this shipment; this will be used
                // for quickly comparing/updating the ShipSense status of unprocessed shipments as new shipments
                // get processed.
                Knowledgebase knowledgebase = new Knowledgebase();
                shipment.ShipSenseEntry = knowledgebase.CompressEntry(entry);
            }
        }

        /// <summary>
        /// Delete the specified shipment.  When deleting individual shipments this function must be used.  However, if deleting an entire order,
        /// its OK just to delete the order using the DeletionService without having to come through here for each shipment.
        /// </summary>
        public static void DeleteShipment(ShipmentEntity shipment)
        {
            UserSession.Security.DemandPermission(PermissionType.ShipmentsVoidDelete, shipment.OrderID);

            bool deleted;

            long shipmentID = shipment.ShipmentID;
            long orderID = shipment.OrderID;

            try
            {
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    adapter.DeleteEntity(shipment);

                    ValidatedAddressManager.DeleteExistingAddresses(adapter, shipmentID, "Ship");

                    adapter.Commit();
                }

                deleted = true;
            }
            catch (ORMConcurrencyException ex)
            {
                deleted = ShipmentCollection.GetCount(SqlAdapter.Default, ShipmentFields.ShipmentID == shipmentID) == 0;

                if (deleted)
                {
                    // It was already deleted, no problem
                    log.Info("The shipment was already deleted.", ex);
                }
            }

            if (deleted)
            {
                lock (siblingData)
                {
                    List<long> shipmentList = siblingData[orderID];
                    if (shipmentList != null)
                    {
                        shipmentList.Remove(shipmentID);
                    }
                }

                DataProvider.RemoveEntity(shipmentID);
                shipment.Order = null;

                Messenger.Current.Send(new ShipmentDeletedMessage(null, shipmentID));
            }
        }

        /// <summary>
        /// Get the sibling information for the given shipment
        /// </summary>
        public static ShipmentSiblingData GetSiblingData(ShipmentEntity shipment)
        {
            lock (siblingData)
            {
                List<long> shipmentList = siblingData[shipment.OrderID];

                // If it doesn't exist, we have to load it
                if (shipmentList == null || !shipmentList.Contains(shipment.ShipmentID))
                {
                    shipmentList = DataProvider.GetRelatedKeys(shipment.OrderID, EntityType.ShipmentEntity);
                    shipmentList.Sort();
                    siblingData[shipment.OrderID] = shipmentList;
                }

                return new ShipmentSiblingData(shipmentList.IndexOf(shipment.ShipmentID) + 1, shipmentList.Count);
            }
        }

        /// <summary>
        /// Get the service used. Returns blank if not processed or "(Deleted)" if the shipment has been deleted.
        /// </summary>
        public static string GetActualServiceUsed(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
            return GetServiceUsedInternal(shipment, shipmentType, shipmentType.GetServiceDescription);
        }

        /// <summary>
        /// Get the service used overridden to ensure compatibility with marketplaces
        /// </summary>
        /// <remarks>
        /// Override one off service types that are shipworks specific to
        /// their more widely known counterparts
        /// </remarks>
        public static string GetOverriddenSerivceUsed(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
            return GetServiceUsedInternal(shipment, shipmentType, shipmentType.GetOveriddenServiceDescription);
        }

        /// <summary>
        /// Gets the service used using the given method
        /// </summary>
        private static string GetServiceUsedInternal(ShipmentEntity shipment, ShipmentType shipmentType, Func<ShipmentEntity, string> descriptionFunc)
        {
            if (!shipment.Processed)
            {
                return "";
            }

            try
            {
                shipmentType.LoadShipmentData(shipment, false);
            }
            catch (ObjectDeletedException)
            {
                return "(Deleted)";
            }
            catch (SqlForeignKeyException)
            {
                return "(Deleted)";
            }

            try
            {
                return descriptionFunc(shipment);
            }
            catch (NotFoundException ex)
            {
                log.Warn(ex);

                return "Unknown Service";
            }
        }

        /// <summary>
        /// Get the carrier name for the given shipment type
        /// </summary>
        public static string GetCarrierName(ShipmentTypeCode shipmentTypeCode)
        {
            if (ShipmentTypeManager.IsFedEx(shipmentTypeCode))
            {
                return "FedEx";
            }
            else if (ShipmentTypeManager.IsUps(shipmentTypeCode))
            {
                return "UPS";
            }
            else if (ShipmentTypeManager.IsPostal(shipmentTypeCode))
            {
                return "USPS";
            }
            else if (shipmentTypeCode == ShipmentTypeCode.Other)
            {
                return "Other";
            }
            else if (shipmentTypeCode == ShipmentTypeCode.OnTrac)
            {
                return "OnTrac";
            }
            else if (shipmentTypeCode == ShipmentTypeCode.iParcel)
            {
                return "i-parcel";
            }
            else if (shipmentTypeCode == ShipmentTypeCode.BestRate)
            {
                return "Best Rate";
            }
            else if (shipmentTypeCode == ShipmentTypeCode.Amazon)
            {
                return "Amazon";
            }

            Debug.Fail("Unhandled shipping type in GetCarrierName");

            return "Other";
        }

        /// <summary>
        /// Get a description for the 'Other' carrier
        /// </summary>
        public static CarrierDescription GetOtherCarrierDescription(ShipmentEntity shipment)
        {
            return new CarrierDescription(shipment);
        }

        /// <summary>
        /// Ensure the carrier-specific data for the shipment exists, like the associated FedEx table row.  Also ensures
        /// that the custom's data is loaded.  Can throw a SqlForeignKeyException  if the shipment's order has been deleted,
        /// or ObjectDeletedException if the shipment itself has been deleted.
        /// </summary>
        public static void EnsureShipmentLoaded(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

            // Ask the concrete type to ensure the carrier specific data
            try
            {
                shipmentType.LoadShipmentData(shipment, false);

                using (SqlAdapter adapter = new SqlAdapter())
                {
                    CustomsManager.LoadCustomsItems(shipment, false, adapter);
                }
            }
            catch (SqlForeignKeyException)
            {
                shipment.DeletedFromDatabase = true;
                throw;
            }
        }

        /// <summary>
        /// Removes the specified shipment from the cache
        /// </summary>
        /// <param name="shipment">Shipment that should be removed from cache</param>
        /// <returns></returns>
        public static void RemoveShipmentFromRatesCache(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                return;
            }

            // Because this is coming from the rate control, and the only thing that causes rate changes from the rate control
            // is the Express1 promo footer, we need to remove the shipment from the cache before we get rates
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                if (lifetimeScope.IsRegisteredWithKey<IRateHashingService>((ShipmentTypeCode) shipment.ShipmentType))
                {
                    IRateHashingService rateHashingService = lifetimeScope.ResolveKeyed<IRateHashingService>((ShipmentTypeCode) shipment.ShipmentType);

                    string cacheHash = rateHashingService.GetRatingHash(shipment);
                    RateCache.Instance.Remove(cacheHash);
                }
            }
        }

        /// <summary>
        /// Get rates for the given shipment using the appropriate ShipmentType
        /// </summary>
        public static RateGroup GetRates(ShipmentEntity shipment, ShipmentType shipmentType)
        {
            // We're going to confirm the shipping address with the store as some stores may change
            // the shipping address depending on the shipping program being used (such as eBay's
            // Global Shipping Program), so we want to get rates for the location the package will be shipped
            // We want to retain the buyer's address on the original shipment object, so we're going to use
            // a cloned shipment to confirm the shipping address with the store. This way the original
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                return lifetimeScope.Resolve<IRatesRetriever>().GetRates(shipment, shipmentType).Value;
            }
        }

        /// <summary>
        /// Track the given shipment using the appropriate ShipmentType for the shipment
        /// </summary>
        public static TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            if (!shipment.Processed)
            {
                throw new ShippingException("The shipment has not been processed.");
            }

            if (shipment.Voided)
            {
                throw new ShippingException("The shipment has been voided.");
            }

            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

            TrackingResult result = shipmentType.TrackShipment(shipment);

            return result;
        }

        /// <summary>
        /// Void the given shipment.  If the shipment is already voided, then no action is taken and no error is reported.  The fact that
        /// it was voided is logged to tango.
        /// </summary>
        [NDependIgnoreLongMethod]
        public static ShipmentEntity VoidShipment(long shipmentID)
        {
            UserSession.Security.DemandPermission(PermissionType.ShipmentsVoidDelete, shipmentID);

            ShipmentEntity shipment = null;

            try
            {
                // Ensure's we are the only one who processes this shipment, if other ShipWorks are running
                using (SqlEntityLock processLock = new SqlEntityLock(shipmentID, "Process Shipment"))
                {
                    shipment = GetShipment(shipmentID);

                    if (shipment == null)
                    {
                        throw new ObjectDeletedException();
                    }

                    // If it's already voided, its all good
                    if (shipment.Voided)
                    {
                        return shipment;
                    }

                    StoreEntity store = StoreManager.GetStore(shipment.Order.StoreID);
                    if (store == null)
                    {
                        throw new ShippingException("The store the shipment was in has been deleted.");
                    }

                    // Get the ShipmentType instance
                    ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

                    // Ensure the carrier specific data has been loaded in case the shipment type needs it for voiding
                    EnsureShipmentLoaded(shipment);

                    InsureShipException voidInsuranceException = null;

                    // Transacted
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                        {
                            if (lifetimeScope.IsRegisteredWithKey<ILabelService>((ShipmentTypeCode) shipment.ShipmentType))
                            {
                                ILabelService labelService =
                                    lifetimeScope.ResolveKeyed<ILabelService>((ShipmentTypeCode) shipment.ShipmentType);

                                labelService.Void(shipment);
                            }

                            shipment.Voided = true;
                            shipment.VoidedDate = DateTime.UtcNow;
                            shipment.VoidedUserID = UserSession.User.UserID;
                            shipment.VoidedComputerID = UserSession.Computer.ComputerID;

                            adapter.SaveAndRefetch(shipment);

                            if (lifetimeScope.Resolve<IInsureShipService>().IsInsuredByInsureShip(shipment))
                            {
                                log.InfoFormat("Shipment {0}  - Void Shipment Start", shipment.ShipmentID);
                                InsureShipPolicy insureShipPolicy = new InsureShipPolicy(TangoWebClient.GetInsureShipAffiliate(store));

                                try
                                {
                                    if (shipment.InsurancePolicy == null)
                                    {
                                        // Make sure the insurance policy has been loaded prior to voiding the policy
                                        ShipmentTypeDataService.LoadInsuranceData(shipment);
                                    }

                                    insureShipPolicy.Void(shipment);
                                }
                                catch (InsureShipException ex)
                                {
                                    // If there was an error voiding the insurance policy, save the exception so we can re-throw at the
                                    // very end of the voiding process to ensure that any other code for voiding can run
                                    voidInsuranceException = ex;
                                }

                                log.InfoFormat("Shipment {0}  - Void Shipment Complete", shipment.ShipmentID);
                            }

                            // Dispatch the shipment voided event
                            ActionDispatcher.DispatchShipmentVoided(shipment, adapter);

                            adapter.Commit();
                        }
                    }

                    // Void the shipment in tango
                    using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                    {
                        ITangoWebClient tangoWebClient = lifetimeScope.Resolve<ITangoWebClientFactory>().CreateWebClient();
                        tangoWebClient.VoidShipment(store, shipment);
                    }

                    // Re-throw the insurance exception if there was one
                    if (voidInsuranceException != null)
                    {
                        string message = string.Format("ShipWorks was not able to void the insurance policy with this shipment. Contact InsureShip at {0} to void the policy.\r\n\r\n{1}",
                            new InsureShipSettings().InsureShipPhoneNumber,
                            voidInsuranceException.Message);

                        throw new ShippingException(message, voidInsuranceException);
                    }
                }
            }
            catch (SqlAppResourceLockException ex)
            {
                log.InfoFormat("Could not obtain lock for void shipment {0}", shipmentID);

                throw new ShippingException("The shipment was being processed or voided on another computer.", ex);
            }
            catch (ShipWorksLicenseException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
            catch (TangoException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }

            return shipment;
        }

        /// <summary>
        /// A utility method to gets the overridden store shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>The store-overridden shipment entity.</returns>
        public static ShipmentEntity GetOverriddenStoreShipment(ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (shipment.Order == null)
            {
                shipment.Order = (OrderEntity) DataProvider.GetEntity(shipment.OrderID);
            }

            StoreEntity storeEntity = shipment.Order?.Store ?? StoreManager.GetStore(shipment.Order.StoreID);
            StoreType storeType = StoreTypeManager.GetType(storeEntity);

            // Clone the shipment and check with the store in case anything was overridden. We clone
            // the original shipment so that the shipment is not altered and inadvertently saved
            // to the database.
            if (!storeType.WillOverrideShipmentDetailsChangeShipment(shipment))
            {
                return shipment;
            }

            ShipmentEntity overriddenShipment = EntityUtility.CloneEntity(shipment);
            storeType.OverrideShipmentDetails(overriddenShipment);

            return overriddenShipment;
        }

        /// <summary>
        /// Checks the license before processing a shipment
        /// </summary>
        /// <param name="storeEntity">The store entity.</param>
        private static void CheckLicense(StoreEntity storeEntity)
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                ILicenseService licenseService = scope.Resolve<ILicenseService>();

                ILicense license = licenseService.GetLicense(storeEntity);

                license.Refresh();
                if (license.IsDisabled)
                {
                    throw new ShippingException(license.DisabledReason);
                }

                try
                {
                    license.EnforceCapabilities(EnforcementContext.CreateLabel);
                }
                catch (ShipWorksLicenseException ex)
                {
                    throw new ShippingException(ex.Message);
                }
            }
        }

        /// <summary>
        /// Validate that the given store is licensed to ship.  If there is an error its stored in licenseCheckCache.  If there is already
        /// an error for the store in licenseCheckCache, then its reused and no trip to tango is taken.
        /// </summary>
        public static Exception ValidateLicense(StoreEntity store, IDictionary<long, Exception> licenseCheckCache)
        {
            long storeID = store.StoreID;

            Exception error;
            if (licenseCheckCache.TryGetValue(storeID, out error))
            {
                // If there was an error from a previous check, throw it again
                if (error != null)
                {
                    return error;
                }
            }
            else
            {
                try
                {
                    CheckLicense(store);
                    licenseCheckCache[storeID] = null;
                }
                catch (ShippingException ex)
                {
                    licenseCheckCache[storeID] = ex;
                    return ex;
                }
            }

            return null;
        }

        /// <summary>
        /// Indicates if the shipment type of the given type code has gone through the full setup wizard \ configuration
        /// </summary>
        public static bool IsShipmentTypeConfigured(ShipmentTypeCode shipmentTypeCode)
        {
            if (shipmentTypeCode == ShipmentTypeCode.None || shipmentTypeCode == ShipmentTypeCode.BestRate)
            {
                return true;
            }

            return ShippingSettings.FetchReadOnly().ConfiguredTypes.Contains(shipmentTypeCode);
        }

        /// <summary>
        /// Indicates if the shipment type has been activated (utilized) by the user and ShipWorks should load and display carrier specific data for the shipment type.
        /// It may not yet be totally "Configured" in the case of a 2x upgrade where the user skips account migration.
        /// </summary>
        public static bool IsShipmentTypeActivated(ShipmentTypeCode shipmentTypeCode)
        {
            if (shipmentTypeCode == ShipmentTypeCode.None || shipmentTypeCode == ShipmentTypeCode.BestRate)
            {
                return true;
            }

            return ShippingSettings.FetchReadOnly().ActivatedTypes.Contains(shipmentTypeCode);
        }

        /// <summary>
        /// Indicates if the given shipment type code is enabled for selection in the shipping window
        /// </summary>
        public static bool IsShipmentTypeEnabled(ShipmentTypeCode shipmentTypeCode)
        {
            return !ShippingSettings.FetchReadOnly().ExcludedTypes.Contains(shipmentTypeCode);
        }

        /// <summary>
        /// Calculates the expected delivery date
        /// </summary>
        public static DateTime? CalculateExpectedDeliveryDate(int? guaranteedDaysToDelivery, params DayOfWeek[] excludedDays)
        {
            return CalculateExpectedDeliveryDate(guaranteedDaysToDelivery, DateTime.Today, excludedDays);
        }

        /// <summary>
        /// Calculates the expected delivery date
        /// </summary>
        public static DateTime? CalculateExpectedDeliveryDate(int? guaranteedDaysToDelivery, DateTime shipmentDate, params DayOfWeek[] excludedDays)
        {
            if (!guaranteedDaysToDelivery.HasValue)
            {
                return null;
            }

            DateTime incrementingDate = shipmentDate;

            while (guaranteedDaysToDelivery > 0)
            {
                incrementingDate = incrementingDate.AddDays(1);

                if (!excludedDays.Contains(incrementingDate.DayOfWeek))
                {
                    guaranteedDaysToDelivery--;
                }
            }

            return incrementingDate;
        }

        /// <summary>
        /// Get a formatted arrival date for a shipment
        /// </summary>
        public static string GetArrivalDescription(DateTime localArrival)
        {
            return string.Format("({0} {1})", localArrival.DayOfWeek, localArrival.ToString("h:mm tt"));
        }

        /// <summary>
        /// Gets the service level based on number of days in transit.
        /// </summary>
        public static ServiceLevelType GetServiceLevel(int transitDays)
        {
            if (transitDays < 0)
            {
                return ServiceLevelType.Anytime;
            }
            if (transitDays <= 1)
            {
                return ServiceLevelType.OneDay;
            }
            if (transitDays == 2)
            {
                return ServiceLevelType.TwoDays;
            }
            if (transitDays == 3)
            {
                return ServiceLevelType.ThreeDays;
            }
            if (transitDays <= 7)
            {
                return ServiceLevelType.FourToSevenDays;
            }
            return ServiceLevelType.Anytime;
        }

        /// <summary>
        /// Update the label format of any unprocessed shipment with the given shipment type code
        /// </summary>
        public static void UpdateLabelFormatOfUnprocessedShipments(ShipmentTypeCode shipmentTypeCode)
        {
            ShippingProfileEntity defaultProfile = ShippingProfileManager.GetDefaultProfile(shipmentTypeCode);

            if (defaultProfile?.RequestedLabelFormat == null)
            {
                // We don't need to do anything if the default profile is somehow null
                return;
            }

            RelationPredicateBucket bucket = GetUnprocessedShipmentsOfTypeBucket(shipmentTypeCode);
            RelationPredicateBucket carrierSpecificUpdateBucket = CreateUnprocessedShipmentsBucket();

            using (SqlAdapter adapter = new SqlAdapter())
            {
                int newLabelFormat = defaultProfile.RequestedLabelFormat.Value;

                adapter.UpdateEntitiesDirectly(new ShipmentEntity { RequestedLabelFormat = newLabelFormat }, bucket);
                ShipmentTypeManager.GetType(shipmentTypeCode).UpdateLabelFormatOfUnprocessedShipments(adapter, newLabelFormat, carrierSpecificUpdateBucket);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Create a predicate bucket for unprocessed shipments
        /// </summary>
        private static RelationPredicateBucket CreateUnprocessedShipmentsBucket()
        {
            RelationPredicateBucket carrierSpecificUpdateBucket = new RelationPredicateBucket();
            carrierSpecificUpdateBucket.PredicateExpression.Add(ShipmentFields.Processed == false);
            return carrierSpecificUpdateBucket;
        }

        /// <summary>
        /// Create a predicate bucket for unprocessed shipments of the given shipment type
        /// </summary>
        private static RelationPredicateBucket GetUnprocessedShipmentsOfTypeBucket(ShipmentTypeCode shipmentTypeCode)
        {
            RelationPredicateBucket bucket = CreateUnprocessedShipmentsBucket();
            bucket.PredicateExpression.AddWithAnd(ShipmentFields.ShipmentType == (int) shipmentTypeCode);
            return bucket;
        }
    }
}
