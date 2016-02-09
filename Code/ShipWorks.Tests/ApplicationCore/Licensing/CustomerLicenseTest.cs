using System.Xml;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using Xunit;
using log4net;
using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class CustomerLicenseTest
    {


        [Fact]
        public void Refresh_SetsLicenseCapabilities_GetLicenseCapabilitiesResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                LicenseCapabilities licenseResponse = new LicenseCapabilities(new XmlDocument());

                var tangoWebClient =
                    mock.Mock<ITangoWebClient>();

                tangoWebClient.Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseResponse);

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                customerLicense.Refresh();

                tangoWebClient.Verify(wc => wc.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()), Times.Once);
            }
        }

        [Fact]
        public void Refresh_SetsDisabledReason_GetLicenseCapabilitiesResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Throws(new TangoException("Disabled for some reason"));

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                customerLicense.Refresh();

                Assert.Equal(customerLicense.DisabledReason, "Disabled for some reason");
            }
        }

        [Fact]
        public void Refresh_LogsDisabledReason_ActivationResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var log = mock.Mock<ILog>();

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Throws(new TangoException("Disabled for some reason"));

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                customerLicense.Refresh();

                log.Verify(l => l.Warn(It.IsAny<TangoException>()), Times.Once);
            }
        }

        [Fact]
        public void Save_RethrowsException_FromLicenseWriter()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var writer = mock.Mock<ICustomerLicenseWriter>();

                writer.Setup(w => w.Write(It.IsAny<ICustomerLicense>())).Throws(new Exception("some random exception"));

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var ex = Assert.Throws<Exception>(() => customerLicense.Save());
                Assert.Equal("some random exception", ex.Message);
            }
        }

        [Fact]
        public void IsOverChannelLimit_ReturnsTrue_WhenActiveChannelsExceedChannelLimitAndNotInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.IsInTrial)
                    .Returns(false);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();

                Assert.True(testObject.IsOverChannelLimit);
            }
        }

        [Fact]
        public void IsOverChannelLimit_ReturnsFalse_WhenActiveChannelsExceedChannelLimitAndInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.IsInTrial)
                    .Returns(true);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();

                Assert.False(testObject.IsOverChannelLimit);
            }
        }

        [Fact]
        public void IsOverChannelLimit_ReturnsFalse_WhenActiveChannelsLessThanChannelLimitAndNotInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.ChannelLimit)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.IsInTrial)
                    .Returns(false);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();
                Assert.False(testObject.IsOverChannelLimit);
            }
        }

        [Fact]
        public void NumberOfChannelsOverLimit_Returns0_WhenActiveChannelsLessThanChannelLimitAndNotInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.ChannelLimit)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.IsInTrial)
                    .Returns(false);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();
                Assert.Equal(0, testObject.NumberOfChannelsOverLimit);
            }
        }

        [Fact]
        public void NumberOfChannelsOverLimit_Returns0_WhenActiveChannelsMoreThanChannelLimitAndInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(10);

                licenseCapabilities
                    .Setup(lc => lc.ChannelLimit)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.IsInTrial)
                    .Returns(true);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();
                Assert.Equal(0, testObject.NumberOfChannelsOverLimit);
            }
        }

        [Fact]
        public void NumberOfChannelsOverLimit_ReturnsNumberOverLimit_WhenActiveChannelsMoreThanChannelLimitAndNotInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(10);

                licenseCapabilities
                    .Setup(lc => lc.ChannelLimit)
                    .Returns(5);

                licenseCapabilities
                    .Setup(lc => lc.IsInTrial)
                    .Returns(false);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();
                Assert.Equal(5, testObject.NumberOfChannelsOverLimit);
            }
        }

        [Fact]
        public void EnforceChannelLimit_DialogShown_WhenLicenseOverChannelLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(5);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                var channelLimitDlg = mock.Mock<IChannelLimitDlg>();

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceChannelLimit();

                channelLimitDlg.Verify(d => d.ShowDialog(), Times.Once);
            }
        }

        [Fact]
        public void EnforceChannelLimit_DialogNotShown_WhenUnderChannelLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(0);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Returns(licenseCapabilities.Object);

                var channelLimitDlg = mock.Mock<IChannelLimitDlg>();

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceChannelLimit();

                channelLimitDlg.Verify(d => d.ShowDialog(), Times.Never);
            }
        }

        [Fact]
        public void EnforceChannelLimit_RefreshIsCalled_WhenOverChannelLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities
                    .Setup(lc => lc.ActiveChannels)
                    .Returns(5);

                Mock<ITangoWebClient> webClient = MockWebClientToReturnCapabilities(mock, licenseCapabilities);

                mock.Mock<IChannelLimitDlg>();

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceChannelLimit();

                // The only way to verify that Refresh is called is by making sure
                // GetLicenseCapabilities is called because that is all refresh does.
                webClient.Verify(d => d.GetLicenseCapabilities(testObject), Times.Once);
            }
        }

        private static Mock<ITangoWebClient> MockWebClientToReturnCapabilities(AutoMock mock, Mock<ILicenseCapabilities> licenseCapabilities)
        {
            var webClient = mock.Mock<ITangoWebClient>();
            webClient
                .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                .Returns(licenseCapabilities.Object);
            return webClient;
        }

        [Fact]
        public void IsShipmentLimitReached_IsTrue_WhenOverShipmentLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(c => c.ProcessedShipments)
                    .Returns(101);

                licenseCapabilities.Setup(c => c.ShipmentLimit)
                    .Returns(100);

                MockWebClientToReturnCapabilities(mock, licenseCapabilities);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();

                Assert.True(testObject.IsShipmentLimitReached);
            }
        }

        [Fact]
        public void IsShipmentLimitReached_IsTrue_WhenAtShipmentLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(c => c.ProcessedShipments)
                    .Returns(100);

                licenseCapabilities.Setup(c => c.ShipmentLimit)
                    .Returns(100);

                MockWebClientToReturnCapabilities(mock, licenseCapabilities);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();

                Assert.True(testObject.IsShipmentLimitReached);
            }
        }

        [Fact]
        public void IsShipmentLimitReached_IsFalse_WhenAtShipmentLimitAndInTrial()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(c => c.ProcessedShipments)
                    .Returns(100);

                licenseCapabilities.Setup(c => c.ShipmentLimit)
                    .Returns(100);

                licenseCapabilities.Setup(c => c.IsInTrial)
                    .Returns(true);

                MockWebClientToReturnCapabilities(mock, licenseCapabilities);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));
                testObject.Refresh();

                Assert.False(testObject.IsShipmentLimitReached);
            }
        }

        [Fact]
        public void EnforceShipmentLimit_CallsShowDialog_WhenShipmentLimitReached()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();
                MockWebClientToReturnCapabilities(mock, licenseCapabilities);

                var dlg = mock.Mock<IDialog>();

                mock.Mock<IUpgradePlanDlgFactory>()
                    .Setup(f => f.Create(It.IsAny<string>()))
                    .Returns(dlg.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceShipmentLimit();

                dlg.Verify(d=>d.ShowDialog(), Times.Once);
            }
        }

        [Fact]
        public void EnforceShipmentLimit_DoesNotCallShowDialog_WhenShipmentLimitNotReached()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var licenseCapabilities = mock.Mock<ILicenseCapabilities>();

                licenseCapabilities.Setup(c => c.ShipmentLimit)
                    .Returns(100);

                MockWebClientToReturnCapabilities(mock, licenseCapabilities);

                var dlg = mock.Mock<IDialog>();

                mock.Mock<IUpgradePlanDlgFactory>()
                    .Setup(f => f.Create(It.IsAny<string>()))
                    .Returns(dlg.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                testObject.EnforceShipmentLimit();

                dlg.Verify(d => d.ShowDialog(), Times.Never);
            }
        }

        [Fact]
        public void Activate_ReturnsActive_WhenTangoReturnsSuccess()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var response = mock.Mock<IAddStoreResponse>();
                response.SetupGet(r => r.Success)
                    .Returns(true);

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.AddStore(It.IsAny<CustomerLicense>(), It.IsAny<StoreEntity>()))
                    .Returns(response.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var result = testObject.Activate(new StoreEntity());

                Assert.Equal(LicenseActivationState.Active, result.Value);
            }
        }

        [Fact]
        public void Activate_ReturnsInvalid_WhenTangoReturnsFailedResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var response = mock.Mock<IAddStoreResponse>();
                response.SetupGet(r => r.Success)
                    .Returns(false);

                response.SetupGet(r => r.Error)
                    .Returns("blah");

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.AddStore(It.IsAny<CustomerLicense>(), It.IsAny<StoreEntity>()))
                    .Returns(response.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var result = testObject.Activate(new StoreEntity());

                Assert.Equal(LicenseActivationState.Invalid, result.Value);
            }
        }

        [Fact]
        public void Activate_ReturnsOverChannelLimit_WhenTangoReturnsOverChannelLimitResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var response = mock.Mock<IAddStoreResponse>();
                response.SetupGet(r => r.Success)
                    .Returns(false);

                response.SetupGet(r => r.Error)
                    .Returns("OverChannelLimit");

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.AddStore(It.IsAny<CustomerLicense>(), It.IsAny<StoreEntity>()))
                    .Returns(response.Object);

                CustomerLicense testObject = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                var result = testObject.Activate(new StoreEntity());

                Assert.Equal(LicenseActivationState.OverChannelLimit, result.Value);
            }
        }
    }
}