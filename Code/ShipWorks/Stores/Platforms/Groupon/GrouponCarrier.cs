using ShipWorks.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using System.Diagnostics;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
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
        public static string GetCarrierCode(ShipmentEntity shipmentEntity)
        {
            string carrierCode = string.Empty;
            switch (((ShipmentTypeCode)shipmentEntity.ShipmentType))
            {
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                    carrierCode = "USPS";
                    break;

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    // The shipment is an Endicia shipment, check to see if it's DHL
                    if (shipmentEntity.Postal != null && ShipmentTypeManager.IsDhl((PostalServiceType)shipmentEntity.Postal.Service))
                    {
                        // The DHL carrier for Endicia is:
                        carrierCode = "DHL";
                    }
                    else if (shipmentEntity.Postal != null && ShipmentTypeManager.IsConsolidator((PostalServiceType)shipmentEntity.Postal.Service))
                    {
                        carrierCode = "USPS";
                    }
                    else
                    {
                        // Use the default carrier for other Endicia types
                        carrierCode = "USPS";
                    }
                    break;

                case ShipmentTypeCode.FedEx:
                    carrierCode = "FedEx";
                    break;

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    ShippingManager.EnsureShipmentLoaded(shipmentEntity);
                    //The shipment is a UPS shipment, check to see if it is UPS-MI
                    if (UpsUtility.IsUpsMiService((UpsServiceType)shipmentEntity.Ups.Service))
                    {
                        carrierCode = "upsmi";

                    }
                    else
                    {
                        carrierCode = "UPS";
                    }
                    break;

                case ShipmentTypeCode.OnTrac:
                    carrierCode = "ont";
                    break;

                case ShipmentTypeCode.iParcel:
                    carrierCode = "i-parcel";
                    break;

                case ShipmentTypeCode.Other:
                    ShippingManager.EnsureShipmentLoaded(shipmentEntity);
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
