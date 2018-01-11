using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request.International
{
    /// <summary>
    /// An ICarrierRequestManipulator implementation that modifies the InternationalTrafficInArmsRegulationsDetail
    /// attributes within the FedEx API's IFedExNativeShipmentRequest object if the shipment has a traffic in arms license number.
    /// </summary>
    public class FedExTrafficInArmsManipulator : IFedExShipRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber)
        {
            return (shipment.FedEx.InternationalTrafficInArmsService ?? false) ||
                   !string.IsNullOrEmpty(shipment.FedEx.TrafficInArmsLicenseNumber);
        }

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            // Make sure all of the properties we'll be accessing have been created
            InitializeRequest(request, shipment);

            if (shipment.FedEx.InternationalTrafficInArmsService ?? false)
            {
                AddTrafficInArmsOption(request);
            }

            if (!string.IsNullOrEmpty(shipment.FedEx.TrafficInArmsLicenseNumber))
            {
                InternationalTrafficInArmsRegulationsDetail armsDetail = new InternationalTrafficInArmsRegulationsDetail();
                armsDetail.LicenseOrExemptionNumber = shipment.FedEx.TrafficInArmsLicenseNumber;

                request.RequestedShipment.SpecialServicesRequested.InternationalTrafficInArmsRegulationsDetail = armsDetail;
            }

            return GenericResult.FromSuccess(request);
        }

        /// <summary>
        /// Adds the traffic in arms option to the request.
        /// </summary>
        private void AddTrafficInArmsOption(IFedExNativeShipmentRequest nativeRequest)
        {
            if (nativeRequest.RequestedShipment.SpecialServicesRequested == null)
            {
                nativeRequest.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            }

            List<ShipmentSpecialServiceType> services = nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes?.ToList() ?? new List<ShipmentSpecialServiceType>();
            services.Add(ShipmentSpecialServiceType.INTERNATIONAL_TRAFFIC_IN_ARMS_REGULATIONS);
            nativeRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = services.ToArray();
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        private void InitializeRequest(ProcessShipmentRequest request, IShipmentEntity shipment)
        {
            request.Ensure(x => x.RequestedShipment);
        }
    }
}
