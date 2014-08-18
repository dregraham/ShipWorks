using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.Domestic
{
    public class FedExUSExpressDomesticAlcoholMapping
    {
        private static List<ColumnPropertyMapDefinition> columnPropertyMap;

        /// <summary>
        /// Gets the mapping for linking a spreadsheet column to a fixture property.
        /// </summary>
        public static List<ColumnPropertyMapDefinition> Mapping
        {
            get
            {
                if (columnPropertyMap == null || !columnPropertyMap.Any())
                {
                    columnPropertyMap = new List<ColumnPropertyMapDefinition>();

                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.City", PropertyName = "CodCity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.CountryCode", PropertyName = "CodCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.PostalCode", PropertyName = "CodPostalCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.StateOrProvinceCode", PropertyName = "CodStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.StreetLines", PropertyName = "CodStreetLines", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Contact.CompanyName", PropertyName = "CodCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Contact.PersonName", PropertyName = "CodPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Contact.PhoneNumber", PropertyName = "CodPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Contact.Title", PropertyName = "CodTitle", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Tins.Number", PropertyName = "CodTinNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Tins.TinType", PropertyName = "CodTinType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackageCount", PropertyName = "PackageCount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.PersonName", PropertyName = "ShipperPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.TransactionDetail.CustomerTransactionId", PropertyName = "CustomerTransactionId", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.City", PropertyName = "RecipientCity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.CountryCode", PropertyName = "RecipientCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.PostalCode", PropertyName = "RecipientPostalCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.Residential", PropertyName = "RecipientResidential", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.StateOrProvinceCode", PropertyName = "RecipientStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.StreetLines", PropertyName = "RecipientStreetLines", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.PersonName", PropertyName = "RecipientPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.PhoneNumber", PropertyName = "RecipientPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.CompanyName", PropertyName = "RecipientCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.CustomerReferences.CustomerReferenceType", PropertyName = "CustomerReferenceType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.CustomerReferences.Value", PropertyName = "CustomerReferenceValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Height", PropertyName = "PackageLineItemHeight", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Length", PropertyName = "PackageLineItemLength", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Units", PropertyName = "PackageLineItemUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Width", PropertyName = "PackageLineItemWidth", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.InsuredValue", PropertyName = "PackageLineItemInsuredValueAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.InsuredValue.Currency", PropertyName = "PackageLineItemInsuredValueCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "PackageLineItemSpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Units", PropertyName = "PackageLineItemWeightUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Value", PropertyName = "PackageLineItemWeightValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.DropoffType", PropertyName = "DropoffType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.PackagingType", PropertyName = "PackagingType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Requestedshipment.RateRequestTypes", PropertyName = "RateRequestTypes", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ServiceType", PropertyName = "ShipmentServiceType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShippingChargesPayment.PaymentType", PropertyName = "ResponsiblePartyPaymentType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = "ResponsiblePartyAccountNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.CodCollectionAmount.Currency", PropertyName = "CodCollectionCurrency", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.CodCollectionAmount.Amount", PropertyName = "CodCollectionAmount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.TotalWeight.Units", PropertyName = "ShipmentWeightUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.TotalWeight.Value", PropertyName = "ShipmentTotalWeightValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SaveLabel", PropertyName = "SaveLabel", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.City", PropertyName = "ShipperCity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.CountryCode", PropertyName = "ShipperCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.PostalCode", PropertyName = "ShipperPostalCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.Residential", PropertyName = "ShipperResidential", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StateOrProvinceCode", PropertyName = "ShipperStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StreetLines", PropertyName = "ShipperStreetLines", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.CompanyName", PropertyName = "ShipperCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.PhoneNumber", PropertyName = "ShipperPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "ResponsiblePartyCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Contact.PersonName", PropertyName = "ResponsiblePartyPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.CodDetail.CollectionType", PropertyName = "CodDetailCollectionType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.CodDetail.ReferenceIndicator", PropertyName = "CodReferenceIndicator", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "SpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested(Repetitions).SpecialServiceTypes", PropertyName = "SpecialServiceType2", SpreadsheetColumnIndex = -1 });
                    
                    // New fields for 2014
                    // This first map entry may need to be mapped to a new property on the US Express Domestic Alcohol fixture rather than re-using the ShipmentSignatureOptionType field of the FedExPrototypeFixture
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes.SignatureOptionDetail", PropertyName = "ShipmentSignatureOptionType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested(Repetitions).SpecialServiceTypes", PropertyName = "PackageLineItemSpecialServiceType2", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes.AlcoholDetail.RecipientType", PropertyName = "PackageLineItemAlcoholDetailRecipientType", SpreadsheetColumnIndex = -1 });
                }

                return columnPropertyMap;
            }
        }
    }
}
