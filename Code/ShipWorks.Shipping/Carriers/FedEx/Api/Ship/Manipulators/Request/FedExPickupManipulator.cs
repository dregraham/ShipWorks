using System;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// Manipulator for adding recipient information to the FedEx request
    /// </summary>
    public class FedExPickupManipulator : FedExShippingRequestManipulatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPickupManipulator" /> class.
        /// </summary>
        public FedExPickupManipulator()
            : this(new FedExSettings(new FedExSettingsRepository()))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExPickupManipulator" /> class.
        /// </summary>
        /// <param name="fedExSettings">The fed ex settings.</param>
        public FedExPickupManipulator(FedExSettings fedExSettings)
            : base(fedExSettings)
        {
        }

        /// <summary>
        /// Add the Recipient info to the FedEx carrier request
        /// </summary>
        /// <param name="request">The FedEx carrier request</param>
        public override void Manipulate(CarrierRequest request)
        {
            // Get the RequestedShipment object for the request
            RequestedShipment requestedShipment = FedExRequestManipulatorUtilities.GetShipServiceRequestedShipment(request);

            // Set the drop off type for the shipment
            requestedShipment.DropoffType = FedExRequestManipulatorUtilities.GetShipmentDropoffType((FedExDropoffType)request.ShipmentEntity.FedEx.DropoffType);

            // Set the shipment date/time
            requestedShipment.ShipTimestamp = new DateTime(request.ShipmentEntity.ShipDate.Ticks, DateTimeKind.Local);
        }
    }
}
