using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request.International;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    /// <summary>
    /// An implementation of the ICarrierRequestManipulator interface that will add the appropriate special
    /// shipment type attributes to the RateRequest object for obtaining FedEx One Rate rate results.
    /// </summary>
    public class FedExRateOneRateManipulator : IFedExRateRequestManipulator
    {
        /// <summary>
        /// Should the manipulator be applied
        /// </summary>
        public bool ShouldApply(IShipmentEntity shipment, FedExRateRequestOptions options) =>
            options.HasFlag(FedExRateRequestOptions.OneRate);

        /// <summary>
        /// Manipulates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        public RateRequest Manipulate(IShipmentEntity shipment, RateRequest request)
        {
            InitializeRequest(request);

            var specialServices = request.RequestedShipment.SpecialServicesRequested;
            specialServices.SpecialServiceTypes = specialServices.SpecialServiceTypes
                .Append(ShipmentSpecialServiceType.FEDEX_ONE_RATE)
                .ToArray();

            return request;
        }

        /// <summary>
        /// Initializes the request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <exception cref="System.ArgumentNullException">request</exception>
        /// <exception cref="CarrierException">An unexpected request type was provided.</exception>
        private void InitializeRequest(RateRequest request)
        {
            request.Ensure(x => x.RequestedShipment)
                .Ensure(x => x.SpecialServicesRequested)
                .Ensure(x => x.SpecialServiceTypes);
        }
    }
}
