using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;
using System.Data.Common;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc
{
    public class OdbcLastModifiedDownloadQueryTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcLastModifiedDownloadQueryTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void GenerateSql_GeneratesSqlThatContainsDownloadQuery()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;
            OdbcColumn column = new OdbcColumn("OnlineLastModified");

            Mock<IOdbcDownloadQuery> downloadQuery = mock.Mock<IOdbcDownloadQuery>();
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
        public void GenerateSql_GeneratesSqlThatContainsDateTime()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;
            OdbcColumn column = new OdbcColumn("OnlineLastModified");

            Mock<IOdbcDownloadQuery> downloadQuery = mock.Mock<IOdbcDownloadQuery>();
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

            Assert.Contains(onlineLastModifiedStartingPoint.ToString("u"), testObject.GenerateSql());
        }

        [Fact]
        public void GenerateSql_GeneratesSqlThatContainsColumnName()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;
            OdbcColumn column = new OdbcColumn("OnlineLastModified");

            Mock<IOdbcDownloadQuery> downloadQuery = mock.Mock<IOdbcDownloadQuery>();
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

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
