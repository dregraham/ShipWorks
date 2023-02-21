using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Service for tracking FedEx shipments
    /// </summary>
    [Component]
    public class FedExTrackingService : IFedExTrackingService
    {
        private readonly IShipEngineTrackingService shipEngineTrackingService;
        private readonly IFedExUtility fedExUtility;
        private readonly IFimsShippingClerk fimsShippingClerk;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExTrackingService(IShipEngineTrackingService shipEngineTrackingService, IFedExUtility fedExUtility, IFimsShippingClerk fimsShippingClerk)
        {
            this.shipEngineTrackingService = shipEngineTrackingService;
            this.fedExUtility = fedExUtility;
            this.fimsShippingClerk = fimsShippingClerk;
        }

        /// <summary>
        /// Track a shipment
        /// </summary>
        public TrackingResult TrackShipment(ShipmentEntity shipment, string trackingUrl)
        {
            if(fedExUtility.IsFimsService((FedExServiceType) shipment.FedEx.Service))
            {
                return fimsShippingClerk.Track(shipment);
            }
            else
            {
                return TrackUsingShipEngine(shipment, trackingUrl);
            }
        }

        /// <summary>
        /// Track a shipment using ShipEngine
        /// </summary>
        private TrackingResult TrackUsingShipEngine(ShipmentEntity shipment, string trackingUrl)
        {
            // This is necessary because .Wait() causes the exception to be caught by the .NET runtime, then
            // rethrown. That's normally fine, but in ShipWorks any exceptions caught by the runtime causes
            // ShipWorks to close, so we have to work around that.
            Exception exception = null;

            TrackingResult trackingResult = null;
            Task.Run(async () =>
            {
                try
                {
                    trackingResult = await shipEngineTrackingService.TrackShipment(shipment, ApiLogSource.FedEx, "fedex", trackingUrl);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }).Wait();

            if (exception != null)
            {
                throw exception;
            }

            return trackingResult;
        }
    }
}
