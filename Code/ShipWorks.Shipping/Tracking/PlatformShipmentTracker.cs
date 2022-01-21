using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
        public void TrackShipments(CancellationToken cancellationToken)
        {
            // Doesn't matter if warehouseId is blank. This is used to filter out shipments. If not a warehouse customer,
            // we will get back all the shipments with a blank warehouse.
            var warehouseId = configurationData.FetchReadOnly().WarehouseID;
            var shipmentsToTrack = trackingRepository.FetchShipmentsToTrack().ToList();
            while (shipmentsToTrack.Any() && !cancellationToken.IsCancellationRequested)
            {
                foreach (var shipment in shipmentsToTrack)
                {
                    platformShipmentTrackerClient.SendShipment(shipment.TrackingNumber, GetCarrierName(shipment.ShipmentTypeCode), warehouseId);
                    trackingRepository.MarkAsSent(shipment);
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                }
            }
        }

        private string GetCarrierName(ShipmentTypeCode shipmentType)
        {
            if (shipmentTypeManager.IsPostal(shipmentType))
            {
                return "usps";
            }
            else if (shipmentTypeManager.IsUps(shipmentType))
            {
                return "ups";
            }
            else if (shipmentType == ShipmentTypeCode.Asendia)
            {
                return "asendia";
            }
            else if (shipmentType == ShipmentTypeCode.DhlExpress)
            {
                return "dhl_express";
            } 
            else if (shipmentType == ShipmentTypeCode.FedEx)
            {
                return "fedex";
            } 
            else if (shipmentType == ShipmentTypeCode.OnTrac)
            {
                return "ontrac";
            }
            else
            {
                // Should never happen.
                Debug.Fail("Unsupported tracking type. Shouldn't have gotten this shipment.");
                log.Warn("Unsupported shipment type found in PlatformShipmentTracker.");
                return EnumHelper.GetDescription(shipmentType).ToLower();
            }
        }

        /// <summary>
        /// Fetch tracking notifications from the hub and populate them
        /// </summary>
        public void PopulateLatestTracking(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}