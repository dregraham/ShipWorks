using System;
using System.Data.Common;
using System.Data.Odbc;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Download
{
    public class OdbcLastModifiedDownloadQueryTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcLastModifiedDownloadQueryTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void GenerateSql_ThrowsOdbcException_WhenOnlineLastModifiedIsNotMapped()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);
            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(downloadQuery.Object, onlineLastModifiedStartingPoint, fieldMap.Object, dbProviderFactory.Object, dataSource.Object);

            ShipWorksOdbcException ex = Assert.Throws<ShipWorksOdbcException>(() => testObject.GenerateSql());

            Assert.Contains("The OnlineLastModified column must be mapped to download by OnlineLastModified.", ex.Message);
        }

        [Fact]
        public void GenerateSql_ThrowsOdbcException_WhenOnlineLastModifiedIsEmptyString()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;
            OdbcColumn column = new OdbcColumn("");

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IExternalOdbcMappableField> externalField = mock.Mock<IExternalOdbcMappableField>();
            externalField.SetupGet(e => e.Column).Returns(column);

            Mock<IOdbcFieldMapEntry> entry = mock.Mock<IOdbcFieldMapEntry>();
            entry.SetupGet(e => e.ExternalField).Returns(externalField.Object);

            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry.Object });

            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>())).Returns(cmdBuilder.Object);

            Mock<DbConnection> connection = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(downloadQuery.Object, onlineLastModifiedStartingPoint, fieldMap.Object, dbProviderFactory.Object, dataSource.Object);

            ShipWorksOdbcException ex = Assert.Throws<ShipWorksOdbcException>(() => testObject.GenerateSql());

            Assert.Contains("The OnlineLastModified column must be mapped to download by OnlineLastModified.", ex.Message);
        }

        [Fact]
        public void GenerateSql_GeneratesSqlThatContainsDownloadQuery()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;
            OdbcColumn column = new OdbcColumn("OnlineLastModified");

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IExternalOdbcMappableField> externalField = mock.Mock<IExternalOdbcMappableField>();
            externalField.SetupGet(e => e.Column).Returns(column);

            Mock<IOdbcFieldMapEntry> entry = mock.Mock<IOdbcFieldMapEntry>();
            entry.SetupGet(e => e.ExternalField).Returns(externalField.Object);

            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry.Object });

            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>())).Returns(cmdBuilder.Object);

            Mock<DbConnection> connection = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(downloadQuery.Object, onlineLastModifiedStartingPoint, fieldMap.Object, dbProviderFactory.Object, dataSource.Object);

            Assert.Contains(originalDownloadQuery, testObject.GenerateSql());
        }

        [Fact]
        public void GenerateSql_GeneratesSqlThatContainsOrderBy()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;
            OdbcColumn column = new OdbcColumn("OnlineLastModified");

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IExternalOdbcMappableField> externalField = mock.Mock<IExternalOdbcMappableField>();
            externalField.SetupGet(e => e.Column).Returns(column);

            Mock<IOdbcFieldMapEntry> entry = mock.Mock<IOdbcFieldMapEntry>();
            entry.SetupGet(e => e.ExternalField).Returns(externalField.Object);

            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry.Object });

            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
            cmdBuilder.Setup(c => c.QuoteIdentifier(It.IsAny<string>())).Returns(column.Name);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>())).Returns(cmdBuilder.Object);

            Mock<DbConnection> connection = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(downloadQuery.Object, onlineLastModifiedStartingPoint, fieldMap.Object, dbProviderFactory.Object, dataSource.Object);

            Assert.EndsWith("ORDER BY OnlineLastModified ASC", testObject.GenerateSql());
        }

        [Fact]
        public void GenerateSql_GeneratesSqlThatContainsColumnName()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;
            OdbcColumn column = new OdbcColumn("OnlineLastModified");

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IExternalOdbcMappableField> externalField = mock.Mock<IExternalOdbcMappableField>();
            externalField.SetupGet(e => e.Column).Returns(column);

            Mock<IOdbcFieldMapEntry> entry = mock.Mock<IOdbcFieldMapEntry>();
            entry.SetupGet(e => e.ExternalField).Returns(externalField.Object);

            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry.Object });

            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
            cmdBuilder.Setup(c => c.QuoteIdentifier(It.IsAny<string>())).Returns(column.Name);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>())).Returns(cmdBuilder.Object);

            Mock<DbConnection> connection = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(downloadQuery.Object, onlineLastModifiedStartingPoint, fieldMap.Object, dbProviderFactory.Object, dataSource.Object);

            Assert.Contains("OnlineLastModified", testObject.GenerateSql());
        }

        [Fact]
        private void PopulateCommandText_CallsSetCommandTextOnCommand()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;
            OdbcColumn column = new OdbcColumn("OnlineLastModified");

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IExternalOdbcMappableField> externalField = mock.Mock<IExternalOdbcMappableField>();
            externalField.SetupGet(e => e.Column).Returns(column);

            Mock<IOdbcFieldMapEntry> entry = mock.Mock<IOdbcFieldMapEntry>();
            entry.SetupGet(e => e.ExternalField).Returns(externalField.Object);

            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry.Object });

            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
            cmdBuilder.Setup(c => c.QuoteIdentifier(It.IsAny<string>())).Returns(column.Name);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>())).Returns(cmdBuilder.Object);

            Mock<DbConnection> connection = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(downloadQuery.Object, onlineLastModifiedStartingPoint, fieldMap.Object, dbProviderFactory.Object, dataSource.Object);

            testObject.ConfigureCommand(command.Object);

            command.Verify(c=> c.ChangeCommandText(testObject.GenerateSql()));
        }

        [Fact]
        private void PopulateCommandText_ThrowsShipWorksOdbcException_WhenOdbcColumnIsBlank()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;
            OdbcColumn column = new OdbcColumn("");

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IExternalOdbcMappableField> externalField = mock.Mock<IExternalOdbcMappableField>();
            externalField.SetupGet(e => e.Column).Returns(column);

            Mock<IOdbcFieldMapEntry> entry = mock.Mock<IOdbcFieldMapEntry>();
            entry.SetupGet(e => e.ExternalField).Returns(externalField.Object);

            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry.Object });

            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
            cmdBuilder.Setup(c => c.QuoteIdentifier(It.IsAny<string>())).Returns(column.Name);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>())).Returns(cmdBuilder.Object);

            Mock<DbConnection> connection = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(downloadQuery.Object, onlineLastModifiedStartingPoint, fieldMap.Object, dbProviderFactory.Object, dataSource.Object);

            Assert.Throws<ShipWorksOdbcException>(() => testObject.ConfigureCommand(command.Object));
        }

        [Fact]
        private void PopulateCommandText_OpensConnection()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;
            OdbcColumn column = new OdbcColumn("OnlineLastModified");

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IExternalOdbcMappableField> externalField = mock.Mock<IExternalOdbcMappableField>();
            externalField.SetupGet(e => e.Column).Returns(column);

            Mock<IOdbcFieldMapEntry> entry = mock.Mock<IOdbcFieldMapEntry>();
            entry.SetupGet(e => e.ExternalField).Returns(externalField.Object);

            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry.Object });

            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
            cmdBuilder.Setup(c => c.QuoteIdentifier(It.IsAny<string>())).Returns(column.Name);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>())).Returns(cmdBuilder.Object);

            Mock<DbConnection> connection = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(downloadQuery.Object, onlineLastModifiedStartingPoint, fieldMap.Object, dbProviderFactory.Object, dataSource.Object);

            testObject.ConfigureCommand(command.Object);

            connection.Verify(c => c.Open());
        }

        [Fact]
        private void PopulateCommandText_AddsOnlineLastModifiedParameter()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;
            OdbcColumn column = new OdbcColumn("OnlineLastModified");

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IExternalOdbcMappableField> externalField = mock.Mock<IExternalOdbcMappableField>();
            externalField.SetupGet(e => e.Column).Returns(column);

            Mock<IOdbcFieldMapEntry> entry = mock.Mock<IOdbcFieldMapEntry>();
            entry.SetupGet(e => e.ExternalField).Returns(externalField.Object);

            Mock<IOdbcFieldMap> fieldMap = mock.Mock<IOdbcFieldMap>();
            fieldMap.Setup(f => f.FindEntriesBy(It.IsAny<EntityField2>())).Returns(new[] { entry.Object });

            Mock<IShipWorksOdbcCommandBuilder> cmdBuilder = mock.Mock<IShipWorksOdbcCommandBuilder>();
            cmdBuilder.Setup(c => c.QuoteIdentifier(It.IsAny<string>())).Returns(column.Name);

            Mock<IShipWorksDbProviderFactory> dbProviderFactory = mock.Mock<IShipWorksDbProviderFactory>();
            dbProviderFactory.Setup(d => d.CreateShipWorksOdbcCommandBuilder(It.IsAny<IShipWorksOdbcDataAdapter>())).Returns(cmdBuilder.Object);

            Mock<DbConnection> connection = mock.Mock<DbConnection>();

            Mock<IOdbcDataSource> dataSource = mock.Mock<IOdbcDataSource>();
            dataSource.Setup(d => d.CreateConnection()).Returns(connection.Object);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(downloadQuery.Object, onlineLastModifiedStartingPoint, fieldMap.Object, dbProviderFactory.Object, dataSource.Object);

            testObject.ConfigureCommand(command.Object);

            command.Verify(c=> c.AddParameter(It.IsAny<OdbcParameter>()));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
