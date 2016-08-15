using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using log4net;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Odbc.Download;
using System;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Saves and Opens Odbc Import Settings from disk.
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.Odbc.Mapping.OdbcSettingsFile" />
    public class OdbcImportSettingsFile : OdbcSettingsFile, IOdbcImportSettingsFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportSettingsFile"/> class.
        /// </summary>
        public OdbcImportSettingsFile(IOdbcFieldMap fieldMap, IMessageHelper messageHelper, Func<Type, ILog> logFactory) 
            : base(fieldMap, messageHelper, logFactory(typeof(OdbcImportSettingsFile)))
        {
        }

        /// <summary>
        /// The action to perform on this file (Import)
        /// </summary>
        public override string Action => "Import";

        /// <summary>
        /// The file extension.
        /// </summary>
        public override string Extension => ".swoim";

        /// <summary>
        /// Gets or sets the ODBC import strategy.
        /// </summary>
        public OdbcImportStrategy OdbcImportStrategy { get; set; }

        /// <summary>
        /// Populates the ODBC settings from.
        /// </summary>
        protected override void PopulateOdbcSettingsFrom(JObject settings)
        {
            base.PopulateOdbcSettingsFrom(settings);

            string importStrategy = settings.GetValue("ImportStrategy")?.ToString();
            if (!string.IsNullOrWhiteSpace(importStrategy))
            {
                OdbcImportStrategy = EnumHelper.GetEnumByApiValue<OdbcImportStrategy>(importStrategy);
            }
        }

        /// <summary>
        /// Saves the settings to the JObject
        /// </summary>
        protected override void SaveOdbcSettingsTo(JObject settings)
        {
            base.SaveOdbcSettingsTo(settings);

            settings.Add("ImportStrategy", EnumHelper.GetApiValue(OdbcImportStrategy));
        }
    }
}