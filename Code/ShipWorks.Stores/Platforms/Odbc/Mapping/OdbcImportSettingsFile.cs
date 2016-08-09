using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    public class OdbcImportSettingsFile : OdbcSettingsFile
    {
        public OdbcImportSettingsFile(IMessageHelper messageHelper, IOdbcFieldMap fieldMap) : base(messageHelper, fieldMap)
        {
        }

        public override string Action => "Import";
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