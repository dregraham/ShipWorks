using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.OpenShip;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// Derives from FedExServiceGateway overwriting the OpenShip methods.
    /// </summary>
    public class FedExOpenShipGateway : FedExServiceGateway
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FedExOpenShipGateway(ICarrierSettingsRepository settingsRepository) : base(settingsRepository)
        {}

        /// <summary>
        /// Communicates with the FedEx API to process a shipment.
        /// </summary>
        public override IFedExNativeShipmentReply Ship(IFedExNativeShipmentRequest nativeShipmentRequest)
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
