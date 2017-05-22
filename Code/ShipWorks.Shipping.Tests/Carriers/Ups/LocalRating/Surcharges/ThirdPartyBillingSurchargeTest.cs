

using System;
using System.Collections.Generic;
using System.Text;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating.Surcharges
{
    public class ThirdPartyBillingSurchargeTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ThirdPartyBillingSurcharge testObject;
        
        public ThirdPartyBillingSurchargeTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = new ThirdPartyBillingSurcharge(
                new Dictionary<UpsSurchargeType, double> {[UpsSurchargeType.ThirdpartyBilling] = .0525});
        }

        [Fact]
        public void Apply_AddsThirdPartyBillingAmountToRate_WhenShipmentHasThirdPartyBilling()
        {
            Mock<IUpsLocalServiceRate> rate = mock.Mock<IUpsLocalServiceRate>();
            rate.SetupGet(r => r.Amount).Returns(25);

            testObject.Apply(new UpsShipmentEntity {PayorType = (int) UpsPayorType.ThirdParty}, rate.Object);

            rate.Verify(r => r.AddAmount((decimal) Math.Round(25 * .0525, 2),
                EnumHelper.GetDescription(UpsSurchargeType.ThirdpartyBilling)));
        }

        [Fact]
        public void Apply_DoesNotAddThirdPartyBillingAmountToRate_WhenShipmentDoesNotHaveThirdPartyBilling()
        {
            Mock<IUpsLocalServiceRate> rate = mock.Mock<IUpsLocalServiceRate>();
            rate.SetupGet(r => r.Amount).Returns(25);

            testObject.Apply(new UpsShipmentEntity { PayorType = (int)UpsPayorType.Sender }, rate.Object);

            rate.Verify(r => r.AddAmount(It.IsAny<decimal>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}