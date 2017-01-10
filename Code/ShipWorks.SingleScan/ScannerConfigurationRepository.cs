using System;
using System.IO;
using System.Xml;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.Hardware.Scanner;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Repository for saving and getting the scanner name
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
        /// Save the scanner name to the registry
        /// </summary>
        public void Save(string name)
        {
            MethodConditions.EnsureArgumentIsNotNull(name, nameof(name));

            string xml = $"<Scanner><Name>{name}</Name></Scanner>";
            File.WriteAllText(fullPath, xml);
        }

        /// <summary>
        /// Get the scanner name from the registry
        /// </summary>
        public string Get()
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(File.ReadAllText(fullPath));

                return xmlDocument.SelectSingleNode("//Scanner/Name")?.InnerText ?? string.Empty;
            }
            catch (Exception ex) when (ex is FileNotFoundException || ex is XmlException)
            {
                log.Error("An error occurred while trying to read scanner settings.", ex);
                return string.Empty;
            }
        }
    }
}