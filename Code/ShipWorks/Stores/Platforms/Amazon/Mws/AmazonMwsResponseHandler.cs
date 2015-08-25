using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Handels api responses from MWS API
    /// </summary>
    public static class AmazonMwsResponseHandler
    {
        /// <summary>
        /// Get an XPathNavigator for the given response
        /// </summary>
        public static XPathNamespaceNavigator GetXPathNavigator(IHttpResponseReader response, AmazonMwsApiCall apiCall, AmazonMwsWebClientSettings mwsSettings)
        {
            if(response == null) 
            {
                throw new ArgumentNullException("response");
            }

            if (mwsSettings == null)
            {
                throw new ArgumentNullException("mwsSettings");
            }

            // create an XML Document for return to the caller
            string responseXml = response.ReadResult();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(responseXml);

            // pre-load the appropriate namespace for xpath querying using the prefix "amz:"
            XPathNamespaceNavigator xpath = new XPathNamespaceNavigator(xmlDocument);
            xpath.Namespaces.AddNamespace("amz", mwsSettings.GetApiNamespace(apiCall).NamespaceName);
            
            return xpath;
        }

        /// <summary>
        /// Raise AmazonExceptions for errors returned to us in the response XML
        /// </summary>
        public static void RaiseErrors(AmazonMwsApiCall api, IHttpResponseReader reader, AmazonMwsWebClientSettings mwsSettings)
        {
            if (mwsSettings == null)
            {
                throw new ArgumentNullException("mwsSettings");
            }

            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            XNamespace ns = mwsSettings.GetApiNamespace(api);

            string responseText = reader.ReadResult();
            XDocument xdoc = XDocument.Parse(responseText);

            var error = (from e in xdoc.Descendants(ns + "Error")
                         select new
                         {
                             Code = (string)e.Element(ns + "Code"),
                             Message = (string)e.Element(ns + "Message")
                         }).FirstOrDefault();

            if (error != null)
            {
                throw new AmazonException(error.Code, error.Message, null);
            }
        }
    }
}
