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

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploadSettingsFile"/> class.
        /// </summary>
        public OdbcUploadSettingsFile(IOdbcFieldMap fieldMap, IMessageHelper messageHelper) : base(fieldMap, messageHelper)
        {
        }

        /// <summary>
        /// Reads the additional paramaters from map.
        /// </summary>
        protected override void ReadAdditionalParamatersFromMap(JObject settings)
        {
            // nothing to do
        }

        /// <summary>
        /// Writes the additional paramaters to map.
        /// </summary>
        protected override void WriteAdditionalParamatersToMap(JObject settings)
        {
            // nothing to do
        }
    }
}