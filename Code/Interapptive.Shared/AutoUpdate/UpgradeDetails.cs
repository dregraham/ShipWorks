using System;
using System.Xml.Serialization;

namespace Interapptive.Shared.AutoUpdate
{
    /// <summary>
    /// Details about an attempted upgrade
    /// </summary>
    [Serializable]
    [XmlRoot("UpgradeDetails")]
    public class UpgradeDetails
    {
        /// <summary>
        /// Version ShipWorks is upgrading to
        /// </summary>
        [XmlElement("UpgradingTo")]
        public string UpgradingTo { get; set; }

        /// <summary>
        /// Version ShipWorks is upgrading to
        /// </summary>
        public Version UpgradingToVersion
        {
            get => Version.TryParse(UpgradingTo, out Version parsedVersion) ? parsedVersion : new Version();
            set => UpgradingTo = value.ToString();
        }
    }
}
