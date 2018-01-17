﻿using System.Collections.Generic;
using ShipWorks.Tests.Integration.MSTest;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.Canada.Express.International
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

                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Format", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Account Number", PropertyName = "BrokerAccountNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Address.City", PropertyName = "BrokerCity", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Address.CountryCode", PropertyName = "BrokerCountryCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Address.PostalCode", PropertyName = "BrokerPostalCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Address.StateOrProvinceCode", PropertyName = "BrokerStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Address.StreetLines", PropertyName = "BrokerStreetLines", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Contact.CompanyName", PropertyName = "BrokerCompanyName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Contact.ContactId", PropertyName = "BrokerContactId", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Contact.EMailAddress", PropertyName = "BrokerEMailAddress", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Contact.PersonName", PropertyName = "BrokerPersonName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Contact.PhoneExtension", PropertyName = "BrokerPhoneExtension", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Contact.PhoneNumber", PropertyName = "BrokerPhoneNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Contact.Title", PropertyName = "BrokerTitle", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Tins.Number", PropertyName = "BrokerTinsNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Broker.Tins.TinType", PropertyName = "BrokerTinType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Brokers.Type", PropertyName = "BrokerType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CommercialInvoice.Comments", PropertyName = "CommercialInvoiceComments", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CommercialInvoice.FreightCharge.Amount", PropertyName = "CommercialInvoiceFreightChargeAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CommercialInvoice.FreightCharge.Currency", PropertyName = "CommercialInvoiceFreightChargeCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CommercialInvoice.Purpose", PropertyName = "CommercialInvoicePurpose", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CommercialInvoice.TaxesOrMiscellaneousCharge.Amount", PropertyName = "CommercialInvoiceTaxesAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CommercialInvoice.TaxesOrMiscellaneousCharge.Currency", PropertyName = "CommercialInvoiceTaxesCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CommercialInvoice.TermsOfSale", PropertyName = "CommercialInvoiceTermsOfSale", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities..Weight.Units", PropertyName = "CommoditiesWeightUnits", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities..Weight.Value", PropertyName = "CommoditiesWeightValue", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.CountryOfManufacture", PropertyName = "CommoditiesCountryOfManufacture", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.CustomsValue.Amount", PropertyName = "CommoditiesCustomsValueAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.CustomsValue.Currency", PropertyName = "CommoditiesCustomsValueCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.Description", PropertyName = "CommoditiesDescription", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.HarmonizedCode", PropertyName = "CommoditiesHarmonizedCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.NumberOfPieces", PropertyName = "CommoditiesNumberOfPieces", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.Quantity", PropertyName = "CommoditiesQuantity", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.QuantityUnits", PropertyName = "CommoditiesQuantityUnits", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.UnitPrice.Amount", PropertyName = "CommoditiesUnitPriceAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.UnitPrice.Currency", PropertyName = "CommoditiesUnitPriceCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description", PropertyName = "CustomsOptionDescription", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type", PropertyName = "CustomsOptionType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsValue.Amount", PropertyName = "CustomsClearanceValueAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsValue.Currency", PropertyName = "CustomsClearanceValueCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.DocumentContent", PropertyName = "CustomsClearanceDocumentContent", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment.DutiesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = "DutiesAccountNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment.DutiesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "DutiesCountryCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment.DutiesPayment.Payor.ResponsibleParty.Contact.PersonName", PropertyName = "DutiesPersonName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment.PaymentType", PropertyName = "DutiesPaymentType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.B13AFilingOption", PropertyName = "ExportDetailB13AFilingOption", SpreadsheetColumnIndex = -1 });
