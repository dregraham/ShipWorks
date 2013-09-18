﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Express1
{
    [TestClass]
    public class Express1RegistrationValidatorTest
    {
        private Express1RegistrationValidator testObject;

        private Express1Registration registration;
        private Mock<IExpress1RegistrationGateway> gateway;
        private Mock<IExpress1RegistrationRepository> repository;
        private Mock<IExpress1PaymentValidator> paymentValidator;

        [TestInitialize]
        public void Initialize()
        {
            gateway = new Mock<IExpress1RegistrationGateway>();
            repository = new Mock<IExpress1RegistrationRepository>();

            registration = new Express1Registration(ShipmentTypeCode.Express1Stamps, gateway.Object, repository.Object, testObject)
            {
                MailingAddress = new PersonAdapter()
                {
                    FirstName = "First",
                    LastName = "Last",
                    Company = "Company",
                    Street1 = "123 Main Street",
                    City = "St. Louis",
                    StateProvCode = "MO",
                    PostalCode = "63102",
                    Email = "someone@somewhere.com",
                    Phone = "123-456-7890"
                },
                Payment = new Express1PaymentInfo(Express1PaymentType.CreditCard)
            };

            paymentValidator = new Mock<IExpress1PaymentValidator>();
            paymentValidator.Setup(v => v.ValidatePaymentInfo(It.IsAny<Express1PaymentInfo>())).Returns(new List<Express1ValidationError>());
            
            testObject = new Express1RegistrationValidator(paymentValidator.Object);
        }

        [TestMethod]
        public void Validate_AddsError_WhenNameIsMissing_Test()
        {
            registration.MailingAddress.FirstName = string.Empty;
            registration.MailingAddress.MiddleName = string.Empty;
            registration.MailingAddress.LastName = string.Empty;

            List<Express1ValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Name is required", errors[0].Message);
        }

        [TestMethod]
        public void Validate_AddsError_WhenCompanyIsMissing_Test()
        {
            registration.MailingAddress.Company = string.Empty;

            List<Express1ValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Company is required", errors[0].Message);
        }

        [TestMethod]
        public void Validate_AddsError_WhenPhoneIsMissing_Test()
        {
            registration.MailingAddress.Phone = string.Empty;
            
            List<Express1ValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Phone number is required", errors[0].Message);
        }

        [TestMethod]
        public void Validate_AddsError_WhenEmailIsMissing_Test()
        {
            registration.MailingAddress.Email = string.Empty;

            List<Express1ValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("An email address is required", errors[0].Message);
        }

        [TestMethod]
        public void Validate_AddsError_WhenStreetIsMissing_Test()
        {
            registration.MailingAddress.Street1 = string.Empty;

            List<Express1ValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("A street address is required", errors[0].Message);
        }

        [TestMethod]
        public void Validate_AddsError_WhenCityIsMissing_Test()
        {
            registration.MailingAddress.City = string.Empty;

            List<Express1ValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("City is required", errors[0].Message);
        }

        [TestMethod]
        public void Validate_AddsError_WhenStateIsMissing_Test()
        {
            registration.MailingAddress.StateProvCode = string.Empty;

            List<Express1ValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("A state/province is required", errors[0].Message);
        }

        [TestMethod]
        public void Validate_AddsError_WhenPostalCodeIsMissing_Test()
        {
            registration.MailingAddress.PostalCode = string.Empty;

            List<Express1ValidationError> errors = testObject.Validate(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("A postal code is required", errors[0].Message);
        }

        [TestMethod]
        public void Validate_DelegatesToPaymentValidator_Test()
        {
            testObject.Validate(registration);

            paymentValidator.Verify(v => v.ValidatePaymentInfo(registration.Payment));
        }

        
        [TestMethod]
        public void ValidatePersonalInfo_AddsError_WhenNameIsMissing_Test()
        {
            registration.MailingAddress.FirstName = string.Empty;
            registration.MailingAddress.MiddleName = string.Empty;
            registration.MailingAddress.LastName = string.Empty;

            List<Express1ValidationError> errors = testObject.ValidatePersonalInfo(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Name is required", errors[0].Message);
        }

        [TestMethod]
        public void ValidatePersonalInfo_AddsError_WhenCompanyIsMissing_Test()
        {
            registration.MailingAddress.Company = string.Empty;

            List<Express1ValidationError> errors = testObject.ValidatePersonalInfo(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Company is required", errors[0].Message);
        }

        [TestMethod]
        public void ValidatePersonalInfo_AddsError_WhenPhoneIsMissing_Test()
        {
            registration.MailingAddress.Phone = string.Empty;

            List<Express1ValidationError> errors = testObject.ValidatePersonalInfo(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Phone number is required", errors[0].Message);
        }

        [TestMethod]
        public void ValidatePersonalInfo_AddsError_WhenEmailIsMissing_Test()
        {
            registration.MailingAddress.Email = string.Empty;

            List<Express1ValidationError> errors = testObject.ValidatePersonalInfo(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("An email address is required", errors[0].Message);
        }

        [TestMethod]
        public void ValidatePersonalInfo_AddsError_WhenStreetIsMissing_Test()
        {
            registration.MailingAddress.Street1 = string.Empty;

            List<Express1ValidationError> errors = testObject.ValidatePersonalInfo(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("A street address is required", errors[0].Message);
        }

        [TestMethod]
        public void ValidatePersonalInfo_AddsError_WhenCityIsMissing_Test()
        {
            registration.MailingAddress.City = string.Empty;

            List<Express1ValidationError> errors = testObject.ValidatePersonalInfo(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("City is required", errors[0].Message);
        }

        [TestMethod]
        public void ValidatePersonalInfo_AddsError_WhenStateIsMissing_Test()
        {
            registration.MailingAddress.StateProvCode = string.Empty;

            List<Express1ValidationError> errors = testObject.ValidatePersonalInfo(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("A state/province is required", errors[0].Message);
        }

        [TestMethod]
        public void ValidatePersonalInfo_AddsError_WhenPostalCodeIsMissing_Test()
        {
            registration.MailingAddress.PostalCode = string.Empty;

            List<Express1ValidationError> errors = testObject.ValidatePersonalInfo(registration);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("A postal code is required", errors[0].Message);
        }



        [TestMethod]
        public void ValidatePaymentInfo_DelegatesToPaymentValidator_Test()
        {
            testObject.Validate(registration);

            paymentValidator.Verify(v => v.ValidatePaymentInfo(registration.Payment));
        }
    }
}
