using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Amazon.SWA;

namespace ShipWorks.Shipping.Services.Telemetry
{
    /// <summary>
    /// Sets the AmazonSWA specific telemetry
    /// </summary>
    [KeyedComponent(typeof(ICarrierTelemetryMutator), ShipmentTypeCode.AmazonSWA)]
    public class AmazonSWATelemetryMutator : ICarrierTelemetryMutator
    {
        /// <summary>
        /// Sets the carrier specific telemetry properties for AmazonSWA
        /// </summary>
        public void MutateTelemetry(ITrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            SetShipmentTelemetry(telemetryEvent, shipment);
        }

        /// <summary>
        /// Sets the AmazonSWA specific shipment telemetry properties
        /// </summary>
        private void SetShipmentTelemetry(ITrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            var amazonSwaShipment = shipment.AmazonSWA;

            telemetryEvent.AddProperty("Label.AmazonSWA.Insurance", amazonSwaShipment.Insurance.ToString());
            telemetryEvent.AddProperty("Label.AmazonSWA.InsuranceValue", amazonSwaShipment.InsuranceValue.ToString());
            telemetryEvent.AddProperty("Label.AmazonSWA.RequestedLabelFormat", EnumHelper.GetDescription((ThermalLanguage) amazonSwaShipment.RequestedLabelFormat));
            telemetryEvent.AddProperty("Label.AmazonSWA.Service", EnumHelper.GetDescription((AmazonSWAServiceType) amazonSwaShipment.Service));
        }
    }
}
