using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request
{
    /// <summary>
    /// An implementation of the CarrierRequest interface that sends a request to the FedEx API for shipping an order/creating a label.
    /// </summary>
    public class FedExShipRequest : CarrierRequest, IFedExShipRequest
    {
        readonly IFedExServiceGatewayFactory serviceGatewayFactory;
        readonly IFedExSettingsRepository settingsRepository;
        readonly IEnumerable<Func<IFedExSettingsRepository, IFedExShipRequestManipulator>> manipulatorFactory;
        readonly Func<ShipmentEntity, ProcessShipmentReply, IFedExShipResponse> createShipResponse;
        private object p1;
        private ShipmentEntity shipmentEntity;
        private object p2;
        private object p3;
        private ICarrierSettingsRepository repo;
        private ProcessShipmentRequest processShipmentRequest;

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

        //TODO: Remove this constructor when manipulators have been converted
        public FedExShipRequest(object p1,
            object shipmentEntity,
            object p2,
            object p3,
            object repo,
            object processShipmentRequest)
        {

        }

        /// <summary>
        /// Submits the request to the FedEx API to ship an order/create a label.
        /// </summary>
        /// <returns>An ICarrierResponse containing the carrier-specific results of the request.</returns>
        public GenericResult<IFedExShipResponse> Submit(ShipmentEntity shipment, int sequenceNumber)
        {
            return manipulatorFactory
                .Select(x => x(settingsRepository))
                .Where(x => x.ShouldApply(shipment))
                .Aggregate(
                    new ProcessShipmentRequest(),
                    (req, manipulator) => manipulator.Manipulate(shipment, req, sequenceNumber))
                .Map(x => serviceGatewayFactory.Create(settingsRepository).Ship(x))
                .Map(x => createShipResponse(shipment, x))
                .Map(x => x.ApplyManipulators());
        }

        //TODO: Remove these lines when manipulators are converted
        public override IEntity2 CarrierAccountEntity
        {
            get { throw new NotImplementedException("REMOVE THIS WHEN MANIPULATORS ARE CONVERTED"); }
        }

        public override ICarrierResponse Submit()
        {
            throw new NotImplementedException("REMOVE THIS WHEN MANIPULATORS ARE CONVERTED");
        }
    }
}
