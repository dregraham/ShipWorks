using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.Telemetry
{
    public interface ICarrierTelemetryMutator
    {
        /// <summary>
        /// Sets the carrier specific telemetry properties
        /// </summary>
        void MutateShipmentTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment);
    }
}
