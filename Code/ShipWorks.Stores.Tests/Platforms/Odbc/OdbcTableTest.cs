using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcTableTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcTableTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Ctor_SetsName()
        {
            OdbcTable testObject = mock.Create<OdbcTable>(new TypedParameter(typeof(string), "SomeTableName"));

            Assert.Equal("SomeTableName", testObject.Name);
        }

        [Fact]
        public void Columns_GetsConnectionFromDataSourceConnection()
        {
            // mock the connection
            Mock<DbConnection> conn = mock.Mock<DbConnection>();
            conn.Setup(c => c.GetSchema(It.IsAny<string>(), It.IsAny<string[]>())).Returns(new DataTable());

            // mock the data source
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);

            // mock the log
            Mock<ILog> log = mock.Mock<ILog>();

            // mock the schema
            Mock<IOdbcSchema> schema = mock.Mock<IOdbcSchema>();
            schema.Object.Load(dataSource.Object);

            OdbcTable testObject = mock.Create<OdbcTable>(new TypedParameter(typeof(string), "SomeTableName"));
            testObject.Load(dataSource.Object, log.Object);

            dataSource.Verify(d => d.CreateConnection());
        }

        [Fact]
        public void Columns_RethrowsShipWorksOdbcException_WhenConnectionOpenThrowsDbException()
        {
            // Mock the exception
            Mock<DbException> ex = mock.Mock<DbException>();
            ex.SetupGet(e => e.Message).Returns("Some database related exception");

            // mock the connection
            Mock<DbConnection> conn = mock.Mock<DbConnection>();
            conn.Setup(c => c.Open()).Throws(ex.Object);

            // mock the log
            Mock<ILog> log = mock.Mock<ILog>();

            // mock the data source
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);
            dataSource.SetupGet(d => d.Name).Returns("shipworksodbc");

            OdbcTable testObject = mock.Create<OdbcTable>(new TypedParameter(typeof(string), "SomeTableName"));

            ShipWorksOdbcException thrownException = Assert.Throws<ShipWorksOdbcException>(() => testObject.Load(dataSource.Object, log.Object));
            Assert.Equal("An error occurred while attempting to open a connection to shipworksodbc.", thrownException.Message);
        }

        [Fact]
        public void Columns_RethrowsShipWorksOdbcException_WhenConnectionGetSchemaThrowsDbException()
        {
            // Mock the exception
            Mock<DbException> ex = mock.Mock<DbException>();
            ex.SetupGet(e => e.Message).Returns("Some database related exception");

            // mock the connection
            Mock<DbConnection> conn = mock.Mock<DbConnection>();
            conn.Setup(c => c.GetSchema(It.IsAny<string>(), It.IsAny<string[]>())).Throws(ex.Object);

            // mock the log
            Mock<ILog> log = mock.Mock<ILog>();

            // mock the data source
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);
            dataSource.SetupGet(d => d.Name).Returns("shipworksodbc");

            OdbcTable testObject = mock.Create<OdbcTable>(new TypedParameter(typeof(string), "SomeTableName"));

            ShipWorksOdbcException thrownException = Assert.Throws<ShipWorksOdbcException>(() => testObject.Load(dataSource.Object, log.Object));
            Assert.Equal("An error occurred while attempting to open a connection to shipworksodbc.", thrownException.Message);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}