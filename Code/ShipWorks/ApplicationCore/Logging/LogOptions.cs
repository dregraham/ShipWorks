using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Linq;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using log4net.Core;

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
        bool logRateCalls = false;
        private Level minLevel = Level.Info;
        private Level maxLevel = Level.Fatal;
        private static List<Level> allLevels; 

        // How many days logs are left behind
        int maxLogAgeDays = 7;

        /// <summary>
        /// Constructor
        /// </summary>
        public LogOptions()
        {
            allLevels = GetAllLevels();
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
            logRateCalls = copyFrom.logRateCalls;

            maxLogAgeDays = copyFrom.maxLogAgeDays;

            minLevel = copyFrom.MinLevel;
            maxLevel = copyFrom.MaxLevel;
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
                xmlWriter.WriteElementString("LogRateCalls", logRateCalls.ToString());
                xmlWriter.WriteElementString("MaxLogAgeDays", maxLogAgeDays.ToString());

                xmlWriter.WriteStartElement("MinLevel");
                xmlWriter.WriteAttributeString("Name", minLevel.Name);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("MaxLevel");
                xmlWriter.WriteAttributeString("Name", maxLevel.Name);
                xmlWriter.WriteEndElement();

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
            logOptions.LogRateCalls = XPathUtility.Evaluate(xpath, "//LogRateCalls", logOptions.LogRateCalls);
            logOptions.MaxLogAgeDays = XPathUtility.Evaluate(xpath, "//MaxLogAgeDays", logOptions.MaxLogAgeDays);

            string levelName = XPathUtility.Evaluate(xpath, "//MinLevel/@Name", InterapptiveOnly.IsInterapptiveUser ? Level.Debug.Name : Level.Info.Name);
            logOptions.MinLevel = FindLevel(levelName, Level.Info);

            levelName = XPathUtility.Evaluate(xpath, "//MaxLevel/@Name", Level.Fatal.Name);
            logOptions.MaxLevel = FindLevel(levelName, Level.Fatal);

            return logOptions;
        }

        /// <summary>
        /// Find a logging level based on name.  If it's not found, return the specified default value.
        /// </summary>
        /// <returns></returns>
        private static Level FindLevel(string levelName, Level defaultLevel)
        {
            if (allLevels.Any(l => l.Name.Equals(levelName, StringComparison.OrdinalIgnoreCase)))
            {
                return allLevels.FirstOrDefault(l => l.Name.Equals(levelName, StringComparison.OrdinalIgnoreCase));
            }

            return defaultLevel;
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
        /// Log API service calls to GetRates
        /// </summary>
        public bool LogRateCalls
        {
            get { return logRateCalls; }
            set { logRateCalls = value; }
        }

        /// <summary>
        /// The maximum age of a log file before automatically being deleted
        /// </summary>
        public int MaxLogAgeDays
        {
            get { return maxLogAgeDays; }
            set { maxLogAgeDays = value; }
        }

        /// <summary>
        /// Minimum logging level
        /// </summary>
        public Level MinLevel
        {
            get { return minLevel; }
            set { minLevel = value; }
        }

        /// <summary>
        /// Maximum logging level
        /// </summary>
        public Level MaxLevel
        {
            get { return maxLevel; }
            set { maxLevel = value; }
        }

        /// <summary>
        /// Returns a list of all the levels we currently support
        /// </summary>
        /// <returns></returns>
        private static List<Level> GetAllLevels()
        {
            return new List<Level>()
            {
                Level.Off,
                Level.Fatal,
                Level.Error,
                Level.Warn,
                Level.Info,
                Level.Debug,
                Level.All
            };
        }
    }
}
