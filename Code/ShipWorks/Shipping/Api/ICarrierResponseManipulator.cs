using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.Api
{
    /// <summary>
    /// Manipulates the shipment based on the Response from the carrier
    /// </summary>
    public interface ICarrierResponseManipulator
    {
        /// <summary>
        /// Manipulates a ShipmentEntity contained in the carrierResponse
        /// </summary>
        /// <param name="carrierResponse">The carrier response.</param>
        void Manipulate(ICarrierResponse carrierResponse);
    }
}