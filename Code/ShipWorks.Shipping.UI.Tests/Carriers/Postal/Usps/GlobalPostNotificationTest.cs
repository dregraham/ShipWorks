using System;
using System.Data.SqlTypes;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.UI.Carriers.Postal.Usps;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.UI.Controls.WebBrowser;
using ShipWorks.Users;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Shipping.UI.Tests.Carriers.Postal.Usps
{
    public class GlobalPostNotificationTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly GlobalPostLabelNotification testObject;

        public GlobalPostNotificationTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<ICurrentUserSettings>()
                .Setup(x => x.ShouldShowNotification(It.IsAny<UserConditionalNotificationType>(), AnyDate))
                .Returns(true);

            testObject = mock.Create<GlobalPostLabelNotification>();
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
            mock.Mock<IDismissableWebBrowserDlgViewModel>()
                .Setup(v => v.Dismissed).Returns(true);

            testObject.Show(new ShipmentEntity() { Postal = new PostalShipmentEntity() { Service = (int) PostalServiceType.GlobalPostEconomyIntl } });

            mock.Mock<ICurrentUserSettings>()
                .Verify(x => x.StopShowingNotification(UserConditionalNotificationType.GlobalPostChange));
        }

        [Fact]
        public void Show_SetsNextGlobalPostNotificationDateToCurrentDatePlusOneDay_WhenNotDismissed()
        {
            DateTime notificationDate = SqlDateTime.MinValue.Value;

            mock.Mock<IDismissableWebBrowserDlgViewModel>()
                .Setup(v => v.Dismissed).Returns(false);

            testObject.Show(new ShipmentEntity() { Postal = new PostalShipmentEntity() { Service = (int) PostalServiceType.GlobalPostEconomyIntl } });

            mock.Mock<ICurrentUserSettings>()
                .Verify(x => x.StopShowingNotificationFor(UserConditionalNotificationType.GlobalPostChange, TimeSpan.FromDays(1)));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}