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
            TrackingResult result = new TrackingResult();
            UpsApiTrackResponse response = ProcessRequest(shipment);

            result.Details.AddRange(response.ResultDetails);
            string summary = $"<b>{response.OverallStatus}</b>";

            if (response.OverallStatus == "Delivered")
            {
                summary += " on " + response.DeliveryDateTime;
            }
            else if (response.DeliveryEstimate.HasValue)
            {
                summary += $"<br/><span style='color: rgb(80, 80, 80);'>Should arrive: {response.DeliveryEstimate.Value}</span>";
            }

            if (!string.IsNullOrEmpty(response.SignedFor))
            {
                summary += $"<br/><span style='color: rgb(80, 80, 80);'>Signed by: {response.SignedFor}</span>";
            }

            result.Summary = summary;

            return result;
        }


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

        private static string GetTrackingNumber(ShipmentEntity shipment)
        {
            string trackingNumber = shipment.TrackingNumber;
            if (InterapptiveOnly.MagicKeysDown)
            {
                if (UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service))
                {
                    trackingNumber = "9102084383041101186729";
                }
                else
                {
                    trackingNumber = "1Z7VW1630324312293";
                }
            }

            return trackingNumber;
        }

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
