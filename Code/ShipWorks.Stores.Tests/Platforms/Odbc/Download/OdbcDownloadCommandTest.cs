using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Autofac.Extras.Moq;
using log4net;
using Moq;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Download
{
    public class OdbcDownloadCommandTest
    {
        [Fact]
        public void Execute_ReturnsEmptyList_WhenDataReaderResultSetIsEmpty()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connection = mock.Mock<DbConnection>();
                connection.Setup(c => c.Open());

                var dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

                var reader = mock.Mock<DbDataReader>();
                reader.SetupSequence(r => r.Read())
                    .Returns(false);
                reader.Setup(r => r.FieldCount).Returns(0);
                reader.Setup(r => r.IsDBNull(It.IsAny<int>())).Returns(false);
                reader.Setup(r => r[It.IsAny<int>()]).Returns("value");
                reader.Setup(r => r.GetName(It.IsAny<int>())).Returns("name");

                var command = mock.Mock<IShipWorksOdbcCommand>();
                command.Setup(c => c.ExecuteReader()).Returns(reader.Object);

                var commandBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
                commandBuilder.Setup(b => b.QuoteIdentifier(It.IsAny<string>())).Returns<string>(x => $"\'{x}\'");

                var dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
                dbProviderFactory.Setup(f => f.CreateOdbcCommand(It.IsAny<DbConnection>()))
                    .Returns(command.Object);
                dbProviderFactory.Setup(f => f.CreateShipWorksOdbcCommandBuilder(It.IsAny<ShipWorksOdbcDataAdapter>()))
                    .Returns(commandBuilder.Object);

                var column = new OdbcColumn("Column Name");

                var externalField = mock.Mock<IExternalOdbcMappableField>();
                externalField.Setup(f => f.Column).Returns(column);

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ExternalField).Returns(externalField.Object);

                var map = mock.Mock<IOdbcFieldMap>();
                map.Setup(m => m.RecordIdentifierSource).Returns("Record ID");
                map.Setup(m => m.Entries).Returns(new List<IOdbcFieldMapEntry> { mapEntry.Object });

                var testObject = mock.Create<OdbcDownloadCommand>();

                Assert.Empty(testObject.Execute());
            }
        }

        [Fact]
        public void Execute_DoesNotAddColumnToRecord_WhenColumnValueIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connection = mock.Mock<DbConnection>();
                connection.Setup(c => c.Open());

                var dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

                var reader = mock.Mock<DbDataReader>();
                reader.SetupSequence(r => r.Read())
                    .Returns(true)
                    .Returns(false);
                reader.Setup(r => r.FieldCount).Returns(2);
                reader.SetupSequence(r => r.IsDBNull(It.IsAny<int>()))
                    .Returns(false)
                    .Returns(true);
                reader.SetupSequence(r => r[It.IsAny<int>()])
                    .Returns("value 1")
                    .Returns(null);
                reader.SetupSequence(r => r.GetName(It.IsAny<int>()))
                    .Returns("column 1")
                    .Returns("column 2");

                var command = mock.Mock<IShipWorksOdbcCommand>();
                command.Setup(c => c.ExecuteReader()).Returns(reader.Object);

                var commandBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
                commandBuilder.Setup(b => b.QuoteIdentifier(It.IsAny<string>())).Returns<string>(x => $"\'{x}\'");

                var dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
                dbProviderFactory.Setup(f => f.CreateOdbcCommand(It.IsAny<DbConnection>()))
                    .Returns(command.Object);
                dbProviderFactory.Setup(f => f.CreateShipWorksOdbcCommandBuilder(It.IsAny<ShipWorksOdbcDataAdapter>()))
                    .Returns(commandBuilder.Object);

                var column = new OdbcColumn("Column Name");

                var externalField = mock.Mock<IExternalOdbcMappableField>();
                externalField.Setup(f => f.Column).Returns(column);

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ExternalField).Returns(externalField.Object);

                var map = mock.Mock<IOdbcFieldMap>();
                map.Setup(m => m.RecordIdentifierSource).Returns("Record ID");
                map.Setup(m => m.Entries).Returns(new List<IOdbcFieldMapEntry> { mapEntry.Object });

                var testObject = mock.Create<OdbcDownloadCommand>();
                var records = testObject.Execute();

                Assert.Null(records.FirstOrDefault().GetValue("column 2"));
            }
        }

        [Fact]
        public void Execute_AddsRecord_WhenRecordContainsAtLeastOneField()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connection = mock.Mock<DbConnection>();
                connection.Setup(c => c.Open());

                var dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

                var reader = mock.Mock<DbDataReader>();
                reader.SetupSequence(r => r.Read())
                    .Returns(true)
                    .Returns(false);
                reader.Setup(r => r.FieldCount).Returns(1);
                reader.Setup(r => r.IsDBNull(It.IsAny<int>())).Returns(false);
                reader.Setup(r => r[It.IsAny<int>()]).Returns("value");
                reader.Setup(r => r.GetName(It.IsAny<int>())).Returns("Column");

                var command = mock.Mock<IShipWorksOdbcCommand>();
                command.Setup(c => c.ExecuteReader()).Returns(reader.Object);

                var commandBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
                commandBuilder.Setup(b => b.QuoteIdentifier(It.IsAny<string>())).Returns<string>(x => $"\'{x}\'");

                var dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
                dbProviderFactory.Setup(f => f.CreateOdbcCommand(It.IsAny<DbConnection>()))
                    .Returns(command.Object);
                dbProviderFactory.Setup(f => f.CreateShipWorksOdbcCommandBuilder(It.IsAny<ShipWorksOdbcDataAdapter>()))
                    .Returns(commandBuilder.Object);

                var column = new OdbcColumn("Column Name");

                var externalField = mock.Mock<IExternalOdbcMappableField>();
                externalField.Setup(f => f.Column).Returns(column);

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ExternalField).Returns(externalField.Object);

                var map = mock.Mock<IOdbcFieldMap>();
                map.Setup(m => m.RecordIdentifierSource).Returns("Record ID");
                map.Setup(m => m.Entries).Returns(new List<IOdbcFieldMapEntry> { mapEntry.Object });

                var testObject = mock.Create<OdbcDownloadCommand>();
                var records = testObject.Execute();

                Assert.Equal(1, records.Count());
            }
        }

        [Fact]
        public void Execute_SetsRecordIdentifier_WhenColumnNameMatchesRecordIdentifierSource()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connection = mock.Mock<DbConnection>();
                connection.Setup(c => c.Open());

                var dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

                var reader = mock.Mock<DbDataReader>();
                reader.SetupSequence(r => r.Read())
                    .Returns(true)
                    .Returns(false);
                reader.Setup(r => r.FieldCount).Returns(1);
                reader.Setup(r => r.IsDBNull(It.IsAny<int>())).Returns(false);
                reader.Setup(r => r[It.IsAny<int>()]).Returns("Record ID value");
                reader.Setup(r => r.GetName(It.IsAny<int>())).Returns("Record ID");

                var command = mock.Mock<IShipWorksOdbcCommand>();
                command.Setup(c => c.ExecuteReader()).Returns(reader.Object);

                var commandBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
                commandBuilder.Setup(b => b.QuoteIdentifier(It.IsAny<string>())).Returns<string>(x => $"\'{x}\'");

                var dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
                dbProviderFactory.Setup(f => f.CreateOdbcCommand(It.IsAny<DbConnection>()))
                    .Returns(command.Object);
                dbProviderFactory.Setup(f => f.CreateShipWorksOdbcCommandBuilder(It.IsAny<ShipWorksOdbcDataAdapter>()))
                    .Returns(commandBuilder.Object);

                var column = new OdbcColumn("Column Name");

                var externalField = mock.Mock<IExternalOdbcMappableField>();
                externalField.Setup(f => f.Column).Returns(column);

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ExternalField).Returns(externalField.Object);

                var map = mock.Mock<IOdbcFieldMap>();
                map.Setup(m => m.RecordIdentifierSource).Returns("Record ID");
                map.Setup(m => m.Entries).Returns(new List<IOdbcFieldMapEntry> { mapEntry.Object });

                var testObject = mock.Create<OdbcDownloadCommand>();
                var records = testObject.Execute();

                Assert.Equal("Record ID value", records.FirstOrDefault().RecordIdentifier);
            }
        }

        [Fact]
        public void Execute_PopulatesCommandTextFromIOdbcDownloadQuery()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connection = mock.Mock<DbConnection>();
                connection.Setup(c => c.Open());

                var dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

                var reader = mock.Mock<DbDataReader>();
                reader.SetupSequence(r => r.Read())
                    .Returns(true)
                    .Returns(false);
                reader.Setup(r => r.FieldCount).Returns(1);
                reader.Setup(r => r.IsDBNull(It.IsAny<int>())).Returns(false);
                reader.Setup(r => r[It.IsAny<int>()]).Returns("Record ID value");
                reader.Setup(r => r.GetName(It.IsAny<int>())).Returns("Record ID");

                var command = mock.Mock<IShipWorksOdbcCommand>();
                command.Setup(c => c.ExecuteReader()).Returns(reader.Object);

                var commandBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
                commandBuilder.Setup(b => b.QuoteIdentifier(It.IsAny<string>())).Returns<string>(x => $"\'{x}\'");

                var dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
                dbProviderFactory.Setup(f => f.CreateOdbcCommand(It.IsAny<DbConnection>()))
                    .Returns(command.Object);
                dbProviderFactory.Setup(f => f.CreateShipWorksOdbcCommandBuilder(It.IsAny<ShipWorksOdbcDataAdapter>()))
                    .Returns(commandBuilder.Object);

                var column = new OdbcColumn("Column Name");

                var externalField = mock.Mock<IExternalOdbcMappableField>();
                externalField.Setup(f => f.Column).Returns(column);

                var mapEntry = mock.Mock<IOdbcFieldMapEntry>();
                mapEntry.Setup(e => e.ExternalField).Returns(externalField.Object);

                var map = mock.Mock<IOdbcFieldMap>();
                map.Setup(m => m.RecordIdentifierSource).Returns("Record ID");
                map.Setup(m => m.Entries).Returns(new List<IOdbcFieldMapEntry> { mapEntry.Object });

                var query = mock.Mock<IOdbcDownloadQuery>();

                var testObject = mock.Create<OdbcDownloadCommand>();
                testObject.Execute();

                query.Verify(q => q.ConfigureCommand(command.Object));
            }
        }

        [Fact]
        public void Execute_ThrowsDownloadException_WhenOdbcExceptionThrown()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var connection = mock.Mock<DbConnection>();
                connection.Setup(c => c.Open());

                var dataSource = mock.Mock<IOdbcDataSource>();
                dataSource.Setup(d => d.CreateConnection()).Throws<ShipWorksOdbcException>();

                var testObject = mock.Create<OdbcDownloadCommand>();

                Assert.Throws<ShipWorksOdbcException>(() => testObject.Execute());
            }
        }
    }
}