using System;
using System.Collections.Generic;
using System.Web.Services.Protocols;
using Autofac.Extras.Moq;
using Interapptive.Shared.Security;
using log4net;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Licensing.Activation;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Settings;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.Activation
{
    public class UspsAccountSetupActivityTest
    {
        [Fact]
        public void Execute_PopulateUspsAccountEntityIsCalled_WhenNoUspsAccounts_AndAssociatedStampsUsernameIsNotEmpty()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>()
                    .Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                Mock<IUspsWebClient> uspsWebClient = mock.Mock<IUspsWebClient>();

                Mock<IShippingSettings> shippingSettings = mock.Mock<IShippingSettings>();
                shippingSettings.Setup(s => s.SetDefaultProvider(It.IsAny<ShipmentTypeCode>()));

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.AssociatedStampsUsername).Returns("bob");

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(license.Object, "1234");

                uspsWebClient.Verify(wc => wc.PopulateUspsAccountEntity(It.IsAny<UspsAccountEntity>()), Times.Once);
            }
        }

        [Fact]
        public void Execute_PopulateUspsAccountEntityIsNotCalled_WhenNoUspsAccounts_AndAssociatedStampsUsernameIsEmptyAndStampsUsernameIsNotEmpty()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>()
                    .Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                Mock<IUspsWebClient> uspsWebClient = mock.Mock<IUspsWebClient>();

                Mock<IShippingSettings> shippingSettings = mock.Mock<IShippingSettings>();
                shippingSettings.Setup(s => s.SetDefaultProvider(It.IsAny<ShipmentTypeCode>()));

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.StampsUsername).Returns("bob");

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(license.Object, "1234");

                uspsWebClient.Verify(wc => wc.PopulateUspsAccountEntity(It.IsAny<UspsAccountEntity>()), Times.Never);
            }
        }

        [Fact]
        public void Execute_NewUspsAccountIsCreated_WhenNoUspsAccounts_AndAssociatedStampsUsernameIsNotEmpty()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ICarrierAccountRepository<UspsAccountEntity>> repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                Mock<IShippingSettings> shippingSettings = mock.Mock<IShippingSettings>();
                shippingSettings.Setup(s => s.SetDefaultProvider(It.IsAny<ShipmentTypeCode>()));

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.AssociatedStampsUsername).Returns("bob");

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(license.Object, "1234");

                repo.Verify(r => r.Save(It.IsAny<UspsAccountEntity>()), Times.Once);
            }
        }

        [Fact]
        public void Execute_NewUspsAccountIsCreated_WhenNoUspsAccounts_AndAssociatedStampsUsernameIsEmptyAndStampsUsernameIsNotEmpty()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<ICarrierAccountRepository<UspsAccountEntity>> repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                Mock<IShippingSettings> shippingSettings = mock.Mock<IShippingSettings>();
                shippingSettings.Setup(s => s.SetDefaultProvider(It.IsAny<ShipmentTypeCode>()));

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.StampsUsername).Returns("bob");

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(license.Object, "1234");

                repo.Verify(r => r.Save(It.IsAny<UspsAccountEntity>()), Times.Once);
            }
        }

        [Fact]
        public void Execute_CreatesExistingAccount_WithCorrectUsername()
        {
            using (var mock = AutoMock.GetLoose())
            {
                UspsAccountEntity createdAccount = null;

                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                repo
                    .Setup(r => r.Save(It.IsAny<UspsAccountEntity>()))
                    .Callback((UspsAccountEntity account) => createdAccount = account);

                Mock<IShippingSettings> shippingSettings = mock.Mock<IShippingSettings>();
                shippingSettings.Setup(s => s.SetDefaultProvider(It.IsAny<ShipmentTypeCode>()));

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.AssociatedStampsUsername).Returns("bob");

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(license.Object, "1234");

                Assert.Equal("bob", createdAccount.Username);
            }
        }

        [Fact]
        public void Execute_CreatesNewAccount_WithCorrectUsername()
        {
            using (var mock = AutoMock.GetLoose())
            {
                UspsAccountEntity createdAccount = null;

                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                repo
                    .Setup(r => r.Save(It.IsAny<UspsAccountEntity>()))
                    .Callback((UspsAccountEntity account) => createdAccount = account);

                Mock<IShippingSettings> shippingSettings = mock.Mock<IShippingSettings>();
                shippingSettings.Setup(s => s.SetDefaultProvider(It.IsAny<ShipmentTypeCode>()));

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.StampsUsername).Returns("bob");

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(license.Object, "1234");

                Assert.Equal("bob", createdAccount.Username);
            }
        }

        [Fact]
        public void Execute_LogsException_WhenUspsWebClientThrowsUspsApiException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                UspsApiException exception = new UspsApiException(new SoapException("Something Went Wrong", null));

                mock.Mock<IUspsWebClient>()
                    .Setup(w => w.PopulateUspsAccountEntity(It.IsAny<UspsAccountEntity>()))
                    .Throws(exception);

                Mock<ILog> logger = mock.Mock<ILog>();

                Mock<Func<Type, ILog>> repo = mock.MockRepository.Create<Func<Type, ILog>>();
                repo.Setup(x => x(It.IsAny<Type>()))
                    .Returns(logger.Object);
                mock.Provide(repo.Object);

                Mock<IShippingSettings> shippingSettings = mock.Mock<IShippingSettings>();
                shippingSettings.Setup(s => s.SetDefaultProvider(It.IsAny<ShipmentTypeCode>()));

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.AssociatedStampsUsername).Returns("bob");

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(license.Object, "some password");

                logger.Verify(l => l.Error($"Error when populating USPS account information: {exception.Message}"), Times.Once);
            }
        }

        [Fact]
        public void Execute_LogsException_WhenUspsWebClientThrowsUspsException()
        {
            using (var mock = AutoMock.GetLoose())
            {
                UspsException exception = new UspsException("Something Went Wrong");

                mock.Mock<IUspsWebClient>()
                    .Setup(w => w.PopulateUspsAccountEntity(It.IsAny<UspsAccountEntity>()))
                    .Throws(exception);

                Mock<ILog> logger = mock.Mock<ILog>();

                Mock<Func<Type, ILog>> repo = mock.MockRepository.Create<Func<Type, ILog>>();
                repo.Setup(x => x(It.IsAny<Type>()))
                    .Returns(logger.Object);
                mock.Provide(repo.Object);

                Mock<IShippingSettings> shippingSettings = mock.Mock<IShippingSettings>();
                shippingSettings.Setup(s => s.SetDefaultProvider(It.IsAny<ShipmentTypeCode>()));

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.AssociatedStampsUsername).Returns("bob");

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(license.Object, "some password");

                logger.Verify(l => l.Error($"Error when populating USPS account information: {exception.Message}"), Times.Once);
            }
        }

        [Fact]
        public void Execute_DelegatesToSecureText_WhenEncryptingUspsAccountPassword()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string password = "1234";
                UspsAccountEntity createdAccount = null;

                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                repo.Setup(r => r.Save(It.IsAny<UspsAccountEntity>()))
                    .Callback((UspsAccountEntity account) => createdAccount = account);

                Mock<IShippingSettings> shippingSettings = mock.Mock<IShippingSettings>();
                shippingSettings.Setup(s => s.SetDefaultProvider(It.IsAny<ShipmentTypeCode>()));

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.AssociatedStampsUsername).Returns("bob");

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(license.Object, password);

                Assert.Equal(SecureText.Encrypt(password, createdAccount.Username), createdAccount.Password);
            }
        }

        [Fact]
        public void Execute_DelegatesToRepository_WhenSavingUspsAccount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                string password = "1234";
                UspsAccountEntity createdAccount = null;

                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                repo.Setup(r => r.Save(It.IsAny<UspsAccountEntity>()))
                    .Callback((UspsAccountEntity account) => createdAccount = account);

                Mock<IShippingSettings> shippingSettings = mock.Mock<IShippingSettings>();
                shippingSettings.Setup(s => s.SetDefaultProvider(It.IsAny<ShipmentTypeCode>()));

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.AssociatedStampsUsername).Returns("bob");

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(license.Object, password);

                repo.Verify(r => r.Save(createdAccount), Times.Once());
            }
        }

        [Fact]
        public void Execute_AccountIsNotCreated_WhenUspsAccountExists()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new[] { new UspsAccountEntity() });

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.AssociatedStampsUsername).Returns("bob");

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(license.Object, "1234");

                repo.Verify(r => r.Save(It.IsAny<UspsAccountEntity>()), Times.Never);
            }
        }

        [Fact]
        public void Execute_AccountIsNotCreated_WhenAssociatedStampsUsernameAndStampsUsernameIsEmpty()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // No accounts exist
                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                Mock<ICustomerLicense> license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.AssociatedStampsUsername).Returns(string.Empty);
                license.Setup(l => l.StampsUsername).Returns(string.Empty);

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(license.Object, "1234");

                repo.Verify(r => r.Save(It.IsAny<UspsAccountEntity>()), Times.Never);
            }
        }

        [Fact]
        public void Execute_AccountIsNotCreated_WhenLicenseIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // No accounts exist
                var repo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity>>();
                repo.Setup(r => r.Accounts)
                    .Returns(new List<UspsAccountEntity>());

                UspsAccountSetupActivity testObject = mock.Create<UspsAccountSetupActivity>();
                testObject.Execute(null, "1234");

                repo.Verify(r => r.Save(It.IsAny<UspsAccountEntity>()), Times.Never);
            }
        }
    }
}
