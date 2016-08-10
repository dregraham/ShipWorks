using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Saves and Opens Odbc Import Settings from disk.
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.Odbc.Mapping.OdbcSettingsFile" />
    public class OdbcImportSettingsFile : OdbcSettingsFile, IOdbcImportSettingsFile
    {
        public OdbcImportSettingsFile(IOdbcFieldMap fieldMap, IMessageHelper messageHelper) : base(fieldMap, messageHelper)
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

        protected override void PopulateOdbcSettingsFrom(JObject settings)
        {
            base.PopulateOdbcSettingsFrom(settings);

            OdbcImportStrategy = EnumHelper.GetEnumByApiValue<OdbcImportStrategy>(settings.GetValue("ImportStrategy").ToString());
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