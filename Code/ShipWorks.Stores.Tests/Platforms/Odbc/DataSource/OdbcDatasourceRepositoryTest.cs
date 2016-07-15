﻿using System;
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
                    .Returns(new[] { "blah" });

                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();

                Mock<IOdbcDataSourceService> dataSourceFactory = mock.Mock<IOdbcDataSourceService>();
                dataSourceFactory.Setup(d => d.CreateEmptyDataSource()).Returns(dataSource.Object);

                OdbcDataSourceRepository repo = mock.Create<OdbcDataSourceRepository>();
                repo.GetDataSources();

                dsnProvider.Verify(d => d.GetDataSourceNames(), Times.Once);
            }
        }

        [Fact]
        public void GetDataSources_ReturnsDataSourceProviders_WithOnlyOneCustomDataSource()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.SetupGet(d => d.IsCustom).Returns(true);

                Mock<IOdbcDataSourceService> dataSourceFactory = mock.Mock<IOdbcDataSourceService>();
                dataSourceFactory.Setup(d => d.CreateEmptyDataSource()).Returns(dataSource.Object);

                Mock<IShipWorksDbProviderFactory> providerFactory = mock.Mock<IShipWorksDbProviderFactory>();
                Mock<IEncryptionProviderFactory> encryptionFactory = mock.Mock<IEncryptionProviderFactory>();
                Func<IOdbcDataSource> odbcDataSourceFactory = () => new EncryptedOdbcDataSource(providerFactory.Object, encryptionFactory.Object);
                var testObject = mock.Create<OdbcDataSourceRepository>(new TypedParameter(typeof(Func<IOdbcDataSource>), odbcDataSourceFactory));
                var odbcDataSources = testObject.GetDataSources();

                Assert.Equal(1, odbcDataSources.Count(d => d.IsCustom));
            }
        }

        [Fact]
        public void GetDataSources_ReturnsDataSourceProviders_WithNamesFromDsnProvider()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOdbcDataSource> dataSourcemock = mock.Mock<IOdbcDataSource>();
                dataSourcemock.SetupGet(d => d.IsCustom).Returns(true);
                dataSourcemock.SetupGet(d => d.Name).Returns("blah");

                Mock<IOdbcDataSourceService> dataSourceFactory = mock.Mock<IOdbcDataSourceService>();
                dataSourceFactory.Setup(d => d.CreateEmptyDataSource()).Returns(dataSourcemock.Object);

                Mock<IDsnProvider> dsnProvider = mock.Mock<IDsnProvider>();
                dsnProvider.Setup(p => p.GetDataSourceNames())
                    .Returns(new[] {"blah"});
                Mock<IShipWorksDbProviderFactory> providerFactory = mock.Mock<IShipWorksDbProviderFactory>();
                Mock<IEncryptionProviderFactory> encryptionFactory = mock.Mock<IEncryptionProviderFactory>();
                Func<IOdbcDataSource> odbcDataSourceFactory = () => new EncryptedOdbcDataSource(providerFactory.Object, encryptionFactory.Object);
                var testObject = mock.Create<OdbcDataSourceRepository>(new TypedParameter(typeof(Func<IOdbcDataSource>), odbcDataSourceFactory));
                var odbcDataSources = testObject.GetDataSources();
                IOdbcDataSource dataSource = odbcDataSources.First(d => d.Name == "blah");

                Assert.Equal("blah", dataSource.Name);
            }
        }

        [Fact]
        public void GetDataSources_ReturnsCollectionOfFiveDataSourcesPlusCustom_WhenGetNextDsnNameReturnsFiveDsns()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IOdbcDataSourceService> dataSourceFactory = mock.Mock<IOdbcDataSourceService>();
                dataSourceFactory.Setup(d => d.CreateEmptyDataSource()).Returns(mock.Mock<IOdbcDataSource>().Object);

                Mock<IDsnProvider> dsnProvider = mock.Mock<IDsnProvider>();
                dsnProvider.Setup(p => p.GetDataSourceNames())
                    .Returns(new[] {"1", "2", "3", "4", "5"});

                var testObject = mock.Create<OdbcDataSourceRepository>();
                var odbcDataSources = testObject.GetDataSources();

                Assert.Equal(6, odbcDataSources.Count());
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
