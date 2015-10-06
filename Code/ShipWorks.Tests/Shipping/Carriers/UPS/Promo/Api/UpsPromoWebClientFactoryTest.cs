using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo.Api
{
    public class UpsPromoWebClientFactoryTest
    {
        private readonly UpsPromoWebClientFactory testObject;
        private Mock<ICarrierAccountRepository<UpsAccountEntity>> upsAccountRepository;
        private Mock<ICarrierSettingsRepository> upsSettingsRepository;

        public UpsPromoWebClientFactoryTest()
        {
            testObject = new UpsPromoWebClientFactory();
            upsAccountRepository = new Mock<ICarrierAccountRepository<UpsAccountEntity>>();
            upsAccountRepository
                .Setup(r => r.GetAccount(It.IsAny<long>()))
                .Returns(new UpsAccountEntity()
                {
                    AccountNumber = "22"
                });

            upsSettingsRepository = new Mock<ICarrierSettingsRepository>();
            upsSettingsRepository
                .Setup(r => r.GetShippingSettings())
                .Returns(new ShippingSettingsEntity()
                {
                    UpsAccessKey = "/zJ4i4UGkI+TqGUaylfws+lDqbv4EV2K" // Decrypted: 3CECFFF7FF6F3365
                });
        }

        [Fact]
        public void CreatePromoClient_ReturnsUpsPromoApiClient_Test()
        {
            UpsPromo promo = new UpsPromo(1, upsSettingsRepository.Object, upsAccountRepository.Object, new Mock<IPromoClientFactory>().Object);

            IUpsApiPromoClient client = testObject.CreatePromoClient(promo);

            Assert.IsType<UpsApiPromoClient>(client);
        }

        [Fact]
        public void CreatePromoClient_PromoApiClientIsNotNull_Test()
        {
            UpsPromo promo = new UpsPromo(1, upsSettingsRepository.Object, upsAccountRepository.Object, new Mock<IPromoClientFactory>().Object);

            IUpsApiPromoClient client = testObject.CreatePromoClient(promo);

            Assert.NotNull(client);
        }
    }
}
