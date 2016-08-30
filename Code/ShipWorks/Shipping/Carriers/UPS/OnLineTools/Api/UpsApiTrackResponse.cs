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
    public class UpsApiTrackResponse
    {
        readonly ILog log = LogManager.GetLogger(typeof(UpsApiTrackClient));
        private string signedFor = string.Empty;

        public UpsApiTrackResponse()
        {
            ResultDetails = new List<TrackingResultDetail>();
        }

        public List<TrackingResultDetail> ResultDetails { get; }

        /// <summary>
        /// This is the last status reported by UPS.  Sometimes it gets delivered, and then
        /// another status comes in.  So if we see any "Delivered" at all, it takes precedence
        /// over any more "recent" status
        /// </summary>
        public string OverallStatusCode { get; private set; }

        public DateTime? DeliveryEstimate { get; set; }

        public DateTime? DeliveryDateTime { get; set; }

        public string SignedFor
        {
            get { return signedFor; }
            set { signedFor = AddressCasing.Apply(value); }
        }

        public string OverallStatus
        {
            get
            {
                // Convert the status code to its readable name
                string statusName = "";
                if (OverallStatusCode == "I") statusName = "In Transit";
                if (OverallStatusCode == "D") statusName = "Delivered";
                if (OverallStatusCode == "X") statusName = "Exception";
                if (OverallStatusCode == "C") statusName = "Pickup";
                if (OverallStatusCode == "M") statusName = "Manifest Pickup";
                return statusName;
            }
        }
        
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

                // Get the navigator for this activity
                XPathNavigator activityNode = activityNodes.Current.Clone();

                if (string.IsNullOrEmpty(SignedFor))
                {
                    SignedFor = XPathUtility.Evaluate(activityNode, "ActivityLocation/SignedForByName", "");
                }

                string statusCode = XPathUtility.Evaluate(activityNode, "Status/StatusType/Code", "");

                // Update the overall status of the package
                if (OverallStatusCode == "" || statusCode == "D")
                {
                    OverallStatusCode = statusCode;
                }

                PopulateDeliveryDateTime(statusCode, activityNode);

                TrackingResultDetail detail = new TrackingResultDetail();
                ResultDetails.Add(detail);

                detail.Activity = GetStatus(activityNode);
                detail.Location = GetLocation(activityNode, shipment);
                detail.Date = GetDate(activityNode);
                detail.Time = GetTime(activityNode);
            }
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
                    DeliveryDateTime = DateTime.ParseExact(concatenatedDateTime, "yyyyMMdd HHmmss", null);
                }
                catch (FormatException ex)
                {
                    log.Warn("Could not parse UPS time: " + time, ex);
                }
            }
        }

        private void PopulateDeliveryEstimate(XPathNavigator xpath)
        {
            DeliveryEstimate = null;
            string deliveryEstimate = XPathUtility.Evaluate(xpath, "//Shipment/ScheduledDeliveryDate", "");
            // Get date into readable format
            if (!string.IsNullOrEmpty(deliveryEstimate))
            {
                DateTime deliveryEstimateDate;
                if (DateTime.TryParseExact(deliveryEstimate, "yyyyMMdd", null, DateTimeStyles.None, out deliveryEstimateDate))
                {
                    DeliveryEstimate = deliveryEstimateDate;
                }
                else
                {
                    log.Warn("Could not parse UPS delivery date: " + deliveryEstimate);
                    Debug.Fail("Could not parse UPS delivery date", deliveryEstimate);
                }
            }
        }

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
