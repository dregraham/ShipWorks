using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.SmartPost
{
    public class FedExSmartPostFixture : FedExPrototypeFixture
    {
        private static List<ColumnPropertyMapDefinition> smartPostColumnPropertyMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExSmartPostFixture" /> class.
        /// </summary>
        public FedExSmartPostFixture()
            : base()
        {
            
        }

        public string ServiceCode { get; set; }
        public string SmartPostIndicia { get; set; }
        public string SmartPostAncillaryEndorsement { get; set; }
        public string SmartPostHubId { get; set; }
        public string SmartPostCustomerManifestId { get; set; }
        public string SmartPostTransactionType { get; set; }
        public string SmartPostLabelStockType { get; set; }

        /// <summary>
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public override ShipmentEntity CreateShipment()
        {
            CustomerReferenceType = "customer_reference";

            ShipmentEntity shipment = base.CreateShipment();

            shipment.FedEx.ReferenceCustomer = string.Empty;
            shipment.FedEx.ReferenceInvoice = string.Empty;

            // FedEx doesn't allow the ref field to be over 30 characters...truncate
            string referencePO = shipment.FedEx.ReferencePO;
            if (!string.IsNullOrWhiteSpace(referencePO) && referencePO.Length > 30)
            {
                shipment.FedEx.ReferencePO = shipment.FedEx.ReferencePO.Replace(" ", "").Substring(0, 30);
            }

            shipment.FedEx.Signature = (int)FedExSignatureType.ServiceDefault;

            shipment.FedEx.SmartPostCustomerManifest = SmartPostCustomerManifestId;
            shipment.FedEx.SmartPostHubID = SmartPostHubId;

            switch (SmartPostIndicia)
            {
                case "MEDIA_MAIL":
                    shipment.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.MediaMail;
                    break;
                case "PARCEL_SELECT":
                    shipment.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.ParcelSelect;
                    break;
                case "PRESORTED_STANDARD":
                    shipment.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.PresortedStandard;
                    break;
                case "PRESORTED_BOUND_PRINTED_MATTER":
                    shipment.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.BoundPrintedMatter;
                    break;
                case "PARCEL_RETURN":
                    shipment.FedEx.SmartPostIndicia = (int)FedExSmartPostIndicia.ParcelReturn;
                    break;
            }
            
            switch (SmartPostAncillaryEndorsement)
            {
                case "CARRIER_LEAVE_IF_NO_RESPONSE":
                    shipment.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.LeaveIfNoResponse;
                    break;
                case "RETURN_SERVICE":
                    shipment.FedEx.SmartPostEndorsement = (int)FedExSmartPostEndorsement.ReturnService;
                    break;
            }

            return shipment;
        }

        /// <summary>
        /// Gets the mapping for linking a spreadsheet column to a fixture property.
        /// </summary>
        public static List<ColumnPropertyMapDefinition> SmartPostMapping
        {
            get
            {
                if (smartPostColumnPropertyMap == null || !smartPostColumnPropertyMap.Any())
                {
                    smartPostColumnPropertyMap = new List<ColumnPropertyMapDefinition>();

                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.ClientDetail.AccountNumber", PropertyName = "FedExAccountNumber", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackageCount", PropertyName = "PackageCount", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShipTimestamp", PropertyName = "ShipTimestamp", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.SpecialServicesRequested.ReturnShipmentDetail.ReturnType", PropertyName = "ReturnType", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.CustomerReferences.Value", PropertyName = "ReturnRmaNumber", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.CustomerReferences.CustomerReferenceType", PropertyName = "ReturnRmaReason", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "SpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.TransactionDetail.CustomerTransactionId", PropertyName = "CustomerTransactionId", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address..CountryCode", PropertyName = "RecipientCountryCode", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address..Residential", PropertyName = "RecipientResidential", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.City", PropertyName = "RecipientCity", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.PostalCode", PropertyName = "RecipientPostalCode", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.StateOrProvinceCode", PropertyName = "RecipientStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.StreetLines", PropertyName = "RecipientStreetLines", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.PersonName", PropertyName = "RecipientPersonName", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.PhoneNumber", PropertyName = "RecipientPhoneNumber", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Units", PropertyName = "PackageLineItemWeightUnits", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Value", PropertyName = "PackageLineItemWeightValue", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Length", PropertyName = "PackageLineItemLength", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Width", PropertyName = "PackageLineItemWidth", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Height", PropertyName = "PackageLineItemHeight", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.DropoffType", PropertyName = "DropoffType", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.PackagingType", PropertyName = "PackagingType", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ServiceType", PropertyName = "ShipmentServiceType", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShippingChargesPayment.PaymentType", PropertyName = "ResponsiblePartyPaymentType", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SmartPostDetail.AncillaryEndorsement", PropertyName = "SmartPostAncillaryEndorsement", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SmartPostDetail.HubId", PropertyName = "SmartPostHubId", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SmartPostDetail.Indicia", PropertyName = "SmartPostIndicia", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SaveLabel", PropertyName = "SaveLabel", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Service Code", PropertyName = "ServiceCode", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.City", PropertyName = "ShipperCity", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.CountryCode", PropertyName = "ShipperCountryCode", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.PostalCode", PropertyName = "ShipperPostalCode", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.Residential", PropertyName = "ShipperResidential", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StateOrProvinceCode", PropertyName = "ShipperStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StreetLines", PropertyName = "ShipperStreetLines", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.CompanyName", PropertyName = "ShipperCompanyName", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.PersonName", PropertyName = "ShipperPersonName", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.PhoneNumber", PropertyName = "ShipperPhoneNumber", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.AccountNumber", PropertyName = "ResponsiblePartyAccountNumber", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.CountryCode", PropertyName = "ResponsiblePartyCountryCode", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.ReturnShipmentDetail.ReturnType", PropertyName = "ReturnType", SpreadsheetColumnIndex = -1 });
                    smartPostColumnPropertyMap.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Transaction Type", PropertyName = "SmartPostTransactionType", SpreadsheetColumnIndex = -1 });
                }

                return smartPostColumnPropertyMap;
            }
        }
    }
}
