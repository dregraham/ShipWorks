using System.Text;
using System.IO;
using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.Ebay.Tokens.Writers
{
    /// <summary>
    /// An ITokenWriter implementation that will write a token in the form of an encrypted XML string.
    /// </summary>
    public class SecureXmlTokenWriter : XmlTokenWriter, ITokenWriter
    {
        private string saltValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureXmlTokenWriter"/> class.
        /// </summary>
        public SecureXmlTokenWriter(Stream stream, string saltValue)
            : base(stream)
        {
            this.saltValue = saltValue;
        }

        /// <summary>
        /// Writes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        public override void Write(EbayToken token)
        {
            // We're going to leverage the method from the base class to build the xml
            // and simply encrypt it before writing it to the stream.
            string tokenXml = BuildTokenXml(token);
            string encryptedTokenXml = SecureText.Encrypt(tokenXml, saltValue);

            byte[] bytes = ASCIIEncoding.UTF8.GetBytes(encryptedTokenXml);
            this.Stream.Write(bytes, 0, bytes.Length);
        }
    }
}
