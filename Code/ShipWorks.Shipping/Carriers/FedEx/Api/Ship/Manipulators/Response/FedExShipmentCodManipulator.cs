using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Response
{
    /// <summary>
    /// Adds COD information to a FedEx Shipment - Does not save COD label
    /// </summary>
    public class FedExShipmentCodManipulator : IFedExShipResponseManipulator
    {
        /// <summary>
        /// Performs manipulation
        /// </summary>
        public GenericResult<ShipmentEntity> Manipulate(ProcessShipmentReply response, ProcessShipmentRequest request, ShipmentEntity shipment)
        {
            if (response.CompletedShipmentDetail.MasterTrackingId != null)
            {
                TrackingId codTrackingID = response.CompletedShipmentDetail.MasterTrackingId;
                shipment.FedEx.CodTrackingNumber = codTrackingID.TrackingNumber;
                shipment.FedEx.CodTrackingFormID = codTrackingID.FormId;
            }

            return shipment;
        }
    }
}
