using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Administration.UpdateFrom2x.Configuration;
using log4net;
using System.Diagnostics;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Configuration
{
    /// <summary>
    /// Utility class for saving per-instance update state information for ShipWorks2x upgrades
    /// </summary>
    public static class ConfigurationMigrationState
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ConfigurationMigrationState));

        // Path to file maintaining state
        static string statefile = Path.Combine(DataPath.InstanceSettings, "upgrade.xml");

        /// <summary>
        /// Result of a previous migration
        /// </summary>
        public static ConfigurationMigrationAction Action
        {
            get
            {
                XElement element = GetElement("Action");

                return (element == null) ? ConfigurationMigrationAction.None : (ConfigurationMigrationAction) (int) element;
            }
            set
            {
                log.InfoFormat("Updating ConfigurationMigrateResult to {0}", value);
                SaveElement(new XElement("Action", (int) value));
            }
        }


        /// <summary>
        /// Set the pending method for templates to be imported.
        /// </summary>
        public static ShipWorks2xApplicationDataSource ApplicationDataSource
        {
            get
            {
                XElement element = GetElement("ApplicationDataSource");

                if (element == null)
                {
                    return new ShipWorks2xApplicationDataSource();
                }
                else
                {
                    return new ShipWorks2xApplicationDataSource
                        {
                            Path = (string) element.Element("Path"),
                            SourceType = (ShipWorks2xApplicationDataSourceType) (int) element.Element("SourceType")
                        };
                }
            }
            set
            {
                SaveElement(new XElement("ApplicationDataSource",
                    new XElement("Path", value.Path),
                    new XElement("SourceType", (int) value.SourceType)));
            }
        }

        /// <summary>
        /// Get the element of the given name
        /// </summary>
        private static XElement GetElement(string name)
        {
            if (!File.Exists(statefile))
            {
                return null;
            }

            try
            {
                XElement root = XElement.Load(statefile);
                return root.Element(name);
            }
            catch (IOException ex)
            {
                log.Error("Failed to open upgrade state for element " + name, ex);
                Debug.Fail("Failed to open upgrade state", ex.Message);

                return null;
            }
        }

        /// <summary>
        /// Save the given XElement to disk
        /// </summary>
        private static void SaveElement(XElement element)
        {
            try
            {
                XElement root = new XElement("ShipWorks");

                if (File.Exists(statefile))
                {
                    root = XElement.Load(statefile);
                }

                var existing = root.Element(element.Name);
                if (existing != null)
                {
                    existing.Remove();
                }

                root.Add(element);

                root.Save(statefile);
            }
            catch (IOException ex)
            {
                log.Error("Failed to save upgrade state for element " + element.Name, ex);
                Debug.Fail("Failed to save upgrade state", ex.Message);
            }
        }
    }
}
