using Interapptive.Shared.UI;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class FakeOdbcSettingsFile : OdbcSettingsFile
    {
        public FakeOdbcSettingsFile(IMessageHelper messageHelper, IOdbcFieldMap fieldMap) : base(messageHelper, fieldMap)
        {
        }

        public override string Action => "FakeAction";

        public override string Extension => "FakeExtension";

        protected override void WriteAdditionalParamatersToMap(JObject settings)
        {
            // Not Needed
        }

        protected override void ReadAdditionalParamatersFromMap(JObject settings)
        {
            // Not Needed
        }
    }
}