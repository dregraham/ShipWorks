using System;
using Autofac.Extras.Moq;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.BestRate
{
    public class BestRateShipmentPreProcessorTest : IDisposable
    {
        readonly AutoMock mock;

        public BestRateShipmentPreProcessorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Test()
        {

        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
