#region

using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Data.Common;
using Xunit;

#endregion

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Download
{
    public class TableOdbcDownloadQueryTest : IDisposable
    {
        private readonly AutoMock mock;

        public TableOdbcDownloadQueryTest()
        {
            mock = AutoMock.GetLoose();
        }

        public void Dispose()
        {
            mock.Dispose();
        }

        [Fact]
        public void GenerateSql_GeneratesCorrectSql()
        {
            var fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(m => m.RecordIdentifierSource).Returns("FieldId");
            fieldMap.Setup(m => m.Entries)
                .Returns(() => new[] {GetFieldMapEntry("field1"), GetFieldMapEntry("field2")});

            var connection = mock.Mock<DbConnection>();
            mock.Mock<IOdbcDataSource>().Setup(s => s.CreateConnection()).Returns(connection.Object);

            var cmdBuilder = mock.MockRepository.Create<IShipWorksOdbcCommandBuilder>();
            cmdBuilder.Setup(b => b.QuoteIdentifier(It.IsAny<string>())).Returns<string>(s => $"[{s}]");

            var dbProvider = mock.Mock<IShipWorksDbProviderFactory>();
            dbProvider.Setup(p => p.CreateShipWorksOdbcCommandBuilder(null))
                .Returns(cmdBuilder.Object);


            var testObject =
                mock.Create<OdbcTableDownloadQuery>(new TypedParameter(typeof(OdbcStoreEntity),
                    new OdbcStoreEntity()
                    {
                        ImportColumnSourceType = (int) OdbcColumnSourceType.Table,
                        ImportColumnSource = "MyTable"
                    }));
            string sql = testObject.GenerateSql();

            Assert.Equal("SELECT [field1], [field2], [FieldId] FROM [MyTable]", sql);
        }

        [Fact]
        public void ConfigureCommand_CallsChangeCommandTextOnCommand()
        {
            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();
            var fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(m => m.RecordIdentifierSource).Returns("FieldId");
            fieldMap.Setup(m => m.Entries)
                .Returns(() => new[] { GetFieldMapEntry("field1"), GetFieldMapEntry("field2") });

            var connection = mock.Mock<DbConnection>();
            mock.Mock<IOdbcDataSource>().Setup(s => s.CreateConnection()).Returns(connection.Object);

            var cmdBuilder = mock.MockRepository.Create<IShipWorksOdbcCommandBuilder>();
            cmdBuilder.Setup(b => b.QuoteIdentifier(It.IsAny<string>())).Returns<string>(s => $"[{s}]");

            var dbProvider = mock.Mock<IShipWorksDbProviderFactory>();
            dbProvider.Setup(p => p.CreateShipWorksOdbcCommandBuilder(null))
                .Returns(cmdBuilder.Object);


            var testObject =
                mock.Create<OdbcTableDownloadQuery>(new TypedParameter(typeof(OdbcStoreEntity),
                    new OdbcStoreEntity()
                    {
                        ImportColumnSourceType = (int)OdbcColumnSourceType.Table,
                        ImportColumnSource = "MyTable"
                    }));

            testObject.ConfigureCommand(command.Object);
        }

        private IOdbcFieldMapEntry GetFieldMapEntry(string externalColumnName)
        {
            var externalField = mock.MockRepository.Create<IExternalOdbcMappableField>();
            externalField.Setup(e => e.Column).Returns(new OdbcColumn(externalColumnName));

            var entry = mock.MockRepository.Create<IOdbcFieldMapEntry>();
            entry.Setup(e => e.ExternalField).Returns(externalField.Object);

            return entry.Object;
        }
    }
}