using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using ShipWorks.Users;
using Interapptive.Shared;
using ShipWorks.ApplicationCore;
using log4net;

namespace ShipWorks.ApplicationCore.Appearance
{
    /// <summary>
    /// Display properties
    /// </summary>
    static class ShipWorksDisplay
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksDisplay));

        const string defaultFile = "display.xml";

        // Color ShipWorks should be
        static ColorScheme defaultColorScheme = ColorScheme.Blue;
        
        // ShipWorks only shows in system tray when minimized
        static bool deafultHideInTray = false;

        /// <summary>
        /// Loads the default display settings
        /// </summary>
        public static void LoadDefault()
        {
            defaultColorScheme = ColorScheme.Blue;
            deafultHideInTray = false;

            if (File.Exists(DefaultFilePath))
            {
                Load(DefaultFilePath);
            }
        }

        /// <summary>
        /// Save the default display settings
        /// </summary>
        public static void SaveDefault()
        {
            Save(DefaultFilePath);
        }

        /// <summary>
        /// The path to the default settings location
        /// </summary>
        private static string DefaultFilePath
        {
            get
            {
                return Path.Combine(DataPath.InstanceSettings, defaultFile);
            }
        }

        /// <summary>
        /// The current color scheme in affect
        /// </summary>
        public static ColorScheme ColorScheme
        {
            get
            {
                return defaultColorScheme;
            }
            set
            {
                defaultColorScheme = value;
            }
        }

        /// <summary>
        /// Controls if ShipWorks should be shown only in the system tray when minimized
        /// </summary>
        public static bool HideInSystemTray
        {
            get
            {
                return deafultHideInTray;
            }
            set
            {
                deafultHideInTray = value;
            }
        }

        /// <summary>
        /// Save the display settings to the given filename
        /// </summary>
        private static void Save(string filename)
        {
            // Simple enough for now not to need text writer
            string xml = string.Format("<Display><ColorScheme>{0}</ColorScheme><SystemTray>{1}</SystemTray></Display>",
                (int) defaultColorScheme,
                deafultHideInTray);

            File.WriteAllText(filename, xml);
        }

        /// <summary>
        /// Load the display settings from the given filename
        /// </summary>
        private static void Load(string filename)
        {
            string xml = File.ReadAllText(filename);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xml);

                defaultColorScheme = (ColorScheme) Convert.ToInt32(xmlDocument.SelectSingleNode("//ColorScheme").InnerText);
                deafultHideInTray = Convert.ToBoolean(xmlDocument.SelectSingleNode("//SystemTray").InnerText);
            }
            catch (XmlException ex)
            {
                log.Error("Failed to load default display settings.", ex);
            }
        }
    }
}
