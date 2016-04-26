using System.Data;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using System.Linq;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcDatasourceRepositoryTest
    {
        [Fact]
        public void GetDataSources_GetsDatasourceFromD()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IDsnRetriever> dsnRetriever = mock.Mock<IDsnRetriever>();
                dsnRetriever.SetupSequence(retriever => retriever.GetNextDsnName())
                    .Returns("blah")
                    .Returns(null);

                var testObject = mock.Create<OdbcDataSourceRepository>();
                var odbcDataSources = testObject.GetDataSources();
                Assert.Equal("blah", odbcDataSources.Single().Name);
            }
        }

        [Fact]
        public void GetDataSources_CollectionCountFive_WhenGetNextDsnNameReturnsFiveDsns()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IDsnRetriever> dsnRetriever = mock.Mock<IDsnRetriever>();
                dsnRetriever.SetupSequence(retriever => retriever.GetNextDsnName())
                    .Returns("1")
                    .Returns("2")
                    .Returns("3")
                    .Returns("4")
                    .Returns("5")
                    .Returns(null);

                var testObject = mock.Create<OdbcDataSourceRepository>();
                var odbcDataSources = testObject.GetDataSources();
                Assert.Equal(5, odbcDataSources.Count());
            }
        }

        public void GetDataSources_ThrowsDataException_WhenGetNextDsnNameThrowsDataException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IDsnRetriever> dsnRetriever = mock.Mock<IDsnRetriever>();
                dsnRetriever.Setup(retriever => retriever.GetNextDsnName())
                    .Throws(new DataException());

                var testObject = mock.Create<OdbcDataSourceRepository>();
                Assert.Throws<DataException>(() => testObject.GetDataSources());
            }
        }

        [Fact]
        public void Dispose_DelegatesToRetrieverDispose()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var dsnRetriever = mock.Mock<IDsnRetriever>();
                using (mock.Create<OdbcDataSourceRepository>())
                {
                    
                }

                dsnRetriever.Verify(d=>d.Dispose(), Times.Once);
            }
        }
    }
}
