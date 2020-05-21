using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS
{
    public class UpsOltLabelServiceTest
    {
        private readonly AutoMock mock;
        private UpsOltLabelService testObject;
        private readonly ShipmentEntity shipment;
        private readonly Mock<IUpsShipmentValidatorFactory> validatorFactory;
        private readonly Mock<IUpsLabelClientFactory> clientFactory;

        public UpsOltLabelServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            MockUpsShippingSettings();

            shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.UpsOnLineTools,
                Ups = new UpsShipmentEntity()
            };

            Mock<IUpsShipmentValidator> validator = mock.Mock<IUpsShipmentValidator>();
            validator.Setup(v => v.ValidateShipment(shipment)).Returns(Result.FromSuccess());

            validatorFactory = mock.Mock<IUpsShipmentValidatorFactory>();
            validatorFactory.Setup(v => v.Create(It.IsAny<ShipmentEntity>())).Returns(validator);
            clientFactory = mock.Mock<IUpsLabelClientFactory>();

            testObject = mock.Create<UpsOltLabelService>();
        }

        [Fact]
        public void Create_DelegatesToValidatorFactoryForValidator()
        {
            testObject.Create(shipment);

            validatorFactory.Verify(v => v.Create(shipment));
        }

        [Fact]
        public void Create_DelegatesToLabelClientFactoryForLabelClient()
        {
            testObject.Create(shipment);

            clientFactory.Verify(f => f.GetClient(shipment));
        }

        private void MockUpsShippingSettings()
        {
            var settingsRepositoryMock = mock.Mock<ICarrierSettingsRepository>();

            var bar =
                mock.MockRepository.Create<IIndex<ShipmentTypeCode, ICarrierSettingsRepository>>();

            settingsRepositoryMock.Setup(x => x.GetShippingSettings()).Returns(
                new ShippingSettingsEntity
                {
                    UpsAllowNoDims = false
                });

            bar.Setup(i => i[ShipmentTypeCode.UpsOnLineTools])
                .Returns(settingsRepositoryMock.Object);

            mock.Provide(bar.Object);
        }
    }
}
