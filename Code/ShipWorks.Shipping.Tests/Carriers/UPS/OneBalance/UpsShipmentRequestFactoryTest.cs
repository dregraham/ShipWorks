using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.Ups.OneBalance;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.OneBalance
{
    public class UpsShipmentRequestFactoryTest
    {
        private readonly AutoMock mock;
        private readonly UpsShipmentRequestFactory testObject;

        public UpsShipmentRequestFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<UpsShipmentRequestFactory>();
        }

        [Fact]
        public void EnsureCarrierShipmentIsNotNull_ThrowsWhenUpsShipmentIsNulls()
        {
        }

    }
}
