using System.Linq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.GlobalShipAddress.Response;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Shipping;
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

            Mock<ICarrierSettingsRepository> MockSettingsRepository = new Mock<ICarrierSettingsRepository>();
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

            FedExRequestFactory fedExRequestFactory = new FedExRequestFactory(new FedExServiceGateway(MockSettingsRepository.Object),
                new FedExOpenShipGateway(MockSettingsRepository.Object),
                MockSettingsRepository.Object,
                new FedExShipmentTokenProcessor(),
                new FedExResponseFactory(new FedExLabelRepository()));
            CarrierRequest searchLocationsRequest = fedExRequestFactory.CreateSearchLocationsRequest(shipment, account);

            ICarrierResponse carrierResponse = searchLocationsRequest.Submit();

            FedExGlobalShipAddressResponse fedExGlobalShipAddressResponse = carrierResponse as FedExGlobalShipAddressResponse;

            Assert.NotNull(carrierResponse);
            carrierResponse.Process();

            Assert.NotNull(fedExGlobalShipAddressResponse);

            Assert.Equal(1, fedExGlobalShipAddressResponse.DistanceAndLocationDetails.Count());
        }
    }
}
