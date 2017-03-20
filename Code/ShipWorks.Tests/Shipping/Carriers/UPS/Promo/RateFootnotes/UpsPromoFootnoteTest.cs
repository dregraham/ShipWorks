using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo.RateFootnotes
{
    public class UpsPromoFootnoteTest
    {
        public static AutoMock GetLooseThatReturnsMocks() =>
            AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });

        [Fact]
        public void UpsPromoFootnote_ReturnsTrue_ForAssociatedWithAmountFooter()
        {
            using (var mock = GetLooseThatReturnsMocks())
            {

                UpsAccountEntity upsAccount = new UpsAccountEntity
                {
                    PromoStatus = (int)UpsPromoStatus.Applied
                };

                var settings = new ShippingSettingsEntity() {UpsAccessKey = "blah"};

                mock.Mock<ICarrierSettingsRepository>().Setup(s => s.GetShippingSettings()).Returns(settings);
                var upsPromo = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(UpsAccountEntity), upsAccount));

                UpsPromoFootnote footnote = new UpsPromoFootnote(null, upsPromo);

                Assert.Equal(true, footnote.AssociatedWithAmountFooter);
            }
        }
    }
}
