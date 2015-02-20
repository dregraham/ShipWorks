using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using System.Diagnostics;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.Postal;


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
                case ShipmentTypeCode.Stamps:
                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Stamps:
                case ShipmentTypeCode.PostalWebTools:
                    carrierCode = "USPS";
                    break;

                case ShipmentTypeCode.Endicia:
                    // The shipment is an Endicia shipment, check to see if it's DHL
                    if (shipmentEntity.Postal != null && ShipmentTypeManager.IsEndiciaDhl((PostalServiceType)shipmentEntity.Postal.Service))
                    {
                        // The DHL carrier for Endicia is:
                        carrierCode = "DHL";
                    }
                    else if (shipmentEntity.Postal != null && ShipmentTypeManager.IsEndiciaConsolidator((PostalServiceType)shipmentEntity.Postal.Service))
                    {
                        carrierCode = "Consolidator";
                    }
                    else
                    {
                        // Use the default carrier for other Endicia types
                        carrierCode = "USPS";
                    }
                    break;

                case ShipmentTypeCode.FedEx:
                    carrierCode = "FedEX";
                    break;

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    carrierCode = "UPS";
                    break;

                case ShipmentTypeCode.OnTrac:
                    carrierCode = "OnTrac";
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
