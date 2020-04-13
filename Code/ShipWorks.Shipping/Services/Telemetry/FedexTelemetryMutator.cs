using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.FedEx;

namespace ShipWorks.Shipping.Services.Telemetry
{
    /// <summary>
    /// Sets FedEx-specific telemetry
    /// </summary>
    [KeyedComponent(typeof(ICarrierTelemetryMutator), ShipmentTypeCode.FedEx)]
    public class FedexTelemetryMutator : ICarrierTelemetryMutator
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
        /// Set Fedex specific shipment telemetry
        /// </summary>
        [NDependIgnoreLongMethod]
        private void SetShipmentTelemetry(TrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            var fedExShipment = shipment.FedEx;

            telemetryEvent.AddProperty("Label.FedEx.BrokerAccount", fedExShipment.BrokerAccount);
            telemetryEvent.AddProperty("Label.FedEx.BrokerCity", fedExShipment.BrokerCity);
            telemetryEvent.AddProperty("Label.FedEx.BrokerCompany", fedExShipment.BrokerCompany);
            telemetryEvent.AddProperty("Label.FedEx.BrokerCountryCode", fedExShipment.BrokerCountryCode);
            telemetryEvent.AddProperty("Label.FedEx.BrokerEmail", fedExShipment.BrokerEmail);
            telemetryEvent.AddProperty("Label.FedEx.BrokerEnabled", fedExShipment.BrokerEnabled.ToString());
            telemetryEvent.AddProperty("Label.FedEx.BrokerFirstName", fedExShipment.BrokerFirstName);
            telemetryEvent.AddProperty("Label.FedEx.BrokerLastName", fedExShipment.BrokerLastName);
            telemetryEvent.AddProperty("Label.FedEx.BrokerPhone", fedExShipment.BrokerPhone);
            telemetryEvent.AddProperty("Label.FedEx.BrokerPhoneExtension", fedExShipment.BrokerPhoneExtension);
            telemetryEvent.AddProperty("Label.FedEx.BrokerPostalCode", fedExShipment.BrokerPostalCode);
            telemetryEvent.AddProperty("Label.FedEx.BrokerStateProvCode", fedExShipment.BrokerStateProvCode);
            telemetryEvent.AddProperty("Label.FedEx.BrokerStreet1", fedExShipment.BrokerStreet1);
            telemetryEvent.AddProperty("Label.FedEx.BrokerStreet2", fedExShipment.BrokerStreet2);
            telemetryEvent.AddProperty("Label.FedEx.BrokerStreet3", fedExShipment.BrokerStreet3);
            telemetryEvent.AddProperty("Label.FedEx.CodAccountNumber", fedExShipment.CodAccountNumber);
            telemetryEvent.AddProperty("Label.FedEx.CodAddFreight", fedExShipment.CodAddFreight.ToString());
            telemetryEvent.AddProperty("Label.FedEx.CodAmount", fedExShipment.CodAmount.ToString());
            telemetryEvent.AddProperty("Label.FedEx.CodChargeBasis", fedExShipment.CodChargeBasis.ToString());
            telemetryEvent.AddProperty("Label.FedEx.CodCity", fedExShipment.CodCity);
            telemetryEvent.AddProperty("Label.FedEx.CodCompany", fedExShipment.CodCompany);
            telemetryEvent.AddProperty("Label.FedEx.CodCountryCode", fedExShipment.CodCountryCode);
            telemetryEvent.AddProperty("Label.FedEx.CodEnabled", fedExShipment.CodEnabled.ToString());
            telemetryEvent.AddProperty("Label.FedEx.CodFirstName", fedExShipment.CodFirstName);
            telemetryEvent.AddProperty("Label.FedEx.CodLastName", fedExShipment.CodLastName);
            telemetryEvent.AddProperty("Label.FedEx.CodOriginID", fedExShipment.CodOriginID.ToString());
            telemetryEvent.AddProperty("Label.FedEx.CodPaymentType", EnumHelper.GetDescription((FedExCodPaymentType) fedExShipment.CodPaymentType));
            telemetryEvent.AddProperty("Label.FedEx.CodPhone", fedExShipment.CodPhone);
            telemetryEvent.AddProperty("Label.FedEx.CodPostalCode", fedExShipment.CodPostalCode);
            telemetryEvent.AddProperty("Label.FedEx.CodStateProvCode", fedExShipment.CodStateProvCode);
            telemetryEvent.AddProperty("Label.FedEx.CodStreet1", fedExShipment.CodStreet1);
            telemetryEvent.AddProperty("Label.FedEx.CodStreet2", fedExShipment.CodStreet2);
            telemetryEvent.AddProperty("Label.FedEx.CodStreet3", fedExShipment.CodStreet3);
            telemetryEvent.AddProperty("Label.FedEx.CodTIN", fedExShipment.CodTIN);
            telemetryEvent.AddProperty("Label.FedEx.CodTrackingFormID", fedExShipment.CodTrackingFormID);
            telemetryEvent.AddProperty("Label.FedEx.CodTrackingNumber", fedExShipment.CodTrackingNumber);
            telemetryEvent.AddProperty("Label.FedEx.CommercialInvoice", fedExShipment.CommercialInvoice.ToString());
            telemetryEvent.AddProperty("Label.FedEx.CommercialInvoiceComments", fedExShipment.CommercialInvoiceComments);
            telemetryEvent.AddProperty("Label.FedEx.CommercialInvoiceFileElectronically", fedExShipment.CommercialInvoiceFileElectronically.ToString());
            telemetryEvent.AddProperty("Label.FedEx.CommercialInvoiceFreight", fedExShipment.CommercialInvoiceFreight.ToString());
            telemetryEvent.AddProperty("Label.FedEx.CommercialInvoiceInsurance", fedExShipment.CommercialInvoiceInsurance.ToString());
            telemetryEvent.AddProperty("Label.FedEx.CommercialInvoiceOther", fedExShipment.CommercialInvoiceOther.ToString());
            telemetryEvent.AddProperty("Label.FedEx.CommercialInvoicePurpose", EnumHelper.GetDescription((FedExCommercialInvoicePurpose) fedExShipment.CommercialInvoicePurpose));
            telemetryEvent.AddProperty("Label.FedEx.CommercialInvoiceReference", fedExShipment.CommercialInvoiceReference);
            telemetryEvent.AddProperty("Label.FedEx.CommercialInvoiceTermsOfSale", EnumHelper.GetDescription((FedExTermsOfSale) fedExShipment.CommercialInvoiceTermsOfSale));
            telemetryEvent.AddProperty("Label.FedEx.Currency", fedExShipment.Currency != null ? EnumHelper.GetDescription((CurrencyType) fedExShipment.Currency) : "USD");
            telemetryEvent.AddProperty("Label.FedEx.CustomsAdmissibilityPackaging", EnumHelper.GetDescription((FedExPhysicalPackagingType) fedExShipment.CustomsAdmissibilityPackaging));
            telemetryEvent.AddProperty("Label.FedEx.CustomsAESEEI", fedExShipment.CustomsAESEEI);
            telemetryEvent.AddProperty("Label.FedEx.CustomsDocumentsDescription", fedExShipment.CustomsDocumentsDescription);
            telemetryEvent.AddProperty("Label.FedEx.CustomsDocumentsOnly", fedExShipment.CustomsDocumentsOnly.ToString());
            telemetryEvent.AddProperty("Label.FedEx.CustomsExportFilingOption", EnumHelper.GetDescription((FedExCustomsExportFilingOption) fedExShipment.CustomsExportFilingOption));
            telemetryEvent.AddProperty("Label.FedEx.CustomsNaftaDeterminationCode", EnumHelper.GetDescription((FedExNaftaDeterminationCode) fedExShipment.CustomsNaftaDeterminationCode));
            telemetryEvent.AddProperty("Label.FedEx.CustomsNaftaEnabled", fedExShipment.CustomsNaftaEnabled.ToString());
            telemetryEvent.AddProperty("Label.FedEx.CustomsNaftaNetCostMethod", EnumHelper.GetDescription((FedExNaftaNetCostMethod) fedExShipment.CustomsNaftaNetCostMethod));
            telemetryEvent.AddProperty("Label.FedEx.CustomsNaftaPreferenceType", EnumHelper.GetDescription((FedExNaftaPreferenceCriteria) fedExShipment.CustomsNaftaPreferenceType));
            telemetryEvent.AddProperty("Label.FedEx.CustomsNaftaProducerId", fedExShipment.CustomsNaftaProducerId);
            telemetryEvent.AddProperty("Label.FedEx.CustomsOptionsDescription", fedExShipment.CustomsOptionsDesription);
            telemetryEvent.AddProperty("Label.FedEx.CustomsOptionsType", EnumHelper.GetDescription((FedExCustomsOptionType) fedExShipment.CustomsOptionsType));
            telemetryEvent.AddProperty("Label.FedEx.CustomsRecipientIdentificationType", EnumHelper.GetDescription((FedExCustomsRecipientIdentificationType) fedExShipment.CustomsRecipientIdentificationType));
            telemetryEvent.AddProperty("Label.FedEx.CustomsRecipientIdentificationValue", fedExShipment.CustomsRecipientIdentificationValue);
            telemetryEvent.AddProperty("Label.FedEx.CustomsRecipientTIN", fedExShipment.CustomsRecipientTIN);
            telemetryEvent.AddProperty("Label.FedEx.DropoffType", EnumHelper.GetDescription((FedExDropoffType) fedExShipment.DropoffType));
            telemetryEvent.AddProperty("Label.FedEx.EmailNotifyBroker", fedExShipment.EmailNotifyBroker != 0 ? "True" : "False");
            telemetryEvent.AddProperty("Label.FedEx.EmailNotifyMessage", fedExShipment.EmailNotifyMessage);
            telemetryEvent.AddProperty("Label.FedEx.EmailNotifyOther", fedExShipment.EmailNotifyOther != 0 ? "True" : "False");
            telemetryEvent.AddProperty("Label.FedEx.EmailNotifyOtherAddress", fedExShipment.EmailNotifyOtherAddress);
            telemetryEvent.AddProperty("Label.FedEx.EmailNotifyRecipient", fedExShipment.EmailNotifyRecipient != 0 ? "True" : "False");
            telemetryEvent.AddProperty("Label.FedEx.EmailNotifySender", fedExShipment.EmailNotifySender != 0 ? "True" : "False");
            telemetryEvent.AddProperty("Label.FedEx.FedExAccountID", fedExShipment.FedExAccountID.ToString());
            telemetryEvent.AddProperty("Label.FedEx.FedExHoldAtLocationEnabled", fedExShipment.FedExHoldAtLocationEnabled.ToString());
            telemetryEvent.AddProperty("Label.FedEx.FimsAirWaybill", fedExShipment.FimsAirWaybill);
            telemetryEvent.AddProperty("Label.FedEx.FreightBookingNumber", fedExShipment.FreightBookingNumber);
            telemetryEvent.AddProperty("Label.FedEx.FreightClass", EnumHelper.GetDescription(fedExShipment.FreightClass));
            telemetryEvent.AddProperty("Label.FedEx.FreightCollectTerms", EnumHelper.GetDescription(fedExShipment.FreightCollectTerms));
            telemetryEvent.AddProperty("Label.FedEx.FreightGuaranteeDate", fedExShipment.FreightGuaranteeDate.ToShortDateString());
            telemetryEvent.AddProperty("Label.FedEx.FreightGuaranteeType", EnumHelper.GetDescription(fedExShipment.FreightGuaranteeType));
            telemetryEvent.AddProperty("Label.FedEx.FreightInsideDelivery", fedExShipment.FreightInsideDelivery.ToString());
            telemetryEvent.AddProperty("Label.FedEx.FreightInsidePickup", fedExShipment.FreightInsidePickup.ToString());
            telemetryEvent.AddProperty("Label.FedEx.FreightLoadAndCount", fedExShipment.FreightLoadAndCount.ToString());
            telemetryEvent.AddProperty("Label.FedEx.FreightRole", EnumHelper.GetDescription(fedExShipment.FreightRole));
            telemetryEvent.AddProperty("Label.FedEx.FreightSpecialServices", EnumHelper.GetDescription((FedExFreightSpecialServicesType) fedExShipment.FreightSpecialServices));
            telemetryEvent.AddProperty("Label.FedEx.FreightTotalHandlingUnits", fedExShipment.FreightTotalHandlinUnits.ToString());
            telemetryEvent.AddProperty("Label.FedEx.HoldCity", fedExShipment.HoldCity);
            telemetryEvent.AddProperty("Label.FedEx.HoldCompanyName", fedExShipment.HoldCompanyName);
            telemetryEvent.AddProperty("Label.FedEx.HoldContactId", fedExShipment.HoldContactId);
            telemetryEvent.AddProperty("Label.FedEx.HoldCountryCode", fedExShipment.HoldCountryCode);
            telemetryEvent.AddProperty("Label.FedEx.HoldEmailAddress", fedExShipment.HoldEmailAddress);
            telemetryEvent.AddProperty("Label.FedEx.HoldFaxNumber", fedExShipment.HoldFaxNumber);
            telemetryEvent.AddProperty("Label.FedEx.HoldLocationId", fedExShipment.HoldLocationId);
            telemetryEvent.AddProperty("Label.FedEx.HoldLocationType", fedExShipment.HoldLocationType != null ? EnumHelper.GetDescription((FedExLocationType) fedExShipment.HoldLocationType) : string.Empty);
            telemetryEvent.AddProperty("Label.FedEx.HoldPagerNumber", fedExShipment.HoldPagerNumber);
            telemetryEvent.AddProperty("Label.FedEx.HoldPersonName", fedExShipment.HoldPersonName);
            telemetryEvent.AddProperty("Label.FedEx.HoldPhoneExtension", fedExShipment.HoldPhoneExtension);
            telemetryEvent.AddProperty("Label.FedEx.HoldPhoneNumber", fedExShipment.HoldPhoneNumber);
            telemetryEvent.AddProperty("Label.FedEx.HoldPostalCode", fedExShipment.HoldPostalCode);
            telemetryEvent.AddProperty("Label.FedEx.HoldResidential", fedExShipment.HoldResidential?.ToString() ?? "False");
            telemetryEvent.AddProperty("Label.FedEx.HoldStateOrProvinceCode", fedExShipment.HoldStateOrProvinceCode);
            telemetryEvent.AddProperty("Label.FedEx.HoldStreet1", fedExShipment.HoldStreet1);
            telemetryEvent.AddProperty("Label.FedEx.HoldStreet2", fedExShipment.HoldStreet2);
            telemetryEvent.AddProperty("Label.FedEx.HoldStreet3", fedExShipment.HoldStreet3);
            telemetryEvent.AddProperty("Label.FedEx.HoldTitle", fedExShipment.HoldTitle);
            telemetryEvent.AddProperty("Label.FedEx.HoldUrbanizationCode", fedExShipment.HoldUrbanizationCode);
            telemetryEvent.AddProperty("Label.FedEx.HomeDeliveryDate", fedExShipment.HomeDeliveryDate.ToShortDateString());
            telemetryEvent.AddProperty("Label.FedEx.HomeDeliveryInstructions", fedExShipment.HomeDeliveryInstructions);
            telemetryEvent.AddProperty("Label.FedEx.HomeDeliveryPhone", fedExShipment.HomeDeliveryPhone);
            telemetryEvent.AddProperty("Label.FedEx.HomeDeliveryType", EnumHelper.GetDescription((FedExHomeDeliveryType) fedExShipment.HomeDeliveryType));
            telemetryEvent.AddProperty("Label.FedEx.ImporterAccount", fedExShipment.ImporterAccount);
            telemetryEvent.AddProperty("Label.FedEx.ImporterCity", fedExShipment.ImporterCity);
            telemetryEvent.AddProperty("Label.FedEx.ImporterCompany", fedExShipment.ImporterCompany);
            telemetryEvent.AddProperty("Label.FedEx.ImporterCountryCode", fedExShipment.ImporterCountryCode);
            telemetryEvent.AddProperty("Label.FedEx.ImporterFirstName", fedExShipment.ImporterFirstName);
            telemetryEvent.AddProperty("Label.FedEx.ImporterLastName", fedExShipment.ImporterLastName);
            telemetryEvent.AddProperty("Label.FedEx.ImporterOfRecord", fedExShipment.ImporterOfRecord.ToString());
            telemetryEvent.AddProperty("Label.FedEx.ImporterPhone", fedExShipment.ImporterPhone);
            telemetryEvent.AddProperty("Label.FedEx.ImporterPostalCode", fedExShipment.ImporterPostalCode);
            telemetryEvent.AddProperty("Label.FedEx.ImporterStateProvCode", fedExShipment.ImporterStateProvCode);
            telemetryEvent.AddProperty("Label.FedEx.ImporterStreet1", fedExShipment.ImporterStreet1);
            telemetryEvent.AddProperty("Label.FedEx.ImporterStreet2", fedExShipment.ImporterStreet2);
            telemetryEvent.AddProperty("Label.FedEx.ImporterStreet3", fedExShipment.ImporterStreet3);
            telemetryEvent.AddProperty("Label.FedEx.ImporterTIN", fedExShipment.ImporterTIN);
            telemetryEvent.AddProperty("Label.FedEx.InternationalTrafficInArmsService", fedExShipment.InternationalTrafficInArmsService?.ToString() ?? "False");
            telemetryEvent.AddProperty("Label.FedEx.IntlExportDetailEntryNumber", fedExShipment.IntlExportDetailEntryNumber);
            telemetryEvent.AddProperty("Label.FedEx.IntlExpotDetailForeignTradeZoneCode", fedExShipment.IntlExportDetailForeignTradeZoneCode);
            telemetryEvent.AddProperty("Label.FedEx.IntlExportDetailLicenseOrPermitExpirationDate", fedExShipment.IntlExportDetailLicenseOrPermitExpirationDate?.ToShortDateString() ?? string.Empty);
            telemetryEvent.AddProperty("Label.FedEx.IntlExportDetailLicenseOrPermitNumber", fedExShipment.IntlExportDetailLicenseOrPermitNumber);
            telemetryEvent.AddProperty("Label.FedEx.IntlExportDetailType", EnumHelper.GetDescription((FedExInternationalControlledExportType) fedExShipment.IntlExportDetailType));
            telemetryEvent.AddProperty("Label.FedEx.LinearUnitType", EnumHelper.GetDescription((FedExLinearUnitOfMeasure) fedExShipment.LinearUnitType));
            telemetryEvent.AddProperty("Label.FedEx.MaskedData", fedExShipment.MaskedData != null ? EnumHelper.GetDescription((FedExMaskedDataType) fedExShipment.MaskedData) : string.Empty);
            telemetryEvent.AddProperty("Label.FedEx.MasterFormID", fedExShipment.MasterFormID);
            telemetryEvent.AddProperty("Label.FedEx.NonStandardContainer", fedExShipment.NonStandardContainer.ToString());
            telemetryEvent.AddProperty("Label.FedEx.OriginResidentialDetermination", fedExShipment.OriginResidentialDetermination.ToString());
            telemetryEvent.AddProperty("Label.FedEx.PackagingType", EnumHelper.GetDescription((FedExPackagingType) fedExShipment.PackagingType));
            telemetryEvent.AddProperty("Label.FedEx.PayorDutiesAccount", fedExShipment.PayorDutiesAccount);
            telemetryEvent.AddProperty("Label.FedEx.PayorDutiesCountryCode", fedExShipment.PayorDutiesCountryCode);
            telemetryEvent.AddProperty("Label.FedEx.PayorDutiesName", fedExShipment.PayorDutiesName);
            telemetryEvent.AddProperty("Label.FedEx.PayorDutiesType", EnumHelper.GetDescription((FedExPayorType) fedExShipment.PayorDutiesType));
            telemetryEvent.AddProperty("Label.FedEx.PayorTransportAccount", fedExShipment.PayorTransportAccount);
            telemetryEvent.AddProperty("Label.FedEx.PayorTransportName", fedExShipment.PayorTransportName);
            telemetryEvent.AddProperty("Label.FedEx.PayorTransportType", EnumHelper.GetDescription((FedExPayorType) fedExShipment.PayorTransportType));
            telemetryEvent.AddProperty("Label.FedEx.ReferenceCustomer", fedExShipment.ReferenceCustomer);
            telemetryEvent.AddProperty("Label.FedEx.ReferenceFIMS", fedExShipment.ReferenceFIMS);
            telemetryEvent.AddProperty("Label.FedEx.ReferenceInvoice", fedExShipment.ReferenceInvoice);
            telemetryEvent.AddProperty("Label.FedEx.ReferencePO", fedExShipment.ReferencePO);
            telemetryEvent.AddProperty("Label.FedEx.ReferenceShipmentIntegrity", fedExShipment.ReferenceShipmentIntegrity);
            telemetryEvent.AddProperty("Label.FedEx.RequestedLabelFormat", EnumHelper.GetDescription((ThermalLanguage) fedExShipment.RequestedLabelFormat));
            telemetryEvent.AddProperty("Label.FedEx.ReturnSaturdayPickup", fedExShipment.ReturnSaturdayPickup.ToString());
            telemetryEvent.AddProperty("Label.FedEx.ReturnsClearance", fedExShipment.ReturnsClearance.ToString());
            telemetryEvent.AddProperty("Label.FedEx.ReturnType", EnumHelper.GetDescription((FedExReturnType) fedExShipment.ReturnType));
            telemetryEvent.AddProperty("Label.FedEx.RmaNumber", fedExShipment.RmaNumber);
            telemetryEvent.AddProperty("Label.FedEx.RmaReason", fedExShipment.RmaReason);
            telemetryEvent.AddProperty("Label.FedEx.SaturdayDelivery", fedExShipment.SaturdayDelivery.ToString());
            telemetryEvent.AddProperty("Label.FedEx.Signature", EnumHelper.GetDescription((FedExSignatureType) fedExShipment.Signature));
            telemetryEvent.AddProperty("Label.FedEx.SmartPostConfirmation", fedExShipment.SmartPostConfirmation.ToString());
            telemetryEvent.AddProperty("Label.FedEx.SmartPostCustomerManifest", fedExShipment.SmartPostCustomerManifest);
            telemetryEvent.AddProperty("Label.FedEx.SmartPostEndorsement", EnumHelper.GetDescription((FedExSmartPostEndorsement) fedExShipment.SmartPostEndorsement));
            telemetryEvent.AddProperty("Label.FedEx.SmartPostHubID", fedExShipment.SmartPostHubID);
            telemetryEvent.AddProperty("Label.FedEx.SmartPostIndicia", EnumHelper.GetDescription((FedExSmartPostIndicia) fedExShipment.SmartPostIndicia));
            telemetryEvent.AddProperty("Label.FedEx.SmartPostUspsApplicationId", fedExShipment.SmartPostUspsApplicationId);
            telemetryEvent.AddProperty("Label.FedEx.ThirdPartyConsignee", fedExShipment.ThirdPartyConsignee.ToString());
            telemetryEvent.AddProperty("Label.FedEx.TrafficInArmsLicenseNumber", fedExShipment.TrafficInArmsLicenseNumber);
            telemetryEvent.AddProperty("Label.FedEx.WeightUnitType", EnumHelper.GetDescription((WeightUnitOfMeasure) fedExShipment.WeightUnitType));
        }

