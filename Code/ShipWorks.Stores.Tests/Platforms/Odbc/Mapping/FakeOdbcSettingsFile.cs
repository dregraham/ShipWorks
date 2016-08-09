using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class FakeOdbcSettingsFile : Stores.Platforms.Odbc.Mapping.FakeOdbcSettingsFile
    {
        public FakeOdbcSettingsFile(IIndex<FileDialogType, IFileDialog> fileDialogFactory, IMessageHelper messageHelper) :
            base(fileDialogFactory, messageHelper)
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