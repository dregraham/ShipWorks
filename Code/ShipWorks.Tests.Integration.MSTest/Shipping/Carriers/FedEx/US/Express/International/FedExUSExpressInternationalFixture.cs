using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.International
{
    public class FedExUSExpressInternationalFixture : FedExInternationalPrototypeFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FedExUSExpressInternationalFixture" /> class.
        /// </summary>
        public FedExUSExpressInternationalFixture()
            : base()
        {

        }

        // Need to check where to assign these three values
        public string ShipperTinType { get; set; }
        public string ShipperTinsNumber { get; set; }
        public string PackageLineItemDimensionUnits { get; set; }

        public string InternationalControlledExportDetailType { get; set; }
        public string InternationalControlledExportDetailForeignTradeZoneCode { get; set; }
        public string InternationalControlledExportDetailEntryNumber { get; set; }
        public string InternationalControlledExportDetailLicenseOrPermitNumber { get; set; }
        public string InternationalControlledExportDetailLicenseOrPermitExpirationDate { get; set; }

        public string TrafficInArmsLicenseOrExemptionNumber { get; set; }


        public string BrokerType { get; set; }
        public string BrokerAccountNumber { get; set; }
        public string BrokerTinType { get; set; }
        public string BrokerTinsNumber { get; set; }
        public string BrokerContactId { get; set; }
        public string BrokerPersonName { get; set; }
        public string BrokerTitle { get; set; }
        public string BrokerCompanyName { get; set; }
        public string BrokerPhoneNumber { get; set; }
        public string BrokerPhoneExtension { get; set; }
        public string BrokerEMailAddress { get; set; }
        public string BrokerStreetLines { get; set; }
        public string BrokerCity { get; set; }
        public string BrokerStateOrProvinceCode { get; set; }
        public string BrokerPostalCode { get; set; }
        public string BrokerCountryCode { get; set; }
        
        public string CustomsRecipientIdType { get; set; }
        public string CustomsRecipientIdValue { get; set; }

        public string CustomsClearanceInsuranceCurrency { get; set; }
        public string CustomsClearanceInsuranceAmount { get; set; }

        public string CommercialInvoiceComments { get; set; }
        public string CommercialInvoiceFreightChargeCurrency { get; set; }
        public string CommercialInvoiceFreightChargeAmount { get; set; }
        public string CommercialInvoiceTaxesCurrency { get; set; }
        public string CommercialInvoiceTaxesAmount { get; set; }
        public string CommercialInvoicePurpose { get; set; }
        public string CommercialInvoiceCustomerReferenceType { get; set; }
        public string CommercialInvoiceCustomerReferenceValue { get; set; }
        public string CommercialInvoiceTermsOfSale { get; set; }

        public string ExportDetailB13AFilingOption { get; set; }
        public string ExportDetailExportComplianceStatement { get; set; }
        
        public string HazardClass { get; set; }
        public string HazardDescriptionID { get; set; }
        public string HazardousPackingGroup { get; set; }
        public string HazardProperShippingName { get; set; }
        public string HazardQuantityAmount { get; set; }
        public string HazardQuantityUnits { get; set; }
        public string DangerCounts { get; set; }
        public string DangerEmergencyContactNumber { get; set; }
        public string DangerOfferor { get; set; }
        public string DangerUnits { get; set; }
        public string PackageDangerousGoodsDetail { get; set; }


        private static List<ColumnPropertyMapDefinition> columnPropertyMap;

        public static List<ColumnPropertyMapDefinition> Mapping
        {
            get
            {
                if (columnPropertyMap == null || !columnPropertyMap.Any())
                {
                    columnPropertyMap = new List<ColumnPropertyMapDefinition>();

                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Brokers.Broker.Account Number", PropertyName = "BrokerAccountNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Address.City", PropertyName = "BrokerCity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.CompanyName", PropertyName = "BrokerCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.ContactId", PropertyName = "BrokerContactId", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Address.CountryCode", PropertyName = "BrokerCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.EMailAddress", PropertyName = "BrokerEMailAddress", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.PersonName", PropertyName = "BrokerPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.PhoneExtension", PropertyName = "BrokerPhoneExtension", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.PhoneNumber", PropertyName = "BrokerPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Address.PostalCode", PropertyName = "BrokerPostalCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Address.StateOrProvinceCode", PropertyName = "BrokerStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Brokers.Broker.Address.StreetLines", PropertyName = "BrokerStreetLines", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Tins.Number", PropertyName = "BrokerTinsNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Tins.TinType", PropertyName = "BrokerTinType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.Title", PropertyName = "BrokerTitle", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Brokers..Type", PropertyName = "BrokerType", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodAccountNumber", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodChargeBasis", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodCity", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodCollectionAmount", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodCollectionCurrency", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodCompanyName", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodCountryCode", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodDetailCollectionType", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodPersonName", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodPhoneNumber", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodPostalCode", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodReferenceIndicator", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodResidential", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodStreetLines", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodTinNumber", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodTinType", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "CodTitle", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CommercialInvoice.Comments", PropertyName = "CommercialInvoiceComments", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CommercialInvoice.CustomerReferences.CustomerReferenceType", PropertyName = "CommercialInvoiceCustomerReferenceType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CommercialInvoice.CustomerReferences.Value", PropertyName = "CommercialInvoiceCustomerReferenceValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CommercialInvoice.FreightCharge.Amount", PropertyName = "CommercialInvoiceFreightChargeAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CommercialInvoice.FreightCharge.Currency", PropertyName = "CommercialInvoiceFreightChargeCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CommercialInvoice.Purpose", PropertyName = "CommercialInvoicePurpose", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CommercialInvoice.TaxesOrMiscellaneousCharge.Amount", PropertyName = "CommercialInvoiceTaxesAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CommercialInvoice.TaxesOrMiscellaneousCharge.Currency", PropertyName = "CommercialInvoiceTaxesCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CommercialInvoice.TermsOfSale", PropertyName = "CommercialInvoiceTermsOfSale", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.CountryOfManufacture", PropertyName = "CommoditiesCountryOfManufacture", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.CustomsValue.Amount", PropertyName = "CommoditiesCustomsValueAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.CustomsValue.Currency", PropertyName = "CommoditiesCustomsValueCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.Description", PropertyName = "CommoditiesDescription", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.HarmonizedCode", PropertyName = "CommoditiesHarmonizedCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.NumberOfPieces", PropertyName = "CommoditiesNumberOfPieces", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.Quantity", PropertyName = "CommoditiesQuantity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.QuantityUnits", PropertyName = "CommoditiesQuantityUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.UnitPrice.Amount", PropertyName = "CommoditiesUnitPriceAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.UnitPrice.Currency", PropertyName = "CommoditiesUnitPriceCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.Weight.Units", PropertyName = "CommoditiesWeightUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.Weight.Value", PropertyName = "CommoditiesWeightValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.CustomerReferences.CustomerReferenceType", PropertyName = "CustomerReferenceType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.CustomerReferences.Value", PropertyName = "CustomerReferenceValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.TransactionDetail.CustomerTransactionId", PropertyName = "CustomerTransactionId", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.DocumentContent", PropertyName = "CustomsClearanceDocumentContent", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.InsuranceCharges.Amount", PropertyName = "CustomsClearanceInsuranceAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.InsuranceCharges.Currency", PropertyName = "CustomsClearanceInsuranceCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CustomsValue.Amount", PropertyName = "CustomsClearanceValueAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CustomsValue.Currency", PropertyName = "CustomsClearanceValueCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CustomsOptions", PropertyName = "CustomsOptionDescription", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CustomsOptions.Type", PropertyName = "CustomsOptionType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.RecipientCustomsId.Type", PropertyName = "CustomsRecipientIdType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.RecipientCustomsId.Value", PropertyName = "CustomsRecipientIdValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Accessibility", PropertyName = "DangerousGoodsAccessibility", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.DangerousGoodsDetail.CargoAircraftOnly", PropertyName = "DangerousGoodsCargoAircraftOnly", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.DropoffType", PropertyName = "DropoffType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.ShipmentDryIceDetail.TotalWeight.Units", PropertyName = "DryIceWeightUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.ShipmentDryIceDetail.TotalWeight.Value", PropertyName = "DryIceWeightValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "DutiesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = "DutiesAccountNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "DutiesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "DutiesCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.DutiesPayment.PaymentType", PropertyName = "DutiesPaymentType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "DutiesPayment.Payor.ResponsibleParty.Contact.PersonName", PropertyName = "DutiesPersonName", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "EmailAddress", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "EmailAllowedSpecialServices", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "EmailFormat", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "EmailLanguageCode", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "EmailNotifyOnDelivery", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "EmailRecipientType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.ExportDetail.B13AFilingOption", PropertyName = "ExportDetailB13AFilingOption", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.ExportDetail.ExportComplianceStatement", PropertyName = "ExportDetailExportComplianceStatement", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "FedExAccountNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest. ExpressFreightDetail.BookingConfirmationNumber", PropertyName = "FreightDetailBookingConfirmationNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.ExpressFreightDetail.PackingListEnclosed", PropertyName = "FreightDetailPackingListEnclosed", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.ExpressFreightDetail.ShippersLoadAndCount", PropertyName = "FreightDetailShippersLoadAndCount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.LocationContactAndAddress.Address.City", PropertyName = "HoldCity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.LocationContactAndAddress.Contact.CompanyName", PropertyName = "HoldCompanyName", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "HoldContactPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.LocationContactAndAddress.Address.CountryCode", PropertyName = "HoldCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.PhoneNumber", PropertyName = "HoldDetailPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.LocationType", PropertyName = "HoldLocationType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.LocationContactAndAddress.Contact.PersonName", PropertyName = "HoldPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.LocationContactAndAddress.Contact.PhoneNumber", PropertyName = "HoldPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.LocationContactAndAddress.Address.PostalCode", PropertyName = "HoldPostalCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.LocationContactAndAddress.Address.StateOrProvinceCode", PropertyName = "HoldStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.LocationContactAndAddress.Address.StreetLines", PropertyName = "HoldStreetLines", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "ImporterOfRecordCity", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "ImporterOfRecordCompanyName", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "ImporterOfRecordCountryCode", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "ImporterOfRecordPersonName", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "ImporterOfRecordPhoneNumber", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "ImporterOfRecordPostalCode", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "ImporterOfRecordStateProvinceCode", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "ImporterOfRecordStreetLines", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.InternationalControlledExportDetail.EntryNumber", PropertyName = "InternationalControlledExportDetailEntryNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.InternationalControlledExportDetail.ForeignTradeZoneCode", PropertyName = "InternationalControlledExportDetailForeignTradeZoneCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.InternationalControlledExportDetail.LicenseOrPermitExpirationDate", PropertyName = "InternationalControlledExportDetailLicenseOrPermitExpirationDate", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.InternationalControlledExportDetail.LicenseOrPermitNumber", PropertyName = "InternationalControlledExportDetailLicenseOrPermitNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.InternationalControlledExportDetail.Type", PropertyName = "InternationalControlledExportDetailType", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "LabelSpecificationLabelStockType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackageCount", PropertyName = "PackageCount", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "PackageLineItemDimensionUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Height", PropertyName = "PackageLineItemHeight", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.InsuredValue.Amount", PropertyName = "PackageLineItemInsuredValueAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.InsuredValue.Currency", PropertyName = "PackageLineItemInsuredValueCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Length", PropertyName = "PackageLineItemLength", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.SpecialServiceTypes.PriorityAlertDetail.Content", PropertyName = "PackageLineItemPriorityContent", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.SpecialServiceTypes.PriorityAlertDetail.EnhancementTypes", PropertyName = "PackageLineItemPriorityEnhancementType", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "PackageLineItemSpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "PackageLineItemSpecialServiceType2", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "PackageLineItemSpecialServiceType3", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Units", PropertyName = "PackageLineItemUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Units", PropertyName = "PackageLineItemWeightUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.value", PropertyName = "PackageLineItemWeightValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Width", PropertyName = "PackageLineItemWidth", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.PackagingType", PropertyName = "PackagingType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RateRequestTypes", PropertyName = "RateRequestTypes", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.City", PropertyName = "RecipientCity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.Recipient.Contact.CompanyName", PropertyName = "RecipientCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.CountryCode", PropertyName = "RecipientCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.PersonName", PropertyName = "RecipientPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.PhoneNumber", PropertyName = "RecipientPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.PostalCode", PropertyName = "RecipientPostalCode", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "RecipientResidential", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.StateOrProvinceCode", PropertyName = "RecipientStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.StreetLines", PropertyName = "RecipientStreetLines", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.Account number", PropertyName = "ResponsiblePartyAccountNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "ResponsiblePartyCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShippingChargesPayment.PaymentType", PropertyName = "ResponsiblePartyPaymentType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Contact.PersonName", PropertyName = "ResponsiblePartyPersonName", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "ReturnRmaNumber", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "ReturnRmaReason", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ReturnshipmentDetail.ReturnType", PropertyName = "ReturnType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SaveLabel", PropertyName = "SaveLabel", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ServiceType", PropertyName = "ShipmentServiceType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SignatureOptionDetail.OptionType", PropertyName = "ShipmentSignatureOptionType", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "ShipmentSignatureReleaseNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.TotalWeight.Value", PropertyName = "ShipmentTotalWeightValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.TotalWeight.Units", PropertyName = "ShipmentWeightUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.City", PropertyName = "ShipperCity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.CompanyName", PropertyName = "ShipperCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.CountryCode", PropertyName = "ShipperCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.Shipper.Contact.PersonName", PropertyName = "ShipperPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.PhoneNumber", PropertyName = "ShipperPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.PostalCode", PropertyName = "ShipperPostalCode", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "ShipperResidential", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StateOrProvinceCode", PropertyName = "ShipperStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StreetLines", PropertyName = "ShipperStreetLines", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment. Shipper.Tins.Number", PropertyName = "ShipperTinsNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment. Shipper.Tins.TinType", PropertyName = "ShipperTinType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShipTimestamp", PropertyName = "ShipTimestamp", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "SpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "SpecialServiceType2", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.InternationalTrafficInArmsRegulationsDetail.LicenseOrExemptionNumber", PropertyName = "TrafficInArmsLicenseOrExemptionNumber", SpreadsheetColumnIndex = -1 });

                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Description.HazardClass", PropertyName = "HazardClass", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Description.ID", PropertyName = "HazardDescriptionID", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Description.PackingGroup", PropertyName = "HazardousPackingGroup", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Description.ProperShippingName", PropertyName = "HazardProperShippingName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Quantity.Amount", PropertyName = "HazardQuantityAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Quantity.Units", PropertyName = "HazardQuantityUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.NumberOfContainers", PropertyName = "DangerCounts", SpreadsheetColumnIndex = -1 });

                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes1", PropertyName = "PackageLineItemSpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes2", PropertyName = "PackageLineItemSpecialServiceType2", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes3", PropertyName = "PackageLineItemSpecialServiceType3", SpreadsheetColumnIndex = -1 });

                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "DangerCounts", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Signatory.EmergencyContactNumber", PropertyName = "DangerEmergencyContactNumber", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "DangerOfferor", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "DangerUnits", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "", PropertyName = "PackageDangerousGoodsDetail", SpreadsheetColumnIndex = -1 });

                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.ContainerType", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Description.PackingDetails.CargoAircraftOnly", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Description.PackingDetails.PackingInstructions", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Description.SequenceNumber", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Options", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Signatory.ContactName", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Signatory.Place", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Signatory.Title", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.RequestedShipment", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServiceTypes.SpecialServicesRequested(Repetitions).SpecialServiceTypes", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "SpecialServiceType1", SpreadsheetColumnIndex = -1 });
                }

                return columnPropertyMap;
            }
        }

        /// <summary>
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public override ShipmentEntity CreateShipment()
        {
            ShipmentEntity shipment = base.CreateShipment();

            shipment.FedEx.TrafficInArmsLicenseNumber = TrafficInArmsLicenseOrExemptionNumber;
            shipment.FedEx.WeightUnitType = ShipmentWeightUnits.ToLower() == "lb" ? (int)WeightUnitOfMeasure.Pounds : (int)WeightUnitOfMeasure.Kilograms;

            SetupCustomsData(shipment);

            SetupCommercialInvoice(shipment);
            SetupBroker(shipment);
            SetupExportDetail(shipment);

            ApplyDangerousGoods(shipment);

            SetPriortyAlertData(shipment);

            SetupRecipientIdentification(shipment);

            return shipment;
        }

        private void SetupExportDetail(ShipmentEntity shipment)
        {
            if ((!string.IsNullOrEmpty(SpecialServiceType1) && SpecialServiceType1.ToLower() == "international_controlled_export_service") ||
                (!string.IsNullOrEmpty(SpecialServiceType2) && SpecialServiceType2.ToLower() == "international_controlled_export_service"))
            {
                // Hard code as this is the only export type in the tests
                shipment.FedEx.IntlExportDetailType = (int)FedExInternationalControlledExportType.Dea236;
                shipment.FedEx.IntlExportDetailEntryNumber = InternationalControlledExportDetailEntryNumber;
                shipment.FedEx.IntlExportDetailForeignTradeZoneCode = InternationalControlledExportDetailForeignTradeZoneCode;
                shipment.FedEx.IntlExportDetailLicenseOrPermitExpirationDate = DateTime.Parse(InternationalControlledExportDetailLicenseOrPermitExpirationDate);
                shipment.FedEx.IntlExportDetailLicenseOrPermitNumber = InternationalControlledExportDetailLicenseOrPermitNumber;
            }
        }

        /// <summary>
        /// Setups the broker.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetupBroker(ShipmentEntity shipment)
        {
            if (!string.IsNullOrEmpty(BrokerAccountNumber))
            {
                // Only the test cases specifying a broker account have broker account info
                shipment.FedEx.BrokerEnabled = true;

                shipment.FedEx.BrokerAccount = BrokerAccountNumber;
                shipment.FedEx.BrokerCity = BrokerCity;
                shipment.FedEx.BrokerCompany = BrokerCompanyName;
                shipment.FedEx.BrokerCountryCode = BrokerCountryCode;
                shipment.FedEx.BrokerEmail = BrokerEMailAddress;

                List<string> names = ParseName(BrokerPersonName);
                shipment.FedEx.BrokerFirstName = names[0];
                shipment.FedEx.BrokerLastName = names[1];

                shipment.FedEx.BrokerPostalCode = BrokerPostalCode;
                shipment.FedEx.BrokerStateProvCode = BrokerStateOrProvinceCode;
                shipment.FedEx.BrokerStreet1 = BrokerStreetLines;

                shipment.FedEx.BrokerPhone = BrokerPhoneNumber;
                shipment.FedEx.BrokerPhoneExtension = BrokerPhoneExtension;

                // Don't need to worry about broker type since all test cases specify IMPORT (this is hard coded in the manipulator)
            }

        }


        /// <summary>
        /// Setups the recipient identification.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected virtual void SetupRecipientIdentification(ShipmentEntity shipment)
        {
            // All test cases have passport as the value
            shipment.FedEx.CustomsRecipientIdentificationType = (int)FedExCustomsRecipientIdentificationType.Passport;
            shipment.FedEx.CustomsRecipientIdentificationValue = CustomsRecipientIdValue;
        }

        /// <summary>
        /// Setups the customs data.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void SetupCustomsData(ShipmentEntity shipment)
        {
            // Set the filing options
            if (!string.IsNullOrEmpty(ExportDetailB13AFilingOption))
            {
                switch (ExportDetailB13AFilingOption.ToLower())
                {
                    case "not_required":
                        shipment.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.NotRequired;
                        break;
                    case "manually_attached":
                        shipment.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.ManuallyAttached;
                        break;
                    case "filed_electronically":
                        shipment.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.FiledElectonically;
                        break;
                    case "summary_reporting":
                        shipment.FedEx.CustomsExportFilingOption = (int)FedExCustomsExportFilingOption.SummaryReporting;
                        break;
                }
            }

            shipment.FedEx.CustomsAESEEI = ExportDetailExportComplianceStatement;



            // TODO: setup the rest of the customs fields
            // CustomsClearanceInsuranceAmount

            if (!string.IsNullOrEmpty(CustomsOptionType))
            {
                // The only option type is Faulty item so just hard code the type here
                shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.FaultyItem;
                shipment.FedEx.CustomsOptionsDesription = CustomsOptionDescription;
            }
            else
            {
                shipment.FedEx.CustomsOptionsType = (int)FedExCustomsOptionType.None;
            }
        }



        /// <summary>
        /// Sets the linear units.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected override void SetLinearUnits(ShipmentEntity shipment)
        {
            base.SetLinearUnits(shipment);

            if (!string.IsNullOrEmpty(PackageLineItemDimensionUnits))
            {
                if (PackageLineItemDimensionUnits.ToLower() == "cm")
                {
                    shipment.FedEx.LinearUnitType = (int)FedExLinearUnitOfMeasure.CM;
                }
                else
                {
                    shipment.FedEx.LinearUnitType = (int)FedExLinearUnitOfMeasure.IN;
                }
            }
        }

        /// <summary>
        /// Setups the commercial invoice.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        protected virtual void SetupCommercialInvoice(ShipmentEntity shipment)
        {
            // All test cases currently have commercial invoices
            shipment.FedEx.CommercialInvoice = true;

            shipment.FedEx.CommercialInvoiceComments = CommercialInvoiceComments;

            if (!string.IsNullOrEmpty(CommercialInvoiceFreightChargeAmount))
            {
                shipment.FedEx.CommercialInvoiceFreight = decimal.Parse(CommercialInvoiceFreightChargeAmount);
            }

            if (!string.IsNullOrEmpty(CommercialInvoiceTaxesAmount))
            {
                shipment.FedEx.CommercialInvoiceOther = decimal.Parse(CommercialInvoiceTaxesAmount);
            }

            // All test cases have sample as the purpose
            shipment.FedEx.CommercialInvoicePurpose = (int)FedExCommercialInvoicePurpose.Sample;
            shipment.FedEx.CommercialInvoiceReference = CommercialInvoiceCustomerReferenceValue;

            // Only non-empty item specifies terms of sale as EXW
            FedExTermsOfSale termsOfSale;
            if (FedExTermsOfSale.TryParse(CommercialInvoiceTermsOfSale, true, out termsOfSale))
            {
                shipment.FedEx.CommercialInvoiceTermsOfSale = (int)termsOfSale;
            }
            else
            {
                shipment.FedEx.CommercialInvoiceTermsOfSale = (int)FedExTermsOfSale.EXW;
            }

            if (!string.IsNullOrWhiteSpace(CustomsClearanceInsuranceAmount))
            {
                shipment.FedEx.CommercialInvoiceInsurance = decimal.Parse(CustomsClearanceInsuranceAmount);
            }
        }

        /// <summary>
        /// Applies the dangerous goods.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void ApplyDangerousGoods(ShipmentEntity shipment)
        {
            if (!string.IsNullOrWhiteSpace(PackageDangerousGoodsDetail) || !string.IsNullOrWhiteSpace(DangerousGoodsAccessibility))
            {
                foreach (FedExPackageEntity package in shipment.FedEx.Packages)
                {
                    package.DangerousGoodsType = (int)GetDangerousGoodsMaterialType();

                    if (package.DangerousGoodsType == (int)FedExDangerousGoodsMaterialType.HazardousMaterials || !string.IsNullOrEmpty(HazardProperShippingName))
                    {
                        package.HazardousMaterialProperName = HazardProperShippingName;
                        package.HazardousMaterialClass = HazardClass;
                        package.HazardousMaterialNumber = HazardDescriptionID;

                        package.HazardousMaterialPackingGroup = (int)FedExHazardousMaterialsPackingGroup.III;
                        package.HazardousMaterialQuantityValue = int.Parse(HazardQuantityAmount);

                        package.HazardousMaterialQuantityValue = GetUnitInt(HazardQuantityUnits);

                        package.DangerousGoodsEmergencyContactPhone = DangerEmergencyContactNumber;

                        package.DangerousGoodsOfferor = DangerOfferor;

                        package.DangerousGoodsPackagingCount = int.Parse(DangerCounts);
                    }
                }
            }
        }

        private FedExDangerousGoodsMaterialType GetDangerousGoodsMaterialType()
        {
            if (string.IsNullOrWhiteSpace(PackageDangerousGoodsDetail))
            {
                return FedExDangerousGoodsMaterialType.NotApplicable;
            }

            switch (PackageDangerousGoodsDetail.ToLower())
            {
                case ("hazardous_materials"): return FedExDangerousGoodsMaterialType.HazardousMaterials;
                case ("orm_d"): return FedExDangerousGoodsMaterialType.OrmD;
                case ("lithium_batteries"): return FedExDangerousGoodsMaterialType.Batteries;
                default: return FedExDangerousGoodsMaterialType.NotApplicable;
            }
        }

        /// <summary>
        /// Gets the unit int.
        /// </summary>
        /// <exception cref="System.ArgumentException"></exception>
        public int GetUnitInt(string unit)
        {
            int unitInt;

            switch (unit.ToLower())
            {
                case "kg":
                    unitInt = (int)FedExHazardousMaterialsQuantityUnits.Kilogram;
                    break;

                case "ml":
                    unitInt = (int)FedExHazardousMaterialsQuantityUnits.Milliliters;
                    break;

                case "l":
                    unitInt = (int)FedExHazardousMaterialsQuantityUnits.Liters;
                    break;
                    
                default:
                    throw new ArgumentException(string.Format("Invalid Unit {0}", unit), unit);
            }

            return unitInt;
        }
    }
}
