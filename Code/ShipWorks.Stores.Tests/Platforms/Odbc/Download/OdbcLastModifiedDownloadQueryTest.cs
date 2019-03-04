using System;
using System.Data.Common;
using System.Data.Odbc;
using Autofac.Extras.Moq;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
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

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                onlineLastModifiedStartingPoint,
                "",
                "");
            
            ShipWorksOdbcException ex = Assert.Throws<ShipWorksOdbcException>(() => testObject.GenerateSql());

            Assert.Contains("The OnlineLastModified column must be mapped to download by OnlineLastModified.", ex.Message);
        }

        [Fact]
        public void GenerateSql_GeneratesSqlThatContainsDownloadQuery()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

           OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                onlineLastModifiedStartingPoint,
                "OnlineLastModified",
                "'OnlineLastModified'");
            
            Assert.Contains(originalDownloadQuery, testObject.GenerateSql());
        }

        [Fact]
        public void GenerateSql_GeneratesSqlThatContainsOrderBy()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                onlineLastModifiedStartingPoint,
                "OnlineLastModified",
                "'OnlineLastModified'");
            
            Assert.EndsWith("ORDER BY 'OnlineLastModified' ASC", testObject.GenerateSql());
        }

        [Fact]
        public void GenerateSql_GeneratesSqlThatContainsColumnName()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                onlineLastModifiedStartingPoint,
                "OnlineLastModified",
                "'OnlineLastModified'");
            
            Assert.Contains("OnlineLastModified", testObject.GenerateSql());
        }

        [Fact]
        public void PopulateCommandText_CallsSetCommandTextOnCommand()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                onlineLastModifiedStartingPoint,
                "OnlineLastModified",
                "'OnlineLastModified'");
            
            testObject.ConfigureCommand(command.Object);

            command.Verify(c => c.ChangeCommandText(testObject.GenerateSql()));
        }

        [Fact]
        public void PopulateCommandText_ThrowsShipWorksOdbcException_WhenOdbcColumnIsBlank()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                onlineLastModifiedStartingPoint,
                string.Empty,
                string.Empty);
            
            Assert.Throws<ShipWorksOdbcException>(() => testObject.ConfigureCommand(command.Object));
        }

        [Fact]
        public void PopulateCommandText_AddsOnlineLastModifiedParameter()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            DateTime onlineLastModifiedStartingPoint = DateTime.UtcNow;

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);
           
            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();

            OdbcLastModifiedDownloadQuery testObject = new OdbcLastModifiedDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                onlineLastModifiedStartingPoint,
                "OnlineLastModified",
                "'OnlineLastModified'");

            testObject.ConfigureCommand(command.Object);

            command.Verify(c => c.AddParameter(It.IsAny<OdbcParameter>()));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
