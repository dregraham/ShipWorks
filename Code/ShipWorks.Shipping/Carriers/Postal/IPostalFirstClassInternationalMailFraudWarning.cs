using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// represents an interface to validate Usps First Class Internatioinal shipments
    /// </summary>
    public interface IPostalFirstClassInternationalMailFraudWarning
    {
        /// <summary>
        /// Warn the user
        /// </summary>
        void Warn(IShipmentEntity shipment);
    }
}