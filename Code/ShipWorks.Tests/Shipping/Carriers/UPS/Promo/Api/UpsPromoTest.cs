using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo.Api
{
    public class UpsPromoTest
    {
        public static AutoMock GetLooseThatReturnsMocks() =>
            AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) {DefaultValue = DefaultValue.Mock});

        [Fact]
        public void GetStatus_Returns_UpsAccountPromoStatus()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {
                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int)UpsPromoStatus.Applied
                };

                mock.Mock<ICarrierSettingsRepository>().Setup(s => s.GetShippingSettings().UpsAccessKey).Returns("blahh");
                var upsPromo = mock.Create<UpsPromo>(new TypedParameter(typeof (UpsAccountEntity), upsAccount));
                
                Assert.Equal(UpsPromoStatus.Applied, upsPromo.GetStatus());
            }
        }
    }
}
