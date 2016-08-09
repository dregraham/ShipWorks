using Autofac.Features.Indexed;
using Interapptive.Shared.UI;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class FakeOdbcSettingsFile : OdbcSettingsFile
    {
        public FakeOdbcSettingsFile(IIndex<FileDialogType, IFileDialog> fileDialogFactory, IMessageHelper messageHelper) :
            base(fileDialogFactory, messageHelper)
        {
        }

        public override string Action { get; }

        public override string Extension { get; }

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