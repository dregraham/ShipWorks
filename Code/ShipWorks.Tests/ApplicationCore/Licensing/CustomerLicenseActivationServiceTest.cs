using System;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using Autofac;
using Autofac.Core;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using log4net;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class CustomerLicenseActivationServiceTest
    {
        [Fact]
        public void Activate_ThrowsShipWorksLicenseException_WhenTangoActivationFails()
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

                var testObject = mock.Create<CustomerLicenseActivationService>();

                var ex =
                    Assert.Throws<ShipWorksLicenseException>(
                        () => testObject.Activate("some@email.com", "randompassword"));
                Assert.Equal("something went wrong", ex.Message);
            }
        }

        [Fact]
        public void Activate_CallsITangoWebClient_WithUsernamePassword()
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

                var testObject = mock.Create<CustomerLicenseActivationService>();
                testObject.Activate("some@email.com", "randompassword");

                tangoWebClient.Verify(w => w.ActivateLicense("some@email.com", "randompassword"), Times.Once);
            }
        }

        [Fact]
        public void Activate_CallsSave_WithCustomerLicense()
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

                var testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate("some@email.com", "randompassword");

                customerLicense.Verify(c => c.Save(), Times.Once);
            }
        }

        [Fact]
        public void Activate_CustomerCreatedWithKey()
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

                // Mock up the CustomerLicense constructor parameter Func<string, ICustomerLicense> 
                var repo = mock.MockRepository.Create<Func<string, ICustomerLicense>>();
                repo.Setup(x => x(It.IsAny<string>()))
                    .Returns(mock.Mock<ICustomerLicense>().Object);
                mock.Provide(repo.Object);

                var testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate("some@email.com", "randompassword");

                repo.Verify(r => r("originalKey"), Times.Once);
            }
        }

        [Fact]
        public void Activate_SetsKey_FromActivationResponse()
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

                var testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate("foo@bar.com", "baz");

                repo.Verify(r => r("TheKey"), Times.Once);
            }
        }

        [Fact]
        public void Activate_PopulateUspsAccountEntityIsCalled_WhenNoUspsAccountsAndActivateLicenseReturnsAssociatedStampsUsername()
        {
            using (var mock = AutoMock.GetLoose())
            {
                MockSuccessfullActivateLicense(mock, "bob");

                mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>()
                    .Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                var uspsWebClient = mock.Mock<IUspsWebClient>();

                var testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate("bob", "1234");

                uspsWebClient.Verify(wc => wc.PopulateUspsAccountEntity(It.IsAny<UspsAccountEntity>()), Times.Once);
            }
        }

        [Fact]
        public void Activate_NewUpsAccountCreated_WhenNoUspsAccountsAndActivateLicenseReturnsAssociatedStampsUsername()
        {
            using (var mock = AutoMock.GetLoose())
            {
                MockSuccessfullActivateLicense(mock, "bob");

                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                var testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate("bob", "1234");

                repo.Verify(r=>r.Save(It.IsAny<UspsAccountEntity>()), Times.Once);
            }
        }

        [Fact]
        public void Activate_AccountCreatedWithCorrectUsername()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string shipworksUsername = "bob";
                string stampsUsername = "kevin";

                MockSuccessfullActivateLicense(mock, stampsUsername);
                
                UspsAccountEntity createdAccount = null;

                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());
                repo
                    .Setup(r => r.Save(It.IsAny<UspsAccountEntity>()))
                    .Callback((UspsAccountEntity account) => createdAccount = account);

                var testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate(shipworksUsername, "1234");

                Assert.Equal(stampsUsername, createdAccount.Username);
            }
        }

        [Fact]
        public void Activate_LogException_WhenUspsWebClientThrowsUspsApiException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                MockSuccessfullActivateLicense(mock, "username");

                UspsApiException exception = new UspsApiException(new SoapException("Something Went Wrong", null));

                mock.Mock<IUspsWebClient>()
                    .Setup(w => w.PopulateUspsAccountEntity(It.IsAny<UspsAccountEntity>()))
                    .Throws(exception);

                Mock<ILog> logger = mock.Mock<ILog>();

                Mock<Func<Type, ILog>> repo = mock.MockRepository.Create<Func<Type, ILog>>();
                repo.Setup(x => x(It.IsAny<Type>()))
                    .Returns(logger.Object);
                mock.Provide(repo.Object);

                CustomerLicenseActivationService testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate("bob", "some password");

                logger.Verify(l => l.Error($"Error when populating USPS account information: {exception.Message}"), Times.Once);
            }
        }

        [Fact]
        public void Activate_LogException_WhenUspsWebClientThrowsUspsException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                MockSuccessfullActivateLicense(mock, "username");

                UspsException exception = new UspsException("Something Went Wrong");

                mock.Mock<IUspsWebClient>()
                    .Setup(w => w.PopulateUspsAccountEntity(It.IsAny<UspsAccountEntity>()))
                    .Throws(exception);

                Mock<ILog> logger = mock.Mock<ILog>();

                Mock<Func<Type, ILog>> repo = mock.MockRepository.Create<Func<Type, ILog>>();
                repo.Setup(x => x(It.IsAny<Type>()))
                    .Returns(logger.Object);
                mock.Provide(repo.Object);

                CustomerLicenseActivationService testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate("bob", "some password");

                logger.Verify(l => l.Error($"Error when populating USPS account information: {exception.Message}"), Times.Once);
            }
        }

        [Fact]
        public void Activate_Delegates_WhenUspsWebClientThrowsUspsException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                MockSuccessfullActivateLicense(mock, "username");

                UspsException exception = new UspsException("Something Went Wrong");

                mock.Mock<IUspsWebClient>()
                    .Setup(w => w.PopulateUspsAccountEntity(It.IsAny<UspsAccountEntity>()))
                    .Throws(exception);

                Mock<ICustomerLicense> customerLicense =
                    mock.Mock<ICustomerLicense>(new TypedParameter(typeof (string), "key"));

                Mock<Func<string, ICustomerLicense>> repo = mock.MockRepository.Create<Func<string, ICustomerLicense>>();
                repo.Setup(x => x(It.IsAny<string>()))
                    .Returns(customerLicense.Object);
                mock.Provide(repo.Object);

                CustomerLicenseActivationService testObject = mock.Create<CustomerLicenseActivationService>();

                ICustomerLicense license = testObject.Activate("bob", "some password");

                // verify that we are sending the key string from the response to the 
                // factory that is creating the customer license object
                repo.Verify(r => r("key"), Times.Once);
                
                // verify that the customer license we get back fromt he activation service 
                // is the same one that the customer license factory created
                Assert.Equal(customerLicense.Object, license);
            }
        }

        [Fact]
        public void Activate_DelegatesToSecureText_WhenEncryptingUspsAccountPassword()
        {
            using (var mock = AutoMock.GetLoose())
            {
                MockSuccessfullActivateLicense(mock, "bob");

                string password = "1234";
                UspsAccountEntity createdAccount = null;

                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                repo.Setup(r => r.Save(It.IsAny<UspsAccountEntity>()))
                    .Callback((UspsAccountEntity account) => createdAccount = account);

                var testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate("bob", password);

                Assert.Equal(SecureText.Encrypt(password, createdAccount.Username), createdAccount.Password);
            }
        }

        [Fact]
        public void Activate_DelegatesToRepository_WhenSavingUspsAccount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                MockSuccessfullActivateLicense(mock, "bob");

                string password = "1234";
                UspsAccountEntity createdAccount = null;

                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                repo.Setup(r => r.Save(It.IsAny<UspsAccountEntity>()))
                    .Callback((UspsAccountEntity account) => createdAccount = account);

                var testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate("bob", password);

                repo.Verify(r => r.Save(createdAccount), Times.Once());
            }
        }

        [Fact]
        public void Activate_AccountNotCreated_WhenUspsAccountExists()
        {
            using (var mock = AutoMock.GetLoose())
            {
                MockSuccessfullActivateLicense(mock, "bob");

                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new[] {new UspsAccountEntity()});

                var testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate("bob", "1234");

                repo.Verify(r => r.Save(It.IsAny<UspsAccountEntity>()), Times.Never);
            }
        }

        [Fact]
        public void Activate_AccountNotCreated_WhenNoAssociatedStampsUsername()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Activate returns empty associated username
                mock.Mock<ITangoWebClient>()
                    .Setup(w => w.ActivateLicense(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(new GenericResult<IActivationResponse>(null)
                    {
                        Context = MockActivateLicense(mock, "originalKey", string.Empty),
                        Success = true
                    });

                // No accounts exist
                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                var testObject = mock.Create<CustomerLicenseActivationService>();

                testObject.Activate("bob", "1234");

                repo.Verify(r => r.Save(It.IsAny<UspsAccountEntity>()), Times.Never);
            }
        }

        private void MockSuccessfullActivateLicense(AutoMock mock, string associatedUsername)
        {
            mock.Mock<ITangoWebClient>()
                .Setup(c => c.ActivateLicense(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new GenericResult<IActivationResponse>(MockActivateLicense(mock, "key", associatedUsername))
                {
                    Success = true
                });
        }

        private IActivationResponse MockActivateLicense(AutoMock mock, string key, string associatedUserName)
        {
            var activationResponseMock = mock.Mock<IActivationResponse>();

            activationResponseMock
                .Setup(r => r.Key)
                .Returns(key);

            activationResponseMock
                .Setup(r => r.AssociatedStampsUserName)
                .Returns(associatedUserName);

            return activationResponseMock.Object;
        }
    }
}