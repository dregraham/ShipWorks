using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.Upload;
using ShipWorks.Templates.Tokens;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Upload
{
    public class OdbcCustomUploadQueryTest
    {
        [Fact]
        public void GenerateSql_DelegatesToProcessTokens()
        {
            const string uploadColumnSource = "Upload Column Source";
            const long shipmentId = 42;

            using (var mock = AutoMock.GetLoose())
            {
                OdbcStoreEntity store = new OdbcStoreEntity()
                {
                    UploadColumnSource = uploadColumnSource
                };

                ShipmentEntity shipment = new ShipmentEntity(shipmentId);

                var tokenProcessor = mock.MockRepository.Create<ITemplateTokenProcessor>();

                var testObject = new OdbcCustomUploadQuery(store, shipment, tokenProcessor.Object);

                testObject.GenerateSql();

                tokenProcessor.Verify(p=>p.ProcessTokens(uploadColumnSource, shipmentId, false), Times.Once);
            }
        }

        [Fact]
        public void GenerateSql_ThrowsShipworksOdbcException_WhenTemplateProcessorThrowsProcessTokenException()
        {
            const string uploadColumnSource = "Upload Column Source";
            const long shipmentId = 42;

            using (var mock = AutoMock.GetLoose())
            {
                OdbcStoreEntity store = new OdbcStoreEntity()
                {
                    UploadColumnSource = uploadColumnSource
                };

                ShipmentEntity shipment = new ShipmentEntity(shipmentId);

                var tokenProcessor = mock.MockRepository.Create<ITemplateTokenProcessor>();
                tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<long>(), false)).Throws<TemplateTokenException>();

                var testObject = new OdbcCustomUploadQuery(store, shipment, tokenProcessor.Object);

                Assert.Throws<ShipWorksOdbcException>(() => testObject.GenerateSql());
            }
        }

        [Fact]
        public void GenerateSql_ReturnsResultsFromProcessTokens()
        {
            const string processTokensResult = "process tokens result";

            using (var mock = AutoMock.GetLoose())
            {
                var tokenProcessor = mock.MockRepository.Create<ITemplateTokenProcessor>();
                tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<bool>()))
                    .Returns(processTokensResult);

                var testObject = new OdbcCustomUploadQuery(new OdbcStoreEntity(), new ShipmentEntity(), tokenProcessor.Object);

                string generateSqlResult = testObject.GenerateSql();

                Assert.Equal(processTokensResult, generateSqlResult);
            }
        }


        [Fact]
        public void ConfigureCommand_SendsGeneratedSqlToChangeCommandText()
        {
            const string processTokensResult = "process tokens result";

            using (var mock = AutoMock.GetLoose())
            {
                var tokenProcessor = mock.MockRepository.Create<ITemplateTokenProcessor>();
                tokenProcessor.Setup(p => p.ProcessTokens(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<bool>()))
                    .Returns(processTokensResult);

                var testObject = new OdbcCustomUploadQuery(new OdbcStoreEntity(), new ShipmentEntity(), tokenProcessor.Object);

                Mock<IShipWorksOdbcCommand> command = mock.MockRepository.Create<IShipWorksOdbcCommand>();

                testObject.ConfigureCommand(command.Object);

                command.Verify(c=>c.ChangeCommandText(processTokensResult), Times.Once);
            }
        }
    }
}
