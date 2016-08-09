using Interapptive.Shared.UI;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Saves and Opens Odbc Upload Settings from disk.
    /// </summary>
    public class OdbcUploadSettingsFile : OdbcSettingsFile
    {
        /// <summary>
        /// The action to perform on this file (Upload)
        /// </summary>
        public override string Action => "Upload";

        /// <summary>
        /// The file extension.
        /// </summary>
        public override string Extension => ".swoum";

        public OdbcUploadSettingsFile(IMessageHelper messageHelper, IOdbcFieldMap fieldMap) : base(messageHelper, fieldMap)
        {
        }

        protected override void ReadAdditionalParamatersFromMap(JObject settings)
        {
            // nothing to do
        }

        protected override void WriteAdditionalParamatersToMap(JObject settings)
        {
            // nothing to do
        }
    }
}