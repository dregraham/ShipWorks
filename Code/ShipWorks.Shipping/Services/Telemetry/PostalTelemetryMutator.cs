using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Shipping.Services.Telemetry
{
    [KeyedComponent(typeof(ICarrierTelemetryMutator), ShipmentTypeCode.PostalWebTools)]
    [KeyedComponent(typeof(ICarrierTelemetryMutator), ShipmentTypeCode.Express1Endicia)]
    [KeyedComponent(typeof(ICarrierTelemetryMutator), ShipmentTypeCode.Express1Usps)]

    public class PostalTelemetryMutator : ICarrierTelemetryMutator
    {
        public void MutateShipmentTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment)
        {
            SetShipmentTelemetry(telemetryEvent, shipment);
            SetPackageTelemetry(telemetryEvent, shipment);
        }

        /// <summary>
        /// Sets the Postal specific shipment telemetry properties
        /// </summary>
        public virtual void SetShipmentTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment)
        {
            var postalShipment = shipment.Postal;
            var shipmentTypeCode = shipment.ShipmentTypeCode.ToString();
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.Confirmation", EnumHelper.GetDescription((PostalConfirmationType)postalShipment.Confirmation));
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.CustomsContentDescription", postalShipment.CustomsContentDescription);
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.PostalCustomsContentType", EnumHelper.GetDescription((PostalCustomsContentType)postalShipment.CustomsContentType));
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.Insurance", postalShipment.Insurance.ToString());
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.InsuranceValue", postalShipment.InsuranceValue.ToString());
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.Memo1", postalShipment.Memo1);
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.Memo2", postalShipment.Memo2);
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.Memo3", postalShipment.Memo3);
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.NonMachinable", postalShipment.NonMachinable.ToString());
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.NonRectangular", postalShipment.NonRectangular.ToString());
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.NoPostage", postalShipment.NoPostage.ToString());
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.PackagingType", EnumHelper.GetDescription((PostalPackagingType)postalShipment.PackagingType));
            telemetryEvent.AddProperty($"Label.{shipmentTypeCode}.Memo1", EnumHelper.GetDescription((PostalServiceType)postalShipment.Service));
        }

        /// <summary>
        /// Sets the carrier specific package telemetry properties
        /// </summary>
        public void SetPackageTelemetry(TrackedDurationEvent telemetryEvent, ShipmentEntity shipment)
        {
           //There are no postal specific fields
        }
    }
}
