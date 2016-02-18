using System.Xml;
using Autofac;
using Autofac.Extras.Moq;
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
        public void Refresh_DefersGettingLicenseCapabilitiesToTangoWebClient()
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
        public void Refresh_SetsDisabledReasonFromLicenseCapabilitiesResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.GetLicenseCapabilities(It.IsAny<ICustomerLicense>()))
                    .Throws(new TangoException("Disabled for some reason"));

                CustomerLicense customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "SomeKey"));

                customerLicense.Refresh();

                Assert.Equal("Disabled for some reason", customerLicense.DisabledReason);
            }
        }

        [Fact]
        public void Refresh_LogsDisabledReason_WhenTangoWebClientThrowsException()
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
        public void Save_RethrowsExceptionFromLicenseWriter()
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