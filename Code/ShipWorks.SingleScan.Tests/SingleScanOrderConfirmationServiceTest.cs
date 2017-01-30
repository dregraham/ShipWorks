using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;
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

        [Fact]
        public void Confirm_ThrowsShippingException_WhenNumberOfMatchedOrdersIsZero()
        {
            Assert.Throws<ShippingException>(() => testObject.Confirm(123, 0, "abcd"));
        }

        [Fact]
        public void Confirm_DelegatesToMessageHelper_WhenNumberOfMatchedOrdersIsGreaterThanOne()
        {
            testObject.Confirm(123, 2, "abcd");
            mock.Mock<IMessageHelper>().Verify(m => m.ShowDialog(It.IsAny<Func<IForm>>()));
        }

        [Fact]
        public void Confirm_DelegatesToStoreManager_WhenNumberOfMatchedOrdersIsGreaterThanOne()
        {
            testObject.Confirm(123, 4, "abcd");
            mock.Mock<IStoreManager>().Verify(s => s.GetRelatedStore(123L));
        }

        [Fact]
        public void Confirm_DelegatesToDlgFactory_WhenNumberOfMatchedOrdersIsGreaterThanOne()
        {
            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns<Func<IForm>>(f =>
            {
                IForm form = f();
                return form.ShowDialog(null);
            });

            mock.Mock<IStoreManager>()
                .Setup(s => s.GetRelatedStore(123))
                .Returns(new StoreEntity() {StoreName = "Amazon"});

            int numberOfMatchedOrders = 3;

            MessagingText messaging = new MessagingText()
            {
                Title = "Multiple Matches Found",
                Body = $"ShipWorks found {numberOfMatchedOrders} orders matching this order number. The most recent order is from your 'Amazon' store. Scan the bar code again or click 'Use Most Recent Order' to print the label(s) for this order.",
                Continue = "Use Most Recent Order"
            };

            testObject.Confirm(123, numberOfMatchedOrders, "abcd");

            mock.Mock<IAutoPrintConfirmationDlgFactory>().Verify(f => f.Create("abcd", messaging));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
