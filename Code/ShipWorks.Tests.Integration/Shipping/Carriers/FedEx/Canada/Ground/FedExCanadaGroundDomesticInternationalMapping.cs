using System.Collections.Generic;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Ground
{
    public class FedExCanadaGroundDomesticInternationalMapping
    {
        private static List<ColumnPropertyMapDefinition> columnPropertyMap = new List<ColumnPropertyMapDefinition>();

        public static List<ColumnPropertyMapDefinition> Mapping
        {
            get
            {
                if (columnPropertyMap == null || columnPropertyMap.Count == 0)
                {
                    columnPropertyMap = new List<ColumnPropertyMapDefinition>();
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.City", PropertyName = "CodCity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.CountryCode", PropertyName = "CodCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.PostalCode", PropertyName = "CodPostalCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.StateOrProvinceCode", PropertyName = "CodStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.StreetLines", PropertyName = "CodStreetLines", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Contact.CompanyName", PropertyName = "CodCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Contact.PhoneNumber", PropertyName = "CodPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.CustomsValue.Amount", PropertyName = "CommoditiesCustomsValueAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.CustomsValue.Currency", PropertyName = "CommoditiesCustomsValueCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.UnitPrice.Amount", PropertyName = "CommoditiesUnitPriceAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Commodities.UnitPrice.Currency", PropertyName = "CommoditiesUnitPriceCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.CountryOfManufacture", PropertyName = "CommoditiesCountryOfManufacture", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.Description", PropertyName = "CommoditiesDescription", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.NumberOfPieces", PropertyName = "CommoditiesNumberOfPieces", SpreadsheetColumnIndex = -1 });
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.Quantity", PropertyName = "CommoditiesQuantity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.QuantityUnits", PropertyName = "CommoditiesQuantityUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.UnitPrice.Amount", PropertyName = "CommoditiesUnitPriceAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.Weight.Units", PropertyName = "CommoditiesWeightUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.Commodities.Weight.Value", PropertyName = "CommoditiesWeightValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CustomsValue.Amount", PropertyName = "CustomsClearanceValueAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.CustomsValue.Currency", PropertyName = "CustomsClearanceValueCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.DocumentContent", PropertyName = "CustomsClearanceDocumentContent", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CustomsClearanceDetail.DutiesPayment.PaymentType", PropertyName = "DutiesPaymentType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "DutiesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = "DutiesAccountNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "DutiesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "DutiesCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "DutiesPayment.Payor.ResponsibleParty.Contact.PersonName", PropertyName = "DutiesPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RateRequestTypes", PropertyName = "RateRequestTypes", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.CompanyName", PropertyName = "RecipientCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.PersonName", PropertyName = "ShipperPersonName", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.SpecialServicesRequested.PackageCount", PropertyName = "PackageCount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackageCount", PropertyName = "PackageCount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.TransactionDetail.CustomerTransactionId", PropertyName = "CustomerTransactionId", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.City", PropertyName = "RecipientCity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.CountryCode", PropertyName = "RecipientCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.PostalCode", PropertyName = "RecipientPostalCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.StateOrProvinceCode", PropertyName = "RecipientStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.StreetLines", PropertyName = "RecipientStreetLines", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.PersonName", PropertyName = "RecipientPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.PhoneNumber", PropertyName = "RecipientPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.CustomerReferences.CustomerReferenceType", PropertyName = "CustomerReferenceType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.CustomerReferences.Value", PropertyName = "CustomerReferenceValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Height", PropertyName = "PackageLineItemHeight", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Length", PropertyName = "PackageLineItemLength", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Units", PropertyName = "PackageLineItemUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Width", PropertyName = "PackageLineItemWidth", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.InsuredValue.Amount", PropertyName = "PackageLineItemInsuredValueAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.InsuredValue.Currency", PropertyName = "PackageLineItemInsuredValueCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.CodDetail.CodCollectionAmount.Amount", PropertyName = "CodCollectionAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.CodDetail.CodCollectionAmount.Currency", PropertyName = "CodCollectionCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Contact.PersonName", PropertyName = "CodPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "PackageLineItemSpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Units", PropertyName = "PackageLineItemWeightUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Value", PropertyName = "PackageLineItemWeightValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.DropoffType", PropertyName = "DropoffType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.PackagingType", PropertyName = "PackagingType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.Recipient.Contact.CompanyName", PropertyName = "RecipientCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "PackageLineItemSpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ServiceType", PropertyName = "ShipmentServiceType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.Shipper.Contact.PersonName", PropertyName = "ShipperPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.PaymentType", PropertyName = "ResponsiblePartyPaymentType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Account number", PropertyName = "ResponsiblePartyAccountNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShipTimestamp", PropertyName = "ShipTimestamp", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested (Repetitions).SpecialServiceTypes", PropertyName = "SpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.TotalWeight.Value", PropertyName = "ShipmentTotalWeightValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.TotalWeight.Units", PropertyName = "ShipmentWeightUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SaveLabel", PropertyName = "SaveLabel", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.City", PropertyName = "ShipperCity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.CountryCode", PropertyName = "ShipperCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.PostalCode", PropertyName = "ShipperPostalCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StateOrProvinceCode", PropertyName = "ShipperStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StreetLines", PropertyName = "ShipperStreetLines", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.CompanyName", PropertyName = "ShipperCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.PhoneNumber", PropertyName = "ShipperPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Account number", PropertyName = "ResponsiblePartyAccountNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "ResponsiblePartyCountryCode", SpreadsheetColumnIndex = -1 });
                    
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.CodDetail.ChargeBasis", PropertyName = "CodChargeBasis", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.CodDetail.CollectionType", PropertyName = "CodDetailCollectionType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.SignatureOptionDetail.OptionType", PropertyName = "ShipmentSignatureOptionType", SpreadsheetColumnIndex = -1 });

                    // Fields added for 2014 certification
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.CodDetail.RateTypeBasis", PropertyName = "CodDetailRateTypeBasis", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.CodDetail.ChargeBasisLevel", PropertyName = "CodChargeBasisLevel", SpreadsheetColumnIndex = -1 });

                    // Columns that are not in the 2014 certification
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.Residential", PropertyName = "ShipperResidential", SpreadsheetColumnIndex = -1 });
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.Residential", PropertyName = "RecipientResidential", SpreadsheetColumnIndex = -1 });
                    // columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Contact.PersonName", PropertyName = "ResponsiblePartyPersonName", SpreadsheetColumnIndex = -1 });
                }

                return columnPropertyMap;
            }
            set
            {
                columnPropertyMap = value;
            }
        }
    }
}
