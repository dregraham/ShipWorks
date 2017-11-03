using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response
{
    /// <summary>
    /// Label repository for FedEx
    /// </summary>
    public interface IFedExLabelRepository
    {
        /// <summary>
        /// Saves Labels and updates shipment.
        /// </summary>
        void SaveLabels(IShipmentEntity shipment, ProcessShipmentReply reply);

        /// <summary>
        /// If we had saved an image for this shipment previously, but the shipment errored out later (like for an MPS), then clear before
        /// we start.
        /// </summary>
        void ClearReferences(IShipmentEntity shipment);
    }
}