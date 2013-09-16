using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal.Express1;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Express1
{
    [TestClass]
    public class Express1RegistrationTest
    {
        private Express1Registration testObject;

        private Mock<IExpress1RegistrationGateway> gateway;
        private Mock<IExpress1RegistrationRepository> repository;
        private Mock<IExpress1RegistrationValidator> validator;

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

            repository = new Mock<IExpress1RegistrationRepository>();
            repository.Setup(r => r.Save(It.IsAny<Express1Registration>()));

            validator = new Mock<IExpress1RegistrationValidator>();
            validator.Setup(v => v.Validate(It.IsAny<Express1Registration>())).Returns(new List<Express1ValidationError>());

            testObject = new Express1Registration(ShipmentTypeCode.Express1Stamps, gateway.Object, repository.Object, validator.Object);
        }

        [TestMethod]
        public void AddExistingAccount_ValidatesRegistration_Test()
        {
            testObject.AddExistingAccount();

            validator.Verify(v => v.Validate(testObject));
        }

        [TestMethod]
        public void AddExistingAccount_DelegatesToRepository_Test()
        {
            testObject.AddExistingAccount();

            repository.Verify(r => r.Save(testObject));
        }

        [TestMethod]
        public void SaveAccount_DelegatesToRepository_Test()
        {
            testObject.AddExistingAccount();

            repository.Verify(r => r.Save(testObject));
        }

        [TestMethod]
        public void CreateNewAccount_ValidatesRegistration_Test()
        {
            testObject.CreateNewAccount();

            validator.Verify(v => v.Validate(testObject));
        }

        [TestMethod]
        public void CreateNewAccount_DelegatesToGateway_Test()
        {
            testObject.CreateNewAccount();

            gateway.Verify(r => r.Register(testObject));
        }

        [TestMethod]
        public void CreateNewAccount_AssignsAccountNumber_Test()
        {
            testObject.CreateNewAccount();

            Assert.AreEqual(registrationResult.AccountNumber, testObject.AccountNumber);
        }

        [TestMethod]
        public void CreateNewAccount_AssignsPassword_Test()
        {
            testObject.CreateNewAccount();

            Assert.AreEqual(registrationResult.Password, testObject.Password);
        }

        [TestMethod]
        public void CreateNewAccount_DelegatesToRepository_Test()
        {
            testObject.CreateNewAccount();

            repository.Verify(r => r.Save(testObject));
        }
    }
}
