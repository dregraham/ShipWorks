using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
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
        /// Propagate order address changes to unprocessed shipments if necessary
        /// </summary>
        public static void PropagateAddressChangesToShipments(IAddressValidationDataAccess dataAccess, long orderID, AddressAdapter originalShippingAddress, AddressAdapter newShippingAddress)
        {
            // If the order shipment address hasn't changed, we don't need to do anything
            if (originalShippingAddress == newShippingAddress)
            {
                return;
            }

            // Get the shipments we might need to update
            List<ShipmentEntity> shipments = dataAccess.LinqCollections.Shipment.Where(x => x.OrderID == orderID && !x.Processed).ToList();

            foreach (ShipmentEntity shipment in shipments)
            {
                // Update the shipment address if its current address is the same as the original shipment address
                AddressAdapter shipmentAddress = new AddressAdapter(shipment, "Ship");
                if (originalShippingAddress == shipmentAddress)
                {
                    newShippingAddress.CopyTo(shipmentAddress);
                    dataAccess.SaveEntity(shipment);
                }
            }
        }

        /// <summary>
        /// Deletes existing validated addresses
        /// </summary>
        public static void DeleteExistingAddresses(SqlAdapter adapter, long entityId)
        {
            DeleteExistingAddresses(new AdapterAddressValidationDataAccess(adapter), entityId);
        }

        /// <summary>
        /// Deletes existing validated addresses
        /// </summary>
        public static void DeleteExistingAddresses(IAddressValidationDataAccess dataAccess, long entityId)
        {
            // Retrieve the addresses
            List<ValidatedAddressEntity> addressesToDelete = dataAccess.LinqCollections.ValidatedAddress.Where(x => x.ConsumerID == entityId).ToList();

            // Mark each address for deletion
            addressesToDelete.ForEach(dataAccess.DeleteEntity);
        }

        /// <summary>
        /// Deletes addresses for all orders associated with the specified customer id
        /// </summary>
        public static void DeleteAddressesForCustomer(DataAccessAdapter adapter, long customerId)
        {
            DeleteAddressesInBulk(adapter, OrderFields.CustomerID == customerId);
        }

        /// <summary>
        /// Deletes addresses for all orders associated with the specified store id
        /// </summary>
        public static void DeleteAddressesForStore(DataAccessAdapter adapter, long storeId)
        {
            DeleteAddressesInBulk(adapter, OrderFields.StoreID == storeId);
        }

        /// <summary>
        /// Bulk delete addresses
        /// </summary>
        private static void DeleteAddressesInBulk(DataAccessAdapter adapter, IPredicate predicateToAdd)
        {
            // Delete all the validated address entries that match the predicate
            RelationPredicateBucket validatedAddressDeleteBucket = new RelationPredicateBucket();
            validatedAddressDeleteBucket.Relations.Add(new EntityRelation(OrderFields.OrderID, ValidatedAddressFields.ConsumerID, RelationType.OneToMany));
            validatedAddressDeleteBucket.PredicateExpression.Add(predicateToAdd);
            adapter.DeleteEntitiesDirectly(typeof(ValidatedAddressEntity), validatedAddressDeleteBucket);
        }

        /// <summary>
        /// Save an order that has just been validated
        /// </summary>
        /// <param name="context">Interface with the database</param>
        /// <param name="order">Order that was validated</param>
        /// <param name="originalShippingAddress">The address of the order before any changes were made to it</param>
        /// <param name="enteredAddress">The address entered into the order, either manually or from a download</param>
        /// <param name="suggestedAddresses">List of addresses suggested by validation</param>
        public static void SaveValidatedOrder(ActionStepContext context, OrderEntity order, AddressAdapter originalShippingAddress, ValidatedAddressEntity enteredAddress, IEnumerable<ValidatedAddressEntity> suggestedAddresses)
        {
            SaveValidatedOrder(new ContextAddressValidationDataAccess(context), order, originalShippingAddress, enteredAddress, suggestedAddresses);
        }

        /// <summary>
        /// Save an order that has just been validated
        /// </summary>
        /// <param name="adapter">Interface with the database</param>
        /// <param name="order">Order that was validated</param>
        /// <param name="originalShippingAddress">The address of the order before any changes were made to it</param>
        /// <param name="enteredAddress">The address entered into the order, either manually or from a download</param>
        /// <param name="suggestedAddresses">List of addresses suggested by validation</param>
        public static void SaveValidatedOrder(SqlAdapter adapter, OrderEntity order, AddressAdapter originalShippingAddress, ValidatedAddressEntity enteredAddress, IEnumerable<ValidatedAddressEntity> suggestedAddresses)
        {
            SaveValidatedOrder(new AdapterAddressValidationDataAccess(adapter), order, originalShippingAddress, enteredAddress, suggestedAddresses);
        }

        /// <summary>
        /// Save an order that has just been validated
        /// </summary>
        /// <param name="dataAccess">Interface with the database</param>
        /// <param name="order">Order that was validated</param>
        /// <param name="originalShippingAddress">The address of the order before any changes were made to it</param>
        /// <param name="enteredAddress">The address entered into the order, either manually or from a download</param>
        /// <param name="suggestedAddresses">List of addresses suggested by validation</param>
        public static void SaveValidatedOrder(IAddressValidationDataAccess dataAccess, OrderEntity order, AddressAdapter originalShippingAddress,
            ValidatedAddressEntity enteredAddress, IEnumerable<ValidatedAddressEntity> suggestedAddresses)
        {
            SaveValidatedEntity(dataAccess, order, enteredAddress, suggestedAddresses);

            PropagateAddressChangesToShipments(dataAccess, order.OrderID, originalShippingAddress, new AddressAdapter(order, "Ship"));
        }

        /// <summary>
        /// Save a validated address
        /// </summary>
        public static void SaveEntityAddress(IAddressValidationDataAccess dataAccess, long entityId, ValidatedAddressEntity address, bool isOriginalAddress)
        {
            // If the address is null, we obviously don't need to save it
            if (address == null)
            {
                return;
            }

            address.ConsumerID = entityId;
            address.IsOriginal = isOriginalAddress;

            dataAccess.SaveEntity(address);
        }

        /// <summary>
        /// Save a shipment that has just been validated
        /// </summary>
        /// <param name="adapter">Interface with the database</param>
        /// <param name="shipment">Shipment that was validated</param>
        /// <param name="enteredAddress">The address entered into the order, either manually or from a download</param>
        /// <param name="suggestedAddresses">List of addresses suggested by validation</param>
        private static void SaveValidatedEntity(SqlAdapter adapter, IEntity2 shipment,
            ValidatedAddressEntity enteredAddress, IEnumerable<ValidatedAddressEntity> suggestedAddresses)
        {
            SaveValidatedEntity(new AdapterAddressValidationDataAccess(adapter), shipment, enteredAddress, suggestedAddresses);
        }

        /// <summary>
        /// Save an entity that has just been validated
        /// </summary>
        /// <param name="dataAccess">Interface with the database</param>
        /// <param name="entity">Entity that was validated</param>
        /// <param name="enteredAddress">The address entered into the entity, either manually or from a download</param>
        /// <param name="suggestedAddresses">List of addresses suggested by validation</param>
        public static void SaveValidatedEntity(IAddressValidationDataAccess dataAccess, IEntity2 entity,
            ValidatedAddressEntity enteredAddress, IEnumerable<ValidatedAddressEntity> suggestedAddresses)
        {
            Debug.Assert(entity.PrimaryKeyFields.Count == 1, "SaveValidatedEntity cannot be called on entities with multiple field keys");
            long entityId = (long)entity.PrimaryKeyFields.Single().CurrentValue;

            DeleteExistingAddresses(dataAccess, entityId);
            SaveEntityAddress(dataAccess, entityId, enteredAddress, true);

            List<ValidatedAddressEntity> suggestedAddressList = suggestedAddresses.ToList();

            foreach (ValidatedAddressEntity address in suggestedAddressList)
            {
                SaveEntityAddress(dataAccess, entityId, address, false);
            }

            // Ensure that we're using optimistic concurrency with this entity because we don't want to overwrite
            // changes that may have happened elsewhere
            IConcurrencyPredicateFactory previousConcurrencyPredicateFactory = entity.ConcurrencyPredicateFactoryToUse;
            entity.ConcurrencyPredicateFactoryToUse = new OptimisticConcurrencyFactory();

            try
            {
                dataAccess.SaveEntity(entity);
            }
            finally
            {
                entity.ConcurrencyPredicateFactoryToUse = previousConcurrencyPredicateFactory;
            }
        }

        /// <summary>
        /// Check whether the specified address can be validated
        /// </summary>
        public static bool EnsureAddressCanBeValidated(AddressAdapter currentShippingAddress)
        {
            if (PostalUtility.IsDomesticCountry(currentShippingAddress.CountryCode) ||
                PostalUtility.IsMilitaryState(currentShippingAddress.CountryCode))
            {
                return true;
            }

            currentShippingAddress.AddressValidationError = "ShipWorks cannot validate international addresses";
            currentShippingAddress.AddressValidationStatus = (int)AddressValidationStatusType.WillNotValidate;

            return false;
        }

        /// <summary>
        /// Validate the selected list of shipments
        /// </summary>
        public static void ValidateShipments(Control owner, List<ShipmentEntity> shipments, Action onCompleted)
        {
            BackgroundExecutor<ShipmentEntity> executor = new BackgroundExecutor<ShipmentEntity>(owner, "Validating Shipment Addresses", "ShipWorks is validating the shipment addresses.", "Shipment {0} of {1}");

            // Code to execute once background load is complete
            executor.ExecuteCompleted += (sender, args) => onCompleted();

            AddressValidator validator = new AddressValidator();

            // Code to execute for each shipment
            executor.ExecuteAsync((shipment, state, issueAdder) =>
            {
                ValidateShipment(shipment, validator, true);
            }, shipments, null); // Execute the code for each shipment
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

            bool canApplyChanges = store.AddressValidationSetting ==
                                    (int)AddressValidationStoreSettingType.ValidateAndApply;

            using (SqlAdapter adapter = new SqlAdapter())
            {
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
                            SaveValidatedOrder(adapter, order, originalShippingAddress, originalAddress, suggestedAddresses);

                            // Validating the order and letting its address propagate means that the current instance of the shipment
                            // won't reflect the changes, so we need to reload it
                            adapter.FetchEntity(shipment);
                        });
                    }
                    else
                    {
                        // Since the addresses don't match, just validate the shipment
                        validator.Validate(shipment, "Ship", canApplyChanges, (originalAddress, suggestedAddresses) =>
                            SaveValidatedEntity(adapter, shipment, originalAddress, suggestedAddresses));
                    }
                }
                catch (ORMConcurrencyException)
                {
                    if (retryOnConcurrencyException)
                    {
                        // Reload the shipment before we retry validation so that we reflect the changes that caused
                        // the concurrency exception
                        adapter.FetchEntity(shipment);
                        ValidateShipment(shipment, validator, false);
                    }
                }
            }
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
