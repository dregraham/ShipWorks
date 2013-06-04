using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Requests;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization.Repositories
{
    /// <summary>
    /// An implementation of the ITokenRepository that manages the interaction between Tango and
    /// eBay for manipulating token data.
    /// </summary>
    public class EbayTokenRepository : ITokenRepository
    {
        private IEbayWebClient webClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayTokenRepository"/> class.
        /// </summary>
        /// <param name="webClient">The web client.</param>
        public EbayTokenRepository(IEbayWebClient webClient)
        {
            this.webClient = webClient;
        }

        /// <summary>
        /// Gets the raw data about the token for the given license in a string format.
        /// </summary>
        /// <returns>A string containing the token data.</returns>
        public string GetTokenData()
        {
            // We're just going to merge/combine the results from a request to tango (to obtain 
            // the token and expiration date values) and then a request to eBay using the token/key 
            // value to obtain the user ID and returned in the form of an XML string.

            // Instantiate a tango request to obtain the token/key and expiration date XML
            XmlDocument tokenDocument = webClient.GetTangoAuthorization();
            
            // Make the request to get the user ID from eBay and append a new node to the 
            // XML string obtained from tango            
            XmlNode tokenNode = SelectTokenNode(tokenDocument, "//Token");
            GetUserResponseType userInfo = webClient.GetUserInfo(tokenNode.InnerText);
            
            // Extract the User ID from the user info and append a UserID node
            // to our token XML document
            XmlNode userNode = tokenDocument.CreateElement("UserID");
            userNode.InnerText = userInfo.User.UserID;

            // We found the eBay node, so we'll add the user node to it (and the document)
            XmlNode ebayNode = SelectTokenNode(tokenDocument, "/eBayToken");
            ebayNode.AppendChild(userNode);

            // Our token document has been completely populated with the ShipWorks/Ebay token, 
            // the expiration date, and the user ID
            return tokenDocument.OuterXml;
        }

        /// <summary>
        /// A helper method that selects a node from a token XML document and throws an EbayException
        /// when a node is not found.
        /// </summary>
        /// <param name="tokenDocument">The token document.</param>
        /// <param name="xPath">The x path.</param>
        /// <returns>An XmlNode.</returns>
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

    }
}
