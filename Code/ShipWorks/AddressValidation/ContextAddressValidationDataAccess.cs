using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.AddressValidation.Predicates;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Database interaction required by address validation that is provided by an ActionStepContext
    /// </summary>
    public class ContextAddressValidationDataAccess : IAddressValidationDataAccess
    {
        private readonly ActionStepContext context;

        /// <summary>
        /// Instantiate the object
        /// </summary>
        public ContextAddressValidationDataAccess(ActionStepContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get validated addresses for the given consuemer and prefix
        /// </summary>
        public IEnumerable<ValidatedAddressEntity> GetValidatedAddressesByConsumerAndPrefix(long consumerId, string prefix)
        {
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                return adapter.GetCollectionFromPredicate<ValidatedAddressEntity>(new AddressSuggestionsForConsumerPredicate(consumerId, prefix)); 
            }
        }

        /// <summary>
        /// Get unprocessed shipments for the given order
        /// </summary>
        public IEnumerable<ShipmentEntity> GetUnprocessedShipmentsForOrder(long orderId)
        {
            using (SqlAdapter adapter = SqlAdapter.Default)
            {
                return adapter.GetCollectionFromPredicate<ShipmentEntity>(new UnprocessedShipmentsForOrderPredicate(orderId));
            }
        }

        /// <summary>
        /// Delete an entity from the database
        /// </summary>
        public void DeleteEntity(IEntity2 entity)
        {
            context.CommitWork.AddForDelete(entity);
        }

        /// <summary>
        /// Save an entity to the database
        /// </summary>
        public void SaveEntity(IEntity2 entity)
        {
            context.CommitWork.AddForSave(entity);
        }
    }
}