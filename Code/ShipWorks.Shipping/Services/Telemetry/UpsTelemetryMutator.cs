using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

namespace ShipWorks.Shipping.Services.Telemetry
{
    /// <summary>
    /// Sets the UPS specific telemetry
    /// </summary>
    [KeyedComponent(typeof(ICarrierTelemetryMutator), ShipmentTypeCode.UpsOnLineTools)]
    [KeyedComponent(typeof(ICarrierTelemetryMutator), ShipmentTypeCode.UpsWorldShip)]
    public class UpsTelemetryMutator : ICarrierTelemetryMutator
    {
        /// <summary>
        /// Sets the carrier specific telemetry properties
        /// </summary>
        public void MutateTelemetry(TrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            SetShipmentTelemetry(telemetryEvent, shipment);
            SetPackageTelemetry(telemetryEvent, shipment);
        }

        /// <summary>
        /// Sets the Ups specific shipment telemetry properties
        /// </summary>
        private void SetShipmentTelemetry(TrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            var upsShipment = shipment.Ups;

            telemetryEvent.AddProperty("Label.Ups.CarbonNeutral", upsShipment.CarbonNeutral.ToString());
            telemetryEvent.AddProperty("Label.Ups.Cn22Number", upsShipment.Cn22Number);
            telemetryEvent.AddProperty("Label.Ups.CodAmount", upsShipment.CodAmount.ToString());
            telemetryEvent.AddProperty("Label.Ups.CodEnabled", upsShipment.CodEnabled.ToString());
            telemetryEvent.AddProperty("Label.Ups.CodPaymentType", EnumHelper.GetDescription((UpsCodPaymentType) upsShipment.CodPaymentType));
            telemetryEvent.AddProperty("Label.Ups.CommercialInvoiceComments", upsShipment.CommercialInvoiceComments);
            telemetryEvent.AddProperty("Label.Ups.CommercialInvoiceFreight", upsShipment.CommercialInvoiceFreight.ToString());
            telemetryEvent.AddProperty("Label.Ups.CommercialInvoiceInsurance", upsShipment.CommercialInvoiceInsurance.ToString());
            telemetryEvent.AddProperty("Label.Ups.CommercialInvoiceOther", upsShipment.CommercialInvoiceOther.ToString());
            telemetryEvent.AddProperty("Label.Ups.CommercialInvoicePurpose", EnumHelper.GetDescription((UpsExportReason) upsShipment.CommercialInvoicePurpose));
            telemetryEvent.AddProperty("Label.Ups.CommercialInvoiceTermsOfSale", EnumHelper.GetDescription((UpsTermsOfSale) upsShipment.CommercialInvoiceTermsOfSale));
            telemetryEvent.AddProperty("Label.Ups.CommercialPaperlessInvoice", upsShipment.CommercialPaperlessInvoice.ToString());
            telemetryEvent.AddProperty("Label.Ups.CostCenter", upsShipment.CostCenter);
            telemetryEvent.AddProperty("Label.Ups.CustomsDescriptions", upsShipment.CustomsDescription);
            telemetryEvent.AddProperty("Label.Ups.CustomsDocumentsOnly", upsShipment.CustomsDocumentsOnly.ToString());
            telemetryEvent.AddProperty("Label.Ups.DeliveryConfirmation", EnumHelper.GetDescription((UpsDeliveryConfirmationType) upsShipment.DeliveryConfirmation));
            telemetryEvent.AddProperty("Label.Ups.EmailNotifyFrom", upsShipment.EmailNotifyFrom);
            telemetryEvent.AddProperty("Label.Ups.EmailNotifyMessage", upsShipment.EmailNotifyMessage);
            telemetryEvent.AddProperty("Label.Ups.EmailNotifyOther", (upsShipment.EmailNotifyOther != 0).ToString());
            telemetryEvent.AddProperty("Label.Ups.EmailNotifyOtherAddress", upsShipment.EmailNotifyOtherAddress);
            telemetryEvent.AddProperty("Label.Ups.EmailNotifyRecipient", (upsShipment.EmailNotifyRecipient != 0).ToString());
            telemetryEvent.AddProperty("Label.Ups.EmailNotifySender", (upsShipment.EmailNotifySender != 0).ToString());
            telemetryEvent.AddProperty("Label.Ups.EmailNotificationSubject", EnumHelper.GetDescription((UpsEmailNotificationSubject) upsShipment.EmailNotifySubject));
            telemetryEvent.AddProperty("Label.Ups.Endorsement", EnumHelper.GetDescription((UspsEndorsementType) upsShipment.Endorsement));
            telemetryEvent.AddProperty("Label.Ups.IrregularIndicator", (upsShipment.IrregularIndicator != 0).ToString());
            telemetryEvent.AddProperty("Label.Ups.NegotiatedRate", upsShipment.NegotiatedRate.ToString());
            telemetryEvent.AddProperty("Label.Ups.PaperlessAdditionalDocumentation", upsShipment.PaperlessAdditionalDocumentation.ToString());
            telemetryEvent.AddProperty("Label.Ups.PayorAccount", upsShipment.PayorAccount);
            telemetryEvent.AddProperty("Label.Ups.PayorCountryCode", upsShipment.PayorCountryCode);
            telemetryEvent.AddProperty("Label.Ups.PayorPostalCode", upsShipment.PayorPostalCode);
            telemetryEvent.AddProperty("Label.Ups.PayorType", EnumHelper.GetDescription((UpsPayorType) upsShipment.PayorType));
            telemetryEvent.AddProperty("Label.Ups.PublishedCharges", upsShipment.PublishedCharges.ToString());
            telemetryEvent.AddProperty("Label.Ups.ReferenceNumber", upsShipment.ReferenceNumber);
            telemetryEvent.AddProperty("Label.Ups.ReferenceNumber2", upsShipment.ReferenceNumber2);
            telemetryEvent.AddProperty("Label.Ups.RequestedLabelFormat", EnumHelper.GetDescription((ThermalLanguage) upsShipment.RequestedLabelFormat));
            telemetryEvent.AddProperty("Label.Ups.ReturnContents", upsShipment.ReturnContents);
            telemetryEvent.AddProperty("Label.Ups.PayorPostalCode", EnumHelper.GetDescription((UpsReturnServiceType) upsShipment.ReturnService));
            telemetryEvent.AddProperty("Label.Ups.ReturnUndeliverableEmail", upsShipment.ReturnUndeliverableEmail);
            telemetryEvent.AddProperty("Label.Ups.SaturdayDelivery", upsShipment.SaturdayDelivery.ToString());
            telemetryEvent.AddProperty("Label.Ups.Service", EnumHelper.GetDescription((UpsServiceType) upsShipment.Service));
            telemetryEvent.AddProperty("Label.Ups.ShipmentChargeAccount", upsShipment.ShipmentChargeAccount);
            telemetryEvent.AddProperty("Label.Ups.ShipmentChargeCountryCode", upsShipment.ShipmentChargeCountryCode);
            telemetryEvent.AddProperty("Label.Ups.ShipmentChargePostalCode", upsShipment.ShipmentChargePostalCode);
            telemetryEvent.AddProperty("Label.Ups.ShipmentChargeType", EnumHelper.GetDescription((UpsShipmentChargeType) upsShipment.ShipmentChargeType));
            telemetryEvent.AddProperty("Label.Ups.ShipperRelease", upsShipment.ShipperRelease.ToString());
            telemetryEvent.AddProperty("Label.Ups.PayorPostalCode", EnumHelper.GetDescription((UpsPostalSubclassificationType) upsShipment.Subclassification));
            telemetryEvent.AddProperty("Label.Ups.WorldShipStatus", EnumHelper.GetDescription((WorldShipStatusType) upsShipment.WorldShipStatus));
        }

