﻿using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.Shipping.Carriers.UPS.Promo.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes;
using ShipWorks.Shipping.Carriers.UPS.WebServices.Promo;
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
            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(bool), true));
            testObject.Apply();

            upsPromo.Verify(p => p.Apply());
        }

        [Fact]
        public void Apply_AddsTelemetryData_WhenExistingAccountIsTrue()
        {

            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(bool), true));
            testObject.Apply();

            telemetry.Verify(t => t.AddProperty("Ups.Promo.Result", "Applied"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AccountNumber", "upsaccountnumber"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AppliedToExistingAccount", "true"));
        }

        [Fact]
        public void Apply_AddsTelemetryData_WhenExistingAccountIsFalse()
        {
            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(bool), false));
            testObject.Apply();

            telemetry.Verify(t => t.AddProperty("Ups.Promo.Result", "Applied"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AccountNumber", "upsaccountnumber"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AppliedToExistingAccount", "false"));
        }

        [Fact]
        public void Decline_DelegatesToUpsPromo()
        {
            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(bool), true));
            testObject.Decline();

            upsPromo.Verify(p => p.Decline());
        }

        [Fact]
        public void Decline_AddsTelemetryData()
        {
            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(bool), true));
            testObject.Decline();

            telemetry.Verify(t => t.AddProperty("Ups.Promo.Result", "Declined"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AccountNumber", "upsaccountnumber"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AppliedToExistingAccount", "true"));
        }

        [Fact]
        public void RemindMe_DelegatesToUpsPromo()
        {
            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(bool), true));
            testObject.RemindMe();

            upsPromo.Verify(p => p.RemindMe());
        }

        [Fact]
        public void RemindMe_AddsTelemetryData()
        {
            TelemetricUpsPromo testObject = mock.Create<TelemetricUpsPromo>(new TypedParameter(typeof(bool), true));
            testObject.RemindMe();

            telemetry.Verify(t => t.AddProperty("Ups.Promo.Result", "Remind Later"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AccountNumber", "upsaccountnumber"));
            telemetry.Verify(t => t.AddProperty("Ups.Promo.AppliedToExistingAccount", "true"));
        }
    }
}