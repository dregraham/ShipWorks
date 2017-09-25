using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Tests.Shared;
using ShipWorks.UI.Dialogs.UserConditionalNotification;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.UI.Tests.Dialogs.UserConditionalNotification
{
    public class UserConditionalNotificationViewModelTest : IDisposable
    {
        readonly AutoMock mock;
        private Mock<IMessageHelper> messageHelper;

        public UserConditionalNotificationViewModelTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            messageHelper = mock.CreateMock<IMessageHelper>();
        }

        [Theory]
        [InlineData(UserConditionalNotificationType.CombineOrders)]
        [InlineData(UserConditionalNotificationType.SplitOrders)]
        public void Show_DelegatesToUserSettings(UserConditionalNotificationType type)
        {
            var testObject = mock.Create<UserConditionalNotificationViewModel>();
            testObject.Show(messageHelper.Object, "Foo", "Bar", type);

            mock.Mock<ICurrentUserSettings>()
                .Verify(x => x.ShouldShowNotification(type));
        }

        [Fact]
        public void Show_DoesNotShowDialog_WhenShouldNotShowDialog()
        {
            mock.Mock<ICurrentUserSettings>()
                .Setup(x => x.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>()))
                .Returns(false);

            var testObject = mock.Create<UserConditionalNotificationViewModel>();
            testObject.Show(messageHelper.Object, "Foo", "Bar", UserConditionalNotificationType.CombineOrders);

            messageHelper.Verify(x => x.ShowDialog(It.IsAny<IDialog>()), Times.Never);
        }

        [Fact]
        public void Show_ShowsDialog_WhenShouldShowDialog()
        {
            mock.Mock<ICurrentUserSettings>()
                .Setup(x => x.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>()))
                .Returns(true);

            var testObject = mock.Create<UserConditionalNotificationViewModel>();
            testObject.Show(messageHelper.Object, "Foo", "Bar", UserConditionalNotificationType.CombineOrders);

            messageHelper.Verify(x => x.ShowDialog(mock.Mock<IUserConditionalNotificationDialog>().Object));
        }

        [Fact]
        public void Show_SetsData_WhenShouldShowDialog()
        {
            mock.Mock<ICurrentUserSettings>()
                .Setup(x => x.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>()))
                .Returns(true);

            var testObject = mock.Create<UserConditionalNotificationViewModel>();
            testObject.Show(messageHelper.Object, "Foo", "Bar", UserConditionalNotificationType.CombineOrders);

            Assert.Equal("Foo", testObject.Title);
            Assert.Equal("Bar", testObject.Message);
        }


        [Fact]
        public void Show_SetsViewModelOnDialog_WhenShouldShowDialog()
        {
            mock.Mock<ICurrentUserSettings>()
                .Setup(x => x.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>()))
                .Returns(true);

            var testObject = mock.Create<UserConditionalNotificationViewModel>();
            testObject.Show(messageHelper.Object, "Foo", "Bar", UserConditionalNotificationType.CombineOrders);

            mock.Mock<IUserConditionalNotificationDialog>()
                .VerifySet(x => x.DataContext = testObject);
        }

        [Fact]
        public void DismissAction_DoesNotDelegateToUserSettings_WhenDoNotShowAgainIsFalse()
        {
            var testObject = mock.Create<UserConditionalNotificationViewModel>();
            testObject.DoNotShowAgain = false;

            testObject.Dismiss.Execute(null);

            mock.Mock<ICurrentUserSettings>()
                .Verify(x => x.StopShowingNotification(It.IsAny<UserConditionalNotificationType>()), Times.Never);
        }

        [Theory]
        [InlineData(UserConditionalNotificationType.CombineOrders)]
        [InlineData(UserConditionalNotificationType.SplitOrders)]
        public void DismissAction_DelegatesToUserSettings_WhenDoNotShowAgainIsTrue(UserConditionalNotificationType type)
        {
            var testObject = mock.Create<UserConditionalNotificationViewModel>();
            testObject.Show(messageHelper.Object, "Foo", "Bar", type);
            testObject.DoNotShowAgain = true;

            testObject.Dismiss.Execute(null);

            mock.Mock<ICurrentUserSettings>()
                .Verify(x => x.StopShowingNotification(type));
        }

        [Fact]
        public void DismissAction_ClosesDialog()
        {
            var testObject = mock.Create<UserConditionalNotificationViewModel>();

            testObject.Dismiss.Execute(null);

            mock.Mock<IUserConditionalNotificationDialog>()
                .Verify(x => x.Close());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
