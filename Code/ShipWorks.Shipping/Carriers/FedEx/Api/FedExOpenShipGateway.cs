﻿using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.OpenShip;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Derives from FedExServiceGateway overwriting the OpenShip methods.
    /// </summary>
    [Component(RegistrationType.SpecificService, Service = typeof(IFedExOpenShipServiceGateway))]
    public class FedExOpenShipGateway : FedExServiceGateway, IFedExOpenShipServiceGateway
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExOpenShipGateway(ICarrierSettingsRepository settingsRepository) : base(settingsRepository)
        { }

        /// <summary>
        /// Communicates with the FedEx API to process a shipment.
        /// </summary>
        public override TelemetricResult<GenericResult<ProcessShipmentReply>> Ship(ProcessShipmentRequest nativeShipmentRequest)
        {
            using (ShipService service = new FedExShipServiceWrapper(new ApiLogEntry(ApiLogSource.FedEx, "Process")))
            {
                return Ship(nativeShipmentRequest, service);
            }
        }

        /// <summary>
        /// Intended to interact with the FedEx API for performing a shipment void.
        /// </summary>
        public override ShipmentReply Void(DeleteShipmentRequest deleteShipmentRequest)
        {
            using (ShipService service = new FedExShipServiceWrapper(new ApiLogEntry(ApiLogSource.FedEx, "Void")))
            {
                return Void(deleteShipmentRequest, service);
            }
        }
    }
}
