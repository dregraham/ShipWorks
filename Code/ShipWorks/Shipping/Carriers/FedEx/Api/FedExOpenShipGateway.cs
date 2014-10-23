using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Request.OpenShip;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    public class FedExOpenShipGateway : FedExServiceGateway
    {
        public FedExOpenShipGateway(ICarrierSettingsRepository settingsRepository) : base(settingsRepository)
        {}

        public FedExOpenShipGateway(ICarrierSettingsRepository settingsRepository, ILogEntryFactory logEntryFactory) : base(settingsRepository, logEntryFactory)
        {}

        public override IFedExNativeShipmentReply Ship(IFedExNativeShipmentRequest nativeShipmentRequest)
        {
            using (ShipService service = new FedExShipServiceWrapper(new ApiLogEntry(ApiLogSource.FedEx, "Process")))
            {
                return Ship(nativeShipmentRequest, service);
            }   
        }
    }
}
