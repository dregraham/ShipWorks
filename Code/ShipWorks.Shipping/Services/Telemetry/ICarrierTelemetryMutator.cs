using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.Telemetry
{
    public interface ICarrierTelemetryMutator
    {
        /// <summary>
        /// Sets the carrier specific shipment telemetry properties
        /// </summary>
        void SetShipmentTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment);

        /// <summary>
        /// Sets the carrier specific package telemetry properties
        /// </summary>
        void SetPackageTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment);
    }
}
