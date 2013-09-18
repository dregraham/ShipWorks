﻿using System;
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
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.ScanForms;
using log4net;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Express1
{
    [TestClass]
    public class Express1AchPaymentValidatorTest
    {
        private Express1AchPaymentValidator testObject;
        private Express1PaymentInfo paymentInfo;

        [TestInitialize]
        public void Initialize()
        {
            paymentInfo = new Express1PaymentInfo(Express1PaymentType.Ach)
                {
                    AchAccountHolderName = "Test User",
                    AchAccountType = Express1AchType.Checking,
                    AchAccountNumber = "1234680",
                    AchBankName = "USBank",
                    AchRoutingId = "000000000",
                    PaymentType = Express1PaymentType.Ach,
                    AchAccountName = "John Doe"
                };
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsNoErrors_WhenValuesValid_Test()
        {
            testObject = new Express1AchPaymentValidator();

            IEnumerable<Express1ValidationError> errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.AreEqual(errors.Count(), 0);
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenPaymentTypeIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // PaymentType is CreditCard
            paymentInfo.PaymentType = Express1PaymentType.CreditCard;
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidPaymentTypeCreditCard));
        }
        
        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenAchAccountHolderNameIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // AchAccountHolderName is null
            paymentInfo.AchAccountHolderName = null;
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchAccountHolderName));

            // AchAccountHolderName is blank
            paymentInfo.AchAccountHolderName = "";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchAccountHolderName));

            // AchAccountHolderName is just spaces
            paymentInfo.AchAccountHolderName = "   ";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchAccountHolderName));
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenAchAccountNumberIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // AchAccountNumber is null
            paymentInfo.AchAccountNumber = null;
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchAccountNumber));

            // AchAccountNumber is blank
            paymentInfo.AchAccountNumber = "";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchAccountNumber));

            // AchAccountNumber is just spaces
            paymentInfo.AchAccountNumber = "   ";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchAccountNumber));
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenAchBankNameIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // AchBankName is null
            paymentInfo.AchBankName = null;
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchBankName));

            // AchBankName is blank
            paymentInfo.AchBankName = "";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchBankName));

            // AchBankName is just spaces
            paymentInfo.AchBankName = "   ";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchBankName));
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenAchRoutingIdIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // AchRoutingId is null
            paymentInfo.AchRoutingId = null;
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchRoutingId));

            // AchRoutingId is blank
            paymentInfo.AchRoutingId = "";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchRoutingId));

            // AchRoutingId is just spaces
            paymentInfo.AchRoutingId = "   ";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchRoutingId));
        }

        [TestMethod]
        public void ValidatePaymentInfo_ReturnsError_WhenAchAccountNameIsInvalid_Test()
        {
            IEnumerable<Express1ValidationError> errors;

            // AchAccountName is null
            paymentInfo.AchAccountName = null;
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchAccountName));

            // AchAccountName is blank
            paymentInfo.AchAccountName = "";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchAccountName));

            // AchAccountName is just spaces
            paymentInfo.AchAccountName = "   ";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.IsTrue(errors.Any(e => e.Message == Express1ValidationErrorMessages.InvalidAchAccountName));
        }
    }
}
