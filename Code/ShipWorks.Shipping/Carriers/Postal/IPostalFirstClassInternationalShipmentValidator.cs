using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// represents an interface to validate Usps First Class Internatioinal shipments
    /// </summary>
    public interface IPostalFirstClassInternationalShipmentValidator
    {
        /// <summary>
        /// validate the shipment
        /// </summary>
        void ValidateShipment(IShipmentEntity shipment);
    }
}