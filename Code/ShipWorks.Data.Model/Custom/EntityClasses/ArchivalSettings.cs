using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace ShipWorks.Data.Model.Custom.EntityClasses
{
    /// <summary>
    /// Archival settings
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [XmlRoot(ElementName = "ArchivalSettings")]
    public class ArchivalSettings
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ArchivalSettings()
        {
            Settings = Enumerable.Empty<ArchivalSetting>().ToList();
        }

        /// <summary>
        /// List of archival settings
        /// </summary>
        public List<ArchivalSetting> Settings { get; set; }
    }

    /// <summary>
    /// Archival settings
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    [XmlRoot(ElementName = "ArchivalSetting")]
    public class ArchivalSetting
    {
        [XmlElement(ElementName = "DateArchived")]
        public DateTime DateArchived { get; set; }

        [XmlElement(ElementName = "SelectedArchivalDate")]
        public DateTime SelectedArchivalDate { get; set; }

        [XmlElement(ElementName = "NeedsFilterRegeneration")]
        public bool NeedsFilterRegeneration { get; set; }

        [XmlElement(ElementName = "SourceShipWorksVersion")]
        public string SourceShipWorksVersion { get; set; }

        [XmlElement(ElementName = "SourceDatabaseName")]
        public string SourceDatabaseName { get; set; }
    }
}
