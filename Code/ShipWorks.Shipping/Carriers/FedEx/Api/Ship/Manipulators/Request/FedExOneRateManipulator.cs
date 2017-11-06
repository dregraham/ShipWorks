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
    /// FedEx IFedExNativeShipmentRequest object with to indicate that the One Rate special
    /// service has been requested based on the service type of the FedEx shipment.
    /// </summary>
    public class FedExOneRateManipulator : IFedExShipRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, int sequenceNumber) =>
            FedExUtility.OneRateServiceTypes.Contains((FedExServiceType) shipment.FedEx.Service);

        /// <summary>
        /// Manipulates the carrier request to conditionally set whether the request should
        /// reflect the One Rate special service type.
        /// </summary>
        public GenericResult<ProcessShipmentRequest> Manipulate(IShipmentEntity shipment, ProcessShipmentRequest request, int sequenceNumber)
        {
            var servicesRequested = request.Ensure(x => x.RequestedShipment)
                .Ensure(x => x.SpecialServicesRequested);

            servicesRequested.SpecialServiceTypes = servicesRequested
                .Ensure(x => x.SpecialServiceTypes)
                .Append(ShipmentSpecialServiceType.FEDEX_ONE_RATE)
                .ToArray();

            return request;
        }
    }
}
