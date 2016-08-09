using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class FakeOdbcSettingsFile : Stores.Platforms.Odbc.Mapping.OdbcSettingsFile
    {
        public FakeOdbcSettingsFile(IMessageHelper messageHelper) : base(messageHelper)
        {
        }

        public override string Action => "FakeAction";

        public override string Extension => "FakeExtension";

        protected override void WriteAdditionalParamatersToMap(JObject map)
        {
            // Not Needed
        }

        protected override void ReadAdditionalParamatersFromMap(JObject map)
        {
            // Not Needed
        }
    }
}