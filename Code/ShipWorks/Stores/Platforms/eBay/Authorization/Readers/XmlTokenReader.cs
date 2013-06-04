using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization.Readers
{
    /// <summary>
    /// An ITokenReader that can read the contents an XML string to create a TokenData object. 
    /// </summary>
    public class XmlTokenReader : ITokenReader
    {
        private XmlDocument tokenXmlDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlTokenReader"/> class.
        /// </summary>
        /// <param name="tokenXml">The token XML.</param>
        public XmlTokenReader(XmlDocument tokenXml)
        {
            tokenXmlDocument = tokenXml;
        }

        /// <summary>
        /// Reads token data.
        /// </summary>
        /// <returns>
        /// A TokenData object that is the result of the data that was read.
        /// </returns>
        public TokenData Read()
        {
            TokenData tokenData = new TokenData();

            try
            {
                // We just want to query the XML document to extract the user ID, expiration date, and key
                tokenData.Key = tokenXmlDocument.SelectSingleNode("//Token").InnerText;
                tokenData.ExpirationDate = DateTime.Parse(tokenXmlDocument.SelectSingleNode("//Expiration").InnerText);

                // The User ID node may not be populated if we're reading from an older token file, so only 
                // set the user ID node if it exists
                XmlNode userIdNode = tokenXmlDocument.SelectSingleNode("//UserID");
                if (userIdNode != null)
                {
                    tokenData.UserId = userIdNode.InnerText;
                }

                return tokenData;
            }
            catch (Exception e)
            {
                throw new EbayException("Unable to read the token data.", e);
            }
        }
    }
}
