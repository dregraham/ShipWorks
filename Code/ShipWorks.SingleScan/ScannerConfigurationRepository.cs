using System;
using System.IO;
using System.Text;
using System.Xml;
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
        public void SaveScannerName(string name)
        {
            try
            {
                MethodConditions.EnsureArgumentIsNotNull(name, nameof(name));

                using (XmlTextWriter writer = new XmlTextWriter(fullPath, Encoding.Unicode))
                {
                    writer.WriteStartElement("Scanner");
                    writer.WriteStartElement("Name");
                    writer.WriteValue(name);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new ScannerConfigurationRepositoryException(
                    "An error occurred while attempting to save scanner name.", ex);
            }
        }

        /// <summary>
        /// Clears out the scanner name from scanner.xml
        /// </summary>
        public void ClearScannerName()
        {
            SaveScannerName(string.Empty);
        }

        /// <summary>
        /// Get the scanner name from scanner.xml
        /// </summary>
        public string GetScannerName()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(File.ReadAllText(fullPath));

                return xmlDocument.SelectSingleNode("//Scanner/Name")?.InnerText ?? string.Empty;
            }
            catch (Exception ex)
            {
                log.Error("An error occurred while trying to read scanner settings.", ex);
                return string.Empty;
            }
        }
    }
}