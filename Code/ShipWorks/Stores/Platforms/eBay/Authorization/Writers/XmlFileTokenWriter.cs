using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using log4net;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization.Writers
{
    /// <summary>
    /// An ITokenWriter implementation that will write a token data in XML format to a file.
    /// </summary>
    public class XmlFileTokenWriter : ITokenWriter
    {
        static readonly ILog log = LogManager.GetLogger(typeof(XmlFileTokenWriter));

        private FileInfo originalFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFileTokenWriter"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public XmlFileTokenWriter(FileInfo file)
        {
            originalFile = file;
        }
        
        /// <summary>
        /// Writes the specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        public void Write(Token token)
        {
            try
            {
                Stream stream = originalFile.OpenWrite();
                using (XmlTokenWriter writer = new XmlTokenWriter(stream))
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
