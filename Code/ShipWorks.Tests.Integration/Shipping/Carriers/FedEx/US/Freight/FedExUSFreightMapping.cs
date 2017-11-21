using System.Collections.Generic;
using System.Linq;
using ShipWorks.Tests.Integration.MSTest;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US.Freight
{
    public class FedExFreightPostFixture : FedExPrototypeFixture
    {
        private static List<ColumnPropertyMapDefinition> columnPropertyMap;

        /// <summary>
        /// Gets the mapping for linking a spreadsheet column to a fixture property
        /// </summary>
        public static List<ColumnPropertyMapDefinition> Mapping
        {
            get
            {
                if (columnPropertyMap == null || !columnPropertyMap.Any())
                {
                    columnPropertyMap = new List<ColumnPropertyMapDefinition>();

                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.TransactionDetail.CustomerTransactionId", PropertyName = nameof(CustomerTransactionId), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShipTimestamp", PropertyName = nameof(ShipTimestamp), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.DropoffType", PropertyName = nameof(DropoffType), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ServiceType", PropertyName = nameof(ShipmentServiceType), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackagingType", PropertyName = nameof(PackagingType), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.TotalWeight.Units", PropertyName = nameof(ShipmentWeightUnits), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.TotalWeight.Value", PropertyName = nameof(ShipmentTotalWeightValue), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.TotalInsuredValue.Currency", PropertyName = nameof(PackageLineItemInsuredValueCurrency), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.TotalInsuredValue.Amount", PropertyName = nameof(PackageLineItemInsuredValueAmount), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.PersonName", PropertyName = nameof(ShipperPersonName), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.CompanyName", PropertyName = nameof(ShipperCompanyName), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.PhoneNumber", PropertyName = nameof(ShipperPhoneNumber), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.StreetLines", PropertyName = nameof(ShipperStreetLines), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.City", PropertyName = nameof(ShipperCity), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.StateOrProvinceCode", PropertyName = nameof(ShipperStateOrProvinceCode), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.PostalCode", PropertyName = nameof(ShipperPostalCode), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.CountryCode", PropertyName = nameof(ShipperCountryCode), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.CompanyName", PropertyName = nameof(RecipientCompanyName), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.PersonName", PropertyName = nameof(RecipientPersonName), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.PhoneNumber", PropertyName = nameof(RecipientPhoneNumber), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.StreetLines", PropertyName = nameof(RecipientStreetLines), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.City", PropertyName = nameof(RecipientCity), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.StateOrProvinceCode", PropertyName = nameof(RecipientStateOrProvinceCode), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.PostalCode", PropertyName = nameof(RecipientPostalCode), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.CountryCode", PropertyName = nameof(RecipientCountryCode), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.PaymentType", PropertyName = nameof(ResponsiblePartyPaymentType), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = nameof(ResponsiblePartyAccountNumber), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.PersonName", PropertyName = nameof(ResponsiblePartyPersonName), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.contact.CountryCode", PropertyName = nameof(ResponsiblePartyCountryCode), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightAccountNumber", PropertyName = nameof(FedExAccountNumber), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.Contact.PersonName", PropertyName = nameof(ShipperPersonName), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.Contact.CompanyName", PropertyName = nameof(ShipperCompanyName), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.StreetLines", PropertyName = nameof(ShipperStreetLines), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.Address.City", PropertyName = nameof(ShipperCity), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.Address.StateOrProvinceCode", PropertyName = nameof(ShipperStateOrProvinceCode), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.Address.PostalCode", PropertyName = nameof(ShipperPostalCode), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.Address.CountryCode", PropertyName = nameof(ShipperCountryCode), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.Role", PropertyName = nameof(FreightRole), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail..CollectTermsType", PropertyName = nameof(FreightCollectTermsType), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.DeclaredValuePerUnit.Currency", PropertyName = nameof(PackageLineItemInsuredValueCurrency), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.DeclaredValuePerUnit.Amount", PropertyName = nameof(PackageLineItemInsuredValueAmount), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail..TotalHandlingUnits", PropertyName = nameof(FreightTotalHandlingUnits), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Pieces", PropertyName = nameof(FreightPieces), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Weight.Units", PropertyName = nameof(FreightItemWeightUnits), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Weight.Value", PropertyName = nameof(FreightItemWeightValue), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Length", PropertyName = nameof(FreightItemLength), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Width", PropertyName = nameof(FreightItemWidth), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Height", PropertyName = nameof(FreightItemHeight), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Units", PropertyName = nameof(FreightItemDimensionUnits), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.FreightClass", PropertyName = nameof(FreightClass), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.FreightClass2", PropertyName = nameof(FreightClass2), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Packaging", PropertyName = nameof(FreightPackaging), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Packaging2", PropertyName = nameof(FreightPackaging2), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Pieces2", PropertyName = nameof(FreightPieces2), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Weight.Units2", PropertyName = nameof(FreightItemWeightUnits2), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Weight.Value2", PropertyName = nameof(FreightItemWeightValue2), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Length2", PropertyName = nameof(FreightItemLength2), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Width2", PropertyName = nameof(FreightItemWidth2), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Height2", PropertyName = nameof(FreightItemHeight2), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Units2", PropertyName = nameof(FreightItemDimensionUnits2), SpreadsheetColumnIndex = -1});
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.LabelSpecification.LabelFormatType", PropertyName = nameof(), SpreadsheetColumnIndex = -1});
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.LabelSpecification.ImageType", PropertyName = nameof(), SpreadsheetColumnIndex = -1});
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.LabelSpecification.LabelStockType", PropertyName = nameof(LabelSpecificationLabelStockType), SpreadsheetColumnIndex = -1});
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.LabelSpecification.LabelPrintingOrientation", PropertyName = nameof(), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.Requestedshipment.RateRequestTypes", PropertyName = nameof(RateRequestTypes), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackageCount", PropertyName = nameof(PackageCount), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.InsuredValue.Amount", PropertyName = nameof(PackageLineItemInsuredValueAmount), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.InsuredValue.Currency", PropertyName = nameof(PackageLineItemInsuredValueCurrency), SpreadsheetColumnIndex = -1});
               }

                return columnPropertyMap;
           }
       }
   }
}