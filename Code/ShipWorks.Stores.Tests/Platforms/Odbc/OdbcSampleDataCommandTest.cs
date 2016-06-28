﻿using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using System;
using System.Data;
using System.Data.Common;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcSampleDataCommandTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcSampleDataCommandTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void Execute_CreatesConnectionFromDataSource()
        {
            Mock<DbDataReader> reader = mock.Mock<DbDataReader>();
            reader.Setup(r => r.GetSchemaTable()).Returns(new DataTable());

            Mock<IShipWorksOdbcCommand> cmd = mock.Mock<IShipWorksOdbcCommand>();
            cmd.Setup(c => c.ExecuteReader(It.IsAny<CommandBehavior>())).Returns(reader.Object);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<string>(), It.IsAny<DbConnection>())).Returns(cmd.Object);

            Mock<DbConnection> conn = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);

            OdbcSampleDataCommand testObject = mock.Create<OdbcSampleDataCommand>();

            testObject.Execute(dataSource.Object, "SELECT * FROM ORDERS", 10);

            dataSource.Verify(d => d.CreateConnection());
        }

        [Fact]
        public void Execute_CallsOpenOnConnection()
        {
            Mock<DbDataReader> reader = mock.Mock<DbDataReader>();
            reader.Setup(r => r.GetSchemaTable()).Returns(new DataTable());

            Mock<IShipWorksOdbcCommand> cmd = mock.Mock<IShipWorksOdbcCommand>();
            cmd.Setup(c => c.ExecuteReader(It.IsAny<CommandBehavior>())).Returns(reader.Object);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<string>(), It.IsAny<DbConnection>())).Returns(cmd.Object);

            Mock<DbConnection> conn = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);

            OdbcSampleDataCommand testObject = mock.Create<OdbcSampleDataCommand>();

            testObject.Execute(dataSource.Object, "SELECT * FROM ORDERS", 10);

            conn.Verify(c => c.Open());
        }

        [Fact]
        public void Execute_CallsCancelOnCommand()
        {
            Mock<DbDataReader> reader = mock.Mock<DbDataReader>();
            reader.Setup(r => r.GetSchemaTable()).Returns(new DataTable());

            Mock<IShipWorksOdbcCommand> cmd = mock.Mock<IShipWorksOdbcCommand>();
            cmd.Setup(c => c.ExecuteReader(It.IsAny<CommandBehavior>())).Returns(reader.Object);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<string>(), It.IsAny<DbConnection>())).Returns(cmd.Object);

            Mock<DbConnection> conn = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);

            OdbcSampleDataCommand testObject = mock.Create<OdbcSampleDataCommand>();

            testObject.Execute(dataSource.Object, "SELECT * FROM ORDERS", 10);

            cmd.Verify(c => c.Cancel());
        }

        [Fact]
        public void Execute_CallsGetSchemaTableOnReader()
        {
            Mock<DbDataReader> reader = mock.Mock<DbDataReader>();
            reader.Setup(r => r.GetSchemaTable()).Returns(new DataTable());

            Mock<IShipWorksOdbcCommand> cmd = mock.Mock<IShipWorksOdbcCommand>();
            cmd.Setup(c => c.ExecuteReader(It.IsAny<CommandBehavior>())).Returns(reader.Object);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<string>(), It.IsAny<DbConnection>())).Returns(cmd.Object);

            Mock<DbConnection> conn = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);

            OdbcSampleDataCommand testObject = mock.Create<OdbcSampleDataCommand>();

            testObject.Execute(dataSource.Object, "SELECT * FROM ORDERS", 10);

            reader.Verify(r => r.GetSchemaTable());
        }

        [Fact]
        public void Execute_CreatesShipWorksOdbcCommandFromDbProviderFactoryWithQueryAndConnection()
        {
            Mock<DbDataReader> reader = mock.Mock<DbDataReader>();
            reader.Setup(r => r.GetSchemaTable()).Returns(new DataTable());

            Mock<IShipWorksOdbcCommand> cmd = mock.Mock<IShipWorksOdbcCommand>();
            cmd.Setup(c => c.ExecuteReader(It.IsAny<CommandBehavior>())).Returns(reader.Object);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<string>(), It.IsAny<DbConnection>())).Returns(cmd.Object);

            Mock<DbConnection> conn = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);

            OdbcSampleDataCommand testObject = mock.Create<OdbcSampleDataCommand>();

            testObject.Execute(dataSource.Object, "SELECT * FROM ORDERS", 10);

            dbProviderFactory.Verify(d => d.CreateOdbcCommand("SELECT * FROM ORDERS", conn.Object));
        }

        [Fact]
        public void Execute_CallsExecuteReaderOnCommandWtihCommandBehavior()
        {
            Mock<DbDataReader> reader = mock.Mock<DbDataReader>();
            reader.Setup(r => r.GetSchemaTable()).Returns(new DataTable());

            Mock<IShipWorksOdbcCommand> cmd = mock.Mock<IShipWorksOdbcCommand>();
            cmd.Setup(c => c.ExecuteReader(It.IsAny<CommandBehavior>())).Returns(reader.Object);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<string>(), It.IsAny<DbConnection>())).Returns(cmd.Object);

            Mock<DbConnection> conn = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);

            OdbcSampleDataCommand testObject = mock.Create<OdbcSampleDataCommand>();

            testObject.Execute(dataSource.Object, "SELECT * FROM ORDERS", 10);

            cmd.Verify(c => c.ExecuteReader(CommandBehavior.KeyInfo));
        }

        [Fact]
        public void Execute_RethrowsShipWorksOdbcException_WhenConnThrowsDbException()
        {
            Mock<DbDataReader> reader = mock.Mock<DbDataReader>();
            reader.Setup(r => r.GetSchemaTable()).Returns(new DataTable());

            Mock<IShipWorksOdbcCommand> cmd = mock.Mock<IShipWorksOdbcCommand>();
            cmd.Setup(c => c.ExecuteReader(It.IsAny<CommandBehavior>())).Returns(reader.Object);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<string>(), It.IsAny<DbConnection>())).Returns(cmd.Object);

            Mock<DbException> dbException = mock.Mock<DbException>();

            Mock<DbConnection> conn = mock.Mock<DbConnection>();
            conn.Setup(c => c.Open()).Throws(dbException.Object);

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);
            dataSource.SetupGet(d => d.Name).Returns("Data Source Name");

            OdbcSampleDataCommand testObject = mock.Create<OdbcSampleDataCommand>();

            ShipWorksOdbcException ex = Assert.Throws<ShipWorksOdbcException>(() => testObject.Execute(dataSource.Object, "SELECT * FROM ORDERS", 10));

            Assert.Contains("Data Source Name", ex.Message);
        }

        [Fact]
        public void Execute_RethrowsShipWorksOdbcException_WhenConnThrowsException()
        {
            Mock<DbDataReader> reader = mock.Mock<DbDataReader>();
            reader.Setup(r => r.GetSchemaTable()).Returns(new DataTable());

            Mock<IShipWorksOdbcCommand> cmd = mock.Mock<IShipWorksOdbcCommand>();
            cmd.Setup(c => c.ExecuteReader(It.IsAny<CommandBehavior>())).Returns(reader.Object);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateOdbcCommand(It.IsAny<string>(), It.IsAny<DbConnection>())).Returns(cmd.Object);

            Mock<DbConnection> conn = mock.Mock<DbConnection>();
            conn.Setup(c => c.Open()).Throws(new Exception("Something went wrong."));

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(conn.Object);
            dataSource.SetupGet(d => d.Name).Returns("Data Source Name");

            OdbcSampleDataCommand testObject = mock.Create<OdbcSampleDataCommand>();

            ShipWorksOdbcException ex = Assert.Throws<ShipWorksOdbcException>(() => testObject.Execute(dataSource.Object, "SELECT * FROM ORDERS", 10));

            Assert.Equal("An error occurred while attempting to retrieve columns for the custom query.", ex.Message);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
