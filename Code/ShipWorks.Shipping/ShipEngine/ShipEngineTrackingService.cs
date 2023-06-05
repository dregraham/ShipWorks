using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Shipping.Tracking.DTO;

namespace ShipWorks.Shipping.ShipEngine
{
    /// <summary>
    /// TrackingService for ShipEngine shipments
    /// </summary>
    [Component]
    public class ShipEngineTrackingService : IShipEngineTrackingService
    {
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineTrackingResultFactory trackingResultFactory;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipEngineTrackingService(
            ICarrierShipmentAdapterFactory shipmentAdapterFactory, 
            IShipEngineWebClient shipEngineWebClient, 
            IShipEngineTrackingResultFactory trackingResultFactory,
             Func<Type, ILog> createLog)
        {
            this.shipmentAdapterFactory = shipmentAdapterFactory;
            this.shipEngineWebClient = shipEngineWebClient;
            this.trackingResultFactory = trackingResultFactory;
            log = createLog(GetType());
        }

        public async Task<TrackingResult> TrackShipment(ShipmentEntity shipment, ApiLogSource apiLogSource, string engineCarrierCode, string trackingUrl)
        {
            var failedOrNoResultsSummary = $"<a href='{trackingUrl}' style='color:blue; background-color:white'>Click here to view tracking information online</a>";

            try { 
                var shipmentAdapter = shipmentAdapterFactory.Get(shipment);

                TrackingInformation trackingInfo;
                if(shipmentAdapter.ShipEngineLabelId.HasValue())
                {
                    trackingInfo = await shipEngineWebClient.Track(shipmentAdapter.ShipEngineLabelId, apiLogSource).ConfigureAwait(false);
                }
                else
                {
                    trackingInfo = await shipEngineWebClient.Track(engineCarrierCode, shipment.TrackingNumber, apiLogSource);
                }
                if (trackingInfo.StatusCode == "UN" || trackingInfo.Events.None())
                {
                    log.Info("No log events or current status unknown.");
                    return new TrackingResult { Summary = failedOrNoResultsSummary };
                }

                return trackingResultFactory.Create(trackingInfo);
            }
            catch (Exception ex)
            {
                log.Warn("Failed to track shipment.", ex);
                return new TrackingResult { Summary = failedOrNoResultsSummary };
            }
        }
    }
}
