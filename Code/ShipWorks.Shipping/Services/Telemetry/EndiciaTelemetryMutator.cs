using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Filters.Content.Conditions.Shipments;

namespace ShipWorks.Shipping.Services.Telemetry
{
    [KeyedComponent(typeof(ICarrierTelemetryMutator), ShipmentTypeCode.Endicia)]
    public class EndiciaTelemetryMutator : PostalTelemetryMutator
    {
        /// <summary>
        /// Sets the Endicia specific shipment telemetry properties
        /// </summary>
        public override void SetShipmentTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment)
        {
            base.SetShipmentTelemetry(telemetryEvent, shipment);
            var endiciaShipment = shipment.Postal.Endicia;
            telemetryEvent.AddProperty($"Label.Endicia.ReferenceID", endiciaShipment.ReferenceID);
            telemetryEvent.AddProperty($"Label.Endicia.ReferenceID2", endiciaShipment.ReferenceID2);
            telemetryEvent.AddProperty($"Label.Endicia.ReferenceID3", endiciaShipment.ReferenceID3);
            telemetryEvent.AddProperty($"Label.Endicia.ReferenceID4", endiciaShipment.ReferenceID4);
            telemetryEvent.AddProperty($"Label.Endicia.RequestedLabelFormat", EnumHelper.GetDescription((LabelFormatType) endiciaShipment.RequestedLabelFormat));
            telemetryEvent.AddProperty($"Label.Endicia.ScanBasedReturn", endiciaShipment.ScanBasedReturn.ToString());
            telemetryEvent.AddProperty($"Label.Endicia.StealthPostage", endiciaShipment.StealthPostage.ToString());
        }
    }
}