//                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.PermitNumber", PropertyName = "InternationalControlledExportDetailLicenseOrPermitNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ExportDetail.ExportComplianceStatement", PropertyName = "ExportDetailExportComplianceStatement", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ImporterOfRecord.Contact.PersonName", PropertyName = "ImporterOfRecordPersonName", SpreadsheetColumnIndex = -1 });                    
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ImporterOfRecord .Contact.CompanyName", PropertyName = "ImporterOfRecordCompanyName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ImporterOfRecord .Contact.PhoneNumber", PropertyName = "ImporterOfRecordPhoneNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ImporterOfRecord.AccountNumber", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ImporterOfRecord.Address.City", PropertyName = "ImporterOfRecordCity", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ImporterOfRecord.Address.CountryCode", PropertyName = "ImporterOfRecordCountryCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ImporterOfRecord.Address.PostalCode", PropertyName = "ImporterOfRecordPostalCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ImporterOfRecord.Address.StateOrProvinceCode", PropertyName = "ImporterOfRecordStateProvinceCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ImporterOfRecord.Address.StreetLines", PropertyName = "ImporterOfRecordStreetLines", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ImporterOfRecord.Tins.Number", PropertyName = "BrokerTinsNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ImporterOfRecord.Tins.TinType", PropertyName = "BrokerTinType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.InsuranceCharges.Amount", PropertyName = "CustomsClearanceInsuranceAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.InsuranceCharges.Currency", PropertyName = "CustomsClearanceInsuranceCurrency", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.UnitPrice.CIMarksAndNumbers", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.DropoffType", PropertyName = "DropoffType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ExpressFreightDetail.BookingConfirmationNumber", PropertyName = "FreightDetailBookingConfirmationNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ExpressFreightDetail.ShippersLoadAndCount", PropertyName = "FreightDetailShippersLoadAndCount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.LabelSpecification.ImageType", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.LabelSpecification.LabelFormatType", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.LabelSpecification.LabelStockType", PropertyName = "LabelSpecificationLabelStockType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackagingType", PropertyName = "PackagingType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RateRequestTypes", PropertyName = "RateRequestTypes", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.City", PropertyName = "RecipientCity", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.CountryCode", PropertyName = "RecipientCountryCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.PostalCode", PropertyName = "RecipientPostalCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.StateOrProvinceCode", PropertyName = "RecipientStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.StreetLines", PropertyName = "RecipientStreetLines", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.CompanyName", PropertyName = "RecipientCompanyName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.PersonName", PropertyName = "RecipientPersonName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.PhoneNumber", PropertyName = "RecipientPhoneNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.CustomerReferences.CustomerReferenceType", PropertyName = "CustomerReferenceType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.CustomerReferences.Value", PropertyName = "CommercialInvoiceCustomerReferenceValue", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.Dimensions.Height", PropertyName = "PackageLineItemHeight", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.Dimensions.Length", PropertyName = "PackageLineItemLength", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.Dimensions.Units", PropertyName = "PackageLineItemDimensionUnits", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.Dimensions.Width", PropertyName = "PackageLineItemWidth", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.InsuredValue.Amount", PropertyName = "PackageLineItemInsuredValueAmount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.InsuredValue.Currency", PropertyName = "PackageLineItemInsuredValueCurrency", SpreadsheetColumnIndex = -1 });
//                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "PackageLineItemSpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.PackageCount", PropertyName = "PackageCount", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes.PriorityAlertDetail.Content", PropertyName = "PackageLineItemPriorityContent", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes.PriorityAlertDetail.EnhancementTypes", PropertyName = "PackageLineItemPriorityEnhancementType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.Weight.Units", PropertyName = "PackageLineItemWeightUnits", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.Weight.Value", PropertyName = "PackageLineItemWeightValue", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ServiceType", PropertyName = "ShipmentServiceType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.City", PropertyName = "ShipperCity", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.CountryCode", PropertyName = "ShipperCountryCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.PostalCode", PropertyName = "ShipperPostalCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.StateOrProvinceCode", PropertyName = "ShipperStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.StreetLines", PropertyName = "ShipperStreetLines", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.CompanyName", PropertyName = "ShipperCompanyName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.eMailAddress", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.PersonName", PropertyName = "ShipperPersonName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.PhoneNumber", PropertyName = "ShipperPhoneNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.PaymentType", PropertyName = "ResponsiblePartyPaymentType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = "ResponsiblePartyAccountNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "ResponsiblePartyCountryCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.Contact.PersonName", PropertyName = "ResponsiblePartyPersonName", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShipTimestamp", PropertyName = "ShipTimestamp", SpreadsheetColumnIndex = -1 });

                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "SpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested(Repititions).SpecialServiceTypes", PropertyName = "SpecialServiceType2", SpreadsheetColumnIndex = -1 });

                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested3.SpecialServiceTypes", PropertyName = "SpecialServiceType3", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested4.SpecialServiceTypes", PropertyName = "SpecialServiceType4", SpreadsheetColumnIndex = -1 });

                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.EventNotificationDetail.EventNotifications.NotificationDetail.Localization.LanguageCode", PropertyName = "EmailLanguageCode", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.EventNotificationDetail.EventNotifications.NotificationDetail.EmailDetail.EmailAddress", PropertyName = "EmailAddress", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.EMailNotificationDetail.Recipients.EMailNotificationRecipientType", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.EventNotificationDetail.EventNotifications.NotificationDetail.NotifyOnDelivery", PropertyName = "EmailNotifyOnDelivery", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.EMailNotificationDetail.Recipients.NotifyOnShipment", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.ShipmentDryIceDetail.TotalWeight.Units", PropertyName = "DryIceWeightUnits", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.ShipmentDryIceDetail.TotalWeight.Value", PropertyName = "DryIceWeightValue", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.InternationalTrafficInArmsRegulationsDetail..LicenseOrExemptionNumber", PropertyName = "TrafficInArmsLicenseOrExemptionNumber", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ReturnShipmentDetail.ReturnType", PropertyName = "ReturnType", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Totalweight.Units", PropertyName = "ShipmentWeightUnits", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Totalweight.Value", PropertyName = "ShipmentTotalWeightValue", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.TransactionDetail.CustomerTransactionId", PropertyName = "CustomerTransactionId", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SaveLabel", PropertyName = "SaveLabel", SpreadsheetColumnIndex = -1 });
                    caExpressInternationalMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Transaction Type", PropertyName = "", SpreadsheetColumnIndex = -1 });

                }
                return caExpressInternationalMapping;
            }
        }
    }
}