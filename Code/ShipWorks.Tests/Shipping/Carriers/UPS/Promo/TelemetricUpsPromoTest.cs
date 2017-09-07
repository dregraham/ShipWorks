using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo
{
    public class TelemetricUpsPromoTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ITrackedEvent> telemetry;
        private readonly Mock<IUpsPromo> upsPromo;

        public TelemetricUpsPromoTest()
        {
            mock = AutoMock.GetLoose();

            telemetry = mock.Mock<ITrackedEvent>();
            upsPromo = mock.Mock<IUpsPromo>();
            upsPromo.SetupGet(p => p.AccountNumber).Returns("upsaccountnumber");
        }

        [Fact]
        public void Apply_DelegatesToUpsPromo()
        {
            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(UpsPromoSource), UpsPromoSource.SetupWizard),
                new TypedParameter(typeof(UpsPromoAccountType), UpsPromoAccountType.NewAccount));
            testObject.Apply();

            upsPromo.Verify(p => p.Apply());
        }

        [Fact]
        public void Apply_AddsTelemetryData_WhenSourceIsPromoFootnote()
        {

            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(UpsPromoSource), UpsPromoSource.PromoFootnote),
                new TypedParameter(typeof(UpsPromoAccountType), UpsPromoAccountType.NewAccount));
            testObject.Apply();

            telemetry.Verify(t => t.AddProperty("Ups.Promo.Result", "Accepted"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AccountNumber", "upsaccountnumber"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AppliedToExistingAccount", "true"));
        }

        [Fact]
        public void Apply_AddsTelemetryData_WhenSourceIsSetupWizard()
        {
            TelemetricUpsPromo testObject =
                mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(UpsPromoSource), UpsPromoSource.SetupWizard),
                    new TypedParameter(typeof(UpsPromoAccountType), UpsPromoAccountType.NewAccount));
            testObject.Apply();

            telemetry.Verify(t => t.AddProperty("Ups.Promo.Result", "Accepted"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AccountNumber", "upsaccountnumber"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AppliedToExistingAccount", "false"));
        }

        [Fact]
        public void Decline_DelegatesToUpsPromo()
        {
            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(UpsPromoSource), UpsPromoSource.SetupWizard),
                new TypedParameter(typeof(UpsPromoAccountType), UpsPromoAccountType.NewAccount));
            testObject.Decline();

            upsPromo.Verify(p => p.Decline());
        }

        [Fact]
        public void Decline_AddsTelemetryData()
        {
            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(UpsPromoSource), UpsPromoSource.PromoFootnote),
                new TypedParameter(typeof(UpsPromoAccountType), UpsPromoAccountType.NewAccount));
            testObject.Decline();

            telemetry.Verify(t => t.AddProperty("Ups.Promo.Result", "Declined"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AccountNumber", "upsaccountnumber"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AppliedToExistingAccount", "true"));
        }

        [Fact]
        public void RemindMe_DelegatesToUpsPromo()
        {
            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(UpsPromoSource), UpsPromoSource.SetupWizard),
                new TypedParameter(typeof(UpsPromoAccountType), UpsPromoAccountType.NewAccount));
            testObject.RemindMe();

            upsPromo.Verify(p => p.RemindMe());
        }

        [Fact]
        public void RemindMe_AddsTelemetryData()
        {
            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(UpsPromoSource), UpsPromoSource.PromoFootnote),
                new TypedParameter(typeof(UpsPromoAccountType), UpsPromoAccountType.NewAccount));
            testObject.RemindMe();

            telemetry.Verify(t => t.AddProperty("Ups.Promo.Result", "Remind Later"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AccountNumber", "upsaccountnumber"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AppliedToExistingAccount", "true"));
        }
    }
}
