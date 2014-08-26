﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Controls;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.BestRate.RateGroupFiltering;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.BestRate;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using ShipWorks.Users;
using ShipWorks.Data.Grid;
using System.Diagnostics;
using log4net;
using ShipWorks.Data.Model;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Actions;
using SD.LLBLGen.Pro.ORMSupportClasses;
using Interapptive.Shared.Utility;
using System.Data;
using ShipWorks.Users.Security;
using ShipWorks.Data.Utility;
using ShipWorks.Stores;
using ShipWorks.ApplicationCore;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Editing;
using ShipWorks.Filters;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.ApplicationCore.Licensing;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Business;
using ShipWorks.Users.Audit;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Templates.Tokens;
using ShipWorks.Editions;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.ShipSense.Packaging;
using System.Xml.Linq;
using ShipWorks.Stores.Content;
using ShipWorks.Shipping.ShipSense.Hashing;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Utility class for working with shipments
    /// </summary>
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
                OrderEntity order = (OrderEntity)DataProvider.GetEntity(orderID);

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
            List<ShipmentEntity> shipments = GetShipments(orderID, false);

            // If there are none, then we can just create a new single shipment attached to the order, and know that's all that needs attached
            if (shipments.Count == 0)
            {
                return InternalCreateFirstShipment(orderID);
            }
            else
            {
                // Create a new shipment from one of the siblings
                return CreateShipment(shipments[0]);
            }
        }

        /// <summary>
        /// Create a new shipment as a sibling of the given shipment
        /// </summary>
        public static ShipmentEntity CreateShipment(ShipmentEntity sibling)
        {
            return InternalCreateShipment(sibling.Order);
        }

        /// <summary>
        /// Create what we konw to be the first shipment for an order
        /// </summary>
        private static ShipmentEntity InternalCreateFirstShipment(long orderID)
        {
            // Get the order 
            OrderEntity order = (OrderEntity)DataProvider.GetEntity(orderID);

            if (order == null)
            {
                throw new SqlForeignKeyException("The order has been deleted.");
            }

            return InternalCreateShipment(order);
        }

        /// <summary>
        /// Create a shipment for the given order.  The order\shipment reference is created between the two objects.
        /// </summary>
        private static ShipmentEntity InternalCreateShipment(OrderEntity order)
        {
            UserSession.Security.DemandPermission(PermissionType.ShipmentsCreateEditProcess, order.OrderID);

            // Create the shipment
            ShipmentEntity shipment = new ShipmentEntity();

            // It goes with the order
            shipment.Order = order;

            // Set some defaults
            shipment.ShipDate = DateTime.Now.Date.AddHours(12);
            shipment.ShipmentType = (int)ShipmentTypeCode.None;
            shipment.Processed = false;
            shipment.Voided = false;
            shipment.ShipmentCost = 0;
            shipment.TrackingNumber = "";
            shipment.ResidentialDetermination = (int)ResidentialDeterminationType.CommercialIfCompany;
            shipment.ResidentialResult = true;
            shipment.ReturnShipment = false;
            shipment.Insurance = false;
            shipment.InsuranceProvider = (int)InsuranceProvider.ShipWorks;
            shipment.BestRateEvents = (int)BestRateEventTypes.None;
            shipment.ShipSenseStatus = (int)ShipSenseStatus.NotApplied;
            shipment.ShipSenseChangeSets = new XElement("ChangeSets").ToString();
            shipment.ShipSenseEntry = new byte[0];

            // We have to get the order items to calculate the weight
            List<EntityBase2> orderItems = DataProvider.GetRelatedEntities(order.OrderID, EntityType.OrderItemEntity);

            // Set the initial weights
            shipment.ContentWeight = orderItems.OfType<OrderItemEntity>().Sum(i => i.Quantity * i.Weight);
            shipment.TotalWeight = shipment.ContentWeight;

            // Content items aren't generated until they are needed
            shipment.CustomsGenerated = false;
            shipment.CustomsValue = 0;

            // Initialize the to address
            PersonAdapter.Copy(order, shipment, "Ship");
            AddressAdapter.Copy(order, shipment, "Ship");

            shipment.ShipAddressValidationError = order.ShipAddressValidationError;
            shipment.ShipAddressValidationStatus = order.ShipAddressValidationStatus;
            shipment.ShipAddressValidationSuggestionCount = order.ShipAddressValidationSuggestionCount;
            shipment.ShipResidentialStatus = (int) ValidationDetailStatusType.Unknown;
            shipment.ShipPOBox = (int) ValidationDetailStatusType.Unknown;
            shipment.ShipUSTerritory = (int) ValidationDetailStatusType.Unknown;
            shipment.ShipMilitaryAddress = (int) ValidationDetailStatusType.Unknown;

            shipment.OriginOriginID = (int)ShipmentOriginSource.Store;

            // The from address will be dependent on the specific service type, but we'll default it to that of the store
            StoreEntity store = StoreManager.GetStore(order.StoreID);
            PersonAdapter.Copy(store, "", shipment, "Origin");
            shipment.OriginFirstName = store.StoreName;

            ShipmentTypeCode shipmentTypeCode = DetermineInitialShipmentType(order);

            // Save the record
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Apply the determined shipment type
                shipment.ShipmentType = (int)shipmentTypeCode;

                // Save the shipment
                adapter.SaveAndRefetch(shipment);

                // Apply the default values to the shipment
                ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
                shipmentType.LoadShipmentData(shipment, false);
                shipmentType.UpdateDynamicShipmentData(shipment);

                adapter.SaveAndRefetch(shipment);

                // Go ahead and create customs if needed
                CustomsManager.LoadCustomsItems(shipment, false);

                ValidatedAddressManager.CopyValidatedAddresses(adapter, order.OrderID, "Ship", shipment.ShipmentID, "Ship");

                adapter.Commit();
            }

            if (shipment.ShipSenseStatus != (int)ShipSenseStatus.NotApplied)
            {
                // Make sure the status is correct in case a rule/profile changed a shipment after ShipSense was applied
                KnowledgebaseEntry entry = new Knowledgebase().GetEntry(shipment.Order);
                shipment.ShipSenseStatus = entry.Matches(shipment) ? (int)ShipSenseStatus.Applied : (int)ShipSenseStatus.Overwritten;
            }

            // Explicitly save the shipment here to delete any entities in the Removed buckets of the 
            // entity collections; after applying ShipSense (where customs items are first loaded in 
            // this path), and entities were removed, they were still being persisted to the database.
            SaveShipment(shipment);

            lock (siblingData)
            {
                List<long> shipmentList = siblingData[(long)shipment.Fields[(int)ShipmentFieldIndex.OrderID].CurrentValue];
                if (shipmentList != null)
                {
                    shipmentList.Add(shipment.ShipmentID);
                }
            }

            return shipment;
        }

        /// <summary>
        /// Determine what the initial shipment type for the given order should be, given the shipping settings rules
        /// </summary>
        private static ShipmentTypeCode DetermineInitialShipmentType(OrderEntity order)
        {
            // Go through each rule and see if we can find one that is applicable
            foreach (ShippingProviderRuleEntity rule in ShippingProviderRuleManager.GetRules())
            {
                long? filterContentID = FilterHelper.GetFilterNodeContentID(rule.FilterNodeID);
                if (filterContentID != null)
                {
                    if (FilterHelper.IsObjectInFilterContent(order.OrderID, filterContentID.Value))
                    {
                        return (ShipmentTypeCode)rule.ShipmentType;
                    }
                }
            }

            return (ShipmentTypeCode)ShippingSettings.Fetch().DefaultType;
        }

        /// <summary>
        /// Get the shipment of the specified ID.  The Order will be attached.
        /// </summary>
        public static ShipmentEntity GetShipment(long shipmentID)
        {
            ShipmentEntity shipment = (ShipmentEntity)DataProvider.GetEntity(shipmentID);
            if (shipment == null)
            {
                log.InfoFormat("Shipment {0} seems to be now deleted.", shipmentID);
                return null;
            }

            OrderEntity order = (OrderEntity)DataProvider.GetEntity(shipment.OrderID);
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
        public static void RefreshShipment(ShipmentEntity shipment)
        {
            if (shipment.DeletedFromDatabase)
            {
                throw new ObjectDeletedException();
            }

            using (SqlAdapter adpater = new SqlAdapter(true))
            {
                // Refresh the entity
                adpater.FetchEntity(shipment);

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
                    CustomsManager.LoadCustomsItems(shipment, true);
                }

                adpater.Commit();
            }
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
            clonedShipment.ThermalType = null;
            clonedShipment.ShipDate = DateTime.Now.Date.AddHours(12);
            clonedShipment.BestRateEvents = 0;

            // Clear out post-processed data on a per shipment-type basis.
            ShipmentTypeManager.ShipmentTypes.ForEach(st => st.ClearDataForCopiedShipment(clonedShipment));

            return clonedShipment;
        }

        /// <summary>
        /// Save the given shipment.
        /// </summary>
        public static void SaveShipment(ShipmentEntity shipment)
        {
            // Ensure the latest ShipSense data is recorded for this shipment before saving
            SaveShipSenseFieldsToShipment(shipment);

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                bool rootDirty = shipment.IsDirty;

                // We only wan't the shipment on down saved... if the order was still attached, it would find
                // everything related to the order, including dirty sibling shipments.
                OrderEntity order = shipment.Order;
                shipment.Order = null;

                // Get all the recursive entities that have the potential to be saved
                List<IEntity2> saveList = new ObjectGraphUtils().ProduceTopologyOrderedList(shipment);

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
                    shipment.Fields[(int)ShipmentFieldIndex.CustomsGenerated].IsChanged = true;
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
        private static void SaveShipSenseFieldsToShipment(ShipmentEntity shipment)
        {
            if (!shipment.Processed)
            {
                // Ensure the order details are populated, so we get the correct hash key
                OrderUtility.PopulateOrderDetails(shipment);

                IEnumerable<IPackageAdapter> packageAdapters = ShipmentTypeManager.GetType(shipment).GetPackageAdapters(shipment);

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
        public static string GetServiceUsed(ShipmentEntity shipment)
        {
            if (!shipment.Processed)
            {
                return "";
            }

            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

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
                return shipmentType.GetServiceDescription(shipment);
            }
            catch (NotFoundException ex)
            {
                log.Warn(ex);

                return "Unknown Service";
            }
        }

        /// <summary>
        /// Get the carrier name for the given shipemnt type
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
            else if (shipmentTypeCode == ShipmentTypeCode.EquaShip)
            {
                return "EquaShip";
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

            Debug.Fail("Unhandled shipping type in GetCarrierName");

            return "Other";
        }

        /// <summary>
        /// Get the carrier name for the given free text carrier name.  i.e. "U S P S", "Fed Ex", "FedEx"
        /// </summary>
        /// <returns>
        /// If freeTextCarrierName can be parsed, "UPS", "USPS", or "FedEx" will be returned.
        /// Otherwise, freeTextCarrierName will be returned.
        /// </returns>
        public static string GetCarrierName(string freeTextCarrierName)
        {
            // Strip out any characters that aren't in UPS, FedEx, or USPS
            string parsedCarrierName = Regex.Replace(freeTextCarrierName, "[^fedxupsFEDXUPS]", "");

            // See if this is UPS, USPS, or FedEx
            if (parsedCarrierName.IndexOf("ups", 0, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "UPS";
            }
            else if (parsedCarrierName.IndexOf("usps", 0, StringComparison.OrdinalIgnoreCase) >= 0
                 || freeTextCarrierName.IndexOf("postal", 0, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "USPS";
            }
            else if (parsedCarrierName.IndexOf("fedex", 0, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return "FedEx";
            }

            return freeTextCarrierName;
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

                CustomsManager.LoadCustomsItems(shipment, false);
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
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
            string cacheHash = shipmentType.GetRatingHash(shipment);
            RateCache.Instance.Remove(cacheHash);
        }

        /// <summary>
        /// Get rates for the given shipment using the appropriate ShipmentType
        /// </summary>
        public static RateGroup GetRates(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
            return GetRates(shipment, shipmentType);
        }

        /// <summary>
        /// Get rates for the given shipment using the appropriate ShipmentType
        /// </summary>
        public static RateGroup GetRates(ShipmentEntity shipment, ShipmentType shipmentType)
        {
            // Ensure data is valid and up-to-date
            shipmentType.UpdateDynamicShipmentData(shipment);

            // We're going to confirm the shipping address with the store as some stores may change 
            // the shipping address depending on the shipping program being used (such as eBay's 
            // Global Shipping Program), so we want to get rates for the location the package will be shipped                

            // We want to retain the buyer's address on the original shipment object, so we're going to use 
            // a cloned shipment to confirm the shipping address with the store. This way the original 
            // shipment is not altered and persisted to the database if the store alters the address
            ShipmentEntity clonedShipment = EntityUtility.CloneEntity(shipment);
            OrderHeader orderHeader = DataProvider.GetOrderHeader(clonedShipment.OrderID);

            // Determine residential status
            if (shipmentType.IsResidentialStatusRequired(clonedShipment))
            {
                clonedShipment.ResidentialResult = ResidentialDeterminationService.DetermineResidentialAddress(clonedShipment);
            }

            // Confirm the address of the cloned shipment with the store giving it a chance to inspect/alter the shipping address
            StoreType storeType = StoreTypeManager.GetType(StoreManager.GetStore(orderHeader.StoreID));
            storeType.OverrideShipmentDetails(clonedShipment);

            // Use the cloned shipment with the potentially adjusted shipping address to get the rates
            RateGroup rateResults = shipmentType.GetRates(clonedShipment);

            // Copy back any best rate events that were set on the clone
            shipment.BestRateEvents |= clonedShipment.BestRateEvents;

            return rateResults;
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

            // Marke sure the type is setup - its possible it's not in the case of upgrading from V2
            if (!IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode))
            {
                throw new ShippingException(String.Format("The '{0}' shipping provider was migrated from ShipWorks 2, and has not yet been configured for ShipWorks 3.", shipmentType.ShipmentTypeName));
            }

            TrackingResult result = shipmentType.TrackShipment(shipment);

            return result;
        }

        /// <summary>
        /// Void the given shipment.  If the shipment is already voided, then no action is taken and no error is reported.  The fact that
        /// it was voided is logged to tango.
        /// </summary>
        public static void VoidShipment(long shipmentID)
        {
            UserSession.Security.DemandPermission(PermissionType.ShipmentsVoidDelete, shipmentID);

            try
            {
                // Ensure's we are the only one who processes this shipment, if other ShipWorks are running
                using (SqlEntityLock processLock = new SqlEntityLock(shipmentID, "Process Shipment"))
                {
                    ShipmentEntity shipment = GetShipment(shipmentID);

                    if (shipment == null)
                    {
                        throw new ObjectDeletedException();
                    }

                    // If it's already voided, its all good
                    if (shipment.Voided)
                    {
                        return;
                    }

                    StoreEntity store = StoreManager.GetStore(shipment.Order.StoreID);
                    if (store == null)
                    {
                        throw new ShippingException("The store the shipment was in has been deleted.");
                    }

                    // Get the ShipmentType instance
                    ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

                    // Marke sure the type is setup - its possible it's not in the case of upgrading from V2
                    if (!IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode))
                    {
                        throw new ShippingException(String.Format("The '{0}' shipping provider was migrated from ShipWorks 2, and has not yet been configured for ShipWorks 3.", shipmentType.ShipmentTypeName));
                    }

                    // Ensure the carrier specific data has been loaded in case the shipment type needs it for voiding
                    EnsureShipmentLoaded(shipment);

                    // Transacted
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        shipmentType.VoidShipment(shipment);

                        shipment.Voided = true;
                        shipment.VoidedDate = DateTime.UtcNow;
                        shipment.VoidedUserID = UserSession.User.UserID;
                        shipment.VoidedComputerID = UserSession.Computer.ComputerID;

                        adapter.SaveAndRefetch(shipment);

                        // Dispatch the shipment voided event
                        ActionDispatcher.DispatchShipmentVoided(shipment, adapter);

                        adapter.Commit();
                    }

                    // Void the shipment in tango
                    TangoWebClient.VoidShipment(store, shipment);
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
                shipment.Order = (OrderEntity)DataProvider.GetEntity(shipment.OrderID);
            }

            StoreEntity storeEntity = StoreManager.GetStore(shipment.Order.StoreID);
            StoreType storeType = StoreTypeManager.GetType(storeEntity);

            // Clone the shipment and check with the store in case anything was overridden. We clone
            // the original shipment so that the shipment is not altered and inadvertently saved
            // to the database.
            ShipmentEntity overriddenShipment = EntityUtility.CloneEntity(shipment);
            storeType.OverrideShipmentDetails(overriddenShipment);

            return overriddenShipment;
        }

        /// <summary>
        /// Processes the shipment.
        /// </summary>
        public static void ProcessShipment(long shipmentID, Dictionary<long, Exception> licenseCheckCache, Func<CounterRatesProcessingArgs, DialogResult> counterRatesProcessing, RateResult selectedRate)
        {
            log.InfoFormat("Shipment {0}  - Process Start", shipmentID);

            UserSession.Security.DemandPermission(PermissionType.ShipmentsCreateEditProcess, shipmentID);

            try
            {
                // Ensure's we are the only one who processes this shipment, if other ShipWorks are running
                using (new SqlEntityLock(shipmentID, "Process Shipment"))
                {
                    ShipmentEntity shipment = GetShipment(shipmentID);

                    if (shipment == null)
                    {
                        throw new ObjectDeletedException();
                    }

                    if (shipment.Processed)
                    {
                        throw new ShipmentAlreadyProcessedException("The shipment has already been processed.");
                    }
                     

                    StoreEntity storeEntity = StoreManager.GetStore(shipment.Order.StoreID);
                    if (storeEntity == null)
                    {
                        throw new ShippingException("The store the shipment was in has been deleted.");
                    }

                    // Get the ShipmentType instance
                    ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);
                    List<ShipmentEntity> shipmentsToTryToProcess = shipmentType.PreProcess(shipment, counterRatesProcessing, selectedRate);

                    // A null value returned from the pre-process method means the user has opted to not continue 
                    // processing after a counter rate was selected as the best rate, so the processing of the shipment should be aborted
                    if (shipmentsToTryToProcess == null)
                    {
                        return;
                    }

                    bool success = false;
                    ShippingException lastException = null;
                    ShipmentEntity processedShipment = null;

                    foreach (ShipmentEntity shipmentToTry in shipmentsToTryToProcess)
                    {
                        try
                        {
                            using (SqlAdapter adapter = new SqlAdapter(true))
                            {
                                adapter.SaveAndRefetch(shipmentToTry);
                                ProcessShipmentHelper(shipmentToTry, storeEntity, licenseCheckCache);

                                adapter.Commit();
                            }

                            success = true;
                            processedShipment = shipmentToTry;

                            break;
                        }
                        catch (ShippingException ex)
                        {
                            lastException = ex;
                        }
                    }

                    if (!success)
                    {
                        Debug.Assert(lastException != null, "If processing is unsuccessful, there should be an exception.");

                        throw new ShippingException(lastException.Message,lastException);
                    }
                    
                    // Log to the knowledge base after everything else has been successful, so an error logging
                    // to the knowledge base does not prevent the shipment from being actually processed.
                    LogToShipSenseKnowledgebase(ShipmentTypeManager.GetType(processedShipment), processedShipment);
                }
            }
            catch (SqlAppResourceLockException ex)
            {
                log.InfoFormat("Could not obtain lock for processing shipment {0}", shipmentID);
                throw new ShippingException("The shipment was being processed on another computer.", ex);
            }
        }

        /// <summary>
        /// Process the given shipment.  If the shipment is already processed, then no action is taken or error reported.  Licensing
        /// is validated, and processing results are logged to tango.
        /// </summary>
        private static void ProcessShipmentHelper(ShipmentEntity shipment, StoreEntity storeEntity, Dictionary<long, Exception> licenseCheckCache)
        {
            ShippingSettingsEntity settings = ShippingSettings.Fetch();

            try
            {
                // Get the ShipmentType instance
                ShipmentType shipmentType = ShipmentTypeManager.GetType(shipment);

                // A null value returned from the pre-process method means the user has opted to not continue 
                // processing after a counter rate was selected as the best rate, so the processing of the shipment should be aborted
                if (shipmentType == null)
                {
                    return;
                }

                // Make sure the type is setup - its possible it's not in the case of upgrading from V2
                if (!IsShipmentTypeConfigured(shipmentType.ShipmentTypeCode))
                {
                    throw new ShippingException(String.Format("The '{0}' shipping provider was migrated from ShipWorks 2, and has not yet been configured for ShipWorks 3.", shipmentType.ShipmentTypeName));
                }

                // Validate that the license is valid
                ValidateLicense(storeEntity, licenseCheckCache);

                // Ensure the carrier specific data has been loaded
                log.InfoFormat("Shipment {0}  - Ensuring loaded", shipment.ShipmentID);
                EnsureShipmentLoaded(shipment);

                // Update the dyamic data of the shipment
                shipmentType.UpdateDynamicShipmentData(shipment);

                // Apply the blank recipient phone# option.  We apply it right to the entity so that
                // its transparent to all the shipping carrier processing.  But we reset it back 
                // after processing, so it doesn't look like that's the phone the customer entered for the shipment.
                if (shipment.ShipPhone.Trim().Length == 0)
                {
                    if (settings.BlankPhoneOption == (int)ShipmentBlankPhoneOption.SpecifiedPhone)
                    {
                        shipment.ShipPhone = settings.BlankPhoneNumber;
                    }
                    else
                    {
                        shipment.ShipPhone = shipment.OriginPhone;
                    }

                    log.InfoFormat("Shipment {1} - Using phone '{0}' for  in place of blank phone.", shipment.ShipPhone, shipment.ShipmentID);
                }

                // Determine residential status
                if (shipmentType.IsResidentialStatusRequired(shipment))
                {
                    shipment.ResidentialResult = ResidentialDeterminationService.DetermineResidentialAddress(shipment);
                }

                InsuranceUtility.ValidateShipment(shipment);

                // Check against the postal restriction for APO/FPO only
                var postalRestriction = EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.PostalApoFpoPoboxOnly, shipment);
                if (postalRestriction.Level != EditionRestrictionLevel.None)
                {
                    throw new ShippingException(postalRestriction.GetDescription());
                }

                var typeRestriction = EditionManager.ActiveRestrictions.CheckRestriction(EditionFeature.ShipmentType, shipmentType.ShipmentTypeCode);
                if (typeRestriction.Level != EditionRestrictionLevel.None)
                {
                    throw new ShippingException(String.Format("Your edition of ShipWorks does not support shipping with '{0}'.", shipmentType.ShipmentTypeName));
                }

                // If they had set this shipment to be a return - we want to make sure it's not processed as one if they switched to something that doesnt support it
                if (!shipmentType.SupportsReturns)
                {
                    shipment.ReturnShipment = false;
                }

                // We're going to allow the store to confirm the shipping address for the shipping label, but we want to 
                // make a note of the original shipping address first, so we can reset the address back after the label 
                // has been generated. This will result in the customer still being able to see where the package went 
                // according to the original cart order
                ShipmentEntity clone = EntityUtility.CloneEntity(shipment);

                // Instantiate the store class to allow it a chance to confirm the shipping address before 
                // the shipping label is created. We don't use the method on the ShippingManager to do this
                // since we want to track the fields that changed.
                StoreType storeType = StoreTypeManager.GetType(storeEntity);
                List<ShipmentFieldIndex> fieldsToRestore = storeType.OverrideShipmentDetails(shipment);

                if (shipment.ShipSenseStatus == (int)ShipSenseStatus.Applied)
                {
                    Knowledgebase knowledgebase = new Knowledgebase();
                    if (knowledgebase.IsOverwritten(shipment))
                    {
                        shipment.ShipSenseStatus = (int)ShipSenseStatus.Overwritten;
                    }
                }

                // Transacted
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    log.InfoFormat("Shipment {0}  - ShipmentType.Process Start", shipment.ShipmentID);
                    shipmentType.ProcessShipment(shipment);
                    log.InfoFormat("Shipment {0}  - ShipmentType.Process Complete", shipment.ShipmentID);

                    // Now that the label is generated, we can reset the shipping fields the store changed back to their 
                    // original values before saving to the database
                    foreach (ShipmentFieldIndex fieldIndex in fieldsToRestore)
                    {
                        // Make sure the field is not seen as dirty since we're setting the shipment back to its original value
                        shipment.SetNewFieldValue((int)fieldIndex, clone.GetCurrentFieldValue((int)fieldIndex));
                        shipment.Fields[(int)fieldIndex].IsChanged = false;
                    }

                    shipment.Processed = true;
                    shipment.ProcessedDate = DateTime.UtcNow;
                    shipment.ProcessedUserID = UserSession.User.UserID;
                    shipment.ProcessedComputerID = UserSession.Computer.ComputerID;

                    // Remove any shipment data that is not necessary for this shipment type
                    // BN: Actually we can't do this here.  Auditing follows some rules, and one of which is that if there are any deletes of 1:1 mapped entities (such as FedEx:Shipment)
                    //     then the whole activity is considered a delete.  So deleting "non active shipment data" actually makes processing show up as a Delete in the audit.
                    // ClearNonActiveShipmentData(shipment, adapter);

                    adapter.SaveAndRefetch(shipment);

                    // For WorldShip actions don't happen until the shipment comes back in after being processed in WorldShip
                    if (!shipmentType.ProcessingCompletesExternally)
                    {
                        // Dispatch the shipment processed event
                        ActionDispatcher.DispatchShipmentProcessed(shipment, adapter);
                        log.InfoFormat("Shipment {0}  - Dispatched", shipment.ShipmentID);
                    }                    

                    adapter.Commit();
                }

                log.InfoFormat("Shipment {0}  - Committed", shipment.ShipmentID);

                // Now log the result to tango.  For WorldShip we can't do this until the shipment comes back in to ShipWorks
                if (!shipmentType.ProcessingCompletesExternally)
                {
                    TangoWebClient.LogShipment(storeEntity, shipment);

                    log.InfoFormat("Shipment {0}  - Accounted", shipment.ShipmentID);
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
            catch (TemplateTokenException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
        
        /// <summary>
        /// Logs the shipment data to the ShipSense knowledge base. All exceptions will be caught
        /// and logged and wrapped in a ShippingException.
        /// </summary>
        private static void LogToShipSenseKnowledgebase(ShipmentType shipmentType, ShipmentEntity shipment)
        {
            try
            {
                IEnumerable<IPackageAdapter> packageAdapters = shipmentType.GetPackageAdapters(shipment);

                // Make sure we have all of the order information
                OrderEntity order = (OrderEntity)DataProvider.GetEntity(shipment.OrderID);
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    adapter.FetchEntityCollection(order.OrderItems, new RelationPredicateBucket(OrderItemFields.OrderID == order.OrderID));
                }

                // Apply the data from the package adapters and the customs items to the knowledge base 
                // entry, so the shipment data will get saved to the knowledge base; the knowledge base
                // is smart enough to know when to save the customs items associated with an entry.
                KnowledgebaseEntry entry = new KnowledgebaseEntry();
                entry.ApplyFrom(packageAdapters, shipment.CustomsItems);

                Knowledgebase knowledgebase = new Knowledgebase();
                knowledgebase.Save(entry, order);
            }
            catch (Exception ex)
            {
                // We may want to eat this exception entirely, so the user isn't impacted 
                log.ErrorFormat("An error occurred writing shipment ID {0} to the knowledge base: {1}", shipment.ShipmentID, ex.Message);
                throw new ShippingException("The shipment was processed successfully, but the data was not logged to ShipSense.", ex);
            }
        }

        /// <summary>
        /// Clears out any other shipment data that is not application to the actual type of the shipment
        /// </summary>
        /// <param name="shipment">Shipment from which to clear extra data</param>
        /// <param name="adapter">SqlAdapter that will be used to delete other shipment data</param>
        private static void ClearNonActiveShipmentData(ShipmentEntity shipment, IDataAccessAdapter adapter)
        {
            ClearOtherShipmentData(adapter, shipment, typeof(UpsShipmentEntity), UpsShipmentFields.ShipmentID, ShipmentTypeCode.UpsOnLineTools, ShipmentTypeCode.UpsWorldShip);
            ClearOtherShipmentData(adapter, shipment, typeof(EndiciaShipmentEntity), EndiciaShipmentFields.ShipmentID, ShipmentTypeCode.Endicia, ShipmentTypeCode.Express1Endicia);
            ClearOtherShipmentData(adapter, shipment, typeof(StampsShipmentEntity), StampsShipmentFields.ShipmentID, ShipmentTypeCode.Stamps, ShipmentTypeCode.Express1Stamps);
            ClearOtherShipmentData(adapter, shipment, typeof(PostalShipmentEntity), PostalShipmentFields.ShipmentID,
                ShipmentTypeCode.PostalWebTools, ShipmentTypeCode.Endicia, ShipmentTypeCode.Stamps, ShipmentTypeCode.Express1Endicia, ShipmentTypeCode.Express1Stamps);
            ClearOtherShipmentData(adapter, shipment, typeof(FedExShipmentEntity), FedExShipmentFields.ShipmentID, ShipmentTypeCode.FedEx);
            ClearOtherShipmentData(adapter, shipment, typeof(OnTracShipmentEntity), OnTracShipmentFields.ShipmentID, ShipmentTypeCode.OnTrac);
            ClearOtherShipmentData(adapter, shipment, typeof(IParcelShipmentEntity), IParcelShipmentFields.ShipmentID, ShipmentTypeCode.iParcel);
            ClearOtherShipmentData(adapter, shipment, typeof(OtherShipmentEntity), OtherShipmentFields.ShipmentID, ShipmentTypeCode.Other);
            ClearOtherShipmentData(adapter, shipment, typeof(EquaShipShipmentEntity), EquaShipShipmentFields.ShipmentID, ShipmentTypeCode.EquaShip);
            ClearOtherShipmentData(adapter, shipment, typeof(BestRateShipmentEntity), BestRateShipmentFields.ShipmentID, ShipmentTypeCode.BestRate);
        }

        /// <summary>
        /// Clear specified shipment data if not relevant
        /// </summary>
        /// <param name="adapter">SqlAdapter that will be used to delete child shipment entities</param>
        /// <param name="shipment">Shipment from which child shipment data will be deleted</param>
        /// <param name="childShipmentType">Type of child shipment that should be deleted</param>
        /// <param name="shipmentIdField">Field that specifies the ShipmentId for the child</param>
        /// <param name="requiredForTypes">Delete this child shipment unless it is one of the specified types</param>
        private static void ClearOtherShipmentData(IDataAccessAdapter adapter, ShipmentEntity shipment, Type childShipmentType, EntityField2 shipmentIdField, params ShipmentTypeCode[] requiredForTypes)
        {
            if (requiredForTypes.Contains((ShipmentTypeCode)shipment.ShipmentType))
            {
                return;
            }

            adapter.DeleteEntitiesDirectly(childShipmentType, new RelationPredicateBucket(shipmentIdField == shipment.ShipmentID));
        }

        /// <summary>
        /// Validate that the given store is licensed to ship.  If there is an error its stored in licenseCheckCache.  If there is already
        /// an error for the store in licenseCheckCache, then its reused and no trip to tango is taken.
        /// </summary>
        private static void ValidateLicense(StoreEntity store, Dictionary<long, Exception> licenseCheckCache)
        {
            long storeID = store.StoreID;

            Exception error;
            if (licenseCheckCache.TryGetValue(storeID, out error))
            {
                // If there was an error from a previous check, throw it again
                if (error != null)
                {
                    throw error;
                }
            }
            else
            {
                try
                {
                    LicenseActivationHelper.EnsureActive(store);

                    licenseCheckCache[storeID] = null;
                }
                catch (ShipWorksLicenseException ex)
                {
                    licenseCheckCache[storeID] = ex;
                    throw;
                }
                catch (TangoException ex)
                {
                    licenseCheckCache[storeID] = ex;
                    throw;
                }
            }
        }

        /// <summary>
        /// Indicates if the shipment type of the given type code has gone through the full setup wizard \ configuration
        /// </summary>
        public static bool IsShipmentTypeConfigured(ShipmentTypeCode shipmentTypeCode)
        {
            if (shipmentTypeCode == ShipmentTypeCode.None)
            {
                return true;
            }

            return ShippingSettings.Fetch().ConfiguredTypes.Contains((int)shipmentTypeCode);
        }

        /// <summary>
        /// Indicates if the shipment type has been activated (utilized) by the user and ShipWorks should load and display carrier specific data for the shipment type.
        /// It may not yet be totally "Configured" in the case of a 2x upgrade where the user skips account migration.
        /// </summary>
        public static bool IsShipmentTypeActivated(ShipmentTypeCode shipmentTypeCode)
        {
            if (shipmentTypeCode == ShipmentTypeCode.None)
            {
                return true;
            }

            return ShippingSettings.Fetch().ActivatedTypes.Contains((int)shipmentTypeCode);
        }

        /// <summary>
        /// Indicates if the given shipment type code is enabled for selection in the shipping window
        /// </summary>
        public static bool IsShipmentTypeEnabled(ShipmentTypeCode shipmentTypeCode)
        {
            return !ShippingSettings.Fetch().ExcludedTypes.Contains((int)shipmentTypeCode);
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
    }
}
