using System;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// Manipulator for adding recipient information to the FedEx request
    /// </summary>
    public class FedExPickupManipulator : IFedExShipRequestManipulator
    {
        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) => true;

        /// <summary>
        /// Add the Recipient info to the FedEx carrier request
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            RequestedShipment requestedShipment = request.Ensure(r => r.RequestedShipment);

            // Set the drop off type for the shipment
            requestedShipment.DropoffType = FedExRequestManipulatorUtilities.GetShipmentDropoffType((FedExDropoffType) shipment.FedEx.DropoffType);

            // Set the shipment date/time
            requestedShipment.ShipTimestamp = new DateTime(shipment.ShipDate.Ticks, DateTimeKind.Local);

            return request;
        }
    }
}
