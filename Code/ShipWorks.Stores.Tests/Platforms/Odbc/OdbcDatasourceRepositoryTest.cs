using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using System;
using System.Data;
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
                Mock<IDnsProvider> dsnRetriever = mock.Mock<IDnsProvider>();
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
                Mock<IDnsProvider> dsnRetriever = mock.Mock<IDnsProvider>();
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

        [Fact]
        public void GetDataSources_ThrowsDataException_WhenGetNextDsnNameThrowsDataException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IDnsProvider> dsnRetriever = mock.Mock<IDnsProvider>();
                dsnRetriever.Setup(retriever => retriever.GetNextDsnName())
                    .Throws(new DataException());

                var testObject = mock.Create<OdbcDataSourceRepository>();
                Assert.Throws<DataException>(() => testObject.GetDataSources());
            }
        }

        [Fact]
        public void GetDataSources_LogsException_WhenGetNextDsnNameThrowsDataException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var log = mock.Mock<ILog>();

                Mock<Func<Type, ILog>> repo = mock.MockRepository.Create<Func<Type, ILog>>();
                repo.Setup(x => x(It.IsAny<Type>()))
                    .Returns(log.Object);
                mock.Provide(repo.Object);

                Mock<IDnsProvider> dsnRetriever = mock.Mock<IDnsProvider>();
                dsnRetriever.Setup(retriever => retriever.GetNextDsnName())
                    .Throws(new DataException());

                var testObject = mock.Create<OdbcDataSourceRepository>();

                try
                {
                    testObject.GetDataSources();
                }
                catch (DataException)
                {
                    // suppress
                }

                //log.Verify(l => l.Error(It.Is<string>(s => s == "Error in GetNextDsnName"), It.IsAny<DataException>()));
                log.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()));
            }
        }

        [Fact]
        public void Dispose_DelegatesToRetrieverDispose()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var dsnRetriever = mock.Mock<IDnsProvider>();
                using (mock.Create<OdbcDataSourceRepository>())
                {
                    
                }

                dsnRetriever.Verify(d=>d.Dispose(), Times.Once);
            }
        }
    }
}
