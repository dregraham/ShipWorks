using System;
using Autofac.Extras.Moq;
using ShipWorks.Stores.Platforms.Odbc;
using System.Data;
using System.Data.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcShipWorksDbProviderFactoryTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcShipWorksDbProviderFactoryTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void CreateOdbcConnection_ReturnsOdbcConnection()
        {
            OdbcShipWorksDbProviderFactory testObject = mock.Create<OdbcShipWorksDbProviderFactory>();
            using (IDbConnection connection = testObject.CreateOdbcConnection())
            {
                Assert.IsAssignableFrom<OdbcConnection>(connection);
            }
        }

        [Fact]
        public void CreateOdbcConnection_ReturnsOdbcConnection_WhenConnectionStringProvided()
        {
            OdbcShipWorksDbProviderFactory testObject = mock.Create<OdbcShipWorksDbProviderFactory>();
            using (IDbConnection connection = testObject.CreateOdbcConnection("dsn=blah"))
            {
                Assert.IsAssignableFrom<OdbcConnection>(connection);
            }
        }

        [Fact]
        public void CreateOdbcConnection_SetsConnectionStringOfConnection_WhenConnectionStringProvided()
        {
            string connectionString = "dsn=blah";
            OdbcShipWorksDbProviderFactory testObject = mock.Create<OdbcShipWorksDbProviderFactory>();
            using (IDbConnection connection = testObject.CreateOdbcConnection(connectionString))
            {
                Assert.Equal(connectionString, connection.ConnectionString);
            }
        }

        [Fact]
        public void CreateOdbcCommand_ReturnsShipworksOdbcCommand()
        {
            OdbcShipWorksDbProviderFactory testObject = mock.Create<OdbcShipWorksDbProviderFactory>();

            using (var connection = testObject.CreateOdbcConnection())
            using (var command = testObject.CreateOdbcCommand("query", connection))
            {
                Assert.IsType<ShipWorksOdbcCommand>(command);
            }
        }

        [Fact]
        public void CreateShipworksOdbcCommandBuilder_ReturnsShipworksOdbcCommandBuilder()
        {
            using (var mock = AutoMock.GetLoose())
            using (var adapter = new OdbcDataAdapter())
            {
                var shipworksDataAdapter = mock.Mock<IShipWorksOdbcDataAdapter>();
                shipworksDataAdapter.SetupGet(a => a.Adapter).Returns(adapter);

                OdbcShipWorksDbProviderFactory testObject = mock.Create<OdbcShipWorksDbProviderFactory>();
                IShipWorksOdbcCommandBuilder shipWorksOdbcCommandBuilder =
                    testObject.CreateShipWorksOdbcCommandBuilder(shipworksDataAdapter.Object);

                Assert.IsType<ShipWorksOdbcCommandBuilder>(shipWorksOdbcCommandBuilder);
            }
        }

        [Fact]
        public void CreateShipworksOdbcDataAdapter_ReturnsShipworksOdbcDataAdapter()
        {
            OdbcShipWorksDbProviderFactory testObject = mock.Create<OdbcShipWorksDbProviderFactory>();
            var shipWorksOdbcDataAdapter = testObject.CreateShipWorksOdbcDataAdapter(string.Empty, null);

            Assert.IsType<ShipWorksOdbcDataAdapter>(shipWorksOdbcDataAdapter);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}

