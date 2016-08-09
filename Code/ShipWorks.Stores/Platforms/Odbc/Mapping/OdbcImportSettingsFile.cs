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
        public OdbcImportSettingsFile(IMessageHelper messageHelper, IOdbcFieldMap fieldMap) : base(messageHelper, fieldMap)
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

        public OdbcImportStrategy OdbcImportStrategy { get; set; }

        /// <summary>
        /// Reads the additional paramaters from map.
        /// </summary>
        protected override void ReadAdditionalParamatersFromMap(JObject settings)
        {
            OdbcImportStrategy = EnumHelper.GetEnumByApiValue<OdbcImportStrategy>(settings.GetValue("ImportStrategy").ToString());
        }

        /// <summary>
        /// Writes the additional paramaters to map.
        /// </summary>
        protected override void WriteAdditionalParamatersToMap(JObject settings)
        {
            settings.Add("ImportStrategy", EnumHelper.GetApiValue(OdbcImportStrategy));
        }
    }
}