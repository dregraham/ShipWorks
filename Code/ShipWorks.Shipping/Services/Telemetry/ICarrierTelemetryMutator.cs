using Interapptive.Shared.Metrics;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Services.Telemetry
{
    /// <summary>
    /// Interface for the carrier specific telemetry mutators
    /// </summary>
    public interface ICarrierTelemetryMutator
    {
        /// <summary>
        /// Sets the carrier specific telemetry properties
        /// </summary>
        void MutateTelemetry(ITrackedDurationEvent telemetryEvent, IShipmentEntity shipment);
    }
}
