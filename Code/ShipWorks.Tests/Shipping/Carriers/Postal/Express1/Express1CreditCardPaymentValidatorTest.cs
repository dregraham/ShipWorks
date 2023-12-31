﻿using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1.Enums;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration.Payment;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Express1
{
    public class Express1CreditCardPaymentValidatorTest
    {
        private Express1CreditCardPaymentValidator testObject;
        private Express1PaymentInfo paymentInfo;
        private PersonAdapter billingAddress;

        public Express1CreditCardPaymentValidatorTest()
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
                CreditCardAccountNumber = "41111111111111111",
                CreditCardVerificationNumber = 411,
                CreditCardExpirationDate = DateTime.Now.AddMonths(5),
                CreditCardType = Express1CreditCardType.Visa,
                CreditCardBillingAddress = billingAddress,
                CreditCardNameOnCard = "John Doe"
            };
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsNoErrors_WhenValuesValid()
        {
            testObject = new Express1CreditCardPaymentValidator();

            IEnumerable<Express1ValidationError> errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.Empty(errors);
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressMissing()
        {
            paymentInfo.CreditCardBillingAddress = null;
            testObject = new Express1CreditCardPaymentValidator();

            IEnumerable<Express1ValidationError> errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.MissingCreditCardBillingAddress));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressFirstNameIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // FirstName is null
            paymentInfo.CreditCardBillingAddress.FirstName = null;
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressFirstName));

            // FirstName is blank
            paymentInfo.CreditCardBillingAddress.FirstName = "";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressFirstName));

            // FirstName is just spaces
            paymentInfo.CreditCardBillingAddress.FirstName = "   ";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressFirstName));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressLastNameIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // LastName is null
            paymentInfo.CreditCardBillingAddress.LastName = null;
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressLastName));

            // LastName is blank
            paymentInfo.CreditCardBillingAddress.LastName = "";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressLastName));

            // LastName is just spaces
            paymentInfo.CreditCardBillingAddress.LastName = "   ";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressLastName));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressCityIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // City is null
            paymentInfo.CreditCardBillingAddress.City = null;
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressCity));

            // City is blank
            paymentInfo.CreditCardBillingAddress.City = "";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressCity));

            // City is just spaces
            paymentInfo.CreditCardBillingAddress.City = "   ";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressCity));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressCountryCodeIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // CountryCode is null
            paymentInfo.CreditCardBillingAddress.CountryCode = null;
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressCountryCode));

            // CountryCode is blank
            paymentInfo.CreditCardBillingAddress.CountryCode = "";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressCountryCode));

            // CountryCode is just spaces
            paymentInfo.CreditCardBillingAddress.CountryCode = "   ";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressCountryCode));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressPostalCodeIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // PostalCode is null
            paymentInfo.CreditCardBillingAddress.PostalCode = null;
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressPostalCode));

            // PostalCode is blank
            paymentInfo.CreditCardBillingAddress.PostalCode = "";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressPostalCode));

            // PostalCode is just spaces
            paymentInfo.CreditCardBillingAddress.PostalCode = "   ";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressPostalCode));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressStateProvCodeIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // StateProvCode is null
            paymentInfo.CreditCardBillingAddress.StateProvCode = null;
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressStateProvince));

            // StateProvCode is blank
            paymentInfo.CreditCardBillingAddress.StateProvCode = "";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressStateProvince));

            // StateProvCode is just spaces
            paymentInfo.CreditCardBillingAddress.StateProvCode = "   ";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressStateProvince));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenBillingAddressStreet1IsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // Street1 is null
            paymentInfo.CreditCardBillingAddress.Street1 = null;
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressStreet));

            // Street1 is blank
            paymentInfo.CreditCardBillingAddress.Street1 = "";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressStreet));

            // Street1 is just spaces
            paymentInfo.CreditCardBillingAddress.Street1 = "   ";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardBillingAddressStreet));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenCardAccountNumberIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // CardAccountNumber is null
            paymentInfo.CreditCardAccountNumber = null;
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardAccountNumber));

            // CardAccountNumber is blank
            paymentInfo.CreditCardAccountNumber = "";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardAccountNumber));

            // CardAccountNumber is just spaces
            paymentInfo.CreditCardAccountNumber = "   ";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardAccountNumber));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenNameOnCardIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // CcNameOnCard is null
            paymentInfo.CreditCardNameOnCard = null;
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardNameOnCard));

            // CcNameOnCard is blank
            paymentInfo.CreditCardNameOnCard = "";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardNameOnCard));

            // CcNameOnCard is just spaces
            paymentInfo.CreditCardNameOnCard = "   ";
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardNameOnCard));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenCardExpirationDateIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // CardExpirationDate is before now
            paymentInfo.CreditCardExpirationDate = DateTime.MinValue;
            testObject = new Express1CreditCardPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidCreditCardExpirationDate));
        }

        [Fact]
        public void ValidatePaymentInfo_DoesNotReturnError_WhenCardExpirationDateIsThisMonth()
        {
            // CardExpirationDate is today
            paymentInfo.CreditCardExpirationDate = DateTime.Today;
            testObject = new Express1CreditCardPaymentValidator();

            IEnumerable<Express1ValidationError> errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.False(errors.Any());
        }

        [Fact]
        public void ValidatePaymentInfo_DoesNotReturnError_WhenCardExpirationDateIsFirstDayOfThisMonth()
        {
            // CardExpirationDate is the first day of the current month
            paymentInfo.CreditCardExpirationDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            testObject = new Express1CreditCardPaymentValidator();

            IEnumerable<Express1ValidationError> errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.False(errors.Any());
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenPaymentTypeIsInvalid()
        {
            // PaymentType is ACH
            paymentInfo.PaymentType = Express1PaymentType.Ach;
            testObject = new Express1CreditCardPaymentValidator();

            IEnumerable<Express1ValidationError> errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1CreditCardPaymentValidator.InvalidPaymentTypeAch));
        }

    }
}
