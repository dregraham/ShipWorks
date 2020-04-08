using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.Telemetry
{
    public interface ICarrierTelemetryMutator
    {
        /// <summary>
        /// Sets the carrier specific shipment telemetry properties
        /// </summary>
        TrackedDurationEvent SetShipmentTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment);

        /// <summary>
        /// Sets the carrier specific package telemetry properties
        /// </summary>
        TrackedDurationEvent SetPackageTelemetry(TrackedDurationEvent telemetryEvent, IPackageAdapter[] packages);
    }
}
