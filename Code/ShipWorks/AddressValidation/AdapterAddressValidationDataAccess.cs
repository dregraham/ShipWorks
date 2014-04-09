using System;
using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Adapter;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.Linq;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Database interaction required by address validation that is provided by a DataAccessAdapter
    /// </summary>
    public class AdapterAddressValidationDataAccess : IAddressValidationDataAccess
    {
        private readonly DataAccessAdapter adapter;

        /// <summary>
        /// Instantiate the object
        /// </summary>
        public AdapterAddressValidationDataAccess(DataAccessAdapter adapter)
        {
            this.adapter = adapter;
        }

        /// <summary>
        /// Allow shipments to be queried
        /// </summary>
        public ILinqCollections LinqCollections
        {
            get
            {
                return new LLBLGenLinqCollections(new LinqMetaData(adapter));
            }
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
            adapter.SaveEntity(entity);
        }
    }
}