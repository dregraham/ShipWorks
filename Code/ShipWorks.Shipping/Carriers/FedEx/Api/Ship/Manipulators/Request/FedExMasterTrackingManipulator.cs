using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// Manipulator for adding master tracking information if not first package
    /// </summary>
    public class FedExMasterTrackingManipulator : IFedExShipRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) => sequenceNumber > 0;

        /// <summary>
        /// A master tracking information if not first package
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            request.Ensure(x => x.RequestedShipment)
                .MasterTrackingId = new TrackingId()
                {
                    FormId = shipment.FedEx.MasterFormID,
                    TrackingNumber = FedExUtility.GetTrackingNumberForApi(shipment.TrackingNumber, shipment.FedEx)
                };

            return request;
        }
    }
}
