using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Api;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo
{
    public class UpsPromoFactoryTest
    {
        private readonly AutoMock mock;

        public UpsPromoFactoryTest()
        {
            mock = AutoMock.GetLoose();

            ShippingSettingsEntity shippingSettings = new ShippingSettingsEntity();
            shippingSettings.UpsAccessKey = SecureText.Encrypt("abcd123", "UPS");
            Mock<ICarrierSettingsRepository> carrierSettingsRepo = mock.Mock<ICarrierSettingsRepository>();
            carrierSettingsRepo.Setup(c => c.GetShippingSettings()).Returns(shippingSettings);

            Mock<IIndex<ShipmentTypeCode, ICarrierSettingsRepository>> repo = mock.MockRepository.Create<IIndex<ShipmentTypeCode, ICarrierSettingsRepository>>();
            repo.Setup(x => x[ShipmentTypeCode.UpsOnLineTools])
                .Returns(carrierSettingsRepo.Object);
            mock.Provide(repo.Object);
        }
    }
}
