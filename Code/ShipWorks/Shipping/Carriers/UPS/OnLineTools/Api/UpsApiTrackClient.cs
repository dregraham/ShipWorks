using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Tracking;
using System.Linq;
using System.Xml;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Wrapper for the UPS tracking API
    /// </summary>
    public static class UpsApiTrackClient
    {
        /// <summary>
        /// Return the tracking results for the given tracking number
        /// </summary>
        public static TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            UpsApiTrackResponse response = ProcessRequest(shipment);
            return response.TrackingResult;
        }

        /// <summary>
        /// Processes the request.
        /// </summary>
        private static UpsApiTrackResponse ProcessRequest(ShipmentEntity shipment)
        {
            // Create the client for connecting to the UPS server
            XmlDocument xmlResponse;
            using (XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.Track, GetAccount()))
            {
                // Only valid tag, the tracking number
                xmlWriter.WriteElementString("TrackingNumber", GetTrackingNumber(shipment));

                if (UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service))
                {
                    xmlWriter.WriteElementString("TrackingOption", "03");
                }

                // Process the XML request
                xmlResponse = UpsWebClient.ProcessRequest(xmlWriter);
            }
            UpsApiTrackResponse response = new UpsApiTrackResponse();
            response.LoadResponse(xmlResponse, shipment);
            return response;
        }

        /// <summary>
        /// Gets the tracking number. Uses a canned tracking number if magic keys are down.
        /// </summary>
        private static string GetTrackingNumber(ShipmentEntity shipment)
        {
            string trackingNumber = shipment.TrackingNumber;
            if (InterapptiveOnly.MagicKeysDown)
            {
                trackingNumber = UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service)
                    ? "9102084383041101186729"
                    : "1ZTT97230394720182";
            }

            return trackingNumber;
        }

        /// <summary>
        /// Gets an account. If no account available, throws.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="UpsException">ShipWorks cannot track a UPS shipment without a configured UPS account.</exception>
        private static UpsAccountEntity GetAccount()
        {
            UpsAccountEntity upsAccount = UpsAccountManager.Accounts.FirstOrDefault();
            if (upsAccount == null)
            {
                throw new UpsException("ShipWorks cannot track a UPS shipment without a configured UPS account.");
            }

            return upsAccount;
        }
    }
}