        /// <summary>
        /// Sets the Ups specific package telemetry properties
        /// </summary>
        private void SetPackageTelemetry(TrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            int packageIndex = 0;

            foreach(UpsPackageEntity package in shipment.Ups.Packages)
            {
                packageIndex++;

                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.BillableWeight", package.BillableWeight.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.DeclaredValue", package.DeclaredValue.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.DryIceEnabled", package.DryIceEnabled.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.DryIceIsForMedicalUse", package.DryIceIsForMedicalUse.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.DryIceRegulationSet", EnumHelper.GetDescription((UpsDryIceRegulationSet) package.DryIceRegulationSet));
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.DryIceWeight", package.DryIceWeight.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.Girth", package.Girth.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.Insurance", package.Insurance.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.InsurancePennyOne", package.InsurancePennyOne.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.InsuranceValue", package.InsuranceValue.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.IsLargePackage", package.IsLargePackage.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.LongestSide", package.LongestSide.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.PackagingType", EnumHelper.GetDescription((UpsPackagingType) package.PackagingType));
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.SecondLongestSize", package.SecondLongestSize.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.VerbalConfirmationEnabled", package.VerbalConfirmationEnabled.ToString());
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.VerbalConfirmationName", package.VerbalConfirmationName);
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.VerbalConfirmationPhone", package.VerbalConfirmationPhone);
                telemetryEvent.AddProperty($"Label.Ups.Package.{packageIndex}.VerbalConfirmationPhoneExtension", package.VerbalConfirmationPhoneExtension);
            }
        }
    }
}
