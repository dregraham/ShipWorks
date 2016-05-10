using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcSchemaTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcSchemaTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Load_PopulatesTables()
        {
            DataTable tableData = new DataTable();
            tableData.Columns.Add("TABLE_NAME", typeof(string));
            tableData.Rows.Add("Order");

            // Mock up the connection
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.GetSchema(It.IsAny<string>())).Returns(tableData);

            // Mock up the data source
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcSchema testObject = mock.Create<OdbcSchema>();

            testObject.Load(dataSource.Object);

            Assert.Equal(1, testObject.Tables.Count(t => t.Name == "Order"));
        }

        [Fact]
        public void Load_DelegatesToDataSource_ForConnection()
        {
            DataTable tableData = new DataTable();
            tableData.Columns.Add("TABLE_NAME", typeof(string));
            tableData.Rows.Add("Order");

            // Mock up the connection
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.GetSchema(It.IsAny<string>())).Returns(tableData);

            // Mock up the data source
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcSchema testObject = mock.Create<OdbcSchema>();

            testObject.Load(dataSource.Object);

            dataSource.Verify(d => d.CreateConnection(), Times.Once);
        }

        [Fact]
        public void Load_DelegatesToConnection_ForTables()
        {
            DataTable tableData = new DataTable();
            tableData.Columns.Add("TABLE_NAME", typeof(string));
            tableData.Rows.Add("Order");

            // Mock up the connection
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.GetSchema(It.IsAny<string>())).Returns(tableData);

            // Mock up the data source
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcSchema testObject = mock.Create<OdbcSchema>();

            testObject.Load(dataSource.Object);

            connection.Verify(c => c.GetSchema("Tables"), Times.Once);
        }

        [Fact]
        public void Load_LogsException_WhenConnectionThrowsOdbcException()
        {
            // Mock up the log factory
            Mock<ILog> log = mock.Mock<ILog>();

            // Mock up the exception that the connection will throw
            Mock<DbException> ex = mock.Mock<DbException>();
            ex.SetupGet(e => e.Message).Returns("Some ODBC Error");

            // Mock up the connection
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.Open()).Throws(ex.Object);

            // Mock up the data source
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcSchema testObject = mock.Create<OdbcSchema>();

            testObject.Load(dataSource.Object);

            // Verify that we pass the exception message to the log
            log.Verify(l => l.Error(ex.Object.Message));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}