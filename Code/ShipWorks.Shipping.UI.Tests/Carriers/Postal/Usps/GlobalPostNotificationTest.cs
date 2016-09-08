using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI.Carriers.Postal.Usps;
using ShipWorks.Tests.Shared;
using ShipWorks.UI.Controls.WebBrowser;
using ShipWorks.Users;
using System;
using System.Data.SqlTypes;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Carriers.Postal.Usps
{
    public class GlobalPostNotificationTest : IDisposable
    {
        private readonly AutoMock mock;
        private const string DisplayUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/3782";
        private const string MoreInfoUrl = "https://stamps.custhelp.com/app/answers/detail/a_id/3802";
        private const string BrowserDlgTitle = "Your GlobalPost Label";

        public GlobalPostNotificationTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void AppliesToSession_ReturnsTrue_WhenNextGlobalPostNotificationDateIsInPast()
        {
            var userSession = mock.Mock<IUserSession>();
            userSession.Setup(u => u.User.Settings.NextGlobalPostNotificationDate)
                .Returns(SqlDateTime.MinValue.Value);

            var testObject = mock.Create<GlobalPostLabelNotification>();

            Assert.True(testObject.AppliesToCurrentUser());
        }

        [Fact]
        public void AppliesToSession_ReturnsFalse_WhenNextGlobalPostNotificationDateIsInFuture()
        {
            var userSession = mock.Mock<IUserSession>();
            userSession.Setup(u => u.User.Settings.NextGlobalPostNotificationDate)
                .Returns(SqlDateTime.MaxValue.Value);

            var testObject = mock.Create<GlobalPostLabelNotification>();

            Assert.False(testObject.AppliesToCurrentUser());
        }

        [Fact]
        public void Show_LoadsBrowserViewModelWithCorrectUrls()
        {
            Uri displayUri = new Uri(DisplayUrl);

            var viewModel = mock.Mock<IDismissableWebBrowserDlgViewModel>();

            var browserFactory = mock.MockFunc<string, IDialog>();
            browserFactory.FunctionOutput.Setup(b => b.DataContext).Returns(viewModel.Object);

            var userSession = mock.Mock<IUserSession>();
            userSession.Setup(u => u.User.Settings.NextGlobalPostNotificationDate)
                .Returns(SqlDateTime.MinValue.Value);

            var testObject = mock.Create<GlobalPostLabelNotification>();

            testObject.Show();

            viewModel.Verify(v => v.Load(displayUri, BrowserDlgTitle, MoreInfoUrl));
        }

        [Fact]
        public void Show_SetsNextGlobalPostNotificationDateToCurrentDatePlusTwoHundredYears_WhenDismissed()
        {
            DateTime notificationDate = SqlDateTime.MinValue.Value;

            var viewModel = mock.Mock<IDismissableWebBrowserDlgViewModel>();
            viewModel.Setup(v => v.Dismissed).Returns(true);

            var browserFactory = mock.MockFunc<string, IDialog>();
            browserFactory.FunctionOutput.Setup(b => b.DataContext).Returns(viewModel.Object);

            var user = new UserEntity
            {
                Settings = new UserSettingsEntity
                {
                    NextGlobalPostNotificationDate = notificationDate
                }
            };

            var userSession = mock.Mock<IUserSession>();
            userSession.Setup(u => u.User)
                .Returns(user);

            var testObject = mock.Create<GlobalPostLabelNotification>();
            testObject.Show();

            Assert.True(user.Settings.NextGlobalPostNotificationDate.Date.Equals(DateTime.UtcNow.AddYears(200).Date));
        }

        [Fact]
        public void Show_SetsNextGlobalPostNotificationDateToCurrentDatePlusOneDay_WhenNotDismissed()
        {
            DateTime notificationDate = SqlDateTime.MinValue.Value;

            var viewModel = mock.Mock<IDismissableWebBrowserDlgViewModel>();
            viewModel.Setup(v => v.Dismissed).Returns(false);

            var browserFactory = mock.MockFunc<string, IDialog>();
            browserFactory.FunctionOutput.Setup(b => b.DataContext).Returns(viewModel.Object);

            var user = new UserEntity
            {
                Settings = new UserSettingsEntity
                {
                    NextGlobalPostNotificationDate = notificationDate
                }
            };

            var userSession = mock.Mock<IUserSession>();
            userSession.Setup(u => u.User)
                .Returns(user);

            var testObject = mock.Create<GlobalPostLabelNotification>();
            testObject.Show();

            Assert.True(userSession.Object.User.Settings.NextGlobalPostNotificationDate.Date.Equals(DateTime.UtcNow.AddDays(1).Date));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}