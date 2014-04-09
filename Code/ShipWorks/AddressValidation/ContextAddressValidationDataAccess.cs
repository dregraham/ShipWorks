using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Actions;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Linq;

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
        /// Allow shipments to be queried
        /// </summary>
        public ILinqCollections LinqCollections
        {
            get
            {
                return new LLBLGenLinqCollections(new LinqMetaData(new SqlAdapter()));
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