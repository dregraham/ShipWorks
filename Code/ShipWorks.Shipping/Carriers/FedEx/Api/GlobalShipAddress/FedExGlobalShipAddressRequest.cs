using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Request
{
    /// <summary>
    /// GlobalShipAddress Request - Used to get Hold At Location addresses.
    /// </summary>
    [Component]
    public class FedExGlobalShipAddressRequest : IFedExGlobalShipAddressRequest
    {
        private readonly IFedExServiceGatewayFactory serviceGatewayFactory;
        private readonly IFedExSettingsRepository settingsRepository;
        private readonly IEnumerable<Func<IFedExSettingsRepository, IFedExGlobalShipAddressRequestManipulator>> manipulatorFactory;
        private readonly Func<SearchLocationsReply, IFedExGlobalShipAddressResponse> createResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExGlobalShipAddressRequest" /> class.
        /// </summary>
        public FedExGlobalShipAddressRequest(
            IFedExServiceGatewayFactory serviceGatewayFactory,
            IFedExSettingsRepository settingsRepository,
            IEnumerable<Func<IFedExSettingsRepository, IFedExGlobalShipAddressRequestManipulator>> manipulatorFactory,
            Func<SearchLocationsReply, IFedExGlobalShipAddressResponse> createResponse)
        {
            this.serviceGatewayFactory = serviceGatewayFactory;
            this.createResponse = createResponse;
            this.manipulatorFactory = manipulatorFactory;
            this.settingsRepository = settingsRepository;
        }

        /// <summary>
        /// Submits the request to the carrier API.
        /// </summary>
        /// <returns>
        /// The GlobalShipReply or an error
        /// </returns>
        public GenericResult<IFedExGlobalShipAddressResponse> Submit(IShipmentEntity shipment) =>
            manipulatorFactory
                .Select(x => x(settingsRepository))
                .Aggregate(
                    new SearchLocationsRequest(),
                    (req, manipulator) => manipulator.Manipulate(shipment, req))
                .Map(x => serviceGatewayFactory.Create(settingsRepository).GlobalShipAddressInquiry(x))
                .Map(createResponse);
    }
}
