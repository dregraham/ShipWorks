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
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
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
                    break;

                case ShipmentTypeCode.Endicia:
                    PostalServiceType postalServiceType = (PostalServiceType)shipment.Postal.Service;
                    if (ShipmentTypeManager.IsEndiciaConsolidator(postalServiceType) || ShipmentTypeManager.IsEndiciaDhl(postalServiceType))
                    {
                        carrierCode = "DHL-GLOBAL";
                    }
                    else
                    {
                        carrierCode = isDomestic ? "USPS" : "USPS-I";
                    }
                    break;

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                    carrierCode = ShipmentTypeManager.GetType(shipmentTypeCode).IsDomestic(shipment) ? "USPS" : "USPS-I";
                break;

                case ShipmentTypeCode.FedEx:
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
    }
}
