using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Ship
{
    /// <summary>
    /// An implementation of the CarrierRequest interface that sends a request to the FedEx API for shipping an order/creating a label.
    /// </summary>
    [Component]
    public class FedExShipRequest : IFedExShipRequest
    {
        readonly IFedExServiceGatewayFactory serviceGatewayFactory;
        readonly IFedExSettingsRepository settingsRepository;
        readonly IEnumerable<Func<IFedExSettingsRepository, IFedExShipRequestManipulator>> manipulatorFactory;
        readonly Func<ShipmentEntity, ProcessShipmentReply, IFedExShipResponse> createShipResponse;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShipRequest" /> class.
        /// </summary>
        public FedExShipRequest(
            IFedExServiceGatewayFactory serviceGatewayFactory,
            IFedExSettingsRepository settingsRepository,
            IEnumerable<Func<IFedExSettingsRepository, IFedExShipRequestManipulator>> manipulatorFactory,
            Func<ShipmentEntity, ProcessShipmentReply, IFedExShipResponse> createShipResponse)
        {
            this.createShipResponse = createShipResponse;
            this.manipulatorFactory = manipulatorFactory;
            this.settingsRepository = settingsRepository;
            this.serviceGatewayFactory = serviceGatewayFactory;
        }

        /// <summary>
        /// Submits the request to the FedEx API to ship an order/create a label.
        /// </summary>
        /// <returns>An ICarrierResponse containing the carrier-specific results of the request.</returns>
        public GenericResult<IFedExShipResponse> Submit(ShipmentEntity shipment, int sequenceNumber)
        {
            return manipulatorFactory
                .Select(x => x(settingsRepository))
                .Where(x => x.ShouldApply(shipment, sequenceNumber))
                .Aggregate(
                    new ProcessShipmentRequest(),
                    (req, manipulator) => manipulator.Manipulate(shipment, req, sequenceNumber))
                .Map(x => serviceGatewayFactory.Create(settingsRepository).Ship(x).Map(r => new { Reply = r, Request = x }))
                .Map(x => new { Response = createShipResponse(shipment, x.Reply), x.Request })
                .Map(x => x.Response.ApplyManipulators(x.Request));
        }
    }
}
