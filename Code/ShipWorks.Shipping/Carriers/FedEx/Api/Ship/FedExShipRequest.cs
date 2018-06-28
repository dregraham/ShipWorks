using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public TelemetricResult<GenericResult<IFedExShipResponse>> Submit(ShipmentEntity shipment, int sequenceNumber)
        {
            IEnumerable<IFedExShipRequestManipulator> manipulators = manipulatorFactory
                .Select(x => x(settingsRepository)).Where(x => x.ShouldApply(shipment, sequenceNumber));

            GenericResult<ProcessShipmentRequest> request = manipulators.Aggregate(
                    new ProcessShipmentRequest(),
                    (req, manipulator) => manipulator.Manipulate(shipment, req, sequenceNumber));

            TelemetricResult<GenericResult<IFedExShipResponse>> telemetricResult = new TelemetricResult<GenericResult<IFedExShipResponse>>("");

            var processResult = request.Bind(
                x => 
                {
                    TelemetricResult<GenericResult<ProcessShipmentReply>> result = serviceGatewayFactory.Create(shipment, settingsRepository).Ship(x);
                    result.CopyTo(telemetricResult);
                    
                    return result.Value.Map(r => new { Reply = r, Request = x });
                }).Map(x => new { Response = createShipResponse(shipment, x.Reply), x.Request })
                .Bind(x => x.Response.ApplyManipulators(x.Request));
            
            telemetricResult.SetValue(processResult);
            return telemetricResult;
        }
    }
}
