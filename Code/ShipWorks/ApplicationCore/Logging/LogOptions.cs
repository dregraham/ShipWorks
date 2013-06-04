using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Xml.XPath;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore.Logging
{
    /// <summary>
    /// Represents all logging options
    /// </summary>
    public class LogOptions
    {
        // What to log
        bool logShipWorks = true;
        bool logServices = true;

        // How many days logs are left behind
        int maxLogAgeDays = 7;

        /// <summary>
        /// Constructor
        /// </summary>
        public LogOptions()
        {

        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public LogOptions(LogOptions copyFrom)
        {
            if (copyFrom == null)
            {
                throw new ArgumentNullException("copyFrom");
            }

            logShipWorks = copyFrom.logShipWorks;
            logServices = copyFrom.logServices;

            maxLogAgeDays = copyFrom.maxLogAgeDays;
        }

        /// <summary>
        /// Save the log options
        /// </summary>
        public void Save(string filename)
        {
            using (XmlTextWriter xmlWriter = new XmlTextWriter(filename, Encoding.UTF8))
            {
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("LogOptions");

                xmlWriter.WriteElementString("LogShipWorks", logShipWorks.ToString());
                xmlWriter.WriteElementString("LogServices", logServices.ToString());
                xmlWriter.WriteElementString("MaxLogAgeDays", maxLogAgeDays.ToString());

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }

        /// <summary>
        /// Load a new LogOptions instance from file.
        /// </summary>
        public static LogOptions Load(string filename)
        {
            XPathDocument xpathDocument = new XPathDocument(filename);
            XPathNavigator xpath = xpathDocument.CreateNavigator();

            LogOptions logOptions = new LogOptions();
            logOptions.LogShipWorks = XPathUtility.Evaluate(xpath, "//LogShipWorks", logOptions.LogShipWorks);
            logOptions.LogServices = XPathUtility.Evaluate(xpath, "//LogServices", logOptions.LogServices);
            logOptions.MaxLogAgeDays = XPathUtility.Evaluate(xpath, "//MaxLogAgeDays", logOptions.MaxLogAgeDays);

            return logOptions;
        }

        /// <summary>
        /// Log ShipWorks operations
        /// </summary>
        public bool LogShipWorks
        {
            get { return logShipWorks; }
            set { logShipWorks = value; }
        }

        /// <summary>
        /// Log API service calls such as Endicia, UPS, eBay, Amazon
        /// </summary>
        public bool LogServices
        {
            get { return logServices; }
            set { logServices = value; }
        }

        /// <summary>
        /// The maximum age of a log file before automatically being deleted
        /// </summary>
        public int MaxLogAgeDays
        {
            get { return maxLogAgeDays; }
            set { maxLogAgeDays = value; }
        }
    }
}
