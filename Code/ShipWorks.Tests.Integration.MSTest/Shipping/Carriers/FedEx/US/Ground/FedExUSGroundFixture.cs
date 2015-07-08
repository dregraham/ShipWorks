using System;
using System.Collections.Generic;
using System.IO;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Express.International;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.FedEx.US.Ground
{
    public class FedExUSGroundFixture : FedExInternationalPrototypeFixture
    {
        public string CustomsRegulatoryControls { get; set; }
        public string DangerCounts { get; set; }
        public string DangerEmergencyContactNumber { get; set; }
        public string DangerOfferor { get; set; }
        public string DangerUnits { get; set; }
        public string EMailNotificationRecipientType { get; set; } // varies between recipient, shipper, broker, and other
        public string HazardClass { get; set; }
        public string HazardDescriptionID { get; set; }
        public string HazardousPackingGroup { get; set; }
        public string HazardProperShippingName { get; set; }
        public string HazardQuantityAmount { get; set; }
        public string HazardQuantityUnits { get; set; }
        public string HomeDeliveryDate { get; set; }
        public string HomeDeliveryPhoneNumber { get; set; }
        public string HomeDeliveryPremiumType { get; set; }
        public string NaftaCostDateBegins { get; set; }  // always blank
        public string NaftaCostDateEnds { get; set; }  // always blank
        public string NaftaNetCostMethod { get; set; }
        public string NaftaPreferenceCriterion { get; set; }
        public string NaftaProducerDetermination { get; set; }
        public string NaftaProducerId { get; set; }
        public string NotifyEMailAddress { get; set; } //always abc@xyx
        public string NotifyEmailFormat { get; set; } // always html
        public string NotifyEmailLanguageCode { get; set; }  // always en
        public string NotifyOnDelivery { get; set; } // always false
        public string PackageDangerousGoodsDetail { get; set; }
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
            shipment.FedEx.ReferenceShipmentIntegrity = string.Empty;

            ApplyDangerousGoods(shipment);

            shipment.FedEx.Signature = GetSignatureType();

            ApplyHomeDelivery(shipment);
            ApplyDangerousGoods(shipment);

            ApplyNafta(shipment);

            shipment.FedEx.CodAddFreight = false;

            return shipment;
        }

        /// <summary>
        /// Applies the nafta.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <exception cref="System.IO.InvalidDataException"></exception>
        private void ApplyNafta(ShipmentEntity shipment)
        {
            if (!string.IsNullOrWhiteSpace(NaftaPreferenceCriterion))
            {
                shipment.FedEx.CustomsNaftaEnabled = true;

                FedExNaftaPreferenceCriteria preference;
                if (!Enum.TryParse(NaftaPreferenceCriterion, true, out preference))
                {
                    throw new InvalidDataException(string.Format("NaftaPreferenceCriterion is invalid {0}", NaftaPreferenceCriterion));
                }
                
                shipment.FedEx.CustomsNaftaPreferenceType = (int) preference;

                shipment.FedEx.CustomsNaftaDeterminationCode = GetNaftaDeterminationCode();

                shipment.FedEx.CustomsNaftaProducerId = NaftaProducerId;

                shipment.FedEx.CustomsNaftaNetCostMethod = GetNaftaNetCostMethod();
            }
        }

        /// <summary>
        /// Gets the nafta net cost method.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.IO.InvalidDataException"></exception>
        private int GetNaftaNetCostMethod()
        {
            int netCostMethod;

            switch (NaftaNetCostMethod.ToUpperInvariant())
            {
                case "NC":
                    netCostMethod= (int) FedExNaftaNetCostMethod.NetCostMethod;
                    break;

                case "NO":
                    netCostMethod=(int) FedExNaftaNetCostMethod.NotCalculated;
                    break;

                default:
                    throw new InvalidDataException(string.Format("NaftaNetCostMethod is invalid {0}", NaftaNetCostMethod));
            }

            return netCostMethod;
        }

        /// <summary>
        /// Gets the nafta determination code.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.IO.InvalidDataException"></exception>
        private int GetNaftaDeterminationCode()
        {
            int determinationCode;

            switch (NaftaProducerDetermination.ToUpperInvariant())
            {
                case "YES":
                    determinationCode = (int) FedExNaftaDeterminationCode.ProducerOfCommodity;
                    break;

                case "NO_1":
                    determinationCode = (int) FedExNaftaDeterminationCode.NotProducerKnowledgeOfCommodity;
                    break;

                case "NO_2":
                    determinationCode = (int) FedExNaftaDeterminationCode.NotProducerWrittenStatement;
                    break;

                case "NO_3":
                    determinationCode = (int) FedExNaftaDeterminationCode.NotProducerSignedCertificate;
                    break;

                case "":
                    determinationCode = 0;
                    break;

                default:
                    throw new InvalidDataException(string.Format("Invalid NaftaProducerDetermination {0}", NaftaProducerDetermination));
            }

            return determinationCode;
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

                    case "SERVICE_DEFAULT":
                        signatureType = (int)FedExSignatureType.ServiceDefault;
                        break;

                    case "":
                        signatureType = (int)FedExSignatureType.ServiceDefault;
                        break;

                    default:
                        throw new InvalidDataException(string.Format("Invalid PackageSignatureOption {0}", PackageSignatureOptionType.ToUpperInvariant()));
                }
            }
            return signatureType;

        }

        /// <summary>
        /// Applies the dangerous goods.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        private void ApplyDangerousGoods(ShipmentEntity shipment)
        {
            if (!string.IsNullOrWhiteSpace(PackageDangerousGoodsDetail))
            {
                foreach (FedExPackageEntity package in shipment.FedEx.Packages)
                {
                    package.DangerousGoodsType = (int) GetDangerousGoodsMaterialType();

                    if (package.DangerousGoodsType == (int) FedExDangerousGoodsMaterialType.HazardousMaterials)
                    {
                        package.HazardousMaterialProperName = HazardProperShippingName;
                        package.HazardousMaterialClass = HazardClass;
                        package.HazardousMaterialNumber = HazardDescriptionID;

                        package.HazardousMaterialPackingGroup = (int) FedExHazardousMaterialsPackingGroup.III;
                        package.HazardousMaterialQuantityValue = int.Parse(HazardQuantityAmount);

                        package.HazardousMaterialQuantityValue = GetUnitInt(HazardQuantityUnits);

                        package.DangerousGoodsEmergencyContactPhone = DangerEmergencyContactNumber;

                        package.DangerousGoodsOfferor = DangerOfferor;

                        package.DangerousGoodsPackagingCount = int.Parse(DangerCounts);
                    }
                }
            }
        }

        private FedExDangerousGoodsMaterialType GetDangerousGoodsMaterialType()
        {
            switch (PackageDangerousGoodsDetail.ToLower())
            {
                case ("hazardous_materials"): return FedExDangerousGoodsMaterialType.HazardousMaterials;
                case ("orm_d"): return FedExDangerousGoodsMaterialType.OrmD;
                case ("battery"): return FedExDangerousGoodsMaterialType.Batteries;
                default: return FedExDangerousGoodsMaterialType.NotApplicable;                    
            }
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

        private static List<ColumnPropertyMapDefinition> usGroundDomesticMapping = new List<ColumnPropertyMapDefinition>();
        public static List<ColumnPropertyMapDefinition> UsGroundDomesticMapping
        {
            get
            {
                if (usGroundDomesticMapping == null || usGroundDomesticMapping.Count == 0)
                {
                    usGroundDomesticMapping = new List<ColumnPropertyMapDefinition>();
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Description", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "GROUND.Service Code #", PropertyName = "CustomerTransactionId", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.CountryOfManufacture", PropertyName = "CommoditiesCountryOfManufacture", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.CustomsValue.Amount", PropertyName = "CommoditiesCustomsValueAmount", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.CustomsValue.Currency", PropertyName = "CommoditiesCustomsValueCurrency", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.Description", PropertyName = "CommoditiesDescription", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.HarmonizedCode", PropertyName = "CommoditiesHarmonizedCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.NaftaDetail.NetCostDateRange.Begins", PropertyName = "NaftaCostDateBegins", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.NaftaDetail.NetCostDateRange.Ends", PropertyName = "NaftaCostDateEnds", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.NaftaDetail.NetCostMethod", PropertyName = "NaftaNetCostMethod", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.NaftaDetail.PreferenceCriterion", PropertyName = "NaftaPreferenceCriterion", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.NaftaDetail.ProducerDetermination", PropertyName = "NaftaProducerDetermination", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.NaftaDetail.ProducerId", PropertyName = "NaftaProducerId", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.NumberOfPieces", PropertyName = "CommoditiesNumberOfPieces", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.UnitPrice.Amount", PropertyName = "CommoditiesUnitPriceAmount", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.UnitPrice.Currency", PropertyName = "CommoditiesUnitPriceCurrency", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.Weight.Quantity", PropertyName = "CommoditiesQuantity", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.Weight.QuantityUnits", PropertyName = "CommoditiesQuantityUnits", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.Weight.Units", PropertyName = "CommoditiesWeightUnits", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.Commodities.Weight.Value", PropertyName = "CommoditiesWeightValue", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsValue.Amount", PropertyName = "CustomsClearanceValueAmount", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsValue.Currency", PropertyName = "CustomsClearanceValueCurrency", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.DocumentContent", PropertyName = "CustomsClearanceDocumentContent", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment.PaymentType", PropertyName = "DutiesPaymentType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = "DutiesAccountNumber", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "DutiesCountryCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.DutiesPayment.Payor.ResponsibleParty.Contact.PersonName", PropertyName = "DutiesPersonName", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.RegulatoryControls", PropertyName = "CustomsRegulatoryControls", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Description", PropertyName = "CustomsOptionDescription", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.CustomsClearanceDetail.CustomsOptions.Type", PropertyName = "CustomsOptionType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.DropoffType", PropertyName = "DropoffType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.LabelSpecification.LabelFormatType", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackageCount", PropertyName = "PackageCount", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.PackagingType", PropertyName = "PackagingType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RateRequestTypes", PropertyName = "RateRequestTypes", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.City", PropertyName = "RecipientCity", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.CountryCode", PropertyName = "RecipientCountryCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.PostalCode", PropertyName = "RecipientPostalCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.Residential", PropertyName = "RecipientResidential", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.StateOrProvinceCode", PropertyName = "RecipientStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Address.StreetLines", PropertyName = "RecipientStreetLines", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.CompanyName", PropertyName = "RecipientCompanyName", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.PersonName", PropertyName = "RecipientPersonName", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Recipient.Contact.PhoneNumber", PropertyName = "RecipientPhoneNumber", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.CustomerReferences.CustomerReferenceType", PropertyName = "CustomerReferenceType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.CustomerReferences.Value", PropertyName = "CustomerReferenceValue", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.Dimensions.Height", PropertyName = "PackageLineItemHeight", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.Dimensions.Length", PropertyName = "PackageLineItemLength", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.Dimensions.Units", PropertyName = "PackageLineItemDimensionUnits", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.Dimensions.Width", PropertyName = "PackageLineItemWidth", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.InsuredValue.Amount", PropertyName = "PackageLineItemInsuredValueAmount", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.InsuredValue.Currency", PropertyName = "PackageLineItemInsuredValueCurrency", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested (Repetitions).SpecialServiceTypes", PropertyName = "PackageLineItemSpecialServiceType2", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.CodDetail.AddTransportationCharges", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.CodDetail.CodCollectionAmount.Amount", PropertyName = "CodCollectionAmount", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.CodDetail.CodCollectionAmount.Currency", PropertyName = "CodCollectionCurrency", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.CodDetail.CollectionType", PropertyName = "CodDetailCollectionType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Accessibility", PropertyName = "DangerousGoodsAccessibility", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.CargoAircraftOnly", PropertyName = "DangerousGoodsCargoAircraftOnly", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Description.HazardClass", PropertyName = "HazardClass", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Description.Id", PropertyName = "HazardDescriptionID", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Description.PackingGroup", PropertyName = "HazardousPackingGroup", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Description.ProperShippingName", PropertyName = "HazardProperShippingName", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.LabelType", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Quantity.Amount", PropertyName = "HazardQuantityAmount", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Containers.HazardousCommodities.Quantity.Units", PropertyName = "HazardQuantityUnits", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.EmergencyContactNumber", PropertyName = "DangerEmergencyContactNumber", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Offeror", PropertyName = "DangerOfferor", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Options", PropertyName = "PackageDangerousGoodsDetail", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Packaging.Count", PropertyName = "DangerCounts", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.DangerousGoodsDetail.Packaging.Units", PropertyName = "DangerUnits", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.SpecialServicesRequested.SignatureOptionDetail.OptionType", PropertyName = "PackageSignatureOptionType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.Weight.Units", PropertyName = "PackageLineItemWeightUnits", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.RequestedPackageLineItems.Weight.Value", PropertyName = "PackageLineItemWeightValue", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ServiceType", PropertyName = "ShipmentServiceType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.City", PropertyName = "ShipperCity", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.CountryCode", PropertyName = "ShipperCountryCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.PostalCode", PropertyName = "ShipperPostalCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.Residential", PropertyName = "ShipperResidential", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.StateOrProvinceCode", PropertyName = "ShipperStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Address.StreetLines", PropertyName = "ShipperStreetLines", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.CompanyName", PropertyName = "ShipperCompanyName", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.PersonName", PropertyName = "ShipperPersonName", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.Shipper.Contact.PhoneNumber", PropertyName = "ShipperPhoneNumber", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.PaymentType", PropertyName = "ResponsiblePartyPaymentType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.AccountNumber", PropertyName = "ResponsiblePartyAccountNumber", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.Address.CountryCode", PropertyName = "ResponsiblePartyCountryCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShippingChargesPayment.Payor.ResponsibleParty.contact.PersonName", PropertyName = "ResponsiblePartyPersonName", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.ShipTimestamp", PropertyName = "ShipTimestamp", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested(Repetitions).SpecialServiceTypes", PropertyName = "SpecialServiceType2", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.CodDetail.CodRecipient.Address.City", PropertyName = "CodCity", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.CodDetail.CodRecipient.Address.CountryCode", PropertyName = "CodCountryCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.CodDetail.CodRecipient.Address.PostalCode", PropertyName = "CodPostalCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.CodDetail.CodRecipient.Address.Residential", PropertyName = "CodResidential", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.CodDetail.CodRecipient.Address.StateOrProvinceCode", PropertyName = "CodStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.CodDetail.CodRecipient.Address.StreetLines", PropertyName = "CodStreetLines", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.CodDetail.CodRecipient.Contact.CompanyName", PropertyName = "CodCompanyName", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.CodDetail.CodRecipient.Contact.PersonName", PropertyName = "CodPersonName", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.CodDetail.CodRecipient.Contact.PhoneNumber", PropertyName = "CodPhoneNumber", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.CodDetail.CodRecipient.Tins.Number", PropertyName = "CodTinNumber", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.CodDetail.CodRecipient.Tins.TinType", PropertyName = "CodTinType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.CodDetail.ReferenceIndicator", PropertyName = "CodReferenceIndicator", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.EMailNotificationDetail.Recipients.EMailAddress", PropertyName = "EmailAddress", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.EMailNotificationDetail.Recipients.EMailNotificationRecipientType", PropertyName = "EMailNotificationRecipientType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.EMailNotificationDetail.Recipients.Format", PropertyName = "EmailFormat", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.EMailNotificationDetail.Recipients.LanguageCode", PropertyName = "EmailLanguageCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.EMailNotificationDetail.Recipients.NotifyOnDelivery", PropertyName = "EmailNotifyOnDelivery", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Address.City", PropertyName = "HoldCity", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Address.CountryCode", PropertyName = "HoldCountryCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Address.PostalCode", PropertyName = "HoldPostalCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Address.Residential", PropertyName = "RecipientResidential", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Address.StateOrProvinceCode", PropertyName = "HoldStateOrProvinceCode", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Address.StreetLines", PropertyName = "HoldStreetLines", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Contact.CompanyName", PropertyName = "HoldCompanyName", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Contact.PersonName", PropertyName = "HoldPersonName", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.LocationContactAndAddress.Contact.PhoneNumber", PropertyName = "HoldPhoneNumber", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HoldAtLocationDetail.PhoneNumber", PropertyName = "HoldDetailPhoneNumber", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.Date", PropertyName = "HomeDeliveryDate", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.HomeDeliveryPremiumType", PropertyName = "HomeDeliveryPremiumType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.HomeDeliveryPremiumDetail.Phone Number", PropertyName = "HomeDeliveryPhoneNumber", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes", PropertyName = "SpecialServiceType1", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.TotalWeight.Units", PropertyName = "ShipmentWeightUnits", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.RequestedShipment.TotalWeight.Value", PropertyName = "ShipmentTotalWeightValue", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.ReturnShipmentDetail.ReturnType", PropertyName = "ReturnType", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "ProcessShipmentRequest.TransactionDetail.CustomerTransactionId", PropertyName = "CustomerTransactionId", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "RequestedShipment.LabelSpecification.ImageType", PropertyName = "", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "SaveLabel", PropertyName = "SaveLabel", SpreadsheetColumnIndex = -1 });
                    usGroundDomesticMapping.Add(new ColumnPropertyMapDefinition { SpreadsheetColumnName = "Transaction Type", PropertyName = "", SpreadsheetColumnIndex = -1 });
                }

                return usGroundDomesticMapping;
            }
            set
            {
                usGroundDomesticMapping = value;
            }
        }
    }
}
