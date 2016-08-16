using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// A factory interface for creating ICarrierServiceManager objects.
    /// </summary>
    public interface IUpsServiceManagerFactory
    {
        /// <summary>
        /// Creates the an ICarrierServiceManager appropriate for the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>An ICarrierServiceManager object.</returns>
        IUpsServiceManager Create(ShipmentEntity shipment);
    }
}
