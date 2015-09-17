using ShipWorks.Data.Model.EntityClasses;
using System.Threading.Tasks;

namespace ShipWorks.AddressValidation
{
    public interface IValidatedAddressManager
    {
        /// <summary>
        /// Validate a single shipment
        /// </summary>
        Task ValidateShipmentAsync(ShipmentEntity shipment, AddressValidator validator);
    }
}
