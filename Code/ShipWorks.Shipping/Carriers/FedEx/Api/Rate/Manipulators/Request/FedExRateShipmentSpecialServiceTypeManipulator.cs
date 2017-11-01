using System;
using System.Collections.Generic;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will add the appropriate special
    /// shipment type attributes to the ProcessShipmentRequest object.
    /// </summary>
    public class FedExRateShipmentSpecialServiceTypeManipulator : IFedExRateRequestManipulator
    {
        readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExRateShipmentSpecialServiceTypeManipulator(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options) => true;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            InitializeRequest(request);

            // Use the ship date of the shipment entity to determine the ship time stamp; not looking at the 
            // ShipTimestamp property of the request here, because there's no guarantee it's been set
            DateTime shipTimestamp = new DateTime(shipment.ShipDate.Ticks, DateTimeKind.Local);
            request.RequestedShipment.ShipTimestamp = shipTimestamp;
            request.RequestedShipment.ShipTimestampSpecified = true;

            // Since we'll be assigning this list back to the native request, create a list of the existing 
            // special service types that are on the request already so we don't overwrite anything
            List<ShipmentSpecialServiceType> specialServiceTypes = new List<ShipmentSpecialServiceType>();
            if (request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes != null)
            {
                specialServiceTypes.AddRange(request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes);
            }

            if (shipTimestamp.Date != dateTimeProvider.Today)
            {
                // This is a future delivery
                specialServiceTypes.Add(ShipmentSpecialServiceType.FUTURE_DAY_SHIPMENT);
            }

            // If dropoff type is regular pick up or courier, then we need to send ShipmentSpecialServiceType.SATURDAY_PICKUP
            // Otherwise, the customer will not be asking for a pickup.
            FedExDropoffType dropoffType = (FedExDropoffType) shipment.FedEx.DropoffType;
            if (dropoffType == FedExDropoffType.RegularPickup || dropoffType == FedExDropoffType.RequestCourier)
            {
                if (shipTimestamp.DayOfWeek == DayOfWeek.Saturday)
                {
                    // This will be a Saturday pickup
                    specialServiceTypes.Add(ShipmentSpecialServiceType.SATURDAY_PICKUP);
                }
            }

            // Pull out the FedEx shipment info from the shipment entity and check if they want Saturday 
            // delivery and whether it could be delivered on a Saturday
            var fedExShipmentEntity = shipment.FedEx;
            if (fedExShipmentEntity.SaturdayDelivery && FedExUtility.CanDeliverOnSaturday((FedExServiceType) fedExShipmentEntity.Service, shipTimestamp))
            {
                specialServiceTypes.Add(ShipmentSpecialServiceType.SATURDAY_DELIVERY);
            }

            if (shipment.FedEx.ReturnsClearance)
            {
                specialServiceTypes.Add(ShipmentSpecialServiceType.RETURNS_CLEARANCE);
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
        private void InitializeRequest(RateRequest request)
        {
            request.Ensure(x => x.RequestedShipment)
                .Ensure(x => x.SpecialServicesRequested)
                .Ensure(x => x.SpecialServiceTypes);
        }
    }
}
