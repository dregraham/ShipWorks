using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using log4net;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.UI.Platforms.Odbc;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcCustomQueryDlgViewModelTest
    {
        [Fact]
        public void Execute_DelegatesToSampleDataCommandExecute()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var sampleDataCommand = mock.Mock<IOdbcSampleDataCommand>();
                var dataSource = mock.Mock<IOdbcDataSource>();

                var testObject = mock.Create<OdbcCustomQueryDlgViewModel>();
                testObject.Query = "myQuery";

                testObject.Execute.Execute(null);

                sampleDataCommand.Verify(c=>c.Execute(dataSource.Object, "myQuery", 10), Times.Once);
            }
        }

        [Fact]
        public void Ok_DelegatesToColumnSourceLoad()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var odbcColumnSource = mock.Mock<IOdbcColumnSource>();
                var dataSource = mock.Mock<IOdbcDataSource>();
                var dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();

                Mock<ILog> logger = mock.GetLogger<OdbcColumnSource>();

                var testObject = mock.Create<OdbcCustomQueryDlgViewModel>();
                testObject.Query = "myQuery";

                testObject.Ok.Execute(mock.MockRepository.Create<IDialog>().Object);

                odbcColumnSource.Verify(
                    s => s.Load(dataSource.Object, logger.Object, testObject.Query, dbProviderFactory.Object),
                    Times.Once);
            }
        }
    }
}