using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;

namespace ShipWorks.Shipping.Services.Telemetry
{
    /// <summary>
    /// Sets the OnTrac specific telemetry
    /// </summary>
    public class OnTracTelemetryMutator : ICarrierTelemetryMutator
    {
        /// <summary>
        /// Sets the carrier specific telemetry properties for OnTrac
        /// </summary>
        public void MutateTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment)
        {
            SetShipmentTelemetry(telemetryEvent, shipment);
        }

        /// <summary>
        /// Sets the OnTrac specific shipment telemetry properties
        /// </summary>
        private void SetShipmentTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment)
        {
            var onTracShipment = shipment.OnTrac;
            telemetryEvent.AddProperty("Label.OnTrac.CodAmount", onTracShipment.CodAmount.ToString());
            telemetryEvent.AddProperty("Label.OnTrac.CodType", EnumHelper.GetDescription((OnTracCodType)onTracShipment.CodType));
            telemetryEvent.AddProperty("Label.OnTrac.DeclaredValue", onTracShipment.DeclaredValue.ToString()); 
            telemetryEvent.AddProperty("Label.OnTrac.Instructions", onTracShipment.Instructions);
            telemetryEvent.AddProperty("Label.OnTrac.Insurance", onTracShipment.Insurance.ToString());
            telemetryEvent.AddProperty("Label.OnTrac.InsurancePennyOne", onTracShipment.InsurancePennyOne.ToString());
            telemetryEvent.AddProperty("Label.OnTrac.InusranceValue", onTracShipment.InsuranceValue.ToString());
            telemetryEvent.AddProperty("Label.OnTrac.IsCod", onTracShipment.IsCod.ToString());
            telemetryEvent.AddProperty("Label.OnTrac.PackagingType", EnumHelper.GetDescription((OnTracPackagingType)onTracShipment.PackagingType));
            telemetryEvent.AddProperty("Label.OnTrac.Reference1", onTracShipment.Reference1);
            telemetryEvent.AddProperty("Label.OnTrac.Reference2", onTracShipment.Reference2);
            telemetryEvent.AddProperty("Label.OnTrac.RequestedLabelFormat", EnumHelper.GetDescription((ThermalLanguage)onTracShipment.RequestedLabelFormat));
            telemetryEvent.AddProperty("Label.OnTrac.SaturdayDelivery", onTracShipment.SaturdayDelivery.ToString());
            telemetryEvent.AddProperty("Label.OnTrac.Service", EnumHelper.GetDescription((OnTracServiceType)onTracShipment.Service));
            telemetryEvent.AddProperty("Label.OnTrac.SignatureRequired", onTracShipment.SignatureRequired.ToString());
        }
    }
}
