using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.Data.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

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
        public static void DeleteExistingAddresses(DataAccessAdapter adapter, long entityId)
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
            addressesToDelete.ForEach(x =>
            {
                AddressEntity addressToDelete = new AddressEntity {AddressID = x.AddressID, IsNew = false};

                dataAccess.DeleteEntity(x);
                dataAccess.DeleteEntity(addressToDelete);
            });
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
            adapter.DeleteEntitiesDirectly(typeof (ValidatedAddressEntity), validatedAddressDeleteBucket);

            // Delete all addresses that are not referenced by a validated address entry
            RelationPredicateBucket addressDeleteBucket = new RelationPredicateBucket();
            addressDeleteBucket.PredicateExpression.Add(new FieldCompareSetPredicate(AddressFields.AddressID, null, ValidatedAddressFields.AddressID, null, SetOperator.In, null, true));
            adapter.DeleteEntitiesDirectly(typeof (AddressEntity), addressDeleteBucket);
        }

        /// <summary>
        /// Save an order that has just been validated
        /// </summary>
        /// <param name="context">Interface with the database</param>
        /// <param name="order">Order that was validated</param>
        /// <param name="originalShippingAddress">The address of the order before any changes were made to it</param>
        /// <param name="enteredAddress">The address entered into the order, either manually or from a download</param>
        /// <param name="suggestedAddresses">List of addresses suggested by validation</param>
        public static void SaveValidatedOrder(ActionStepContext context, OrderEntity order, AddressAdapter originalShippingAddress, AddressEntity enteredAddress, IEnumerable<AddressEntity> suggestedAddresses)
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
        public static void SaveValidatedOrder(DataAccessAdapter adapter, OrderEntity order, AddressAdapter originalShippingAddress, AddressEntity enteredAddress, IEnumerable<AddressEntity> suggestedAddresses)
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
        public static void SaveValidatedOrder(IAddressValidationDataAccess dataAccess, OrderEntity order, AddressAdapter originalShippingAddress, AddressEntity enteredAddress, IEnumerable<AddressEntity> suggestedAddresses)
        {
            DeleteExistingAddresses(dataAccess, order.OrderID);
            SaveOrderAddress(dataAccess, order, enteredAddress, true);

            List<AddressEntity> suggestedAddressList = suggestedAddresses.ToList();

            foreach (AddressEntity address in suggestedAddressList)
            {
                SaveOrderAddress(dataAccess, order, address, false);
            }

            order.ShipAddressValidationSuggestionCount = suggestedAddressList.Count();
            dataAccess.SaveEntity(order);

            PropagateAddressChangesToShipments(dataAccess, order.OrderID, originalShippingAddress, new AddressAdapter(order, "Ship"));
        }

        /// <summary>
        /// Save a validated address
        /// </summary>
        public static void SaveOrderAddress(IAddressValidationDataAccess dataAccess, OrderEntity order, AddressEntity address, bool isOriginalAddress)
        {
            // If the address is null, we obviously don't need to save it
            if (address == null)
            {
                return;
            }

            ValidatedAddressEntity validatedAddressEntity = new ValidatedAddressEntity
            {
                ConsumerID = order.OrderID,
                Address = address,
                IsOriginal = isOriginalAddress
            };

            dataAccess.SaveEntity(validatedAddressEntity);
        }
    }
}
