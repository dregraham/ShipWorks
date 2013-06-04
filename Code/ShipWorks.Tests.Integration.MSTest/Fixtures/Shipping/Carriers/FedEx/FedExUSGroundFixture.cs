using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;

namespace ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.FedEx
{
    public class FedExUSGroundFixture : FedExInternationalPrototypeFixture
    {
        public string PackageDangerousGoodsDetail { get; set; }
        public string HazardProperShippingName { get; set; }
        public string HazardClass { get; set; }
        public string HazardDescriptionID { get; set; }
        public string HazardousPackingGroup { get; set; }
        public string HazardQuantityAmount { get; set; }
        public string HazardQuantityUnits { get; set; }
        public string DangerEmergencyContactNumber { get; set; }
        public string DangerOfferor { get; set; }
        public string DangerCounts { get; set; }
        public string DangerUnits { get; set; }

        public string PackageSignatureOptionType { get; set; }
        public string HomeDeliveryPremiumType { get; set; }
        public string HomeDeliveryDate { get; set; }
        public string HomeDeliveryPhoneNumber { get; set; }

        public string EMailNotificationRecipientType { get; set; } // varies between recipient, shipper, broker, and other
        public string NotifyEMailAddress { get; set; } //always abc@xyx
        public string NotifyOnDelivery { get; set; } // always false
        public string NotifyEmailFormat { get; set; } // always html
        public string NotifyEmailLanguageCode { get; set; }  // always en

        public string NaftaPreferenceCriterion { get; set; }  
        public string NaftaProducerDetermination { get; set; }
        public string NaftaProducerId { get; set; }  
        public string NaftaNetCostMethod { get; set; }
        public string NaftaCostDateBegins { get; set; }  // always blank
        public string NaftaCostDateEnds { get; set; }  // always blank
        public string CustomsRegulatoryControls { get; set; }

        public string PackageLineItemDimensionUnits { get; set; }

        /// <summary>
        /// Creates the shipment.
        /// </summary>
        /// <returns></returns>
        public override ShipmentEntity CreateShipment()
        {
            ShipmentEntity shipment = base.CreateShipment();

            shipment.FedEx.ReferenceCustomer = string.Empty;
            shipment.FedEx.ReferenceInvoice = string.Empty;

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
                case ("lithium_batteries"): return FedExDangerousGoodsMaterialType.LithiumBatteries;
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
    }
}
