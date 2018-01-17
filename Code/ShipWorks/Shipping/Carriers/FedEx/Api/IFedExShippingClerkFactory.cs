using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Factory responsible for creating the correct IFedExShippingClerk for a FedEx shipment.
    /// </summary>
    public interface IFedExShippingClerkFactory
    {
        /// <summary>
        /// Creates a shipping clerk
        /// </summary>
        IFedExShippingClerk Create();

        /// <summary>
        /// Creates a shipping clerk for a shipment
        /// </summary>
        IFedExShippingClerk Create(IShipmentEntity shipment);

        /// <summary>
        /// Creates a shipping clerk for a counter rates shipment
        /// </summary>
        IFedExShippingClerk CreateForCounterRates(IShipmentEntity shipment);
    }
}