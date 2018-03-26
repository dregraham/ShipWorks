using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.Custom.EntityClasses;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Partial class extension of the LLBLGen ConfigurationEntity
    /// </summary>
    public partial class ConfigurationEntity
    {
        /// <summary>
        /// Get or set an object for the ArchivalSetting XML
        /// </summary>
        public ArchivalSettings ArchivalSettingObject
        {
            get
            {
                return string.IsNullOrWhiteSpace(ArchivalSettingsXml) ?
                    new ArchivalSettings() : 
                    SerializationUtility.DeserializeFromXml<ArchivalSettings>(ArchivalSettingsXml);
            }
            set
            {
                ArchivalSettingsXml = SerializationUtility.SerializeToXml(value);
            }
        }
    }
}
