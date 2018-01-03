using System;
using System.Collections.Generic;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will add the appropriate special
    /// shipment type attributes to the IFedExNativeShipmentRequest object.
    /// </summary>
    public class FedExShipmentSpecialServiceTypeManipulator : IFedExShipRequestManipulator
    {
        private readonly IFedExSettingsRepository settings;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShipmentSpecialServiceTypeManipulator(IFedExSettingsRepository settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Does this manipulator apply to this shipment
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber)
        {
            return true;
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(shipment, request);

            // Use the ship date of the shipment entity to determine the ship time stamp; not looking at the 
            // ShipTimestamp property of the request here, because there's no guarantee it's been set
            DateTime shipTimestamp = new DateTime(shipment.ShipDate.Ticks, DateTimeKind.Local);

            // Since we'll be assigning this list back to the native request, create a list of the existing 
            // special service types that are on the request already so we don't overwrite anything
            List<ShipmentSpecialServiceType> specialServiceTypes = new List<ShipmentSpecialServiceType>();
            if (request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes != null)
            {
                specialServiceTypes.AddRange(request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            }

            if (shipTimestamp.Date != DateTime.Today && !shipment.FedEx.ReturnSaturdayPickup)
            {
                // This is a future delivery
                specialServiceTypes.Add(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT);
            }

            // If dropoff type is regular pick up or courier, then we need to send ShipmentSpecialServiceType.SATURDAY_PICKUP
            // Otherwise, the customer will not be asking for a pickup.
            FedExDropoffType dropoffType = (FedExDropoffType) shipment.FedEx.DropoffType;
            if ((dropoffType == FedExDropoffType.RegularPickup || dropoffType == FedExDropoffType.RequestCourier) &&
                shipTimestamp.DayOfWeek == DayOfWeek.Saturday)
            {
                // This will be a Saturday pickup
                specialServiceTypes.Add(ShipmentSpecialServiceType.SATURDAY_PICKUP);
            }

            // Pull out the FedEx shipment info from the shipment entity and check if they want Saturday 
            // delivery and whether it could be delivered on a Saturday
            IFedExShipmentEntity fedExShipmentEntity = shipment.FedEx;
            if (fedExShipmentEntity.SaturdayDelivery && FedExUtility.CanDeliverOnSaturday((FedExServiceType) fedExShipmentEntity.Service, shipTimestamp))
            {
                // Saturday delivery is available 
                specialServiceTypes.Add(ShipmentSpecialServiceType.SATURDAY_DELIVERY);
            }

            if (fedExShipmentEntity.ThirdPartyConsignee)
            {
                specialServiceTypes.Add(ShipmentSpecialServiceType.THIRD_PARTY_CONSIGNEE);
            }

            // Assign the updated special service types list back to the request
            request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = specialServiceTypes.ToArray();

            return request;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private void InitializeRequest(IShipmentEntity shipment, ProcessShipmentRequest request)
        {
            MethodConditions.EnsureArgumentIsNotNull(request, nameof(request));
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            request.Ensure(r => r.RequestedShipment)
                .Ensure(rs => rs.SpecialServicesRequested)
                .Ensure(ssr => ssr.SpecialServiceTypes);
        }
    }
}
