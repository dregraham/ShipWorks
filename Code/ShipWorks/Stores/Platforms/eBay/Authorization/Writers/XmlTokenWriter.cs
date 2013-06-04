using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization.Writers
{
    /// <summary>
    /// An ITokenWriter implementation that will write a token in the form of an XML string.
    /// </summary>
    public class XmlTokenWriter : ITokenWriter, IDisposable
    {
        private Stream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlTokenWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public XmlTokenWriter(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        protected Stream Stream
        {
            get { return stream; }
        }

        /// <summary>
        /// Writes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        public virtual void Write(Token token)
        {
            string tokenXml = BuildTokenXml(token);

            // We don't close the stream here because the caller may still be writing to 
            // the stream after this.
            byte[] bytes = ASCIIEncoding.UTF8.GetBytes(tokenXml);
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Builds the token XML.
        /// </summary>
        /// <returns></returns>
        protected string BuildTokenXml(Token token)
        {
            const string TokenXmlFormat = @"<?xml version=""1.0"" encoding=""utf-8""?>
<eBayToken>
  <Token>{0}</Token>
  <Expiration>{1}</Expiration>
  <UserID>{2}</UserID>
</eBayToken>";

            return string.Format(TokenXmlFormat, token.Key, token.ExpirationDate.ToString("yyyy-MM-dd HH:mm:ss"), token.UserId);
        }
        
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            stream.Dispose();
            GC.SuppressFinalize(this); 
        }
    }
}
