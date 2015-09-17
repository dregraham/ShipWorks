using ShipWorks.Data.Model.EntityClasses;
using System.Threading.Tasks;

namespace ShipWorks.AddressValidation
{
    /// <summary>
    /// Wrap the validated address manager in an instance
    /// </summary>
    public class ValidatedAddressManagerWrapper : IValidatedAddressManager
    {
        /// <summary>
        /// Validate a single shipment
        /// </summary>
        public Task ValidateShipmentAsync(ShipmentEntity shipment, AddressValidator validator)
        {
            return ValidatedAddressManager.ValidateShipmentAsync(shipment, validator);
        }
    }
}
