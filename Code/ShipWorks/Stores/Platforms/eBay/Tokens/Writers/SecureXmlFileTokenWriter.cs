using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;

namespace ShipWorks.Stores.Platforms.Ebay.Tokens.Writers
{
    /// <summary>
    /// An ITokenWriter implementation that will write a token data to a file in the form of an encrypted XML string.
    /// </summary>
    public class SecureXmlFileTokenWriter : ITokenWriter
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SecureXmlFileTokenWriter));

        private FileInfo originalFile;
        private string saltValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecureXmlFileTokenWriter"/> class.
        /// </summary>
        public SecureXmlFileTokenWriter(FileInfo file, string saltValue)
        {
            originalFile = file;
            this.saltValue = saltValue;
        }

        /// <summary>
        /// Writes the specified token.
        /// </summary>
        public void Write(EbayToken token)
        {
            try
            {
                Stream stream = originalFile.OpenWrite();
                using (SecureXmlTokenWriter writer = new SecureXmlTokenWriter(stream, saltValue))
                {
                    writer.Write(token);
                }
            }
            catch (IOException ex)
            {
                // device failure most likely
                string message = string.Format("ShipWorks was unable to write token to the file: '{0}'", originalFile.Name);
                log.Error(message, ex);

                throw new EbayException(message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                // No access
                string message = string.Format("ShipWorks was unable to write token from the file. Access denied to: '{0}'", originalFile.Name);
                log.Error(message, ex);

                // show the error
                throw new EbayException(message, ex);
            }
            catch (Exception ex)
            {
                string message = string.Format("ShipWorks could not write the token file: '{0}'", originalFile.Name);
                log.Error(message, ex);

                throw new EbayException(message, ex);
            }
        }
    }
}
