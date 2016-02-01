using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Moq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Express1
{
    public class Express1RegistrationTest
    {
        private Express1Registration testObject;

        private Mock<IExpress1RegistrationGateway> gateway;
        private Mock<IExpress1RegistrationRepository> repository;
        private Mock<IExpress1RegistrationValidator> validator;
        private Mock<IExpress1PasswordEncryptionStrategy> encryptionStrategy;

        private Express1RegistrationResult registrationResult;

        public Express1RegistrationTest()
        {
            registrationResult = new Express1RegistrationResult
            {
                AccountNumber = "123456",
                Password = "SuperUltraMegaSecretPassword"
            };

            gateway = new Mock<IExpress1RegistrationGateway>();
            gateway.Setup(g => g.Register(It.IsAny<Express1Registration>())).Returns(registrationResult);
            gateway.Setup(g => g.VerifyAccount(It.IsAny<Express1Registration>()));

            repository = new Mock<IExpress1RegistrationRepository>();
            repository.Setup(r => r.Save(It.IsAny<Express1Registration>()));

            validator = new Mock<IExpress1RegistrationValidator>();
            validator.Setup(v => v.Validate(It.IsAny<Express1Registration>())).Returns(new List<Express1ValidationError>());
            validator.Setup(v => v.ValidatePersonalInfo(It.IsAny<Express1Registration>()))
                     .Returns(new List<Express1ValidationError>());
            validator.Setup(v => v.ValidatePaymentInfo(It.IsAny<Express1Registration>()))
                     .Returns(new List<Express1ValidationError>());

            encryptionStrategy = new Mock<IExpress1PasswordEncryptionStrategy>();
            encryptionStrategy.Setup(s => s.EncryptPassword(It.IsAny<Express1Registration>())).Returns("ThePasswordHasBeenEncrypted");

            testObject = new Express1Registration(ShipmentTypeCode.Express1Usps, gateway.Object, repository.Object,encryptionStrategy.Object, validator.Object);
        }

        [Fact]
        public void AddExistingAccount_ValidatesRegistration()
        {
            testObject.AddExistingAccount();

            validator.Verify(v => v.ValidatePersonalInfo(testObject), Times.Once());
        }

        [Fact]
        public void AddExistingAccount_VerifiesAccount()
        {
            testObject.AddExistingAccount();

            gateway.Verify(r => r.VerifyAccount(testObject));
        }

        [Fact]
        public void AddExistingAccount_DelegatesToRepository()
        {
            testObject.AddExistingAccount();

            repository.Verify(r => r.Save(testObject), Times.Once());
        }

        [Fact]
        public void SaveAccount_DelegatesToRepository()
        {
            testObject.AddExistingAccount();

            repository.Verify(r => r.Save(testObject), Times.Once());
        }

        [Fact]
        public void CreateNewAccount_ValidatesRegistration()
        {
            testObject.CreateNewAccount();

            validator.Verify(v => v.Validate(testObject), Times.Once());
        }

        [Fact]
        public void CreateNewAccount_DelegatesToGateway()
        {
            testObject.CreateNewAccount();

            gateway.Verify(r => r.Register(testObject), Times.Once());
        }

        [Fact]
        public void CreateNewAccount_AssignsAccountNumber()
        {
            testObject.CreateNewAccount();

            Assert.Equal(registrationResult.AccountNumber, testObject.UserName);
        }

        [Fact]
        public void CreateNewAccount_AssignsPassword()
        {
            testObject.CreateNewAccount();

            Assert.Equal(registrationResult.Password, testObject.PlainTextPassword);
        }

        [Fact]
        public void CreateNewAccount_DelegatesToRepository()
        {
            testObject.CreateNewAccount();

            repository.Verify(r => r.Save(testObject), Times.Once());
        }

        [Fact]
        public void ValidatePersonalInfo_DelegatesToValidator()
        {
            testObject.ValidatePersonalInfo();

            validator.Verify(v => v.ValidatePersonalInfo(testObject), Times.Once());
        }

        [Fact]
        public void ValidatePaymentInfo_DelegatesToValidator()
        {
            testObject.ValidatePaymentInfo();

            validator.Verify(v => v.ValidatePaymentInfo(testObject), Times.Once());
        }

        [Fact]
        public void DeleteAccount_DelegatesToRepository_WhenAccountIDHasValue()
        {
            testObject.AccountId = 1000;

            testObject.DeleteAccount();

            repository.Verify(r => r.Delete(testObject), Times.Once());
        }

        [Fact]
        public void DeleteAccount_DoesNotDelegateToRepository_WhenAccountIDIsNull()
        {
            testObject.AccountId = null;

            testObject.DeleteAccount();

            repository.Verify(r => r.Delete(testObject), Times.Never());
        }

        [Fact]
        public void DeleteAccount_DoesNotDelegateToRepository_WhenAccountIDHasNotBeenAssigned()
        {
            testObject.DeleteAccount();

            repository.Verify(r => r.Delete(testObject), Times.Never());
        }

        [Fact]
        public void EncryptedPassword_DelegatesToEncryptionStrategy()
        {
            string encryptedPassword = testObject.EncryptedPassword;

            encryptionStrategy.Verify(s => s.EncryptPassword(testObject), Times.Once());
        }

        [Fact]
        public void EncryptedPassword_DoesNotModifyValue_FromEncryptionStrategy()
        {
            encryptionStrategy.Setup(s => s.EncryptPassword(It.IsAny<Express1Registration>())).Returns("ThePasswordHasBeenEncrypted");
            string encryptedPassword = testObject.EncryptedPassword;

            Assert.Equal("ThePasswordHasBeenEncrypted", encryptedPassword);
        }
    }
}
