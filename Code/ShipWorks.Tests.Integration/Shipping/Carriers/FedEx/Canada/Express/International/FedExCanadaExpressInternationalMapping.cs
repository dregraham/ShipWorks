﻿using System.Collections.Generic;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Express.International
{
    public static class FedExCanadaExpressInternationalMapping
    {
        private static List<ColumnPropertyMapDefinition> caExpressInternationalMapping;

        public static List<ColumnPropertyMapDefinition> Mapping
        {
            get
            {
                if (caExpressInternationalMapping == null)
                {
                    caExpressInternationalMapping = new List<ColumnPropertyMapDefinition>();

                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Broker.Contact.PhoneNumber", PropertyName = "BrokerPhoneNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Address.City", PropertyName = "BrokerCity", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Address.CountryCode", PropertyName = "BrokerCountryCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Address.PostalCode", PropertyName = "BrokerPostalCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Address.StateOrProvinceCode", PropertyName = "BrokerStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.CompanyName", PropertyName = "BrokerCompanyName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.ContactId", PropertyName = "BrokerContactId", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.EMailAddress", PropertyName = "BrokerEMailAddress", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.PersonName", PropertyName = "BrokerPersonName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.PhoneExtension", PropertyName = "BrokerPhoneExtension", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.PhoneNumber", PropertyName = "BrokerPhoneNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Contact.Title", PropertyName = "BrokerTitle", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Tins.Number", PropertyName = "BrokerTinsNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Brokers.Broker.Tins.TinType", PropertyName = "BrokerTinType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CommercialInvoice.FreightCharge.Amount", PropertyName = "CommercialInvoiceFreightChargeAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CommercialInvoice.FreightCharge.Currency", PropertyName = "CommercialInvoiceFreightChargeCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CommercialInvoice.TaxesOrMiscellaneousCharge.Amount", PropertyName = "CommercialInvoiceTaxesAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CommercialInvoice.TaxesOrMiscellaneousCharge.Currency", PropertyName = "CommercialInvoiceTaxesCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.CustomsValue.Amount", PropertyName = "CommoditiesCustomsValueAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.CustomsValue.Currency", PropertyName = "CommoditiesCustomsValueCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.UnitPrice.Amount", PropertyName = "CommoditiesUnitPriceAmount", SpreadsheetColumnIndex = -1 });
                    //not used in spreadsheet: caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.UnitPrice.CIMarksAndNumbers", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.UnitPrice.Currency", PropertyName = "CommoditiesUnitPriceCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Brokers..Type", PropertyName = "BrokerType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Brokers.Broker.Account Number", PropertyName = "BrokerAccountNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Brokers.Broker.Address.StreetLines", PropertyName = "BrokerStreetLines", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CommercialInvoice.Comments", PropertyName = "CommercialInvoiceComments", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CommercialInvoice.CustomerInvoiceNumber", PropertyName = "CustomerReferenceValue", SpreadsheetColumnIndex = -1 });
                    // this has the same value as PurposeOfShipmentDescription: caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CommercialInvoice.Purpose", PropertyName = "CommercialInvoicePurpose", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CommercialInvoice.PurposeOfShipmentDescription", PropertyName = "CommercialInvoicePurpose", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CommercialInvoice.TermsOfSale", PropertyName = "CommercialInvoiceTermsOfSale", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.CountryOfManufacture", PropertyName = "CommoditiesCountryOfManufacture", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.Description", PropertyName = "CommoditiesDescription", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.HarmonizedCode", PropertyName = "CommoditiesHarmonizedCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.NumberOfPieces", PropertyName = "CommoditiesNumberOfPieces", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.Quantity", PropertyName = "CommoditiesQuantity", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.QuantityUnits", PropertyName = "CommoditiesQuantityUnits", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.Weight.Units", PropertyName = "CommoditiesWeightUnits", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.Weight.Value", PropertyName = "CommoditiesWeightValue", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CustomsOptions.Description", PropertyName = "CustomsOptionDescription", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CustomsOptions.Type", PropertyName = "CustomsOptionType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CustomsValue.Amount", PropertyName = "CustomsClearanceValueAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CustomsValue.Currency", PropertyName = "CustomsClearanceValueCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.DocumentContent", PropertyName = "CustomsClearanceDocumentContent", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.DutiesPayment..PaymentType", PropertyName = "DutiesPaymentType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.ExportDetail.B13AFilingOption", PropertyName = "ExportDetailB13AFilingOption", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.ExportDetail.ExportComplianceStatement", PropertyName = "ExportDetailExportComplianceStatement", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.ExportDetail.PermitNumber", PropertyName = "InternationalControlledExportDetailLicenseOrPermitNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.InsuranceCharges.Amount", PropertyName = "CustomsClearanceInsuranceAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.InsuranceCharges.Currency", PropertyName = "CustomsClearanceInsuranceCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "DutiesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = "DutiesAccountNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "DutiesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "DutiesCountryCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "DutiesPayment.Payor.ResponsibleParty.Contact.PersonName", PropertyName = "DutiesPersonName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "EMailNotificationDetail.Localization.LanguageCode", PropertyName = "EmailLanguageCode", SpreadsheetColumnIndex = -1 });
                    //not used in spreadsheet: caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "EMailNotificationDetail.Recipients.EMailNotificationRecipientType", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "EMailNotificationDetail.Recipients.NotifyOnDelivery", PropertyName = "EmailNotifyOnDelivery", SpreadsheetColumnIndex = -1 });
                    //not used in spreadsheet: caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "EMailNotificationDetail.Recipients.NotifyOnShipment", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Format", PropertyName = "EmailFormat", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "FreightCharge.CustomerReferenceType", PropertyName = "CustomerReferenceType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ImporterOfRecord.Address.City", PropertyName = "ImporterOfRecordCity", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ImporterOfRecord.Address.CountryCode", PropertyName = "ImporterOfRecordCountryCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ImporterOfRecord.Address.PostalCode", PropertyName = "ImporterOfRecordPostalCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ImporterOfRecord.Address.StateOrProvinceCode", PropertyName = "ImporterOfRecordStateProvinceCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ImporterOfRecord.Address.StreetLines", PropertyName = "ImporterOfRecordStreetLines", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ImporterOfRecord.Contact.CompanyName", PropertyName = "ImporterOfRecordCompanyName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ImporterOfRecord.Contact.PersonName", PropertyName = "ImporterOfRecordPersonName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ImporterOfRecord.Contact.PhoneNumber", PropertyName = "ImporterOfRecordPhoneNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.ExpressFreightDetail.BookingConfirmationNumber", PropertyName = "FreightDetailBookingConfirmationNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.ExpressFreightDetail.ShippersLoadAndCount", PropertyName = "FreightDetailShippersLoadAndCount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RateRequestTypes", PropertyName = "RateRequestTypes", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.SpecialServicesRequested.PackageCount", PropertyName = "PackageCount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.TransactionDetail.CustomerTransactionId", PropertyName = "CustomerTransactionId", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.City", PropertyName = "RecipientCity", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.CountryCode", PropertyName = "RecipientCountryCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.PostalCode", PropertyName = "RecipientPostalCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.StateOrProvinceCode", PropertyName = "RecipientStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.StreetLines", PropertyName = "RecipientStreetLines", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.eMailAddress", PropertyName = "EmailAddress", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.PersonName", PropertyName = "RecipientPersonName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.PhoneNumber", PropertyName = "RecipientPhoneNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.CustomerReference.Value", PropertyName = "CommercialInvoiceCustomerReferenceValue", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Height", PropertyName = "PackageLineItemHeight", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Length", PropertyName = "PackageLineItemLength", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Units", PropertyName = "PackageLineItemDimensionUnits", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Width", PropertyName = "PackageLineItemWidth", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.InsuredValue.Amount", PropertyName = "PackageLineItemInsuredValueAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.InsuredValue.Currency", PropertyName = "PackageLineItemInsuredValueCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.ShipmentDryIceDetail.TotalWeight.Units", PropertyName = "DryIceWeightUnits", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.ShipmentDryIceDetail.TotalWeight.Value", PropertyName = "DryIceWeightValue", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "PackageLineItemSpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes.PriorityAlertDetail.Content", PropertyName = "PackageLineItemPriorityContent", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes.PriorityAlertDetail.EnhancementTypes", PropertyName = "PackageLineItemPriorityEnhancementType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Units", PropertyName = "PackageLineItemWeightUnits", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Value", PropertyName = "PackageLineItemWeightValue", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.DropoffType", PropertyName = "DropoffType", SpreadsheetColumnIndex = -1 });
                    //not used in spreadsheet: caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.LabelSpecification.ImageType", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    //not used in spreadsheet: caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.LabelSpecification.LabelFormatType", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.LabelSpecification.LabelStockType", PropertyName = "LabelSpecificationLabelStockType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.PackagingType", PropertyName = "PackagingType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.Recipient.Contact.CompanyName", PropertyName = "RecipientCompanyName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ServiceType", PropertyName = "ShipmentServiceType", SpreadsheetColumnIndex = -1 });
                    //not used in spreadsheet: caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.Shipper.Contact.eMailAddress", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.Shipper.Contact.PersonName", PropertyName = "ShipperPersonName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShippingChargesPayment.PaymentType", PropertyName = "ResponsiblePartyPaymentType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = "ResponsiblePartyAccountNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShipTimestamp", PropertyName = "ShipTimestamp", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested(Repititions).SpecialServiceTypes", PropertyName = "SpecialServiceType2", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "SpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.InternationalTrafficInArmsRegulationsDetail.LicenseOrExemptionNumber", PropertyName = "TrafficInArmsLicenseOrExemptionNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ReturnShipmentDetail.ReturnType", PropertyName = "ReturnType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.TotalWeight.Units", PropertyName = "ShipmentWeightUnits", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.TotalWeight.Value", PropertyName = "ShipmentTotalWeightValue", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SaveLabel", PropertyName = "SaveLabel", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.City", PropertyName = "ShipperCity", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.CountryCode", PropertyName = "ShipperCountryCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.PostalCode", PropertyName = "ShipperPostalCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StateOrProvinceCode", PropertyName = "ShipperStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StreetLines", PropertyName = "ShipperStreetLines", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.CompanyName", PropertyName = "ShipperCompanyName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.PhoneNumber", PropertyName = "ShipperPhoneNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "ResponsiblePartyCountryCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Contact.PersonName", PropertyName = "ResponsiblePartyPersonName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Transaction Type", PropertyName = "", SpreadsheetColumnIndex = -1 });

                }
                return caExpressInternationalMapping;
            }
        }
    }
}