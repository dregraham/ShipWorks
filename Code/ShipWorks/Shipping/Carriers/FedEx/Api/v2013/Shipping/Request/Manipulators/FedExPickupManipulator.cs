using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.v2013.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013.Shipping.Request.Manipulators
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
