using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.Promo
{
    public class UpsPromoPolicyTest
    {
        private UpsPromoPolicy testObject;

        public UpsPromoPolicyTest()
        {
            testObject = new UpsPromoPolicy(new DateTimeProvider());
        }

        [Fact]
        public void IsEligible_ReturnsTrue_WhenStatusIsNoneAndNotRemindedLater()
        {
            Assert.True(testObject.IsEligible(GetMockedPromo(UpsPromoStatus.None)));
        }

        [Fact]
        public void IsEligible_ReturnsFalse_WhenStatusIsDeclined()
        {
            Assert.False(testObject.IsEligible(GetMockedPromo(UpsPromoStatus.Declined)));
        }

        [Fact]
        public void IsEligible_ReturnsFalse_WhenStatusIsApplied()
        {
            Assert.False(testObject.IsEligible(GetMockedPromo(UpsPromoStatus.Applied)));
        }

        [Fact]
        public void IsEligible_ReturnsFalse_WhenStatusIsNoneAndRemindMeLasterJustCalled()
        {
            IUpsPromo promo = GetMockedPromo(UpsPromoStatus.None);
            testObject.RemindLater(promo);

            Assert.False(testObject.IsEligible(promo));
        }

        [Fact]
        public void IsEligible_ReturnsTrue_WhenStatusIsNoneAndReminderMoreThanEightHoursAgo()
        {
            IUpsPromo promo = GetMockedPromo(UpsPromoStatus.None);

            Mock<IDateTimeProvider> dateTimeProvider = new Mock<IDateTimeProvider>();

            testObject = new UpsPromoPolicy(dateTimeProvider.Object);

            dateTimeProvider.Setup(x => x.Now)
                .Returns(DateTime.Now);

            testObject.RemindLater(promo);

            dateTimeProvider.Setup(x => x.Now)
                .Returns(DateTime.Now.AddHours(8).AddMinutes(1));

            Assert.True(testObject.IsEligible(promo));
        }

        private static IUpsPromo GetMockedPromo(UpsPromoStatus promoStatus)
        {
            Mock<IUpsPromo> promo = new Mock<IUpsPromo>();

            promo.Setup(p => p.AccountId)
                .Returns(42);

            promo.Setup(p => p.GetStatus())
                .Returns(promoStatus);

            return promo.Object;
        }

    }
}
