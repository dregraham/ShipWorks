using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using ShipWorks.Stores.Platforms.Ebay.Authorization.Readers;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Repositories;
using ShipWorks.Stores.Platforms.Ebay.Authorization.Writers;
using ShipWorks.Stores.Platforms.Ebay.Requests;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization
{
    /// <summary>
    /// An implementation of the ITokenFactory that is specific to the needs of the eBay token/authorization. 
    /// </summary>
    public class EbayTokenFactory : ITokenFactory
    {
        // We're using secure readers/writers in this factory, so we need to have a salt value
        // for the encryption/decryption process
        public const string SaltValue = "token";

        /// <summary>
        /// Creates an ITokenReader reading a string of token data.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        /// <returns>
        /// An ITokenReader object.
        /// </returns>
        public ITokenReader CreateReader(string tokenData)
        {
            try
            {
                XmlDocument tokenDocument = new XmlDocument();
                tokenDocument.LoadXml(tokenData);

                return new XmlTokenReader(tokenDocument);
            }
            catch (Exception e)
            {
                string message = string.Format("Unable to create a reader for the token data provider: {0}", tokenData);
                throw new EbayException(message, e);
            }
        }

        /// <summary>
        /// Creates an ITokenReader capable of reading from a file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// An ITokenReader object.
        /// </returns>
        public ITokenReader CreateReader(System.IO.FileInfo file)
        {
            return new SecureXmlFileTokenReader(file, SaltValue);
        }

        /// <summary>
        /// Creates the writer.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <returns>
        /// An ITokenWriter object.
        /// </returns>
        public ITokenWriter CreateWriter(System.IO.Stream stream)
        {
            return new SecureXmlTokenWriter(stream, SaltValue);
        }

        /// <summary>
        /// Creates the writer.
        /// </summary>
        /// <param name="file">The file to write to.</param>
        /// <returns>
        /// An ITokenWriter object
        /// </returns>
        public ITokenWriter CreateWriter(System.IO.FileInfo file)
        {
            return new SecureXmlFileTokenWriter(file, SaltValue);
        }

        /// <summary>
        /// Creates the repository.
        /// </summary>
        /// <returns>
        /// An ITokenRepository object
        /// </returns>
        public ITokenRepository CreateRepository(string license)
        {
            // We want to hit the actual Tango/eBay sites with this factory, so we are 
            // going use the "real" eBay web client to create our repository.
            return new EbayTokenRepository(new EbayWebClient());
        }                
    }
}
