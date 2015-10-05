using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo.Api
{
    public class UpsPromoWebClientFactoryTest
    {
        private readonly UpsPromoWebClientFactory testObject;

        public UpsPromoWebClientFactoryTest()
        {
            testObject = new UpsPromoWebClientFactory();
        }

        [Fact]
        public void CreatePromoClient_ReturnsUpsPromoApiClient_Test()
        {
            UpsPromo promo = new UpsPromo(1, string.Empty, new Mock<ICarrierAccountRepository<UpsAccountEntity>>().Object, new Mock<IPromoClientFactory>().Object);

            IUpsApiPromoClient client = testObject.CreatePromoClient(promo);

            Assert.IsType<UpsApiPromoClient>(client);
        }

        [Fact]
        public void CreatePromoClient_PromoApiClientIsNotNull_Test()
        {
            UpsPromo promo = new UpsPromo(1, string.Empty, new Mock<ICarrierAccountRepository<UpsAccountEntity>>().Object, new Mock<IPromoClientFactory>().Object);

            IUpsApiPromoClient client = testObject.CreatePromoClient(promo);

            Assert.NotNull(client);
        }
    }
}
