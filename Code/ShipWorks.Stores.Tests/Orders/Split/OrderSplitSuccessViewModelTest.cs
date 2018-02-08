using System;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task ShowSuccessConfirmation_OnlyShowsDialog_WhenRequested(bool showDialog, int expectedInvocations)
        {
            mock.Mock<ICurrentUserSettings>()
                .Setup(x => x.ShouldShowNotification(UserConditionalNotificationType.SplitOrders))
                .Returns(showDialog);

            await testObject.ShowSuccessConfirmation(Enumerable.Empty<string>());

            mock.Mock<IAsyncMessageHelper>()
                .Verify(x => x.ShowDialog(It.IsAny<Func<IDialog>>()), Times.Exactly(expectedInvocations));
        }

        [Fact]
        public async Task ShowSuccessConfirmation_SetsViewModelProperties_WhenDialogShouldBeShown()
        {
            await testObject.ShowSuccessConfirmation(new[] { "foo", "bar" });

            Assert.Contains("foo", testObject.SplitOrders);
            Assert.Contains("bar", testObject.SplitOrders);
        }

        [Fact]
        public async Task ShowSuccessConfirmation_SetsDataContext_OnDialog()
        {
            mock.Mock<IAsyncMessageHelper>()
                .Setup(x => x.ShowDialog(It.IsAny<Func<IDialog>>()))
                .Callback((Func<IDialog> func) => func())
                .ReturnsAsync(true);

            await testObject.ShowSuccessConfirmation(Enumerable.Empty<string>());

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
