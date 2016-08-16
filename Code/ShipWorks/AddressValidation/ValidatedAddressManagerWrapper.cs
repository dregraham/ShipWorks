using System.Threading.Tasks;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Wrap the validated address manager in an instance
    /// </summary>
    public class ValidatedAddressManagerWrapper : IValidatedAddressManager
    {
        private readonly IAddressValidator addressValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        public ValidatedAddressManagerWrapper(IAddressValidator addressValidator)
        {
            this.addressValidator = addressValidator;
        }

        /// <summary>
        /// Validate a single shipment
        /// </summary>
        public Task ValidateShipmentAsync(ShipmentEntity shipment) =>
            ValidatedAddressManager.ValidateShipmentAsync(shipment, addressValidator);

        /// <summary>
        /// Copy all the validated address from one entity to another
        /// </summary>
        public void CopyValidatedAddresses(SqlAdapter sqlAdapter, long fromEntityId, string fromPrefix, long toEntityId, string toPrefix) =>
            ValidatedAddressManager.CopyValidatedAddresses(sqlAdapter, fromEntityId, fromPrefix, toEntityId, toPrefix);
    }
}
