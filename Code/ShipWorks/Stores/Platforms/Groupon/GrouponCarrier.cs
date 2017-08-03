using System.Diagnostics;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
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
                    carrierCode = GetPostalCarrierCode(shipmentEntity);
                    break;

                case ShipmentTypeCode.FedEx:
                    carrierCode = GetFedexCarrierCode(shipmentEntity);
                    break;

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    //The shipment is a UPS shipment, check to see if it is UPS-MI
                    carrierCode = UpsUtility.IsUpsMiService((UpsServiceType) shipmentEntity.Ups.Service) ? "upsmi" : "ups";
                    break;

                case ShipmentTypeCode.OnTrac:
                    carrierCode = "ont";
                    break;

                case ShipmentTypeCode.iParcel:
                    carrierCode = "i-parcel";
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

        /// <summary>
        /// Gets the fedex carrier code.
        /// </summary>
        private static string GetFedexCarrierCode(IShipmentEntity shipmentEntity)
        {
            string carrierCode = string.Empty;

            if (shipmentEntity.FedEx != null)
            {
                FedExServiceType service = (FedExServiceType) shipmentEntity.FedEx.Service;
                if (service == FedExServiceType.PriorityOvernight)
                {
                    carrierCode = "Fexp";
                }
                else if (service == FedExServiceType.GroundHomeDelivery)
                {
                    carrierCode = "Fedexhd";
                }
                else if (service == FedExServiceType.SmartPost)
                {
                    carrierCode = "fedexsp";
                }
                else
                {
                    carrierCode = "fedex";
                }
            }
            return carrierCode;
        }

        /// <summary>
        /// Gets the carrier code for Usps shipments
        /// </summary>
        private static string GetPostalCarrierCode(IShipmentEntity shipmentEntity)
        {
            string carrierCode;

            if (shipmentEntity.Postal != null && ShipmentTypeManager.IsDhl((PostalServiceType) shipmentEntity.Postal.Service))
            {
                // Per Groupon -
                // DHLSM has more restrictive checks in terms of the format of the tracking number, when compared to
                // DHLGM. Given that there is no other difference between these two, in terms of how they are treated
                // by our system, I'd suggest that you upload the DHLSM related tracking numbers as DHLGM
                carrierCode = "dhlgm";
            }
            else if (shipmentEntity.Postal != null &&
                     ShipmentTypeManager.IsConsolidator((PostalServiceType) shipmentEntity.Postal.Service))
            {
                carrierCode = "usps";
            }
            else if (shipmentEntity.Postal != null &&
                     (PostalServiceType) shipmentEntity.Postal.Service == PostalServiceType.MediaMail)
            {
                carrierCode = "uspsmm";
            }
            else
            {
                // Use the default carrier for other Endicia types
                carrierCode = "usps";
            }

            return carrierCode;
        }
    }
}
