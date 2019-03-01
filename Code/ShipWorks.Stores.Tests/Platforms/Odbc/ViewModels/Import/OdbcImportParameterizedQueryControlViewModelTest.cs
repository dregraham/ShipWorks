using System;
using System.Data;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.ViewModels.Import
{
    public class OdbcImportParameterizedQueryControlViewModelTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcImportParameterizedQueryControlViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }
        
        [Fact]
        public void Load_SetsCustomQuery()
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();

            OdbcImportParameterizedQueryControlViewModel testObject = mock.Create<OdbcImportParameterizedQueryControlViewModel>();
            testObject.Load(dataSource.Object, OdbcImportStrategy.OnDemand, "my query");
            
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
            
            OdbcImportParameterizedQueryControlViewModel testObject = mock.Create<OdbcImportParameterizedQueryControlViewModel>();
            testObject.Load(dataSource.Object, OdbcImportStrategy.OnDemand, query);
            
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
            
            OdbcImportParameterizedQueryControlViewModel testObject = mock.Create<OdbcImportParameterizedQueryControlViewModel>();
            testObject.Load(dataSource.Object, OdbcImportStrategy.OnDemand, query);
            
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
            
            OdbcImportParameterizedQueryControlViewModel testObject = mock.Create<OdbcImportParameterizedQueryControlViewModel>();
            testObject.Load(dataSource.Object, OdbcImportStrategy.OnDemand, query);

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
            
            OdbcImportParameterizedQueryControlViewModel testObject = mock.Create<OdbcImportParameterizedQueryControlViewModel>();
            testObject.Load(dataSource.Object, OdbcImportStrategy.OnDemand, query);

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
            
            OdbcImportParameterizedQueryControlViewModel testObject = mock.Create<OdbcImportParameterizedQueryControlViewModel>();
            testObject.Load(dataSource.Object, OdbcImportStrategy.OnDemand, query);

            Assert.False(testObject.ValidateQuery());
        }
        
        [Theory]
        [InlineData(OdbcImportStrategy.OnDemand, "Order Number")]
        [InlineData(OdbcImportStrategy.ByModifiedTime, "Last Modified Date")]
        public void ParameterizedQueryInfo_UsesCorrectParameterType(OdbcImportStrategy importStrategy, string parameterType)
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();

            OdbcImportParameterizedQueryControlViewModel testObject = mock.Create<OdbcImportParameterizedQueryControlViewModel>();
            testObject.Load(dataSource.Object, importStrategy, "my query");
            
            string expectedValue = $"Enter a query below, using a ? to represent the {parameterType} parameter. If you would like to test your query, you can use the default value or enter your own sample parameter.";
            
            Assert.Equal(expectedValue, testObject.ParameterizedQueryInfo);
        }

        [Fact]
        public void Load_SetsSampleParameterValue()
        {
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();

            OdbcImportParameterizedQueryControlViewModel testObject = mock.Create<OdbcImportParameterizedQueryControlViewModel>();
            testObject.Load(dataSource.Object, OdbcImportStrategy.OnDemand, "my query");
            
            Assert.Equal("'0'", testObject.SampleParameterValue);
        } 
        
        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}