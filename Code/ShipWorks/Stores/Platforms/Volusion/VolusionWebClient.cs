using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml.XPath;
using System.Collections.Specialized;
using Interapptive.Shared.Net;
using System.Net;
using System.Xml;
using ShipWorks.Shipping.Carriers.Postal;
using log4net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using System.IO;
using ShipWorks.Shipping;
using System.Web;
using System.Globalization;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Web client for the Volusion API.  
    /// 
    /// Not to be confused with the VolusionWebSession class which is used
    /// for screen-scraping connection details for the api
    /// </summary>
    public class VolusionWebClient
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(VolusionWebClient));

        // the store instanec
        VolusionStoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionWebClient(VolusionStoreEntity store)
        {
            this.store = store;
        }

        /// <summary>
        /// Tests credentials against the store to see if they are valid
        /// </summary>
        public bool ValidateCredentials()
        {
            try
            {
                var response = GetCustomer(-1);

                // It seems that sometimes volusion can return null for a GetOrders call and that's OK.  But for GetCustomer, from what I saw,
                // it only does that if the credentials are bad.  Even for a bad customerID, you still get a response (provided the credentials
                // are correct)
                return response != null;
            }
            catch (VolusionException ex)
            {
                log.Error("Failed in ValidateCredentials", ex);

                return false;
            }
        }

        /// <summary>
        /// Returns customer information for customer id
        /// </summary>
        public IXPathNavigable GetCustomer(long customerId)
        {
            try
            {
                List<string> selectColumns = new List<string>();
                selectColumns.Add("*");

                NameValueCollection whereClause = new NameValueCollection();
                whereClause.Add("CustomerID", customerId.ToString());

                HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
                ConfigureRequest(submitter, "Generic\\Customers", selectColumns, whereClause);

                return ProcessRequest(submitter, "GetCustomer");
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(VolusionException));
            }
        }

        /// <summary>
        /// Downloads orders that are Ready To Ship
        /// </summary>
        public IXPathNavigable GetOrders()
        {
            List<string> selectColumns = new List<string>();
            selectColumns.Add("*");

            NameValueCollection whereClause = new NameValueCollection();
            whereClause.Add("OrderStatus", "Ready to Ship");

            HttpVariableRequestSubmitter submitter = new HttpVariableRequestSubmitter();
            ConfigureRequest(submitter, "Generic\\Orders", selectColumns, whereClause);

            return ProcessRequest(submitter, "GetOrders");
        }

        /// <summary>
        /// Uploads shipment details to Volusion
        /// </summary>
        public void UploadShipmentDetails(ShipmentEntity shipment, bool sendEmail)
        {
            OrderEntity order = shipment.Order;

            if (order.IsManual)
            {
                log.WarnFormat("Not uploading shipment details since order {0} is manual.", order.OrderID);
                return;
            }

            try
            {
                ShippingManager.EnsureShipmentLoaded(shipment);
            }
            catch (ObjectDeletedException)
            {
                // Shipment was deleted
                return;
            }
            catch (SqlForeignKeyException)
            {
                // Shipment was deleted
                return;
            }

            try
            {
                string trackingNumber = shipment.TrackingNumber;
                string gateway = GetVolusionGateway(shipment);

                // Adjust tracking details per Mail Innovations and others
                // From Volusion chat:
                // Interapptive: OK. Do you know if I send a mail innovations tracking number with UPS as the carrier, 
                // will the user get a tracking link to ups.com's tracking page?
                // Veronica M: Yes.
                if (UpsUtility.IsUpsMiService((UpsServiceType)shipment.Ups.Service))
                {
                    if (shipment.Ups.UspsTrackingNumber.Length > 0)
                    {
                        trackingNumber = shipment.Ups.UspsTrackingNumber;
                    }
                }

                Uri uri = GetStoreApiEndpoint();

                // Add the query
                uri = new Uri(string.Format("{0}?Login={1}&EncryptedPassword={2}&Import={3}",
                        uri.OriginalString,
                        store.WebUserName,
                        store.ApiPassword,
                        "Insert"));

                string postXml;

                // build the request content to be POSTed to volusion.
                // MUST be UTF8 or Volusion just silently fails.
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.UTF8);

                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("xmldata");
                    xmlWriter.WriteStartElement("TrackingNumbers");

                    xmlWriter.WriteElementString("Gateway", gateway);
                    xmlWriter.WriteElementString("OrderID", order.OrderNumber.ToString());
                    xmlWriter.WriteElementString("ShipDate", shipment.ShipDate.ToShortDateString());
                    xmlWriter.WriteElementString("Shipment_Cost", shipment.ShipmentCost.ToString());
                    
                    if (trackingNumber.Length > 0)
                    {
                        // Volusion's XSD only allows tracking numbers up to 30 characters; anything over 30 will cause Volusion to
                        // fail silently and the order won't get marked as shipped
                        xmlWriter.WriteElementString("TrackingNumber", trackingNumber.Length > 30 ? trackingNumber.Substring(0, 30) : trackingNumber);
                    }
                    else
                    {
                        xmlWriter.WriteElementString("TrackingNumber", order.OrderNumber + "NoTrack");
                    }

                    xmlWriter.WriteElementString("MarkOrderShipped", "true");
                    xmlWriter.WriteElementString("SendShippedEmail", sendEmail.ToString().ToLower(CultureInfo.InvariantCulture));

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();

                    stream.Position = 0;

                    using (StreamReader reader = new StreamReader(stream))
                    {
                        postXml = reader.ReadToEnd();
                    }
                }

                // get data to be posted
                byte[] postBytes = Encoding.UTF8.GetBytes(postXml);

                // Volusion only accepts a blank content-type?
                HttpBinaryPostRequestSubmitter submitter = new HttpBinaryPostRequestSubmitter(postBytes, "");
                submitter.Uri = uri;

                // no need to look for anything in the response.  Failures will bubble as VolusionExceptions
                ProcessRequest(submitter, "UploadShipmentDetails");
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(VolusionException));
            }
        }

        /// <summary>
        /// Gets the Shipping Gateway (carrier) required by Volusion
        /// </summary>
        public static string GetVolusionGateway(ShipmentEntity shipment)
        {
            switch ((ShipmentTypeCode)shipment.ShipmentType)
            {
                case ShipmentTypeCode.FedEx:
                    return "FEDEX";

                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    return "UPS";

                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.PostalWebTools:
                    return "USPS";

                case ShipmentTypeCode.Usps:
                case ShipmentTypeCode.Endicia:
                    PostalServiceType service = (PostalServiceType) shipment.Postal.Service;

                    if (ShipmentTypeManager.IsDhl(service))
                    {
                        return "DHL";
                    }
                    else if (ShipmentTypeManager.IsEndiciaConsolidator(service))
                    {
                        return "OTHER";
                    }

                    return "USPS";

                default:
                    return "";
            }
        }

        /// <summary>
        /// Executes a request, validates the response, and turns it into an xml document
        /// </summary>
        private IXPathNavigable ProcessRequest(HttpRequestSubmitter submitter, string action)
        {
            try
            {
                ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.Volusion, action);
                logEntry.LogRequest(submitter);

                using (IHttpResponseReader reader = submitter.GetResponse())
                {
                    string responseData = reader.ReadResult();
                    logEntry.LogResponse(responseData);

                    if (responseData.Length > 0)
                    {
                        try
                        {
                            XmlDocument document = new XmlDocument();
                            document.LoadXml(responseData);

                            XPathNavigator xpath = document.CreateNavigator();

                            // check for errors
                            CheckResponse(xpath);

                            return xpath;
                        }
                        catch (XmlException)
                        {
                            log.ErrorFormat("Invalid Xml Response: {0}", responseData);

                            throw new VolusionException("ShipWorks received an invalid response from Volusion.", responseData);
                        }
                    }
                    else
                    {
                        // sometimes Volusion indicates success with a blank response (wonderful)
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(VolusionException));
            }
        }

        /// <summary>
        /// Checks response xml for error information
        /// </summary>
        private void CheckResponse(XPathNavigator xpath)
        {
            bool success = XPathUtility.Evaluate(xpath, "//Success", true);
            if (!success)
            {
                string errorMessage = XPathUtility.Evaluate(xpath, "//Message", "An error message was not provided by Volusion.");

                throw new VolusionException(errorMessage);
            }
        }

        /// <summary>
        /// Setup a request 
        /// </summary>
        private void ConfigureRequest(HttpVariableRequestSubmitter submitter, string operationName, List<string> selectColumns, NameValueCollection whereClause)
        {
            submitter.Verb = HttpVerb.Get;

            submitter.Uri = GetStoreApiEndpoint();
            submitter.Variables.Add("Login", store.WebUserName);
            submitter.Variables.Add("EncryptedPassword", store.ApiPassword);
            submitter.Variables.Add("API_Name", operationName);

            submitter.Variables.Add("SELECT_Columns", String.Join(",", selectColumns.ToArray()));

            // add the where clause
            foreach (string key in whereClause.Keys)
            {
                string value = whereClause[key];

                submitter.Variables.Add("WHERE_Column", key);
                submitter.Variables.Add("WHERE_Value", value);
            }
        }

        /// <summary>
        /// Get the URL of the API endpoint for the store
        /// </summary>
        private Uri GetStoreApiEndpoint()
        {
            try
            {
                string url = store.StoreUrl;

                if (!url.EndsWith("aspx", StringComparison.OrdinalIgnoreCase))
                {
                    if (!url.EndsWith("/"))
                    {
                        url += "/";
                    }

                    url += "net/WebService.aspx";
                }

                return new Uri(url);
            }
            catch (UriFormatException)
            {
                throw new VolusionException("The Volusion store url specified is not a valid url.");
            }
        }
    }
}
