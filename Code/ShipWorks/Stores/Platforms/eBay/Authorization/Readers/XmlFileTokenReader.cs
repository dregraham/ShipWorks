using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using log4net;

namespace ShipWorks.Stores.Platforms.Ebay.Authorization.Readers
{
    /// <summary>
    /// An ITokenReader that can read the contents of an XML file to create a TokenData object.
    /// </summary>
    public class XmlFileTokenReader : ITokenReader, IDisposable
    {
        static readonly ILog log = LogManager.GetLogger(typeof(XmlFileTokenReader));

        private Stream fileStream;
        private FileInfo originalFile; 

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlFileTokenReader"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public XmlFileTokenReader(FileInfo file)
        {
            try
            {
                originalFile = file;
                fileStream = file.OpenRead();                
            }
            catch (Exception e)
            {
                string message = string.Format("Unable to read token data from token file: {0}", file.FullName);
                throw new EbayException(message, e);
            }
        }

        /// <summary>
        /// Gets the file stream.
        /// </summary>
        protected Stream FileStream
        {
            get { return fileStream; }
            set { fileStream = value; }
        }

        /// <summary>
        /// Gets the name of the original file.
        /// </summary>
        /// <value>
        /// The name of the original file.
        /// </value>
        protected string OriginalFileName
        {
            get { return originalFile.Name; }
        }
        
        /// <summary>
        /// Reads token data.
        /// </summary>
        /// <returns>
        /// A TokenData object that is the result of the data that was read.
        /// </returns>
        public virtual TokenData Read()
        {
            try
            {
                // Make sure we're at the beginning of the stream, and read the file 
                // contents of the file into an Xml document
                fileStream.Seek(0, SeekOrigin.Begin);
                XmlDocument tokenXmlDocument = new XmlDocument();

                using (StreamReader streamReader = new StreamReader(this.FileStream))
                {
                    tokenXmlDocument.Load(streamReader);
                }

                // We have an Xml document, so now we can just re-use the functionality 
                // of the token Xml reader to extract the token data
                XmlTokenReader reader = new XmlTokenReader(tokenXmlDocument);
                return reader.Read();
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
                string message = string.Format("ShipWorks was unable to access the token file. Access denied to: '{0}'", this.OriginalFileName);
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            fileStream.Dispose();
            GC.SuppressFinalize(this); 
        }
    }
}
