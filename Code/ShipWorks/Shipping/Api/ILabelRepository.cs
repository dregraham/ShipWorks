using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;

namespace ShipWorks.Shipping.Carriers.Api
{
    /// <summary>
    /// Interface for an object which saves Labels to the database and associates labels with the
    /// shipment.
    /// </summary>
    public interface ILabelRepository
    {
        /// <summary>
        /// Saves Labels and updates shipment.
        /// </summary>
        void SaveLabels(ICarrierResponse response);

        /// <summary>
        /// If we had saved an image for this shipment previously, but the shipment errored out later (like for an MPS), then clear before
        /// we start.
        /// </summary>
        /// <param name="shipment"></param>
        void ClearReferences(ShipmentEntity shipment);
    }
}