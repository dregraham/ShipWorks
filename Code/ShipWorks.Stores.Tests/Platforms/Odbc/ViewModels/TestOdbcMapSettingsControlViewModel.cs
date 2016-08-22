using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels;
using System;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.ViewModels
{
    public class TestOdbcMapSettingsControlViewModel : OdbcMapSettingsControlViewModel
    {
        public TestOdbcMapSettingsControlViewModel(IMessageHelper messageHelper, Func<string, IOdbcColumnSource> columnSourceFactory) : base(messageHelper, columnSourceFactory)
        {
        }

        public override bool ColumnSourceIsTable { get; set; }

        public override void SaveMapSettings(OdbcStoreEntity store)
        {
            // not implemented for testing the abstract class
        }

        public override void LoadMapSettings(OdbcStoreEntity store)
        {
            // not implemented for testing the abstract class
        }

        public override string CustomQueryColumnSourceName => "Test query column source name";
    }
}
