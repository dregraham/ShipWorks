using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Net;
using System.IO;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Authorization
{
    /// <summary>
    /// An eBay implementation of the ITangoAuthorizationRequest interface.
    /// </summary>
    public class EbayTangoAuthorizationRequest : ITangoAuthorizationRequest
    {
        private const string TangoUrlFormat = "https://www.interapptive.com/ebay/auth/token.php?action=getpendingtoken&license={0}";

        private string license;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayTangoAuthorizationRequest"/> class.
        /// </summary>
        /// <param name="license">The license.</param>
        public EbayTangoAuthorizationRequest(string license)
        {
            this.license = license;
        }

        /// <summary>
        /// Authorizes the specified license.
        /// </summary>
        /// <returns>The response to the authorization request.</returns>
        public XmlDocument Authorize()
        {
            try
            {
                // Submit the request to Tango and transform the response into an XML document
                string response = SubmitTangoRequest();
                XmlDocument tokenDocument = CreateTokenXmlDocument(response);

                // Inspect the document to make sure it contains the expected data elements
                XPathNavigator navigator = tokenDocument.CreateNavigator();
                if (navigator.Evaluate("string(//Token)").ToString().Length == 0)
                {
                    throw new EbayException("Interapptive did not receive the token content from eBay.");
                }

                if (navigator.Evaluate("string(//Expiration)").ToString().Length == 0)
                {
                    throw new EbayException("Interapptive did not receive the expiration date for the token from eBay.");
                }

                // The response contains both the token and the expiration date, so we're 
                // ready to return the XML document
                return tokenDocument;
            }
            catch (Exception ex)
            {
                throw WebHelper.TranslateWebException(ex, typeof(EbayException));
            }
        }

        /// <summary>
        /// Submits the tango request to obtain the token information.
        /// </summary>
        /// <returns>The response from the request.</returns>
        private string SubmitTangoRequest()
        {
            // Create the request Url for the given license
            string tangoUrl = string.Format(TangoUrlFormat, license);
            Uri requestUri = new Uri(tangoUrl);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.UserAgent = "shipworks";

            // Submit the request and make sure it was successful
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new EbayException(response.StatusDescription, (Exception)null);
            }

            ValidateInterapptiveCertificate(request);

            // Read and return the response to the request
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd().Trim();
            }
        }

        /// <summary>
        /// Validates the interapptive certificate.
        /// </summary>
        /// <param name="request">The request.</param>
        private void ValidateInterapptiveCertificate(HttpWebRequest request)
        {
            // Ensure the site has a valid interapptive secure certificate
            if (request.ServicePoint.Certificate.Subject.IndexOf("www.interapptive.com") == -1 ||
                request.ServicePoint.Certificate.Subject.IndexOf("Interapptive, Inc") == -1)
            {
                throw new EbayException("The SSL certificate on the server is invalid.", (Exception)null);
            }
        }

        /// <summary>
        /// Creates the token XML document.
        /// </summary>
        /// <param name="xml">The XML.</param>
        /// <returns></returns>
        private XmlDocument CreateTokenXmlDocument(string xml)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);

                // We received valid XML, so query the XML document to see if it contains an error
                XPathNavigator xpath = document.CreateNavigator();
                string error = (string)xpath.Evaluate("string(//Error/Description)");

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
