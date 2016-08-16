using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using Xunit;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Express1.Enums;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration;
using ShipWorks.Shipping.Carriers.Postal.Express1.Registration.Payment;
using ShipWorks.Shipping.ScanForms;
using log4net;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Express1
{
    public class Express1AchPaymentValidatorTest
    {
        private Express1AchPaymentValidator testObject;
        private Express1PaymentInfo paymentInfo;

        public Express1AchPaymentValidatorTest()
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

        [Fact]
        public void ValidatePaymentInfo_ReturnsNoErrors_WhenValuesValid()
        {
            testObject = new Express1AchPaymentValidator();

            IEnumerable<Express1ValidationError> errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.Equal(errors.Count(), 0);
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenPaymentTypeIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // PaymentType is CreditCard
            paymentInfo.PaymentType = Express1PaymentType.CreditCard;
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidPaymentTypeCreditCard));
        }
        
        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenAchAccountHolderNameIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // AchAccountHolderName is null
            paymentInfo.AchAccountHolderName = null;
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchAccountHolderName));

            // AchAccountHolderName is blank
            paymentInfo.AchAccountHolderName = "";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchAccountHolderName));

            // AchAccountHolderName is just spaces
            paymentInfo.AchAccountHolderName = "   ";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchAccountHolderName));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenAchAccountNumberIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // AchAccountNumber is null
            paymentInfo.AchAccountNumber = null;
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchAccountNumber));

            // AchAccountNumber is blank
            paymentInfo.AchAccountNumber = "";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchAccountNumber));

            // AchAccountNumber is just spaces
            paymentInfo.AchAccountNumber = "   ";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchAccountNumber));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenAchBankNameIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // AchBankName is null
            paymentInfo.AchBankName = null;
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchBankName));

            // AchBankName is blank
            paymentInfo.AchBankName = "";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchBankName));

            // AchBankName is just spaces
            paymentInfo.AchBankName = "   ";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchBankName));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenAchRoutingIdIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // AchRoutingId is null
            paymentInfo.AchRoutingId = null;
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchRoutingId));

            // AchRoutingId is blank
            paymentInfo.AchRoutingId = "";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchRoutingId));

            // AchRoutingId is just spaces
            paymentInfo.AchRoutingId = "   ";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchRoutingId));
        }

        [Fact]
        public void ValidatePaymentInfo_ReturnsError_WhenAchAccountNameIsInvalid()
        {
            IEnumerable<Express1ValidationError> errors;

            // AchAccountName is null
            paymentInfo.AchAccountName = null;
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchAccountName));

            // AchAccountName is blank
            paymentInfo.AchAccountName = "";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchAccountName));

            // AchAccountName is just spaces
            paymentInfo.AchAccountName = "   ";
            testObject = new Express1AchPaymentValidator();

            errors = testObject.ValidatePaymentInfo(paymentInfo);
            Assert.True(errors.Any(e => e.Message == Express1AchPaymentValidator.InvalidAchAccountName));
        }
    }
}
