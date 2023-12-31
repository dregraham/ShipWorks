﻿using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared;
using Interapptive.Shared.Business;
using Interapptive.Shared.Enums;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.AddressValidation.Enums;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
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
        public static void PropagateAddressChangesToShipments(ISqlAdapter adapter, long orderID, AddressAdapter originalShippingAddress, AddressAdapter newShippingAddress)
        {
            PropagateAddressChangesToShipments(new AdapterAddressValidationDataAccess(adapter), orderID, originalShippingAddress, newShippingAddress);
        }

        /// <summary>
        /// Propagates the address change to billing.
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
        /// Gets the original address for an entity
        /// </summary>
        public static ValidatedAddressEntity GetOriginalAddress(ISqlAdapter dataAccess, long entityId, string prefix)
        {
            return GetSuggestedAddresses(dataAccess, entityId, prefix)
                .Where(entity => entity.IsOriginal)
                .OrderByDescending(entity => entity.ValidatedAddressID).FirstOrDefault();
        }

        /// <summary>
        /// Gets the associated validated addresses for an entity
        /// </summary>
        public static List<ValidatedAddressEntity> GetSuggestedAddresses(ISqlAdapter dataAccess, long entityId, string prefix)
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
        public static void DeleteExistingAddresses(ISqlAdapter adapter, long entityId, string prefix)
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
        public static void SaveValidatedOrder(IActionStepContext context, ValidatedOrderShipAddress validatedOrderAddressInfo)
        {
            SaveValidatedOrderShipAddress(new ContextAddressValidationDataAccess(context), validatedOrderAddressInfo);
        }

        /// <summary>
        /// Save an order that has just been validated
        /// </summary>
        /// <param name="adapter">Interface with the database</param>
        /// <param name="validatedOrderAddressInfo">The validated order address information.</param>
        public static void SaveValidatedOrder(SqlAdapter adapter, ValidatedOrderShipAddress validatedOrderAddressInfo)
        {
            SaveValidatedOrderShipAddress(new AdapterAddressValidationDataAccess(adapter), validatedOrderAddressInfo);
        }

        /// <summary>
        /// Save an order that has just been validated
        /// </summary>
        /// <param name="dataAccess">Interface with the database</param>
        /// <param name="validatedOrderAddressInfo">The validated order address information.</param>
        public static void SaveValidatedOrderShipAddress(IAddressValidationDataAccess dataAccess, ValidatedOrderShipAddress validatedOrderAddressInfo)
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

            OrderEntity order = (OrderEntity) validatedOrderAddressInfo.Entity;
            PropagateAddressChangesToShipments(dataAccess, order.OrderID, validatedOrderAddressInfo.OriginalShippingAddress, newShippingAddress, addressSuggestions);
            PropagateAddressChangeToBilling(dataAccess, order, validatedOrderAddressInfo.OriginalShippingAddress, newShippingAddress, addressSuggestions);
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
        public static void SaveValidatedShipmentAddress(SqlAdapter adapter, ValidatedShipmentShipAddress validatedShippingAddress)
        {
            SaveValidatedEntity(new AdapterAddressValidationDataAccess(adapter), validatedShippingAddress);
        }

        /// <summary>
        /// Save an entity that has just been validated
        /// </summary>
        /// <param name="dataAccess">Interface with the database</param>
        /// <param name="validatedAddress">The validated address.</param>
        private static void SaveValidatedEntity(IAddressValidationDataAccess dataAccess, ValidatedShipAddressBase validatedAddress)
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

        /// <summary>
        /// Validate a single shipment
        /// </summary>
        public static Task ValidateShipmentAsync(ShipmentEntity shipment, IAddressValidator validator)
        {
            // 3 retries is an arbitrary decision, but it should cover the case where we get a concurrency exception
            // validating the order and then the shipment, since the background process validates each at different times
            return ValidateShipmentAsync(shipment, validator, 3);
        }

        /// <summary>
        /// Validates an individual shipment
        /// </summary>
        /// <param name="shipment">Shipment to validate</param>
        /// <param name="validator">Address validator to use</param>
        /// <param name="retryCount">How many times should validation be retried</param>
        [NDependIgnoreLongMethod]
        private static async Task ValidateShipmentAsync(ShipmentEntity shipment, IAddressValidator validator, int retryCount)
        {
            AddressAdapter shipmentAdapter = new AddressAdapter(shipment, "Ship");

            // If the shipment address is anything but pending, we don't need to do anything
            if (shipmentAdapter.AddressValidationStatus != (int) AddressValidationStatusType.Pending)
            {
                return;
            }

            OrderEntity order = shipment.Order ?? DataProvider.GetEntity(shipment.OrderID) as OrderEntity;
            if (order == null)
            {
                return;
            }

            StoreEntity store = DataProvider.GetEntity(order.StoreID) as StoreEntity;
            if (store == null || !AddressValidationPolicy.ShouldAutoValidate(store, shipmentAdapter))
            {
                // If we can't find the store or if its not set up for auto-validation, we shouldn't do any validation
                return;
            }

            bool shouldRetry = false;
            bool canApplyChanges = shipmentAdapter.IsDomesticCountry() ?
                store.DomesticAddressValidationSetting == AddressValidationStoreSettingType.ValidateAndApply :
                store.InternationalAddressValidationSetting == AddressValidationStoreSettingType.ValidateAndApply;

            try
            {
                AddressAdapter orderAdapter = new AddressAdapter(order, "Ship");

                if (orderAdapter == shipmentAdapter &&
                    orderAdapter.AddressValidationStatus == shipmentAdapter.AddressValidationStatus)
                {
                    // Since the order and shipment addresses match, validate the order and let propagation take care of updating the shipment
                    AddressAdapter originalShippingAddress = new AddressAdapter();
                    orderAdapter.CopyTo(originalShippingAddress);

                    await validator.ValidateAsync(order.ShipPerson.ConvertTo<AddressAdapter>(), store, canApplyChanges, (originalAddress, suggestedAddresses) =>
                    {
                        // Use a low priority for deadlocks, since we'll just try again
                        using (new SqlDeadlockPriorityScope(-4))
                        {
                            using (SqlAdapter sqlAdapter = new SqlAdapter(true))
                            {
                                ValidatedOrderShipAddress validatedOrderAddress = new ValidatedOrderShipAddress(order, originalAddress, suggestedAddresses.ToList(), originalShippingAddress);
                                SaveValidatedOrder(sqlAdapter, validatedOrderAddress);

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
                else if (!shipment.Processed)
                {
                    // Since the addresses don't match, just validate the shipment
                    await validator.ValidateAsync(shipment.ShipPerson.ConvertTo<AddressAdapter>(), store,
                        canApplyChanges, (originalAddress, suggestedAddresses) =>
                    {
                        // Use a low priority for deadlocks, since we'll just try again
                        using (new SqlDeadlockPriorityScope(-4))
                        {
                            using (SqlAdapter sqlAdapter = new SqlAdapter(true))
                            {
                                ValidatedShipmentShipAddress shippingAddress = new ValidatedShipmentShipAddress(shipment, originalAddress, suggestedAddresses);
                                SaveValidatedShipmentAddress(sqlAdapter, shippingAddress);

                                sqlAdapter.Commit();
                            }
                        }
                    });
                }
            }
            catch (ORMConcurrencyException)
            {
                shouldRetry = true;
            }
            catch (SqlDeadlockException)
            {
                shouldRetry = true;
            }
            catch (SqlException ex) when (ex.Message.Contains("deadlock"))
            {
                shouldRetry = true;
            }

            if (shouldRetry)
            {
                await RetryValidation(shipment, order, validator, retryCount);
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
        private static async Task RetryValidation(ShipmentEntity shipment, OrderEntity order, IAddressValidator validator, int retryCount)
        {
            if (retryCount == 0)
            {
                return;
            }

            using (SqlAdapter sqlAdapter = new SqlAdapter())
            {
                // Reload the shipment before we retry validation so that we reflect the changes that caused
                // the concurrency exception
                sqlAdapter.FetchEntity(shipment);

                if (order != null)
                {
                    // Remove the order from the cache so that when we retry the validation, we force a reload
                    DataProvider.RemoveEntity(order.OrderID);
                }
            }

            await ValidateShipmentAsync(shipment, validator, retryCount - 1);
        }
    }
}
