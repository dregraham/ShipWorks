using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators
{
    /// <summary>
    /// Adds COD informaiton to a FedEx Shipment - Does not save COD label
    /// </summary>
    public class FedExShipmentCodManipulator : ICarrierResponseManipulator
    {
        private IFedExNativeShipmentReply processShipmentReply;

        private ShipmentEntity shipment;

        /// <summary>
        /// Performs manipulation
        /// </summary>
        public void Manipulate(ICarrierResponse carrierResponse)
        {
            FedExShipResponse fedExShipResponse = (FedExShipResponse) carrierResponse;

            shipment = fedExShipResponse.Shipment;
            processShipmentReply = fedExShipResponse.NativeResponse as IFedExNativeShipmentReply;

            // For COD we have to save off the cod tracking info.  Null for ground

            if (processShipmentReply.CompletedShipmentDetail.MasterTrackingId != null)
            {
                TrackingId codTrackingID = processShipmentReply.CompletedShipmentDetail.MasterTrackingId;
                shipment.FedEx.CodTrackingNumber = codTrackingID.TrackingNumber;
                shipment.FedEx.CodTrackingFormID = codTrackingID.FormId;
            }
        }
    }
}
