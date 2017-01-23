using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using System;
using System.Collections.Concurrent;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.Promo
{
    public class UpsPromoPolicyTest : IDisposable
    {
        private const int defaultAccountId = 42;
        private AutoMock mock;
        private ConcurrentDictionary<long, DateTime> remindLaterAccounts;

        public UpsPromoPolicyTest()
        {
            mock = AutoMock.GetLoose();
            remindLaterAccounts = new ConcurrentDictionary<long, DateTime>();
            mock.Provide(remindLaterAccounts);
        }

        [Fact]
        public void IsEligible_ReturnsTrue_WhenStatusIsNoneAndNotRemindedLater()
        {
            var testObject = mock.Create<UpsPromoPolicy>();
            Assert.True(testObject.IsEligible(GetMockedPromo(UpsPromoStatus.None)));
        }

        [Fact]
        public void IsEligible_ReturnsFalse_WhenStatusIsDeclined()
        {
            var testObject = mock.Create<UpsPromoPolicy>();
            Assert.False(testObject.IsEligible(GetMockedPromo(UpsPromoStatus.Declined)));
        }

        [Fact]
        public void IsEligible_ReturnsFalse_WhenStatusIsApplied()
        {
            var testObject = mock.Create<UpsPromoPolicy>();
            Assert.False(testObject.IsEligible(GetMockedPromo(UpsPromoStatus.Applied)));
        }

        [Fact]
        public void IsEligible_ReturnsFalse_WhenStatusIsNoneAndRemindMeLasterJustCalled()
        {
            var testObject = mock.Create<UpsPromoPolicy>();

            IUpsPromo promo = GetMockedPromo(UpsPromoStatus.None);
            testObject.RemindLater(promo);

            Assert.False(testObject.IsEligible(promo));
        }

        [Fact]
        public void IsEligible_ReturnsTrue_WhenStatusIsNoneAndReminderMoreThanEightHoursAgo()
        {
            IUpsPromo promo = GetMockedPromo(UpsPromoStatus.None);

            Mock<IDateTimeProvider> dateTimeProvider = mock.Mock<IDateTimeProvider>();

            var testObject = mock.Create<UpsPromoPolicy>();

            dateTimeProvider.Setup(x => x.Now)
                .Returns(DateTime.Now);

            testObject.RemindLater(promo);

            dateTimeProvider.Setup(x => x.Now)
                .Returns(DateTime.Now.AddHours(8).AddMinutes(1));

            Assert.True(testObject.IsEligible(promo));
        }

        [Fact]
        public void RemindLater_AddsEntryWithNow()
        {
            IUpsPromo promo = GetMockedPromo(UpsPromoStatus.None);
            DateTime now = new DateTime(2016, 8, 18, 14, 48, 10);

            Mock<IDateTimeProvider> dateTimeProvider = mock.Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(x => x.Now).Returns(now);

            var testObject = mock.Create<UpsPromoPolicy>();

            testObject.RemindLater(promo);

            Assert.Equal(now, remindLaterAccounts[defaultAccountId]);
        }

        [Fact]
        public void RemindLater_UpdatesTime_WhenRemindedAgain()
        {
            IUpsPromo promo = GetMockedPromo(UpsPromoStatus.None);
            DateTime now = new DateTime(2016, 8, 18, 14, 48, 10);
            DateTime later = new DateTime(2016, 8, 18, 15, 48, 10);
            Mock<IDateTimeProvider> dateTimeProvider = mock.Mock<IDateTimeProvider>();

            var testObject = mock.Create<UpsPromoPolicy>();

            dateTimeProvider.Setup(x => x.Now).Returns(now);
            testObject.RemindLater(promo);
            Assert.Equal(now, remindLaterAccounts[defaultAccountId]);

            dateTimeProvider.Setup(x => x.Now).Returns(later);
            testObject.RemindLater(promo);
            Assert.Equal(later, remindLaterAccounts[defaultAccountId]);
        }

        private IUpsPromo GetMockedPromo(UpsPromoStatus promoStatus)
        {
            Mock<IUpsPromo> promo = mock.MockRepository.Create<IUpsPromo>();

            promo.Setup(p => p.AccountId)
                .Returns(defaultAccountId);

            promo.Setup(p => p.GetStatus())
                .Returns(promoStatus);

            return promo.Object;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
