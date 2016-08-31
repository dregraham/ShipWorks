using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Tracking;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Xml.XPath;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// Processes the API Response of a UPS tracking web call.
    /// </summary>
    public class UpsApiTrackResponse
    {
        readonly ILog log = LogManager.GetLogger(typeof(UpsApiTrackClient));

        private string signedFor = string.Empty;
        private string overallStatusCode;

        private DateTime? deliveryEstimate;
        private DateTime? deliveryDateTime;

        private readonly List<TrackingResultDetail> resultDetails = new List<TrackingResultDetail>();

        /// <summary>
        /// Gets the tracking result.
        /// </summary>
        public TrackingResult TrackingResult
        {
            get
            {
                TrackingResult result = new TrackingResult();
                result.Details.AddRange(resultDetails);

                string overallStatusDescription = GetStatusDescription(overallStatusCode);
                string summary = $"<b>{overallStatusDescription}</b>";

                if (overallStatusDescription == "Delivered")
                {
                    summary += " on " + deliveryDateTime;
                }
                else if (deliveryEstimate.HasValue)
                {
                    summary += $"<br/><span style='color: rgb(80, 80, 80);'>Should arrive: {deliveryEstimate.Value}</span>";
                }

                if (!string.IsNullOrEmpty(signedFor))
                {
                    summary += $"<br/><span style='color: rgb(80, 80, 80);'>Signed by: {AddressCasing.Apply(signedFor)}</span>";
                }

                result.Summary = summary;

                return result;
            }
        }

        /// <summary>
        /// Loads the response.
        /// </summary>
        public void LoadResponse(XmlDocument response, ShipmentEntity shipment)
        {
            XPathNavigator xpath = response.CreateNavigator();

            // Delivery date
            PopulateDeliveryEstimate(xpath);

            // Get all the package results nodes
            XPathNodeIterator packageNodes = xpath.Select("//Shipment/Package");

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

                    if (string.IsNullOrEmpty(signedFor))
                    {
                        signedFor = XPathUtility.Evaluate(activityNode, "ActivityLocation/SignedForByName", "");
                    }

                    string statusCode = XPathUtility.Evaluate(activityNode, "Status/StatusType/Code", "");

                    // Update the overall status of the package
                    if (overallStatusCode == "" || statusCode == "D")
                    {
                        overallStatusCode = statusCode;
                    }

                    PopulateDeliveryDateTime(statusCode, activityNode);

                    TrackingResultDetail detail = new TrackingResultDetail();
                    resultDetails.Add(detail);

                    detail.Activity = GetStatus(activityNode);
                    detail.Location = GetLocation(activityNode, shipment);
                    detail.Date = GetDate(activityNode);
                    detail.Time = GetTime(activityNode);
                }
            }
        }

        /// <summary>
        /// Convert the status code to its readable name
        /// </summary>
        private string GetStatusDescription(string statusCode)
        {
            Dictionary<string, string> statusCodesAndNames = new Dictionary<string, string>()
            {
                {"I", "In Transit"},
                {"D", "Delivered"},
                {"X", "Exception"},
                {"C", "Pickup"},
                {"M", "Manifest Pickup"},
            };

            return statusCodesAndNames.ContainsKey(statusCode) ? statusCodesAndNames[statusCode] : string.Empty;
        }

        /// <summary>
        /// Sets delivery date if activity status is delivered.
        /// </summary>
        private void PopulateDeliveryDateTime(string statusCode, XPathNavigator activityNode)
        {
            if (statusCode=="D")
            {
                string date = XPathUtility.Evaluate(activityNode, "Date", "");
                string time = XPathUtility.Evaluate(activityNode, "Time", "").ToLower();
                string concatenatedDateTime = $"{date} {time}";

                try
                {
                    deliveryDateTime = DateTime.ParseExact(concatenatedDateTime, "yyyyMMdd HHmmss", null);
                }
                catch (FormatException ex)
                {
                    log.Warn("Could not parse UPS time: " + time, ex);
                }
            }
        }

        /// <summary>
        /// Populates the delivery estimate based on the ScheduledDeliveryDate
        /// </summary>
        private void PopulateDeliveryEstimate(XPathNavigator xpath)
        {
            deliveryEstimate = null;
            string scheduledDeliveryDate = XPathUtility.Evaluate(xpath, "//Shipment/ScheduledDeliveryDate", "");
            // Get date into readable format
            if (!string.IsNullOrEmpty(scheduledDeliveryDate))
            {
                DateTime deliveryEstimateDate;
                if (DateTime.TryParseExact(scheduledDeliveryDate, "yyyyMMdd", null, DateTimeStyles.None, out deliveryEstimateDate))
                {
                    deliveryEstimate = deliveryEstimateDate;
                }
                else
                {
                    log.Warn($"Could not parse UPS delivery date: {scheduledDeliveryDate}");
                    Debug.Fail("Could not parse UPS delivery date", scheduledDeliveryDate);
                }
            }
        }

        /// <summary>
        /// Gets the status for the activity
        /// </summary>
        private string GetStatus(XPathNavigator activityNode)
        {
            string statusDesc = XPathUtility.Evaluate(activityNode, "Status/StatusType/Description", "");

            statusDesc = AddressCasing.Apply(statusDesc);
            // This helps the output quite a bit
            if (statusDesc == "Billing Information Received")
            {
                statusDesc = "Billing Information<br>Received";
            }

            return statusDesc;
        }

        /// <summary>
        /// Gets the location of the activity
        /// </summary>
        private string GetLocation(XPathNavigator activityNode, ShipmentEntity shipment)
        {
            string city = XPathUtility.Evaluate(activityNode, "ActivityLocation/Address/City", "");
            string stateCode = XPathUtility.Evaluate(activityNode, "ActivityLocation/Address/StateProvinceCode", "");
            string countryCode = XPathUtility.Evaluate(activityNode, "ActivityLocation/Address/CountryCode", "");


            // Cleanup the city and status descriptions
            city = AddressCasing.Apply(city);


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

            return location;
        }

        /// <summary>
        /// Gets the time of the activity
        /// </summary>
        private string GetTime(XPathNavigator activityNode)
        {
            string time = XPathUtility.Evaluate(activityNode, "Time", "").ToLower();
            // Get time into readable format
            try
            {
                time = DateTime.ParseExact(time, "HHmmss", null).ToString("h:mm tt").ToLower();
            }
            catch (FormatException ex)
            {
                log.Warn("Could not parse UPS time: " + time, ex);
            }

            return time;
        }

        /// <summary>
        /// Gets the date of the activity
        /// </summary>
        private string GetDate(XPathNavigator activityNode)
        {
            string date = XPathUtility.Evaluate(activityNode, "Date", "");
            // Get date into readable format
            try
            {
                date = DateTime.ParseExact(date, "yyyyMMdd", null).ToString("M/dd/yyy");
            }
            catch (FormatException ex)
            {
                log.Warn("Could not parse UPS date: " + date, ex);
            }

            return date;
        }

    }
}
