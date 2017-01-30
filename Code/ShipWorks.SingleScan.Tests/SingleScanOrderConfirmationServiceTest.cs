using System;
using Autofac.Extras.Moq;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class SingleScanOrderConfirmationServiceTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly SingleScanOrderConfirmationService testObject;

        public SingleScanOrderConfirmationServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<SingleScanOrderConfirmationService>();
        }

        [Fact]
        public void Confirm_ReturnsTrue_WhenNumberOfMatchedOrdersIsOne()
        {
            Assert.True(testObject.Confirm(123, 1, "abcd"));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
