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
        public void GetDataSources_ReturnsDataSourceProviders_WithNamesFromDsnProvider()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IDsnProvider> dsnProvider = mock.Mock<IDsnProvider>();
                dsnProvider.Setup(p => p.GetDataSourceNames())
                    .Returns(new[] {"blah"});

                var testObject = mock.Create<OdbcDataSourceRepository>();
                var odbcDataSources = testObject.GetDataSources();
                Assert.Equal("blah", odbcDataSources.Single().Name);
            }
        }

        [Fact]
        public void GetDataSources_ReturnsCollectionOfFiveDataSources_WhenGetNextDsnNameReturnsFiveDsns()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IDsnProvider> dsnProvider = mock.Mock<IDsnProvider>();
                dsnProvider.Setup(p => p.GetDataSourceNames())
                    .Returns(new[] {"1", "2", "3", "4", "5"});

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
                Mock<IDsnProvider> dsnProvider = mock.Mock<IDsnProvider>();
                dsnProvider.Setup(provider => provider.GetDataSourceNames())
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

                Mock<IDsnProvider> dsnRetriever = mock.Mock<IDsnProvider>();
                dsnRetriever.Setup(retriever => retriever.GetDataSourceNames())
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

                log.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()));
            }
        }
    }
}
