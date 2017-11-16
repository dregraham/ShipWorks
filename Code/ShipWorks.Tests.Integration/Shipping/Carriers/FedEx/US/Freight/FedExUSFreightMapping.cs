using System.Collections.Generic;
using System.Linq;
using ShipWorks.Tests.Integration.MSTest;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US.Freight
{
    public class FedExFreightPostFixture : FedExPrototypeFixture
    {
        public string FreightRole;
        public string FreightCollectTermsType;
        public string FreightTotalHandlingUnits;
        public string FreightClass;
        public string FreightPackaging;
        public string FreightPieces;

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

                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.Role", PropertyName = nameof(FreightRole), SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail..CollectTermsType", PropertyName = "FreightCollectTermsType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail..TotalHandlingUnits", PropertyName = "FreightTotalHandlingUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.FreightClass", PropertyName = "FreightClass", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Pieces", PropertyName = "FreightPieces", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Packaging", PropertyName = "FreightPackaging", SpreadsheetColumnIndex = -1 });

                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.TransactionDetail.CustomerTransactionId", PropertyName = nameof(CustomerTransactionId), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShipTimestamp", PropertyName = nameof(ShipTimestamp), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.DropoffType", PropertyName = nameof(DropoffType), SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ServiceType", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackagingType", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.TotalWeight.Units", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.TotalWeight.Value", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.TotalInsuredValue.Currency", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.TotalInsuredValue.Amount", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.PersonName", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.CompanyName", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.PhoneNumber", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.StreetLines", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.City", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.StateOrProvinceCode", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.PostalCode", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.CountryCode", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.CompanyName", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.PersonName", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.PhoneNumber", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.StreetLines", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.City", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.StateOrProvinceCode", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.PostalCode", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.CountryCode", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.PaymentType", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.PersonName", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.contact.CountryCode", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightAccountNumber", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.Contact.PersonName", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.Contact.CompanyName", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.StreetLines", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.Address.City", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.Address.StateOrProvinceCode", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.Address.PostalCode", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.FedExFreightBillingContactAndAddress.Address.CountryCode", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.Role", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.CollectTermsType", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.DeclaredValuePerUnit.Currency", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.DeclaredValuePerUnit.Amount", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.TotalHandlingUnits", PropertyName = "", SpreadsheetColumnIndex = -1});

                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Weight.Units", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Weight.Value", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Length", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Width", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Height", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Units", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.FreightClass", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Packaging", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Pieces", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Weight.Units", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Weight.Value", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Length", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Width", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Height", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.FreightShipmentDetail.LineItems.Dimensions.Units", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.LabelSpecification.LabelFormatType", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.LabelSpecification.ImageType", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.LabelSpecification.LabelStockType", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.LabelSpecification.LabelPrintingOrientation", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.Requestedshipment.RateRequestTypes", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackageCount", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.InsuredValue.Amount", PropertyName = "", SpreadsheetColumnIndex = -1});
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition {SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.InsuredValue.Currency", PropertyName = "", SpreadsheetColumnIndex = -1});
                }

                return columnPropertyMap;
            }
        }
    }
}