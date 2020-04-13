using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
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
        public void MutateTelemetry(ITrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            SetShipmentTelemetry(telemetryEvent, shipment);
            SetPackageTelemetry(telemetryEvent, shipment);
        }

        /// <summary>
        /// Sets the carrier specific shipment telemetry properties
        /// </summary>
        private void SetShipmentTelemetry(ITrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
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
        private void SetPackageTelemetry(ITrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            int packageIndex = 0;

            foreach (var iParcelPackageEntity in shipment.IParcel.Packages)
            {
                var package = (IParcelPackageEntity) iParcelPackageEntity;
                packageIndex++;

                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.DeclaredValue", package.DeclaredValue.ToString());
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.DimsAddWeight", package.DimsAddWeight.ToString());
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.DimsHeight", package.DimsHeight.ToString());
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.DimsLength", package.DimsLength.ToString());
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.DimsProfileID", package.DimsProfileID.ToString());
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.DimsWeight", package.DimsWeight.ToString());
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.DimsWidth", package.DimsWidth.ToString());
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.Insurance", package.Insurance.ToString());
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.InsuranceValue", package.InsuranceValue.ToString());
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.InsurancePennyOne", package.InsurancePennyOne.ToString());
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.ParcelNumber", package.ParcelNumber);
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.SkuAndQuantities", package.SkuAndQuantities);
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.TrackingNumber", package.TrackingNumber);
                telemetryEvent.AddProperty($"Label.iParcel.Package.{packageIndex}.Weight", package.Weight.ToString());
            }
        }
    }
}