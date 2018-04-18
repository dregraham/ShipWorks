using System;
using System.Data.SqlTypes;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.UI.Carriers.Postal.Usps;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.UI.Controls.WebBrowser;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Carriers.Postal.Usps
{
    public class GlobalPostNotificationTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly GlobalPostLabelNotification testObject;

        public GlobalPostNotificationTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<GlobalPostLabelNotification>();
        }

        [Fact]
        public void AppliesToCurrentUser_ReturnsTrue_WhenNextGlobalPostNotificationDateIsInPast()
        {
            Assert.True(testObject.AppliesToCurrentUser());
        }

        [Fact]
        public void AppliesToCurrentUser_ReturnsFalse_WhenNextGlobalPostNotificationDateIsInFuture()
        {
            mock.Mock<IUserSession>()
                .Setup(u => u.User)
                .Returns(new UserEntity { Settings = new UserSettingsEntity { NextGlobalPostNotificationDate = SqlDateTime.MaxValue.Value } });

            Assert.False(testObject.AppliesToCurrentUser());
        }

        [Fact]
        public void Show_LoadsBrowserViewModelWithCorrectUrls_WhenShipmentIsGlobalPost()
        {
            Uri displayUri = new Uri("https://stamps.custhelp.com/app/answers/detail/a_id/3782");

            testObject.Show(new ShipmentEntity() { Postal = new PostalShipmentEntity() { Service = (int) PostalServiceType.GlobalPostEconomyIntl } });

            mock.Mock<IDismissableWebBrowserDlgViewModel>()
                .Verify(v => v.Load(displayUri, "Your GlobalPost Label", "https://stamps.custhelp.com/app/answers/detail/a_id/3802"));
        }

        [Fact]
        public void Show_LoadsBrowserViewModelWithCorrectUrls_WhenShipmentIsUspsGAP()
        {
            Uri displayUri = new Uri("https://stamps.custhelp.com/app/answers/detail/a_id/5174");

            var shipment = Create.Shipment()
                .AsPostal(p => p.AsUsps()
                    .Set(x => x.Service = (int) PostalServiceType.InternationalFirst)
                    .Set(x => x.PackagingType = (int) PostalPackagingType.Envelope)
                    .Set(x => x.CustomsContentType = (int) PostalCustomsContentType.Other))
                .Build();
            testObject.Show(shipment);

            mock.Mock<IDismissableWebBrowserDlgViewModel>()
                .Verify(v => v.Load(displayUri, "Your First-Class International Envelope Label", "http://support.shipworks.com/support/solutions/articles/4000114989"));
        }

        [Fact]
        public void Show_LoadsBrowserViewModelWithCorrectUrls_WhenShipmentIsEndiciaGAP()
        {
            Uri displayUri = new Uri("https://stamps.custhelp.com/app/answers/detail/a_id/5175");

            var shipment = Create.Shipment()
                .AsPostal(p => p.AsEndicia()
                    .Set(x => x.Service = (int) PostalServiceType.InternationalFirst)
                    .Set(x => x.PackagingType = (int) PostalPackagingType.Envelope)
                    .Set(x => x.CustomsContentType = (int) PostalCustomsContentType.Other))
                .Build();
            testObject.Show(shipment);

            mock.Mock<IDismissableWebBrowserDlgViewModel>()
                .Verify(v => v.Load(displayUri, "Your First-Class International Envelope Label", "http://support.shipworks.com/support/solutions/articles/4000114989"));
        }

        [Theory]
        [InlineData(PostalServiceType.InternationalExpress)]
        [InlineData(PostalServiceType.InternationalFirst)]
        [InlineData(PostalServiceType.InternationalPriority)]
        public void Show_LoadsBrowserViewModelWithCorrectUrls_WhenShipmentIsPresort(PostalServiceType service)
        {
            Uri displayUri = new Uri("https://stamps.custhelp.com/app/answers/detail/a_id/5229");

            var shipment = Create.Shipment().AsPostal(p => p.Set(x => x.Service = (int) service)).Build();
            testObject.Show(shipment);

            mock.Mock<IDismissableWebBrowserDlgViewModel>()
                .Verify(v => v.Load(displayUri, "Your International Label", "http://support.shipworks.com/support/solutions/articles/4000121488-presort-labels"));
        }

        [Fact]
        public void Show_SetsNextGlobalPostNotificationDateToMaxSqlDate_WhenDismissed()
        {
            DateTime notificationDate = SqlDateTime.MinValue.Value;

            mock.Mock<IDismissableWebBrowserDlgViewModel>()
                .Setup(v => v.Dismissed).Returns(true);

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

            testObject.Show(new ShipmentEntity() { Postal = new PostalShipmentEntity() { Service = (int) PostalServiceType.GlobalPostEconomyIntl } });

            Assert.Equal(SqlDateTime.MaxValue.Value.Date, user.Settings.NextGlobalPostNotificationDate.Date);
        }

        [Fact]
        public void Show_SetsNextGlobalPostNotificationDateToCurrentDatePlusOneDay_WhenNotDismissed()
        {
            DateTime notificationDate = SqlDateTime.MinValue.Value;

            mock.Mock<IDismissableWebBrowserDlgViewModel>()
                .Setup(v => v.Dismissed).Returns(false);

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
            testObject.Show(new ShipmentEntity() { Postal = new PostalShipmentEntity() { Service = (int) PostalServiceType.GlobalPostEconomyIntl } });

            Assert.True(userSession.Object.User.Settings.NextGlobalPostNotificationDate.Date.Equals(DateTime.UtcNow.AddDays(1).Date));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}