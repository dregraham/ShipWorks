using Autofac;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcColumnSourceTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcColumnSourceTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Ctor_SetsName()
        {
            OdbcColumnSource testObject = mock.Create<OdbcColumnSource>(new TypedParameter(typeof(string), "SomeTableName"));

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

            OdbcColumnSource testObject = mock.Create<OdbcColumnSource>(new TypedParameter(typeof(string), "SomeTableName"));
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

            OdbcColumnSource testObject = mock.Create<OdbcColumnSource>(new TypedParameter(typeof(string), "SomeTableName"));

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

            OdbcColumnSource testObject = mock.Create<OdbcColumnSource>(new TypedParameter(typeof(string), "SomeTableName"));

            ShipWorksOdbcException thrownException = Assert.Throws<ShipWorksOdbcException>(() => testObject.Load(dataSource.Object, log.Object));
            Assert.Equal("An error occurred while attempting to open a connection to shipworksodbc.", thrownException.Message);
        }

        [Fact]
        public void Load_RethrowsShipWorksOdbcException_WhenDbConnectionThrowsDbException()
        {
            Mock<DbException> exception = mock.Mock<DbException>();
            exception.SetupGet(ex => ex.Message).Returns("Something went wrong");

            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.Open()).Throws(exception.Object);
            Mock<ILog> log = mock.Mock<ILog>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.SetupGet(d => d.Name).Returns("SomeName");
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcColumnSource table = new OdbcColumnSource("Orders");

            Assert.Throws<ShipWorksOdbcException>(() => table.Load(dataSource.Object, log.Object));

            log.Verify(l => l.Error(exception.Object));
        }

        [Fact]
        public void Load_RethrowsShipWorksOdbcException_WhenDbConnectionThrowsGeneralException()
        {
            Exception ex = new Exception("Something went wrong");
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.Open()).Throws(ex);
            Mock<ILog> log = mock.Mock<ILog>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.SetupGet(d => d.Name).Returns("SomeName");
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcColumnSource table = new OdbcColumnSource("Orders");

            Assert.Throws<ShipWorksOdbcException>(() => table.Load(dataSource.Object, log.Object));

            log.Verify(l => l.Error(ex));
        }

        [Fact]
        public void Load_OpensConnection()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("foo", typeof(string));
            dataTable.Columns.Add("bar", typeof(string));
            dataTable.Columns.Add("baz", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Rows.Add(string.Empty, string.Empty, string.Empty, "ColumnName1");
            dataTable.Rows.Add(string.Empty, string.Empty, string.Empty, "ColumnName2");

            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.GetSchema(It.IsAny<string>(), It.IsAny<string[]>())).Returns(dataTable);
            Mock<ILog> log = mock.Mock<ILog>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.SetupGet(d => d.Name).Returns("SomeName");
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcColumnSource table = new OdbcColumnSource("Orders");

            table.Load(dataSource.Object, log.Object);
            dataTable.Dispose();
            connection.Verify(c => c.Open());
        }

        [Fact]
        public void LoadWithProviderFactory_RethrowsShipWorksOdbcException_WhenDbConnectionThrowsDbException()
        {
            Mock<DbException> exception = mock.Mock<DbException>();
            exception.SetupGet(ex => ex.Message).Returns("Something went wrong");

            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.Open()).Throws(exception.Object);
            Mock<ILog> log = mock.Mock<ILog>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.SetupGet(d => d.Name).Returns("SomeName");
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();

            OdbcColumnSource table = new OdbcColumnSource("Orders");

            Assert.Throws<ShipWorksOdbcException>(() => table.Load(dataSource.Object, log.Object, "SELECT * FROM [Order]", dbProviderFactory.Object));

            log.Verify(l => l.Error(exception.Object));
        }

        [Fact]
        public void LoadWithProviderFactory_RethrowsShipWorksOdbcException_WhenDbConnectionThrowsGeneralException()
        {
            Exception ex = new Exception("Something went wrong");
            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.Open()).Throws(ex);
            Mock<ILog> log = mock.Mock<ILog>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.SetupGet(d => d.Name).Returns("SomeName");
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();

            OdbcColumnSource table = new OdbcColumnSource("Orders");

            Assert.Throws<ShipWorksOdbcException>(() => table.Load(dataSource.Object, log.Object, "SELECT * FROM [Order]", dbProviderFactory.Object));

            log.Verify(l => l.Error(ex));
        }

        [Fact]
        public void LoadWithProviderFactory_OpensConnection()
        {
            DataTable dataTable = new DataTable();

            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.GetSchema(It.IsAny<string>(), It.IsAny<string[]>())).Returns(dataTable);
            Mock<ILog> log = mock.Mock<ILog>();
            Mock<DbDataReader> reader = mock.Mock<DbDataReader>();
            reader.Setup(r => r.GetSchemaTable()).Returns(dataTable);
            Mock<IShipWorksOdbcCommand> cmd = mock.Mock<IShipWorksOdbcCommand>();
            cmd.Setup(c => c.ExecuteReader(CommandBehavior.SchemaOnly)).Returns(reader.Object);
            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<string>(), It.IsAny<DbConnection>())).Returns(cmd.Object);

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.SetupGet(d => d.Name).Returns("SomeName");
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcColumnSource table = new OdbcColumnSource("Orders");

            table.Load(dataSource.Object, log.Object, "SELECT * FROM [Order]", dbProviderFactory.Object);
            dataTable.Dispose();
            connection.Verify(c => c.Open());
        }

        [Fact]
        public void LoadWithProviderFactory_SetsColumnsFromReaderGetSchemaTable()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("BaseColumnName", typeof(string));
            dataTable.Rows.Add("ColumnName1");
            dataTable.Rows.Add("ColumnName2");

            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.GetSchema(It.IsAny<string>(), It.IsAny<string[]>())).Returns(dataTable);
            Mock<ILog> log = mock.Mock<ILog>();
            Mock<DbDataReader> reader = mock.Mock<DbDataReader>();
            reader.Setup(r => r.GetSchemaTable()).Returns(dataTable);
            Mock<IShipWorksOdbcCommand> cmd = mock.Mock<IShipWorksOdbcCommand>();
            cmd.Setup(c => c.ExecuteReader(CommandBehavior.SchemaOnly)).Returns(reader.Object);
            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<string>(), It.IsAny<DbConnection>())).Returns(cmd.Object);

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.SetupGet(d => d.Name).Returns("SomeName");
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcColumnSource table = new OdbcColumnSource("Orders");

            table.Load(dataSource.Object, log.Object, "SELECT * FROM [Order]", dbProviderFactory.Object);

            dataTable.Dispose();

            Assert.True(table.Columns.Count(c => c.Name == "ColumnName1") == 1);
            Assert.True(table.Columns.Count(c => c.Name == "ColumnName2") == 1);
        }

        [Fact]
        public void Load_GetsSchemaFromConnection()
        {
            string[] restriction =
                {
                    null, // table_catalog
                    null, // table_schema
                    "Orders", // table_name
                    null // table_type
                };

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("foo", typeof(string));
            dataTable.Columns.Add("bar", typeof(string));
            dataTable.Columns.Add("baz", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Rows.Add(string.Empty, string.Empty, string.Empty, "ColumnName1");
            dataTable.Rows.Add(string.Empty, string.Empty, string.Empty, "ColumnName2");

            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.GetSchema(It.IsAny<string>(), It.IsAny<string[]>())).Returns(dataTable);
            Mock<ILog> log = mock.Mock<ILog>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.SetupGet(d => d.Name).Returns("SomeName");
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcColumnSource table = new OdbcColumnSource("Orders");

            table.Load(dataSource.Object, log.Object);
            dataTable.Dispose();
            connection.Verify(c => c.GetSchema("Columns", restriction));
        }

        [Fact]
        public void Load_SetsColumnsFromConnectionGetSchema()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("foo", typeof(string));
            dataTable.Columns.Add("bar", typeof(string));
            dataTable.Columns.Add("baz", typeof(string));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Rows.Add(string.Empty, string.Empty, string.Empty, "ColumnName1");
            dataTable.Rows.Add(string.Empty, string.Empty, string.Empty, "ColumnName2");

            Mock<DbConnection> connection = mock.Mock<DbConnection>();
            connection.Setup(c => c.GetSchema(It.IsAny<string>(), It.IsAny<string[]>())).Returns(dataTable);
            Mock<ILog> log = mock.Mock<ILog>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.SetupGet(d => d.Name).Returns("SomeName");
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcColumnSource table = new OdbcColumnSource("Orders");

            table.Load(dataSource.Object, log.Object);

            dataTable.Dispose();

            Assert.True(table.Columns.Count(c => c.Name == "ColumnName1") == 1);
            Assert.True(table.Columns.Count(c => c.Name == "ColumnName2") == 1);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}