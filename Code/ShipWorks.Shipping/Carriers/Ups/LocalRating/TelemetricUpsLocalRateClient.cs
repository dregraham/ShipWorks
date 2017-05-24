using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating
{
    /// <summary>
    /// Client for local rating with telemetry
    /// </summary>
    [KeyedComponent(typeof(IUpsRateClient), UpsRatingMethod.LocalOnly)]
    public class TelemetricUpsLocalRateClient : IUpsRateClient
    {
        private readonly IUpsRateClient localRateClient;
        private readonly Func<string, ITrackedDurationEvent> telemetryFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TelemetricUpsLocalRateClient"/> class.
        /// </summary>
        public TelemetricUpsLocalRateClient(UpsLocalRateClient localRateClient, Func<string, ITrackedDurationEvent> telemetryFactory)
        {
            this.localRateClient = localRateClient;
            this.telemetryFactory = telemetryFactory;
        }

        /// <summary>
        /// Get rates from the local rate client and collect telemetry
        /// </summary>
        public GenericResult<List<UpsServiceRate>> GetRates(ShipmentEntity shipment)
        {
            using (ITrackedDurationEvent telemetryEvent = telemetryFactory("Shipping.Rating.Ups.LocalRating"))
            {
                GenericResult<List<UpsServiceRate>> serviceRates = localRateClient.GetRates(shipment);
                
                string serviceNames;
                string quantity;
                if (serviceRates.Value == null)
                {
                    serviceNames = "None";
                    quantity = "0";
                }
                else
                {
                    serviceNames = string.Join(",",
                        serviceRates.Value.Select(r => EnumHelper.GetDescription(r.Service)).ToList());
                    quantity = serviceRates.Value.Count.ToString();
                }

                telemetryEvent.AddProperty("Results.Quantity", quantity);
                telemetryEvent.AddProperty("Results.AvailableServices", serviceNames);
                telemetryEvent.AddProperty("Shipment.Origin.StateProvince", shipment.OriginStateProvCode);
                telemetryEvent.AddProperty("Shipment.Origin.PostalCode", shipment.OriginPostalCode);
                telemetryEvent.AddProperty("Shipment.Destination.StateProvince", shipment.ShipStateProvCode);
                telemetryEvent.AddProperty("Shipment.Destination.PostalCode", shipment.ShipPostalCode);

                for (int i = 0; i < shipment.Ups.Packages.Count; i++)
                {
                    telemetryEvent.AddProperty($"Shipment.RatedWeight.{i}", shipment.Ups.Packages[i].BillableWeight.ToString());
                    telemetryEvent.AddProperty($"Shipment.Dimensions.Length.{i}", shipment.Ups.Packages[i].DimsLength.ToString(CultureInfo.InvariantCulture));
                    telemetryEvent.AddProperty($"Shipment.Dimensions.Width.{i}", shipment.Ups.Packages[i].DimsWidth.ToString(CultureInfo.InvariantCulture));
                    telemetryEvent.AddProperty($"Shipment.Dimensions.Height.{i}", shipment.Ups.Packages[i].DimsHeight.ToString(CultureInfo.InvariantCulture));
                }

                return serviceRates;
            }
        }
    }
}
