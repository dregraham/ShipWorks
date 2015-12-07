using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Tracking;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using log4net;
using System.Diagnostics;
using Interapptive.Shared;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Wrapper for the UPS tracking API
    /// </summary>
    public static class UpsApiTrackClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(UpsApiTrackClient));

        /// <summary>
        /// Return the tracking results for the given tracking number
        /// </summary>
        [NDependIgnoreLongMethod]
        [NDependIgnoreComplexMethodAttribute]
        public static TrackingResult TrackShipment(ShipmentEntity shipment)
        {
            UpsAccountEntity account = UpsAccountManager.Accounts.FirstOrDefault();
            if (account == null)
            {
                throw new UpsException("ShipWorks cannot track a UPS shipment without a configured UPS account.");
            }

            string trackingNumber = shipment.TrackingNumber;
            if (InterapptiveOnly.MagicKeysDown)
            {
                if (UpsUtility.IsUpsMiService((UpsServiceType)shipment.Ups.Service))
                {
                    trackingNumber = "9102084383041101186729";
                }
                else
                {
                    trackingNumber = "1Z7VW1630324312293";    
                }
            }

            // Create the client for connecting to the UPS server
            XmlDocument response;
            using (XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.Track, account))
            {
                // Only valid tag, the tracking number
                xmlWriter.WriteElementString("TrackingNumber", trackingNumber);

                if (UpsUtility.IsUpsMiService((UpsServiceType)shipment.Ups.Service))
                {
                    xmlWriter.WriteElementString("TrackingOption", "03");
                }

                // Process the XML request
                response = UpsWebClient.ProcessRequest(xmlWriter);
            }

            TrackingResult result = new TrackingResult();

            // Create the XPath engine
            XPathNavigator xpath = response.CreateNavigator();

            // Delivery date
            string deliveryEstimate = XPathUtility.Evaluate(xpath, "//Shipment/ScheduledDeliveryDate", "");

            // Get date into readable format
            try
            {
                if (!string.IsNullOrEmpty(deliveryEstimate))
                {
                    deliveryEstimate = DateTime.ParseExact(deliveryEstimate, "yyyyMMdd", null).ToString("M/dd/yyy");
                }
            }
            catch (FormatException ex)
            {
                log.Warn("Could not parse UPS delivery date: " + deliveryEstimate, ex);
                Debug.Fail("Could not parse UPS delivery date", ex.Message);
            }

            // This is the last status reported by UPS.  Sometimes it gets delievered, and then
            // another status comes in.  So if we see any "Delivered" at all, it takes precendence
            // over any more "recent" status
            string overallStatus = "";

            // Get all the package results nodes
            XPathNodeIterator packageNodes = xpath.Select("//Shipment/Package");

            string deliveryDateTime = "";
            string signedFor = "";

            string lastDate = "";

            // Just do the first package
            if (packageNodes.MoveNext())
            {
                // Get the navigator for this package
                XPathNavigator packageNode = packageNodes.Current.Clone();

                // Get all the activity nodes
                XPathNodeIterator activityNodes = packageNode.Select("Activity");

                // Go through each one
                while (activityNodes.MoveNext())
                {
                    // Get the navigator for this activity
                    XPathNavigator activityNode = activityNodes.Current.Clone();

                    string city = XPathUtility.Evaluate(activityNode, "ActivityLocation/Address/City", "");
                    string stateCode = XPathUtility.Evaluate(activityNode, "ActivityLocation/Address/StateProvinceCode", "");
                    string countryCode = XPathUtility.Evaluate(activityNode, "ActivityLocation/Address/CountryCode", "");

                    if (string.IsNullOrEmpty(signedFor))
                    {
                        signedFor = XPathUtility.Evaluate(activityNode, "ActivityLocation/SignedForByName", "");
                    }

                    string statusCode = XPathUtility.Evaluate(activityNode, "Status/StatusType/Code", "");
                    string statusDesc = XPathUtility.Evaluate(activityNode, "Status/StatusType/Description", "");

                    string date = XPathUtility.Evaluate(activityNode, "Date", "");
                    string time = XPathUtility.Evaluate(activityNode, "Time", "").ToLower();

                    // Cleanup the city and status descriptions
                    city = AddressCasing.Apply(city);
                    statusDesc = AddressCasing.Apply(statusDesc);
                    signedFor = AddressCasing.Apply(signedFor);

                    // Build the location string
                    string location = city;

                    if (stateCode != "")
                    {
                        location += ((location != "") ? ", " : "") + stateCode;
                    }

                    if (countryCode != shipment.AdjustedOriginCountryCode())
                    {
                        // Only show the country code if it differs than the country of origin
                        location += ((location != "") ? ", " : "") + Geography.GetCountryName(countryCode);
                    }

                    // Convert the status code to its readable name
                    string statusName = "";
                    if (statusCode == "I") statusName = "In Transit";
                    if (statusCode == "D") statusName = "Delivered";
                    if (statusCode == "X") statusName = "Exception";
                    if (statusCode == "C") statusName = "Pickup";
                    if (statusCode == "M") statusName = "Manifest Pickup";

                    // This helps the output quite a bit
                    if (statusDesc == "Billing Information Received")
                    {
                        statusDesc = "Billing Information<br>Received";
                    }

                    // Update the overall status of the package
                    if (overallStatus == "" || statusCode == "D")
                    {
                        overallStatus = statusName;
                    }

                    // Get date into readable format
                    try
                    {
                        date = DateTime.ParseExact(date, "yyyyMMdd", null).ToString("M/dd/yyy");
                    }
                    catch (FormatException ex)
                    {
                        log.Warn("Could not parse UPS date: " + date, ex);
                    }

                    // Get time into readable format
                    try
                    {
                        time = DateTime.ParseExact(time, "HHmmss", null).ToString("h:mm tt").ToLower();
                    }
                    catch (FormatException ex)
                    {
                        log.Warn("Could not parse UPS time: " + time, ex);
                    }

                    // Only show scheduled delivery if its not delivered yet
                    if (statusCode == "D")
                    {
                        deliveryDateTime = date + " " + time;
                    }

                    TrackingResultDetail detail = new TrackingResultDetail();
                    result.Details.Add(detail);

                    detail.Activity = statusDesc;
                    detail.Location = location;
                    detail.Date = (lastDate != date) ? date : "";
                    detail.Time = time;

                    lastDate = date;
                }
            }

            string summary = string.Format("<b>{0}</b>", overallStatus);

            if (overallStatus == "Delivered")
            {
                summary += " on " + deliveryDateTime;
            }
            else if (!string.IsNullOrEmpty(deliveryEstimate))
            {
                summary += string.Format("<br/><span style='color: rgb(80, 80, 80);'>Should arrive: {0}</span>", deliveryEstimate);
            }

            if (!string.IsNullOrEmpty(signedFor))
            {
                summary += string.Format("<br/><span style='color: rgb(80, 80, 80);'>Signed by: {0}</span>", signedFor);
            }

            result.Summary = summary;

            return result;
        }
    }
}
