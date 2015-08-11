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

        [TestInitialize]
        public void Initialize()
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
        public void AddExistingAccount_ValidatesRegistration_Test()
        {
            testObject.AddExistingAccount();

            validator.Verify(v => v.ValidatePersonalInfo(testObject), Times.Once());
        }

        [Fact]
        public void AddExistingAccount_VerifiesAccount_Test()
        {
            testObject.AddExistingAccount();

            gateway.Verify(r => r.VerifyAccount(testObject));
        }

        [Fact]
        public void AddExistingAccount_DelegatesToRepository_Test()
        {
            testObject.AddExistingAccount();

            repository.Verify(r => r.Save(testObject), Times.Once());
        }

        [Fact]
        public void SaveAccount_DelegatesToRepository_Test()
        {
            testObject.AddExistingAccount();

            repository.Verify(r => r.Save(testObject), Times.Once());
        }

        [Fact]
        public void CreateNewAccount_ValidatesRegistration_Test()
        {
            testObject.CreateNewAccount();

            validator.Verify(v => v.Validate(testObject), Times.Once());
        }

        [Fact]
        public void CreateNewAccount_DelegatesToGateway_Test()
        {
            testObject.CreateNewAccount();

            gateway.Verify(r => r.Register(testObject), Times.Once());
        }

        [Fact]
        public void CreateNewAccount_AssignsAccountNumber_Test()
        {
            testObject.CreateNewAccount();

            Assert.AreEqual(registrationResult.AccountNumber, testObject.UserName);
        }

        [Fact]
        public void CreateNewAccount_AssignsPassword_Test()
        {
            testObject.CreateNewAccount();

            Assert.AreEqual(registrationResult.Password, testObject.PlainTextPassword);
        }

        [Fact]
        public void CreateNewAccount_DelegatesToRepository_Test()
        {
            testObject.CreateNewAccount();

            repository.Verify(r => r.Save(testObject), Times.Once());
        }

        [Fact]
        public void ValidatePersonalInfo_DelegatesToValidator_Test()
        {
            testObject.ValidatePersonalInfo();

            validator.Verify(v => v.ValidatePersonalInfo(testObject), Times.Once());
        }

        [Fact]
        public void ValidatePaymentInfo_DelegatesToValidator_Test()
        {
            testObject.ValidatePaymentInfo();

            validator.Verify(v => v.ValidatePaymentInfo(testObject), Times.Once());
        }

        [Fact]
        public void DeleteAccount_DelegatesToRepository_WhenAccountIDHasValue_Test()
        {
            testObject.AccountId = 1000;

            testObject.DeleteAccount();

            repository.Verify(r => r.Delete(testObject), Times.Once());
        }

        [Fact]
        public void DeleteAccount_DoesNotDelegateToRepository_WhenAccountIDIsNull_Test()
        {
            testObject.AccountId = null;

            testObject.DeleteAccount();

            repository.Verify(r => r.Delete(testObject), Times.Never());
        }

        [Fact]
        public void DeleteAccount_DoesNotDelegateToRepository_WhenAccountIDHasNotBeenAssigned_Test()
        {
            testObject.DeleteAccount();

            repository.Verify(r => r.Delete(testObject), Times.Never());
        }

        [Fact]
        public void EncryptedPassword_DelegatesToEncryptionStrategy_Test()
        {
            string encryptedPassword = testObject.EncryptedPassword;

            encryptionStrategy.Verify(s => s.EncryptPassword(testObject), Times.Once());
        }

        [Fact]
        public void EncryptedPassword_DoesNotModifyValue_FromEncryptionStrategy_Test()
        {
            encryptionStrategy.Setup(s => s.EncryptPassword(It.IsAny<Express1Registration>())).Returns("ThePasswordHasBeenEncrypted");
            string encryptedPassword = testObject.EncryptedPassword;

            Assert.AreEqual("ThePasswordHasBeenEncrypted", encryptedPassword);
        }
    }
}
