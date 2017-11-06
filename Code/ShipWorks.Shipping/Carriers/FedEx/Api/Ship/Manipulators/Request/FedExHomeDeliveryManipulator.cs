using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.Manipulators
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that configures the 
    /// FedEx IFedExNativeShipmentRequest object with home delivery options depending on the 
    /// shipment entity.
    /// </summary>
    public class FedExHomeDeliveryManipulator : IFedExShipRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) =>
            shipment.FedEx.Service == (int) FedExServiceType.GroundHomeDelivery &&
            shipment.FedEx.HomeDeliveryType != (int) FedExHomeDeliveryType.None;

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            InitializeRequest(request);

            // This shipment is configured for home delivery, so configure the service type and home delivery details
            ConfigureServiceType(request);
            ConfigureHomeDeliveryDetails(request, (FedExHomeDeliveryType) shipment.FedEx.HomeDeliveryType, shipment.FedEx);

            // Finally use the home delivery instructions for the shipment
            request.RequestedShipment.DeliveryInstructions = shipment.FedEx.HomeDeliveryInstructions;

            return request;
        }

        /// <summary>
        /// Gets the service type list.
        /// </summary>
        private void ConfigureServiceType(ProcessShipmentRequest request)
        {
            var requestedServices = request.RequestedShipment.SpecialServicesRequested;
            requestedServices.SpecialServiceTypes = requestedServices.SpecialServiceTypes
                .Append(ShipmentSpecialServiceType.HOME_DELIVERY_PREMIUM)
                .ToArray();
        }

        /// <summary>
        /// Configures the home delivery details of the IFedExNativeShipmentRequest.
        /// </summary>
        /// <param name="request">The native request.</param>
        /// <param name="homeDeliveryType">Type of the home delivery.</param>
        /// <param name="fedExShipment">The FedEx shipment.</param>
        private void ConfigureHomeDeliveryDetails(IFedExNativeShipmentRequest request, FedExHomeDeliveryType homeDeliveryType, IFedExShipmentEntity fedExShipment)
        {
            // Figure out the FedEx API premium type value
            HomeDeliveryPremiumType premiumType;
            switch (homeDeliveryType)
            {
                case FedExHomeDeliveryType.DateCertain:
                    premiumType = HomeDeliveryPremiumType.DATE_CERTAIN;
                    break;
                case FedExHomeDeliveryType.Evening:
                    premiumType = HomeDeliveryPremiumType.EVENING;
                    break;
                default:
                    premiumType = HomeDeliveryPremiumType.APPOINTMENT;
                    break;
            }

            HomeDeliveryPremiumDetail homeDeliveryDetail = new HomeDeliveryPremiumDetail();
            homeDeliveryDetail.HomeDeliveryPremiumType = premiumType;
            homeDeliveryDetail.PhoneNumber = fedExShipment.HomeDeliveryPhone;

            if (homeDeliveryType == FedExHomeDeliveryType.DateCertain)
            {
                homeDeliveryDetail.Date = fedExShipment.HomeDeliveryDate;
                homeDeliveryDetail.DateSpecified = true;
            }

            request.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail = homeDeliveryDetail;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private void InitializeRequest(ProcessShipmentRequest request) =>
            request.Ensure(x => x.RequestedShipment)
                .Ensure(x => x.SpecialServicesRequested)
                .Ensure(x => x.SpecialServiceTypes);
    }
}
