using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Microsoft.Web.Services3.Referral;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    public static class InsureShipCarrierCode
    {
        /// <summary>
        /// Gets the carrier code for a policy request
        /// </summary>
        public static string GetCarrierCode(ShipmentEntity shipment)
        {
            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode) shipment.ShipmentType;
            bool isDomestic = ShipmentTypeManager.GetType(shipmentTypeCode).IsDomestic(shipment);

            string carrierCode;

            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.Amazon:
                    carrierCode = GetCarrierCodeForAmazon(shipment);
                    break;

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    carrierCode = GetUpsCarrierCode(shipment, isDomestic);
                    break;

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    carrierCode = GetCarrierCodeForUspsEndicia(shipment, isDomestic);
                    break;

                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                    carrierCode = ShipmentTypeManager.GetType(shipmentTypeCode).IsDomestic(shipment) ? "USPS" : "USPS-I";
                break;

                case ShipmentTypeCode.FedEx:
                    carrierCode = GetFedExCarrierCode(shipment, isDomestic);
                    break;

                case ShipmentTypeCode.OnTrac:
                    carrierCode = shipment.OnTrac.InsurancePennyOne ? "ONTRAC-P1" : "ONTRAC";
                    break;

                case ShipmentTypeCode.iParcel:
                    carrierCode = shipment.IParcel.Packages[0].InsurancePennyOne ? "I-PARCEL-P1" : "I-PARCEL";
                    break;

                case ShipmentTypeCode.Other:
                    carrierCode = "OTHER";
                    break;

                default:
                    throw new ArgumentOutOfRangeException("shipment", string.Format("ShipmentType '{0}' not valid for InsureShip", EnumHelper.GetDescription(shipmentTypeCode)));
            }

            return carrierCode;
        }

        /// <summary>
        /// Get carrier code for USPS and Endicia
        /// </summary>
        private static string GetCarrierCodeForUspsEndicia(ShipmentEntity shipment, bool isDomestic)
        {
            string carrierCode;
            PostalServiceType postalServiceType = (PostalServiceType) shipment.Postal.Service;
            if (ShipmentTypeManager.IsConsolidator(postalServiceType) || ShipmentTypeManager.IsDhl(postalServiceType))
            {
                carrierCode = "DHL-GLOBAL";
            }
            else
            {
                carrierCode = isDomestic ? "USPS" : "USPS-I";
            }
            return carrierCode;
        }

        /// <summary>
        /// Get carrier code for Amazon
        /// </summary>
        private static string GetCarrierCodeForAmazon(ShipmentEntity shipment)
        {
            string carrierCode;
            carrierCode = shipment.Amazon.CarrierName;
            if (carrierCode.Equals("STAMPS_DOT_COM", StringComparison.OrdinalIgnoreCase))
            {
                carrierCode = "USPS";
            }
            return carrierCode;
        }

        /// <summary>
        /// Gets the FedEx carrier code based on the shipment configuration.
        /// </summary>
        private static string GetFedExCarrierCode(ShipmentEntity shipment, bool isDomestic)
        {
            string carrierCode;
            FedExServiceType fedExServiceType = (FedExServiceType) shipment.FedEx.Service;
            if (fedExServiceType == FedExServiceType.InternationalEconomy || fedExServiceType == FedExServiceType.InternationalEconomyFreight || fedExServiceType == FedExServiceType.SmartPost)
            {
                carrierCode = isDomestic ? "E-FEDEX" : "E-FEDEX-I";
            }
            else
            {
                if (isDomestic)
                {
                    carrierCode = shipment.FedEx.Packages[0].InsurancePennyOne ? "FEDEX-P1" : "FEDEX";
                }
                else
                {
                    carrierCode = "FEDEX-I";
                }
            }
            return carrierCode;
        }

        /// <summary>
        /// Gets the UPS carrier code based on the shipment configuration.
        /// </summary>
        private static string GetUpsCarrierCode(ShipmentEntity shipment, bool isDomestic)
        {
            string carrierCode;
            if (UpsUtility.IsUpsMiOrSurePostService((UpsServiceType)shipment.Ups.Service))
            {
                carrierCode = isDomestic ? "E-UPS" : "E-UPS-I";
            }
            else
            {
                if (shipment.Ups.Packages[0].InsurancePennyOne)
                {
                    carrierCode = "UPS-P1";
                }
                else
                {
                    carrierCode = isDomestic ? "UPS" : "UPS-I";
                }
            }
            return carrierCode;
        }
    }
}
