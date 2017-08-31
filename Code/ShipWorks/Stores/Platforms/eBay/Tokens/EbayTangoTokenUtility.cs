﻿using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.XPath;
using Autofac;
using Interapptive.Shared.Net;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Stores.Platforms.Ebay.Tokens
{
    /// <summary>
    /// Responsible for retrieving tokens that have been generated by eBay and sent to tango
    /// </summary>
    public class EbayTangoTokenUtility
    {
        private const string TangoUrlFormat = "https://www.interapptive.com/ebay/auth/token.php?action=getpendingtoken&license={0}";

        string lookupKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayTangoTokenUtility"/> class.
        /// </summary>
        public EbayTangoTokenUtility()
        {
            this.lookupKey = ShipWorksSession.InstanceID.ToString();
        }

        /// <summary>
        /// Gets the raw data about the token for the given license in a string format.
        /// </summary>
        public EbayToken GetTokenData()
        {
            // We're just going to merge/combine the results from a request to tango (to obtain
            // the token and expiration date values) and then a request to eBay using the token/key
            // value to obtain the user ID and returned in the form of an XML string.

            // Instantiate a tango request to obtain the token/key and expiration date XML
            XmlDocument tokenDocument = SubmitTangoRequest();

            // Extract the token
            EbayToken token = new EbayToken
            {
                Token = SelectTokenNode(tokenDocument, "//Token").InnerText
            };

            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                // Get the user info
                IEbayWebClient webClient = lifetimeScope.Resolve<IEbayWebClient>();
                token.UserId = webClient.GetUser(token).UserID;

                // Set the expiration
                token.ExpirationDate = DateTime.Parse(SelectTokenNode(tokenDocument, "//Expiration").InnerText);

                return token;
            }
        }

        /// <summary>
        /// A helper method that selects a node from a token XML document and throws an EbayException when a node is not found.
        /// </summary>
        private XmlNode SelectTokenNode(XmlDocument tokenDocument, string xPath)
        {
            XmlNode node = tokenDocument.SelectSingleNode(xPath);

            if (node == null)
            {
                string message = string.Format("ShipWorks was unable to find the {0} node in the token XML: {1}", xPath, tokenDocument.OuterXml);
                throw new EbayException(message);
            }

            return node;
        }

        /// <summary>
        /// Submits the tango request to obtain the token information.
        /// </summary>
        private XmlDocument SubmitTangoRequest()
        {
            try
            {
                // Create the request Url for the given license
                string tangoUrl = string.Format(TangoUrlFormat, lookupKey);
                Uri requestUri = new Uri(tangoUrl);

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUri);
                request.UserAgent = "shipworks";

                // Submit the request and make sure it was successful
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new EbayException(response.StatusDescription, (Exception) null);
                }

                ValidateInterapptiveCertificate(request);

                string xmlResponse;

                // Read and return the response to the request
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    xmlResponse = reader.ReadToEnd().Trim();
                }

                // transform the response into an XML document
                XmlDocument tokenDocument = CreateTokenXmlDocument(xmlResponse);

                // The response contains both the token and the expiration date, so we're ready to return the XML document
                return tokenDocument;
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EbayException));
            }
        }

        /// <summary>
        /// Validates the interapptive certificate.
        /// </summary>
        private void ValidateInterapptiveCertificate(HttpWebRequest request)
        {
            // Ensure the site has a valid interapptive secure certificate
            if (request.ServicePoint.Certificate.Subject.IndexOf("www.interapptive.com") == -1 ||
                request.ServicePoint.Certificate.Subject.IndexOf("Interapptive, Inc") == -1)
            {
                throw new EbayException("The SSL certificate on the server is invalid.", (Exception) null);
            }
        }

        /// <summary>
        /// Creates the token XML document.
        /// </summary>
        private XmlDocument CreateTokenXmlDocument(string xml)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);

                // We received valid XML, so query the XML document to see if it contains an error
                XPathNavigator xpath = document.CreateNavigator();
                string error = (string) xpath.Evaluate("string(//Error/Description)");

                if (error.Length > 0)
                {
                    // The XML contained an error so throw an eBay exception
                    throw new EbayException(error);
                }

                return document;
            }
            catch (XmlException e)
            {
                throw new EbayException("The token content is not well-formed", e);
            }
        }
    }
}
