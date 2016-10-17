using System;
using System.Data;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using log4net;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.DataSource
{
    public class OdbcDatasourceRepositoryTest
    {
        [Fact]
        public void GetDataSources_DelegatesToDsnProviderForDataSourceNames()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IDsnProvider> dsnProvider = mock.Mock<IDsnProvider>();
                dsnProvider.Setup(p => p.GetDataSourceNames())
                    .Returns(new[] { new DsnInfo("dsnName", "dsnDriver") });

                OdbcDataSourceRepository repo = mock.Create<OdbcDataSourceRepository>();
                repo.GetDataSources();

                dsnProvider.Verify(d => d.GetDataSourceNames(), Times.Once);
            }
        }

        [Fact]
        public void GetDataSources_ReturnsDataSourceProviders_WithNamesFromDsnProvider()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOdbcDataSource> dataSourceMock = mock.Mock<IOdbcDataSource>();
                dataSourceMock.SetupGet(d => d.IsCustom).Returns(true);
                dataSourceMock.SetupGet(d => d.Name).Returns("dsnName");
                dataSourceMock.SetupGet(d => d.Driver).Returns("dsnDriver");

                Mock <IDsnProvider> dsnProvider = mock.Mock<IDsnProvider>();
                dsnProvider.Setup(p => p.GetDataSourceNames())
                    .Returns(new[] { new DsnInfo("dsnName", "dsnDriver") });

                Mock<IShipWorksDbProviderFactory> providerFactory = mock.Mock<IShipWorksDbProviderFactory>();
                Mock<IEncryptionProviderFactory> encryptionFactory = mock.Mock<IEncryptionProviderFactory>();
                Func<IOdbcDataSource> odbcDataSourceFactory = () => new EncryptedOdbcDataSource(providerFactory.Object, encryptionFactory.Object);
                var testObject = mock.Create<OdbcDataSourceRepository>(new TypedParameter(typeof(Func<IOdbcDataSource>), odbcDataSourceFactory));
                var odbcDataSources = testObject.GetDataSources();

                IOdbcDataSource dataSource = odbcDataSources.First(d => d.Name == "dsnName");

                Assert.Equal("dsnName", dataSource.Name);
                Assert.Equal("dsnDriver", dataSource.Driver);
            }
        }

        [Fact]
        public void GetDataSources_ReturnsCollectionOfFiveDataSources_WhenGetNextDsnNameReturnsFiveDsns()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IDsnProvider> dsnProvider = mock.Mock<IDsnProvider>();
                dsnProvider.Setup(p => p.GetDataSourceNames())
                    .Returns(new[] { new DsnInfo("1", "driver 1"), new DsnInfo("2", "driver 2"), new DsnInfo("3", "driver 3"), new DsnInfo("4", "driver 4"), new DsnInfo("5", "driver 5") });

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

                Mock<IDsnProvider> dsnProvider = mock.Mock<IDsnProvider>();
                dsnProvider.Setup(dp => dp.GetDataSourceNames())
                    .Throws(new DataException());

                var testObject = mock.Create<OdbcDataSourceRepository>();

                Assert.Throws<DataException>(() => testObject.GetDataSources());
                log.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()));
            }
        }
    }
}
