using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation.Predicates;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Database interaction required by address validation that is provided by a DataAccessAdapter
    /// </summary>
    public class AdapterAddressValidationDataAccess : IAddressValidationDataAccess
    {
        private readonly SqlAdapter adapter;

        /// <summary>
        /// Instantiate the object
        /// </summary>
        public AdapterAddressValidationDataAccess(SqlAdapter adapter)
        {
            this.adapter = adapter;
        }

        /// <summary>
        /// Get validated addresses for the given consuemer and prefix
        /// </summary>
        public IEnumerable<ValidatedAddressEntity> GetValidatedAddressesByConsumerAndPrefix(long consumerId, string prefix)
        {
            return adapter.GetCollectionFromPredicate<ValidatedAddressEntity>(new AddressSuggestionsForConsumerPredicate(consumerId, prefix));
        }

        /// <summary>
        /// Get unprocessed shipments for the given order
        /// </summary>
        public IEnumerable<ShipmentEntity> GetUnprocessedShipmentsForOrder(long orderId)
        {
            return adapter.GetCollectionFromPredicate<ShipmentEntity>(new UnprocessedShipmentsForOrderPredicate(orderId));
        }

        /// <summary>
        /// Delete an entity from the database
        /// </summary>
        public void DeleteEntity(IEntity2 entity)
        {
            adapter.DeleteEntity(entity);
        }

        /// <summary>
        /// Save an entity to the database
        /// </summary>
        public void SaveEntity(IEntity2 entity)
        {
            adapter.SaveAndRefetch(entity);
        }
    }
}