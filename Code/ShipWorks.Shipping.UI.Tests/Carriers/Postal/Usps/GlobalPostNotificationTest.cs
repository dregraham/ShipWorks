﻿using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
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

        public GlobalPostNotificationTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void AppliesToSession_ReturnsTrue_WhenNextGlobalPostNotificationDateIsInPast()
        {
            var testObject = mock.Create<GlobalPostLabelNotification>();

            Assert.True(testObject.AppliesToCurrentUser());
        }

        [Fact]
        public void AppliesToSession_ReturnsFalse_WhenNextGlobalPostNotificationDateIsInFuture()
        {
            mock.Mock<IUserSession>()
                .Setup(u => u.User)
                .Returns(new UserEntity { Settings = new UserSettingsEntity { NextGlobalPostNotificationDate = SqlDateTime.MaxValue.Value } });

            var testObject = mock.Create<GlobalPostLabelNotification>();

            Assert.False(testObject.AppliesToCurrentUser());
        }

        [Fact]
        public void Show_LoadsBrowserViewModelWithCorrectUrls_WhenShipmentIsGlobalPost()
        {
            Uri displayUri = new Uri("https://stamps.custhelp.com/app/answers/detail/a_id/3782");

            var viewModel = mock.Mock<IDismissableWebBrowserDlgViewModel>();
            var dialog = mock.MockRepository.Create<IDialog>();
            dialog.Setup(d => d.DataContext).Returns(viewModel.Object);
            mock.MockFunc<string, IDialog>(dialog);
            
            var testObject = mock.Create<GlobalPostLabelNotification>();

            testObject.Show(new ShipmentEntity() { Postal = new PostalShipmentEntity() { Service = (int)PostalServiceType.GlobalPostEconomyIntl } });

            viewModel.Verify(v => v.Load(displayUri, "Your GlobalPost Label", "https://stamps.custhelp.com/app/answers/detail/a_id/3802"));
        }

        [Fact]
        public void Show_LoadsBrowserViewModelWithCorrectUrls_WhenShipmentIsUspsGAP()
        {
            Uri displayUri = new Uri("https://stamps.custhelp.com/app/answers/detail/a_id/5174");

            var viewModel = mock.Mock<IDismissableWebBrowserDlgViewModel>();
            var dialog = mock.MockRepository.Create<IDialog>();
            dialog.Setup(d => d.DataContext).Returns(viewModel.Object);
            mock.MockFunc<string, IDialog>(dialog);

            var testObject = mock.Create<GlobalPostLabelNotification>();

            testObject.Show(new ShipmentEntity() { ShipmentTypeCode = ShipmentTypeCode.Usps, Postal = new PostalShipmentEntity() { Service = (int)PostalServiceType.InternationalFirst } });

            viewModel.Verify(v => v.Load(displayUri, "Your First-Class International Envelope Label", "http://support.shipworks.com/support/solutions/articles/4000114989"));
        }

        [Fact]
        public void Show_LoadsBrowserViewModelWithCorrectUrls_WhenShipmentIsEndiciaGAP()
        {
            Uri displayUri = new Uri("https://stamps.custhelp.com/app/answers/detail/a_id/5175");

            var viewModel = mock.Mock<IDismissableWebBrowserDlgViewModel>();
            var dialog = mock.MockRepository.Create<IDialog>();
            dialog.Setup(d => d.DataContext).Returns(viewModel.Object);
            mock.MockFunc<string, IDialog>(dialog);

            var testObject = mock.Create<GlobalPostLabelNotification>();

            testObject.Show(new ShipmentEntity() { ShipmentTypeCode = ShipmentTypeCode.Endicia, Postal = new PostalShipmentEntity() { Service = (int)PostalServiceType.InternationalFirst } });

            viewModel.Verify(v => v.Load(displayUri, "Your First-Class International Envelope Label", "http://support.shipworks.com/support/solutions/articles/4000114989"));
        }

        [Fact]
        public void Show_SetsNextGlobalPostNotificationDateToMaxSqlDate_WhenDismissed()
        {
            DateTime notificationDate = SqlDateTime.MinValue.Value;

            var viewModel = mock.Mock<IDismissableWebBrowserDlgViewModel>();
            viewModel.Setup(v => v.Dismissed).Returns(true);

            var dialog = mock.MockRepository.Create<IDialog>();
            dialog.Setup(d => d.DataContext).Returns(viewModel.Object);
            mock.MockFunc<string, IDialog>(dialog);

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
            testObject.Show(new ShipmentEntity() { Postal = new PostalShipmentEntity() { Service = (int)PostalServiceType.GlobalPostEconomyIntl } });

            Assert.Equal(SqlDateTime.MaxValue.Value.Date, user.Settings.NextGlobalPostNotificationDate.Date);
        }

        [Fact]
        public void Show_SetsNextGlobalPostNotificationDateToCurrentDatePlusOneDay_WhenNotDismissed()
        {
            DateTime notificationDate = SqlDateTime.MinValue.Value;

            var viewModel = mock.Mock<IDismissableWebBrowserDlgViewModel>();
            viewModel.Setup(v => v.Dismissed).Returns(false);

            var dialog = mock.MockRepository.Create<IDialog>();
            dialog.Setup(d => d.DataContext).Returns(viewModel.Object);
            mock.MockFunc<string, IDialog>(dialog);

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
            testObject.Show(new ShipmentEntity() { Postal = new PostalShipmentEntity() { Service = (int)PostalServiceType.GlobalPostEconomyIntl } });

            Assert.True(userSession.Object.User.Settings.NextGlobalPostNotificationDate.Date.Equals(DateTime.UtcNow.AddDays(1).Date));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}