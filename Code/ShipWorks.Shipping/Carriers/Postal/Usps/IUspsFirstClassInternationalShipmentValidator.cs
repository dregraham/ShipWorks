using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// represents an interface to validate Usps First Class Internatioinal shipments
    /// </summary>
    public interface IUspsFirstClassInternationalShipmentValidator
    {
        /// <summary>
        /// validate the shipment
        /// </summary>
        void ValidateShipment(IShipmentEntity shipment);
    }
}