using System;
using System.Text;
using System.IO;
using Interapptive.Shared.Security;
using log4net;

namespace ShipWorks.Stores.Platforms.Ebay.Tokens.Readers
{
    /// <summary>
    /// An ITokenReader that can read the contents of an XML file that was secured/encrypted with the
    /// Interapptive.Shared.Utility.SecureText class.
    /// </summary>
    public class SecureXmlFileTokenReader : XmlFileTokenReader, ITokenReader
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SecureXmlFileTokenReader));

        // Consider moving this to an external class (SecureToken???) so it is available to writers as well
        private string saltValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureXmlFileTokenReader"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public SecureXmlFileTokenReader(FileInfo file, string saltValue)
            : base(file)
        {
            this.saltValue = saltValue;
        }

        /// <summary>
        /// Reads token data.
        /// </summary>
        /// <returns>
        /// A TokenData object that is the result of the data that was read.
        /// </returns>
        public override EbayToken Read()
        {
            try
            {
                // Read the encrypted content into a string
                string fileContent = string.Empty;
                using (StreamReader reader = new StreamReader(this.FileStream))
                {
                    fileContent = reader.ReadToEnd();
                }

                // Decrypt the content and write the decrypted content to a new stream
                string decryptedFileContent = SecureText.Decrypt(fileContent, saltValue);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    byte[] bytes = ASCIIEncoding.UTF8.GetBytes(decryptedFileContent);
                    memoryStream.Write(bytes, 0, bytes.Length);

                    // We're going to swap out the original file stream with this memory stream
                    // containing the decrypted content. The call to the base class is inside the
                    // using statement so the stream is remains open and can be read from.
                    this.FileStream = memoryStream;

                    // Now that our file stream contains the decrypted contents of the token file,
                    // we can just rely on the base class to do the rest of the work.
                    return base.Read();
                }
            }
            catch (IOException ex)
            {
                // device failure most likely
                string message = string.Format("ShipWorks was unable to read token from the file: '{0}'", this.OriginalFileName);
                log.Error(message, ex);

                throw new EbayException(message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                // No access
                string message = string.Format("ShipWorks was unable to read token from the file. Access denied to: '{0}'", this.OriginalFileName);
                log.Error(message, ex);

                // show the error
                throw new EbayException(message, ex);
            }
            catch (Exception ex)
            {
                string message = string.Format("ShipWorks could not read the token file: '{0}'", this.OriginalFileName);
                log.Error(message, ex);

                throw new EbayException(message, ex);
            }
        }
    }
}
