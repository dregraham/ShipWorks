using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Tracking;
using System.IO;
using System.Xml;
using ShipWorks.ApplicationCore.Logging;
using System.Net;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using System.Text.RegularExpressions;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.Business;
using Interapptive.Shared.Net;
using System.Web;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Provides a wrapper around the USPS tracking API
    /// </summary>
    public static class PostalWebClientTracking
    {
        // Regex to break the USPS activity data into its pieces
        static Regex uspsActivityRegex1 = new Regex(@"
            (?<Date>\w+[ ]+[0-9]+)[ ]+
            (?<Time>(?:[0-9]|:)+[ ]+(?:am|pm))[ ]+
            (?<Activity>.*)[ ]+
            (?<City>\w+)[ ]+
            (?<State>\w\w)[ ]+
            (?<Zip>[0-9]{5})",
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        // Regex to break the USPS activity data into its pieces
        static Regex uspsActivityRegex2 = new Regex(@"
				(?<Activity>.*?),[ ]+
				(?<Date>\w+[ ]+[0-9]+),[ ]+[0-9]+,[ ]+
				(?<Time>(?:[0-9]|:)+[ ]+(?:am|pm)),[ ]+
				(?<City>\w+),[ ]+
				(?<State>\w\w)[ ]+
				(?<Zip>[0-9]{5})",
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        // Regex for the ugly "ELECTRONIC SHIPPING INFO RECIEVED" line
        static Regex uspsInfoReceivedRegex = new Regex(@"
                ELECTRONIC[ ](?<Activity>SHIPPING[ ]INFO[ ]RECEIVED),
                [ ]+
                (?<Date>\w+[ ]+[0-9]+),[ ]+[0-9]+",
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        /// <summary>
        /// Get the tracking result for the given tracking number
        /// </summary>
        public static TrackingResult TrackShipment(string trackingNumber)
        {
            if (InterapptiveOnly.MagicKeysDown)
            {
                trackingNumber = "9101805213907363038661";
            }

            string xmlRequest;

            using (StringWriter writer = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(writer);
                xmlWriter.Formatting = Formatting.Indented;

                // Open tag
                xmlWriter.WriteStartElement("TrackRequest");

                // Username and password
                xmlWriter.WriteAttributeString("USERID", PostalWebUtility.UspsUsername);
                xmlWriter.WriteAttributeString("PASSWORD", PostalWebUtility.UspsPassword);

                xmlWriter.WriteStartElement("TrackID");
                xmlWriter.WriteAttributeString("ID", trackingNumber);
                xmlWriter.WriteEndElement();

                // Close of the main element tag
                xmlWriter.WriteEndElement();

                xmlRequest = writer.ToString();
            }

            // Log the request
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.UspsNoPostage, "Track");
            logger.LogRequest(xmlRequest);

            // Process the request
            string xmlResponse = ProcessXmlRequest(xmlRequest);

            // Log the response
            logger.LogResponse(xmlResponse);

            // Load the USPS response
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(xmlResponse);
            }
            catch (XmlException)
            {
                throw new ShippingException("The USPS server returned an invalid response or the request was blocked.");
            }

            XPathNavigator xpath = xmlDocument.CreateNavigator();

            // See if there was an error
            XmlNodeList errorNodes = xmlDocument.GetElementsByTagName("Error");
            if (errorNodes.Count > 0)
            {
                string error = XPathUtility.Evaluate(xpath, "//Error/Description", "The USPS server returned an unspecified error.");

                // Throw the exception
                throw new ShippingException(error);
            }

            TrackingResult result = new TrackingResult();
            result.Summary = XPathUtility.Evaluate(xpath, "//TrackSummary", "(None)");

            // Create a detail record for each item
            foreach (XPathNavigator xpathDetail in xpath.Select("//TrackDetail"))
            {
                string detail = xpathDetail.Value;

                // Try format #1
                Match match = uspsActivityRegex1.Match(detail);

                // Otherwise try form #2
                if (!match.Success)
                {
                    match = uspsActivityRegex2.Match(detail);
                }

                // Otherwise maybe it's format #3
                if (!match.Success)
                {
                    match = uspsInfoReceivedRegex.Match(detail);
                }

                // If one of them worked, we can pull out the individual details
                if (match.Success)
                {
                    string city = AddressCasing.Apply(match.Groups["City"].Value);

                    string date = match.Groups["Date"].Value;
                    string time = match.Groups["Time"].Value;
                    string location = city + (city.Length > 0 ? ", " : "") + match.Groups["State"].Value;
                    string activity = AddressCasing.Apply(match.Groups["Activity"].Value);

                    result.Details.Add(new TrackingResultDetail
                        {
                            Date = date,
                            Time = time,
                            Location = location,
                            Activity = activity
                        });
                }
                else
                {
                    result.Details.Add(new TrackingResultDetail { Activity = detail });
                }
            }

            return result;
        }

        /// <summary>
        /// Process the given XML request and return the response
        /// </summary>
        private static string ProcessXmlRequest(string xmlRequest)
        {
            // The production server URL
            string serverUrl = PostalWebUtility.UseTestServer ?
                "http://stg-production.shippingapis.com/ShippingApi.dll?API=TrackV2&XML="
                : "http://production.shippingapis.com/ShippingAPI.dll?API=TrackV2&XML=";

            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(serverUrl + HttpUtility.UrlEncode(xmlRequest));
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                // See if we got a valid response
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new WebException(response.StatusDescription);
                }

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ShippingException));
            }
        }
    }
}
