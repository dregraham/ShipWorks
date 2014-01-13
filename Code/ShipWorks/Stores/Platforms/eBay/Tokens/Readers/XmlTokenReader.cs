using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ShipWorks.Stores.Platforms.Ebay.Tokens.Readers
{
    /// <summary>
    /// An ITokenReader that can read the contents an XML string to create a TokenData object. 
    /// </summary>
    public class XmlTokenReader : ITokenReader
    {
        XmlDocument tokenXmlDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlTokenReader"/> class.
        /// </summary>
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
        public EbayToken Read()
        {
            EbayToken token = new EbayToken();

            try
            {
                // We just want to query the XML document to extract the user ID, expiration date, and key
                token.Token = tokenXmlDocument.SelectSingleNode("//Token").InnerText;
                token.ExpirationDate = DateTime.Parse(tokenXmlDocument.SelectSingleNode("//Expiration").InnerText);

                // The User ID node may not be populated if we're reading from an older token file, so only 
                // set the user ID node if it exists
                XmlNode userIdNode = tokenXmlDocument.SelectSingleNode("//UserID");
                if (userIdNode != null)
                {
                    token.UserId = userIdNode.InnerText;
                }

                return token;
            }
            catch (Exception e)
            {
                throw new EbayException("Unable to read the token data.", e);
            }
        }
    }
}
