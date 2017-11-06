using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding master tracking information if not first package
    /// </summary>
    public class FedExMasterTrackingManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExMasterTrackingManipulator" /> class.
        /// </summary>
        public FedExMasterTrackingManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExMasterTrackingManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExMasterTrackingManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// A master tracking information if not first package
        /// </summary>
        public override void Manipulate(CarrierRequest request)
        {
            // Get the RequestedShipment object for the request
            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            if (request.SequenceNumber > 0)
            {
                requestedShipment.MasterTrackingId = new TrackingId()
                {
                    FormId = request.ShipmentEntity.FedEx.MasterFormID,
                    TrackingNumber = FedExUtility.GetTrackingNumberForApi(request.ShipmentEntity.TrackingNumber, request.ShipmentEntity.FedEx)
                };
            }
        }
    }
}
