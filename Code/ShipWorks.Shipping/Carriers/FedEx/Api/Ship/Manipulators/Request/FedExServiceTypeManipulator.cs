using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding service type information to the FedEx request
    /// </summary>
    public class FedExServiceTypeManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExServiceTypeManipulator" /> class.
        /// </summary>
        public FedExServiceTypeManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExServiceTypeManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExServiceTypeManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Add the Service Type to the FedEx carrier request
        /// </summary>
        /// <param name="request">The FedEx carrier request</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Get the RequestedShipment object for the request
            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            FedExShipmentEntity fedExShipment = request.ShipmentEntity.FedEx;

            // Set the service type for the shipment
            requestedShipment.ServiceType = FedExRequestManipulatorUtilities.GetApiServiceType((FedExServiceType)fedExShipment.Service);
        }
    }
}
