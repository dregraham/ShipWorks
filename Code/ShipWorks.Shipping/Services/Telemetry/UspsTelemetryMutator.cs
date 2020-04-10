using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Conditions.Shipments;

namespace ShipWorks.Shipping.Services.Telemetry
{
    /// <summary>
    /// Sets the USPS specific telemetry
    /// </summary>
    [KeyedComponent(typeof(ICarrierTelemetryMutator), ShipmentTypeCode.Usps)]
    public class UspsTelemetryMutator : PostalTelemetryMutator
    {
        /// <summary>
        /// Sets the USPS specific shipment telemetry properties
        /// </summary>
        public override void SetShipmentTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment)
        {
            base.SetShipmentTelemetry(telemetryEvent, shipment);
            var uspsShipment = shipment.Postal.Usps;

            telemetryEvent.AddProperty($"Label.Usps.RequestedLabelFormat", EnumHelper.GetDescription((LabelFormatType)uspsShipment.RequestedLabelFormat));
            telemetryEvent.AddProperty($"Label.Usps.HidePostage", uspsShipment.HidePostage.ToString());
            telemetryEvent.AddProperty($"Label.Usps.RequireFullAddressValidation", uspsShipment.RequireFullAddressValidation.ToString());
        }
    }
}
