using System;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the dropoff type property of the
    /// FedEx API's RateRequest object.
    /// </summary>
    public class FedExRatePickupManipulator : IFedExRateRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options) => true;

        /// <summary>
        /// Add the Recipient info to the FedEx carrier request
        /// </summary>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            InitializeRequest(request);

            request.RequestedShipment.DropoffType = GetDropoffType(shipment.FedEx);
            request.RequestedShipment.DropoffTypeSpecified = true;

            return request;
        }

        /// <summary>
        /// Gets the type of the dropoff.
        /// </summary>
        /// <param name="fedExShipment">The fed ex shipment.</param>
        /// <returns>The FedEx API DropoffType value.</returns>
        /// <exception cref="System.InvalidOperationException">Invalid FedEx ServiceType</exception>
        private DropoffType GetDropoffType(IFedExShipmentEntity fedExShipment)
        {
            switch ((FedExDropoffType) fedExShipment.DropoffType)
            {
                case FedExDropoffType.BusinessServiceCenter: return DropoffType.BUSINESS_SERVICE_CENTER;
                case FedExDropoffType.DropBox: return DropoffType.DROP_BOX;
                case FedExDropoffType.RegularPickup: return DropoffType.REGULAR_PICKUP;
                case FedExDropoffType.RequestCourier: return DropoffType.REQUEST_COURIER;
                case FedExDropoffType.Station: return DropoffType.STATION;
            }

            throw new InvalidOperationException("Invalid FedEx ServiceType " + (FedExDropoffType) fedExShipment.DropoffType);
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        public void InitializeRequest(RateRequest request) =>
            request.Ensure(x => x.RequestedShipment);
    }
}
