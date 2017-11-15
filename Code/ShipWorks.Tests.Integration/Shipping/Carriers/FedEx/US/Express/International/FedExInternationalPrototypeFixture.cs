﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.Integration.Shipping.Carriers.FedEx.US.Express.International
{
    public class FedExInternationalPrototypeFixture : FedExPrototypeFixture
    {
        public string CommoditiesCustomsValueAmount { get; set; }
        public string CommoditiesQuantity { get; set; }
        public string CommoditiesDescription { get; set; }
        public string CommoditiesWeightValue { get; set; }
        public string CommoditiesHarmonizedCode { get; set; }
        public string CommoditiesCountryOfManufacture { get; set; }
        public string CommoditiesNumberOfPieces { get; set; }
        public string CommoditiesWeightUnits { get; set; }
        public string CommoditiesQuantityUnits { get; set; }
        public string CommoditiesUnitPriceCurrency { get; set; }
        public string CommoditiesUnitPriceAmount { get; set; }
        public string CommoditiesCustomsValueCurrency { get; set; }
        public string DutiesPaymentType { get; set; }
        public string DutiesAccountNumber { get; set; }
        public string DutiesPersonName { get; set; }
        public string DutiesCountryCode { get; set; }
        public string CustomsClearanceDocumentContent { get; set; }
        public string CustomsClearanceValueCurrency { get; set; }
        public string CustomsClearanceValueAmount { get; set; }
        public string ImporterOfRecordAccountNumber { get; set; }
        public string ImporterOfRecordTinType { get; set; }
        public string ImporterOfRecordTinNumber { get; set; }
        public string ImporterOfRecordCountryCode { get; set; }
        public string ImporterOfRecordPersonName { get; set; }
        public string ImporterOfRecordCompanyName { get; set; }
        public string ImporterOfRecordStreetLines { get; set; }
        public string ImporterOfRecordCity { get; set; }
        public string ImporterOfRecordStateProvinceCode { get; set; }
        public string ImporterOfRecordPostalCode { get; set; }
        public string ImporterOfRecordPhoneNumber { get; set; }
        public string CustomsOptionType { get; set; }
        public string CustomsOptionDescription { get; set; }
        public bool CommercialInvoiceFileElectronically { get; set; }


        public string SignatoryContactName { get; set; }
        public string SignatoryTitle { get; set; }
        public string SignatoryPlace { get; set; }

        public string ContainerType { get; set; }
        public string NumberOfContainers { get; set; }
        public string PackingDetailsCargoAircraftOnly { get; set; }
        public string PackingDetailsPackingInstructions { get; set; }

        public string BatteryMaterial { get; set; }
        public string BatteryPacking { get; set; }
        public string BatteryRegulatorySubType { get; set; }

        /// <summary>
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public override ShipmentEntity CreateShipment(OrderEntity order)
        {
            ShipmentEntity shipment = base.CreateShipment(order);

            SetupCustomsItems(shipment);
            SetupCustomsShipment(shipment);
            SetupDuties(shipment);
            SetupImporterOfRecord(shipment);

            return shipment;
        }

        /// <summary>
        /// Setups the customs shipment.
        /// </summary>
        private void SetupCustomsShipment(ShipmentEntity shipment)
        {
            if (String.Equals(CustomsClearanceDocumentContent,"documents_only",StringComparison.InvariantCultureIgnoreCase))
            {
                shipment.FedEx.CustomsDocumentsOnly = true;
            }

            shipment.FedEx.CustomsDocumentsDescription = CustomsClearanceDocumentContent;
            shipment.FedEx.CommercialInvoiceFileElectronically = CommercialInvoiceFileElectronically;

            if (!string.IsNullOrEmpty(CustomsClearanceValueAmount))
            {
                shipment.CustomsValue = decimal.Parse(CustomsClearanceValueAmount) * shipment.FedEx.Packages.Count; ;
            }

            AddCustomsOptions(shipment, CustomsOptionType, CustomsOptionDescription);
        }

        /// <summary>
        /// Parses the full name into a list of string containing at least two items (first and last name)
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <returns>The individual names.</returns>
        protected List<string> ParseName(string fullName)
        {
            List<string> names = fullName.Split(new char[] { ' ' }).ToList();

            if (names.Count == 0)
            {
                names.Add(fullName);
                names.Add(string.Empty);
            }

            while (names.Count < 2)
            {
                names.Add(string.Empty);
            }

            return names;
        }

        private void SetupImporterOfRecord(ShipmentEntity shipment)
        {
            if (!string.IsNullOrEmpty(ImporterOfRecordCountryCode))
            {
                shipment.FedEx.ImporterOfRecord = true;


                List<string> names = ParseName(ImporterOfRecordPersonName);
                shipment.FedEx.ImporterFirstName = names[0];
                shipment.FedEx.ImporterLastName = names[1];

                shipment.FedEx.ImporterCompany = ImporterOfRecordCompanyName;
                shipment.FedEx.ImporterStreet1 = ImporterOfRecordStreetLines;
                shipment.FedEx.ImporterCity = ImporterOfRecordCity;
                shipment.FedEx.ImporterStateProvCode = ImporterOfRecordStateProvinceCode;
                shipment.FedEx.ImporterPostalCode = ImporterOfRecordPostalCode;
                shipment.FedEx.ImporterCountryCode = ImporterOfRecordCountryCode;
                shipment.FedEx.ImporterPhone = ImporterOfRecordPhoneNumber;
            }
        }

        /// <summary>
        /// Setups the customs items.
        /// </summary>
        private void SetupCustomsItems(ShipmentEntity shipment)
        {
            if (!string.IsNullOrEmpty(CommoditiesQuantity) && CommoditiesQuantity != "0")
            {
                int numberOfItems = int.Parse(CommoditiesQuantity);

                // Add the customs items to the shipment
                ShipmentCustomsItemEntity customsItem;
                customsItem = new ShipmentCustomsItemEntity();
                customsItem.Weight = double.Parse(CommoditiesWeightValue);
                customsItem.HarmonizedCode = CommoditiesHarmonizedCode ?? string.Empty;
                customsItem.CountryOfOrigin = CommoditiesCountryOfManufacture;
                customsItem.Quantity = int.Parse(CommoditiesQuantity);
                customsItem.UnitValue = decimal.Parse(CommoditiesCustomsValueAmount);
                customsItem.UnitPriceAmount = decimal.Parse(CommoditiesUnitPriceAmount);
                customsItem.NumberOfPieces = int.Parse(CommoditiesNumberOfPieces);
                customsItem.Description = CommoditiesDescription;

                shipment.CustomsItems.Add(customsItem);
            }
        }

        /// <summary>
        /// Gets the currency.
        /// </summary>
        /// <returns>A CurrencyType value based on the CommodiditiesUnitPriceCurrency.</returns>
        private CurrencyType GetCurrencyType()
        {
            CurrencyType currencyType = CurrencyType.USD;

            if (CommoditiesUnitPriceCurrency == "CAD")
            {
                currencyType = CurrencyType.CAD;
            }

            return currencyType;
        }

        /// <summary>
        /// Setups the duties.
        /// </summary>
        private void SetupDuties(ShipmentEntity shipment)
        {
            if (!string.IsNullOrEmpty(DutiesPaymentType))
            {
                shipment.FedEx.PayorDutiesType = (int)GetPaymentType(DutiesPaymentType);
                shipment.FedEx.PayorTransportName = DutiesPersonName;
                shipment.FedEx.PayorDutiesCountryCode = DutiesCountryCode;

                string dutiesAccountNumber = DutiesAccountNumber.ToLower();
                if (dutiesAccountNumber == "please use shippers's account number" ||
                    dutiesAccountNumber == "use third party account number" ||
                    dutiesAccountNumber == "use recipient account number" ||
                    dutiesAccountNumber == "use sender account number" )
                {
                    shipment.FedEx.PayorDutiesAccount = "224813333";
                }
                else
                {
                    shipment.FedEx.PayorDutiesAccount = DutiesAccountNumber;
                }

                //if (shipment.FedEx.PayorDutiesType == (int) FedExPayorType.Recipient || shipment.FedEx.PayorDutiesType == (int) FedExPayorType.ThirdParty)
                //{
                //    shipment.FedEx.PayorDutiesName = DutiesPersonName;
                //}
            }
        }
    }
}
