using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate
{
    /// <summary>
    /// An implementation of the CarrierRequest that issues FedEx RateRequest request types.
    /// </summary>
    [Component]
    public class FedExRateRequest : IFedExRateRequest
    {
        private readonly IFedExServiceGatewayFactory serviceGatewayFactory;
        private readonly IFedExSettingsRepository settingsRepository;
        private readonly IEnumerable<Func<IFedExSettingsRepository, IFedExRateRequestManipulator>> manipulatorFactory;
        private readonly Func<RateReply, IFedExRateResponse> createRateRespose;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExRateRequest" /> class.
        /// </summary>
        public FedExRateRequest(
            IFedExServiceGatewayFactory serviceGatewayFactory,
            IFedExSettingsRepository settingsRepository,
            IEnumerable<Func<IFedExSettingsRepository, IFedExRateRequestManipulator>> manipulatorFactory,
            Func<RateReply, IFedExRateResponse> createRateRespose)
        {
            this.createRateRespose = createRateRespose;
            this.manipulatorFactory = manipulatorFactory;
            this.serviceGatewayFactory = serviceGatewayFactory;
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Submits the request to the carrier API.
        /// </summary>
        public IFedExRateResponse Submit(IShipmentEntity shipment) =>
            Submit(shipment, FedExRateRequestOptions.None);

        /// <summary>
        /// Submits the request to the carrier API.
        /// </summary>
        public IFedExRateResponse Submit(IShipmentEntity shipment, FedExRateRequestOptions options)
        {
            var request = manipulatorFactory
                .Select(x => x(settingsRepository))
                .Where(x => x.ShouldApply(shipment, options))
                .Aggregate(new RateRequest(), (req, manipulator) => manipulator.Manipulate(shipment, req));

            RateReply nativeResponse = serviceGatewayFactory.Create(settingsRepository).GetRates(request, shipment);

            return createRateRespose(nativeResponse);
        }
    }
}
