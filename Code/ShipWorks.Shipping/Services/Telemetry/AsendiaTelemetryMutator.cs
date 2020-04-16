using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Services.Telemetry
{
    /// <summary>
    /// Sets Asendia specific telemetry
    /// </summary>
    [KeyedComponent(typeof(ICarrierTelemetryMutator), ShipmentTypeCode.Asendia)]
    public class AsendiaTelemetryMutator : ICarrierTelemetryMutator
    {
        /// <summary>
        /// Sets the carrier specific telemetry properties
        /// </summary>
        public void MutateTelemetry(ITrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            SetShipmentTelemetry(telemetryEvent, shipment);
        }

        /// <summary>
        /// Sets Asendia specific shipment telemetry
        /// </summary>
        private void SetShipmentTelemetry(ITrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            var asendiaShipment = shipment.Asendia;

            telemetryEvent.AddProperty("Label.Asendia.AsendiaAccountID", asendiaShipment.AsendiaAccountID.ToString());
            telemetryEvent.AddProperty("Label.Asendia.Contents", EnumHelper.GetDescription((ShipEngineContentsType) asendiaShipment.Contents));
            telemetryEvent.AddProperty("Label.Asendia.DimsAddWeight", asendiaShipment.DimsAddWeight.ToString());
            telemetryEvent.AddProperty("Label.Asendia.DimsHeight", asendiaShipment.DimsHeight.ToString());
            telemetryEvent.AddProperty("Label.Asendia.DimsLength", asendiaShipment.DimsLength.ToString());
            telemetryEvent.AddProperty("Label.Asendia.DimsWeight", asendiaShipment.DimsWeight.ToString());
            telemetryEvent.AddProperty("Label.Asendia.DimsWidth", asendiaShipment.DimsWidth.ToString());
            telemetryEvent.AddProperty("Label.Asendia.Insurance", asendiaShipment.Insurance.ToString());
            telemetryEvent.AddProperty("Label.Asendia.InsuranceValue", asendiaShipment.InsuranceValue.ToString());
            telemetryEvent.AddProperty("Label.Asendia.NonDelivery", EnumHelper.GetDescription((ShipEngineNonDeliveryType) asendiaShipment.NonDelivery));
            telemetryEvent.AddProperty("Label.Asendia.NonMachinable", asendiaShipment.NonMachinable.ToString());
            telemetryEvent.AddProperty("Label.Asendia.RequestedLabelFormat", EnumHelper.GetDescription((ThermalLanguage) asendiaShipment.RequestedLabelFormat));
            telemetryEvent.AddProperty("Label.Asendia.Service", EnumHelper.GetDescription(asendiaShipment.Service));
        }
    }
}