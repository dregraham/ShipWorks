﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.FedEx
{
    public class FedExUSGroundAlcoholFixture : FedExPrototypeFixture
    {
        public string HomeDeliveryDate { get; set; }
        public string HomeDeliveryPhoneNumber { get; set; }
        public string HomeDeliveryPremiumType { get; set; }
        public string PackageLineItemDimensionUnits { get; set; }
        public string PackageSignatureOptionType { get; set; }

        /// <summary>
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public override ShipmentEntity CreateShipment()
        {
            ShipmentEntity shipment = base.CreateShipment();

            shipment.FedEx.ReferenceCustomer = string.Empty;
            shipment.FedEx.ReferenceInvoice = string.Empty;

            shipment.FedEx.Signature = GetSignatureType();

            ApplyHomeDelivery(shipment);

            shipment.FedEx.CodAddFreight = false;

            return shipment;
        }

        /// <summary>
        /// Applies the home delivery information.
        /// </summary>
        private void ApplyHomeDelivery(ShipmentEntity shipment)
        {
            if (!string.IsNullOrWhiteSpace(HomeDeliveryPremiumType))
            {
                shipment.FedEx.HomeDeliveryType = GetHomeDeliveryType();

                // Can't deliver on Sunday or Monday
                if (!string.IsNullOrWhiteSpace(HomeDeliveryDate))
                {
                    switch (DateTime.Today.DayOfWeek)
                    {
                        case DayOfWeek.Sunday:
                            shipment.FedEx.HomeDeliveryDate = DateTime.Today.AddDays(9);
                            break;
                        case DayOfWeek.Monday:
                            shipment.FedEx.HomeDeliveryDate = DateTime.Today.AddDays(8);
                            break;
                        default:
                            shipment.FedEx.HomeDeliveryDate = DateTime.Today.AddDays(7);
                            break;
                    }
                }

                if (!string.IsNullOrWhiteSpace(HomeDeliveryPhoneNumber))
                {
                    shipment.FedEx.HomeDeliveryPhone = HomeDeliveryPhoneNumber;
                }
            }
        }

        /// <summary>
        /// Gets the type of the home delivery.
        /// </summary>
        private int GetHomeDeliveryType()
        {
            int deliveryType;
            switch (HomeDeliveryPremiumType.ToUpperInvariant())
            {
                case "DATE_CERTAIN":
                    deliveryType = (int) FedExHomeDeliveryType.DateCertain;
                    break;

                case "EVENING":
                    deliveryType = (int) FedExHomeDeliveryType.Evening;
                    break;

                case "APPOINTMENT":
                    deliveryType = (int) FedExHomeDeliveryType.Appointment;
                    break;

                case "":
                    deliveryType = 0;
                    break;

                default:
                    deliveryType = (int) FedExHomeDeliveryType.None;
                    break;
            }

            return deliveryType;
        }

        /// <summary>
        /// Gets the type of the signature.
        /// </summary>
        /// <exception cref="System.IO.InvalidDataException"></exception>
        private int GetSignatureType()
        {
            int signatureType = 0;

            if (!string.IsNullOrEmpty(PackageSignatureOptionType))
            {
                switch (PackageSignatureOptionType.ToUpperInvariant())
                {
                    case "NO_SIGNATURE_REQUIRED":
                        signatureType = (int) FedExSignatureType.NoSignature;
                        break;

                    case "DIRECT":
                        signatureType = (int) FedExSignatureType.Direct;
                        break;

                    case "INDIRECT":
                        signatureType = (int) FedExSignatureType.Indirect;
                        break;

                    case "ADULT":
                        signatureType = (int) FedExSignatureType.Adult;
                        break;

                    case "":
                        signatureType = 0;
                        break;

                    default:
                        throw new InvalidDataException(string.Format("Invalid PackageSignatureOption {0}", PackageSignatureOptionType.ToUpperInvariant()));
                }
            }
            return signatureType;

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
                    unitInt = (int) FedExHazardousMaterialsQuantityUnits.Kilogram;
                    break;

                case "ml":
                    unitInt = (int) FedExHazardousMaterialsQuantityUnits.Milliliters;
                    break;

                default:
                    throw new ArgumentException(string.Format("Invalid Unit {0}", unit), unit);
            }

            return unitInt;
        }

        private static List<ColumnPropertyMapDefinition> mapping = new List<ColumnPropertyMapDefinition>();
        public static List<ColumnPropertyMapDefinition> Mapping
        {
            get
            {
                if (mapping == null || mapping.Count == 0)
                {
                    mapping = new List<ColumnPropertyMapDefinition>();
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.City", PropertyName = "CodCity", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.CountryCode", PropertyName = "CodCountryCode", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.PostalCode", PropertyName = "CodPostalCode", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.Residential", PropertyName = "CodResidential", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.StateOrProvinceCode", PropertyName = "CodStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Address.StreetLines", PropertyName = "CodStreetLines", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Contact.CompanyName", PropertyName = "CodCompanyName", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Contact.PersonName", PropertyName = "CodPersonName", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Contact.PhoneNumber", PropertyName = "CodPhoneNumber", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Tins.Number", PropertyName = "CodTinNumber", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodRecipient.Tins.TinType", PropertyName = "CodTinType", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackageCount", PropertyName = "PackageCount", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RateRequestTypes", PropertyName = "RateRequestTypes", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.Shipper.Contact.PersonName", PropertyName = "ShipperPersonName", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShipTimestamp", PropertyName = "ShipTimestamp", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.TransactionDetail.CustomerTransactionId", PropertyName = "CustomerTransactionId", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.City", PropertyName = "RecipientCity", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.CountryCode", PropertyName = "RecipientCountryCode", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.PostalCode.PostalCode", PropertyName = "RecipientPostalCode", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.Residential", PropertyName = "RecipientResidential", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.StateOrProvinceCode", PropertyName = "RecipientStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Address.StreetLines", PropertyName = "RecipientStreetLines", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.PersonName", PropertyName = "RecipientPersonName", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Recipient.Contact.PhoneNumber", PropertyName = "RecipientPhoneNumber", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.CustomerReferences.CustomerReferenceType", PropertyName = "CustomerReferenceType", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Height", PropertyName = "PackageLineItemHeight", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Length", PropertyName = "PackageLineItemLength", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Units", PropertyName = "PackageLineItemDimensionUnits", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Dimensions.Width", PropertyName = "PackageLineItemWidth", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.InsuredValue.Amount", PropertyName = "PackageLineItemInsuredValueAmount", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.InsuredValue.Currency", PropertyName = "PackageLineItemInsuredValueCurrency", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.CodDetail.CodCollectionAmount.Amount", PropertyName = "CodCollectionAmount", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Units", PropertyName = "PackageLineItemWeightUnits", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.Weight.Value", PropertyName = "PackageLineItemWeightValue", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.DropoffType", PropertyName = "DropoffType", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.PackagingType", PropertyName = "PackagingType", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.Recipient.Contact.CompanyName", PropertyName = "RecipientCompanyName", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ServiceType", PropertyName = "ShipmentServiceType", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShippingChargesPayment.PaymentType", PropertyName = "ResponsiblePartyPaymentType", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = "ResponsiblePartyAccountNumber", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType", PropertyName = "HomeDeliveryPremiumType", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "SpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.TotalWeight.Units", PropertyName = "ShipmentWeightUnits", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.TotalWeight.Value", PropertyName = "ShipmentTotalWeightValue", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SaveLabel", PropertyName = "SaveLabel", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.City", PropertyName = "ShipperCity", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.CountryCode", PropertyName = "ShipperCountryCode", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.PostalCode", PropertyName = "ShipperPostalCode", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.Residential", PropertyName = "ShipperResidential", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StateOrProvinceCode", PropertyName = "ShipperStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Address.StreetLines", PropertyName = "ShipperStreetLines", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Shipper.Contact.PhoneNumber", PropertyName = "ShipperPhoneNumber", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "ResponsiblePartyCountryCode", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ShippingChargesPayment.Payor.ResponsibleParty.Contact.PersonName", PropertyName = "ResponsiblePartyPersonName", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.HomeDeliveryPremiumDetail.Date", PropertyName = "HomeDeliveryDate", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SpecialServicesRequested.HomeDeliveryPremiumDetail.Phone Number", PropertyName = "HomeDeliveryPhoneNumber", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "PackageLineItemSpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedPackageLineItems.SpecialServicesRequested.SpecialServiceTypes2", PropertyName = "PackageLineItemSpecialServiceType2", SpreadsheetColumnIndex = -1 });
                    mapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "CodDetail.CollectionType", PropertyName = "CodDetailCollectionType", SpreadsheetColumnIndex = -1 });
                }

                return mapping;
            }
            set
            {
                mapping = value;
            }
        }
    }
}
