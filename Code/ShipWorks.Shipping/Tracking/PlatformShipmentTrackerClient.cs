using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Shipping.Tracking.DTO;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// Send and get tracking number information
    /// </summary>
    [Component]
    public class PlatformShipmentTrackerClient : IPlatformShipmentTrackerClient
    {
        private readonly IWarehouseRequestFactory warehouseRequestFactory;
        private readonly IWarehouseRequestClient warehouseRequestClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformShipmentTrackerClient(IWarehouseRequestFactory warehouseRequestFactory, IWarehouseRequestClient warehouseRequestClient)
        {
            this.warehouseRequestFactory = warehouseRequestFactory;
            this.warehouseRequestClient = warehouseRequestClient;
        }
        
        /// <summary>
        /// Send Shipment information to the hub
        /// </summary>
        public async Task SendShipment(string trackingNumber, string carrierCode, string warehouseID)
        {
            var trackingRequest = new TrackingRequest()
            {
                TrackingNumber = trackingNumber,
                CarrierCode = carrierCode,
                WarehouseId = warehouseID
            };
            var request = warehouseRequestFactory.Create(WarehouseEndpoints.Tracking, Method.POST, trackingRequest);
            var result = await warehouseRequestClient.MakeRequest(request, "Tracking").ConfigureAwait(false);
        }

        /// <summary>
        /// Get tracking information from the hub
        /// </summary>
        public async Task<IEnumerable<TrackingNotification>> GetTracking(string WarehouseID, DateTime lastUpdateDate)
        {
            var request = warehouseRequestFactory.Create(WarehouseEndpoints.GetTrackingUpdatesAfter(lastUpdateDate), Method.GET, null);
            return await warehouseRequestClient.MakeRequest<IEnumerable<TrackingNotification>>(request, "Tracking").ConfigureAwait(false);
            
        }
    }
}