using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Repository for saving and getting the scanner name from scanner.xml in the instance settings folder
    /// </summary>
    public class ScannerConfigurationRepository : IScannerConfigurationRepository
    {
        private readonly string fullPath;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScannerConfigurationRepository(Func<Type, ILog> logFactory)
        {
            log = logFactory(typeof(ScannerConfigurationRepository));
            fullPath = Path.Combine(DataPath.InstanceSettings, "scanner.xml");
        }

        /// <summary>
        /// Save the scanner name from scanner.xml
        /// </summary>
        /// <exception cref="ScannerConfigurationRepositoryException">Throws when fails to write file to disk</exception>
        public GenericResult<string> SaveScannerName(string name)
        {
            MethodConditions.EnsureArgumentIsNotNull(name, nameof(name));

            try
            {
                using (XmlTextWriter writer = new XmlTextWriter(fullPath, Encoding.Unicode))
                {
                    writer.WriteStartElement("Scanner");
                    writer.WriteStartElement("Name");
                    writer.WriteValue(name);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

                return GenericResult.FromSuccess(string.Empty);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return GenericResult.FromError("An error occurred while attempting to save scanner name.", ex.Message);
            }
        }

        /// <summary>
        /// Clears out the scanner name from scanner.xml
        /// </summary>
        public GenericResult<string> ClearScannerName()
        {
            return SaveScannerName(string.Empty);
        }

        /// <summary>
        /// Get the scanner name from scanner.xml
        /// </summary>
        public GenericResult<string> GetScannerName()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(ReadSettingsFile());

                return
                    GenericResult.FromSuccess(xmlDocument.SelectSingleNode("//Scanner/Name")?.InnerText ?? string.Empty);
            }
            catch (Exception ex) when(ex is XPathException || ex is XmlException)
            {
                log.Error("An error occurred while trying to read scanner settings.", ex);
            }

            return GenericResult.FromSuccess(string.Empty);
        }

        /// <summary>
        /// Read the scanner.xml file from disk
        /// </summary>
        /// <returns>will return an empty string if the file does not exist or cannot be read</returns>
        private string ReadSettingsFile()
        {
            try
            {
                return File.ReadAllText(fullPath);
            }
            catch (Exception ex)
            {
                log.Error("An error occurred while trying to read scanner settings.", ex);
                return string.Empty;
            }
        }
    }
}