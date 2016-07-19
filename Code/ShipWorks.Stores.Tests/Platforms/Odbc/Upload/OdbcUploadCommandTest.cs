using System;
using System.Data.Common;
using System.Data.Odbc;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Upload
{
    public class OdbcUploadCommandTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcUploadCommandTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Execute_CreatesConnectionFromDataSource()
        {
            Mock<DbConnection> conn = mock.Mock<DbConnection>();
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();
            command.Setup(c => c.ExecuteNonQuery()).Returns(4);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<DbConnection>())).Returns(command.Object);

            OdbcUploadCommand testObject = mock.Create<OdbcUploadCommand>();
            testObject.Execute();

            dataSource.Verify(d => d.CreateConnection());
        }

        [Fact]
        public void Execute_OpensConnection()
        {
            Mock<DbConnection> conn = mock.Mock<DbConnection>();
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();
            command.Setup(c => c.ExecuteNonQuery()).Returns(4);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<DbConnection>())).Returns(command.Object);

            OdbcUploadCommand testObject = mock.Create<OdbcUploadCommand>();
            testObject.Execute();

            conn.Verify(c => c.Open());
        }

        [Fact]
        public void Execute_CreatesCommandFromDbProviderFactory_WithConnection()
        {
            Mock<DbConnection> conn = mock.Mock<DbConnection>();
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();
            command.Setup(c => c.ExecuteNonQuery()).Returns(4);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<DbConnection>())).Returns(command.Object);

            OdbcUploadCommand testObject = mock.Create<OdbcUploadCommand>();
            testObject.Execute();

            dbProviderFactory.Verify(d => d.CreateOdbcCommand(conn.Object));
        }

        [Fact]
        public void Execute_ConfiguresCommand()
        {
            Mock<IOdbcQuery> query = mock.Mock<IOdbcQuery>();

            Mock<DbConnection> conn = mock.Mock<DbConnection>();
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();
            command.Setup(c => c.ExecuteNonQuery()).Returns(4);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<DbConnection>())).Returns(command.Object);

            OdbcUploadCommand testObject = mock.Create<OdbcUploadCommand>();
            testObject.Execute();

            query.Verify(v => v.ConfigureCommand(command.Object));
        }

        [Fact]
        public void Execute_CallsExecuteNonQueryOnShopworksOdbcCommand()
        {
            Mock<DbConnection> conn = mock.Mock<DbConnection>();
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();
            command.Setup(c => c.ExecuteNonQuery()).Returns(4);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<DbConnection>())).Returns(command.Object);

            OdbcUploadCommand testObject = mock.Create<OdbcUploadCommand>();
            testObject.Execute();

            command.Verify(c=> c.ExecuteNonQuery());
        }

        [Fact]
        public void Execute_RethrowsOdbcException()
        {
            Mock<DbException> exception = mock.Mock<DbException>();
            exception.SetupGet(e => e.Message).Returns("Some random exception");

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Throws(exception.Object);

            OdbcUploadCommand testObject = mock.Create<OdbcUploadCommand>();

            ShipWorksOdbcException ex = Assert.Throws<ShipWorksOdbcException>(() => testObject.Execute());
            Assert.Equal(exception.Object.Message, ex.Message);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}