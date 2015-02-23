using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using RestSharp.Validation;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data.Model.RelationClasses;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Manage validated addresses
    /// </summary>
    public static class ValidatedAddressManager
    {
        /// <summary>
        /// Propagates the address changes to shipments.
        /// </summary>
        public static void PropagateAddressChangesToShipments(SqlAdapter adapter, long orderID, AddressAdapter originalShippingAddress, AddressAdapter newShippingAddress)
        {
            PropagateAddressChangesToShipments(new AdapterAddressValidationDataAccess(adapter), orderID, originalShippingAddress, newShippingAddress);
        }

        /// <summary>
        /// Propogates the address change to billing.
        /// </summary>
        private static void PropagateAddressChangeToBilling(IAddressValidationDataAccess dataAccess, OrderEntity order, AddressAdapter originalShippingAddress, AddressAdapter newShippingAddress, List<ValidatedAddressEntity> addressSuggestions)
        {
            // If the order shipment address hasn't changed, we don't need to do anything
            if (originalShippingAddress == newShippingAddress &&
                originalShippingAddress.AddressValidationStatus == newShippingAddress.AddressValidationStatus)
            {
                return;
            }

            // Update the shipment address if its current address is the same as the original shipment address
            AddressAdapter billingAddress = new AddressAdapter(order, "Bill");
            if (originalShippingAddress == billingAddress)
            {
                newShippingAddress.CopyTo(billingAddress);
                billingAddress.AddressValidationStatus = newShippingAddress.AddressValidationStatus;
                billingAddress.AddressValidationSuggestionCount = newShippingAddress.AddressValidationSuggestionCount;
                billingAddress.AddressValidationError = newShippingAddress.AddressValidationError;

                CopyValidatedAddresses(dataAccess, addressSuggestions, order.OrderID, "Bill");

                dataAccess.SaveEntity(order);
            }
        }

        /// <summary>
        /// Propagate order address changes to unprocessed shipments if necessary
        /// </summary>
        public static void PropagateAddressChangesToShipments(IAddressValidationDataAccess dataAccess, long orderID, AddressAdapter originalShippingAddress, AddressAdapter newShippingAddress)
        {
            PropagateAddressChangesToShipments(dataAccess, orderID, originalShippingAddress, newShippingAddress, null);
        }

        /// <summary>
        /// Propagate order address changes to unprocessed shipments if necessary
        /// </summary>
        private static void PropagateAddressChangesToShipments(IAddressValidationDataAccess dataAccess, long orderID, AddressAdapter originalShippingAddress, 
            AddressAdapter newShippingAddress, List<ValidatedAddressEntity> addressSuggestions)
        {
            // If the order shipment address hasn't changed, we don't need to do anything
            if (originalShippingAddress == newShippingAddress &&
                originalShippingAddress.AddressValidationStatus == newShippingAddress.AddressValidationStatus)
            {
                return;
            }

            // Get the shipments we might need to update
            IEnumerable<ShipmentEntity> shipments = dataAccess.GetUnprocessedShipmentsForOrder(orderID);

            foreach (ShipmentEntity shipment in shipments)
            {
                // Update the shipment address if its current address is the same as the original shipment address
                AddressAdapter shipmentAddress = new AddressAdapter(shipment, "Ship");
                if (originalShippingAddress == shipmentAddress)
                {
                    newShippingAddress.CopyTo(shipmentAddress);
                    shipmentAddress.AddressValidationStatus = newShippingAddress.AddressValidationStatus;
                    shipmentAddress.AddressValidationSuggestionCount = newShippingAddress.AddressValidationSuggestionCount;
                    shipmentAddress.AddressValidationError = newShippingAddress.AddressValidationError;

                    if (addressSuggestions == null)
                    {
                        CopyValidatedAddresses(dataAccess, orderID, "Ship", shipment.ShipmentID, "Ship");    
                    }
                    else
                    {
                        CopyValidatedAddresses(dataAccess, addressSuggestions, shipment.ShipmentID, "Ship");
                    }

                    dataAccess.SaveEntity(shipment);
                }
            }
        }

        /// <summary>
        /// Gets the associated validated addresses for an entity
        /// </summary>
        public static List<ValidatedAddressEntity> GetSuggestedAddresses(SqlAdapter dataAccess, long entityId, string prefix)
        {
            return GetSuggestedAddresses(new AdapterAddressValidationDataAccess(dataAccess), entityId, prefix);
        }

        /// <summary>
        /// Gets the associated validated addresses for an entity
        /// </summary>
        public static List<ValidatedAddressEntity> GetSuggestedAddresses(IAddressValidationDataAccess dataAccess, long entityId, string prefix)
        {
            return dataAccess.GetValidatedAddressesByConsumerAndPrefix(entityId, prefix).ToList();
        }

        /// <summary>
        /// Deletes existing validated addresses
        /// </summary>
        public static void DeleteExistingAddresses(SqlAdapter adapter, long entityId, string prefix)
        {
            DeleteExistingAddresses(new AdapterAddressValidationDataAccess(adapter), entityId, prefix);
        }

        /// <summary>
        /// Deletes existing validated addresses
        /// </summary>
        public static void DeleteExistingAddresses(IAddressValidationDataAccess dataAccess, long entityId, string prefix)
        {
            // Retrieve the addresses
            List<ValidatedAddressEntity> addressesToDelete = GetSuggestedAddresses(dataAccess, entityId, prefix);

            // Mark each address for deletion
            addressesToDelete.ForEach(dataAccess.DeleteEntity);
        }

        /// <summary>
        /// Save an order that has just been validated
        /// </summary>
        /// <param name="context">Interface with the database</param>
        /// <param name="validatedOrderAddressInfo">The validated order address information.</param>
        /// <param name="originalAddress">The address of the order before any changes were made to it</param>
        public static void SaveValidatedOrder(ActionStepContext context, ValidatedOrderAddress validatedOrderAddressInfo, AddressAdapter originalAddress)
        {
            SaveValidatedOrderShipAddress(new ContextAddressValidationDataAccess(context), validatedOrderAddressInfo, originalAddress);
        }

        /// <summary>
        /// Save an order that has just been validated
        /// </summary>
        /// <param name="adapter">Interface with the database</param>
        /// <param name="validatedOrderAddressInfo">The validated order address information.</param>
        /// <param name="originalAddress">The address of the order before any changes were made to it</param>
        public static void SaveValidatedOrder(SqlAdapter adapter, ValidatedOrderAddress validatedOrderAddressInfo, AddressAdapter originalAddress)
        {
            SaveValidatedOrderShipAddress(new AdapterAddressValidationDataAccess(adapter), validatedOrderAddressInfo, originalAddress);
        }

        /// <summary>
        /// Save an order that has just been validated
        /// </summary>
        /// <param name="dataAccess">Interface with the database</param>
        /// <param name="validatedOrderAddressInfo">The validated order address information.</param>
        /// <param name="originalAddress">The address of the order before any changes were made to it</param>
        public static void SaveValidatedOrderShipAddress(IAddressValidationDataAccess dataAccess, ValidatedOrderAddress validatedOrderAddressInfo, AddressAdapter originalAddress)
        {
            SaveValidatedEntity(dataAccess, validatedOrderAddressInfo);

            AddressAdapter newShippingAddress = new AddressAdapter(validatedOrderAddressInfo.Entity, validatedOrderAddressInfo.Prefix);

            // We can't guarantee that the suggested addresses we need to copy have been stored in the database yet.  For example,
            // Action Tasks do everything in a unit of work so they don't get written until the end
            List<ValidatedAddressEntity> addressSuggestions = new List<ValidatedAddressEntity>();

            if (validatedOrderAddressInfo.EnteredAddress != null)
            {
                addressSuggestions.Add(validatedOrderAddressInfo.EnteredAddress);
            }
            addressSuggestions.AddRange(validatedOrderAddressInfo.SuggestedAddresses);

            // TODO: Refactor propogate methods to use the validated order address info
            OrderEntity order = (OrderEntity) validatedOrderAddressInfo.Entity;
            PropagateAddressChangesToShipments(dataAccess, order.OrderID, originalAddress, newShippingAddress, addressSuggestions);
            PropagateAddressChangeToBilling(dataAccess, order, originalAddress, newShippingAddress, addressSuggestions);
        }

        /// <summary>
        /// Save a validated address
        /// </summary>
        public static void SaveEntityAddress(IAddressValidationDataAccess dataAccess, long entityId, ValidatedAddressEntity address)
        {
            // If the address is null, we obviously don't need to save it
            if (address == null)
            {
                return;
            }

            address.ConsumerID = entityId;

            dataAccess.SaveEntity(address);
        }

        /// <summary>
        /// Save a shipment that has just been validated
        /// </summary>
        /// <param name="adapter">Interface with the database</param>
        /// <param name="validatedShippingAddress">The validated shipping address.</param>
        public static void SaveValidatedShipmentAddress(SqlAdapter adapter, ValidatedShippingAddress validatedShippingAddress)
        {
            SaveValidatedEntity(new AdapterAddressValidationDataAccess(adapter), validatedShippingAddress);
        }

        /// <summary>
        /// Save an entity that has just been validated
        /// </summary>
        /// <param name="dataAccess">Interface with the database</param>
        /// <param name="validatedAddress">The validated address.</param>
        private static void SaveValidatedEntity(IAddressValidationDataAccess dataAccess, ValidatedAddressBase validatedAddress)
        {
            long entityId = EntityUtility.GetEntityId(validatedAddress.Entity);

            DeleteExistingAddresses(dataAccess, entityId, validatedAddress.Prefix);
            SaveEntityAddress(dataAccess, entityId, validatedAddress.EnteredAddress);

            List<ValidatedAddressEntity> suggestedAddressList = validatedAddress.SuggestedAddresses.ToList();

            foreach (ValidatedAddressEntity address in suggestedAddressList)
            {
                SaveEntityAddress(dataAccess, entityId, address);
            }

            // Ensure that we're using optimistic concurrency with this entity because we don't want to overwrite
            // changes that may have happened elsewhere
            IConcurrencyPredicateFactory previousConcurrencyPredicateFactory = validatedAddress.Entity.ConcurrencyPredicateFactoryToUse;
            validatedAddress.Entity.ConcurrencyPredicateFactoryToUse = new OptimisticConcurrencyFactory();

            try
            {
                dataAccess.SaveEntity(validatedAddress.Entity);
            }
            finally
            {
                validatedAddress.Entity.ConcurrencyPredicateFactoryToUse = previousConcurrencyPredicateFactory;
            }
        }

        ///// <summary>
        ///// Save an entity that has just been validated
        ///// </summary>
        ///// <param name="dataAccess">Interface with the database</param>
        ///// <param name="entity">Entity that was validated</param>
        ///// <param name="enteredAddress">The address entered into the entity, either manually or from a download</param>
        ///// <param name="suggestedAddresses">List of addresses suggested by validation</param>
        ///// <param name="prefix">Address prefix for the entity</param>
        //public static void SaveValidatedEntity(IAddressValidationDataAccess dataAccess, IEntity2 entity,
        //    ValidatedAddressEntity enteredAddress, IEnumerable<ValidatedAddressEntity> suggestedAddresses,
        //    string prefix)
        //{
        //    long entityId = EntityUtility.GetEntityId(entity);

        //    DeleteExistingAddresses(dataAccess, entityId, prefix);
        //    SaveEntityAddress(dataAccess, entityId, enteredAddress);

        //    List<ValidatedAddressEntity> suggestedAddressList = suggestedAddresses.ToList();

        //    foreach (ValidatedAddressEntity address in suggestedAddressList)
        //    {
        //        SaveEntityAddress(dataAccess, entityId, address);
        //    }

        //    // Ensure that we're using optimistic concurrency with this entity because we don't want to overwrite
        //    // changes that may have happened elsewhere
        //    IConcurrencyPredicateFactory previousConcurrencyPredicateFactory = entity.ConcurrencyPredicateFactoryToUse;
        //    entity.ConcurrencyPredicateFactoryToUse = new OptimisticConcurrencyFactory();

        //    try
        //    {
        //        dataAccess.SaveEntity(entity);
        //    }
        //    finally
        //    {
        //        entity.ConcurrencyPredicateFactoryToUse = previousConcurrencyPredicateFactory;
        //    }
        //}

        /// <summary>
        /// Check whether the specified address can be validated
        /// </summary>
        public static bool EnsureAddressCanBeValidated(AddressAdapter currentShippingAddress)
        {
            if (string.IsNullOrEmpty(currentShippingAddress.CountryCode))
            {
                currentShippingAddress.AddressValidationError = "ShipWorks cannot validate an address without a country.";
                currentShippingAddress.AddressValidationStatus = (int)AddressValidationStatusType.BadAddress;

                return false;
            }

            if (!PostalUtility.IsDomesticCountry(currentShippingAddress.CountryCode) &&
                !PostalUtility.IsMilitaryState(currentShippingAddress.CountryCode))
            {
                currentShippingAddress.AddressValidationError = "ShipWorks cannot validate international addresses";
                currentShippingAddress.AddressValidationStatus = (int)AddressValidationStatusType.WillNotValidate;

                return false;
            }

            if (string.IsNullOrEmpty(currentShippingAddress.Street1))
            {
                currentShippingAddress.AddressValidationError = "ShipWorks cannot validate an address without a first line.";
                currentShippingAddress.AddressValidationStatus = (int)AddressValidationStatusType.BadAddress;

                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate a single shipment
        /// </summary>
        public static void ValidateShipment(ShipmentEntity shipment, AddressValidator validator)
        {
            ValidateShipment(shipment, validator, true);
        }

        /// <summary>
        /// Validates an individual shipment
        /// </summary>
        /// <param name="shipment">Shipment to validate</param>
        /// <param name="validator">Address validator to use</param>
        /// <param name="retryOnConcurrencyException">Should the validation be retried if there is a concurrency exception</param>
        private static void ValidateShipment(ShipmentEntity shipment, AddressValidator validator, bool retryOnConcurrencyException)
        {
            AddressAdapter shipmentAdapter = new AddressAdapter(shipment, "Ship");

            // If the shipment address is anything but pending, we don't need to do anything
            if (shipmentAdapter.AddressValidationStatus != (int) AddressValidationStatusType.Pending)
            {
                return;
            }

            OrderEntity order = DataProvider.GetEntity(shipment.OrderID) as OrderEntity;
            if (order == null)
            {
                return;
            }

            StoreEntity store = DataProvider.GetEntity(order.StoreID) as StoreEntity;
            if (store == null || !ShouldAutoValidate(store))
            {
                // If we can't find the store or if its not set up for auto-validation, we shouldn't do any validation
                return;
            }

            bool canApplyChanges = store.AddressValidationSetting == (int)AddressValidationStoreSettingType.ValidateAndApply;

            try
            {
                AddressAdapter orderAdapter = new AddressAdapter(order, "Ship");

                if (orderAdapter == shipmentAdapter)
                {
                    // Since the order and shipment addresses match, validate the order and let propagation take care of updating the shipment
                    AddressAdapter originalShippingAddress = new AddressAdapter();
                    orderAdapter.CopyTo(originalShippingAddress);

                    validator.Validate(order, "Ship", canApplyChanges, (originalAddress, suggestedAddresses) =>
                    {
                        // Use a low priority for deadlocks, since we'll just try again
                        using (new SqlDeadlockPriorityScope(-4))
                        {
                            using (SqlAdapter sqlAdapter = new SqlAdapter(true))
                            {
                                ValidatedOrderAddress validatedOrderAddress = new ValidatedOrderAddress(order, originalAddress, suggestedAddresses.ToList());
                                SaveValidatedOrder(sqlAdapter, validatedOrderAddress, originalShippingAddress);

                                sqlAdapter.Commit();
                            }
                        }
                    });

                    using (SqlAdapter sqlAdapter = new SqlAdapter())
                    {
                        // Validating the order and letting its address propagate means that the current instance of the shipment
                        // won't reflect the changes, so we need to reload it. We also need to update the reference to the order,
                        // since it's had its address updated.  This also applies to shipments other than the current shipment that
                        // had their addresses modified by propagation.
                        sqlAdapter.FetchEntity(shipment);
                        shipment.Order = order;
                    }
                }
                else
                {
                    if (!shipment.Processed)
                    {
                        // Since the addresses don't match, just validate the shipment
                        validator.Validate(shipment, "Ship", canApplyChanges, (originalAddress, suggestedAddresses) =>
                        {
                            // Use a low priority for deadlocks, since we'll just try again
                            using (new SqlDeadlockPriorityScope(-4))
                            {
                                using (SqlAdapter sqlAdapter = new SqlAdapter(true))
                                {
                                    ValidatedShippingAddress shippingAddress = new ValidatedShippingAddress(shipment, originalAddress, suggestedAddresses);
                                    SaveValidatedShipmentAddress(sqlAdapter, shippingAddress);

                                    sqlAdapter.Commit();
                                }
                            }
                        });
                    }
                }
            }
            catch (ORMConcurrencyException)
            {
                RetryValidation(shipment, validator, retryOnConcurrencyException);
            }
            catch (SqlDeadlockException)
            {
                RetryValidation(shipment, validator, retryOnConcurrencyException);
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("deadlock"))
                {
                    RetryValidation(shipment, validator, retryOnConcurrencyException);   
                }
            }
        }

        /// <summary>
        /// Copy all the validated address from one entity to another
        /// </summary>
        public static void CopyValidatedAddresses(SqlAdapter sqlAdapter, long fromEntityId, string fromPrefix, long toEntityId, string toPrefix)
        {
            CopyValidatedAddresses(new AdapterAddressValidationDataAccess(sqlAdapter), fromEntityId, fromPrefix, toEntityId, toPrefix);
        }

        /// <summary>
        /// Copy all the validated address from one entity to another
        /// </summary>
        public static void CopyValidatedAddresses(IAddressValidationDataAccess dataAccess, long fromEntityId, string fromPrefix, long toEntityId, string toPrefix)
        {
            List<ValidatedAddressEntity> addresses = GetSuggestedAddresses(dataAccess, fromEntityId, fromPrefix);

            CopyValidatedAddresses(dataAccess, addresses, toEntityId, toPrefix);
        }

        /// <summary>
        /// Copy address suggestions into an entity
        /// </summary>
        private static void CopyValidatedAddresses(IAddressValidationDataAccess dataAccess, IEnumerable<ValidatedAddressEntity> sourceSuggestions, long toEntityId, string toPrefix)
        {
            DeleteExistingAddresses(dataAccess, toEntityId, toPrefix);

            foreach (var address in sourceSuggestions)
            {
                ValidatedAddressEntity clonedAddress = EntityUtility.CloneEntity(address);
                clonedAddress.IsNew = true;
                clonedAddress.ConsumerID = toEntityId;
                clonedAddress.AddressPrefix = toPrefix;

                foreach (IEntityField2 field in clonedAddress.Fields)
                {
                    field.IsChanged = true;
                }

                dataAccess.SaveEntity(clonedAddress);
            }
        }

        /// <summary>
        /// Attempt to retry validation if necessary
        /// </summary>
        private static void RetryValidation(ShipmentEntity shipment, AddressValidator validator, bool retryOnConcurrencyException)
        {
            if (!retryOnConcurrencyException)
            {
                return;
            }

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                // Reload the shipment before we retry validation so that we reflect the changes that caused
                // the concurrency exception
                sqlAdapter.FetchEntity(shipment);
            }

            ValidateShipment(shipment, validator, false);
        }

        /// <summary>
        /// Gets whether the specified store is set up for auto-validation
        /// </summary>
        private static bool ShouldAutoValidate(StoreEntity store)
        {
            AddressValidationStoreSettingType setting = (AddressValidationStoreSettingType) store.AddressValidationSetting;
            return setting == AddressValidationStoreSettingType.ValidateAndApply ||
                   setting == AddressValidationStoreSettingType.ValidateAndNotify;
        }
    }
}
