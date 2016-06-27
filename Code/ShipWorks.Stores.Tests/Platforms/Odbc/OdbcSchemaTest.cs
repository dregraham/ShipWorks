using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using System;
using System.Data;
using System.Data.Common;
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
            Mock<DbConnection> connection;
            using (DataTable tableData = new DataTable())
            {
                tableData.Columns.Add("TABLE_NAME", typeof(string));
                tableData.Rows.Add("Order");

                // Mock up the connection
                connection = mock.Mock<DbConnection>();
                connection.Setup(c => c.GetSchema(It.IsAny<string>())).Returns(tableData);
                Mock<IOdbcColumnSource> columnSourceMock = new Mock<IOdbcColumnSource>();
                mock.Mock<IOdbcColumnSourceFactory>()
                    .Setup(s => s.CreateTable("Order"))
                    .Returns(columnSourceMock.Object);

                // Mock up the data source
                Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

                OdbcSchema testObject = mock.Create<OdbcSchema>();

                testObject.Load(dataSource.Object);

                Assert.Contains(columnSourceMock.Object, testObject.Tables);
            }
        }

        [Fact]
        public void Load_DelegatesToDataSource_ForConnection()
        {
            using (DataTable tableData = new DataTable())
            {
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
        }

        [Fact]
        public void Load_DelegatesToConnection_ForTables()
        {
            using (DataTable tableData = new DataTable())
            {
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

            Assert.Throws<ShipWorksOdbcException>(() => testObject.Load(dataSource.Object));

            // Verify that we pass the exception message to the log
            log.Verify(l => l.Error(ex.Object.Message));
        }

        [Fact]
        public void Load_ReThrowsShipWorksException_WhenConnectionThrowsOdbcException()
        {
            // Mock up the exception that the connection will throw
            Mock<DbException> ex = mock.Mock<DbException>();
            ex.SetupGet(e => e.Message).Returns("Some ODBC Error");

            // Mock up the connection
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.Open()).Throws(ex.Object);

            // Mock up the data source
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);
            dataSource.SetupGet(d => d.Name).Returns("shipworksodbc");

            OdbcSchema testObject = mock.Create<OdbcSchema>();
            ShipWorksOdbcException thrownEx = Assert.Throws<ShipWorksOdbcException>(() => testObject.Load(dataSource.Object));

            Assert.Equal("An error occurred while attempting to open a connection to shipworksodbc.", thrownEx.Message);
        }

        [Fact]
        public void Load_ReThrowsShipWorksException_WhenGetSchemaThrowsException()
        {
            // Mock up the connection
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.GetSchema(It.IsAny<string>())).Throws(new Exception("something went wrong"));

            // Mock up the data source
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);
            dataSource.SetupGet(d => d.Name).Returns("shipworksodbc");

            OdbcSchema testObject = mock.Create<OdbcSchema>();
            ShipWorksOdbcException thrownEx = Assert.Throws<ShipWorksOdbcException>(() => testObject.Load(dataSource.Object));

            Assert.Equal("An error occurred while attempting to retrieve a list of tables from the shipworksodbc data source.", thrownEx.Message);
        }

        [Fact]
        public void LoadWithQuery_TableNameCreatedIsCustomQuery()
        {
            Mock<IOdbcColumnSource> odbcColumnSource = new Mock<IOdbcColumnSource>();
            Mock<IOdbcColumnSourceFactory> columnSourceFactory = mock.Mock<IOdbcColumnSourceFactory>();
            columnSourceFactory.Setup(f => f.CreateTable(It.IsAny<string>()))
                .Returns(odbcColumnSource.Object);

            Mock<IOdbcDataSource> odbcDataSource = new Mock<IOdbcDataSource>();

            OdbcSchema testObject = mock.Create<OdbcSchema>();

            testObject.Load(odbcDataSource.Object, "");

            columnSourceFactory.Verify(f=>f.CreateTable("Custom Query"), Times.Once);
        }

        [Fact]
        public void LoadWithQuery_OdbcColumnSourceLoadedWithCorrectParameters()
        {
            string query = "the Query";
            Mock<IOdbcColumnSource> odbcColumnSource = new Mock<IOdbcColumnSource>();
            Mock<IOdbcColumnSourceFactory> columnSourceFactory = mock.Mock<IOdbcColumnSourceFactory>();
            columnSourceFactory.Setup(f => f.CreateTable(It.IsAny<string>()))
                .Returns(odbcColumnSource.Object);

            var dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();

            Mock<IOdbcDataSource> odbcDataSource = new Mock<IOdbcDataSource>();

            var log = mock.Mock<ILog>();

            Mock<Func<Type, ILog>> repo = mock.MockRepository.Create<Func<Type, ILog>>();
            repo.Setup(x => x(It.IsAny<Type>()))
                .Returns(log.Object);
            mock.Provide(repo.Object);


            OdbcSchema testObject = mock.Create<OdbcSchema>();
            testObject.Load(odbcDataSource.Object, query);
            
            odbcColumnSource.Verify(s => s.Load(odbcDataSource.Object, log.Object, query, dbProviderFactory.Object), Times.Once);
        }

        [Fact]
        public void LoadWithQuery_TableOnlyIncludesSchemaTable()
        {
            Mock<IOdbcColumnSource> odbcColumnSource = new Mock<IOdbcColumnSource>();
            Mock<IOdbcColumnSourceFactory> columnSourceFactory = mock.Mock<IOdbcColumnSourceFactory>();
            columnSourceFactory.Setup(f => f.CreateTable(It.IsAny<string>()))
                .Returns(odbcColumnSource.Object);

            Mock<IOdbcDataSource> odbcDataSource = new Mock<IOdbcDataSource>();

            OdbcSchema testObject = mock.Create<OdbcSchema>();

            testObject.Load(odbcDataSource.Object, "");

            Assert.Equal(new [] {odbcColumnSource.Object}, testObject.Tables);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}