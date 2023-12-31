﻿using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Enums;

namespace ShipWorks.Shipping.Services.Telemetry
{
    /// <summary>
    /// Sets the AmazonSFP specific telemetry
    /// </summary>
    [KeyedComponent(typeof(ICarrierTelemetryMutator), ShipmentTypeCode.AmazonSFP)]
    public class AmazonSFPTelemetryMutator : ICarrierTelemetryMutator
    {
        /// <summary>
        /// Sets the carrier specific telemetry properties
        /// </summary>
        public void MutateTelemetry(ITrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            SetShipmentTelemetry(telemetryEvent, shipment);
        }

        /// <summary>
        /// Sets the carrier specific shipment telemetry properties
        /// </summary>
        private void SetShipmentTelemetry(ITrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            var amazonSfpShipment = shipment.AmazonSFP;

            telemetryEvent.AddProperty("Label.AmazonSFP.AmazonUniqueShipmentID", amazonSfpShipment.AmazonUniqueShipmentID);
            telemetryEvent.AddProperty("Label.AmazonSFP.CarrierName", amazonSfpShipment.CarrierName);
            telemetryEvent.AddProperty("Label.AmazonSFP.DeclaredValue", amazonSfpShipment.DeclaredValue.ToString());
            telemetryEvent.AddProperty("Label.AmazonSFP.DeliveryExperience", EnumHelper.GetDescription((AmazonSFPDeliveryExperienceType) amazonSfpShipment.DeliveryExperience));
            telemetryEvent.AddProperty("Label.AmazonSFP.DimsAddWeight", amazonSfpShipment.DimsAddWeight.ToString());
            telemetryEvent.AddProperty("Label.AmazonSFP.DimsHeight", amazonSfpShipment.DimsHeight.ToString());
            telemetryEvent.AddProperty("Label.AmazonSFP.DimsLength", amazonSfpShipment.DimsLength.ToString());
            telemetryEvent.AddProperty("Label.AmazonSFP.DimsWidth", amazonSfpShipment.DimsWidth.ToString());
            telemetryEvent.AddProperty("Label.AmazonSFP.DimsProfileID", amazonSfpShipment.DimsProfileID.ToString());
            telemetryEvent.AddProperty("Label.AmazonSFP.DimsWeight", amazonSfpShipment.DimsWeight.ToString());
            telemetryEvent.AddProperty("Label.AmazonSFP.Insurance", amazonSfpShipment.Insurance.ToString());
            telemetryEvent.AddProperty("Label.AmazonSFP.InsuranceValue", amazonSfpShipment.InsuranceValue.ToString());
            telemetryEvent.AddProperty("Label.AmazonSFP.Reference", amazonSfpShipment.Reference1);
            telemetryEvent.AddProperty("Label.AmazonSFP.RequestedLabelFormat", EnumHelper.GetDescription((ThermalLanguage) amazonSfpShipment.RequestedLabelFormat));
            telemetryEvent.AddProperty("Label.AmazonSFP.ShippingServiceID", amazonSfpShipment.ShippingServiceID);
            telemetryEvent.AddProperty("Label.AmazonSFP.ShippingServiceName", amazonSfpShipment.ShippingServiceName);
        }
    }
}