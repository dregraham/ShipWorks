using System;
using Autofac.Extras.Moq;
using ShipWorks.Data;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class SingleScanOrderShortcutTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly SingleScanOrderShortcut testObject;

        public SingleScanOrderShortcutTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            var dataProvider = mock.Mock<IDataProvider>();
            dataProvider.Setup(o => o.GetOrderNumberComplete(777006)).Returns(string.Empty);
            dataProvider.Setup(o => o.GetOrderNumberComplete(123006)).Returns("123");

            testObject = mock.Create<SingleScanOrderShortcut>();
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
            Assert.False(testObject.AppliesTo("dude"));
        }

        [Fact]
        public void Contains_ReturnsFalse_WhenBarcodeTextContainsPrefixButNotPostfix()
        {
            Assert.False(testObject.AppliesTo("SWOdude"));
        }

        [Fact]
        public void Contains_ReturnsFalse_WhenBarcodeTextContainsPostfixButNotPrefix()
        {
            Assert.False(testObject.AppliesTo("dude006"));
        }

        [Fact]
        public void Contains_ReturnsTrue_WhenBarcodeTextContainsPrefixAndPostfix()
        {
            Assert.True(testObject.AppliesTo("SWO12345006"));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}