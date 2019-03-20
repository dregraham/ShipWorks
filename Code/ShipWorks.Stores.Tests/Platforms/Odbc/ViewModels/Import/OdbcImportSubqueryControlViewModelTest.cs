using System;
using System.Data;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.ViewModels.Import
{
    public class OdbcImportSubqueryControlViewModelTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcImportSubqueryControlViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }
        
        [Fact]
        public void Load_SetsCustomQuery()
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();

            OdbcImportSubqueryControlViewModel testObject = mock.Create<OdbcImportSubqueryControlViewModel>();
            testObject.Load(dataSource.Object, "my query");
            
            Assert.Equal("my query", testObject.CustomQuery);
        }
        
        [Fact]
        public void ValidateQuery_ReturnsTrue_WhenQueryIsValid()
        {
            string query = "my query";
            
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            mock.Mock<IOdbcSampleDataCommand>()
                .Setup(s => s.Execute(dataSource.Object, query, It.IsAny<int>()))
                .Returns(new DataTable());
            
            OdbcImportSubqueryControlViewModel testObject = mock.Create<OdbcImportSubqueryControlViewModel>();
            testObject.Load(dataSource.Object, query);
            
            Assert.True(testObject.ValidateQuery());
        }
        
        [Fact]
        public void ValidateQuery_ReturnsFalse_WhenSampleDataCommandThrowsShipWorksOdbcException()
        {
            string query = "my query";
            
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            mock.Mock<IOdbcSampleDataCommand>()
                .Setup(s => s.Execute(dataSource.Object, query, It.IsAny<int>()))
                .Throws(new ShipWorksOdbcException("Something went wrong"));
            
            OdbcImportSubqueryControlViewModel testObject = mock.Create<OdbcImportSubqueryControlViewModel>();
            testObject.Load(dataSource.Object, query);
            
            Assert.False(testObject.ValidateQuery());
        }

        [Fact] public void ValidateQuery_ShowsError_WhenSampleDataCommandThrowsShipWorksOdbcException()
        {
            string query = "my query";
            string errorMessage = "Something went wrong";
            Mock<IMessageHelper> messageHelper = mock.Mock<IMessageHelper>();
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            mock.Mock<IOdbcSampleDataCommand>()
                .Setup(s => s.Execute(dataSource.Object, query, It.IsAny<int>()))
                .Throws(new ShipWorksOdbcException(errorMessage));
            
            OdbcImportSubqueryControlViewModel testObject = mock.Create<OdbcImportSubqueryControlViewModel>();
            testObject.Load(dataSource.Object, query);

            testObject.ValidateQuery();
            
            messageHelper.Verify(m => m.ShowError(errorMessage), Times.Once);
        }
        
        [Fact]
        public void ValidateQuery_ShowsError_WhenCustomQueryIsEmpty()
        {
            string query = string.Empty;
            
            Mock<IMessageHelper> messageHelper = mock.Mock<IMessageHelper>();
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            mock.Mock<IOdbcSampleDataCommand>()
                .Setup(s => s.Execute(dataSource.Object, query, It.IsAny<int>()))
                .Throws(new ShipWorksOdbcException("Something went wrong"));
            
            OdbcImportSubqueryControlViewModel testObject = mock.Create<OdbcImportSubqueryControlViewModel>();
            testObject.Load(dataSource.Object, query);

            testObject.ValidateQuery();
            
            messageHelper.Verify(m => m.ShowError("Please enter a valid query."), Times.Once);
        }

        [Fact]
        public void ValidateQuery_ReturnsFalse_WhenCustomQueryIsEmpty()
        {
            string query = string.Empty;
            
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            mock.Mock<IOdbcSampleDataCommand>()
                .Setup(s => s.Execute(dataSource.Object, query, It.IsAny<int>()))
                .Throws(new ShipWorksOdbcException("Something went wrong"));
            
            OdbcImportSubqueryControlViewModel testObject = mock.Create<OdbcImportSubqueryControlViewModel>();
            testObject.Load(dataSource.Object, query);

            Assert.False(testObject.ValidateQuery());
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}