using System.Linq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Tests.Shared.Carriers.FedEx;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.GlobalShipAddress.IntegrationTest
{
    public class GlobalShipAddressIntegrationTest
    {
        public void SearchLocation_ActuallyCallsFedEx_IntegrationTest()
        {
            // MARKED WITH THE IGNORE ATTRIBUTE SINCE THIS IS AN INTEGRATION TEST - WE DON'T WANT THIS TEST
            // TO BE EXECUTED IN THE NORMAL BUILD PROCESS
            FedExAccountEntity account = new FedExAccountEntity { AccountNumber = "602344126", MeterNumber = "118575561" };

            ShipmentEntity shipment = BuildFedExShipmentEntity.SetupRequestShipmentEntity();
            shipment.FedEx.Service = (int) FedExServiceType.FedExGround;

            Mock<IFedExSettingsRepository> MockSettingsRepository = new Mock<IFedExSettingsRepository>();
            MockSettingsRepository.Setup(x => x.GetShippingSettings())
                                  .Returns(new ShippingSettingsEntity()
                                  {
                                      FedExUsername = "Gaz3GPxQNv8E2ljM",
                                      FedExPassword = "h/Rat4y3/B6Pp2HlIiCPOHPUotyx+ihaH2wiHI42kds=",
                                  });
            MockSettingsRepository.Setup(x => x.UseTestServer)
                                  .Returns(true);

            MockSettingsRepository.Setup(x => x.GetAccount(It.IsAny<ShipmentEntity>()))
                                  .Returns(account);

            //TODO: See if we can use Autofac for this
            FedExRequestFactory fedExRequestFactory = new FedExRequestFactory(
                new FedExServiceGatewayFactory(
                    _ => new FedExServiceGateway(MockSettingsRepository.Object),
                    _ => new FedExOpenShipGateway(MockSettingsRepository.Object)),
                MockSettingsRepository.Object,
                new FedExShipmentTokenProcessor(),
                new FedExResponseFactory(),
                null);
            var searchLocationsRequest = fedExRequestFactory.CreateSearchLocationsRequest();
            var carrierResponse = searchLocationsRequest.Submit(shipment);

            Assert.NotNull(carrierResponse);
            var result = carrierResponse.Value.Process();

            Assert.Equal(1, result.Value.Count());
        }
    }
}
