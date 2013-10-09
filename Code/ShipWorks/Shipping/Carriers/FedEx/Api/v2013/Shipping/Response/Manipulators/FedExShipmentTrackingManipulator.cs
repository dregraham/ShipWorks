using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Response.Manipulators
{
    /// <summary>
    /// Manipulator to add tracking informaiton to shipment
    /// </summary>
    public class FedExShipmentTrackingManipulator : ICarrierResponseManipulator
    {
        private IFedExNativeShipmentReply processShipmentReply;

        private ShipmentEntity shipment;

        /// <summary>
        /// Adds tracking information to shipment
        /// </summary>
        public void Manipulate(ICarrierResponse carrierResponse)
        {
            FedExShipResponse fedExShipResponse = (FedExShipResponse) carrierResponse;

            shipment = fedExShipResponse.Shipment;
            processShipmentReply = fedExShipResponse.NativeResponse as IFedExNativeShipmentReply;

            if (processShipmentReply.CompletedShipmentDetail.CompletedPackageDetails[0].SequenceNumber == "1")
            {
                SetShipmentTrackingNumber();
            }

            SetPackageTrackingNumber();
        }

        /// <summary>
        /// Sets the package tracking number
        /// </summary>
        private void SetPackageTrackingNumber()
        {
            CompletedPackageDetail completedPackageDetail = processShipmentReply.CompletedShipmentDetail.CompletedPackageDetails[0];

            FedExPackageEntity fedExPackageEntity = shipment.FedEx.Packages[int.Parse(completedPackageDetail.SequenceNumber) - 1];
            fedExPackageEntity.TrackingNumber = completedPackageDetail.TrackingIds[0].TrackingNumber;
        }

        /// <summary>
        /// Sets the shipment tracking number (master for multiple packages, package tracking if just one)
        /// </summary>
        private void SetShipmentTrackingNumber()
        {
            if (processShipmentReply.CompletedShipmentDetail.MasterTrackingId != null)
            {
                shipment.TrackingNumber = processShipmentReply.CompletedShipmentDetail.MasterTrackingId.TrackingNumber;
                shipment.FedEx.MasterFormID = processShipmentReply.CompletedShipmentDetail.MasterTrackingId.FormId;
            }
            else
            {
                shipment.TrackingNumber = processShipmentReply.CompletedShipmentDetail.CompletedPackageDetails[0].TrackingIds[0].TrackingNumber;
                shipment.FedEx.MasterFormID = "";
            }
        }
    }
}