        /// <summary>
        /// Set FedEx specific package telemetry
        /// </summary>
        private void SetPackageTelemetry(TrackedDurationEvent telemetryEvent, IShipmentEntity shipment)
        {
            int packageIndex = 0;

            foreach (IFedExPackageEntity package in shipment.FedEx.Packages)
            {
                packageIndex++;

                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.AlcoholRecipientType", ((ShipWorks.Shipping.Carriers.FedEx.WebServices.OpenShip.AlcoholRecipientType) package.AlcoholRecipientType).ToString("G"));
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.BatteryMaterial", EnumHelper.GetDescription(package.BatteryMaterial));
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.BatteryPacking", EnumHelper.GetDescription(package.BatteryPacking));
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.BatteryRegulatorySubtype", EnumHelper.GetDescription(package.BatteryRegulatorySubtype));
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.ContainerType", package.ContainerType);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.ContainsAlcohol", package.ContainsAlcohol.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.DangerousGoodsAccessibilityType", EnumHelper.GetDescription((FedExDangerousGoodsAccessibilityType) package.DangerousGoodsAccessibilityType));
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.DangerousGoodsCargoAircraftOnly", package.DangerousGoodsCargoAircraftOnly.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.DangerousGoodsEmergencyContactPhone", package.DangerousGoodsEmergencyContactPhone);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.DangerousGoodsEnabled", package.DangerousGoodsEnabled.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.DangerousGoodsOfferor", package.DangerousGoodsOfferor);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.DangerousGoodsPackagingCount", package.DangerousGoodsPackagingCount.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.DangerousGoodsType", EnumHelper.GetDescription((FedExDangerousGoodsMaterialType) package.DangerousGoodsType));
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.DeclaredValue", package.DeclaredValue.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.DimsAddWeight", package.DimsAddWeight.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.DimsWeight", package.DimsWeight.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.DryIceWeight", package.DryIceWeight.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.FedExPackageID", package.FedExPackageID.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.FreightPackaging", EnumHelper.GetDescription(package.FreightPackaging));
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.FreightPieces", package.FreightPieces.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.HazardousMaterialClass", package.HazardousMaterialClass);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.HazardousMaterialNumber", package.HazardousMaterialNumber);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.HazardousMaterialPackingGroup", EnumHelper.GetDescription((FedExHazardousMaterialsPackingGroup) package.HazardousMaterialPackingGroup));
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.HazardousMaterialProperName", package.HazardousMaterialProperName);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.HazardousMaterialQuantityUnits", EnumHelper.GetDescription((FedExHazardousMaterialsQuantityUnits) package.HazardousMaterialQuanityUnits));
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.HazardousMaterialQuantityValue", package.HazardousMaterialQuantityValue.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.HazardousMaterialTechnicalName", package.HazardousMaterialTechnicalName);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.Insurance", package.Insurance.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.InsurancePennyOne", package.InsurancePennyOne.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.InsuranceValue", package.InsuranceValue.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.NumberOfContainers", package.NumberOfContainers.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.PackingDetailsCargoAircraftOnly", package.PackingDetailsCargoAircraftOnly.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.PackingDetailsPackingInstructions", package.PackingDetailsPackingInstructions);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.PriorityAlert", package.PriorityAlert.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.PriorityAlertDetailContent", package.PriorityAlertDetailContent);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.PriorityAlertEnhancementType", EnumHelper.GetDescription((FedExPriorityAlertEnhancementType) package.PriorityAlertEnhancementType));
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.SignatoryContactName", package.SignatoryContactName);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.SignatoryPlace", package.SignatoryPlace);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.SignatoryTitle", package.SignatoryTitle);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.SkidPieces", package.SkidPieces.ToString());
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.TrackingNumber", package.TrackingNumber);
                telemetryEvent.AddProperty($"Label.FedEx.Package.{packageIndex}.Weight", package.Weight.ToString());
            }
        }
    }
}
