using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data;

namespace ShipWorks.Shipping.Tracking
{
    /// <summary>
    /// Class to send tracking information and receive 
    /// </summary>
    [Component]
    public class PlatformShipmentTracker : IPlatformShipmentTracker
    {
        private readonly ITrackingRepository trackingRepository;
        private readonly IPlatformShipmentTrackerClient platformShipmentTrackerClient;
        private readonly IConfigurationData configurationData;
        private readonly IShipmentTypeManager shipmentTypeManager;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public PlatformShipmentTracker(ITrackingRepository trackingRepository, 
            IPlatformShipmentTrackerClient platformShipmentTrackerClient, IConfigurationData configurationData,
            IShipmentTypeManager shipmentTypeManager, Func<Type, ILog> logFactory)
        {
            this.trackingRepository = trackingRepository;
            this.platformShipmentTrackerClient = platformShipmentTrackerClient;
            this.configurationData = configurationData;
            this.shipmentTypeManager = shipmentTypeManager;
            log = logFactory(GetType());
        }
        /// <summary>
        /// Get all the shipments and send them up to hub for tracking
        /// </summary>
        public async Task TrackShipments(CancellationToken cancellationToken)
        {
            // Doesn't matter if warehouseId is blank. This is used to filter out shipments. If not a warehouse customer,
            // we will get back all the shipments with a blank warehouse.
            var warehouseId = configurationData.FetchReadOnly().WarehouseID;
            var shipmentsToTrack = (await trackingRepository.FetchShipmentsToTrack().ConfigureAwait(false))
                .ToList();
            
            while (shipmentsToTrack.Any() && !cancellationToken.IsCancellationRequested)
            {
                bool oneShipmentWorked = false;
                foreach (var shipment in shipmentsToTrack)
                {
                    var clientResult = await platformShipmentTrackerClient.SendShipment(shipment.TrackingNumber, GetCarrierName(shipment.ShipmentTypeCode), warehouseId).ConfigureAwait(false);

                    if (clientResult.Success)
                    {
                        await trackingRepository.MarkAsSent(shipment).ConfigureAwait(false);
                        oneShipmentWorked = true;
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                }

                if (!oneShipmentWorked)
                {
                    return;
                }

                shipmentsToTrack = (await trackingRepository.FetchShipmentsToTrack().ConfigureAwait(false))
                    .ToList();
            }
        }

        /// <summary>
        /// Return the platform friendly carrier name
        /// </summary>
        private string GetCarrierName(ShipmentTypeCode shipmentType)
        {
            if (shipmentTypeManager.IsPostal(shipmentType))
            {
                return "usps";
            }

            if (shipmentTypeManager.IsUps(shipmentType))
            {
                return "ups";
            }

            if (shipmentType == ShipmentTypeCode.FedEx)
            {
                return "fedex";
            }

            if (shipmentType == ShipmentTypeCode.DhlEcommerce)
            {
                return "dhl_global_mail";
            }

            // Should never happen.
            Debug.Fail($"Unsupported shipment type {shipmentType}. Shouldn't have gotten this shipment.");
            log.Warn($"Unsupported shipment type {shipmentType} found in PlatformShipmentTracker.");
            return EnumHelper.GetDescription(shipmentType).ToLower();
        }

        /// <summary>
        /// Fetch tracking notifications from the hub and populate them
        /// </summary>
        public async Task PopulateLatestTracking(CancellationToken cancellationToken)
        {
            var warehouseId = configurationData.FetchReadOnly().WarehouseID;
            
            var latestNotificationDate = await trackingRepository.GetLatestNotificationDate().ConfigureAwait(false);
            while (!cancellationToken.IsCancellationRequested)
            {
                var notifications = await platformShipmentTrackerClient
                    .GetTracking(warehouseId, latestNotificationDate).ConfigureAwait(false);
                notifications = notifications.ToList();
                
                if (!notifications.Any())
                {
                    break;
                }

                foreach (var notification in notifications)
                {
                    latestNotificationDate = notification.HubTimestamp;
                    await trackingRepository.SaveNotification(notification).ConfigureAwait(false);
                }
            }
        }
    }
}