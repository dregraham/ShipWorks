using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SWA;

namespace ShipWorks.Shipping.Services.Telemetry
{
    public class AmazonSWATelemetryMutator : ICarrierTelemetryMutator
    {
        /// <summary>
        /// Sets the carrier specific telemetry properties for AmazonSWA
        /// </summary>
        public void MutateTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment)
        {
            SetShipmentTelemetry(telemetryEvent, shipment);
        }

        /// <summary>
        /// Sets the AmazonSWA specific shipment telemetry properties
        /// </summary>
        private void SetShipmentTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment)
        {
            var swaShipment = shipment.AmazonSWA;
            telemetryEvent.AddProperty("Label.AmazonSWA.Insurance", swaShipment.Insurance.ToString());
            telemetryEvent.AddProperty("Label.AmazonSWA.InsuranceValue", swaShipment.InsuranceValue.ToString());
            telemetryEvent.AddProperty("Label.AmazonSWA.Insurance", EnumHelper.GetDescription((ThermalLanguage)swaShipment.RequestedLabelFormat));
            telemetryEvent.AddProperty("Label.AmazonSWA.Insurance", EnumHelper.GetDescription((AmazonSWAServiceType)swaShipment.Service));
        }
    }
}
