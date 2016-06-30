using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.UI.Platforms.Odbc;
using System.Data;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcCustomQueryDlgViewModelTest
    {
        private readonly int numberOfResults = 25;

        [Fact]
        public void Execute_DelegatesToSampleDataCommandExecute()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var sampleDataCommand = mock.Mock<IOdbcSampleDataCommand>();
                var dataSource = mock.Mock<IOdbcDataSource>();

                sampleDataCommand.Setup(c => c.Execute(dataSource.Object, "myQuery", 25)).Returns(new DataTable());

                var testObject = mock.Create<OdbcCustomQueryDlgViewModel>();
                testObject.Query = "myQuery";

                testObject.Execute.Execute(null);

                sampleDataCommand.Verify(c=>c.Execute(dataSource.Object, "myQuery", numberOfResults), Times.Once);
            }
        }

        [Fact]
        public void Execute_ShowsMessage_WhenExecuteThrowsException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var dataSource = mock.Mock<IOdbcDataSource>();

                var sampleDataCommand = mock.Mock<IOdbcSampleDataCommand>();
                sampleDataCommand.Setup(c => c.Execute(dataSource.Object, "myQuery", numberOfResults))
                    .Throws(new ShipWorksOdbcException("error message"));

                var messageHelper = mock.Mock<IMessageHelper>();
                
                var testObject = mock.Create<OdbcCustomQueryDlgViewModel>();
                testObject.Query = "myQuery";

                testObject.Execute.Execute(null);

                messageHelper.Verify(mh=>mh.ShowError("error message"));
            }
        }

        [Fact]
        public void Ok_SavesQueryToColumnSource_WhenExecuteSucceeds()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var dataSource = mock.Mock<IOdbcDataSource>();
                var sampleDataCommand = mock.Mock<IOdbcSampleDataCommand>();
                sampleDataCommand.Setup(c => c.Execute(dataSource.Object, "myQuery", numberOfResults)).Returns(new DataTable());

                var columnSource = mock.Mock<IOdbcColumnSource>();
                var testObject = mock.Create<OdbcCustomQueryDlgViewModel>();
                testObject.Query = "myQuery";

                var customQueryDialog = mock.MockRepository.Create<IDialog>();

                testObject.Ok.Execute(customQueryDialog.Object);

                columnSource.VerifySet(s=>s.Query = "myQuery",Times.Once);
            }
        }

        [Fact]
        public void Ok_ClosesDialog_WhenExecuteSucceeds()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var dataSource = mock.Mock<IOdbcDataSource>();
                var sampleDataCommand = mock.Mock<IOdbcSampleDataCommand>();
                sampleDataCommand.Setup(c => c.Execute(dataSource.Object, "myQuery", numberOfResults)).Returns(new DataTable());
                
                var testObject = mock.Create<OdbcCustomQueryDlgViewModel>();
                testObject.Query = "myQuery";

                var customQueryDialog = mock.MockRepository.Create<IDialog>();

                testObject.Ok.Execute(customQueryDialog.Object);

                customQueryDialog.Verify(d=>d.Close(), Times.Once);
            }
        }

        [Fact]
        public void Ok_DoesNotSaveQueryToColumnSource_WhenExecuteFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var columnSource = mock.Mock<IOdbcColumnSource>();

                var dataSource = mock.Mock<IOdbcDataSource>();

                var sampleDataCommand = mock.Mock<IOdbcSampleDataCommand>();
                sampleDataCommand.Setup(c => c.Execute(dataSource.Object, "myQuery", numberOfResults))
                    .Throws(new ShipWorksOdbcException("error message"));


                var testObject = mock.Create<OdbcCustomQueryDlgViewModel>();
                testObject.Query = "myQuery";

                testObject.Ok.Execute(null);

                columnSource.VerifySet(s => s.Query = "myQuery", Times.Never);
            }
        }

        [Fact]
        public void Ok_DoesNotCloseDialog_WhenExecuteFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var dataSource = mock.Mock<IOdbcDataSource>();
                var sampleDataCommand = mock.Mock<IOdbcSampleDataCommand>();
                sampleDataCommand.Setup(c => c.Execute(dataSource.Object, "myQuery", numberOfResults))
                    .Throws<ShipWorksOdbcException>();

                var testObject = mock.Create<OdbcCustomQueryDlgViewModel>();
                testObject.Query = "myQuery";

                var customQueryDialog = mock.MockRepository.Create<IDialog>();

                testObject.Ok.Execute(customQueryDialog.Object);

                customQueryDialog.Verify(d => d.Close(), Times.Never);
            }
        }
    }
}