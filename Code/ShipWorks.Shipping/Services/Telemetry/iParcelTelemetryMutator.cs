using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel.Enums;

namespace ShipWorks.Shipping.Services.Telemetry
{
    /// <summary>
    /// Set the iParcel specific telemetry
    /// </summary>
    [KeyedComponent(typeof(ICarrierTelemetryMutator), ShipmentTypeCode.iParcel)]
    public class iParcelTelemetryMutator : ICarrierTelemetryMutator
    {
        /// <summary>
        /// Sets the carrier specific telemetry properties
        /// </summary>
        public void MutateTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment)
        {
            SetShipmentTelemetry(telemetryEvent, shipment);
            SetPackageTelemetry(telemetryEvent, shipment);
        }

        /// <summary>
        /// Sets the carrier specific shipment telemetry properties
        /// </summary>
        private void SetShipmentTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment)
        {
            var iParcelShipment = shipment.IParcel;

            telemetryEvent.AddProperty("Label.iParcel.Reference", iParcelShipment.Reference);
            telemetryEvent.AddProperty("Label.iParcel.IsDeliveryDutyPaid", iParcelShipment.IsDeliveryDutyPaid.ToString());
            telemetryEvent.AddProperty("Label.iParcel.Service", EnumHelper.GetDescription((iParcelServiceType) iParcelShipment.Service));
            telemetryEvent.AddProperty("Label.iParcel.RequestedLabelFormat", EnumHelper.GetDescription((ThermalLanguage) iParcelShipment.RequestedLabelFormat));
            telemetryEvent.AddProperty("Label.iParcel.TrackByEmail", iParcelShipment.TrackByEmail.ToString());
            telemetryEvent.AddProperty("Label.iParcel.TrackBySMS", iParcelShipment.TrackBySMS.ToString());
        }

        /// <summary>
        /// Sets the carrier specific package telemetry properties
        /// </summary>
        private void SetPackageTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment)
        {
            var packages = shipment.IParcel.Packages;

            for (int i = 0; i < packages.Count; i++)
            {
                var package = packages[i];

                telemetryEvent.AddProperty($"Label.Package.{i}.DeclaredValue", package.DeclaredValue.ToString());
                telemetryEvent.AddProperty($"Label.Package.{i}.DimsAddWeight", package.DimsAddWeight.ToString());
                telemetryEvent.AddProperty($"Label.Package.{i}.DimsHeight", package.DimsHeight.ToString());
                telemetryEvent.AddProperty($"Label.Package.{i}.DimsLength", package.DimsLength.ToString());
                telemetryEvent.AddProperty($"Label.Package.{i}.DimsProfileID", package.DimsProfileID.ToString());
                telemetryEvent.AddProperty($"Label.Package.{i}.DimsWeight", package.DimsWeight.ToString());
                telemetryEvent.AddProperty($"Label.Package.{i}.DimsWidth", package.DimsWidth.ToString());
                telemetryEvent.AddProperty($"Label.Package.{i}.Insurance", package.Insurance.ToString());
                telemetryEvent.AddProperty($"Label.Package.{i}.InsuranceValue", package.InsuranceValue.ToString());
                telemetryEvent.AddProperty($"Label.Package.{i}.InsurancePennyOne", package.InsurancePennyOne.ToString());
                telemetryEvent.AddProperty($"Label.Package.{i}.ParcelNumber", package.ParcelNumber);
                telemetryEvent.AddProperty($"Label.Package.{i}.SkuAndQuantities", package.SkuAndQuantities);
                telemetryEvent.AddProperty($"Label.Package.{i}.TrackingNumber", package.TrackingNumber);
                telemetryEvent.AddProperty($"Label.Package.{i}.Weight", package.Weight.ToString());
            }
        }
    }
}