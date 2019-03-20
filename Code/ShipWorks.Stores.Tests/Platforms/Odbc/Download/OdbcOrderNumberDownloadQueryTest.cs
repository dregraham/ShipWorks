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
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Download
{
    public class OdbcOrderNumberDownloadQueryTest : IDisposable
    {
        private readonly AutoMock mock;

        public OdbcOrderNumberDownloadQueryTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void GenerateSql_ThrowsOdbcException_WhenOrderNumberIsNotMapped()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            string orderNumber = "OrderNumber";

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            OdbcOrderNumberDownloadQuery testObject =new OdbcOrderNumberDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                orderNumber,
                string.Empty,
                string.Empty);

            ShipWorksOdbcException ex = Assert.Throws<ShipWorksOdbcException>(() => testObject.GenerateSql());

            Assert.Contains("The OrderNumber column must be mapped to download orders On Demand.", ex.Message);
        }

        [Fact]
        public void GenerateSql_GeneratesSqlThatContainsDownloadQuery()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            string orderNumber = "OrderNumber";

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            OdbcOrderNumberDownloadQuery testObject =new OdbcOrderNumberDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                orderNumber,
                "OrderNumber",
                "'OrderNumber'");

            Assert.Contains(originalDownloadQuery, testObject.GenerateSql());
        }

        [Fact]
        public void GenerateSql_GeneratesSqlThatContainsColumnName()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            string orderNumber = "OrderNumber";

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            OdbcOrderNumberDownloadQuery testObject =new OdbcOrderNumberDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                orderNumber,
                "OrderNumber",
                "'OrderNumber'");

            Assert.Contains("OrderNumber", testObject.GenerateSql());
        }

        [Fact]
        public void PopulateCommandText_CallsSetCommandTextOnCommand()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            string orderNumber = "OrderNumber";

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();

            OdbcOrderNumberDownloadQuery testObject =new OdbcOrderNumberDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                orderNumber,
                "OrderNumber",
                "'OrderNumber'");

            testObject.ConfigureCommand(command.Object);

            command.Verify(c=> c.ChangeCommandText(testObject.GenerateSql()));
        }

        [Fact]
        public void PopulateCommandText_ThrowsShipWorksOdbcException_WhenOdbcColumnIsBlank()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            string orderNumber = "OrderNumber";

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();

            OdbcOrderNumberDownloadQuery testObject =new OdbcOrderNumberDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                orderNumber,
                "",
                "");

            Assert.Throws<ShipWorksOdbcException>(() => testObject.ConfigureCommand(command.Object));
        }

        [Fact]
        public void PopulateCommandText_AddsOrderNumberParameter()
        {
            string originalDownloadQuery = "SELECT * FROM FOO";
            string orderNumber = "OrderNumber";

            Mock<IOdbcQuery> downloadQuery = mock.Mock<IOdbcQuery>();
            downloadQuery.Setup(d => d.GenerateSql()).Returns(originalDownloadQuery);

            Mock<IShipWorksOdbcCommand> command = mock.Mock<IShipWorksOdbcCommand>();

            OdbcOrderNumberDownloadQuery testObject =new OdbcOrderNumberDownloadQuery(
                OdbcColumnSourceType.CustomQuery,
                downloadQuery.Object,
                orderNumber,
                "OrderNumber",
                "'OrderNumber'");
            
            testObject.ConfigureCommand(command.Object);

            command.Verify(c=> c.AddParameter(It.IsAny<OdbcParameter>()));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
