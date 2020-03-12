using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.Tracking;
using ShipWorks.ApplicationCore.Logging;
using System.Threading.Tasks;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Track a Ups Shipment
    /// </summary>
    [Component]
    public class UpsTrackClient : IUpsTrackClient
    {
        private readonly IShipEngineWebClient shipEngineWebClient;
        private readonly IShipEngineTrackingResultFactory trackingResultFactory;

        public UpsTrackClient(IShipEngineWebClient shipEngineWebClient, IShipEngineTrackingResultFactory trackingResultFactory)
        {
            this.shipEngineWebClient = shipEngineWebClient;
            this.trackingResultFactory = trackingResultFactory;
        }

        /// <summary>
        /// Track the shipment
        /// </summary>
        public async Task<TrackingResult> TrackShipment(ShipmentEntity shipment)
        {
            TrackingResult trackingResult;
            if (string.IsNullOrEmpty(shipment.Ups.ShipEngineLabelID))
            {
               trackingResult = UpsApiTrackClient.TrackShipment(shipment);
            }
            else
            {
                try
                {
                    var shipEngineTrackingInformation = await shipEngineWebClient.Track(shipment.Ups.ShipEngineLabelID, ApiLogSource.UPS).ConfigureAwait(false);
                    trackingResult = trackingResultFactory.Create(shipEngineTrackingInformation);
                }
                catch (ShipEngineException ex)
                {
                    throw new UpsException(ex.Message, ex);
                }
            }

            return trackingResult;
        }
    }
}
