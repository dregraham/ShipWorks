using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.Data.Adapter;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Manage validated addresses
    /// </summary>
    public static class ValidatedAddressManager
    {
        /// <summary>
        /// Propagate order address changes to unprocessed shipments if necessary
        /// </summary>
        public static void PropagateAddressChangesToShipments(SqlAdapter adapter, long orderID, PersonAdapter originalShippingAddress, PersonAdapter newShippingAddress)
        {
            // If the order shipment address hasn't changed, we don't need to do anything
            if (originalShippingAddress == newShippingAddress)
            {
                return;
            }

            // Get the shipments we might need to update
            LinqMetaData metaData = new LinqMetaData(adapter);
            List<ShipmentEntity> shipments = metaData.Shipment.Where(x => x.OrderID == orderID && !x.Processed).ToList();

            foreach (ShipmentEntity shipment in shipments)
            {
                // Update the shipment address if its current address is the same as the original shipment address
                PersonAdapter shipmentAddress = new PersonAdapter(shipment, "Ship");
                if (originalShippingAddress == shipmentAddress)
                {
                    newShippingAddress.CopyTo(shipmentAddress);
                }

                adapter.SaveEntity(shipment);
            }
        }

        /// <summary>
        /// Deletes existing validated addresses
        /// </summary>
        public static void DeleteExistingAddresses(ActionStepContext context, long entityId)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                DeleteExistingAddresses(adapter, entityId, entity2 => context.CommitWork.AddForDelete(entity2));
            }
        }

        /// <summary>
        /// Deletes existing validated addresses
        /// </summary>
        public static void DeleteExistingAddresses(DataAccessAdapter adapter, long entityId)
        {
            DeleteExistingAddresses(adapter, entityId, entity2 => adapter.DeleteEntity(entity2));
        }

        /// <summary>
        /// Deletes existing validated addresses
        /// </summary>
        private static void DeleteExistingAddresses(IDataAccessAdapter adapter, long entityId, Action<IEntity2> deleteAction)
        {
            // Retrieve the addresses 
            LinqMetaData metaData = new LinqMetaData(adapter);
            List<ValidatedAddressEntity> addressesToDelete = metaData.ValidatedAddress.Where(x => x.ConsumerID == entityId).ToList();

            // Mark each address for deletion
            addressesToDelete.ForEach(x =>
            {
                AddressEntity addressToDelete = new AddressEntity {AddressID = x.AddressID, IsNew = false};

                deleteAction(x);
                deleteAction(addressToDelete);
            });
        }
    }
}
