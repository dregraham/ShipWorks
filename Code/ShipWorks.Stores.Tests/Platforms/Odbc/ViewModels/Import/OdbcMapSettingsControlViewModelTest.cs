using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.ViewModels.Import
{
    public class OdbcMapSettingsControlViewModelTest
    {
        [Fact]
        public void ValidateRequiredMapSettings_DoesNotShowError_WhenSelectedTableExists_AndColumnSourceIsTable()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var columnSourceMock = mock.MockRepository.Create<IOdbcColumnSource>();
                columnSourceMock.Setup(s => s.Name).Returns("cs name");

                var messageHelperMock = mock.Mock<IMessageHelper>();
                var testObject = mock.Create<TestOdbcMapSettingsControlViewModel>();

                var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
                var schemaMock = mock.MockRepository.Create<IOdbcSchema>();
                schemaMock.Setup(s => s.Tables).Returns(new[] {columnSourceMock.Object});

                testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());

                testObject.SelectedTable = columnSourceMock.Object;
                testObject.ColumnSourceIsTable = true;
                
                testObject.ValidateRequiredMapSettings();
                messageHelperMock.Verify(m=>m.ShowError(It.IsAny<string>()), Times.Never);
            }
        }

        [Fact]
        public void ValidateRequiredMapSettings_ReturnsTrue_WhenSelectedTableExists_AndColumnSourceIsTable()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var columnSourceMock = mock.MockRepository.Create<IOdbcColumnSource>();
                columnSourceMock.Setup(s => s.Name).Returns("cs name");

                var testObject = mock.Create<TestOdbcMapSettingsControlViewModel>();

                var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
                var schemaMock = mock.MockRepository.Create<IOdbcSchema>();
                schemaMock.Setup(s => s.Tables).Returns(new[] { columnSourceMock.Object });

                testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());

                testObject.SelectedTable = columnSourceMock.Object;
                testObject.ColumnSourceIsTable = true;

                Assert.True(testObject.ValidateRequiredMapSettings());
            }
        }

        [Fact]
        public void ValidateRequiredMapSettings_ReturnsFalse_WhenSelectedTableNotInDatabase_AndColumnSourceIsTable()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var columnSourceMock = mock.MockRepository.Create<IOdbcColumnSource>();
                columnSourceMock.Setup(s => s.Name).Returns("cs name");

                var testObject = mock.Create<TestOdbcMapSettingsControlViewModel>();

                var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
                var schemaMock = mock.MockRepository.Create<IOdbcSchema>();
                schemaMock.Setup(s => s.Tables).Returns(new IOdbcColumnSource[0] );

                testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());

                testObject.SelectedTable = columnSourceMock.Object;
                testObject.ColumnSourceIsTable = true;

                Assert.False(testObject.ValidateRequiredMapSettings());
            }
        }

        [Fact]
        public void ValidateRequiredMapSettings_ShowsError_WhenSelectedTableNotInDatabase_AndColumnSourceIsTable()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var columnSourceMock = mock.MockRepository.Create<IOdbcColumnSource>();
                columnSourceMock.Setup(s => s.Name).Returns("cs name");

                var messageHelperMock = mock.Mock<IMessageHelper>();
                var testObject = mock.Create<TestOdbcMapSettingsControlViewModel>();

                var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
                var schemaMock = mock.MockRepository.Create<IOdbcSchema>();

                testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());

                testObject.SelectedTable = columnSourceMock.Object;
                testObject.ColumnSourceIsTable = true;

                testObject.ValidateRequiredMapSettings();
                messageHelperMock.Verify(m => m.ShowError("The selected table does not exist in the current data source"), Times.Once);
            }
        }

        [Fact]
        public void ValidateRequiredMapSettings_ReturnsFalse_WhenSelectedTableIsNull_AndColumnSourceIsTable()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var messageHelperMock = mock.Mock<IMessageHelper>();
                var testObject = mock.Create<TestOdbcMapSettingsControlViewModel>();

                var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
                var schemaMock = mock.MockRepository.Create<IOdbcSchema>();

                testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());
                testObject.ColumnSourceIsTable = true;
                testObject.SelectedTable = null;

                testObject.ValidateRequiredMapSettings();

                messageHelperMock.Verify(m => m.ShowError("Please select a table before continuing to the next page."), Times.Once);
            }
        }

        [Fact]
        public void ValidateRequiredMapSettings_ShowsError_WhenCustomQueryIsEmpty_AndColumnSourceIsNotTable()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var messageHelperMock = mock.Mock<IMessageHelper>();
                var testObject = mock.Create<TestOdbcMapSettingsControlViewModel>();

                var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
                var schemaMock = mock.MockRepository.Create<IOdbcSchema>();
                testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());
                testObject.CustomQuery = string.Empty;
                testObject.ColumnSourceIsTable = false;

                testObject.ValidateRequiredMapSettings();

                messageHelperMock.Verify(m => m.ShowError("Please enter a valid query before continuing to the next page."), Times.Once);
            }
        }

        [Fact]
        public void ValidateRequiredMapSettings_ReturnsFalse_WhenCustomQueryIsEmpty_AndColumnSourceIsNotTable()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<TestOdbcMapSettingsControlViewModel>();

                var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
                var schemaMock = mock.MockRepository.Create<IOdbcSchema>();
                testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());
                testObject.CustomQuery = string.Empty;
                testObject.ColumnSourceIsTable = false;

                Assert.False(testObject.ValidateRequiredMapSettings());
            }
        }

        [Fact]
        public void ValidateRequiredMapSettings_ReturnsTrue_WhenCustomQueryIsNotNull_AndColumnSourceIsNotTable()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var testObject = mock.Create<TestOdbcMapSettingsControlViewModel>();

                var dataSourceMock = mock.MockRepository.Create<IOdbcDataSource>();
                var schemaMock = mock.MockRepository.Create<IOdbcSchema>();
                testObject.Load(dataSourceMock.Object, schemaMock.Object, "blah", new OdbcStoreEntity());
                testObject.CustomQuery = "select *";
                testObject.ColumnSourceIsTable = false;
                Assert.True(testObject.ValidateRequiredMapSettings());
            }
        }
    }
}
