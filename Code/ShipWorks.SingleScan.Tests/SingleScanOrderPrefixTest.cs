using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class SingleScanOrderPrefixTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly SingleScanOrderPrefix testObject;

        public SingleScanOrderPrefixTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            var orderManager = mock.Mock<IOrderManager>();
            orderManager.Setup(o => o.FetchOrder(777006)).Returns((OrderEntity) null);
            orderManager.Setup(o => o.FetchOrder(123006)).Returns(new OrderEntity() {OrderNumber = 123});

            testObject = mock.Create<SingleScanOrderPrefix>();
        }

        [Fact]
        public void GetDisplayText_ReturnsBarcodeText_WhenContainsReturnsFalse()
        {
            Assert.Equal("text", testObject.GetDisplayText("text"));
        }

        [Fact]
        public void GetDisplayText_ReturnsBarcodeText_WhenBarcodeTextContainsPrefixButIsNotANumber()
        {
            Assert.Equal("SWOtext006", testObject.GetDisplayText("SWOtext006"));
        }

        [Fact]
        public void GetDisplayText_ReturnsBarcodeText_WhenBarcodeTextContainsPrefixButOrderIsNotFound()
        {
            Assert.Equal("SWO777006", testObject.GetDisplayText("SWO777006"));
        }

        [Fact]
        public void GetDisplayText_ReturnsOrderNumber_WhenBarcodeTextContainsPrefixAndOrderIsFound()
        {
            Assert.Equal("123", testObject.GetDisplayText("SWO123006"));
        }

        [Fact]
        public void Contains_ReturnsFalse_WhenBarcodeTextDoesNotContainPrefixOrPostfix()
        {
            Assert.False(testObject.Contains("dude"));
        }

        [Fact]
        public void Contains_ReturnsFalse_WhenBarcodeTextContainsPrefixButNotPostfix()
        {
            Assert.False(testObject.Contains("SWOdude"));
        }

        [Fact]
        public void Contains_ReturnsFalse_WhenBarcodeTextContainsPostfixButNotPrefix()
        {
            Assert.False(testObject.Contains("dude006"));
        }

        [Fact]
        public void Contains_ReturnsTrue_WhenBarcodeTextContainsPrefixAndPostfix()
        {
            Assert.True(testObject.Contains("SWO12345006"));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}