using System;
using System.IO;
using System.Reactive;
using System.Xml;
using log4net;

namespace ShipWorks.ApplicationCore.Appearance
{
    /// <summary>
    /// Display properties
    /// </summary>
    public static class ShipWorksDisplay
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksDisplay));
        private const string defaultFile = "display.xml";

        // Color ShipWorks should be
        private static ColorScheme defaultColorScheme = ColorScheme.Blue;

        // ShipWorks only shows in system tray when minimized
        private static bool deafultHideInTray = false;

        public static event EventHandler ColorSchemeChanged;

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
                if (defaultColorScheme != value)
                {
                    defaultColorScheme = value;
                    ColorSchemeChanged?.Invoke(Unit.Default, EventArgs.Empty);
                }
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
