using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Settings;
using ShipWorks.Shared.Users;
using ShipWorks.Tests.Shared;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Tests.Users
{
    public class CurrentUserSettingsTest : IDisposable
    {
        private readonly AutoMock mock;
        private UserSettingsEntity settings;

        public CurrentUserSettingsTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            settings = new UserSettingsEntity();

            mock.Mock<IUserSession>()
                .Setup(x => x.UpdateSettings(It.IsAny<Action<UserSettingsEntity>>()))
                .Callback((Action<UserSettingsEntity> x) => x(settings));

            mock.Mock<IUserSession>()
                .Setup(x => x.Settings)
                .Returns(settings);
        }

        [Fact]
        public void ShouldShowNotification_ReturnsTrue_WhenDialogSettingsDoesNotContainType()
        {
            settings.DialogSettingsObject = new DialogSettings
            {
                NotificationDialogSettings = new NotificationDialogSetting[] { UserConditionalNotificationType.SplitOrders }
            };

            var testObject = mock.Create<CurrentUserSettings>();
            var result = testObject.ShouldShowNotification(UserConditionalNotificationType.CombineOrders);

            Assert.True(result);
        }

        [Fact]
        public void ShouldShowNotification_ReturnsTrue_WhenDialogSettingsIsEmpty()
        {
            var testObject = mock.Create<CurrentUserSettings>();
            var result = testObject.ShouldShowNotification(UserConditionalNotificationType.CombineOrders);

            Assert.True(result);
        }

        [Fact]
        public void ShouldShowNotification_ReturnsTrue_WhenSettingsIsNull()
        {
            mock.Mock<IUserSession>().Reset();

            var testObject = mock.Create<CurrentUserSettings>();
            var result = testObject.ShouldShowNotification(UserConditionalNotificationType.CombineOrders);

            Assert.True(result);
        }

        [Fact]
        public void ShouldShowNotification_ReturnsFalse_WhenDialogSettingsContainsType()
        {
            settings.DialogSettingsObject = new DialogSettings
            {
                NotificationDialogSettings = new NotificationDialogSetting[] { UserConditionalNotificationType.CombineOrders }
            };

            var testObject = mock.Create<CurrentUserSettings>();
            var result = testObject.ShouldShowNotification(UserConditionalNotificationType.CombineOrders);

            Assert.False(result);
        }

        [Fact]
        public void StopShowingNotification_WhenUserHasNoDialogSettings_SavesDialogSettings()
        {
            var testObject = mock.Create<CurrentUserSettings>();
            testObject.StopShowingNotification(UserConditionalNotificationType.CombineOrders);

            Assert.Contains(UserConditionalNotificationType.CombineOrders,
                settings.DialogSettingsObject.NotificationDialogSettings);
        }

        [Fact]
        public void StopShowingNotification_DoesNotSaveDuplicates_WhenDialogIsDismissedTwice()
        {
            var testObject = mock.Create<CurrentUserSettings>();
            testObject.StopShowingNotification(UserConditionalNotificationType.CombineOrders);
            testObject.StopShowingNotification(UserConditionalNotificationType.CombineOrders);

            Assert.Equal(1, settings.DialogSettingsObject.NotificationDialogSettings.Count());
        }

        [Fact]
        public void StopShowingNotification_WhenUserHasExistingDialogSettings_SavesDialogSettings()
        {
            settings.DialogSettingsObject = new DialogSettings
            {
                NotificationDialogSettings = new NotificationDialogSetting[] { UserConditionalNotificationType.SplitOrders }
            };

            var testObject = mock.Create<CurrentUserSettings>();
            testObject.StopShowingNotification(UserConditionalNotificationType.CombineOrders);

            Assert.Contains(UserConditionalNotificationType.CombineOrders,
                settings.DialogSettingsObject.NotificationDialogSettings);
        }

        [Theory]
        [InlineData(UserConditionalNotificationType.CombineOrders, UserConditionalNotificationType.SplitOrders)]
        [InlineData(UserConditionalNotificationType.SplitOrders, UserConditionalNotificationType.CombineOrders)]
        public void StopShowingNotification_WhenMultipleDialogsHaveBeenDismissed_SortsNotificationTypes(
            UserConditionalNotificationType first, UserConditionalNotificationType second)
        {
            var testObject = mock.Create<CurrentUserSettings>();
            testObject.StopShowingNotification(first);
            testObject.StopShowingNotification(second);

            Assert.Equal(new NotificationDialogSetting[] { UserConditionalNotificationType.CombineOrders, UserConditionalNotificationType.SplitOrders },
                settings.DialogSettingsObject.NotificationDialogSettings);
        }

        [Fact]
        public void StartShowingNotifications_RemovesTypeFromDialogSettings()
        {
            settings.DialogSettingsObject = new DialogSettings
            {
                DismissedNotifications = new[] { UserConditionalNotificationType.SplitOrders }
            };

            var testObject = mock.Create<CurrentUserSettings>();
            testObject.StartShowingNotification(UserConditionalNotificationType.SplitOrders);

            Assert.Empty(settings.DialogSettingsObject.DismissedNotifications);
        }

        [Fact]
        public void StartShowingNotifications_RemovesTypeFromDialogSettings_WhenDialogSettingsIsEmpty()
        {
            settings.DialogSettingsObject = new DialogSettings
            {
                DismissedNotifications = new UserConditionalNotificationType[0]
            };

            var testObject = mock.Create<CurrentUserSettings>();
            testObject.StartShowingNotification(UserConditionalNotificationType.SplitOrders);

            Assert.Empty(settings.DialogSettingsObject.DismissedNotifications);
        }

        [Fact]
        public void ShouldShowNotification_ReturnsTrue_WhenUserSettingsIsNull()
        {
            settings = null;
            var testObject = mock.Create<CurrentUserSettings>();
            Assert.True(testObject.ShouldShowNotification(UserConditionalNotificationType.CombineOrders));

            settings = new UserSettingsEntity();
        }

        [Fact]
        public void SetUIMode_SetsUIMode()
        {
            var userSettings = new UserSettingsEntity
            {
                UIMode = UIMode.OrderLookup
            };

            mock.Mock<IUserSession>()
                .Setup(x => x.User)
                .Returns(new UserEntity
                {
                    Settings = userSettings
                });

            var testObject = mock.Create<CurrentUserSettings>();
            testObject.SetUIMode(UIMode.Batch);

            Assert.Equal(UIMode.Batch, userSettings.UIMode);
        }

        [Fact]
        public void SetUIMode_SavesUserEntity()
        {
            var userSettings = new UserSettingsEntity
            {
                UIMode = UIMode.OrderLookup
            };

            mock.Mock<IUserSession>()
                .Setup(x => x.User)
                .Returns(new UserEntity
                {
                    Settings = userSettings
                });

            var testObject = mock.Create<CurrentUserSettings>();
            testObject.SetUIMode(UIMode.Batch);

            mock.Mock<ISqlAdapter>().Verify(a=>a.SaveAndRefetch(userSettings));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
