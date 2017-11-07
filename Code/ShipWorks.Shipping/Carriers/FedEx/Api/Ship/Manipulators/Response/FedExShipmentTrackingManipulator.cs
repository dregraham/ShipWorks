using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Response
{
    /// <summary>
    /// Manipulator to add tracking information to shipment
    /// </summary>
    public class FedExShipmentTrackingManipulator : IFedExShipResponseManipulator
    {
        private ShipmentEntity shipment;
        private ProcessShipmentReply response;

        /// <summary>
        /// Adds tracking information to shipment
        /// </summary>
        public GenericResult<ShipmentEntity> Manipulate(ProcessShipmentReply response, ShipmentEntity shipment)
        {
            this.response = response;
            this.shipment = shipment;

            if (FedExUtility.IsFreightLtlService(shipment.FedEx.Service))
            {
                SetShipmentTrackingNumber();

                // TODO: See if we need to handle packages.
            }
            else
            {
                if (response.CompletedShipmentDetail.CompletedPackageDetails[0].SequenceNumber == "1")
                {
                    SetShipmentTrackingNumber();
                }

                SetPackageTrackingNumber();
            }

            return shipment;
        }

        /// <summary>
        /// Sets the package tracking number
        /// </summary>
        private void SetPackageTrackingNumber()
        {
            CompletedPackageDetail completedPackageDetail = response.CompletedShipmentDetail.CompletedPackageDetails[0];

            FedExPackageEntity fedExPackageEntity = shipment.FedEx.Packages[int.Parse(completedPackageDetail.SequenceNumber) - 1];
            fedExPackageEntity.TrackingNumber = completedPackageDetail.TrackingIds[0].TrackingNumber;
        }

        /// <summary>
        /// Sets the shipment tracking number (master for multiple packages, package tracking if just one)
        /// </summary>
        private void SetShipmentTrackingNumber()
        {
            if (response.CompletedShipmentDetail.MasterTrackingId != null)
            {
                shipment.TrackingNumber = response.CompletedShipmentDetail.MasterTrackingId.TrackingNumber;
                shipment.FedEx.MasterFormID = response.CompletedShipmentDetail.MasterTrackingId.FormId;
            }
            else
            {
                // To track SmartPost on USPS.com, we need to save the application id for removal later
                if ((FedExServiceType)shipment.FedEx.Service == FedExServiceType.SmartPost)
                {
                    shipment.FedEx.SmartPostUspsApplicationId = response.CompletedShipmentDetail.CompletedPackageDetails[0].TrackingIds[0].UspsApplicationId;
                }

                string trackingNumber = response.CompletedShipmentDetail.CompletedPackageDetails[0].TrackingIds[0].TrackingNumber; ;
                shipment.TrackingNumber = FedExUtility.BuildTrackingNumber(trackingNumber, shipment.FedEx);

                shipment.FedEx.MasterFormID = "";
            }
        }
    }
}
