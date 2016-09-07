using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.UI.Carriers.Postal.Usps;
using ShipWorks.Tests.Shared;
using ShipWorks.UI.Controls.WebBrowser;
using ShipWorks.Users;
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
            userSession.Setup(u => u.User.NextGlobalPostNotificationDate)
                .Returns(new DateTime(1990, 01, 01));

            var testObject = mock.Create<GlobalPostLabelNotification>();

            Assert.True(testObject.AppliesToSession());
        }

        [Fact]
        public void AppliesToSession_ReturnsFalse_WhenNextGlobalPostNotificationDateIsInFuture()
        {
            var userSession = mock.Mock<IUserSession>();
            userSession.Setup(u => u.User.NextGlobalPostNotificationDate)
                .Returns(new DateTime(2500, 01, 01));

            var testObject = mock.Create<GlobalPostLabelNotification>();

            Assert.False(testObject.AppliesToSession());
        }

        [Fact]
        public void Show_LoadsBrowserViewModelWithCorrectUrls()
        {
            Uri displayUri = new Uri(DisplayUrl);

            var viewModel = mock.Mock<IDismissableWebBrowserDlgViewModel>();

            var browserFactory = mock.MockFunc<string, IDialog>();
            browserFactory.FunctionOutput.Setup(b => b.DataContext).Returns(viewModel.Object);

            var userSession = mock.Mock<IUserSession>();
            userSession.Setup(u => u.User.NextGlobalPostNotificationDate)
                .Returns(new DateTime(1990, 01, 01));

            var testObject = mock.Create<GlobalPostLabelNotification>();

            testObject.Show();

            viewModel.Verify(v => v.Load(displayUri, BrowserDlgTitle, MoreInfoUrl));
        }

        [Fact]
        public void Show_SetsNextGlobalPostNotificationDateToCurrentDatePlusTwoHundredYears_WhenDismissed()
        {
            DateTime notificationDate = new DateTime(1990, 01, 01).ToUniversalTime();

            var viewModel = mock.Mock<IDismissableWebBrowserDlgViewModel>();
            viewModel.Setup(v => v.Dismissed).Returns(true);

            var browserFactory = mock.MockFunc<string, IDialog>();
            browserFactory.FunctionOutput.Setup(b => b.DataContext).Returns(viewModel.Object);

            var user = new UserEntity();
            user.NextGlobalPostNotificationDate = notificationDate;

            var userSession = mock.Mock<IUserSession>();
            userSession.Setup(u => u.User)
                .Returns(user);

            var testObject = mock.Create<GlobalPostLabelNotification>();
            testObject.Show();

            Assert.True(user.NextGlobalPostNotificationDate.Date.Equals(DateTime.UtcNow.AddYears(200).Date));
        }

        [Fact]
        public void Show_SetsNextGlobalPostNotificationDateToCurrentDatePlusOneDay_WhenNotDismissed()
        {
            DateTime notificationDate = new DateTime(1990, 01, 01).ToUniversalTime();

            var viewModel = mock.Mock<IDismissableWebBrowserDlgViewModel>();
            viewModel.Setup(v => v.Dismissed).Returns(false);

            var browserFactory = mock.MockFunc<string, IDialog>();
            browserFactory.FunctionOutput.Setup(b => b.DataContext).Returns(viewModel.Object);

            var user = new UserEntity();
            user.NextGlobalPostNotificationDate = notificationDate;

            var userSession = mock.Mock<IUserSession>();
            userSession.Setup(u => u.User)
                .Returns(user);

            var testObject = mock.Create<GlobalPostLabelNotification>();
            testObject.Show();

            Assert.True(userSession.Object.User.NextGlobalPostNotificationDate.Date.Equals(DateTime.UtcNow.AddDays(1).Date));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}