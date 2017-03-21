using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Security;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.Shipping.Carriers.UPS.Promo.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

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

        [Fact]
        public void GetFootnoteFactory_ReturnsNull_WhenPromoPolicyIsNotEligible()
        {
            Mock<IUpsPromoPolicy> promoPolicy = mock.Mock<IUpsPromoPolicy>();
            promoPolicy.Setup(p => p.IsEligible(It.IsAny<UpsPromo>())).Returns(false);

            UpsPromoFactory testObject = mock.Create<UpsPromoFactory>();

            Assert.Null(testObject.GetFootnoteFactory(new UpsAccountEntity()));
        }

        [Fact]
        public void GetFootnoteFactory_ReturnsUpsPromoFootnoteFactory_WhenPromoPolicyIsEligible()
        {
            Mock<IUpsPromoPolicy> promoPolicy = mock.Mock<IUpsPromoPolicy>();
            promoPolicy.Setup(p => p.IsEligible(It.IsAny<UpsPromo>())).Returns(true);

            UpsPromoFactory testObject = mock.Create<UpsPromoFactory>();

            Assert.IsAssignableFrom<UpsPromoFootnoteFactory>(testObject.GetFootnoteFactory(new UpsAccountEntity()));
        }

    }
}
