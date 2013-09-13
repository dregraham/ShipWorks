using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1.Enums;
using ShipWorks.Shipping.ScanForms;
using log4net;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Express1
{
    [TestClass]
    public class Express1CreditCardPaymentValidatorTest
    {
        private Express1CreditCardPaymentValidator testObject;
        private Express1PaymentInfo paymentInfo;
        private PersonAdapter billingAddress;

        [TestInitialize]
        public void Initialize()
        {
            billingAddress = new PersonAdapter
                {
                    FirstName = "xxxxxxxxxxxx",
                    LastName = "xxxxxxxxxxxx",
                    Company = "xxxxxxxxxxxx",
                    Street1 = "xxxxxxxxxxxx",
                    City = "St. Louis",
                    StateProvCode = "xxxxxxxxxxxx",
                    PostalCode = "xxxxxxxxxxxx",
                    CountryCode = "xxxxxxxxxxxx"
                };

            paymentInfo = new Express1PaymentInfo(Express1PaymentType.CreditCard)
                {
                    CardAccountNumber = "41111111111111111",
                    CardCvn = 411,
                    CardExpirationDate = DateTime.Now.AddMonths(5),
                    CardType = Express1CreditCardType.Visa,
                    CardBillingAddress = billingAddress
                };
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsNoErrors_WhenValuesValid_Test()
        {
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            IEnumerable<Express1ValidationError> errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 0);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressMissing_Test()
        {
            paymentInfo.CardBillingAddress = null;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);
            
            IEnumerable<Express1ValidationError> errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressFirstNameIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors; 

            // FirstName is null
            paymentInfo.CardBillingAddress.FirstName = null;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // FirstName is blank
            paymentInfo.CardBillingAddress.FirstName = "";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // FirstName is just spaces
            paymentInfo.CardBillingAddress.FirstName = "   ";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressLastNameIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // LastName is null
            paymentInfo.CardBillingAddress.LastName = null;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // LastName is blank
            paymentInfo.CardBillingAddress.LastName = "";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // LastName is just spaces
            paymentInfo.CardBillingAddress.LastName = "   ";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressCityIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // City is null
            paymentInfo.CardBillingAddress.City = null;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // City is blank
            paymentInfo.CardBillingAddress.City = "";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // City is just spaces
            paymentInfo.CardBillingAddress.City = "   ";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressCountryCodeIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // CountryCode is null
            paymentInfo.CardBillingAddress.CountryCode = null;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // CountryCode is blank
            paymentInfo.CardBillingAddress.CountryCode = "";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // CountryCode is just spaces
            paymentInfo.CardBillingAddress.CountryCode = "   ";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressPostalCodeIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // PostalCode is null
            paymentInfo.CardBillingAddress.PostalCode = null;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // PostalCode is blank
            paymentInfo.CardBillingAddress.PostalCode = "";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // PostalCode is just spaces
            paymentInfo.CardBillingAddress.PostalCode = "   ";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressStateProvCodeIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // StateProvCode is null
            paymentInfo.CardBillingAddress.StateProvCode = null;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // StateProvCode is blank
            paymentInfo.CardBillingAddress.StateProvCode = "";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // StateProvCode is just spaces
            paymentInfo.CardBillingAddress.StateProvCode = "   ";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressStreet1IsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // Street1 is null
            paymentInfo.CardBillingAddress.Street1 = null;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // Street1 is blank
            paymentInfo.CardBillingAddress.Street1 = "";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // Street1 is just spaces
            paymentInfo.CardBillingAddress.Street1 = "   ";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenCardAccountNumberIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // CardAccountNumber is null
            paymentInfo.CardAccountNumber = null;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // CardAccountNumber is blank
            paymentInfo.CardAccountNumber = "";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // CardAccountNumber is just spaces
            paymentInfo.CardAccountNumber = "   ";
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenCardCvnIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // CardCvn is 0
            paymentInfo.CardCvn = 0;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);

            // CardCvn is less than 0
            paymentInfo.CardCvn = -1;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenCardExpirationDateIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // CardExpirationDate is before now
            paymentInfo.CardExpirationDate = DateTime.MinValue;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenPaymentTypeIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // PaymentType is ACH
            paymentInfo.PaymentType = Express1PaymentType.Ach;
            testObject = new Express1CreditCardPaymentValidator(paymentInfo);

            errors = testObject.ValidatePaymentInfo();
            Assert.AreEqual(errors.Count(), 1);
        }

    }
}
