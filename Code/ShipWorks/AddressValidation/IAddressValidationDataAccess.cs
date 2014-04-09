using System.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Defines database interaction required by address validation
    /// </summary>
    /// <summary>This interface abstracts data access so that validation classes don't have to deal
    /// with the differences between SqlAdapter and ActionTaskContext. It also allows them to be tested.</summary>
    public interface IAddressValidationDataAccess
    {
        /// <summary>
        /// Allow shipments to be queried
        /// </summary>
        IQueryable<ShipmentEntity> Shipment { get; }

        /// <summary>
        /// Allow validated addresses to be queried
        /// </summary>
        IQueryable<ValidatedAddressEntity> ValidatedAddress { get; }

        /// <summary>
        /// Delete an entity from the database
        /// </summary>
        void DeleteEntity(IEntity2 entity);

        /// <summary>
        /// Save an entity to the database
        /// </summary>
        void SaveEntity(IEntity2 entity);
    }
}