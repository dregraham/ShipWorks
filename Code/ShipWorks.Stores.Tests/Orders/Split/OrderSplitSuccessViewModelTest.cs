using System;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Stores.Orders.Split;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Stores.Tests.Orders.Split
{
    public class OrderSplitSuccessViewModelTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly OrderSplitSuccessViewModel testObject;

        public OrderSplitSuccessViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<OrderSplitSuccessViewModel>();

            mock.Mock<ICurrentUserSettings>()
                .Setup(x => x.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>()))
                .Returns(true);
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public void ShowSuccessConfirmation_OnlyShowsDialog_WhenRequested(bool showDialog, int expectedInvocations)
        {
            var dialog = mock.Mock<IOrderSplitSuccessDialog>().Object;

            mock.Mock<ICurrentUserSettings>()
                .Setup(x => x.ShouldShowNotification(UserConditionalNotificationType.SplitOrders))
                .Returns(showDialog);

            testObject.ShowSuccessConfirmation(Enumerable.Empty<string>());

            mock.Mock<IMessageHelper>()
                .Verify(x => x.ShowDialog(dialog), Times.Exactly(expectedInvocations));
        }

        [Fact]
        public void ShowSuccessConfirmation_SetsViewModelProperties_WhenDialogShouldBeShown()
        {
            testObject.ShowSuccessConfirmation(new[] { "foo", "bar" });

            Assert.Contains("foo", testObject.SplitOrders);
            Assert.Contains("bar", testObject.SplitOrders);
        }

        [Fact]
        public void ShowSuccessConfirmation_SetsDataContext_OnDialog()
        {
            testObject.ShowSuccessConfirmation(Enumerable.Empty<string>());

            mock.Mock<IOrderSplitSuccessDialog>()
                .VerifySet(x => x.DataContext = testObject);
        }

        [Theory]
        [InlineData(true, 1)]
        [InlineData(false, 0)]
        public void Dismiss_CallsStopShowingNotification_BasedOnDoNotShowAgainProperty(bool doNotShowAgain, int expectedInvocations)
        {
            testObject.DoNotShowAgain = doNotShowAgain;

            testObject.Dismiss.Execute(null);

            mock.Mock<ICurrentUserSettings>()
                .Verify(x => x.StopShowingNotification(UserConditionalNotificationType.SplitOrders), Times.Exactly(expectedInvocations));
        }

        [Fact]
        public void Dismiss_CallsClose_OnDialog()
        {
            testObject.Dismiss.Execute(null);

            mock.Mock<IOrderSplitSuccessDialog>().Verify(x => x.Close());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
