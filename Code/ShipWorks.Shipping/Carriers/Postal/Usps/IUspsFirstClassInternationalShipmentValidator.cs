using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// represents an interface to validate Usps First Class Internatioinal shipments
    /// </summary>
    [Service]
    public interface IUspsFirstClassInternationalShipmentValidator
    {
        /// <summary>
        /// validate the shipment
        /// </summary>
        void ValidateShipment(ShipmentEntity shipment);
    }
}