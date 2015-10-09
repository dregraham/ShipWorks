using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Shipping.Carriers.UPS.Promo;
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
            Assert.True(testObject.IsEligible(GetAccount(UpsPromoStatus.None)));
        }

        [Fact]
        public void IsEligible_ReturnsFalse_WhenStatusIsDeclined()
        {
            Assert.False(testObject.IsEligible(GetAccount(UpsPromoStatus.Declined)));
        }

        [Fact]
        public void IsEligible_ReturnsFalse_WhenStatusIsApplied()
        {
            Assert.False(testObject.IsEligible(GetAccount(UpsPromoStatus.Applied)));
        }

        [Fact]
        public void IsEligible_ReturnsFalse_WhenStatusIsNoneAndRemindMeLasterJustCalled()
        {
            UpsAccountEntity account = GetAccount(UpsPromoStatus.None);
            testObject.RemindLater(account);

            Assert.False(testObject.IsEligible(account));
        }

        [Fact]
        public void IsEligible_ReturnsTrue_WhenStatusIsNoneAndReminderMoreThanEightHoursAgo()
        {
            UpsAccountEntity account = GetAccount(UpsPromoStatus.None);

            Mock<IDateTimeProvider> dateTimeProvider = new Mock<IDateTimeProvider>();

            testObject = new UpsPromoPolicy(dateTimeProvider.Object);

            dateTimeProvider.Setup(x => x.Now)
                .Returns(DateTime.Now);

            testObject.RemindLater(account);

            dateTimeProvider.Setup(x => x.Now)
                .Returns(DateTime.Now.AddHours(8).AddMinutes(1));

            Assert.True(testObject.IsEligible(account));
        }

        private static UpsAccountEntity GetAccount(UpsPromoStatus promoStatus)
        {
            return new UpsAccountEntity(42)
            {
                PromoStatus = (byte) promoStatus
            };
        }

    }
}
