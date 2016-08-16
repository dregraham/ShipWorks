using System.Threading.Tasks;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Manage validated addresses
    /// </summary>
    public interface IValidatedAddressManager
    {
        /// <summary>
        /// Validate a single shipment
        /// </summary>
        Task ValidateShipmentAsync(ShipmentEntity shipment);

        /// <summary>
        /// Copy all the validated address from one entity to another
        /// </summary>
        void CopyValidatedAddresses(SqlAdapter sqlAdapter, long fromEntityId, string fromPrefix, long toEntityId, string toPrefix);
    }
}
