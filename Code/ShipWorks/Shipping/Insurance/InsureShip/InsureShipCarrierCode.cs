using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;

namespace ShipWorks.Shipping.Insurance.InsureShip
{
    public static class InsureShipCarrierCode
    {
        /// <summary>
        /// Gets the carrier code for a policy request
        /// </summary>
        public static string GetCarrierCode(ShipmentEntity shipment)
        {
            ShipmentTypeCode shipmentTypeCode = (ShipmentTypeCode)shipment.ShipmentType;
            
            string carrierCode;

            switch (shipmentTypeCode)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    carrierCode = shipment.Ups.Packages[0].InsurancePennyOne ? "UPS-P1" : "UPS";
                    break;

                case ShipmentTypeCode.Endicia:
                    PostalServiceType postalServiceType = (PostalServiceType)shipment.Postal.Service;
                    if (ShipmentTypeManager.IsEndiciaConsolidator(postalServiceType) || ShipmentTypeManager.IsEndiciaDhl(postalServiceType))
                    {
                        carrierCode = "OTHER";
                    }
                    else
                    {
                        carrierCode = ShipmentTypeManager.GetType(shipmentTypeCode).IsDomestic(shipment) ? "USPS" : "USPS-I";
                    }
                    break;

                case ShipmentTypeCode.Stamps:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Stamps:
                    carrierCode = ShipmentTypeManager.GetType(shipmentTypeCode).IsDomestic(shipment) ? "USPS" : "USPS-I";
                break;

                case ShipmentTypeCode.FedEx:
                    carrierCode = shipment.FedEx.Packages[0].InsurancePennyOne ? "FEDEX-P1" : "FEDEX";
                    break;

                case ShipmentTypeCode.OnTrac:
                case ShipmentTypeCode.iParcel:
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
