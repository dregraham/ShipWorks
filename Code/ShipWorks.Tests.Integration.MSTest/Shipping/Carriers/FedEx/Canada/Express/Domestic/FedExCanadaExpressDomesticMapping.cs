using System.Collections.Generic;
using System.Linq;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.Canada.Express.Domestic
{
    public class FedExCanadaExpressDomesticMapping : FedExPrototypeFixture
    {
        private static List<ColumnPropertyMapDefinition> columnPropertyMap;

        public static List<ColumnPropertyMapDefinition> Mapping
        {
            get
            {
                if (columnPropertyMap == null || !columnPropertyMap.Any())
                {
                    columnPropertyMap = new List<ColumnPropertyMapDefinition>();

                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "EMailNotificationDetail.Localization.LanguageCode", PropertyName = "EmailLanguageCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "EMailNotificationDetail.Recipients.EMailAddress", PropertyName = "EmailAddress", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "EMailNotificationDetail.Recipients.NotifyOnDelivery", PropertyName = "EmailNotifyOnDelivery", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.LocationContactAndAddress.Contact.CompanyName", PropertyName = "HoldCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.LocationContactAndAddress.Contact.PersonName", PropertyName = "HoldPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.LocationContactAndAddress.Contact.PhoneNumber", PropertyName = "HoldContactPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "HoldAtLocationDetail.PhoneNumber", PropertyName = "HoldPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "LocationContactAndAddress.Address.City", PropertyName = "HoldCity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "LocationContactAndAddress.Address.CountryCode", PropertyName = "HoldCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "LocationContactAndAddress.Address.PostalCode", PropertyName = "HoldPostalCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "LocationContactAndAddress.Address.StateOrProvinceCode", PropertyName = "HoldStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackageCount", PropertyName = "PackageCount", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RateRequestTypes", PropertyName = "RateRequestTypes", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.CompanyName", PropertyName = "RecipientCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.PersonName", PropertyName = "ShipperPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShipTimestamp", PropertyName = "ShipTimestamp", SpreadsheetColumnIndex = -1 });
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
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.CargoAircraftOnly", PropertyName = "DangerousGoodsCargoAircraftOnly", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DryIceWeight.Units", PropertyName = "DryIceWeightUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.DryIceWeight.Value", PropertyName = "DryIceWeightValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "PackageLineItemSpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes.PriorityAlertDetail.Content", PropertyName = "PackageLineItemPriorityContent", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes.PriorityAlertDetail.EnhancementTypes", PropertyName = "PackageLineItemPriorityEnhancementType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Units", PropertyName = "PackageLineItemWeightUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Value", PropertyName = "PackageLineItemWeightValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.DropoffType", PropertyName = "DropoffType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.PackagingType", PropertyName = "PackagingType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ServiceType", PropertyName = "ShipmentServiceType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.PaymentType", PropertyName = "ResponsiblePartyPaymentType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = "ResponsiblePartyAccountNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested(Repititions).SpecialServiceTypes", PropertyName = "SpecialServiceType2", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "SpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes.ReturnShipmentDetail.ReturnType", PropertyName = "ReturnType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.TotalWeight.Units", PropertyName = "ShipmentWeightUnits", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.TotalWeight.Value", PropertyName = "ShipmentTotalWeightValue", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SaveLabel", PropertyName = "SaveLabel", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.City", PropertyName = "ShipperCity", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.CountryCode", PropertyName = "ShipperCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.PostalCode", PropertyName = "ShipperPostalCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StateOrProvinceCode", PropertyName = "ShipperStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StreetLines", PropertyName = "ShipperStreetLines", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.CompanyName", PropertyName = "ShipperCompanyName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.PhoneNumber", PropertyName = "ShipperPhoneNumber", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "ResponsiblePartyCountryCode", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Contact.PersonName", PropertyName = "ResponsiblePartyPersonName", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.DangerousGoodsDetail.Accessibility", PropertyName = "DangerousGoodsAccessibility", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Address.StreetLines", PropertyName = "HoldStreetLines", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.SignatureOptionDetail.OptionType", PropertyName = "ShipmentSignatureOptionType", SpreadsheetColumnIndex = -1 });
                    columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.SignatureOptionDetail.SignatureReleaseNumber", PropertyName = "ShipmentSignatureReleaseNumber", SpreadsheetColumnIndex = -1 });

                    // Columns no longer referenced in CA Exp Dom tab
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ExpressFreightDetail.BookingConfirmationNumber", PropertyName = "FreightDetailBookingConfirmationNumber", SpreadsheetColumnIndex = -1 });
                    //columnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ExpressFreightDetail.ShippersLoadAndCount", PropertyName = "FreightDetailShippersLoadAndCount", SpreadsheetColumnIndex = -1 });
                }

                return columnPropertyMap;
            }
        }
    }
}
