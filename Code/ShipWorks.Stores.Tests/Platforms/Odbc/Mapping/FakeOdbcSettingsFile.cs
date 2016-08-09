using Interapptive.Shared.UI;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Fake OdbcSettingsFile to implement abstract class OdbcSettingsFile
    /// </summary>
    /// <seealso cref="ShipWorks.Stores.Platforms.Odbc.Mapping.OdbcSettingsFile" />
    public class FakeOdbcSettingsFile : OdbcSettingsFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeOdbcSettingsFile"/> class.
        /// </summary>
        public FakeOdbcSettingsFile(IOdbcFieldMap fieldMap, IMessageHelper messageHelper) : base(fieldMap, messageHelper)
        {
        }

        /// <summary>
        /// The action to perform on this file (Import/Upload)
        /// </summary>
        public override string Action => "FakeAction";

        /// <summary>
        /// The file extension.
        /// </summary>
        public override string Extension => "FakeExtension";

        /// <summary>
        /// Writes the additional paramaters to map.
        /// </summary>
        protected override void WriteAdditionalParamatersToMap(JObject settings)
        {
            // Not Needed
        }

        /// <summary>
        /// Reads the additional paramaters from map.
        /// </summary>
        protected override void ReadAdditionalParamatersFromMap(JObject settings)
        {
            // Not Needed
        }
    }
}