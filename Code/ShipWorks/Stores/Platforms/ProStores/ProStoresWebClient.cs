﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;

namespace ShipWorks.Stores.Platforms.ProStores
{
    /// <summary>
    /// WebClient for connecting to ProStores
    /// </summary>
    [SuppressMessage("CSharp.Analyzers",
        "CA5351: Do not use insecure cryptographic algorithm MD5",
        Justification = "This is what ProStores currently uses")]
    public static class ProStoresWebClient
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ProStoresWebClient));

        const string appID = "fa6af8fd9e63ffcd7ac86125eceefa37";
        const string secret = "e1d82dbf4ef9a0bb189806e3bf18770c";

        const int orderPageSize = 50;

        // Extracts the rest name\verb from a URL
        static Regex restActionRegex = new Regex("/([^/]+?/[^/]+?)$", RegexOptions.Compiled);

        class TaggedXmlTextWriter : XmlTextWriter
        {
            public TaggedXmlTextWriter(StreamWriter writer) : base(writer) { }
            public object Tag { get; set; }
        }

        /// <summary>
        /// Get the store's API URL info from the given entry point
        /// </summary>
        public static XmlDocument GetStoreApiInfo(string apiEntryPoint)
        {
            try
            {
                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                request.Uri = new Uri(apiEntryPoint);

                return ProcessRestRequest(request);
            }
            catch (XmlException ex)
            {
                throw new ProStoresException("The API Entry Point URL is not valid.", ex);
            }
            catch (UriFormatException ex)
            {
                throw new ProStoresException("The API Entry Point URL is not valid.", ex);
            }
        }

        /// <summary>
        /// Get an auth ticket and use it to create the correct API Logon URL
        /// </summary>
        public static string CreateApiLogonUrl(ProStoresStoreEntity store, out string ticket)
        {
            HttpVariableRequestSubmitter ticketRequest = new HttpVariableRequestSubmitter();
            ticketRequest.Uri = new Uri(GetRestUrl(store, "auth", "getTicket"));

            XmlDocument ticketResponse = ProcessRestRequest(ticketRequest);
            ticket = ticketResponse.SelectSingleNode("//Ticket").InnerText;
            string ticketTimestamp = ticketResponse.SelectSingleNode("//Timestamp").InnerText;

            HttpVariableRequestSubmitter logonUrl = new HttpVariableRequestSubmitter();
            logonUrl.Variables.Add("ts", ticketTimestamp);
            logonUrl.Variables.Add("acl", GetRestDesiredAcl());
            logonUrl.Variables.Add("ticket", ticket);

            PrepareRestRequestVariables(logonUrl);

            return string.Format("{0}?{1}", store.ApiTokenLogonUrl, QueryStringUtility.GetQueryString(logonUrl.Variables, QueryStringEncodingCasing.Upper));
        }

        /// <summary>
        /// Gets the token that the user generated by authorizing at the ApiLogonUrl for the given ticket
        /// </summary>
        public static XmlDocument GetTokenFromTicket(ProStoresStoreEntity store, string ticket)
        {
            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
            request.Uri = new Uri(GetRestUrl(store, "auth", "getToken"));
            request.Variables.Add("ticket", ticket);

            return ProcessRestRequest(request);
        }

        /// <summary>
        /// If not already using tokens, or not already known, checks to see if it's possible to use tokens to login to the store.
        /// </summary>
        public static void CheckTokenLoginMethodAvailability(ProStoresStoreEntity store)
        {
            if (store.LoginMethod == (int) ProStoresLoginMethod.ApiToken)
            {
                return;
            }

            if (store.LegacyCanUpgrade)
            {
                return;
            }

            string url = GetXteUrl(store);

            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();

                string versionHeader = response.Headers["X-StoreSense"];
                Version storeVersion = new Version(versionHeader);

                if (storeVersion >= new Version(8, 2))
                {
                    store.LegacyCanUpgrade = true;

                    using (SqlAdapter adapter = new SqlAdapter())
                    {
                        adapter.SaveAndRefetch(store);
                    }
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ProStoresException));
            }
        }

        /// <summary>
        /// Test the connection to the XTE system for the given store
        /// </summary>
        public static void TestXteConnection(ProStoresStoreEntity store)
        {
            XmlTextWriter xmlRequest = CreateXteRequest(store, "StoreStats");

            xmlRequest.WriteElementString("Action", "Query");
            xmlRequest.WriteStartElement("Query");
            xmlRequest.WriteStartElement("Select");
            xmlRequest.WriteElementString("CustomerCount", "");
            xmlRequest.WriteEndElement();
            xmlRequest.WriteEndElement();

            ProcessXteRequest(xmlRequest, "TestConnection");
        }

        /// <summary>
        /// Get the number of online orders for the store after the given date
        /// </summary>
        public static int GetOrderCount(ProStoresStoreEntity store, DateTime? lastModified)
        {
            XmlTextWriter xmlRequest = CreateXteRequest(store, "Invoice");

            xmlRequest.WriteElementString("Action", "Query");
            xmlRequest.WriteStartElement("Query");
            xmlRequest.WriteAttributeString("start", "1");
            xmlRequest.WriteAttributeString("count", "1");

            xmlRequest.WriteStartElement("Select");
            xmlRequest.WriteElementString("InvoiceNumber", "");
            xmlRequest.WriteEndElement();

            WriteInvoiceWhereElements(xmlRequest, lastModified);

            xmlRequest.WriteEndElement();

            XmlDocument response = ProcessXteRequest(xmlRequest, "GetOrderCount");

            int count = XPathUtility.Evaluate(response.CreateNavigator(), "//QueryStats/Total", -1);

            // Store does not support QueryStats\Total
            if (count == -1)
            {
                return GetOrderCountLegacy(store, lastModified);
            }

            return count;
        }

        /// <summary>
        /// Get the number of online orders for the store after the given date
        /// </summary>
        private static int GetOrderCountLegacy(ProStoresStoreEntity store, DateTime? lastModified)
        {
            XmlTextWriter xmlRequest = CreateXteRequest(store, "Invoice");

            xmlRequest.WriteElementString("Action", "Query");
            xmlRequest.WriteStartElement("Query");

            xmlRequest.WriteStartElement("Select");
            xmlRequest.WriteElementString("InvoiceNumber", "");
            xmlRequest.WriteEndElement();

            WriteInvoiceWhereElements(xmlRequest, lastModified);

            xmlRequest.WriteEndElement();

            XmlDocument response = ProcessXteRequest(xmlRequest, "GetOrderCount");

            return response.SelectNodes("/XTE/Response/Invoice").Count;
        }

        /// <summary>
        /// Get the next page of orders with LastModified dates greater than the specified date
        /// </summary>
        public static XmlDocument GetNextOrderPage(ProStoresStoreEntity store, DateTime? lastModified, bool proVersion)
        {
            XmlTextWriter xmlRequest = CreateXteRequest(store, "Invoice");

            xmlRequest.WriteElementString("Action", "Query");
            xmlRequest.WriteStartElement("Query");
            xmlRequest.WriteAttributeString("start", "1");
            xmlRequest.WriteAttributeString("count", "50");

            xmlRequest.WriteStartElement("Select");
            WriteInvoiceSelectElements(xmlRequest, proVersion, store.LoginMethod == (int) ProStoresLoginMethod.ApiToken);
            xmlRequest.WriteEndElement();

            WriteInvoiceWhereElements(xmlRequest, lastModified);

            // Ordered
            xmlRequest.WriteStartElement("OrderBy");
            xmlRequest.WriteStartElement("LastModifiedDate");
            xmlRequest.WriteAttributeString("direction", "asc");
            xmlRequest.WriteEndElement();
            xmlRequest.WriteEndElement();

            xmlRequest.WriteEndElement();

            return ProcessXteRequest(xmlRequest, "GetOrders");
        }

        /// <summary>
        /// Write all the invoice elements we need to select for downloading to shipworks.  newerApi means 8.2+
        /// </summary>
        private static void WriteInvoiceSelectElements(XmlTextWriter xmlRequest, bool proVersion, bool newerApi)
        {
            string selectXml = @"

                <InvoiceNumber />
                <EnterDate timezone='GMT' />
                <LastModifiedDate timezone='GMT' />

                <AuthorizationBy/>
                <AuthorizationDate timezone='GMT' />
                <Authorized/>

                <Confirmed />
                <ConfirmationNumber />

                <FirstName />
                <LastName />
                <BillToCompany />
                <BillToStreet />
                <BillToStreet2 />
                <BillToDistrict />
                <BillToCity />
                <BillToState />
                <BillToPostalCode />
                <BillToCountry />
                <BillToPhone />
                <Email />
                <Customer>
                   <Email />
                   <CustomerNumberFull />
                </Customer>

                <Recipient />
                <Company />
                <ShipToStreet />
                <ShipToStreet2 />
                <ShipToDistrict />
                <ShipToCity />
                <ShipToState />
                <ShipToPostalCode />
                <County />
                <ShipToCountry />
                <ShipToPhone />

                <PaymentMethod />
                <CreditCard />
                <CreditCardExpiration />
                <CreditCardHolderName />
                <MaskedCreditCardNumber />

                <SelectedShippingQuote />
                <ShipMethodDescription />
                <ShipDate/>

                <Status />
                <Note />
                <OrderNote />

                <PromotionCode />
                <PromotionDiscount />
                <Tax />
                <Shipping />
                <Total />
            ";

            selectXml += "<Message />";
            if (proVersion) selectXml += "<GiftWrapEnabled />";

            selectXml += @"
                <Details>
                    <Name />
                    <Quantity />
                    <SKU />
                    <PriceBeforeTax />
                    <Weight />
                    <IsBackordered />
                    ";

            if (newerApi)
            {
                selectXml += @"
                    <AttributeText1 />
                    <AttributeText2 />
                    <AttributeText3 />
                    <AttributeText4 />
                    <AttributeText5 />
                    <AttributeText6 />
                    <AttributeText7 />
                    <AttributeText8 />
                ";
            }
            else
            {
                selectXml += @"
                    <ProductColor />
                    <ProductSize />";
            }

            selectXml += @"
                    <Product>

                        <ProductNumber />
                        <Description/>
                        <SKU />
                        <Isbn />
                        <Upc />
                        <MarketplaceDisplayImageUrl />

                        <Attribute1Label />
                        <Attribute2Label />";

            if (newerApi)
            {
                selectXml += @"
                        <Attribute3Label />
                        <Attribute4Label />
                        <Attribute5Label />
                        <Attribute6Label />
                        <Attribute7Label />
                        <Attribute8Label />
                   ";
            }

            if (proVersion)
            {
                selectXml +=
                    "<Cost />";
            }

            selectXml += @"
                    </Product>";

            if (proVersion)
            {
                selectXml += @"
                    <ProductAttribute>
                        <Cost />
                    </ProductAttribute>";
            }

            selectXml +=
                "</Details>";

            xmlRequest.WriteRaw(selectXml);
        }

        /// <summary>
        /// Write all the elements for the "Where" condition of the invoice select
        /// </summary>
        private static void WriteInvoiceWhereElements(XmlTextWriter xmlRequest, DateTime? lastModified)
        {
            xmlRequest.WriteStartElement("Where");

            if (lastModified != null)
            {
                xmlRequest.WriteStartElement("LastModifiedDate");
                xmlRequest.WriteAttributeString("operator", ">");
                xmlRequest.WriteString(lastModified.Value.ToString("s") + "+00:00");
                xmlRequest.WriteEndElement();
            }

            // No abandoned invoices
            xmlRequest.WriteStartElement("InvoiceNumber");
            xmlRequest.WriteAttributeString("operator", ">");
            xmlRequest.WriteString("0");
            xmlRequest.WriteEndElement();

            xmlRequest.WriteEndElement();
        }

        /// <summary>
        /// Upload the shipment details of the given shipment for the specified ProStores store
        /// </summary>
        public static void UploadShipmentDetails(ProStoresStoreEntity store, ShipmentEntity shipment)
        {
            if (shipment.Order.IsManual)
            {
                log.WarnFormat("Not uploading shipment details since order {0} is manual.", shipment.Order.OrderID);
                return;
            }

            string trackingNumber = shipment.TrackingNumber;

            // Adjust tracking details per Mail Innovations and others
            WorldShipUtility.DetermineAlternateTracking(shipment, (track, service) =>
                {
                    if (track.Length > 0)
                    {
                        trackingNumber = track;
                    }
                });

            ProStoresOrderEntity order = (ProStoresOrderEntity) shipment.Order;

            HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();

            request.Uri = new Uri(GetRestUrl(store, string.Format("Invoices/{0}", order.OrderNumber), "Ship"));

            request.Variables.Add("token", store.ApiToken);
            request.Variables.Add("shipdate", shipment.ShipDate.ToString("MM/dd/yy"));

            // I have seen it all 3 ways in the docs.  I know 'trackno' works for our store - but maybe the others work on different platforms?
            request.Variables.Add("trackno", trackingNumber);
            request.Variables.Add("tracking.number", trackingNumber);
            request.Variables.Add("tracking_number", trackingNumber);

            ProcessRestRequest(request);
        }

        /// <summary>
        /// Process the given request.
        /// </summary>
        private static XmlDocument ProcessRestRequest(HttpVariableRequestSubmitter request)
        {
            request.Verb = HttpVerb.Get;

            PrepareRestRequestVariables(request);

            string restAction = restActionRegex.Match(request.Uri.AbsolutePath).Groups[1].Value.Replace("/", "_");

            // Log the query string we are sending
            ApiLogEntry logEntry = new ApiLogEntry(ApiLogSource.ProStores, restAction);
            logEntry.LogRequest(request);

            return ExecuteRequest(request, logEntry);
        }

        /// <summary>
        /// Get the REST url to execute the given action name and verb for the given store
        /// </summary>
        private static string GetRestUrl(ProStoresStoreEntity store, string name, string verb)
        {
            string baseUrl = store.ApiRestSecureUrl;
            if (!baseUrl.EndsWith("/"))
            {
                baseUrl += "/";
            }

            return string.Format("{0}{1}/{2}{3}", baseUrl, name, verb, store.ApiRestScriptSuffix);
        }

        /// <summary>
        /// Create an XmlWriter initialized with the header for a request for the given store
        /// </summary>
        [NDependIgnoreLongMethod]
        private static XmlTextWriter CreateXteRequest(ProStoresStoreEntity store, string requestType)
        {
            StreamWriter writer = new StreamWriter(new MemoryStream(), StringUtility.Iso8859Encoding);

            TaggedXmlTextWriter xmlWriter = new TaggedXmlTextWriter(writer);
            xmlWriter.Formatting = Formatting.Indented;
            xmlWriter.Tag = store;

            string xteVersion = (store.LoginMethod == (int) ProStoresLoginMethod.ApiToken) ? "3.0" : "2.1";

            // Open
            xmlWriter.WriteStartDocument();

            // Open document
            xmlWriter.WriteStartElement("XTE");
            xmlWriter.WriteAttributeString("local", "");
            xmlWriter.WriteAttributeString("version", xteVersion);

            // Client name
            xmlWriter.WriteStartElement("Client");
            xmlWriter.WriteElementString("Name", "interapptive - 10001");
            xmlWriter.WriteEndElement();

            // Write the logon fields
            if (store.LoginMethod == (int) ProStoresLoginMethod.LegacyUserPass)
            {
                xmlWriter.WriteStartElement("Logon");

                xmlWriter.WriteElementString("Zone", "store");
                xmlWriter.WriteElementString("ShortName", store.ShortName);
                xmlWriter.WriteElementString("UserName", store.Username);
                xmlWriter.WriteElementString("Password", SecureText.Decrypt(store.LegacyPassword, store.Username));

                xmlWriter.WriteEndElement();
            }
            else
            {
                xmlWriter.WriteStartElement("Logon");

                HttpVariableRequestSubmitter request = new HttpVariableRequestSubmitter();
                request.Variables.Add("token", store.ApiToken);
                PrepareRestRequestVariables(request);

                xmlWriter.WriteElementString("AppId", appID);
                xmlWriter.WriteElementString("Timestamp", request.Variables.Single(v => v.Name == "ts").Value);
                xmlWriter.WriteElementString("Token", store.ApiToken);
                xmlWriter.WriteElementString("Signature", request.Variables.Single(v => v.Name == "sig").Value);

                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteStartElement("Request");
            xmlWriter.WriteAttributeString("type", requestType);
            xmlWriter.WriteAttributeString("version", xteVersion);

            return xmlWriter;
        }

        /// <summary>
        /// Process the given request
        /// </summary>
        private static XmlDocument ProcessXteRequest(XmlTextWriter xmlWriter, string logName)
        {
            ProStoresStoreEntity store = (ProStoresStoreEntity) ((TaggedXmlTextWriter) xmlWriter).Tag;

            // Close out the XML
            xmlWriter.WriteEndDocument();
            xmlWriter.Flush();

            // We create it as a memory stream, so we know that's what it is
            MemoryStream stream = (MemoryStream) xmlWriter.BaseStream;

            // We need it as a string so we can log it
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream, StringUtility.Iso8859Encoding);
            string requestXml = reader.ReadToEnd();

            // Log the request
            ApiLogEntry logger = new ApiLogEntry(ApiLogSource.ProStores, logName);
            logger.LogRequest(requestXml);

            HttpBinaryPostRequestSubmitter request = new HttpBinaryPostRequestSubmitter(stream.ToArray(), "text/xml");
            request.Uri = new Uri(GetXteUrl(store));

            // Get the response
            return ExecuteRequest(request, logger);
        }

        /// <summary>
        /// Execute the given request
        /// </summary>
        private static XmlDocument ExecuteRequest(HttpRequestSubmitter request, ApiLogEntry logEntry)
        {
            try
            {
                using (IHttpResponseReader response = request.GetResponse())
                {
                    if (!response.HttpWebResponse.ContentType.ToUpper(CultureInfo.InvariantCulture).Contains("TEXT/XML"))
                    {
                        // Sometimes ProStores responds with the HTML of their maintenance page
                        log.Error(string.Format("The request to ProStores could not be processed. ProStores returned a response with an unexpected content type: {0}", response.HttpWebResponse.ContentType));
                        throw new ProStoresException("ShipWorks encountered an unexpected response from ProStores that could not be processed. Please try again later.");
                    }

                    string responseXml = response.ReadResult();

                    // Log the response
                    logEntry.LogResponse(responseXml);

                    // Load the response and return it
                    XmlDocument xmlResponse = new XmlDocument();
                    xmlResponse.LoadXml(responseXml);

                    CheckForErrors(xmlResponse);

                    return xmlResponse;
                }
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(ProStoresException));
            }
        }

        /// <summary>
        /// Check the xml document for errors.
        /// </summary>
        private static void CheckForErrors(XmlDocument xmlResponse)
        {
            XPathNavigator xpath = xmlResponse.CreateNavigator();

            // Determine if an error occurred
            string errorCode = XPathUtility.Evaluate(xpath, "//Outcome", "Failure");

            // Error occurred
            if (errorCode != "Success")
            {
                XPathNavigator error = xpath.SelectSingleNode("//Errors/Error");

                if (error != null)
                {
                    throw new ProStoresApiException(error.Value, error.GetAttribute("type", ""));
                }
            }
        }

        /// <summary>
        /// Get the URL for xte processing
        /// </summary>
        private static string GetXteUrl(ProStoresStoreEntity store)
        {
            if (store.LoginMethod == (int) ProStoresLoginMethod.ApiToken)
            {
                return store.ApiXteUrl;
            }
            else
            {
                string url = store.LegacyAdminUrl.Trim();

                if (url.IndexOf(Uri.SchemeDelimiter) == -1)
                {
                    url = "https://" + url;
                }

                if (!url.ToLower().StartsWith("https://"))
                {
                    throw new ProStoresException("The ProStores host name must use a secure https connection.");
                }

                string xte = store.LegacyXtePath.Trim();
                string prefix = store.LegacyPrefix.Trim();

                // No prefix, its just Admin URL + XTE Url
                if (prefix.Length == 0)
                {
                    if (!url.EndsWith("/"))
                    {
                        url += "/";
                    }

                    if (xte[0] == '/')
                    {
                        xte = xte.Substring(1, xte.Length - 1);
                    }

                    url = url + xte;
                }
                // With a prefix, we have to do some crap
                else
                {
                    Uri baseUri = new Uri(url);

                    // We need from "xte" over on the xte url
                    int xteIndex = xte.IndexOf("xte");

                    if (xteIndex == -1)
                    {
                        throw new ProStoresException("The value 'xte' was not found in the XTE Process URL.");
                    }

                    xte = xte.Substring(xteIndex, xte.Length - xteIndex);

                    if (prefix.StartsWith("/"))
                    {
                        prefix = prefix.Substring(1, prefix.Length - 1);
                    }

                    if (prefix.EndsWith("/"))
                    {
                        prefix = prefix.Substring(0, prefix.Length - 1);
                    }

                    // We need the base host + prefx + mofifiex xte
                    url = "https://" + baseUri.Host + "/" + prefix + "/" + xte;
                }

                return url;
            }
        }

        /// <summary>
        /// Add required boilerplate parameters to the given rest request
        /// </summary>
        private static void PrepareRestRequestVariables(HttpVariableRequestSubmitter request)
        {
            request.VariableEncodingCasing = QueryStringEncodingCasing.Upper;

            request.Variables.Add("appid", appID);

            // The request for building the API Login URL adds a timestamp from a return call
            if (!request.Variables.Any(v => v.Name == "ts"))
            {
                request.Variables.Add("ts", GetRestTimestamp());
            }

            string signature = GetRestSignature(request);
            request.Variables.Add("sig", signature);
        }

        /// <summary>
        /// Get the REST authentication signature to append to the ProStores REST URL
        /// </summary>
        private static string GetRestSignature(HttpVariableRequestSubmitter request)
        {
            List<string> signList = new List<string>
                {
                    "appid",
                    "ts",
                    "acl",
                    "ticket",
                    "token"
                };

            // The signature must be generated by the correct ordered parameters.
            List<HttpVariable> variables = request.Variables
                .Where(v => signList.Contains(v.Name))
                .OrderBy(v => signList.IndexOf(v.Name))
                .ToList();

            // Re-order the variables in the original request
            foreach (HttpVariable variable in variables)
            {
                request.Variables.Remove(variable);
                request.Variables.Insert(variables.IndexOf(variable), variable);
            }

            string queryString = QueryStringUtility.GetQueryString(variables, QueryStringEncodingCasing.Upper);

            // We add on the secret for hash computation
            string withSecret = secret + "&" + queryString;

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(withSecret));

            StringBuilder hex = new StringBuilder(hash.Length * 2);
            foreach (byte b in hash)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }

        /// <summary>
        /// Get the given date as a unix epoch time.
        /// </summary>
        private static string GetRestTimestamp()
        {
            return ((int) ((TimeSpan) (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified))).TotalSeconds).ToString();
        }

        /// <summary>
        /// Get the ACL we request
        /// </summary>
        private static string GetRestDesiredAcl()
        {
            return
                "customer.mgr.view," +
                "store.mgr.view," +
                "order.mgr.view," +
                "order.mgr.auth," +
                "order.mgr.entry," +
                "product.mgr.view," +
                "shipping.mgr.view"
                ;
        }
    }
}
