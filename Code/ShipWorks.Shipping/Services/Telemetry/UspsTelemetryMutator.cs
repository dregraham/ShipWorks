using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityInterfaces;
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
        protected override void SetShipmentTelemetry(ITrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            base.SetShipmentTelemetry(telemetryEvent, shipment);
            var uspsShipment = shipment.Postal.Usps;

            telemetryEvent.AddProperty($"Label.Usps.RequestedLabelFormat", EnumHelper.GetDescription((ThermalLanguage) uspsShipment.RequestedLabelFormat));
            telemetryEvent.AddProperty($"Label.Usps.HidePostage", uspsShipment.HidePostage.ToString());
            telemetryEvent.AddProperty($"Label.Usps.RequireFullAddressValidation", uspsShipment.RequireFullAddressValidation.ToString());
        }
    }
}