using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Activation;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.Activation
{
    public class CustomerLicenseActivationActivityTest
    {
        [Fact]
        public void Execute_ThrowsShipWorksLicenseException_WhenTangoActivationFails()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ITangoWebClient>().Setup(w => w.ActivateLicense(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new GenericResult<IActivationResponse>(null)
                    {
                        Success = false,
                        Message = "something went wrong",
                        Context = null
                    });

                var testObject = mock.Create<CustomerLicenseActivationActivity>();

                ShipWorksLicenseException ex = Assert.Throws<ShipWorksLicenseException>(
                        () => testObject.Execute("some@email.com", "randompassword"));

                Assert.Equal("something went wrong", ex.Message);
            }
        }

        [Fact]
        public void Execute_DelegatesToCustomerLicense_ToSave()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var tangoWebClient = mock.Mock<ITangoWebClient>();

                tangoWebClient.Setup(w => w.ActivateLicense(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new GenericResult<IActivationResponse>(null)
                    {
                        Context = MockActivateLicense(mock, "originalKey", "bob"),
                        Success = true
                    });

                var customerLicense = mock.Mock<ICustomerLicense>();

                var testObject = mock.Create<CustomerLicenseActivationActivity>();

                testObject.Execute("some@email.com", "randompassword");

                customerLicense.Verify(c => c.Save(), Times.Once);
            }
        }
        
        [Fact]
        public void Execute_SetsCustomerLicenseKey_FromActivationResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.ActivateLicense(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new GenericResult<IActivationResponse>(null)
                    {
                        Success = true,
                        Context = MockActivateLicense(mock, "TheKey", "bob")
                    });

                var customerLicense = mock.Create<CustomerLicense>(new NamedParameter("key", "someKey"));

                // Mock up the CustomerLicense constructor parameter Func<string, ICustomerLicense>
                var repo = mock.MockRepository.Create<Func<string, ICustomerLicense>>();
                repo.Setup(x => x(It.IsAny<string>()))
                    .Returns(customerLicense);
                mock.Provide(repo.Object);

                var testObject = mock.Create<CustomerLicenseActivationActivity>();

                testObject.Execute("foo@bar.com", "baz");

                repo.Verify(r => r("TheKey"), Times.Once);
            }
        }

        [Fact]
        public void Execute_SetsStampsUsername_FromActivationResponse()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var activationResponse = MockActivateLicense(mock, "TheKey", "bob");
                activationResponse.StampsUsername = "stampsUsername";

                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.ActivateLicense(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new GenericResult<IActivationResponse>(null)
                    {
                        Success = true,
                        Context = activationResponse
                    });

                string setStampsUsername = string.Empty;

                var customerLicense = mock.Mock<ICustomerLicense>(new NamedParameter("key", "someKey"));
                customerLicense.SetupSet(l => l.StampsUsername = It.IsAny<string>())
                    .Callback<string>(value => setStampsUsername = value);

                var testObject = mock.Create<CustomerLicenseActivationActivity>();

                testObject.Execute("foo@bar.com", "baz");

                Assert.Equal(activationResponse.StampsUsername, setStampsUsername);
            }
        }

        [Fact]
        public void Activate_DelegatestToTangoWebClient_ToActivateLicense_WithEmailAndPassword()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var tangoWebClient = mock.Mock<ITangoWebClient>();

                tangoWebClient.Setup(w => w.ActivateLicense(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new GenericResult<IActivationResponse>(null)
                    {
                        Context = MockActivateLicense(mock, "key", "user"),
                        Success = true
                    });

                var testObject = mock.Create<CustomerLicenseActivationActivity>();
                testObject.Execute("some@email.com", "randompassword");

                tangoWebClient.Verify(w => w.ActivateLicense("some@email.com", "randompassword"), Times.Once);
            }
        }

        private IActivationResponse MockActivateLicense(AutoMock mock, string key, string associatedUserName)
        {
            var activationResponseMock = mock.Mock<IActivationResponse>();

            activationResponseMock
                .Setup(r => r.Key)
                .Returns(key);

            activationResponseMock
                .Setup(r => r.AssociatedStampsUsername)
                .Returns(associatedUserName);

            return activationResponseMock.Object;
        }

    }
}
