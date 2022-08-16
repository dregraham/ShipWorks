using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Amazon-related utility methods
    /// </summary>
    public static class AmazonUtility
    {
        /// <summary>
        /// Get the carrier name and tracking number
        /// </summary>
        public static (string carrierName, string carrierCode, string trackingNumber) GetCarrierInfoAndTrackingNumber(ShipmentEntity shipment)
        {
            // Per an email on 9/11/07, Amazon will only respond correctly if the code is in upper case, and if its also apart of the method.
            ShipmentTypeCode shipmentType = (ShipmentTypeCode) shipment.ShipmentType;

            string trackingNumber = shipment.TrackingNumber;

            // Get the carrier based on what we currently know, we'll check it in the DetermineAlternateTracking below
            string carrier = GetCarrierName(shipment, shipmentType);

            // Adjust tracking details per Mail Innovations and others
            WorldShipUtility.DetermineAlternateTracking(shipment, (track, service) =>
            {
                if (track.Length > 0)
                {
                    trackingNumber = track;
                    carrier = "UPS Mail Innovations";
                }
                else
                {
                    shipmentType = ShipmentTypeCode.Other;
                }
            });

            string carrierCode = carrier;
            string carrierName = null;

            if (!GetValidCarrierCodes().Contains(carrier))
            {
                carrierCode = "Other";
                carrierName = carrier;
            }

            return (carrierName, carrierCode, trackingNumber);
        }

        /// <summary>
        /// Gets the carrier for the shipment.  If the shipment type is Other, it will use Other.Carrier.
        /// </summary>
        public static string GetCarrierName(ShipmentEntity shipment, ShipmentTypeCode shipmentTypeCode)
        {
            if (shipment.ShipmentType == (int) ShipmentTypeCode.Other)
            {
                return ShippingManager.GetOtherCarrierDescription(shipment).Name;
            }

            if (shipment.ShipmentType == (int) ShipmentTypeCode.DhlEcommerce)
            {
                return "DHL";
            }

            if (ShipmentTypeManager.ShipmentTypeCodeSupportsDhl((ShipmentTypeCode) shipment.ShipmentType))
            {
                PostalServiceType service = (PostalServiceType) shipment.Postal.Service;

                // The shipment is an Endicia or Stamps shipment, check to see if it's DHL
                if (ShipmentTypeManager.IsDhl(service))
                {
                    // The DHL carrier for Endicia/Stamps is:
                    return "DHL eCommerce";
                }

                if (ShipmentTypeManager.IsConsolidator(service))
                {
                    return "Consolidator";
                }

                // Use the default carrier for other Endicia types
                return ShippingManager.GetCarrierName(shipmentTypeCode);
            }

            return ShippingManager.GetCarrierName(shipmentTypeCode);
        }

        /// <summary>
        /// List of valid carrier codes from: https://sellercentral.amazon.com/gp/help/help.html?itemID=G200137470&
        /// </summary>
        public static HashSet<string> GetValidCarrierCodes() =>
            new HashSet<string>
            {
                "Blue Package",
                "Canada Post",
                "City Link",
                "DHL",
                "DHL Global Mail",
                "Fastway",
                "FedEx",
                "FedEx SmartPost",
                "GLS",
                "GO!",
                "Hermes Logistik Gruppe",
                "Newgistics",
                "NipponExpress",
                "OnTrac",
                "OSM",
                "Parcelforce",
                "Royal Mail",
                "SagawaExpress",
                "Streamlite",
                "Target",
                "TNT",
                "UPS",
                "UPS Mail Innovations",
                "USPS",
                "YamatoTransport"
            };
    }
}
