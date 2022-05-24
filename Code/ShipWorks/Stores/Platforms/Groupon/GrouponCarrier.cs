using System.Diagnostics;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Stores.Platforms.Groupon
{
    public static class GrouponCarrier
    {
        /// <summary>
        /// Gets the carrier code.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns></returns>
        public static string GetCarrierCode(IShipmentEntity shipmentEntity)
        {
            string carrierCode = string.Empty;
            switch (((ShipmentTypeCode) shipmentEntity.ShipmentType))
            {
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    carrierCode = (shipmentEntity.Postal != null && ShipmentTypeManager.IsDhl((PostalServiceType) shipmentEntity.Postal.Service)) ? "dhl" : "usps";
                    break;

                case ShipmentTypeCode.FedEx:
                    carrierCode = "fedex";
                    break;

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    //The shipment is a UPS shipment, check to see if it is UPS-MI
                    carrierCode = UpsUtility.IsUpsMiService((UpsServiceType) shipmentEntity.Ups.Service) ? "upsmi" : "ups";
                    break;

                case ShipmentTypeCode.DhlEcommerce:
                    carrierCode = "dhl";
                    break;

                case ShipmentTypeCode.Other:
                    carrierCode = shipmentEntity.Other.Carrier;
                    break;

                default:
                    Debug.Fail("Unhandled ShipmentTypeCode in Groupon.GetCarrierCode");
                    carrierCode = "Other";
                    break;
            }

            return carrierCode;
        }
    }
}
